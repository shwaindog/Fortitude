// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQServer<T> : IDisposable where T : IPQLevel0Quote
{
    bool IsStarted { get; }
    void StartServices();
    T?   Register(string ticker);
    void Unregister(T quote);
    void Publish(T quote);
    void SetNextSequenceNumberToZero(string ticker);
    void SetNextSequenceNumberToFullUpdate(string ticker);
}

public class PQServer<T> : IPQServer<T> where T : class, IPQLevel0Quote
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQServer<T>));

    private readonly ISocketDispatcherResolver         dispatcherResolver;
    private readonly IMap<uint, T>                     entities        = new ConcurrentMap<uint, T>();
    private readonly IDoublyLinkedList<IPQLevel0Quote> heartbeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
    private readonly ISyncLock                         heartBeatSync   = new YieldLockLight();

    private readonly IMarketConnectionConfig         marketConnectionConfig;
    private readonly IPricingServerConfig            pricingServerConfig;
    private readonly Func<ISourceTickerQuoteInfo, T> quoteFactory;
    private readonly IPQServerHeartBeatSender        serverHeartBeatSender;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer>
        snapShotServerFactory;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer>
        updateServerFactory;

    private IPQSnapshotServer? snapshotServer;
    private IPQUpdateServer?   updateServer;

    public PQServer
    (IMarketConnectionConfig marketConnectionConfig,
        IPQServerHeartBeatSender serverHeartBeatSender,
        ISocketDispatcherResolver socketDispatcherResolver,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> snapShotServerFactory,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> updateServerFactory,
        Func<ISourceTickerQuoteInfo, T>? quoteFactory = null)
    {
        this.quoteFactory           = quoteFactory ?? ReflectionHelper.CtorBinder<ISourceTickerQuoteInfo, T>();
        dispatcherResolver          = socketDispatcherResolver;
        this.marketConnectionConfig = marketConnectionConfig;
        pricingServerConfig         = marketConnectionConfig.PricingServerConfig!;
        this.serverHeartBeatSender  = serverHeartBeatSender;
        this.snapShotServerFactory  = snapShotServerFactory;
        this.updateServerFactory    = updateServerFactory;

        serverHeartBeatSender.ServerLinkedLock   = heartBeatSync;
        serverHeartBeatSender.ServerLinkedQuotes = heartbeatQuotes;
    }

    public T? Register(string ticker)
    {
        var tickerInfo = marketConnectionConfig.GetSourceTickerInfo(ticker);
        if (tickerInfo != null)
            if (!entities.TryGetValue(tickerInfo.SourceTickerId, out var ent))
            {
                ent              = quoteFactory(tickerInfo);
                ent.PQSequenceId = uint.MaxValue;
                entities.Add(tickerInfo.SourceTickerId, ent);
                // publish identical quote leaving next quote to also be zero.
                var quote = quoteFactory(tickerInfo);
                quote.PQSequenceId = uint.MaxValue;
                quote.HasUpdates   = true;
                Publish(quote);

                if (!serverHeartBeatSender.HasStarted) serverHeartBeatSender.StartSendingHeartBeats();

                return quote;
            }

        return null;
    }

    public void SetNextSequenceNumberToZero(string ticker)
    {
        var tickerInfo = marketConnectionConfig.GetSourceTickerInfo(ticker);
        if (tickerInfo != null)
            if (entities.TryGetValue(tickerInfo.SourceTickerId, out var ent))
                ent!.PQSequenceId = uint.MaxValue;
    }

    public void Unregister(T quote)
    {
        if (entities.TryGetValue(quote.SourceTickerQuoteInfo!.SourceTickerId, out var ent))
        {
            quote.ResetFields();
            quote.HasUpdates = true;
            Publish(quote);
            entities.Remove(quote.SourceTickerQuoteInfo.SourceTickerId);
            heartBeatSync.Acquire();
            try
            {
                heartbeatQuotes.Remove(ent!);
            }
            finally
            {
                heartBeatSync.Release();
            }

            if (entities.Count == 0)
                if (serverHeartBeatSender.HasStarted)
                    serverHeartBeatSender.StopAndWaitUntilFinished();
        }
    }

    public void Publish(T quote)
    {
        if (!quote.HasUpdates) return;
        if (entities.TryGetValue(quote.SourceTickerQuoteInfo!.SourceTickerId, out var ent))
        {
            ent!.Lock.Acquire();
            try
            {
                var seqId = ent.PQSequenceId;
                ent.CopyFrom((ILevel0Quote)quote);
                quote.HasUpdates = false;
                ent.PQSequenceId = seqId;
            }
            finally
            {
                ent.Lock.Release();
            }
            if (ent.PQSequenceId == uint.MaxValue) ent.HasUpdates = true;

            if (ent.HasUpdates)
            {
                if (updateServer != null && updateServer.IsStarted) updateServer.Send(ent);
                // Logger.Info("Published {0}", ent);
            }
            else
            {
                Logger.Warn("Publish request has no changes so will not be published.");
            }

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
        entities.Clear();
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
            if (entities.TryGetValue(tickerInfo.SourceTickerId, out var ent))
                ent!.HasUpdates = true;
    }

    private void OnSnapshotContextRequest(IConversationRequester cx, PQSnapshotIdsRequest snapshotIdsRequest)
    {
        foreach (var streamId in snapshotIdsRequest.RequestSourceTickerIds)
            if (entities.TryGetValue(streamId, out var ent))
                cx.Send(ent!);
    }

    #region FeedReferential management

    public bool IsStarted { get; private set; }

    public void StartServices()
    {
        if (!IsStarted)
        {
            snapshotServer = snapShotServerFactory(pricingServerConfig.SnapshotConnectionConfig!
                                                 , dispatcherResolver);
            snapshotServer.OnSnapshotRequest               += OnSnapshotContextRequest;
            snapshotServer.ReceivedSourceTickerInfoRequest += OnReceivedSourceTickerInfoRequest;
            snapshotServer.Start();
            updateServer = updateServerFactory(pricingServerConfig.UpdateConnectionConfig!
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
