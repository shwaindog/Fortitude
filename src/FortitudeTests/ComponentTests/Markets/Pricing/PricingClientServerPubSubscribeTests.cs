#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

#endregion

namespace FortitudeTests.ComponentTests.Markets.Pricing;

[TestClass]
[NoMatchingProductionClass]
public class PricingClientServerPubSubscribeTests
{
    private static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(typeof(PricingClientServerPubSubscribeTests));

    private LocalHostPQClientTestSetup pqClientSetup = null!;

    public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        pqClientSetup = new LocalHostPQClientTestSetup();
        pqClientSetup.InitializeClientPrereqs();
    }

    [TestCleanup]
    public void TearDown()
    {
        pqClientSetup.TearDown();
        FLoggerFactory.GracefullyTerminateProcessLogging();
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields()
    {
        Logger.Info("Starting Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServerL2QuoteServerSetup = new LocalHostPQServerLevel2QuoteTestSetup();

        var autoResetEvent = new AutoResetEvent(false);
        var pqPublisher = pqServerL2QuoteServerSetup.CreatePQPublisher();
        var sourcePriceQuote = pqServerL2QuoteServerSetup.Level2PriceQuote;

        Logger.Info("Started PQServer");
        // logger.Info("About to publish first quote {0}" sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        // setup listener after publish means first message will be missed and snapshot will be required.
        ILevel2Quote? alwaysUpdatedQuote = null;

        var pqClient = pqClientSetup.CreatePQClient();
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == pqServerL2QuoteServerSetup.SourceTickerQuoteInfo.Source &&
                                  stqi.Ticker == pqServerL2QuoteServerSetup.SourceTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel2Quote>(pqServerL2QuoteServerSetup.SourceTickerQuoteInfo, 0);
        streamSubscription!.Subscribe(
            pQuote =>
            {
                Logger.Info("Client Received pQuote {0}", pQuote);
                alwaysUpdatedQuote = pqServerL2QuoteServerSetup.ConvertPQToLevel2QuoteWithSourceNameLayer(pQuote);
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
        Logger.Info("Started PQClient and subscribed");

        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SinglePrice == 0m) &&
               count++ < 10) // depending on pub subscribe first quote may be empty
        {
            Logger.Info("Awaiting first non-empty quote as alwaysUpdateQuote is {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(3_000);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        Logger.Info("Received first update {0}", alwaysUpdatedQuote);
        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("First diff.");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        ResetL2QuoteLayers(sourcePriceQuote);

        NonPublicInvocator.SetAutoPropertyInstanceField(sourcePriceQuote,
            (Level2PriceQuote pq) => pq.AdapterSentTime, new DateTime(2015, 08, 15, 11, 36, 13));
        // adapter becomes sourceTime on Send
        Logger.Info("About to publish second empty quote. {0}", sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(3_000); // 20 ms seems to work expand wait time if errors
        Logger.Info("Received second update {0}", alwaysUpdatedQuote);
        pqClient.Dispose();
        pqPublisher.Dispose();
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("Second diff.");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        autoResetEvent.WaitOne(3_000);
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
        Logger.Info("Finished Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        //FLoggerFactory.GracefullyTerminateProcessLogging();
        pqServerL2QuoteServerSetup.TearDown();
    }

    private static void SetExpectedDiffFieldsToSame(ILevel2Quote destinationSnapshot, ILevel2Quote sourcePriceQuote)
    {
        NonPublicInvocator.SetAutoPropertyInstanceField(destinationSnapshot,
            (Level2PriceQuote pq) => pq.AdapterSentTime, sourcePriceQuote.AdapterSentTime);
        NonPublicInvocator.SetAutoPropertyInstanceField(destinationSnapshot,
            (Level2PriceQuote pq) => pq.ClientReceivedTime, sourcePriceQuote.ClientReceivedTime);
        NonPublicInvocator.SetAutoPropertyInstanceField(destinationSnapshot,
            (Level2PriceQuote pq) => pq.IsAskPriceTopUpdated, sourcePriceQuote.IsAskPriceTopUpdated);
        NonPublicInvocator.SetAutoPropertyInstanceField(destinationSnapshot,
            (Level2PriceQuote pq) => pq.IsBidPriceTopUpdated, sourcePriceQuote.IsBidPriceTopUpdated);
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Lvl3TraderLayerQuoteFullDepthLastTraderTrade_SyncViaUpdateAndResets_PublishesAllFieldsAndResets()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize,
            LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
            LastTradedFlags.LastTradedTime);
        // setup listener if listening before publishing the updates should be enough that no snapshot is required.
        var autoResetEvent = new AutoResetEvent(false);
        ILevel3Quote? alwaysUpdatedQuote = null;
        var clientConnectionConfig = pqClientSetup.ClientConnectionsConfigRepository.Find(LocalHostPQTestSetupCommon.ExchangeName);
        clientConnectionConfig!.SourceTickerConfig = null;

        var pqClient = pqClientSetup.CreatePQClient();

        var pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();

        var pqPublisher = pqServerL3QuoteServerSetup.CreatePQPublisher();
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == pqServerL3QuoteServerSetup.SourceTickerQuoteInfo.Source &&
                                  stqi.Ticker == pqServerL3QuoteServerSetup.SourceTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel3Quote>(pqServerL3QuoteServerSetup.SourceTickerQuoteInfo, 0);

        streamSubscription!.Subscribe(
            pQuote =>
            {
                alwaysUpdatedQuote = pqServerL3QuoteServerSetup.ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(pQuote);
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
        var sourcePriceQuote = pqServerL3QuoteServerSetup.GenerateL3QuoteWithTraderLayerAndLastTrade();
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        autoResetEvent.WaitOne(2_000); // 30 ms seems to work expand wait time if errors 
        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        ResetL2QuoteLayers(sourcePriceQuote);

        SetExpectedDiffFieldsToSame(sourcePriceQuote, destinationSnapshot);
        // adapter becomes sourceTime on Send
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(2_000); // 20 ms seems to work expand wait time if errors
        pqClient.Dispose();
        pqPublisher.Dispose();
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
        //FLoggerFactory.GracefullyTerminateProcessLogging();
        pqServerL3QuoteServerSetup.TearDown();
    }

    private static void ResetL2QuoteLayers(ILevel2Quote level2PriceQuote)
    {
        ((OrderBook)level2PriceQuote.BidBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsBidBookChanged = true;
        ((OrderBook)level2PriceQuote.AskBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsAskBookChanged = true;
    }
}
