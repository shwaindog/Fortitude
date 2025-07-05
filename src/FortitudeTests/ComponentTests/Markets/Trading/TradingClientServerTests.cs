// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Config.Authentication;
using FortitudeMarkets.Config.TradingConfig;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Publication;
using FortitudeMarkets.Trading.ORX.Subscription;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.ComponentTests.Markets.Trading;

[TestClass]
[NoMatchingProductionClass]
public class TradingClientServerTests
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TradingClientServerTests));

    private OrxTradingClient    orxClient = null!;
    private OrxTradingServer?   orxTradingServer;
    private TradingServerConfig tradingServerConfig = null!;

    [TestInitialize]
    public void Setup()
    {
        logger.Info("Starting setup of TradingClientServerTests");

        tradingServerConfig =
            new TradingServerConfig
                (new NetworkTopicConnectionConfig
                    ("TestTradingServer", SocketConversationProtocol.TcpAcceptor
                   , new List<IEndpointConfig>
                     {
                         new EndpointConfig(TestMachineConfig.LoopBackIpAddress, TestMachineConfig.TradingServerPort, CountryCityCodes.AUinMEL)
                     }), supportedVenueFeatures: VenueFeatures.Amends);
        logger.Info("Ended setup of TradingClientServerTests");
    }

    [TestCleanup]
    public void TearDown()
    {
        logger.Info("Test complete starting shutdown");
        orxClient.Dispose();
        orxTradingServer?.ShutdownImmediate();
        // FLoggerFactory.WaitUntilDrained();
    }

    [TestCategory("Integration")]
    [TestCategory("LoopBackIPRequired")]
    [TestMethod]
    [Timeout(30_000)]
    public void StartedTradingServer_ClientJoinsSendsOrder_ServerSendsConfirmation()
    {
        var orxServer = OrxServerMessaging.BuildTcpResponder(tradingServerConfig.TradingServerConnectionConfig);

        var clientOrderUpdatedResetEvent = new AutoResetEvent(false);
        var serverAutoResetEvent         = new AutoResetEvent(false);
        var serverResponseTradingHandler = new TradingFeedHandle
        {
            IsAvailable = true
        };
        orxTradingServer = new OrxTradingServer(orxServer, serverResponseTradingHandler, true);

        orxTradingServer.OnAuthenticate += OrxTradingServer_OnAuthenticate;

        orxServer.Start();

        var orderStatus = OrderStatus.New;

        ITransmittableOrder? clientLastOrderReceived = null;

        orxClient = new OrxTradingClient
            (tradingServerConfig.ToggleProtocolDirection()
           , SingletonSocketDispatcherResolver.Instance
           , "TradingClientServerTest",
             new LoginCredentials("testLoginId", "testPassword"), 12221, true, new TradingFeedWatchdog(),
             new LoggingAlertManager(), false);
        orxClient.OrderUpdate += orderUpdate =>
        {
            clientLastOrderReceived = orderUpdate.Order!.AsTransmittableOrder;
            orderStatus             = orderUpdate.Order!.Status;
            logger.Info("****** ORDER UPDATED ******** orderStatus : {0} for order {1}", orderStatus, clientLastOrderReceived);
            // Console.WriteLine("orderStatus : {0}", orderStatus);
            clientOrderUpdatedResetEvent.Set();
        };

        Thread.Sleep(500);
        Assert.IsTrue(orxClient.IsAvailable);
        var orderId      = new OrderId(1234);
        var timeInForce  = TimeInForce.GoodTillCancelled;
        var creationTime = new DateTime(2018, 3, 30, 2, 4, 11);
        var orderSubmitRequest =
            new OrderSubmitRequest
                (new SpotTransmittableOrder (orderId, (ushort)1234, accountId: 9876u, OrderSide.Bid, 1.23456m, 300_000m
                         , OrderType.PassiveLimit, creationTime, OrderStatus.PendingNew, timeInForce, null, 100_000m
                         , 0.00025m, 10_000m, tickerName:  "TestTicker"),
                 1, new DateTime(2018, 3, 30, 2, 18, 2)
               , new DateTime(2018, 3, 30, 2, 18, 2), "Tag")
                {
                    AutoRecycleAtRefCountZero = false
                };
        orderSubmitRequest.IncrementRefCount();
        orxClient.SubmitOrderRequest(orderSubmitRequest);

        logger.Info("Just submitted order from orxClient");
        AwaitEvent(clientOrderUpdatedResetEvent, 2_000, "Client Await Submit Order");

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(OrderStatus.Active, orderStatus);
        Assert.AreEqual(1u, clientLastOrderReceived.OrderId.AdapterOrderId!.Value);
        var serverResponseOrder = serverResponseTradingHandler.LastReceivedOrder;
        Assert.IsNotNull(serverResponseOrder);
        serverResponseOrder.AutoRecycleAtRefCountZero = false;
        Assert.IsNotNull(serverResponseOrder.OrderPublisher);
        Assert.IsNotNull(serverResponseOrder.OrderPublisher.UnderlyingSession);

        IVenueOrder? clientLastVenueOrderReceived = null;

        var venueAutoResetEvent = new AutoResetEvent(false);
        orxClient.VenueOrderUpdated += update =>
        {
            clientLastVenueOrderReceived = new VenueOrder(update.VenueOrder!);
            venueAutoResetEvent.Set();
        };

        var serverVenueOrderUpdate =
            new VenueOrderUpdate
                (new VenueOrder
                     (new VenueOrderId("VenueOrderId23_0123", ""),
                      new OrderId(orderId.ClientOrderId, 0, 1),
                      OrderStatus.New, new Venue(1234, "TestVenueName")
                    , new DateTime(2018, 4, 4, 14, 49, 43),
                      new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(20), "TestTicker"
                    , 1.2345m, 100_000m),
                 new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(111),
                 new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(110),
                 new DateTime(2018, 4, 4, 14, 49, 43).AddMilliseconds(112))
                {
                    AutoRecycleAtRefCountZero = false
                };
        serverVenueOrderUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnVenueOrder(serverVenueOrderUpdate);
        AwaitEvent(venueAutoResetEvent, 5_000, "Client Await VenueOrder Updated");

        Assert.IsNotNull(clientLastVenueOrderReceived);
        Assert.AreEqual((MutableString)"VenueOrderId23_0123"
                      , clientLastVenueOrderReceived.VenueOrderId!.VenueClientOrderId);

        IExecution? lastExecution = null;

        var executionAutoResetEvent = new AutoResetEvent(false);
        orxClient.Execution += executionUpdate =>
        {
            logger.Info("orxClient Received {0}", executionUpdate);
            lastExecution = executionUpdate.Execution?.Clone();
            executionAutoResetEvent.Set();
        };

        var serverExecutionUpdate =
            new ExecutionUpdate
                (new Execution
                     (new ExecutionId("ExecutionId", 123, ""),
                      new Venue(23, "TestVenue"), new VenueOrderId("VenueOrderId23_0123", "")
                    , clientLastOrderReceived.OrderId,
                      new DateTime(2018, 3, 23, 20, 33, 1), 1.23456m, 10_000, 10_000, 1.23456m,
                      new PartyPortfolio(1234, 1234)
                    , new DateTime(2018, 3, 26),
                      ExecutionType.CounterPartyGave, ExecutionStageType.Trade)
               , ExecutionUpdateType.Created)
                {
                    AutoRecycleAtRefCountZero = false
                };
        serverExecutionUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnExecution(serverExecutionUpdate);

        AwaitEvent(executionAutoResetEvent, 2_000, "Client Await Execution Update");

        Assert.IsNotNull(lastExecution);

        var clientEditOrder = clientLastOrderReceived;
        clientLastOrderReceived = null;

        orxClient.OrderAmend += amendedOrder =>
        {
            logger.Info("orxClient Received {0}", amendedOrder);
            clientLastOrderReceived = amendedOrder.Clone();
            clientOrderUpdatedResetEvent.Set();
        };

        serverResponseTradingHandler.OrderAmendResponse += orderAmended =>
        {
            logger.Info("serverResponseTradingHandler Received {0}", orderAmended);
            serverAutoResetEvent.Set();
        };

        var amendOrderRequest = new OrderAmend(1_000_000);
        amendOrderRequest.IncrementRefCount();
        amendOrderRequest.AutoRecycleAtRefCountZero = false;
        orxClient.AmendOrderRequest(clientEditOrder, amendOrderRequest);

        AwaitEvent(serverAutoResetEvent, 2_000, "Server Change Order Size OrderAmendResponse");
        AwaitEvent(clientOrderUpdatedResetEvent, 2_000, "Client Order Amend Size Response");
        clientOrderUpdatedResetEvent.WaitOne(2_000);

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(1_000_000, ((IMutableSpotOrder)clientLastOrderReceived!).Size);

        clientEditOrder         = clientLastOrderReceived;
        clientLastOrderReceived = null;

        orxClient.CancelOrder(clientEditOrder);

        serverResponseOrder.Status = OrderStatus.Dead;
        var serverOrderUpdate = new OrderUpdate(serverResponseOrder,
                                                OrderUpdateEventType.OrderDead, new DateTime(2018, 3, 30, 2, 35, 43));
        serverOrderUpdate.AutoRecycleAtRefCountZero = false;
        serverOrderUpdate.IncrementRefCount();
        serverResponseTradingHandler.OnOrderUpdate(serverOrderUpdate);

        AwaitEvent(clientOrderUpdatedResetEvent, 2_000, "Client Cancel Confirmed");

        Assert.IsNotNull(clientLastOrderReceived);
        Assert.AreEqual(OrderStatus.Dead, clientLastOrderReceived.Status);
    }

    private void AwaitEvent(AutoResetEvent waitOn, int timeout, string message)
    {
        var startWait = DateTime.Now;
        var wasSet    = waitOn.WaitOne(timeout);
        var waitTime  = DateTime.Now - startWait;
        if (!wasSet)
            logger.Warn("Timed out waiting {0}ms on {1}", timeout, message);
        else if (waitTime > TimeSpan.FromMilliseconds(100)) logger.Warn("Waited {0}ms for {1}", waitTime.TotalMilliseconds, message);
    }

    private bool OrxTradingServer_OnAuthenticate
    (ISocketSessionContext clientSessionConnection, MutableString? usr,
        MutableString? pwd, out IUserData authData, out MutableString message)
    {
        message  = "All Good";
        authData = new UserData((MutableString)"TestUser", new AuthenticationData(AuthenticationType.None, null));
        return true;
    }
}
