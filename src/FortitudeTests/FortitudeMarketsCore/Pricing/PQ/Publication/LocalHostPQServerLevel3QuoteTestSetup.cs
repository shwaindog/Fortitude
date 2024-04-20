#region

using FortitudeCommon.DataStructures.Maps.IdMap;
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

    [TestInitialize]
    public void SetupPQServer()
    {
        CreatePQPublisher();
    }

    [TestCleanup]
    public void TearDown()
    {
        PqServer?.Dispose();
    }

    public PQPublisher<PQLevel3Quote> CreatePQPublisher()
    {
        LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize;
        LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                          LastTradedFlags.LastTradedTime;
        InitializeServerPrereqs();
        PqServer = new PQServer<PQLevel3Quote>(MarketConnectionConfig, HeartBeatSender, ServerDispatcherResolver,
            PqSnapshotFactory, PqUpdateFactory);
        PqPublisher = new PQPublisher<PQLevel3Quote>(PqServer);
        PqPublisher.RegisterTickersWithServer(MarketConnectionConfig);
        Logger.Info("Started PQServer");
        Level3PriceQuote = GenerateL3QuoteWithTraderLayerAndLastTrade();
        return PqPublisher;
    }

    public Level3PriceQuote GenerateL3QuoteWithTraderLayerAndLastTrade()
    {
        var sourceBidBook = GenerateBook(20, 1.1123m, -0.0001m, 100000m, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));
        var sourceAskBook = GenerateBook(20, 1.1125m, 0.0001m, 100000m, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));

        UpdateTraderQuoteBook(sourceBidBook, NameIdLookupGenerator, 20, 1, 10000, 1000);
        UpdateTraderQuoteBook(sourceAskBook, NameIdLookupGenerator, 20, 1, 20000, 500);
        var toggleBool = false;
        decimal growVolume = 10000;
        var traderNumber = 1;

        var recentlyTraded = GenerateRecentlyTraded(10, 1.1124m, 0.00005m, new DateTime(2015, 10, 18, 11, 33, 48),
            new TimeSpan(20 * TimeSpan.TicksPerMillisecond),
            (price, time) =>
                new LastTraderPaidGivenTrade(price, time, growVolume += growVolume, toggleBool = !toggleBool,
                    toggleBool = !toggleBool, "TraderName" + ++traderNumber), NameIdLookupGenerator);

        // setup source quote
        return new Level3PriceQuote(SourceTickerQuoteInfo,
            new DateTime(2015, 08, 06, 22, 07, 23).AddMilliseconds(123),
            false,
            1.234538m,
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
            true,
            recentlyTraded,
            1008,
            43749887,
            new DateTime(2017, 12, 29, 21, 0, 0));
    }


    public Level3PriceQuote ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(IPQLevel3Quote pQuote) => new(pQuote);

    private static OrderBook GenerateBook<T>(int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
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

        return new OrderBook(generatedLayers.Cast<IPriceVolumeLayer>().ToList());
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
