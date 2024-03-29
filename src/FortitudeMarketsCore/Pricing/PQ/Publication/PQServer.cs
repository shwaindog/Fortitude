﻿#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public class PQServer<T> : IPQServer<T> where T : class, IPQLevel0Quote
{
    private readonly ISocketDispatcherResolver dispatcherResolver;
    private readonly IMap<uint, T> entities = new ConcurrentMap<uint, T>();
    private readonly IDoublyLinkedList<IPQLevel0Quote> heartbeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
    private readonly ISyncLock heartBeatSync = new YieldLockLight();
    private readonly Func<ISourceTickerQuoteInfo, T> quoteFactory;
    private readonly IPQServerHeartBeatSender serverHeartBeatSender;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer>
        snapShotServerFactory;

    private readonly ISnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig;

    private readonly Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer>
        updateServerFactory;

    private IPQSnapshotServer? snapshotServer;
    private IPQUpdateServer? updateServer;

    public PQServer(ISnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig,
        IPQServerHeartBeatSender serverHeartBeatSender,
        ISocketDispatcherResolver socketDispatcherResolver,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> snapShotServerFactory,
        Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> updateServerFactory,
        Func<ISourceTickerQuoteInfo, T>? quoteFactory = null)
    {
        this.quoteFactory = quoteFactory ?? ReflectionHelper.CtorBinder<ISourceTickerQuoteInfo, T>();
        dispatcherResolver = socketDispatcherResolver;
        this.snapshotUpdatePricingServerConfig = snapshotUpdatePricingServerConfig;
        this.serverHeartBeatSender = serverHeartBeatSender;
        this.snapShotServerFactory = snapShotServerFactory;
        this.updateServerFactory = updateServerFactory;
        serverHeartBeatSender.ServerLinkedLock = heartBeatSync;
        serverHeartBeatSender.ServerLinkedQuotes = heartbeatQuotes;
    }

    public T? Register(string ticker)
    {
        var tickerInfo =
            snapshotUpdatePricingServerConfig.SourceTickerPublicationConfigs!.FirstOrDefault(
                tii => ticker.Equals(tii.Ticker, StringComparison.InvariantCultureIgnoreCase));
        if (tickerInfo != null)
            if (!entities.TryGetValue(tickerInfo.Id, out var ent))
            {
                ent = quoteFactory(tickerInfo);
                ent.PQSequenceId = uint.MaxValue;
                entities.Add(tickerInfo.Id, ent);
                var quote = quoteFactory(tickerInfo);
                quote.PQSequenceId = uint.MaxValue;
                quote.HasUpdates = true;
                Publish(quote);

                if (!serverHeartBeatSender.HasStarted) serverHeartBeatSender.StartSendingHeartBeats();

                return quote;
            }

        return null;
    }

    public void Unregister(T quote)
    {
        if (entities.TryGetValue(quote.SourceTickerQuoteInfo!.Id, out var ent))
        {
            quote.ResetFields();
            Publish(quote);
            entities.Remove(quote.SourceTickerQuoteInfo.Id);
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
        if (entities.TryGetValue(quote.SourceTickerQuoteInfo!.Id, out var ent))
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

            if (updateServer != null && updateServer.IsStarted) updateServer.Send(ent);
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

        if (serverHeartBeatSender.HasStarted)
            serverHeartBeatSender.StopAndWaitUntilFinished();
        if (updateServer != null)
        {
            updateServer.Stop();
            updateServer = null;
        }

        if (snapshotServer != null)
        {
            snapshotServer.Stop();
            snapshotServer = null;
        }

        IsStarted = false;
    }

    private void OnSnapshotContextRequest(IConversationRequester cx, uint[] streamIDs)
    {
        foreach (var streamId in streamIDs)
            if (entities.TryGetValue(streamId, out var ent))
                cx.Send(ent!);
    }

    #region FeedReferential management

    public bool IsStarted { get; private set; }

    public void StartServices()
    {
        if (!IsStarted)
        {
            snapshotServer = snapShotServerFactory(snapshotUpdatePricingServerConfig.SnapshotConnectionConfig!
                , dispatcherResolver);
            snapshotServer.OnSnapshotRequest += OnSnapshotContextRequest;
            snapshotServer.Start();
            updateServer = updateServerFactory(snapshotUpdatePricingServerConfig.UpdateConnectionConfig!
                , dispatcherResolver);
            updateServer.Start();
            serverHeartBeatSender.UpdateServer = updateServer;
            IsStarted = true;
        }
    }

    #endregion
}
