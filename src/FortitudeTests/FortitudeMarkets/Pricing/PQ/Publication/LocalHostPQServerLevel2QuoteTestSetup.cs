// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel2QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public Level2PriceQuote           Level2PriceQuote = null!;
    public PQPublisher<PQLevel2Quote> PqPublisher      = null!;
    public PQServer<PQLevel2Quote>    PqServer         = null!;

    [TestInitialize]
    public void SetupPQServer()
    {
        CreatePQPublisher();
        Level2PriceQuoteTests.GenerateL2QuoteWithSourceNameLayer(FirstTickerInfo);
    }

    public void InitializeLevel2QuoteConfig()
    {
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
        InitializeServerPrereqs();
    }

    public PQPublisher<PQLevel2Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        InitializeServerPrereqs();
        var useMarketConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQLevel2Quote>
            (useMarketConnectionConfig, HeartBeatSender, ServerDispatcherResolver, PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQLevel2Quote>(PqServer);
        PqPublisher.RegisterTickersWithServer(useMarketConnectionConfig);
        Logger.Info("Started PQServer");
        Level2PriceQuote = Level2PriceQuoteTests.GenerateL2QuoteWithSourceNameLayer(FirstTickerInfo);
        return PqPublisher;
    }

    [TestCleanup]
    public void TearDown()
    {
        PqServer.Dispose();
    }
}
