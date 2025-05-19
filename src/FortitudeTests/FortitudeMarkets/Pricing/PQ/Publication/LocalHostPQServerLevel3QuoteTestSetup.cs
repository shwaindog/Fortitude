// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel3QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public PublishableLevel3PriceQuote           Level3PriceQuote = null!;
    public PQPublisher<PQPublishableLevel3Quote> PqPublisher      = null!;
    public PQServer<PQPublishableLevel3Quote>    PqServer         = null!;

    public string Ticker = TestTicker;

    [TestInitialize]
    public void SetupPQServer()
    {
        CreatePQPublisher();
    }

    [TestCleanup]
    public void TearDown()
    {
        PqServer?.Dispose();
        Logger.Info("LocalHostPQServerLevel3QuoteTestSetup PqServer closed");
    }

    public void InitializeLevel3QuoteConfig()
    {
        LayerDetails = LayerFlagsExtensions.FullCounterPartyOrdersFlags;
        LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
    }

    public PQPublisher<PQPublishableLevel3Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize;
        LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
        var useConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQPublishableLevel3Quote>
            (useConnectionConfig, HeartBeatSender, ServerDispatcherResolver, PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQPublishableLevel3Quote>(PqServer);
        PqPublisher.RegisterStreamWithServer(useConnectionConfig);
        Logger.Info("Started PQServer");
        Level3PriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(FirstTickerInfo);
        return PqPublisher;
    }
}
