#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders.Server;
using FortitudeMarkets.Trading.ORX.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Publication;

public sealed class OrxTradingServer : OrxAuthenticationServer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OrxTradingServer));
    private readonly bool errorSupport;
    private readonly ITradingFeed feed;
    private readonly AdapterOrderSessionTracker orderSessionTracker;
    private readonly Dictionary<string, bool> statuses = new();
    private uint adapterOrderId;

    public OrxTradingServer(IOrxMessageResponder acceptorSession, ITradingFeed feed, bool errorSupport = false)
        : base(acceptorSession, TradingVersionInfo.CurrentVersion)
    {
        this.feed = feed;
        feed.Recycler = new Recycler();
        this.errorSupport = errorSupport;
        orderSessionTracker = new AdapterOrderSessionTracker(Recycler);

        var orxPublisher = acceptorSession;

        orxPublisher.SerializationRepository.RegisterSerializer<OrxStatusMessage>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxOrderUpdate>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxOrderAmendResponse>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxExecutionUpdate>();
        orxPublisher.SerializationRepository.RegisterSerializer<OrxVenueOrderUpdate>();
        acceptorSession.NewClient += OnNewTradingClientConversation;

        feed.FeedStatusUpdate += OnStatus;
        feed.OrderUpdate += OnOrderUpdated;
        feed.OrderAmendResponse += OnAmendOrderResponse;
        feed.Execution += OnExecution;
        feed.VenueOrderUpdated += OnVenueOrder;
    }

    public void Shutdown()
    {
        OnDisconnecting();
        AcceptorSession.Stop(CloseReason.Completed, "OrxTradingServer is closing");
    }

    public void ShutdownImmediate()
    {
        WaitForClientsToClose = false;
        AcceptorSession.Stop(CloseReason.Completed, "OrxTradingServer is closing no graceful shutdown");
    }

    private void OnNewTradingClientConversation(IConversationRequester cx)
    {
        var clientDecoder = (IOrxStreamDecoder)cx.StreamListener!.Decoder!;
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<OrxOrderSubmitRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<OrxOrderSubmitRequest>($"{nameof(OrxTradingServer)}.{nameof(OnSubmit)}", OnSubmit));
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<OrxCancelRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<OrxCancelRequest>($"{nameof(OrxTradingServer)}.{nameof(OnCancel)}", OnCancel));
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<OrxAmendRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<OrxAmendRequest>($"{nameof(OrxTradingServer)}.{nameof(AmendOrder)}", AmendOrder));
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

    protected override bool Validate(OrxLogonRequest request, MessageHeader messageHeader, IConversation conversation)
    {
        if (base.Validate(request, messageHeader, conversation))
        {
            var requestingConversation = (IConversationRequester)conversation;
            var orxUpStatusMessage = Recycler.Borrow<OrxStatusMessage>();
            orxUpStatusMessage.Configure(feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                "general line", Recycler);
            requestingConversation.Send(orxUpStatusMessage);
            lock (statuses)
            {
                foreach (var kv in statuses)
                {
                    var orxStatusMessage = Recycler.Borrow<OrxStatusMessage>();
                    orxStatusMessage.Configure(
                        kv.Value && feed.IsAvailable ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
                        kv.Key, Recycler);
                    requestingConversation.Send(orxStatusMessage);
                }
            }

            return true;
        }

        return false;
    }

    private void OnSubmit(OrxOrderSubmitRequest orderSubmitRequest, MessageHeader messageHeader, IConversation conversation)
    {
        var id = Interlocked.Increment(ref adapterOrderId);
        if (!Stopping)
        {
            var orxOrderPublisher = Recycler.Borrow<OrxOrderPublisher>();
            orxOrderPublisher.Configure((IConversationRequester)conversation, AcceptorSession, errorSupport);
            orderSubmitRequest.OrderDetails!.OrderPublisher = orxOrderPublisher;
            orderSubmitRequest.OrderDetails.OrderId.AdapterOrderId = id;

            feed.SubmitOrderRequest(orderSubmitRequest);
            if (!orderSubmitRequest.OrderDetails.IsInError())
            {
                var orxOrder = orderSubmitRequest.OrderDetails.AsOrxOrder.Clone();
                orderSessionTracker.RegisterOrderIdWithSession(orxOrder, conversation);
            }

            orxOrderPublisher.DecrementRefCount();
        }
        else
        {
            var conversationRequester = (IConversationRequester)conversation;
            var orxOrderStatusUpdate = Recycler.Borrow<OrxOrderStatusUpdate>();
            orxOrderStatusUpdate.Configure(orderSubmitRequest.OrderDetails!.OrderId, OrderStatus.Dead,
                OrderUpdateEventType.Error | OrderUpdateEventType.OrderRejected, Recycler);
            conversationRequester.Send(orxOrderStatusUpdate);
            orxOrderStatusUpdate.DecrementRefCount();
        }
    }

    private void AmendOrder(OrxAmendRequest orxAmendRequest, MessageHeader messageHeader, IConversation conversation)
    {
        var conversationRequester = (IConversationRequester)conversation;
        if (!Stopping)
        {
            var previousOrder = orderSessionTracker.FindOrderFromSessionId(
                orxAmendRequest.OrderDetails!.OrderId.AdapterOrderId!.Value, conversation);
            if (previousOrder == null)
            {
                var orxAmendReject = Recycler.Borrow<OrxAmendReject>();
                orxAmendReject.Configure();
                conversationRequester.Send(orxAmendReject);
                orxAmendReject.DecrementRefCount();
                return;
            }

            feed.AmendOrderRequest(previousOrder, orxAmendRequest.Amendment!);
        }
        else
        {
            var orxAmendReject = Recycler.Borrow<OrxAmendReject>();
            orxAmendReject.Configure();
            conversationRequester.Send(orxAmendReject);
        }
    }

    private void OnCancel(OrxCancelRequest message, MessageHeader messageHeader, IConversation repositorySession)
    {
        var order = orderSessionTracker.FindOrderFromSessionId(message.OrderId!.AdapterOrderId!.Value, repositorySession);
        if (order != null) feed.CancelOrder(order);
    }

    private void OnStatus(string? feedName, bool feedStatus)
    {
        var orxStatusMessage = Recycler.Borrow<OrxStatusMessage>();
        orxStatusMessage.Configure(feedStatus ? OrxExchangeStatus.Up : OrxExchangeStatus.Down,
            feedName ?? "general line", Recycler);
        AcceptorSession.Broadcast(orxStatusMessage);
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

    private void OnOrderUpdated(IOrderUpdate orderUpdate)
    {
        if (orderUpdate.Order?.OrderPublisher?.UnderlyingSession is { IsStarted: true })
        {
            if (orderUpdate is not OrxOrderUpdate orxOrderUpdate)
            {
                orxOrderUpdate = Recycler.Borrow<OrxOrderUpdate>();
                orxOrderUpdate.CopyFrom(orderUpdate);
            }

            orderUpdate.Order.OrderPublisher.UnderlyingSession.Send(orxOrderUpdate);
            if (orderUpdate is not OrxOrderUpdate) orxOrderUpdate.DecrementRefCount();
        }

        if (!orderUpdate.Order!.IsPending() || orderUpdate.Order!.Status == OrderStatus.Unknown)
            orderSessionTracker.UnregisterOrderWithSession(orderUpdate.Order!);
    }

    private void OnAmendOrderResponse(IOrderAmendResponse amendResponse)
    {
        if (!amendResponse.Order!.IsPending() || amendResponse.Order!.Status == OrderStatus.Unknown)
            orderSessionTracker.UnregisterOrderWithSession(amendResponse.Order!);

        if (amendResponse.Order?.OrderPublisher?.UnderlyingSession != null)
        {
            if (amendResponse is not OrxOrderAmendResponse orxOrderAmendResponse)
            {
                orxOrderAmendResponse = Recycler.Borrow<OrxOrderAmendResponse>();
                orxOrderAmendResponse.CopyFrom(amendResponse);
            }

            amendResponse.Order.OrderPublisher.UnderlyingSession.Send(orxOrderAmendResponse);
            if (amendResponse is not OrxOrderAmendResponse) orxOrderAmendResponse.DecrementRefCount();
        }
    }

    private void OnVenueOrder(IVenueOrderUpdate venueOrderUpdate)
    {
        var orderSession = orderSessionTracker.FindSessionFromOrderId(venueOrderUpdate.VenueOrder!.OrderId!);
        if (orderSession != null)
        {
            if (venueOrderUpdate is not OrxVenueOrderUpdate orxOrxVenueOrderUpdate)
            {
                orxOrxVenueOrderUpdate = Recycler.Borrow<OrxVenueOrderUpdate>();
                orxOrxVenueOrderUpdate.CopyFrom(venueOrderUpdate);
            }

            orderSession.Send(orxOrxVenueOrderUpdate);
            if (venueOrderUpdate is not OrxVenueOrderUpdate) orxOrxVenueOrderUpdate.DecrementRefCount();
        }
    }

    private void OnExecution(IExecutionUpdate executionUpdate)
    {
        if (executionUpdate.Execution?.OrderId != null)
        {
            if (executionUpdate is not OrxExecutionUpdate orxExecutionUpdate)
            {
                orxExecutionUpdate = Recycler.Borrow<OrxExecutionUpdate>();
                orxExecutionUpdate.CopyFrom(executionUpdate);
            }

            var orderSession = orderSessionTracker.FindSessionFromOrderId(executionUpdate.Execution.OrderId);
            orderSession!.Send(orxExecutionUpdate);
            if (executionUpdate is not OrxExecutionUpdate) orxExecutionUpdate.DecrementRefCount();
        }
    }
}
