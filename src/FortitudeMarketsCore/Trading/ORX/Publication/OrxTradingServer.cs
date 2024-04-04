#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
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

    public OrxTradingServer(IOrxMessageResponder messageMessageResponder, ITradingFeed feed, bool errorSupport = false)
        : base(messageMessageResponder, TradingVersionInfo.CurrentVersion)
    {
        this.feed = feed;
        this.errorSupport = errorSupport;
        orderSessionTracker = new AdapterOrderSessionTracker(Recycler);

        var orxPublisher = messageMessageResponder;

        orxPublisher.DeserializationRepository.RegisterDeserializer<OrxOrderSubmitRequest>(OnSubmit);
        orxPublisher.DeserializationRepository.RegisterDeserializer<OrxCancelRequest>(OnCancel);
        orxPublisher.DeserializationRepository.RegisterDeserializer<OrxAmendRequest>(AmendOrder);

        orxPublisher.SerializationRepository.RegisterSerializer<OrxStatusMessage>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxOrderUpdate>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxOrderAmendResponse>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxExecutionUpdate>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxVenueOrderUpdate>();

        feed.FeedStatusUpdate += OnStatus;
        feed.OrderUpdate += OnOrder;
        feed.OrderAmendResponse += OnAmendOrderResponse;
        feed.Execution += OnExecution;
        feed.VenueOrderUpdated += OnVenueOrder;
    }

    protected override void OnClientRemoved(IConversationRequester client)
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

    protected override bool Validate(OrxLogonRequest request, object? context, IConversation? repositorySession)
    {
        if (base.Validate(request, context, repositorySession))
        {
            var orxUpStatusMessage = Recycler.Borrow<OrxStatusMessage>();
            orxUpStatusMessage.Configure(feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                "general line", Recycler);
            MessageMessageResponder.Send(repositorySession!, orxUpStatusMessage);
            lock (statuses)
            {
                foreach (var kv in statuses)
                {
                    var orxStatusMessage = Recycler.Borrow<OrxStatusMessage>();
                    orxStatusMessage.Configure(
                        kv.Value && feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                        kv.Key, Recycler);
                    MessageMessageResponder.Send(repositorySession!, orxStatusMessage);
                }
            }

            return true;
        }

        return false;
    }

    private void OnSubmit(OrxOrderSubmitRequest orderSubmitRequest, object? context, IConversation? repositorySession)
    {
        var id = Interlocked.Increment(ref adapterOrderId).ToString();
        if (!Stopping)
        {
            var orxOrderPublisher = Recycler.Borrow<OrxOrderPublisher>();
            orxOrderPublisher.Configure(repositorySession!, MessageMessageResponder, errorSupport);
            orderSubmitRequest.OrderDetails!.OrderPublisher = orxOrderPublisher;
            orderSubmitRequest.OrderDetails.OrderId.VenueAdapterOrderId = id;

            feed.SubmitOrderRequest(orderSubmitRequest);
            if (!orderSubmitRequest.OrderDetails.IsInError())
            {
                var orxOrder = Recycler.Borrow<OrxOrder>();
                orxOrder.CopyFrom(orderSubmitRequest.OrderDetails);
                orderSessionTracker.RegisterOrderIdWithSession(orxOrder, repositorySession!);
            }

            orxOrderPublisher.DecrementRefCount();
        }
        else
        {
            var orxOrderStatusUpdate = Recycler.Borrow<OrxOrderStatusUpdate>();
            orxOrderStatusUpdate.Configure(orderSubmitRequest.OrderDetails!.OrderId, OrderStatus.Dead,
                OrderUpdateEventType.Error | OrderUpdateEventType.OrderRejected, Recycler);
            MessageMessageResponder.Send(repositorySession!, orxOrderStatusUpdate);
        }
    }

    private void AmendOrder(OrxAmendRequest orxAmendRequest, object? context, IConversation? repositorySession)
    {
        if (!Stopping)
        {
            var previousOrder = orderSessionTracker.FindOrderFromSessionId(
                orxAmendRequest.OrderDetails!.OrderId.VenueAdapterOrderId!, repositorySession!);
            if (previousOrder == null)
            {
                var orxAmendReject = Recycler.Borrow<OrxAmendReject>();
                orxAmendReject.Configure();
                MessageMessageResponder.Send(repositorySession!, orxAmendReject);
                return;
            }

            feed.AmendOrderRequest(previousOrder, orxAmendRequest.Amendment!);
        }
        else
        {
            var orxAmendReject = Recycler.Borrow<OrxAmendReject>();
            orxAmendReject.Configure();
            MessageMessageResponder.Send(repositorySession!, orxAmendReject);
        }
    }

    private void OnCancel(OrxCancelRequest message, object? context, IConversation? repositorySession)
    {
        var order = orderSessionTracker.FindOrderFromSessionId(message.OrderId!.VenueAdapterOrderId!
            , repositorySession!);
        if (order != null) feed.CancelOrder(order);
    }

    private void OnStatus(string? feedName, bool feedStatus)
    {
        var orxStatusMessage = Recycler.Borrow<OrxStatusMessage>();
        orxStatusMessage.Configure(feedStatus ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
            feedName ?? "general line", Recycler);
        MessageMessageResponder.Broadcast(orxStatusMessage);
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
            var orxOrderUpdate = Recycler.Borrow<OrxOrderUpdate>();
            orxOrderUpdate.CopyFrom(orderUpdate);
            MessageMessageResponder.Send(orderUpdate.Order.OrderPublisher.UnderlyingSession,
                orxOrderUpdate);
            orxOrderUpdate.DecrementRefCount();
        }
    }

    private void OnAmendOrderResponse(IOrderAmendResponse amendResponse)
    {
        if (!amendResponse.Order!.IsPending() || amendResponse.Order!.Status == OrderStatus.Unknown)
            orderSessionTracker.UnregisterOrderWithSession(amendResponse.Order!);

        if (amendResponse.Order?.OrderPublisher?.UnderlyingSession != null)
        {
            var orxOrderAmendResponse = Recycler.Borrow<OrxOrderAmendResponse>();
            orxOrderAmendResponse.CopyFrom(amendResponse);
            MessageMessageResponder.Send(amendResponse.Order.OrderPublisher.UnderlyingSession,
                orxOrderAmendResponse);
            amendResponse.DecrementRefCount();
        }
    }

    private void OnVenueOrder(IVenueOrderUpdate venueOrderUpdate)
    {
        var orderSession = orderSessionTracker.FindSessionFromOrderId(venueOrderUpdate.VenueOrder!.OrderId!);
        if (orderSession != null)
        {
            var orxOrxVenueOrderUpdate = Recycler.Borrow<OrxVenueOrderUpdate>();
            orxOrxVenueOrderUpdate.CopyFrom(venueOrderUpdate);
            MessageMessageResponder.Send(orderSession, orxOrxVenueOrderUpdate);
        }
    }

    private void OnExecution(IExecutionUpdate executionUpdate)
    {
        if (executionUpdate.Execution?.OrderId != null)
        {
            var orderSession = orderSessionTracker.FindSessionFromOrderId(executionUpdate.Execution.OrderId);
            var orxExecutionUpdate = Recycler.Borrow<OrxExecutionUpdate>();
            orxExecutionUpdate.CopyFrom(executionUpdate);
            MessageMessageResponder.Send(orderSession!, new OrxExecutionUpdate(executionUpdate));
        }
    }
}
