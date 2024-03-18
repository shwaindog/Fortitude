#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.ComponentTests.Markets.Pricing;

[TestClass]
[NoMatchingProductionClass]
public class PricingClientServerPubSubscribeTests
{
    private const string ExchangeName = "ComponentTestExchange";
    private const ushort ExchangeId = 1;
    private const string TestTicker = "EUR/USD";
    private const ushort TickerId = 1;

    private static readonly IFLogger logger =
        FLoggerFactory.Instance.GetLogger(typeof(PricingClientServerPubSubscribeTests));

    private PQServerHeartBeatSender hbSender = null!;
    private INameIdLookupGenerator nameIdLookupGenerator = null!;
    private OSNetworkingController networkingController = null!;
    private Func<ISocketConnectionConfig, PQSnapshotServer> pqSnapshotFactory = null!;
    private Func<ISocketDispatcher, IConnectionConfig, string, string, IPQUpdateServer> pqUpdateFactory = null!;
    private PricingServersConfigRepository pricingServersConfigRepository = null!;
    private IPQSocketSubscriptionRegristrationFactory<IPQSnapshotClient> snapshotClientFactory = null!;
    private SnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig = null!;
    private ISocketDispatcher socketDispatcher = null!;
    private Func<string, ISocketDispatcher> socketDispatcherFactory = null!;
    private SourceTickerPublicationConfig sourceTickerPublicationConfig = null!;
    private SourceTickerPublicationConfigRepository sourceTickerPublicationConfigs = null!;
    private SourceTickerQuoteInfo sourceTickerQuoteInfo = null!;
    private IPQSocketSubscriptionRegristrationFactory<IPQUpdateClient> updateClientFactory = null!;

    public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        nameIdLookupGenerator = new NameIdLookupGenerator();
        sourceTickerPublicationConfig =
            new SourceTickerPublicationConfig(
                UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(ExchangeId, TickerId),
                ExchangeName, TestTicker, 20, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails, lastTradedFlags);
        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(sourceTickerPublicationConfig);
        sourceTickerPublicationConfigs
            = new SourceTickerPublicationConfigRepository(new[] { sourceTickerPublicationConfig });
        snapshotUpdatePricingServerConfig = new SnapshotUpdatePricingServerConfig(ExchangeName,
            MarketServerType.MarketData,
            new[]
            {
                new ConnectionConfig("TestSnapshotServer", TestMachineConfig.LoopBackIpAddress,
                    TestMachineConfig.ServerSnapshotPort,
                    ConnectionDirectionType.Both, "none", 500)
                , new ConnectionConfig("TestUpdateServer", TestMachineConfig.LoopBackIpAddress,
                    TestMachineConfig.ServerUpdatePort, ConnectionDirectionType.Publisher,
                    TestMachineConfig.NetworkSubAddress, 500)
            }, null, 9000, sourceTickerPublicationConfigs, false, false);
        pricingServersConfigRepository =
            new PricingServersConfigRepository(new[] { snapshotUpdatePricingServerConfig });
        networkingController = new OSNetworkingController();
        hbSender = new PQServerHeartBeatSender();
        socketDispatcherFactory = (dispatcherDescription) =>
        {
            var socketSelector = new SocketSelector(1000, networkingController);
            var socketDispatcherListener = new SocketDispatcherListener(socketSelector, dispatcherDescription);
            var socketDispatcherSender = new SocketDispatcherSender("socketDispatcherSender");
            return new SocketDispatcher(socketDispatcherListener, socketDispatcherSender);
        };
        socketDispatcher = new SocketDispatcher(
            new SocketDispatcherListener(new SocketSelector(1000, networkingController), "socketDispatcher"),
            new SocketDispatcherSender("socketDispatcherSender"));

