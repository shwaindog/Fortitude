#region

using System.Reactive.Disposables;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public class PQClient : IDisposable
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQClient));
    private readonly bool allowUpdatesCatchup;

    private readonly IDictionary<string, PQPricingServerSubscriptionContext> feeds =
        new Dictionary<string, PQPricingServerSubscriptionContext>();

    private readonly IOSParallelController osParallelController;
    private readonly IPQClientSyncMonitoring pqClientSyncMonitoring;
    private readonly IPricingClientConfigRepository pricingServersConfigRepository;
    private readonly IIntraOSThreadSignal shutDownSignal;
    private readonly IPQConversationRepository<IPQSnapshotClient> snapshotClientFactory;
    private readonly IPQQuoteSerializerRepository snapshotSerializationRepository = new PQQuoteSerializerRepository();

    private readonly object unsubscribeSyncLock = new();
    private readonly IPQConversationRepository<IPQUpdateClient> updateClientFactory;

    private int missingRefs;

    public PQClient(IPricingClientConfigRepository pricingServersConfigRepository,
        ISocketDispatcherResolver socketDispatcherResolver,
        bool allowUpdatesCatchup = true,
        IPQConversationRepository<IPQUpdateClient>? updateClientFactory = null,
        IPQConversationRepository<IPQSnapshotClient>? snapshotClientFactory = null)
    {
        osParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        shutDownSignal = osParallelController.SingleOSThreadActivateSignal(false);
        pqClientSyncMonitoring = new PQClientSyncMonitoring(GetSourceServerConfig, RequestSnapshots);
        this.pricingServersConfigRepository = pricingServersConfigRepository;
        this.allowUpdatesCatchup = allowUpdatesCatchup;
        this.snapshotClientFactory = snapshotClientFactory ?? new PQSnapshotClientRepository(socketDispatcherResolver);
        this.updateClientFactory = updateClientFactory ?? new PQUpdateClientRepository(socketDispatcherResolver);
    }

    public void Dispose()
    {
        lock (feeds)
        {
            foreach (var ctxt in feeds.Values)
                if (ctxt.PricingServerConfig != null)
                {
                    foreach (var sub in ctxt.Subscriptions)
                        Unsubscribe(ctxt.PricingServerConfig, sub.Ticker);
                    ctxt.Subscriptions.Clear();
                }

            feeds.Clear();
            missingRefs = 0;
            shutDownSignal.Set();
        }
    }

    public IPQTickerFeedSubscriptionQuoteStream<T>? GetQuoteStream<T>(
        ISourceTickerQuoteInfo sourceTickerQuoteInfo, uint syncRetryMsOverride = 60000) where T : PQLevel0Quote, new()
    {
        PQPricingServerSubscriptionContext? retrievePricingServerSubscriptionContext;
        lock (feeds)
        {
            if (!feeds.TryGetValue(sourceTickerQuoteInfo.Source, out retrievePricingServerSubscriptionContext))
            {
                feeds.Add(sourceTickerQuoteInfo.Source, retrievePricingServerSubscriptionContext =
                    new PQPricingServerSubscriptionContext());
                retrievePricingServerSubscriptionContext.PricingServerConfig =
                    GetFeedReferential(sourceTickerQuoteInfo.Source);
            }
        }

        if (retrievePricingServerSubscriptionContext.Subscriptions.Select(csub => csub.Source + csub.Ticker)
            .Any(st => st == sourceTickerQuoteInfo.Source + sourceTickerQuoteInfo.Ticker))
            throw new Exception("Subscription for " + sourceTickerQuoteInfo.Ticker + " on " +
                                sourceTickerQuoteInfo.Source + " already exists");
        var sub = CreateQuoteStream<T>(sourceTickerQuoteInfo, retrievePricingServerSubscriptionContext
            .PricingServerConfig, syncRetryMsOverride);
        if (sub == null) return null;
        retrievePricingServerSubscriptionContext.Subscriptions.Add(sub);
        sub.AddCleanupAction(Disposable.Create(() =>
        {
            retrievePricingServerSubscriptionContext.Subscriptions.Remove(sub);
            if (retrievePricingServerSubscriptionContext.PricingServerConfig != null)
                Unsubscribe(retrievePricingServerSubscriptionContext.PricingServerConfig, sub.Ticker);
            if (retrievePricingServerSubscriptionContext.Subscriptions.Count != 0) return;
            lock (feeds)
            {
                feeds.Remove(sub.Source);
            }
        }));
        return sub;
    }

    private PQTickerFeedSubscriptionQuoteStream<T>? CreateQuoteStream<T>(
        ISourceTickerQuoteInfo sourceTickerQuoteInfo, ISnapshotUpdatePricingServerConfig? marketsServerConfig,
        uint syncRetryMsOverride)
        where T : PQLevel0Quote, new()
    {
        PQTickerFeedSubscriptionQuoteStream<T>? pqTickerFeedSubscriptionQuoteStream = null;
        ISourceTickerPublicationConfig? sourceTickerPublicationConfig = null;
        if (marketsServerConfig != null)
            sourceTickerPublicationConfig = marketsServerConfig.SourceTickerPublicationConfigs?.FirstOrDefault(
                tii => tii.Ticker == sourceTickerQuoteInfo.Ticker);
        if (sourceTickerPublicationConfig != null)
        {
            var quoteDeserializer = snapshotSerializationRepository.GetQuoteDeserializer(sourceTickerPublicationConfig);
            if (quoteDeserializer != null)
                throw new Exception("Subscription for " + sourceTickerQuoteInfo.Ticker + " on " +
                                    marketsServerConfig?.Name +
                                    " already exists");
            quoteDeserializer = snapshotSerializationRepository.CreateQuoteDeserializer<T>(
                new SourceTickerClientAndPublicationConfig(
                    sourceTickerPublicationConfig, syncRetryMsOverride, allowUpdatesCatchup));

            if (quoteDeserializer is IPQDeserializer<T> pqQuoteDeserializer)
            {
                pqTickerFeedSubscriptionQuoteStream = new PQTickerFeedSubscriptionQuoteStream<T>(marketsServerConfig!,
                    sourceTickerQuoteInfo, pqQuoteDeserializer.PublishedQuote);
                var disposable = pqQuoteDeserializer.Subscribe(pqTickerFeedSubscriptionQuoteStream);
                pqTickerFeedSubscriptionQuoteStream.AddCleanupAction(disposable);
            }
            else
            {
                throw new ArgumentException("Unexpected quote serializer returned.");
            }

            pqClientSyncMonitoring.RegisterNewDeserializer(quoteDeserializer);

            var updateClient
                = updateClientFactory.RetrieveOrCreateConversation(marketsServerConfig!.UpdateConnectionConfig!);
            updateClient.MessageStreamDecoder.AddMessageDeserializer(sourceTickerPublicationConfig.Id
                , pqQuoteDeserializer);
            var snapShotClient
                = snapshotClientFactory.RetrieveOrCreateConversation(marketsServerConfig.SnapshotConnectionConfig!);
            snapShotClient.MessageStreamDecoder.AddMessageDeserializer(sourceTickerPublicationConfig.Id
                , pqQuoteDeserializer);

            pqClientSyncMonitoring.CheckStartMonitoring();

            Logger.Info("Subscribed to {0}", sourceTickerPublicationConfig);
        }
        else
        {
            Logger.Warn($"Cannot subscribe to Ticker {sourceTickerQuoteInfo.Ticker} for source " +
                        $"{marketsServerConfig?.Name ?? "serverConfig is null"} not found in config repo " +
                        $"{marketsServerConfig?.SnapshotConnectionConfig?.Current.Hostname ?? "serverConfig is null"}");
        }

        return pqTickerFeedSubscriptionQuoteStream;
    }

    private void Unsubscribe(ISnapshotUpdatePricingServerConfig feedRef, string ticker)
    {
        var sourceTickerPublicationConfig =
            feedRef.SourceTickerPublicationConfigs!.FirstOrDefault(tii => tii.Ticker == ticker);
        if (sourceTickerPublicationConfig != null)
        {
            var quoteDeserializer = snapshotSerializationRepository.GetQuoteDeserializer(sourceTickerPublicationConfig)
                                    ?? throw new Exception(
                                        $"Subscription for {ticker} on {feedRef.Name} does not exists");


            lock (unsubscribeSyncLock)
            {
                snapshotClientFactory.RemoveConversation(feedRef.SnapshotConnectionConfig!);
                updateClientFactory.RemoveConversation(feedRef.UpdateConnectionConfig!);

                pqClientSyncMonitoring.UnregisterSerializer(quoteDeserializer);
            }

            snapshotSerializationRepository.RemoveQuoteDeserializer(sourceTickerPublicationConfig);

            if (!snapshotSerializationRepository.HasPictureDeserializers)
                pqClientSyncMonitoring.CheckStopMonitoring();

            Logger.Info($"Unsubscribed from {sourceTickerPublicationConfig}");
        }
        else
        {
            Logger.Warn($"Cannot unsubscribe to Ticker {ticker} for source {feedRef.Name} not found in " +
                        $"config repo {feedRef.SnapshotConnectionConfig?.Current.Hostname}");
        }
    }

    private ISnapshotUpdatePricingServerConfig? GetFeedReferential(string feedName)
    {
        var feedRef = pricingServersConfigRepository.Find(feedName);
        if (feedRef != null) return feedRef;
        if (++missingRefs == 1)
            osParallelController.ScheduleWithEarlyTrigger(shutDownSignal, GetMissingFeedReferentials!, 60000);
        return null;
    }

    private void GetMissingFeedReferentials(object state, bool timeout)
    {
        lock (feeds)
        {
            if (missingRefs <= 0) return;
            foreach (var feed in feeds)
                if (feed.Value.PricingServerConfig == null)
                {
                    feed.Value.PricingServerConfig = pricingServersConfigRepository.Find(feed.Key);
                    if (feed.Value.PricingServerConfig != null)
                        missingRefs--;
                }

            if (missingRefs > 0)
                osParallelController.ScheduleWithEarlyTrigger(shutDownSignal, GetMissingFeedReferentials!, 60000);
        }
    }

    internal ISnapshotUpdatePricingServerConfig? GetSourceServerConfig(string sourceName)
    {
        ISnapshotUpdatePricingServerConfig? result = null;
        lock (feeds)
        {
            if (feeds.TryGetValue(sourceName, out var context))
                result = context.PricingServerConfig;
        }

        return result;
    }

    internal void RequestSnapshots(INetworkTopicConnectionConfig cfg, List<IUniqueSourceTickerIdentifier> streams)
    {
        var snap = snapshotClientFactory.RetrieveConversation(cfg);
        snap?.RequestSnapshots(streams);
    }

    private class PQPricingServerSubscriptionContext
    {
        public readonly List<IPQTickerFeedSubscription> Subscriptions = new();
        public ISnapshotUpdatePricingServerConfig? PricingServerConfig;
    }
}
