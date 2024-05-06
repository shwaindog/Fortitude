#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel2QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public Level2PriceQuote Level2PriceQuote = null!;
    public PQPublisher<PQLevel2Quote> PqPublisher = null!;
    public PQServer<PQLevel2Quote> PqServer = null!;

    [TestInitialize]
    public void SetupPQServer()
    {
        CreatePQPublisher();
        Level2PriceQuoteTests.GenerateL2QuoteWithSourceNameLayer(FirstTickerQuoteInfo);
    }

    public PQPublisher<PQLevel2Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        InitializeServerPrereqs();
        var useMarketConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQLevel2Quote>(useMarketConnectionConfig, HeartBeatSender, ServerDispatcherResolver,
            PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQLevel2Quote>(PqServer);
        PqPublisher.RegisterTickersWithServer(useMarketConnectionConfig);
        Logger.Info("Started PQServer");
        Level2PriceQuote = Level2PriceQuoteTests.GenerateL2QuoteWithSourceNameLayer(FirstTickerQuoteInfo);
        return PqPublisher;
    }

    [TestCleanup]
    public void TearDown()
    {
        PqServer.Dispose();
    }
}
