// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Converters;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

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
        ILevel2Quote? alwaysUpdatedQuote = null;

        var pqClient               = pqClientSetup.CreatePQClient(pricingClientConfig);
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == pqServerL2QuoteServerSetup.FirstTickerQuoteInfo.Source &&
                                  stqi.Ticker == pqServerL2QuoteServerSetup.FirstTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel2Quote>(pqServerL2QuoteServerSetup.FirstTickerQuoteInfo, 0);
        var updateNonEmpty     = true;
        streamSubscription!.Subscribe(
                                      pQuote =>
                                      {
                                          // Logger.Info("Client Received pQuote {0}", pQuote);
                                          if (updateNonEmpty || pQuote.SinglePrice == 0)
                                              alwaysUpdatedQuote = pQuote.ToL2PriceQuote();
                                          else
                                              Logger.Info("Skipping non-empty");
                                          if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
                                      });
        Logger.Info("Started PQClient and subscribed");

        pqPublisher.PublishQuoteUpdateAs(sourcePriceQuote, PQMessageFlags.CompleteUpdate);

        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SinglePrice == 0m) &&
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
        FLoggerFactory.WaitUntilDrained();
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        updateNonEmpty = false;
        ResetL2QuoteLayers(sourcePriceQuote);

        NonPublicInvocator.SetAutoPropertyInstanceField(sourcePriceQuote,
                                                        (Level2PriceQuote pq) => pq.AdapterSentTime, new DateTime(2015, 08, 15, 11, 36, 13));
        // adapter becomes sourceTime on Send
        Logger.Info("About to publish second empty quote. {0}", sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        alwaysUpdatedQuote = null;
        count              = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote!.SinglePrice != 0m) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0)
                Logger.Info("Awaiting first empty quote as alwaysUpdateQuote = {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(50);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Logger.Info("Second diff.");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
        Logger.Info("Finished Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Logger.Info("Test complete start shutdown");
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
        var pqServerL3QuoteServerSetup = new LocalHostPQServerLevel3QuoteTestSetup();
        pqServerL3QuoteServerSetup.InitializeLevel3QuoteConfig();

        var pricingServerConfig = pqServerL3QuoteServerSetup.DefaultServerMarketsConfig.ShiftPortsBy(2);
        var pricingClientConfig = pricingServerConfig.ToggleProtocolDirection("PQClient_Level3");
        var autoResetEvent      = new AutoResetEvent(false);

        ILevel3Quote? alwaysUpdatedQuote = null;

        var clientConnectionConfig = pricingClientConfig.Find(LocalHostPQTestSetupCommon.ExchangeName);

        clientConnectionConfig!.SourceTickerConfig = null;

        var pqClient = pqClientSetup.CreatePQClient(pricingClientConfig);


        var pqPublisher            = pqServerL3QuoteServerSetup.CreatePQPublisher(pricingServerConfig.Find(LocalHostPQTestSetupCommon.ExchangeName));
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(LocalHostPQTestSetupCommon.ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            Logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == pqServerL3QuoteServerSetup.FirstTickerQuoteInfo.Source &&
                                  stqi.Ticker == pqServerL3QuoteServerSetup.FirstTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };

        var streamSubscription = pqClient.GetQuoteStream<PQLevel3Quote>(pqServerL3QuoteServerSetup.FirstTickerQuoteInfo, 0);
        var updateNonEmpty     = true;
        streamSubscription!.Subscribe(
                                      pQuote =>
                                      {
                                          Logger.Info("Client Received pQuote {0}", pQuote);
                                          if (updateNonEmpty || pQuote.SinglePrice == 0)
                                              alwaysUpdatedQuote = pQuote.ToL3PriceQuote();
                                          else
                                              Logger.Info("Skipping non-empty");
                                          if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
                                      });
        var sourcePriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(pqServerL3QuoteServerSetup.FirstTickerQuoteInfo);
        pqPublisher.PublishQuoteUpdateAs(sourcePriceQuote, PQMessageFlags.CompleteUpdate);
        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SinglePrice == 0m) &&
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
        alwaysUpdatedQuote = null;
        // adapter becomes sourceTime on Send
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(3_000); // 20 ms seems to work expand wait time if errors
        count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote!.SinglePrice != 0m) &&
               count++ < 200) // depending on pub subscribe first quote may be empty
        {
            if (count % 20 == 0)
                Logger.Info("Awaiting first empty quote as alwaysUpdateQuote = {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(50);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);

        Logger.Info("Second diff");
        Logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        Logger.Info("Test complete start shutdown");
        pqServerL3QuoteServerSetup.TearDown();
    }

    private static void ResetL2QuoteLayers(Level2PriceQuote level2PriceQuote)
    {
        ((OrderBook)level2PriceQuote.BidBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsBidBookChanged = true;
        ((OrderBook)level2PriceQuote.AskBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsAskBookChanged = true;
        level2PriceQuote.SinglePrice                             = 0m;
    }
}
