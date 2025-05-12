// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reactive.Disposables;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

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

    private readonly ISourceTickerInfoRegistry sourceTickerInfoRegistry;

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
        sourceTickerInfoRegistry =
            new SourceTickerInfoRegistry
                ("PQClient", marketsConfig.Markets.Values
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

    public ISourceTickerInfoRegistry RequestSourceTickerForSource(string sourceName)
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
        snapShotClient.SourceTickerInfoRegistry = sourceTickerInfoRegistry;

        snapShotClient.RequestSourceTickerInfoList();
        return sourceTickerInfoRegistry;
    }

    public ISourceTickerInfoRegistry RequestSourceTickerForAllSources()
    {
        foreach (var marketConnectionConfig in marketsConfig.Markets.Values) RequestSourceTickerForSource(marketConnectionConfig.SourceName);
        return sourceTickerInfoRegistry;
    }

    public IPQTickerFeedSubscriptionQuoteStream<T>? GetQuoteStream<T>
    (
        ISourceTickerInfo sourceTickerInfo, uint syncRetryMsOverride = 60000) where T : PQPublishableTickInstant, new()
    {
        PQSourceSubscriptionsContext? sourceSubscriptionsContext;
        lock (sourceSubscriptions)
        {
            if (!sourceSubscriptions.TryGetValue(sourceTickerInfo.SourceName, out sourceSubscriptionsContext))
            {
                sourceSubscriptions.Add(sourceTickerInfo.SourceName, sourceSubscriptionsContext =
                                            new PQSourceSubscriptionsContext());
                sourceSubscriptionsContext.MarketConnectionConfig = GetFeedReferential(sourceTickerInfo.SourceName);
            }
        }

        var foundSourceTickerQuoteConfig
            = sourceTickerInfoRegistry.GetSourceTickerInfo(sourceTickerInfo.SourceName, sourceTickerInfo.InstrumentName);
        if (sourceSubscriptionsContext.MarketConnectionConfig == null || foundSourceTickerQuoteConfig == null) return null;
        if (sourceSubscriptionsContext.Subscriptions.Select(ssc => ssc.Source + ssc.Ticker)
                                      .Any(st => st == sourceTickerInfo.SourceName + sourceTickerInfo.InstrumentName))
            throw new Exception("Subscription for " + sourceTickerInfo.InstrumentName + " on " +
                                sourceTickerInfo.SourceName + " already exists");
        var foundMarketConnectionConfig = sourceSubscriptionsContext.MarketConnectionConfig;
        var sub                         = CreateQuoteStream<T>(sourceTickerInfo, foundMarketConnectionConfig);
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
        (ISourceTickerInfo sourceTickerInfo, IMarketConnectionConfig? marketConnectionConfig) where T : PQPublishableTickInstant, new()
    {
        PQTickerFeedSubscriptionQuoteStream<T>? pqTickerFeedSubscriptionQuoteStream = null;

        IPricingServerConfig? pricingServerConfig = null;

        if (marketConnectionConfig != null) pricingServerConfig = marketConnectionConfig.PricingServerConfig;
        if (pricingServerConfig != null)
        {
            var tickerPricingSubscriptionConfig = new TickerPricingSubscriptionConfig(sourceTickerInfo, pricingServerConfig);
            var quoteDeserializer               = deserializationRepository.GetDeserializer(sourceTickerInfo);
            if (quoteDeserializer != null)
                throw new Exception("Subscription for " + sourceTickerInfo.InstrumentName + " on " +
                                    marketConnectionConfig?.SourceName +
                                    " already exists");
            quoteDeserializer = deserializationRepository.CreateQuoteDeserializer<T>(tickerPricingSubscriptionConfig);

            if (quoteDeserializer is IPQQuoteDeserializer<T> pqQuoteDeserializer)
            {
                pqTickerFeedSubscriptionQuoteStream = new PQTickerFeedSubscriptionQuoteStream<T>(pricingServerConfig,
                                                                                                 sourceTickerInfo
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
            updateClient.DeserializerRepository.RegisterDeserializer(sourceTickerInfo.SourceTickerId, pqQuoteDeserializer);
            var snapShotClient
                = snapshotClientFactory.RetrieveOrCreateConversation(pricingServerConfig.SnapshotConnectionConfig);
            snapShotClient.DeserializerRepository.RegisterDeserializer(sourceTickerInfo.SourceTickerId, pqQuoteDeserializer);

            pqClientSyncMonitoring.CheckStartMonitoring();

            Logger.Info("Subscribed to {0}", pricingServerConfig);
        }
        else
        {
            Logger.Warn($"Cannot subscribe to Ticker {sourceTickerInfo.InstrumentName} for source " +
                        $"{marketConnectionConfig?.SourceName ?? "serverConfig is null"} not found in MarketConnectionConfig " +
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
             ?? throw new Exception($"Subscription for {ticker} on {feedRef.SourceName} does not exists");


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
            Logger.Warn($"Cannot unsubscribe to Ticker {ticker} for source {feedRef.SourceName} not found in " +
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

    internal void RequestSnapshots(INetworkTopicConnectionConfig cfg, List<ISourceTickerInfo> streams)
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
