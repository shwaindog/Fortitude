// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication;

public interface IPQServer<T> : IDisposable where T : IPQMessage
{
    bool IsStarted { get; }
    void StartServices();
    T?   Register(string ticker);
    void Unregister(T quote);
    void Publish(T quote);
    void SetNextSequenceNumberToZero(string ticker);
    void SetNextSequenceNumberToFullUpdate(string ticker);
}

public class PQServer<T> : IPQServer<T> where T : class, IPQMessage, new()
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQServer<T>));

    private readonly ISocketDispatcherResolver     dispatcherResolver;
    private readonly IMap<uint, IPQMessage>        lastPubEntities          = new ConcurrentMap<uint, IPQMessage>();
    private readonly Recycler                      dispatchEntitiesRecycler = new();
    private readonly IDoublyLinkedList<IPQMessage> heartbeatQuotes          = new DoublyLinkedList<IPQMessage>();
    private readonly ISyncLock                     heartBeatSync            = new YieldLockLight();

    private readonly IMarketConnectionConfig    marketConnectionConfig;
    private readonly IPricingServerConfig       pricingServerConfig;
    private readonly Func<ISourceTickerInfo, T> quoteFactory;
    private readonly IPQServerHeartBeatSender   serverHeartBeatSender;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer>
        snapShotServerFactory;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer>
        updateServerFactory;

    private readonly Func<ISourceTickerInfo, T> freshInstanceFactory = ReflectionHelper.CtorBinder<ISourceTickerInfo, T>();

    private IPQSnapshotServer? snapshotServer;
    private IPQUpdateServer?   updateServer;

    public PQServer(IMarketConnectionConfig marketConnectionConfig,
        IPQServerHeartBeatSender serverHeartBeatSender,
        ISocketDispatcherResolver socketDispatcherResolver,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> snapShotServerFactory,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> updateServerFactory,
        Func<ISourceTickerInfo, T>? quoteFactory = null)
    {
        this.quoteFactory           = quoteFactory ?? DefaultRecyclerFactory;
        dispatcherResolver          = socketDispatcherResolver;
        this.marketConnectionConfig = marketConnectionConfig;
        pricingServerConfig         = marketConnectionConfig.PricingServerConfig!;
        this.serverHeartBeatSender  = serverHeartBeatSender;
        this.snapShotServerFactory  = snapShotServerFactory;
        this.updateServerFactory    = updateServerFactory;

        serverHeartBeatSender.ServerLinkedLock   = heartBeatSync;
        serverHeartBeatSender.ServerLinkedQuotes = heartbeatQuotes;
    }

    private T DefaultRecyclerFactory(ISourceTickerInfo sourceTickerInfo)
    {
        var borrowed = dispatchEntitiesRecycler.Borrow<T>();
        if (borrowed.SourceTickerInfo is not PQSourceTickerInfo)
        {
            borrowed.SourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        }
        else
        {
            borrowed.SourceTickerInfo.StateReset();
            borrowed.SourceTickerInfo.CopyFrom(sourceTickerInfo, CopyMergeFlags.FullReplace);
        }
        return borrowed;
    }

    public T? Register(string ticker)
    {
        var tickerInfo = marketConnectionConfig.GetSourceTickerInfo(ticker);
        if (tickerInfo != null)
        {
            if (!lastPubEntities.TryGetValue(tickerInfo.SourceInstrumentId, out var updateEnt))
            {
                updateEnt = freshInstanceFactory(tickerInfo);

                updateEnt.PQSequenceId = uint.MaxValue;
                lastPubEntities.TryAdd(tickerInfo.SourceInstrumentId, updateEnt);
                // publish identical quote leaving next quote to also be zero.
                var quote = quoteFactory(tickerInfo);
                quote.PQSequenceId = uint.MaxValue;
                quote.HasUpdates   = true;

                Publish(quote);

                if (!serverHeartBeatSender.HasStarted) serverHeartBeatSender.StartSendingHeartBeats();

                return quote;
            }
        }

        return null;
    }

    public void SetNextSequenceNumberToZero(string ticker)
    {
        var tickerInfo = marketConnectionConfig.GetSourceTickerInfo(ticker);
        if (tickerInfo != null)
            if (lastPubEntities.TryGetValue(tickerInfo.SourceInstrumentId, out var ent))
                ent!.PQSequenceId = uint.MaxValue;
    }

    public void Unregister(T quote)
    {
        if (lastPubEntities.TryGetValue(quote.StreamId, out var ent))
        {
            quote.ResetWithTracking();
            quote.HasUpdates = true;
            Publish(quote);
            lastPubEntities.Remove(quote.StreamId);
            heartBeatSync.Acquire();
            try
            {
                heartbeatQuotes.Remove(ent!);
            }
            finally
            {
                heartBeatSync.Release();
            }

            if (lastPubEntities.Count == 0)
                if (serverHeartBeatSender.HasStarted)
                    serverHeartBeatSender.StopAndWaitUntilFinished();
        }
    }

    public void Publish(T quote)
    {
        if (!quote.HasUpdates) return;
        if (lastPubEntities.TryGetValue(quote.StreamId, out var ent))
        {
            var pubUpdate = DefaultRecyclerFactory(ent!.SourceTickerInfo!);
            pubUpdate.Lock.Acquire();
            ent.Lock.Acquire();
            try
            {
                var seqId = ent.PQSequenceId;
                ent.CopyFrom(quote);
                ent.PQSequenceId = seqId;
                pubUpdate.CopyFrom(ent, CopyMergeFlags.FullReplace);
                quote.UpdateComplete(seqId);
                ent.UpdateComplete(seqId);
                ent.PQSequenceId++;
            }
            finally
            {
                ent.Lock.Release();
                pubUpdate.Lock.Release();
            }
            if (pubUpdate.PQSequenceId == uint.MaxValue) pubUpdate.HasUpdates = true;

            if (pubUpdate.HasUpdates)
            {
                if (updateServer is { IsStarted: true }) updateServer.Send(pubUpdate);
                // Logger.Info("Published {0}", ent);
            }
            else
            {
                Logger.Warn("Publish request has no changes so will not be published.");
            }
            pubUpdate.DecrementRefCount();

            heartBeatSync.Acquire();
            try
            {
                heartbeatQuotes.Remove(ent);
                heartbeatQuotes.AddLast(ent);
                ent.LastPublicationTime = TimeContext.UtcNow;
            }
            finally
            {
                heartBeatSync.Release();
            }
        }
        else
        {
            throw new ArgumentException("Trying to publish unregistered quote");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        lastPubEntities.Clear();
        heartBeatSync.Acquire();
        try
        {
            heartbeatQuotes.Clear();
        }
        finally
        {
            heartBeatSync.Release();
        }

        if (serverHeartBeatSender.HasStarted) serverHeartBeatSender.StopAndWaitUntilFinished();
        if (updateServer != null)
        {
            updateServer.Stop(CloseReason.Completed, "PQServer is closing");
            updateServer = null;
        }

        if (snapshotServer != null)
        {
            snapshotServer.Stop(CloseReason.Completed, "PQServer is closing");
            snapshotServer = null;
        }

        IsStarted = false;
    }

    public void SetNextSequenceNumberToFullUpdate(string ticker)
    {
        var tickerInfo = marketConnectionConfig.GetSourceTickerInfo(ticker);
        if (tickerInfo != null)
            if (lastPubEntities.TryGetValue(tickerInfo.SourceInstrumentId, out var ent))
                ent!.HasUpdates = true;
    }

    private void OnSnapshotContextRequest(IConversationRequester cx, PQSnapshotIdsRequest snapshotIdsRequest)
    {
        foreach (var streamId in snapshotIdsRequest.RequestSourceTickerIds)
            if (lastPubEntities.TryGetValue(streamId, out var lastPubEnt))
            {
                var snapshotEnt = DefaultRecyclerFactory(lastPubEnt!.SourceTickerInfo!);
                snapshotEnt.Lock.Acquire();
                lastPubEnt.Lock.Acquire();
                try
                {
                    snapshotEnt.QuoteBehavior = lastPubEnt.QuoteBehavior;
                    snapshotEnt.CopyFrom(lastPubEnt, CopyMergeFlags.FullReplace);
                    snapshotEnt.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.FromAdapterSnapshot;
                    snapshotEnt.FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.IsAdapterReplay;
                    cx.Send(snapshotEnt);
                }
                finally
                {
                    lastPubEnt.Lock.Release();
                    snapshotEnt.Lock.Release();
                }
                snapshotEnt.DecrementRefCount();
            }
    }

    #region FeedReferential management

    public bool IsStarted { get; private set; }

    public void StartServices()
    {
        if (!IsStarted)
        {
            snapshotServer = snapShotServerFactory(pricingServerConfig.SnapshotConnectionConfig
                                                 , dispatcherResolver);
            snapshotServer.OnSnapshotRequest               += OnSnapshotContextRequest;
            snapshotServer.ReceivedSourceTickerInfoRequest += OnReceivedSourceTickerInfoRequest;
            snapshotServer.Start();
            updateServer = updateServerFactory(pricingServerConfig.UpdateConnectionConfig
                                             , dispatcherResolver);
            updateServer.Start();
            serverHeartBeatSender.UpdateServer = updateServer;
            IsStarted                          = true;
        }
    }

    private void OnReceivedSourceTickerInfoRequest
    (PQSourceTickerInfoRequest sourceTickerInfoRequest
      , IConversationRequester clientConversationRequester)
    {
        var response = new PQSourceTickerInfoResponse(marketConnectionConfig.AllSourceTickerInfos);
        response.RequestId = sourceTickerInfoRequest.RequestId;
        clientConversationRequester.Send(response);
    }

    #endregion
}
