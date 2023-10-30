#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public class PQServer<T> : IPQServer<T> where T : class, IPQLevel0Quote
{
    private readonly ISocketDispatcher dispatcher;
    private readonly IMap<uint, T> entities = new ConcurrentMap<uint, T>();
    private readonly IDoublyLinkedList<IPQLevel0Quote> heartbeatQuotes = new DoublyLinkedList<IPQLevel0Quote>();
    private readonly ISyncLock heartBeatSync = new YieldLockLight();
    private readonly Func<ISourceTickerQuoteInfo, T> quoteFactory;
    private readonly IPQServerHeartBeatSender serverHeartBeatSender;

    private readonly Func<ISocketDispatcher, IConnectionConfig, string, IPQSnapshotServer>
        snapShotServerFactory;

    private readonly ISnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig;

    private readonly Func<ISocketDispatcher, IConnectionConfig, string, string, IPQUpdateServer>
        updateServerFactory;

    private IPQSnapshotServer? snapshotServer;
    private IPQUpdateServer? updateServer;

    public PQServer(ISnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig,
        IPQServerHeartBeatSender serverHeartBeatSender,
        ISocketDispatcher socketDispatcher,
        Func<ISocketDispatcher, IConnectionConfig, string, IPQSnapshotServer> snapShotServerFactory,
        Func<ISocketDispatcher, IConnectionConfig, string, string, IPQUpdateServer> updateServerFactory,
        Func<ISourceTickerQuoteInfo, T>? quoteFactory = null)
    {
        this.quoteFactory = quoteFactory ?? ReflectionHelper.CtorBinder<ISourceTickerQuoteInfo, T>();
        dispatcher = socketDispatcher;
        dispatcher.DispatcherDescription = "PQServer";
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
                ent.CopyFrom(quote);
                quote.HasUpdates = false;
                ent.PQSequenceId = seqId;
            }
            finally
            {
                ent.Lock.Release();
            }

            if (updateServer != null && updateServer.IsConnected) updateServer.Send(ent);
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
            updateServer.Disconnect();
            updateServer = null;
        }

        if (snapshotServer != null)
        {
            snapshotServer.Disconnect();
            snapshotServer = null;
        }

        IsStarted = false;
    }

    private void OnSnapshotRequest(ISocketSessionConnection cx, uint[] streamIDs)
    {
        foreach (var streamId in streamIDs)
            if (entities.TryGetValue(streamId, out var ent))
                snapshotServer!.Send(cx, ent!);
    }

    #region FeedReferential management

    public bool IsStarted { get; private set; }

    public void StartServices()
    {
        if (!IsStarted)
        {
            snapshotServer = snapShotServerFactory(dispatcher,
                snapshotUpdatePricingServerConfig.SnapshotConnectionConfig!,
                snapshotUpdatePricingServerConfig.Name!);
            snapshotServer.SnapshotClientStreamFromSubscriber.OnSnapshotRequest += OnSnapshotRequest;
            snapshotServer.Connect();
            updateServer = updateServerFactory(dispatcher,
                snapshotUpdatePricingServerConfig.UpdateConnectionConfig!,
                snapshotUpdatePricingServerConfig.Name!,
                snapshotUpdatePricingServerConfig.UpdateConnectionConfig!.NetworkSubAddress!);
            updateServer.SocketStreamFromSubscriber.BlockUntilConnected();
            serverHeartBeatSender.UpdateServer = updateServer;
            IsStarted = true;
        }
    }

    #endregion
}
