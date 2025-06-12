// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel2QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public PublishableLevel2PriceQuote           Level2PriceQuote = null!;
    public PQPublisher<PQPublishableLevel2Quote> PqPublisher      = null!;
    public PQServer<PQPublishableLevel2Quote>    PqServer         = null!;

    [TestInitialize]
    public void SetupPQServer()
    {
        CreatePQPublisher();
        Level2PriceQuoteTests.GenerateL2QuoteWithSourceNameLayer(FirstTickerInfo);
    }

    public void InitializeLevel2QuoteConfig()
    {
        InitializeServerPrereqs();
    }

    public PQPublisher<PQPublishableLevel2Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        InitializeServerPrereqs();
        var useMarketConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQPublishableLevel2Quote>
            (useMarketConnectionConfig, HeartBeatSender, ServerDispatcherResolver, PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQPublishableLevel2Quote>(PqServer);
        PqPublisher.RegisterStreamWithServer(useMarketConnectionConfig);
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
