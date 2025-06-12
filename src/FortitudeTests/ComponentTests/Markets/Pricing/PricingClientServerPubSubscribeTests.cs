// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Converters;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeTests.FortitudeMarkets.Pricing.PQ;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

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
        // FLoggerFactory.WaitUntilDrained();
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields()
    {
        Logger.Info("Starting Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);

        var pqServerL2QuoteServerSetup = new LocalHostPQServerLevel2QuoteTestSetup();
        pqServerL2QuoteServerSetup.LayerDetails |= LayerFlags.Ladder;
        pqServerL2QuoteServerSetup.InitializeLevel2QuoteConfig();

        var pqServerL2QuoteServerConfig = pqServerL2QuoteServerSetup.DefaultServerMarketsConfig.ShiftPortsBy(20);
        var pricingClientConfig         = pqServerL2QuoteServerConfig.ToggleProtocolDirection("PQClient_Level2");

        var autoResetEvent = new AutoResetEvent(false);
        var pqPublisher = pqServerL2QuoteServerSetup.CreatePQPublisher(pqServerL2QuoteServerConfig.Find(LocalHostPQTestSetupCommon.ExchangeName));
        var sourcePriceQuote = pqServerL2QuoteServerSetup.Level2PriceQuote;

        Logger.Info("Started PQServer");
        // logger.Info("About to publish first quote {0}" sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        // setup listener after publish means first message will be missed and snapshot will be required.
        IPublishableLevel2Quote? alwaysUpdatedQuote = null;

        var pqClient = pqClientSetup.CreatePQClient(pricingClientConfig);

        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.SourceName == pqServerL2QuoteServerSetup.FirstTickerInfo.SourceName &&
                                  stqi.InstrumentName == pqServerL2QuoteServerSetup.FirstTickerInfo.InstrumentName))
                autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQPublishableLevel2Quote>(pqServerL2QuoteServerSetup.FirstTickerInfo, 0);
        var updateNonEmpty     = true;
        streamSubscription!.Subscribe
            (pQuote =>
            {
                // Logger.Info("Client Received pQuote {0}", pQuote);
                if (updateNonEmpty || pQuote.SingleTickValue == 0)
                {
                    alwaysUpdatedQuote?.Recycle();
                    alwaysUpdatedQuote = pQuote.ToPublishableL2PriceQuote();
                }
                else
                    Logger.Info($"Skipping non-empty, {nameof(updateNonEmpty)}: {updateNonEmpty} with {nameof(pQuote.SingleTickValue)}: {pQuote.SingleTickValue}");
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
        Logger.Info("Started PQClient and subscribed");

        pqPublisher.PublishQuoteUpdateAs(sourcePriceQuote, PQMessageFlags.CompleteUpdate);

        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SingleTickValue == 0m) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0) Logger.Info("Awaiting first non-empty quote as alwaysUpdateQuote is {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(200);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("First diff.");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Thread.Sleep(1_000);
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        updateNonEmpty = false;
        ResetL2QuoteLayers(sourcePriceQuote);

        NonPublicInvocator.SetAutoPropertyInstanceField
            (sourcePriceQuote, (PublishableLevel2PriceQuote pq) => pq.AdapterSentTime, new DateTime(2015, 08, 15, 11, 36, 13));
        // adapter becomes sourceTime on Send
        Logger.Info("About to publish second empty quote. {0}", sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        alwaysUpdatedQuote = null;
        count              = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote!.SingleTickValue != 0m || 
                alwaysUpdatedQuote.FeedMarketConnectivityStatus != FeedConnectivityStatusFlags.None) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0) Logger.Info("Awaiting first empty quote as alwaysUpdateQuote = {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(50);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("Second diff.");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        var areEquivalent = sourcePriceQuote.AreEquivalent(destinationSnapshot);
        if (!areEquivalent)
        {
            Thread.Sleep(2_000);
        }
        Assert.IsTrue(areEquivalent);
        Logger.Info("Finished Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Logger.Info("Test complete start shutdown");
        pqServerL2QuoteServerSetup.TearDown();
    }

    private static void SetExpectedDiffFieldsToSame(IPublishableLevel2Quote destinationSnapshot, IPublishableLevel2Quote sourcePriceQuote)
    {
        ((IMutablePublishableLevel2Quote)destinationSnapshot).FeedMarketConnectivityStatus &= ~(FeedConnectivityStatusFlags.IsAdapterReplay);
        ((IMutablePublishableLevel2Quote)destinationSnapshot).FeedMarketConnectivityStatus |= (sourcePriceQuote.FeedMarketConnectivityStatus & FeedConnectivityStatusFlags.IsAdapterReplay);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (destinationSnapshot, (PublishableLevel2PriceQuote pq) => pq.AdapterSentTime, sourcePriceQuote.AdapterSentTime);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (destinationSnapshot, (PublishableLevel2PriceQuote pq) => pq.ClientReceivedTime, sourcePriceQuote.ClientReceivedTime);
        var nonPubQuote = destinationSnapshot.AsNonPublishable;
        NonPublicInvocator.SetAutoPropertyInstanceField
            (nonPubQuote, (Level2PriceQuote pq) => pq.IsAskPriceTopChanged, sourcePriceQuote.IsAskPriceTopChanged);
        NonPublicInvocator.SetAutoPropertyInstanceField
            (nonPubQuote, (Level2PriceQuote pq) => pq.IsBidPriceTopChanged, sourcePriceQuote.IsBidPriceTopChanged);
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Lvl3TraderLayerQuoteFullDepthLastTraderTrade_SyncViaUpdateAndResets_PublishesAllFieldsAndResets()
    {
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize,
              LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
              LastTradedFlags.LastTradedTime);
        // setup listener if listening before publishing the updates should be enough that no snapshot is required.
        var pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();
        pqServerL3QuoteServerSetup.LayerDetails |= LayerFlags.Ladder;
        pqServerL3QuoteServerSetup.InitializeLevel3QuoteConfig();

        var pricingServerConfig = pqServerL3QuoteServerSetup.DefaultServerMarketsConfig.ShiftPortsBy(2);
        var pricingClientConfig = pricingServerConfig.ToggleProtocolDirection("PQClient_Level3");
        var autoResetEvent      = new AutoResetEvent(false);

        IPublishableLevel3Quote? alwaysUpdatedQuote = null;

        var clientConnectionConfig = pricingClientConfig.Find(LocalHostPQTestSetupCommon.ExchangeName);

        clientConnectionConfig!.SourceTickerConfig = null;

        var pqClient = pqClientSetup.CreatePQClient(pricingClientConfig);


        var pqPublisher = pqServerL3QuoteServerSetup.CreatePQPublisher(pricingServerConfig.Find(LocalHostPQTestSetupCommon.ExchangeName));

        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.SourceName == pqServerL3QuoteServerSetup.SecondTickerInfo.SourceName &&
                                  stqi.InstrumentName == pqServerL3QuoteServerSetup.SecondTickerInfo.InstrumentName))
                autoResetEvent.Set();
        };

        var streamSubscription = pqClient.GetQuoteStream<PQPublishableLevel3Quote>(pqServerL3QuoteServerSetup.SecondTickerInfo, 0);
        var updateNonEmpty     = true;
        streamSubscription!.Subscribe
            (pQuote =>
            {
                Logger.Info("Client Received pQuote {0}", pQuote);
                if (updateNonEmpty || pQuote.SingleTickValue == 0)
                {
                    alwaysUpdatedQuote?.Recycle();
                    alwaysUpdatedQuote = pQuote.ToPublishableL3PriceQuote();
                }
                else
                    Logger.Info($"Skipping non-empty, {nameof(updateNonEmpty)}: {updateNonEmpty} with {nameof(pQuote.SingleTickValue)}: {pQuote.SingleTickValue}");
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
        var sourcePriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(pqServerL3QuoteServerSetup.SecondTickerInfo);
        pqPublisher.PublishQuoteUpdateAs(sourcePriceQuote, PQMessageFlags.CompleteUpdate);
        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SingleTickValue == 0m) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0) Logger.Info("Awaiting first non-empty quote as alwaysUpdateQuote is {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(50);
        }

        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("First diff");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        updateNonEmpty = false;
        ResetL2QuoteLayers(sourcePriceQuote);

        SetExpectedDiffFieldsToSame(sourcePriceQuote, destinationSnapshot);
        alwaysUpdatedQuote.Recycle();
        alwaysUpdatedQuote = null;
        // adapter becomes sourceTime on Send
        Logger.Info("About to publish = sourcePriceQuote with SingleTickValue {0}.", sourcePriceQuote.SingleTickValue);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(3_000); // 20 ms seems to work expand wait time if errors
        count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote!.SingleTickValue != 0m ||
                alwaysUpdatedQuote.FeedMarketConnectivityStatus != FeedConnectivityStatusFlags.None) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0) Logger.Info("Awaiting first empty quote as alwaysUpdateQuote = {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(50);
        }
        
        var isNotNull = alwaysUpdatedQuote != null;
        if (!isNotNull)
        {
            FLoggerFactory.WaitUntilDrained();
        }
        Assert.IsTrue(isNotNull);
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);

        Logger.Info("Second diff");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        var areEquivalent = sourcePriceQuote.AreEquivalent(destinationSnapshot);
        if (!areEquivalent)
        {
            FLoggerFactory.WaitUntilDrained();
            Thread.Sleep(2_000);
        }
        Assert.IsTrue(areEquivalent);

        Logger.Info("Test complete start shutdown");
        FLoggerFactory.WaitUntilDrained();
        pqServerL3QuoteServerSetup.TearDown();
    }

    private static void ResetL2QuoteLayers(PublishableLevel2PriceQuote level2PriceQuote)
    {
        level2PriceQuote.BidBook.ResetWithTracking();
        level2PriceQuote.OrderBook.IsBidBookChanged = true;
        level2PriceQuote.AskBook.ResetWithTracking();
        level2PriceQuote.OrderBook.IsAskBookChanged = true;

        level2PriceQuote.SingleTickValue = 0m;
    }
}
