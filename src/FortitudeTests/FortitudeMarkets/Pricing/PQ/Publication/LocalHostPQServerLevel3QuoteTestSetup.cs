// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel3QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public Level3PriceQuote           Level3PriceQuote = null!;
    public PQPublisher<PQLevel3Quote> PqPublisher      = null!;
    public PQServer<PQLevel3Quote>    PqServer         = null!;

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
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize;
        LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
    }

    public PQPublisher<PQLevel3Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize;
        LastTradedFlags =
            LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven | LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
        var useConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQLevel3Quote>
            (useConnectionConfig, HeartBeatSender, ServerDispatcherResolver, PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQLevel3Quote>(PqServer);
        PqPublisher.RegisterTickersWithServer(useConnectionConfig);
        Logger.Info("Started PQServer");
        Level3PriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(FirstTickerInfo);
        return PqPublisher;
    }
}
