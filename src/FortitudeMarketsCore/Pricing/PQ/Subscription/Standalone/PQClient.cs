// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reactive.Disposables;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public class PQClient : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQClient));

    private readonly IPQClientQuoteDeserializerRepository deserializationRepository
        = new PQClientQuoteDeserializerRepository("PQClient", new Recycler());

    private readonly IMarketsConfig marketsConfig;

    private readonly IOSParallelController   osParallelController;
    private readonly IPQClientSyncMonitoring pqClientSyncMonitoring;
    private readonly IIntraOSThreadSignal    shutDownSignal;

    private readonly IPQConversationRepository<IPQSnapshotClient> snapshotClientFactory;

    private readonly IDictionary<string, PQSourceSubscriptionsContext> sourceSubscriptions =
        new Dictionary<string, PQSourceSubscriptionsContext>();

    private readonly ISourceTickerQuoteInfoRegistry sourceTickerQuoteInfoRegistry;

    private readonly object unsubscribeSyncLock = new();

    private readonly IPQConversationRepository<IPQUpdateClient> updateClientFactory;

    private int missingRefs;

    public PQClient
    (IMarketsConfig marketsConfig, ISocketDispatcherResolver socketDispatcherResolver
      , IPQConversationRepository<IPQUpdateClient>? updateClientFactory = null
      , IPQConversationRepository<IPQSnapshotClient>? snapshotClientFactory = null)
    {
        osParallelController   = OSParallelControllerFactory.Instance.GetOSParallelController;
        shutDownSignal         = osParallelController.SingleOSThreadActivateSignal(false);
        pqClientSyncMonitoring = new PQClientSyncMonitoring(GetSourceServerConfig, RequestSnapshots);
        sourceTickerQuoteInfoRegistry =
            new SourceTickerQuoteInfoRegistry
                ("PQClient", marketsConfig.Markets
                                          .SelectMany(mcc => mcc.AllSourceTickerInfos));

        this.marketsConfig         = marketsConfig;
        this.snapshotClientFactory = snapshotClientFactory ?? new PQSnapshotClientRepository(socketDispatcherResolver);
        this.updateClientFactory   = updateClientFactory ?? new PQUpdateClientRepository(socketDispatcherResolver);
    }

    public void Dispose()
    {
        lock (sourceSubscriptions)
        {
            foreach (var ssc in sourceSubscriptions.Values)
                if (ssc.MarketConnectionConfig != null)
                {
                    foreach (var sub in ssc.Subscriptions) Unsubscribe(ssc.MarketConnectionConfig, sub.Ticker);
                    ssc.Subscriptions.Clear();
                }

            sourceSubscriptions.Clear();
            missingRefs = 0;
            shutDownSignal.Set();
        }
    }

    public ISourceTickerQuoteInfoRegistry RequestSourceTickerForSource(string sourceName)
    {
        PQSourceSubscriptionsContext? sourceSubscriptionsContext;
        lock (sourceSubscriptions)
        {
            if (!sourceSubscriptions.TryGetValue(sourceName, out sourceSubscriptionsContext))
            {
                sourceSubscriptions.Add
                    (sourceName, sourceSubscriptionsContext = new PQSourceSubscriptionsContext());
                sourceSubscriptionsContext.MarketConnectionConfig = GetFeedReferential(sourceName);
            }
        }

        if (sourceSubscriptionsContext.MarketConnectionConfig == null)
            throw new Exception($"No MarketConnectionConfig exists for source {sourceName}");
        var pricingServerConfig = sourceSubscriptionsContext.MarketConnectionConfig.PricingServerConfig;

        if (pricingServerConfig == null) throw new Exception($"No PricingServerConfig exists for source {sourceName}");

        var snapShotClient = snapshotClientFactory.RetrieveOrCreateConversation(pricingServerConfig.SnapshotConnectionConfig);
        snapShotClient.SourceTickerQuoteInfoRegistry = sourceTickerQuoteInfoRegistry;

        snapShotClient.RequestSourceTickerQuoteInfoList();
        return sourceTickerQuoteInfoRegistry;
    }

    public ISourceTickerQuoteInfoRegistry RequestSourceTickerForAllSources()
    {
        foreach (var marketConnectionConfig in marketsConfig.Markets) RequestSourceTickerForSource(marketConnectionConfig.Name);
        return sourceTickerQuoteInfoRegistry;
    }

    public IPQTickerFeedSubscriptionQuoteStream<T>? GetQuoteStream<T>
    (
        ISourceTickerQuoteInfo sourceTickerQuoteInfo, uint syncRetryMsOverride = 60000) where T : PQLevel0Quote, new()
    {
        PQSourceSubscriptionsContext? sourceSubscriptionsContext;
        lock (sourceSubscriptions)
        {
            if (!sourceSubscriptions.TryGetValue(sourceTickerQuoteInfo.Source, out sourceSubscriptionsContext))
            {
                sourceSubscriptions.Add(sourceTickerQuoteInfo.Source, sourceSubscriptionsContext =
                                            new PQSourceSubscriptionsContext());
                sourceSubscriptionsContext.MarketConnectionConfig = GetFeedReferential(sourceTickerQuoteInfo.Source);
            }
        }

        var foundSourceTickerQuoteConfig
            = sourceTickerQuoteInfoRegistry.GetSourceTickerInfo(sourceTickerQuoteInfo.Source, sourceTickerQuoteInfo.Ticker);
        if (sourceSubscriptionsContext.MarketConnectionConfig == null || foundSourceTickerQuoteConfig == null) return null;
        if (sourceSubscriptionsContext.Subscriptions.Select(ssc => ssc.Source + ssc.Ticker)
                                      .Any(st => st == sourceTickerQuoteInfo.Source + sourceTickerQuoteInfo.Ticker))
            throw new Exception("Subscription for " + sourceTickerQuoteInfo.Ticker + " on " +
                                sourceTickerQuoteInfo.Source + " already exists");
        var foundMarketConnectionConfig = sourceSubscriptionsContext.MarketConnectionConfig;
        var sub                         = CreateQuoteStream<T>(sourceTickerQuoteInfo, foundMarketConnectionConfig);
        if (sub == null) return null;
        sourceSubscriptionsContext.Subscriptions.Add(sub);
        sub.AddCleanupAction(Disposable.Create(() =>
        {
            sourceSubscriptionsContext.Subscriptions.Remove(sub);
            if (sourceSubscriptionsContext.MarketConnectionConfig != null) Unsubscribe(sourceSubscriptionsContext.MarketConnectionConfig, sub.Ticker);
            if (sourceSubscriptionsContext.Subscriptions.Count != 0) return;
            lock (sourceSubscriptions)
            {
                sourceSubscriptions.Remove(sub.Source);
            }
        }));
        return sub;
    }

    private PQTickerFeedSubscriptionQuoteStream<T>? CreateQuoteStream<T>
        (ISourceTickerQuoteInfo sourceTickerQuoteInfo, IMarketConnectionConfig? marketConnectionConfig) where T : PQLevel0Quote, new()
    {
        PQTickerFeedSubscriptionQuoteStream<T>? pqTickerFeedSubscriptionQuoteStream = null;

        IPricingServerConfig? pricingServerConfig = null;

        if (marketConnectionConfig != null) pricingServerConfig = marketConnectionConfig.PricingServerConfig;
        if (pricingServerConfig != null)
        {
            var tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig(sourceTickerQuoteInfo, pricingServerConfig);
            var quoteDeserializer               = deserializationRepository.GetDeserializer(sourceTickerQuoteInfo);
            if (quoteDeserializer != null)
                throw new Exception("Subscription for " + sourceTickerQuoteInfo.Ticker + " on " +
                                    marketConnectionConfig?.Name +
                                    " already exists");
            quoteDeserializer = deserializationRepository.CreateQuoteDeserializer<T>(tickerPricingSubscriptionConfig);

            if (quoteDeserializer is IPQQuoteDeserializer<T> pqQuoteDeserializer)
            {
                pqTickerFeedSubscriptionQuoteStream = new PQTickerFeedSubscriptionQuoteStream<T>(pricingServerConfig,
                                                                                                 sourceTickerQuoteInfo
                                                                                               , pqQuoteDeserializer.PublishedQuote);
                var disposable = pqQuoteDeserializer.Subscribe(pqTickerFeedSubscriptionQuoteStream);
                pqTickerFeedSubscriptionQuoteStream.AddCleanupAction(disposable);
            }
            else
            {
                throw new ArgumentException("Unexpected quote serializer returned.");
            }

            pqClientSyncMonitoring.RegisterNewDeserializer(quoteDeserializer);

            var updateClient
                = updateClientFactory.RetrieveOrCreateConversation(pricingServerConfig.UpdateConnectionConfig);
            updateClient.DeserializerRepository.RegisterDeserializer(sourceTickerQuoteInfo.SourceTickerId, pqQuoteDeserializer);
            var snapShotClient
                = snapshotClientFactory.RetrieveOrCreateConversation(pricingServerConfig.SnapshotConnectionConfig);
            snapShotClient.DeserializerRepository.RegisterDeserializer(sourceTickerQuoteInfo.SourceTickerId, pqQuoteDeserializer);

            pqClientSyncMonitoring.CheckStartMonitoring();

            Logger.Info("Subscribed to {0}", pricingServerConfig);
        }
        else
        {
            Logger.Warn($"Cannot subscribe to Ticker {sourceTickerQuoteInfo.Ticker} for source " +
                        $"{marketConnectionConfig?.Name ?? "serverConfig is null"} not found in MarketConnectionConfig " +
                        $"{marketConnectionConfig?.ToString() ?? "serverConfig is null"}");
        }

        return pqTickerFeedSubscriptionQuoteStream;
    }

    private void Unsubscribe(IMarketConnectionConfig feedRef, string ticker)
    {
        var sourceTickerPublicationConfig = feedRef.GetSourceTickerInfo(ticker);
        if (sourceTickerPublicationConfig != null)
        {
            var pricingServerConfig = feedRef.PricingServerConfig!;
            var quoteDeserializer =
                deserializationRepository.GetDeserializer(sourceTickerPublicationConfig)
             ?? throw new Exception($"Subscription for {ticker} on {feedRef.Name} does not exists");


            lock (unsubscribeSyncLock)
            {
                snapshotClientFactory.RemoveConversation(pricingServerConfig.SnapshotConnectionConfig);
                updateClientFactory.RemoveConversation(pricingServerConfig.UpdateConnectionConfig);

                pqClientSyncMonitoring.UnregisterSerializer(quoteDeserializer);
            }

            deserializationRepository.UnregisterDeserializer(sourceTickerPublicationConfig);

            if (!deserializationRepository.RegisteredMessageIds.Any()) pqClientSyncMonitoring.CheckStopMonitoring();

            Logger.Info($"Unsubscribed from {sourceTickerPublicationConfig}");
        }
        else
        {
            Logger.Warn($"Cannot unsubscribe to Ticker {ticker} for source {feedRef.Name} not found in " +
                        $"config repo {feedRef}");
        }
    }

    private IMarketConnectionConfig? GetFeedReferential(string feedName)
    {
        var feedRef = marketsConfig.Find(feedName);
        if (feedRef != null) return feedRef;
        if (++missingRefs == 1) osParallelController.ScheduleWithEarlyTrigger(shutDownSignal, GetMissingMarketConnectionConfig!, 60000);
        return null;
    }

    private void GetMissingMarketConnectionConfig(object state, bool timeout)
    {
        lock (sourceSubscriptions)
        {
            if (missingRefs <= 0) return;
            foreach (var feed in sourceSubscriptions)
                if (feed.Value.MarketConnectionConfig == null)
                {
                    feed.Value.MarketConnectionConfig = marketsConfig.Find(feed.Key);
                    if (feed.Value.MarketConnectionConfig != null) missingRefs--;
                }

            if (missingRefs > 0) osParallelController.ScheduleWithEarlyTrigger(shutDownSignal, GetMissingMarketConnectionConfig!, 60000);
        }
    }

    internal IMarketConnectionConfig? GetSourceServerConfig(string sourceName)
    {
        IMarketConnectionConfig? result = null;
        lock (sourceSubscriptions)
        {
            if (sourceSubscriptions.TryGetValue(sourceName, out var context)) result = context.MarketConnectionConfig;
        }

        return result;
    }

    internal void RequestSnapshots(INetworkTopicConnectionConfig cfg, List<ISourceTickerQuoteInfo> streams)
    {
        var snap = snapshotClientFactory.RetrieveConversation(cfg);
        snap?.RequestSnapshots(streams);
    }

    private class PQSourceSubscriptionsContext
    {
        public readonly List<IPQTickerFeedSubscription> Subscriptions = new();

        public IMarketConnectionConfig? MarketConnectionConfig;
    }
}
