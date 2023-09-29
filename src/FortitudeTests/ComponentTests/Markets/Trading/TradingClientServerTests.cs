using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Configuration.ClientServerConfig.Authentication;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.Counterparties;
using FortitudeMarketsCore.Trading.Executions;
using FortitudeMarketsCore.Trading.Orders;
using FortitudeMarketsCore.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.Orders.Products;
using FortitudeMarketsCore.Trading.Orders.Products.General;
using FortitudeMarketsCore.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Publication;
using FortitudeMarketsCore.Trading.ORX.Subscription;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.ComponentTests.Markets.Trading
{
    [TestClass, NoMatchingProductionClass]
    public class TradingClientServerTests
    {
        private OSNetworkingController networkingController;
        private ISocketDispatcher serverSocketDispatcher;
        private ISocketDispatcher clientSocketDispatcher;
        private TradingServerConfig tradingServerConfig;

        [TestInitialize]
        public void Setup()
        {
            networkingController = new OSNetworkingController();
            serverSocketDispatcher = new SocketDispatcher(
                new SocketDispatcherListener(new SocketSelector(1000, networkingController), "TestTradingServer"),
                new SocketDispatcherSender());
            clientSocketDispatcher = new SocketDispatcher(
                new SocketDispatcherListener(new SocketSelector(1000, networkingController), "TestTradingClient"),
                new SocketDispatcherSender());

            tradingServerConfig = new TradingServerConfig(short.MaxValue, "TestExchangeAdapter",
                MarketServerType.Trading, new[]{ new ConnectionConfig("TestSnapshotServer",
                    TestMachineConfig.LoopBackIpAddress, TestMachineConfig.TradingServerPort,
                    ConnectionDirectionType.Both, "none", 500)}, new TimeTable(),
                Observable.Empty<IMarketServerConfigUpdate<ITradingServerConfig>>(),
                OrderType.Limit,
                TimeInForce.GoodTillCancelled | TimeInForce.ImmediateOrCancel, VenueFeatures.Amends,
                true, new SnapshotUpdatePricingServerConfig("TestExchangeName",
                    MarketServerType.MarketData,
                    new[]
                    {
                        new ConnectionConfig("TestSnapshotServer", TestMachineConfig.LoopBackIpAddress,
                            TestMachineConfig.ServerSnapshotPort,
                            ConnectionDirectionType.Both, "none", 500),
                        new ConnectionConfig("TestUpdateServer", TestMachineConfig.LoopBackIpAddress,
                            TestMachineConfig.ServerUpdatePort, ConnectionDirectionType.Publisher,
                            TestMachineConfig.NetworkSubAddress, 500)
                    }, null, 9000, null, false, false));
        }

        [TestCategory("Integration")]
        [TestCategory("LoopBackIPRequired")]
        [TestMethod]
        public void StartedTradingServer_ClientJoinsSendsOrder_ServerSendsConfirmation()
        {
            var orxServer = new OrxServerMessaging(serverSocketDispatcher, networkingController, 
                TestMachineConfig.TradingServerPort, "TestTradingServer");
            var tradingHandler = new TradingFeedHandle
            {
                IsAvailable = true
            };
            var orxTradingServer = new OrxTradingServer(orxServer, tradingHandler, true);
            orxTradingServer.OnAuthenticate += OrxTradingServer_OnAuthenticate;

            orxServer.Connect();

            var orderStatus = OrderStatus.New;
            IOrder lastOrder = null;

            var orxClient = new OrxTradingClient(clientSocketDispatcher, networkingController, tradingServerConfig,
                new LoginCredentials("testLoginId", "testPassword"), "testAccount", true, new TradingFeedWatchdog(),
                new LoggingAlertManager(), false, true);
            orxClient.OrderUpdate += orderUpdate =>
            {
                lastOrder = new Order(orderUpdate.Order);
                orderStatus = orderUpdate.Order.Status;
            };

            Thread.Sleep(400);
            orxClient.SubmitOrderRequest(new OrderSubmitRequest(new Order(new OrderId(1234, "Test1234", 
                        0, null, null, "Tracking1234"), TimeInForce.GoodTillCancelled, 
                    new DateTime(2018, 3, 30, 2, 4, 11), OrderStatus.New, new SpotOrder( 
                        OrderSide.Bid, "TestTicker", 1.23456m, 300_000L, OrderType.Limit, 100_000m, 0.00025m, 10_000m), 
                    new DateTime(2018, 3, 30, 2, 18, 2), new Parties(null, 
                        new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId", 
                        new BookingInfo("TestAccount", "TestSubAccount"))), new DateTime(2018, 3, 30, 2, 18, 2), 
                    new VenueCriteria( new List<IVenue>() {new Venue(23, "TestVenue")}, 
                        VenueSelectionMethod.Default), null, null, "", null),
                1, new DateTime(2018, 3, 30, 2, 18, 2), new DateTime(2018, 3, 30, 2, 18, 2), "Tag"));
            Thread.Sleep(80);

            Assert.IsNotNull(lastOrder);
            Assert.AreEqual(OrderStatus.Active, orderStatus);
            Assert.AreEqual((MutableString)"1", lastOrder.OrderId.VenueAdapterOrderId);

            IVenueOrder lastVenueOrder = null;

            orxClient.VenueOrderUpdated += update => { lastVenueOrder = new VenueOrder(update.VenueOrder); };

            tradingHandler.OnVenueOrder(new VenueOrderUpdate(new VenueOrder(new VenueOrderId("VenueOrderId23_0123", null),
                    new OrderId(1234, "Test1234", 1, "1", null, "TrackingId1234"), 
                OrderStatus.New, new Venue(1234, "TestVenueName"), new DateTime(2018, 4, 4, 14, 49, 43),
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(20), "TestTicker", 1.2345m, 100_000m),
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(111), 
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(110),
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(112)));
            
            Thread.Sleep(50);

            Assert.IsNotNull(lastVenueOrder);
            Assert.AreEqual((MutableString)"VenueOrderId23_0123", lastVenueOrder.VenueOrderId.VenueClientOrderId);

            IExecution lastExecution = null;

            orxClient.Execution += executionUpdate => { lastExecution = executionUpdate.Execution; };

            tradingHandler.OnExecution(new ExecutionUpdate(new Execution(new ExecutionId("ExecutionId", 123, null), 
                new Venue(23, "TestVenue"), new VenueOrderId("VenueOrderId23_0123", null), lastOrder.OrderId, 
                new DateTime(2018, 3, 23, 20, 33, 1), 1.23456m, 10_000, 10_000, 1.23456m, 
                new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                    new BookingInfo("TestAccount", "TestSubAccount")), new DateTime(2018, 3, 26), 
                ExecutionType.CounterPartyGave, ExecutionStageType.Trade), ExecutionUpdateType.Created));

            Thread.Sleep(50);

            Assert.IsNotNull(lastExecution);

            var editOrder = lastOrder;
            lastOrder = null;

            orxClient.OrderAmend += amendedOrder => { lastOrder = amendedOrder.Clone(); };

            orxClient.AmendOrderRequest(editOrder, new OrderAmend(1_000_000));

            Thread.Sleep(50);

            Assert.IsNotNull(lastOrder);
            Assert.AreEqual(1_000_000, ((ISpotOrder)lastOrder.Product).Size);

            editOrder = lastOrder;
            lastOrder = null;

            orxClient.CancelOrder(editOrder);

            Thread.Sleep(50);

            tradingHandler.LastReceivedOrder.Status = OrderStatus.Dead;
            tradingHandler.OnOrderUpdate(new OrderUpdate(tradingHandler.LastReceivedOrder, 
                OrderUpdateEventType.OrderAmended, new DateTime(2018, 3, 30, 2, 35, 43)));
            
            Thread.Sleep(50);

            Assert.IsNotNull(lastOrder);
            Assert.AreEqual(OrderStatus.Dead, lastOrder.Status);
        }

        private bool OrxTradingServer_OnAuthenticate(ISessionConnection clientSessionConnection, MutableString usr, 
            MutableString pwd, out IUserData authData, out MutableString message)
        {
            message = "All Good";
            authData = new UserData((MutableString)"TestUser", new AuthenticationData(AuthenticationType.None, null));
            return true;
        }
    }
}
