#region

using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Configuration.ClientServerConfig.Authentication;
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
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TradingClientServerTests));
    private TradingServerConfig tradingServerConfig = null!;

    [TestInitialize]
    public void Setup()
    {
        logger.Info("Starting setup of TradingClientServerTests");

        tradingServerConfig = new TradingServerConfig(
            new NetworkTopicConnectionConfig("TestTradingServer", SocketConversationProtocol.TcpAcceptor
                , new List<IEndpointConfig>
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.TradingServerPort)
                }), supportedVenueFeatures: VenueFeatures.Amends);
        logger.Info("Ended setup of TradingClientServerTests");
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    public void StartedTradingServer_ClientJoinsSendsOrder_ServerSendsConfirmation()
    {
        var orxServer = OrxServerMessaging.BuildTcpResponder(tradingServerConfig.TradingServerConnectionConfig);
        var clientOrderAutoResetEvent = new AutoResetEvent(false);
        var serverResponseTradingHandler = new TradingFeedHandle
        {
            IsAvailable = true
        };
        var orxTradingServer = new OrxTradingServer(orxServer, serverResponseTradingHandler, true);
        orxTradingServer.OnAuthenticate += OrxTradingServer_OnAuthenticate;

        orxServer.Connect();

        var orderStatus = OrderStatus.New;
        IOrder? clientLastOrderReceived = null;

        var orxClient = new OrxTradingClient(tradingServerConfig.ToggleProtocolDirection()
            , SingletonSocketDispatcherResolver.Instance
            , "TradingClientServerTest",
            new LoginCredentials("testLoginId", "testPassword"), "testAccount", true, new TradingFeedWatchdog(),
            new LoggingAlertManager(), false);
        orxClient.OrderUpdate += orderUpdate =>
        {
            clientLastOrderReceived = new Order(orderUpdate.Order!);
            orderStatus = orderUpdate.Order!.Status;
            logger.Info("orderStatus : {0}", orderStatus);
            // Console.WriteLine("orderStatus : {0}", orderStatus);
            clientOrderAutoResetEvent.Set();
        };

        Thread.Sleep(2000);
        Assert.IsTrue(orxClient.IsAvailable);
        var orderId = new OrderId(1234, "Test1234", 0, "", null, "Tracking1234");
        var timeInForce = TimeInForce.GoodTillCancelled;
        var creationTime = new DateTime(2018, 3, 30, 2, 4, 11);
        var orderSubmitRequest = new OrderSubmitRequest(new Order(orderId, timeInForce,
                creationTime, OrderStatus.New, new SpotOrder(orderId, timeInForce, creationTime,
                    OrderSide.Bid, "TestTicker", 1.23456m, 300_000L, OrderType.Limit, 100_000m, 0.00025m, 10_000m),
                new DateTime(2018, 3, 30, 2, 18, 2), new Parties(null,
                    new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                        new BookingInfo("TestAccount", "TestSubAccount"))), new DateTime(2018, 3, 30, 2, 18, 2),
                new VenueCriteria(new List<IVenue>() { new Venue(23, "TestVenue") },
                    VenueSelectionMethod.Default), null, null, "", null),
            1, new DateTime(2018, 3, 30, 2, 18, 2), new DateTime(2018, 3, 30, 2, 18, 2), "Tag")
        {
            AutoRecycleAtRefCountZero = false
        };
        orderSubmitRequest.IncrementRefCount();
        orxClient.SubmitOrderRequest(orderSubmitRequest);

        clientOrderAutoResetEvent.WaitOne(5_000);

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(OrderStatus.Active, orderStatus);
        Assert.AreEqual((MutableString)"1", clientLastOrderReceived.OrderId.VenueAdapterOrderId);
        var serverResponseOrder = serverResponseTradingHandler.LastReceivedOrder;
        Assert.IsNotNull(serverResponseOrder);
        Assert.IsNotNull(serverResponseOrder.OrderPublisher);
        Assert.IsNotNull(serverResponseOrder.OrderPublisher.UnderlyingSession);

        IVenueOrder? clientLastVenueOrderReceived = null;

        var venueAutoResetEvent = new AutoResetEvent(false);
        orxClient.VenueOrderUpdated += update =>
        {
            clientLastVenueOrderReceived = new VenueOrder(update.VenueOrder!);
            venueAutoResetEvent.Set();
        };

        var serverVenueOrderUpdate = new VenueOrderUpdate(new VenueOrder(
                new VenueOrderId("VenueOrderId23_0123", ""),
                new OrderId(1234, "Test1234", 1, "1", null, "TrackingId1234"),
                OrderStatus.New, new Venue(1234, "TestVenueName"), new DateTime(2018, 4, 4, 14, 49, 43),
                new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(20), "TestTicker", 1.2345m, 100_000m),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(111),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(110),
            new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(112))
        {
            AutoRecycleAtRefCountZero = false
        };
        serverVenueOrderUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnVenueOrder(serverVenueOrderUpdate);
        venueAutoResetEvent.WaitOne(5_000);

        Assert.IsNotNull(clientLastVenueOrderReceived);
        Assert.AreEqual((MutableString)"VenueOrderId23_0123"
            , clientLastVenueOrderReceived.VenueOrderId!.VenueClientOrderId);

        IExecution? lastExecution = null;

        var executionAutoResetEvent = new AutoResetEvent(false);
        orxClient.Execution += executionUpdate =>
        {
            lastExecution = executionUpdate.Execution?.Clone();
            executionAutoResetEvent.Set();
        };

        var serverExecutionUpdate = new ExecutionUpdate(new Execution(
            new ExecutionId("ExecutionId", 123, ""),
            new Venue(23, "TestVenue"), new VenueOrderId("VenueOrderId23_0123", ""), clientLastOrderReceived.OrderId,
            new DateTime(2018, 3, 23, 20, 33, 1), 1.23456m, 10_000, 10_000, 1.23456m,
            new Party("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                new BookingInfo("TestAccount", "TestSubAccount")), new DateTime(2018, 3, 26),
            ExecutionType.CounterPartyGave, ExecutionStageType.Trade), ExecutionUpdateType.Created)
        {
            AutoRecycleAtRefCountZero = false
        };
        serverExecutionUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnExecution(serverExecutionUpdate);

        executionAutoResetEvent.WaitOne(5_000);

        Assert.IsNotNull(lastExecution);

        var clientEditOrder = clientLastOrderReceived;
        clientLastOrderReceived = null;

        orxClient.OrderAmend += amendedOrder => { clientLastOrderReceived = amendedOrder.Clone(); };

        var amendOrderRequest = new OrderAmend(1_000_000);
        amendOrderRequest.IncrementRefCount();
        amendOrderRequest.AutoRecycleAtRefCountZero = false;
        orxClient.AmendOrderRequest(clientEditOrder, amendOrderRequest);

        clientOrderAutoResetEvent.WaitOne(5_000);

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(1_000_000, ((ISpotOrder)clientLastOrderReceived.Product!).Size);

        clientEditOrder = clientLastOrderReceived;
        clientLastOrderReceived = null;

        orxClient.CancelOrder(clientEditOrder);

        clientOrderAutoResetEvent.WaitOne(5_000);

        serverResponseOrder.Status = OrderStatus.Dead;
        var serverOrderUpdate = new OrderUpdate(serverResponseOrder,
            OrderUpdateEventType.OrderAmended, new DateTime(2018, 3, 30, 2, 35, 43));
        serverExecutionUpdate.AutoRecycleAtRefCountZero = false;
        serverExecutionUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnOrderUpdate(serverOrderUpdate);

        clientOrderAutoResetEvent.WaitOne(5_000);

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(OrderStatus.Dead, clientLastOrderReceived.Status);
    }

    private bool OrxTradingServer_OnAuthenticate(ISocketSessionContext clientSessionConnection, MutableString? usr,
        MutableString? pwd, out IUserData authData, out MutableString message)
    {
        message = "All Good";
        authData = new UserData((MutableString)"TestUser", new AuthenticationData(AuthenticationType.None, null));
        return true;
    }
}
