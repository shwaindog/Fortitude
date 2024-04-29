#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeCommon.Types;

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
        GenerateL2QuoteWithSourceNameLayer();
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
        Level2PriceQuote = GenerateL2QuoteWithSourceNameLayer();
        return PqPublisher;
    }

    [TestCleanup]
    public void TearDown()
    {
        PqServer.Dispose();
    }

    public Level2PriceQuote GenerateL2QuoteWithSourceNameLayer()
    {
        var i = 0;
        var sourceBidBook = GenerateBook(BookSide.BidBook, 20, 1.1123m, -0.0001m, 100000m, 10000m,
            (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));
        var sourceAskBook = GenerateBook(BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m,
            (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));

        UpdateSourceQuoteBook(sourceBidBook, NameIdLookupGenerator, 20, 20, 1);
        UpdateSourceQuoteBook(sourceAskBook, NameIdLookupGenerator, 20, 20, 1);

        // setup source quote
        return new Level2PriceQuote(SourceTickerQuoteInfo,
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123),
            false, 1.234538m,
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234),
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345),
            DateTime.Parse("2015-08-06 22:07:23.123"),
            new DateTime(2015, 08, 06, 22, 07, 22),
            true,
            new DateTime(2015, 08, 06, 22, 07, 22),
            false,
            true,
            new PeriodSummary(),
            sourceBidBook,
            true,
            sourceAskBook,
            true);
    }

    private static OrderBook GenerateBook<T>(BookSide bookSide, int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
        decimal startingVolume, decimal deltaVolumePerLayer, Func<decimal, decimal, T> genNewLayerObj)
        where T : IPriceVolumeLayer
    {
        var generatedLayers = new List<T>();
        var currentPrice = startingPrice;
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            generatedLayers.Add(genNewLayerObj(currentPrice, currentVolume));
            currentPrice += deltaPricePerLayer;
            currentVolume += deltaVolumePerLayer;
        }

        return new OrderBook(bookSide, generatedLayers.Cast<IPriceVolumeLayer>().ToList());
    }


    private static void UpdateSourceQuoteBook(IOrderBook toUpdate, INameIdLookupGenerator nameLookupGenerator,
        int numberOfLayers,
        decimal startingVolume, decimal deltaVolumePerLayer)
    {
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            var sourceLayer = (IMutableSourcePriceVolumeLayer)toUpdate[i]!;

            string? traderName = null;
            if (startingVolume != 0m && deltaVolumePerLayer != 0m)
            {
                traderName = $"SourceNameUpdate{i}";
                nameLookupGenerator.GetOrAddId(traderName);
            }

            sourceLayer.SourceName = traderName;
            sourceLayer.Volume = currentVolume + i * deltaVolumePerLayer;
        }
    }
}