        pqSnapshotFactory = PQSnapshotServer.BuildTcpResponder;
        pqUpdateFactory = (dispatcher, connConfig, socketUseDescription, networkSubAddress) =>
            new PQUpdatePublisher(dispatcher, networkingController, connConfig,
                socketUseDescription, networkSubAddress);
        snapshotClientFactory = new PQSnapshotClientRegistrationFactory(networkingController);
        updateClientFactory = new PQUpdateClientRegistrationFactory(networkingController);
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields()
    {
        logger.Info("Starting Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);
        var autoResetEvent = new AutoResetEvent(false);
        var pqServer = new PQServer<PQLevel2Quote>(snapshotUpdatePricingServerConfig, hbSender, socketDispatcher,
            pqSnapshotFactory, pqUpdateFactory);
        var pqPublisher = new PQPublisher<PQLevel2Quote>(pqServer);
        pqPublisher.RegisterTickersWithServer(sourceTickerPublicationConfigs);
        var sourcePriceQuote = GenerateL2QuoteWithSourceNameLayer();

        logger.Info("Started PQServer");
        // logger.Info("About to publish first quote {0}", sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        // setup listener after publish means first message will be missed and snapshot will be required.
        ILevel2Quote? alwaysUpdatedQuote = null;
        var pqClient = new PQClient(pricingServersConfigRepository, snapshotClientFactory, updateClientFactory,
            socketDispatcherFactory);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel2Quote>(sourceTickerPublicationConfig, 0);
        streamSubscription!.Subscribe(
            pQuote =>
            {
                // logger.Info("Client Received pQuote {0}", pQuote);
                alwaysUpdatedQuote = ConvertPQToLevel2QuoteWithSourceNameLayer(pQuote);
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
        logger.Info("Started PQClient and subscribed");

        var count = 0;
        while ((alwaysUpdatedQuote == null || alwaysUpdatedQuote.SinglePrice == 0m) &&
               count++ < 10) // depending on pub subscribe first quote may be empty
        {
            logger.Info("Awaiting first non-empty quote as alwaysUpdateQuote is {0}.", alwaysUpdatedQuote);
            autoResetEvent.WaitOne(3_000);
        }

        Assert.IsNotNull(alwaysUpdatedQuote);
        logger.Info("Received first update {0}", alwaysUpdatedQuote);
        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        logger.Info("First diff.");
        logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        ResetL2QuoteLayers(sourcePriceQuote);

        NonPublicInvocator.SetAutoPropertyInstanceField(sourcePriceQuote,
            (Level2PriceQuote pq) => pq.AdapterSentTime, new DateTime(2015, 08, 15, 11, 36, 13));
        // adapter becomes sourceTime on Send
        logger.Info("About to publish second empty quote. {0}", sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(60000); // 20 ms seems to work expand wait time if errors
        logger.Info("Received second update {0}", alwaysUpdatedQuote);
        pqClient.Dispose();
        pqPublisher.Dispose();
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        logger.Info("Second diff.");
        logger.Info(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Thread.Sleep(5_000);
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
        logger.Info("Finished Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        //FLoggerFactory.GracefullyTerminateProcessLogging();
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
        var autoResetEvent = new AutoResetEvent(false);
        ILevel3Quote? alwaysUpdatedQuote = null;
        var pqClient = new PQClient(pricingServersConfigRepository, snapshotClientFactory, updateClientFactory,
            socketDispatcherFactory);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel3Quote>(sourceTickerPublicationConfig, 0);

        streamSubscription!.Subscribe(
            pQuote =>
            {
                alwaysUpdatedQuote = ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(pQuote);
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });

        var pqServer = new PQServer<PQLevel3Quote>(snapshotUpdatePricingServerConfig, hbSender, socketDispatcher,
            pqSnapshotFactory, pqUpdateFactory);
        var pqPublisher = new PQPublisher<PQLevel3Quote>(pqServer);
        pqPublisher.RegisterTickersWithServer(sourceTickerPublicationConfigs);
        var sourcePriceQuote = GenerateL3QuoteWithTraderLayerAndLastTrade();
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        autoResetEvent.WaitOne(2_000); // 30 ms seems to work expand wait time if errors 
        var destinationSnapshot = alwaysUpdatedQuote!.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

        ResetL2QuoteLayers(sourcePriceQuote);

        SetExpectedDiffFieldsToSame(sourcePriceQuote, destinationSnapshot);
        // adapter becomes sourceTime on Send
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
        autoResetEvent.WaitOne(2_000); // 20 ms seems to work expand wait time if errors
        pqClient.Dispose();
        pqPublisher.Dispose();
        destinationSnapshot = alwaysUpdatedQuote.Clone();
        SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
        Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
        Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
        //FLoggerFactory.GracefullyTerminateProcessLogging();
    }

    private static void ResetL2QuoteLayers(ILevel2Quote level2PriceQuote)
    {
        ((OrderBook)level2PriceQuote.BidBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsBidBookChanged = true;
        ((OrderBook)level2PriceQuote.AskBook).StateReset();
        ((IMutableLevel2Quote)level2PriceQuote).IsAskBookChanged = true;
    }

    private Level3PriceQuote GenerateL3QuoteWithTraderLayerAndLastTrade()
    {
        var sourceBidBook = GenerateBook(20, 1.1123m, -0.0001m, 100000m, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));
        var sourceAskBook = GenerateBook(20, 1.1125m, 0.0001m, 100000m, 10000m,
            (price, volume) => new TraderPriceVolumeLayer(price, volume));

        UpdateTraderQuoteBook(sourceBidBook, nameIdLookupGenerator, 20, 1, 10000, 1000);
        UpdateTraderQuoteBook(sourceAskBook, nameIdLookupGenerator, 20, 1, 20000, 500);
        var toggleBool = false;
        decimal growVolume = 10000;
        var traderNumber = 1;

        var recentlyTraded = GenerateRecentlyTraded(10, 1.1124m, 0.00005m, new DateTime(2015, 10, 18, 11, 33, 48),
            new TimeSpan(20 * TimeSpan.TicksPerMillisecond),
            (price, time) =>
                new LastTraderPaidGivenTrade(price, time, growVolume += growVolume, toggleBool = !toggleBool,
                    toggleBool = !toggleBool, "TraderName" + ++traderNumber), nameIdLookupGenerator);

        // setup source quote
        return new Level3PriceQuote(sourceTickerQuoteInfo,
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

    private Level2PriceQuote GenerateL2QuoteWithSourceNameLayer()
    {
        var i = 0;
        var sourceBidBook = GenerateBook(20, 1.1123m, -0.0001m, 100000m, 10000m,
            (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));
        var sourceAskBook = GenerateBook(20, 1.1125m, 0.0001m, 100000m, 10000m,
            (price, volume) => new SourcePriceVolumeLayer(price, volume, "SourceName" + i++, true));

        UpdateSourceQuoteBook(sourceBidBook, nameIdLookupGenerator, 20, 20, 1);
        UpdateSourceQuoteBook(sourceAskBook, nameIdLookupGenerator, 20, 20, 1);

        // setup source quote
        return new Level2PriceQuote(sourceTickerQuoteInfo,
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

    private static Level3PriceQuote ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(IPQLevel3Quote pQuote) =>
        new(pQuote);

    private static Level2PriceQuote ConvertPQToLevel2QuoteWithSourceNameLayer(IPQLevel2Quote pQuote) => new(pQuote);

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
