#region

using System.Reactive.Linq;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
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
using FortitudeMarketsCore.Trading.Orders.Products.General;
using FortitudeMarketsCore.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Publication;
using FortitudeMarketsCore.Trading.ORX.Subscription;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.ComponentTests.Markets.Trading;

[TestClass]
[NoMatchingProductionClass]
public class TradingClientServerTests
{
    private ISocketDispatcher clientSocketDispatcher = null!;

    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TradingClientServerTests));
    private OSNetworkingController networkingController = null!;
    private ISocketDispatcher serverSocketDispatcher = null!;
    private TradingServerConfig tradingServerConfig = null!;

    [TestInitialize]
    public void Setup()
    {
        logger.Info("Starting setup of TradingClientServerTests");
        networkingController = new OSNetworkingController();
        serverSocketDispatcher = new SocketDispatcher(
            new SocketDispatcherListener(new SocketSelector(1000, networkingController), "TestTradingServer"),
            new SocketDispatcherSender("ServerSocketDispatcher"));
        clientSocketDispatcher = new SocketDispatcher(
            new SocketDispatcherListener(new SocketSelector(1000, networkingController), "TestTradingClient"),
            new SocketDispatcherSender("ClientSocketDispatcher"));

        tradingServerConfig = new TradingServerConfig(short.MaxValue, "TestExchangeAdapter",
            MarketServerType.Trading, new[]
            {
                new ConnectionConfig("TestSnapshotServer",
                    TestMachineConfig.LoopBackIpAddress, TestMachineConfig.TradingServerPort,
                    ConnectionDirectionType.Both, "none", 500)
            }, new TimeTable(),
            Observable.Empty<IMarketServerConfigUpdate<ITradingServerConfig>>(),
            OrderType.Limit,
            TimeInForce.GoodTillCancelled | TimeInForce.ImmediateOrCancel, VenueFeatures.Amends,
            true, new SnapshotUpdatePricingServerConfig("TestExchangeName",
                MarketServerType.MarketData,
                new[]
                {
                    new ConnectionConfig("TestSnapshotServer", TestMachineConfig.LoopBackIpAddress,
                        TestMachineConfig.ServerSnapshotPort,
                        ConnectionDirectionType.Both, "none", 500)
                    , new ConnectionConfig("TestUpdateServer", TestMachineConfig.LoopBackIpAddress,
                        TestMachineConfig.ServerUpdatePort, ConnectionDirectionType.Publisher,
                        TestMachineConfig.NetworkSubAddress, 500)
                }, null, 9000, Enumerable.Empty<ISourceTickerPublicationConfig>(), false, false));
        logger.Info("Ended setup of TradingClientServerTests");
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void StartedTradingServer_ClientJoinsSendsOrder_ServerSendsConfirmation()
    {
        var orxServer = new OrxServerMessaging(serverSocketDispatcher, networkingController,
            TestMachineConfig.TradingServerPort, "TestTradingServer");
        var orderAutoResetEvent = new AutoResetEvent(false);
        var tradingHandler = new TradingFeedHandle
        {
            IsAvailable = true
        };
        var orxTradingServer = new OrxTradingServer(orxServer, tradingHandler, true);
        orxTradingServer.OnAuthenticate += OrxTradingServer_OnAuthenticate;

        orxServer.Connect();

        var orderStatus = OrderStatus.New;
        IOrder? lastOrder = null;

        var orxClient = new OrxTradingClient(clientSocketDispatcher, networkingController, tradingServerConfig,
            new LoginCredentials("testLoginId", "testPassword"), "testAccount", true, new TradingFeedWatchdog(),
            new LoggingAlertManager(), false, true);
        orxClient.OrderUpdate += orderUpdate =>
        {
            lastOrder = new Order(orderUpdate.Order!);
            orderStatus = orderUpdate.Order!.Status;
            logger.Info("orderStatus : {0}", orderStatus);
            // Console.WriteLine("orderStatus : {0}", orderStatus);
            orderAutoResetEvent.Set();
        };

        Thread.Sleep(40);
        Assert.IsTrue(orxClient.IsAvailable);
        var orderId = new OrderId(1234, "Test1234", 0, "", null, "Tracking1234");
        var timeInForce = TimeInForce.GoodTillCancelled;
        var creationTime = new DateTime(2018, 3, 30, 2, 4, 11);
        orxClient.SubmitOrderRequest(new OrderSubmitRequest(new Order(orderId, timeInForce,
                creationTime, OrderStatus.New, new SpotOrder(orderId, timeInForce, creationTime,
                    OrderSide.Bid, "TestTicker", 1.23456m, 300_000L, OrderType.Limit, 100_000m, 0.00025m, 10_000m),
                new DateTime(2018, 3, 30, 2, 18, 2), new Parties(null,
                    new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                        new BookingInfo("TestAccount", "TestSubAccount"))), new DateTime(2018, 3, 30, 2, 18, 2),
                new VenueCriteria(new List<IVenue>() { new Venue(23, "TestVenue") },
                    VenueSelectionMethod.Default), null, null, "", null),
            1, new DateTime(2018, 3, 30, 2, 18, 2), new DateTime(2018, 3, 30, 2, 18, 2), "Tag"));

        orderAutoResetEvent.WaitOne(1_000);

        Assert.IsNotNull(lastOrder);
        Assert.AreEqual(OrderStatus.Active, orderStatus);
        Assert.AreEqual((MutableString)"1", lastOrder.OrderId.VenueAdapterOrderId);

        IVenueOrder? lastVenueOrder = null;

        var venueAutoResetEvent = new AutoResetEvent(false);
        orxClient.VenueOrderUpdated += update =>
        {
            lastVenueOrder = new VenueOrder(update.VenueOrder!);
            venueAutoResetEvent.Set();
        };

        tradingHandler.OnVenueOrder(new VenueOrderUpdate(new VenueOrder(new VenueOrderId("VenueOrderId23_0123", ""),
                new OrderId(1234, "Test1234", 1, "1", null, "TrackingId1234"),
                OrderStatus.New, new Venue(1234, "TestVenueName"), new DateTime(2018, 4, 4, 14, 49, 43),
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(20), "TestTicker", 1.2345m, 100_000m),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(111),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(110),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(112)));

        venueAutoResetEvent.WaitOne(1_000);

        Assert.IsNotNull(lastVenueOrder);
        Assert.AreEqual((MutableString)"VenueOrderId23_0123", lastVenueOrder.VenueOrderId!.VenueClientOrderId);

        IExecution? lastExecution = null;

        var executionAutoResetEvent = new AutoResetEvent(false);
        orxClient.Execution += executionUpdate =>
        {
            lastExecution = executionUpdate.Execution;
            executionAutoResetEvent.Set();
        };

        tradingHandler.OnExecution(new ExecutionUpdate(new Execution(new ExecutionId("ExecutionId", 123, ""),
            new Venue(23, "TestVenue"), new VenueOrderId("VenueOrderId23_0123", ""), lastOrder.OrderId,
            new DateTime(2018, 3, 23, 20, 33, 1), 1.23456m, 10_000, 10_000, 1.23456m,
            new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                new BookingInfo("TestAccount", "TestSubAccount")), new DateTime(2018, 3, 26),
            ExecutionType.CounterPartyGave, ExecutionStageType.Trade), ExecutionUpdateType.Created));

        executionAutoResetEvent.WaitOne(1_000);

        Assert.IsNotNull(lastExecution);

        var editOrder = lastOrder;
        lastOrder = null;

        orxClient.OrderAmend += amendedOrder => { lastOrder = amendedOrder.Clone(); };

        orxClient.AmendOrderRequest(editOrder, new OrderAmend(1_000_000));

        orderAutoResetEvent.WaitOne(1_000);

        Assert.IsNotNull(lastOrder);
        Assert.AreEqual(1_000_000, ((ISpotOrder)lastOrder.Product!).Size);

        editOrder = lastOrder;
        lastOrder = null;

        orxClient.CancelOrder(editOrder);

        orderAutoResetEvent.WaitOne(1_000);

        tradingHandler.LastReceivedOrder!.Status = OrderStatus.Dead;
        tradingHandler.OnOrderUpdate(new OrderUpdate(tradingHandler.LastReceivedOrder,
            OrderUpdateEventType.OrderAmended, new DateTime(2018, 3, 30, 2, 35, 43)));

        orderAutoResetEvent.WaitOne(1_000);

        Assert.IsNotNull(lastOrder);
        Assert.AreEqual(OrderStatus.Dead, lastOrder.Status);
    }

    private bool OrxTradingServer_OnAuthenticate(ISessionConnection clientSessionConnection, MutableString? usr,
        MutableString? pwd, out IUserData authData, out MutableString message)
    {
        message = "All Good";
        authData = new UserData((MutableString)"TestUser", new AuthenticationData(AuthenticationType.None, null));
        return true;
    }
}
