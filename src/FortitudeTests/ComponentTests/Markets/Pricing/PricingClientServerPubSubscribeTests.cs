using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.ComponentTests.Markets.Pricing
{
    [TestClass, NoMatchingProductionClass]
    public class PricingClientServerPubSubscribeTests
    {
        private const string ExchangeName = "ComponentTestExchange";
        private const ushort ExchangeId = 1;
        private const string TestTicker = "EUR/USD";
        private const ushort TickerId = 1;
        private PQServerHeartBeatSender hbSender;
        private INameIdLookupGenerator nameIdLookupGenerator;
        private Func<ISocketDispatcher, IConnectionConfig, string, IPQSnapshotServer> pqSnapshotFactory;
        private Func<ISocketDispatcher, IConnectionConfig, string, string, IPQUpdateServer> pqUpdateFactory;
        private PricingServersConfigRepository pricingServersConfigRepository;
        private SnapshotUpdatePricingServerConfig snapshotUpdatePricingServerConfig;
        private ISocketDispatcher socketDispatcher;
        private SourceTickerPublicationConfigRepository sourceTickerPublicationConfigs;
        private SourceTickerPublicationConfig sourceTickerPublicationConfig;
        private SourceTickerQuoteInfo sourceTickerQuoteInfo;
        private OSNetworkingController networkingController;
        private Func<string, ISocketDispatcher> socketDispatcherFactory;
        private IPQSocketSubscriptionRegristrationFactory<IPQSnapshotClient> snapshotClientFactory;
        private IPQSocketSubscriptionRegristrationFactory<IPQUpdateClient> updateClientFactory;

        public void Setup(LayerFlags layerDetails, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
        {
            nameIdLookupGenerator = new NameIdLookupGenerator();
            sourceTickerPublicationConfig =
                new SourceTickerPublicationConfig(
                    UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(ExchangeId, TickerId),
                    ExchangeName, TestTicker, 20, 0.00001m, 0.1m, 100, 0.1m, 250, layerDetails, lastTradedFlags);
            sourceTickerQuoteInfo = new SourceTickerQuoteInfo(sourceTickerPublicationConfig);
            sourceTickerPublicationConfigs = new SourceTickerPublicationConfigRepository(new[] {sourceTickerPublicationConfig});
            snapshotUpdatePricingServerConfig = new SnapshotUpdatePricingServerConfig(ExchangeName,
                MarketServerType.MarketData,
                new[]
                {
                    new ConnectionConfig("TestSnapshotServer", TestMachineConfig.LoopBackIpAddress,
                        TestMachineConfig.ServerSnapshotPort,
                        ConnectionDirectionType.Both, "none", 500),
                    new ConnectionConfig("TestUpdateServer", TestMachineConfig.LoopBackIpAddress,
                        TestMachineConfig.ServerUpdatePort, ConnectionDirectionType.Publisher,
                        TestMachineConfig.NetworkSubAddress, 500)
                }, null, 9000, sourceTickerPublicationConfigs, false, false);
            pricingServersConfigRepository =
                new PricingServersConfigRepository(new[] {snapshotUpdatePricingServerConfig});
            networkingController = new OSNetworkingController();
            hbSender = new PQServerHeartBeatSender();
            socketDispatcherFactory = (dispatcherDescription) =>
            {
                var socketSelector = new SocketSelector(1000, networkingController);
                var socketDispatcherListener = new SocketDispatcherListener(socketSelector, dispatcherDescription);
                var socketDispatcherSender = new SocketDispatcherSender();
                return new SocketDispatcher(socketDispatcherListener, socketDispatcherSender);
            };
            socketDispatcher = new SocketDispatcher(
                new SocketDispatcherListener(new SocketSelector(1000, networkingController)), 
                new SocketDispatcherSender());
            pqSnapshotFactory = (dispatcher, connConfig, socketUseDescription) =>
                    new PQSnapshotServer(dispatcher, networkingController, 
                    connConfig, socketUseDescription);
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
            Setup(LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName);
            var autoResetEvent = new AutoResetEvent(false);
            var pqServer = new PQServer<PQLevel2Quote>(snapshotUpdatePricingServerConfig, hbSender, socketDispatcher,
                pqSnapshotFactory, pqUpdateFactory);
            var pqPublisher = new PQPublisher<PQLevel2Quote>(pqServer);
            pqPublisher.RegisterTickersWithServer(sourceTickerPublicationConfigs);
            var sourcePriceQuote = GenerateL2QuoteWithSourceNameLayer();
            pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

            // setup listener after publish means first message will be missed and snapshot will be required.
            ILevel2Quote alwaysUpdatedQuote = null;
            var pqClient = new PQClient(pricingServersConfigRepository, snapshotClientFactory, updateClientFactory, 
                socketDispatcherFactory);
            var streamSubscription = pqClient.GetQuoteStream<IPQLevel2Quote>(sourceTickerPublicationConfig, 0);
            streamSubscription.Subscribe(
                pQuote =>
                {
                    alwaysUpdatedQuote = ConvertPQToLevel2QuoteWithSourceNameLayer(pQuote);
                    if (pQuote.PQSequenceId > 0)
                    {
                        autoResetEvent.Set();
                    }
                });
            autoResetEvent.WaitOne(3000);
            var destinationSnapshot = alwaysUpdatedQuote.Clone();
            SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
            Console.Out.WriteLine("First diff.");
            Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
            Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

            ResetL2QuoteLayers(sourcePriceQuote);

            NonPublicInvocator.SetAutoPropertyInstanceField(sourcePriceQuote,
                (Level2PriceQuote pq) => pq.AdapterSentTime, new DateTime(2015, 08, 15, 11, 36, 13));
            // adapter becomes sourceTime on Send
            pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
            autoResetEvent.WaitOne(20); // 20 ms seems to work expand wait time if errors
            pqClient.Dispose();
            pqPublisher.Dispose();
            destinationSnapshot = alwaysUpdatedQuote.Clone();
            SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
            Console.Out.WriteLine("Second diff.");
            Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
            Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
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
            ILevel3Quote alwaysUpdatedQuote = null;
            var pqClient = new PQClient(pricingServersConfigRepository, snapshotClientFactory, updateClientFactory,
                socketDispatcherFactory);
            var streamSubscription = pqClient.GetQuoteStream<IPQLevel3Quote>(sourceTickerPublicationConfig, 0);
            
            streamSubscription.Subscribe(
                pQuote =>
                {
                    alwaysUpdatedQuote = ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(pQuote);
                    if (pQuote.PQSequenceId > 0)
                    {
                        autoResetEvent.Set();
                    }
                });

            var pqServer = new PQServer<PQLevel3Quote>(snapshotUpdatePricingServerConfig, hbSender, socketDispatcher,
                pqSnapshotFactory, pqUpdateFactory);
            var pqPublisher = new PQPublisher<PQLevel3Quote>(pqServer);
            pqPublisher.RegisterTickersWithServer(sourceTickerPublicationConfigs);
            var sourcePriceQuote = GenerateL3QuoteWithTraderLayerAndLastTrade();
            pqPublisher.PublishQuoteUpdate(sourcePriceQuote);

            autoResetEvent.WaitOne(50); // 30 ms seems to work expand wait time if errors 
            var destinationSnapshot = alwaysUpdatedQuote.Clone();
            SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
            Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
            Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));

            ResetL2QuoteLayers(sourcePriceQuote);

            SetExpectedDiffFieldsToSame(sourcePriceQuote, destinationSnapshot);
            // adapter becomes sourceTime on Send
            pqPublisher.PublishQuoteUpdate(sourcePriceQuote);
            autoResetEvent.WaitOne(20); // 20 ms seems to work expand wait time if errors
            pqClient.Dispose();
            pqPublisher.Dispose();
            destinationSnapshot = alwaysUpdatedQuote.Clone();
            SetExpectedDiffFieldsToSame(destinationSnapshot, sourcePriceQuote);
            Console.Out.WriteLine(sourcePriceQuote.DiffQuotes(destinationSnapshot));
            Assert.IsTrue(sourcePriceQuote.AreEquivalent(destinationSnapshot));
            //FLoggerFactory.GracefullyTerminateProcessLogging();
        }

        private void ResetL2QuoteLayers(ILevel2Quote level2PriceQuote)
        {
            ((OrderBook) level2PriceQuote.BidBook).Reset();
            ((IMutableLevel2Quote)level2PriceQuote).IsBidBookChanged = true;
            ((OrderBook) level2PriceQuote.AskBook).Reset();
            ((IMutableLevel2Quote)level2PriceQuote).IsAskBookChanged = true;
        }

        private ILevel3Quote GenerateL3QuoteWithTraderLayerAndLastTrade()
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

        private ILevel2Quote GenerateL2QuoteWithSourceNameLayer()
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

        private Level3PriceQuote ConvertPQToLevel3QuoteWithTraderForLayerAndLastTradeQuote(IPQLevel3Quote pQuote)
        {
            return new Level3PriceQuote(pQuote);
        }

        private ILevel2Quote ConvertPQToLevel2QuoteWithSourceNameLayer(IPQLevel2Quote pQuote)
        {
            return new Level2PriceQuote(pQuote);
        }

        private OrderBook GenerateBook<T>(int numberOfLayers, decimal startingPrice, decimal deltaPricePerLayer,
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

        private RecentlyTraded GenerateRecentlyTraded<T>(int numberOfRecentlyTraded, decimal startingPrice,
            decimal deltaPrice,
            DateTime startingTime, TimeSpan deltaTime, Func<decimal, DateTime, T> generateLastTraded,
            INameIdLookupGenerator nameIdLookupGen = null) where T : IMutableLastTrade
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

        private void UpdateSourceQuoteBook(IOrderBook toUpdate, INameIdLookupGenerator nameLookupGenerator,
            int numberOfLayers,
            decimal startingVolume, decimal deltaVolumePerLayer)
        {
            var currentVolume = startingVolume;
            for (var i = 0; i < numberOfLayers; i++)
            {
                var sourceLayer = (IMutableSourcePriceVolumeLayer) toUpdate[i];

                string traderName = null;
                if (startingVolume != 0m && deltaVolumePerLayer != 0m)
                {
                    traderName = $"SourceNameUpdate{i}";
                    nameLookupGenerator.GetOrAddId(traderName);
                }
                sourceLayer.SourceName = traderName;
                sourceLayer.Volume = currentVolume + i * deltaVolumePerLayer;
            }
        }

        private void UpdateTraderQuoteBook(IOrderBook toUpdate, INameIdLookupGenerator nameLookupGenerator,
            int numberOfLayers,
            int numberOfTradersPerLayer, decimal startingVolume, decimal deltaVolumePerLayer)
        {
            var currentVolume = startingVolume;
            for (var i = 0; i < numberOfLayers; i++)
            {
                var traderLayer = (IMutableTraderPriceVolumeLayer) toUpdate[i];
                for (var j = 0; j < numberOfTradersPerLayer; j++)
                {
                    string traderName = null;
                    if (startingVolume != 0m && deltaVolumePerLayer != 0m)
                    {
                        traderName = $"TraderLayer{i}_{j}";
                        nameLookupGenerator.GetOrAddId(traderName);
                    }
                    if (traderLayer.Count <= j)
                    {
                        traderLayer.Add(traderName, currentVolume + j * deltaVolumePerLayer);
                    }
                    else
                    {
                        var traderLayerInfo = traderLayer[j];
                        traderLayerInfo.TraderName = traderName;
                        traderLayerInfo.TraderVolume = currentVolume + j * deltaVolumePerLayer;
                    }
                }
                currentVolume += deltaVolumePerLayer;
            }
        }
    }
}