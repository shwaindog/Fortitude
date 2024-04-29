#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeCommon.Types;

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
        Level3PriceQuote = GenerateL3QuoteWithTraderLayerAndLastTrade();
        return PqPublisher;
    }

    public Level3PriceQuote GenerateL3QuoteWithTraderLayerAndLastTrade(int i = 0)
    {
        var priceDiff = i * 0.00015m;
        var volDiff = i * 5_000m;
        var sourceBidBook = GenerateBook(BookSide.BidBook, 20, 1.1123m + priceDiff, -0.0001m, 100000m + volDiff, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));
        var sourceAskBook = GenerateBook(BookSide.AskBook, 20, 1.1125m, 0.0001m, 100000m, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));


        var volStart = i * 1_000m;
        UpdateTraderQuoteBook(sourceBidBook, NameIdLookupGenerator, 20, 1, 10000 + volStart, 1000 + volDiff);
        UpdateTraderQuoteBook(sourceAskBook, NameIdLookupGenerator, 20, 1, 20000 + volStart, 500 + volDiff);
        var toggleBool = false;
        decimal growVolume = 10000;
        var traderNumber = 1;

        var recentlyTraded = GenerateRecentlyTraded(10, 1.1124m, 0.00005m + priceDiff
            , new DateTime(2015, 10, 18, 11, 33, 48) + TimeSpan.FromSeconds(i),
            new TimeSpan(20 + 1 * TimeSpan.TicksPerMillisecond),
            (price, time) =>
                new LastTraderPaidGivenTrade(price, time, growVolume += growVolume, toggleBool = !toggleBool,
                    toggleBool = !toggleBool, "TraderName" + ++traderNumber), NameIdLookupGenerator);

        // setup source quote
        return new Level3PriceQuote(SourceTickerQuoteInfo,
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123 + i),
            false,
            1.234538m + priceDiff,
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(234 + 1),
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(345 + 1),
            DateTime.Parse("2015-08-06 22:07:23.123"),
            new DateTime(2015, 08, 06, 22, 07, 22).AddMilliseconds(i),
            true,
            new DateTime(2015, 08, 06, 22, 07, 22).AddMilliseconds(i),
            false,
            true,
            new PeriodSummary(),
            sourceBidBook,
            true,
            sourceAskBook,
            true,
            recentlyTraded,
            1008 + (uint)i,
            43749887 + (uint)i,
            new DateTime(2017, 12, 29, 21, 0, 0).AddMilliseconds(i));
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


    private static RecentlyTraded GenerateRecentlyTraded<T>(int numberOfRecentlyTraded, decimal startingPrice,
        decimal deltaPrice,
        DateTime startingTime, TimeSpan deltaTime, Func<decimal, DateTime, T> generateLastTraded,
        INameIdLookupGenerator? nameIdLookupGen = null) where T : IMutableLastTrade
    {
        var currentPrice = startingPrice;
        var currentDateTime = startingTime;
        var lastTrades = new List<IMutableLastTrade>(numberOfRecentlyTraded);

        for (var i = 0; i < numberOfRecentlyTraded; i++)
        {
            var lastTrade = generateLastTraded(currentPrice, currentDateTime);
            if (lastTrade is ILastTraderPaidGivenTrade ltpgt)
                nameIdLookupGen?.GetOrAddId(ltpgt.TraderName);
            lastTrades.Add(lastTrade);
            currentPrice += deltaPrice;
            currentDateTime += deltaTime;
        }

        return new RecentlyTraded(lastTrades);
    }

    private static void UpdateTraderQuoteBook(IOrderBook toUpdate, INameIdLookupGenerator nameLookupGenerator,
        int numberOfLayers,
        int numberOfTradersPerLayer, decimal startingVolume, decimal deltaVolumePerLayer)
    {
        var currentVolume = startingVolume;
        for (var i = 0; i < numberOfLayers; i++)
        {
            var traderLayer = (IMutableTraderPriceVolumeLayer)toUpdate[i]!;
            for (var j = 0; j < numberOfTradersPerLayer; j++)
            {
                string? traderName = null;
                if (startingVolume != 0m && deltaVolumePerLayer != 0m)
                {
                    traderName = $"TraderLayer{i}_{j}";
                    nameLookupGenerator.GetOrAddId(traderName);
                }

                if (traderLayer.Count <= j)
                {
                    traderLayer.Add(traderName!, currentVolume + j * deltaVolumePerLayer);
                }
                else
                {
                    var traderLayerInfo = traderLayer[j]!;
                    traderLayerInfo.TraderName = traderName;
                    traderLayerInfo.TraderVolume = currentVolume + j * deltaVolumePerLayer;
                }
            }

            currentVolume += deltaVolumePerLayer;
        }
    }
}
