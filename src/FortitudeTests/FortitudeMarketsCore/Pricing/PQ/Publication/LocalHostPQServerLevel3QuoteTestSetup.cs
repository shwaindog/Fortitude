#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[NoMatchingProductionClass]
public class LocalHostPQServerLevel3QuoteTestSetup : LocalHostPQServerTestSetupBase
{
    public Level3PriceQuote Level3PriceQuote = null!;
    public PQPublisher<PQLevel3Quote> PqPublisher = null!;
    public PQServer<PQLevel3Quote> PqServer = null!;

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
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize;
        LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                          LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
    }

    public PQPublisher<PQLevel3Quote> CreatePQPublisher(IMarketConnectionConfig? overrideMarketConnectionConfig = null)
    {
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize;
        LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                          LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
        var useConnectionConfig = overrideMarketConnectionConfig ?? DefaultServerMarketConnectionConfig;
        PqServer = new PQServer<PQLevel3Quote>(useConnectionConfig, HeartBeatSender, ServerDispatcherResolver,
            PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQLevel3Quote>(PqServer);
        PqPublisher.RegisterTickersWithServer(useConnectionConfig);
        Logger.Info("Started PQServer");
        Level3PriceQuote = Level3PriceQuoteTests.GenerateL3QuoteWithTraderLayerAndLastTrade(FirstTickerQuoteInfo);
        return PqPublisher;
    }
}
