#region

using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeMarketsApi.Trading;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public sealed class OrxTradingServer : OrxAuthenticationServer
{
    private readonly bool errorSupport;
    private readonly ITradingFeed feed;
    private readonly AdapterOrderSessionTracker orderSessionTracker;
    private readonly Dictionary<string, bool> statuses = new();
    private int adapterOrderId;

    public OrxTradingServer(OrxServerMessaging messagePublisher, ITradingFeed feed, bool errorSupport = false)
        : base(messagePublisher, TradingVersionInfo.CurrentVersion)
    {
        this.feed = feed;
        this.errorSupport = errorSupport;
        orderSessionTracker = new AdapterOrderSessionTracker(OrxRecyclingFactory);

        IOrxPublisher orxPublisher = messagePublisher;

        orxPublisher.StreamFromSubscriber.RegisterDeserializer<OrxOrderSubmitRequest>(OnSubmit);
        orxPublisher.StreamFromSubscriber.RegisterDeserializer<OrxCancelRequest>(OnCancel);
        orxPublisher.StreamFromSubscriber.RegisterDeserializer<OrxAmendRequest>(AmendOrder);

        orxPublisher.RegisterSerializer<OrxStatusMessage>();
        orxPublisher.RegisterSerializer<OrxOrderUpdate>();
        orxPublisher.RegisterSerializer<OrxOrderAmendResponse>();
        orxPublisher.RegisterSerializer<OrxExecutionUpdate>();
        orxPublisher.RegisterSerializer<OrxVenueOrderUpdate>();

        feed.FeedStatusUpdate += OnStatus;
        feed.OrderUpdate += OnOrder;
        feed.OrderAmendResponse += OnAmendOrderResponse;
        feed.Execution += OnExecution;
        feed.VenueOrderUpdated += OnVenueOrder;
    }


    protected override void OnClientRemoved(ISocketSessionConnection client)
    {
        foreach (var order in orderSessionTracker.ReturnAllOrdersForSession(client)) feed.CancelOrder(order);
        orderSessionTracker.UnregisterSession(client);
    }

    protected override void OnDisconnecting()
    {
        OnStatus(null, false);
        base.OnDisconnecting();
        orderSessionTracker.ClearAll();
    }

    protected override bool Validate(OrxLogonRequest request, object? context, ISession? repositorySession)
    {
        if (base.Validate(request, context, repositorySession))
        {
            var orxUpStatusMessage = OrxRecyclingFactory.Borrow<OrxStatusMessage>();
            orxUpStatusMessage.Configure(feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                "general line", OrxRecyclingFactory);
            MessagePublisher.Send(repositorySession!, orxUpStatusMessage);
            lock (statuses)
            {
                foreach (var kv in statuses)
                {
                    var orxStatusMessage = OrxRecyclingFactory.Borrow<OrxStatusMessage>();
                    orxStatusMessage.Configure(
                        kv.Value && feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                        kv.Key, OrxRecyclingFactory);
                    MessagePublisher.Send(repositorySession!, orxStatusMessage);
                }
            }

            return true;
        }

        return false;
    }

    private void OnSubmit(OrxOrderSubmitRequest orderSubmitRequest, object? context, ISession? repositorySession)
    {
        var id = Interlocked.Increment(ref adapterOrderId).ToString();
        if (!Stopping)
        {
            var orxOrderPublisher = OrxRecyclingFactory.Borrow<OrxOrderPublisher>();
            orxOrderPublisher.Configure(repositorySession!, MessagePublisher, errorSupport);
            orderSubmitRequest.OrderDetails!.OrderPublisher = orxOrderPublisher;
            orderSubmitRequest.OrderDetails.OrderId.VenueAdapterOrderId = id;

            feed.SubmitOrderRequest(orderSubmitRequest);
            if (!orderSubmitRequest.OrderDetails.IsInError())
            {
                var orxOrder = OrxRecyclingFactory.Borrow<OrxOrder>();
                orxOrder.CopyFrom(orderSubmitRequest.OrderDetails);
                orderSessionTracker.RegisterOrderIdWithSession(orxOrder, repositorySession!);
            }

            OrxRecyclingFactory.Recycle(orxOrderPublisher);
        }
        else
        {
            var orxOrderStatusUpdate = OrxRecyclingFactory.Borrow<OrxOrderStatusUpdate>();
            orxOrderStatusUpdate.Configure(orderSubmitRequest.OrderDetails!.OrderId, OrderStatus.Dead,
                OrderUpdateEventType.Error | OrderUpdateEventType.OrderRejected, OrxRecyclingFactory);
            MessagePublisher.Send(repositorySession!, orxOrderStatusUpdate);
        }
    }

    private void AmendOrder(OrxAmendRequest orxAmendRequest, object? context, ISession? repositorySession)
    {
        if (!Stopping)
        {
            var previousOrder = orderSessionTracker.FindOrderFromSessionId(
                orxAmendRequest.OrderDetails!.OrderId.VenueAdapterOrderId!, repositorySession!);
            if (previousOrder == null)
            {
                var orxAmendReject = OrxRecyclingFactory.Borrow<OrxAmendReject>();
                orxAmendReject.Configure();
                MessagePublisher.Send(repositorySession!, orxAmendReject);
                return;
            }

            feed.AmendOrderRequest(previousOrder, orxAmendRequest.Amendment!);
        }
        else
        {
            var orxAmendReject = OrxRecyclingFactory.Borrow<OrxAmendReject>();
            orxAmendReject.Configure();
            MessagePublisher.Send(repositorySession!, orxAmendReject);
        }
    }

    private void OnCancel(OrxCancelRequest message, object? context, ISession? repositorySession)
    {
        var order = orderSessionTracker.FindOrderFromSessionId(message.OrderId!.VenueAdapterOrderId!
            , repositorySession!);
        if (order != null) feed.CancelOrder(order);
    }

    private void OnStatus(string? feedName, bool feedStatus)
    {
        var orxStatusMessage = OrxRecyclingFactory.Borrow<OrxStatusMessage>();
        orxStatusMessage.Configure(feedStatus ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
            feedName ?? "general line", OrxRecyclingFactory);
        MessagePublisher.Broadcast(orxStatusMessage);
        if (string.IsNullOrEmpty(feedName))
        {
            if (!feedStatus)
                foreach (var session in orderSessionTracker.AllRegisteredSessions())
                foreach (var order in orderSessionTracker.ReturnAllOrdersForSession(session))
                    feed.CancelOrder(order);
        }
        else
        {
            lock (statuses)
            {
                statuses[feedName] = feedStatus;
            }
        }
    }

    private void OnOrder(IOrderUpdate orderUpdate)
    {
        if (!orderUpdate.Order!.IsPending() || orderUpdate.Order!.Status == OrderStatus.Unknown)
            orderSessionTracker.UnregisterOrderWithSession(orderUpdate.Order!);

        if (orderUpdate.Order?.OrderPublisher?.UnderlyingSession != null)
        {
            var orxOrderUpdate = OrxRecyclingFactory.Borrow<OrxOrderUpdate>();
            orxOrderUpdate.CopyFrom(orderUpdate);
            MessagePublisher.Send(orderUpdate.Order.OrderPublisher.UnderlyingSession,
                orxOrderUpdate);
        }
    }

    private void OnAmendOrderResponse(IOrderAmendResponse amendResponse)
    {
        if (!amendResponse.Order!.IsPending() || amendResponse.Order!.Status == OrderStatus.Unknown)
            orderSessionTracker.UnregisterOrderWithSession(amendResponse.Order!);

        if (amendResponse.Order?.OrderPublisher?.UnderlyingSession != null)
        {
            var orxOrderAmendResponse = OrxRecyclingFactory.Borrow<OrxOrderAmendResponse>();
            orxOrderAmendResponse.CopyFrom(amendResponse);
            amendResponse.DecrementRefCount();
            MessagePublisher.Send(amendResponse.Order.OrderPublisher.UnderlyingSession,
                orxOrderAmendResponse);
        }
    }

    private void OnVenueOrder(IVenueOrderUpdate venueOrderUpdate)
    {
        var orderSession = orderSessionTracker.FindSessionFromOrderId(venueOrderUpdate.VenueOrder!.OrderId!);
        if (orderSession != null)
        {
            var orxOrxVenueOrderUpdate = OrxRecyclingFactory.Borrow<OrxVenueOrderUpdate>();
            orxOrxVenueOrderUpdate.CopyFrom(venueOrderUpdate);
            MessagePublisher.Send(orderSession, orxOrxVenueOrderUpdate);
        }
    }

    private void OnExecution(IExecutionUpdate executionUpdate)
    {
        if (executionUpdate.Execution?.OrderId != null)
        {
            var orderSession = orderSessionTracker.FindSessionFromOrderId(executionUpdate.Execution.OrderId);
            var orxExecutionUpdate = OrxRecyclingFactory.Borrow<OrxExecutionUpdate>();
            orxExecutionUpdate.CopyFrom(executionUpdate);
            MessagePublisher.Send(orderSession!, new OrxExecutionUpdate(executionUpdate));
        }
    }
}
