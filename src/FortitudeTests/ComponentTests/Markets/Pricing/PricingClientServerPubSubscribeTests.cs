#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;
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

    private ISocketDispatcherResolver clientDispatcherResolver = null!;

    private PQServerHeartBeatSender hbSender = null!;
    private IMarketConnectionConfig marketConnectionConfig = null!;
    private INameIdLookupGenerator nameIdLookupGenerator = null!;
    private OSNetworkingController networkingController = null!;
    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> pqSnapshotFactory = null!;
    private Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> pqUpdateFactory = null!;
    private IMarketConnectionConfigRepository pricingConnectionsConfigRepository = null!;
    private IPricingServerConfig pricingServerConfig = null!;
    private ISocketDispatcherResolver serverDispatcherResolver = null!;
    private IPQConversationRepository<IPQSnapshotClient> snapshotClientFactory = null!;
    private ISourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    private ISourceTickersConfig sourceTickersConfig = null!;
    private IPQConversationRepository<IPQUpdateClient> updateClientFactory = null!;

    public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        nameIdLookupGenerator = new NameIdLookupGenerator();
        sourceTickersConfig =
            new SourceTickersConfig(new TickerConfig(TickerId, TestTicker, TickerAvailability.AllEnabled, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails
                , 20, lastTradedFlags));
        sourceTickerQuoteInfo = sourceTickersConfig.GetSourceTickerInfo(ExchangeId, ExchangeName, TestTicker)!;
        pricingServerConfig = new PricingServerConfig(
            new NetworkTopicConnectionConfig("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor,
                new List<IEndpointConfig>
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.ServerSnapshotPort)
                }, "TestSnapshotServerDescription")
            , new NetworkTopicConnectionConfig("TestUpdateServer", SocketConversationProtocol.UdpPublisher,
                new List<IEndpointConfig>
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.ServerUpdatePort, subnetMask: TestMachineConfig.NetworkSubAddress)
                }, "TestUpdateServerDescription"
                , connectionAttributes: SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast));
        marketConnectionConfig = new MarketConnectionConfig(1, ExchangeName, MarketConnectionType.Pricing
            , sourceTickersConfig, pricingServerConfig);
        pricingConnectionsConfigRepository =
            new MarketConnectionConfigRepository(marketConnectionConfig);
        networkingController = new OSNetworkingController();
        hbSender = new PQServerHeartBeatSender();
        serverDispatcherResolver = new SimpleSocketDispatcherResolver(new SocketDispatcher(
            new SimpleSocketRingPollerListener("PQServer", 100
                , new SocketSelector(100, networkingController)),
            new SimpleSocketRingPollerSender("PQServer", 100)));
        clientDispatcherResolver = new SimpleSocketDispatcherResolver(new SocketDispatcher(
            new SimpleSocketRingPollerListener("PQClient", 100
                , new SocketSelector(100, networkingController)),
            new SimpleSocketRingPollerSender("PQClient", 100)));

        pqSnapshotFactory = PQSnapshotServer.BuildTcpResponder;
        pqUpdateFactory = PQUpdatePublisher.BuildUdpMulticastPublisher;
        snapshotClientFactory = new PQSnapshotClientRepository(clientDispatcherResolver);
        updateClientFactory = new PQUpdateClientRepository(clientDispatcherResolver);
    }

    [TestCleanup]
    public void TearDown()
    {
        FLoggerFactory.GracefullyTerminateProcessLogging();
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields()
    {
        logger.Info("Starting Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields");
        Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);
        var autoResetEvent = new AutoResetEvent(false);
        var pqServer = new PQServer<PQLevel2Quote>(marketConnectionConfig, hbSender, serverDispatcherResolver
            ,
            pqSnapshotFactory, pqUpdateFactory);
        var pqPublisher = new PQPublisher<PQLevel2Quote>(pqServer);
        pqPublisher.RegisterTickersWithServer(marketConnectionConfig);
        var sourcePriceQuote = GenerateL2QuoteWithSourceNameLayer();

        logger.Info("Started PQServer");
        // logger.Info("About to publish first quote {0}" sourcePriceQuote);
        pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

        // setup listener after publish means first message will be missed and snapshot will be required.
        ILevel2Quote? alwaysUpdatedQuote = null;
        var pqClient = new PQClient(pricingConnectionsConfigRepository.ToggleProtocolDirection()
            , SingletonSocketDispatcherResolver.Instance, updateClientFactory, snapshotClientFactory);
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == sourceTickerQuoteInfo.Source &&
                                  stqi.Ticker == sourceTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel2Quote>(sourceTickerQuoteInfo, 0);
        streamSubscription!.Subscribe(
            pQuote =>
            {
                logger.Info("Client Received pQuote {0}", pQuote);
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
        pqServer.Dispose();
        pqClient.Dispose();
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
        var clientMarketConnectionConfigRepository = pricingConnectionsConfigRepository.ToggleProtocolDirection();
        var clientConnectionConfig = clientMarketConnectionConfigRepository.Find(ExchangeName);
        clientConnectionConfig!.SourceTickerConfig = null;

        var pqClient = new PQClient(clientMarketConnectionConfigRepository
            , SingletonSocketDispatcherResolver.Instance, updateClientFactory, snapshotClientFactory);

        var pqServer = new PQServer<PQLevel3Quote>(marketConnectionConfig, hbSender, serverDispatcherResolver
            ,
            pqSnapshotFactory, pqUpdateFactory);
        var pqPublisher = new PQPublisher<PQLevel3Quote>(pqServer);
        pqPublisher.RegisterTickersWithServer(marketConnectionConfig);
        var availableSourceTickers = pqClient.RequestSourceTickerForSource(ExchangeName);
        availableSourceTickers.UpdatedSourceTickerInfos += infos =>
        {
            logger.Info("Client Received SourceTickerQuoteInfos [{0}]", string.Join(", ", infos));
            if (infos.Any(stqi => stqi.Source == sourceTickerQuoteInfo.Source &&
                                  stqi.Ticker == sourceTickerQuoteInfo.Ticker)) autoResetEvent.Set();
        };
        autoResetEvent.WaitOne(3_000);
        var streamSubscription = pqClient.GetQuoteStream<PQLevel3Quote>(sourceTickerQuoteInfo, 0);

        streamSubscription!.Subscribe(
            pQuote =>
            {
                alwaysUpdatedQuote = ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(pQuote);
                if (pQuote.PQSequenceId > 0) autoResetEvent.Set();
            });
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
        pqServer.Dispose();
        pqClient.Dispose();
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

    private static Level3PriceQuote ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(IPQLevel3Quote pQuote) => new(pQuote);

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
