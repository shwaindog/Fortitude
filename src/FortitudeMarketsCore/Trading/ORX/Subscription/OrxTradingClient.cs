#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsApi.Monitoring.Logging;
using FortitudeMarketsApi.Trading;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Subscription;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public class OrxTradingClient : OrxHistoricalTradesClient, ITradingFeedListener
{
    protected readonly Dictionary<string, IOrder> ActiveOrders = new();

    private readonly IAlertManager? alertMgr;

    protected readonly Dictionary<string, IOrderAmend> AmendingOrders = new();

    protected readonly bool CancelOnAmendReject;
    private readonly bool forwardAllMessages;
    private readonly ITradingFeedWatchdog? tradingFeedWatchdog;

    public OrxTradingClient(ISocketDispatcher socksDispatcher,
        IOSNetworkingController networkingController, ITradingServerConfig tradingServerConfig,
        ILoginCredentials loginCredentials, string defaultAccount, bool forwardAllMessages,
        ITradingFeedWatchdog? tradingFeedWatchdog = null,
        IAlertManager? alertMgr = null, bool cancelOnAmendReject = true, bool keepAlive = false)
        : base(new OrxTradingClientMessaging(socksDispatcher, networkingController
                , tradingServerConfig.ServerConnections!.First(),
                tradingServerConfig.Name!, 1, keepAlive), tradingServerConfig.Name!, loginCredentials,
            defaultAccount)
    {
        CancelOnAmendReject = cancelOnAmendReject;
        this.forwardAllMessages = forwardAllMessages;

        this.alertMgr = alertMgr;

        Subscriber.StreamToPublisher.RegisterSerializer<OrxOrderSubmitRequest>();
        Subscriber.StreamToPublisher.RegisterSerializer<OrxAmendRequest>();
        Subscriber.StreamToPublisher.RegisterSerializer<OrxCancelRequest>();

        Subscriber.RegisterDeserializer<OrxStatusMessage>(HandleStatusUpdate);
        Subscriber.RegisterDeserializer<OrxVenueOrderUpdate>(HandleVenueOrder);

        Subscriber.RegisterDeserializer<OrxSubmitReject>(HandleSubmitReject);
        Subscriber.RegisterDeserializer<OrxCancelReject>(HandleCancelReject);
        Subscriber.RegisterDeserializer<OrxAmendReject>(HandleAmendReject);
        Subscriber.RegisterDeserializer<OrxOrderUpdate>(HandleOrderUpdate);
        Subscriber.RegisterDeserializer<OrxOrderAmendResponse>(HandleOrderAmendResponse);
        Subscriber.RegisterDeserializer<OrxExecutionUpdate>(HandleExecution);

        Subscriber.RegisterDeserializer<OrxTickerMessage>((m, c, cx) =>
            Subscriber.Logger.Info("Instrument update: " + m));

        Subscriber.OnConnected += () =>
        {
            SupportedTimeInForce = tradingServerConfig.SupportedTimeInForce;
            SupportedVenueFeatures = tradingServerConfig.SupportedVenueFeatures;
        };
        Subscriber.OnConnected += OnConnected;
        Subscriber.OnDisconnecting += OnDisconnecting;
        Subscriber.OnDisconnected += () =>
        {
            SupportedTimeInForce = TimeInForce.None;
            SupportedVenueFeatures = VenueFeatures.None;
            PreTradeSupported = false;
            PreTradeAsFinal = false;
        };
        Subscriber.OnDisconnected += OnDisconnected;

        Subscriber.Connect();

        this.tradingFeedWatchdog = tradingFeedWatchdog;
        OrderIdGenFunc = prefix => IdGen.Next(ServerName);
    }

    public TimeInForce SupportedTimeInForce { get; private set; }
    public VenueFeatures SupportedVenueFeatures { get; private set; }

    protected bool PreTradeSupported { get; private set; }
    protected bool PreTradeAsFinal { get; private set; }

    public Func<IOrderSubmitRequest, string> OrderIdGenFunc { get; set; }

    public event Action<IOrderUpdate>? OrderUpdate;
#pragma warning disable 67
    public event Action<IOrderAmendResponse>? OrderAmendResponse;
#pragma warning restore 67
    public event Action<IExecutionUpdate>? Execution;

    public event Action? Closed;
    public event Action<IVenueOrderUpdate>? VenueOrderUpdated;

    #region IDisposable Members

    public void Dispose()
    {
        Subscriber.Disconnect();
        Closed?.Invoke();
    }

    #endregion

    public event Action<IOrder>? OrderAmend;

    private void CancelAfterAmendReject(IOrderId orderId)
    {
        if (CancelOnAmendReject)
        {
            Subscriber.Logger.Warn("Cancel on AmendReject enabled, will cancel the order.");
            Subscriber.Send(new OrxCancelRequest(new OrxOrderId(orderId)));
        }
    }

    public void SubmitOrderRequest(IOrderSubmitRequest orderSubmitRequest)
    {
        var order = orderSubmitRequest.OrderDetails!;
        if (!IsAvailable)
        {
            RaiseOrderError(order, "Cannot submit order: " + ServerName + " not available");
        }
        else if (tradingFeedWatchdog != null &&
                 !tradingFeedWatchdog.IsOrderValid(order, out var rejectionReason))
        {
            RaiseOrderError(order, $"Watchdog rejection: {rejectionReason}");
        }
        else
        {
            Subscriber.Logger.Info(new NewOrderLog((ISpotOrder)order.Product!));
            lock (ActiveOrders)
            {
                ActiveOrders.Add(order.OrderId.ClientOrderId.ToString(), order);
            }

            Subscriber.Send(BuildSubmitRequest(orderSubmitRequest));
            Subscriber.Logger.Info(order.OrderId.ClientOrderId + " Sent");
            order.Status = OrderStatus.PendingNew;
        }
    }

    public void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest)
    {
        if (order.Status == OrderStatus.PendingNew)
        {
            Subscriber.Logger.Warn("Cannot amend order, confirmation from exchange not received yet " + ServerName);
            return;
        }

        if (IsAvailable)
        {
            bool isActive;
            lock (ActiveOrders)
            {
                isActive = ActiveOrders.ContainsKey(order.OrderId.ClientOrderId.ToString());
            }

            if (!isActive)
            {
                RaiseOrderError(order, "Cannot amend order, not found as active on " + ServerName);
                return;
            }

            lock (AmendingOrders)
            {
                if (AmendingOrders.ContainsKey(order.OrderId.ClientOrderId.ToString()))
                {
                    Subscriber.Logger.Warn("Cannot execute amend on " + ServerName +
                                           " as the order is already waiting for an amend reply.");
                    return;
                }

                AmendingOrders.Add(order.OrderId.ClientOrderId.ToString(), amendOrderRequest);
            }

            if ((SupportedVenueFeatures & VenueFeatures.Amends) > 0)
            {
                var amendSize = amendOrderRequest.NewQuantity;
                if (amendOrderRequest.NewDisplaySize > 0 && (SupportedVenueFeatures & VenueFeatures.IceBerg) == 0)
                    amendSize = Math.Min(amendOrderRequest.NewDisplaySize, amendSize);
                if (order.Product!.RequiresAmendment(amendOrderRequest))
                {
                    Subscriber.Logger.Info(new AmendOrderLog(order, amendOrderRequest));
                    Subscriber.Send(new OrxAmendRequest(new OrxOrder(order), 0,
                        TimeContext.UtcNow,
                        TimeContext.UtcNow, null, new OrxOrderAmend(amendOrderRequest)));
                }
                else
                {
                    Subscriber.Logger.Info("No need to amend order " + order.OrderId.ClientOrderId +
                                           " on the exchange as nothing changes.");
                    Subscriber.Logger.Info(new OrderAmendLog((ISpotOrder)order.Product));
                    lock (AmendingOrders)
                    {
                        AmendingOrders.Remove(order.OrderId.ClientOrderId.ToString());
                    }

                    OnOrderAmend(order);
                }
            }
            else
            {
                Subscriber.Logger.Warn(ServerName + " cannot Amend order, will cancel.");
                CancelOrder(order);
            }
        }
        else
        {
            RaiseOrderError(order, "Cannot amend order: " + ServerName + " not available");
            Subscriber.Logger.Warn(new AbortedOrderLog((ISpotOrder)order.Product!));
            OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.OrderAmended, TimeContext.UtcNow));
        }
    }

    private void RaiseOrderError(IOrder order, string errorMsg)
    {
        RaiseOrderError(order, (MutableString)errorMsg);
    }

    private void RaiseOrderError(IOrder order, IMutableString errorMsg)
    {
        Subscriber.Logger.Warn(new AbortedOrderLog((ISpotOrder)order.Product!));
        OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.Error, TimeContext.UtcNow));
    }

    protected virtual OrxOrderSubmitRequest BuildSubmitRequest(IOrderSubmitRequest order)
    {
        if (MutableString.IsNullOrEmpty(order.OrderDetails?.Parties?.SellSide?.Portfolio?.Portfolio))
            order.OrderDetails!.Parties!.SellSide!.Portfolio!.Portfolio = DefaultAccount;
        return new OrxOrderSubmitRequest(order);
    }

    #region OrxClient Callbacks

    private void OnDisconnecting()
    {
        Subscriber.Logger.Info("Disconnection detected, will try to cancel sent orders.");
        for (var disconectionAttempt = 0; ActiveOrders.Count > 0 && disconectionAttempt < 5; disconectionAttempt++)
        {
            IEnumerable<IOrder> activeOrdersOnDisconnect;
            lock (ActiveOrders)
            {
                activeOrdersOnDisconnect = ActiveOrders.Values.ToList();
            }

            foreach (var order in activeOrdersOnDisconnect)
            {
                var sentTimeout = 10;
                while ((order.Status == OrderStatus.New || order.Status == OrderStatus.PendingNew)
                       && sentTimeout > 0)
                {
                    Subscriber.Logger.Info(
                        "Order {0} not acknowledged yet, waiting...", order.ToString());
                    sentTimeout--;
                    Thread.Sleep(20);
                }

                if (order.Status != OrderStatus.New
                    && order.Status != OrderStatus.PendingNew)
                    if (order.Status == OrderStatus.Dead)
                        Subscriber.Logger.Info(
                            "Order {0} is already Dead. No need to cancel it anymore.", order.ToString());
                    else CancelOrder(order);
            }

            if (ActiveOrders.Count > 0) Thread.Sleep(100);
            if (ActiveOrders.Count > 0)
                Subscriber.Logger.Info(
                    "Orders still not finished after disconectionAttempt {0} from Orx. There are still {1} " +
                    "orders active. We tried to cancel {2} of them.",
                    disconectionAttempt, ActiveOrders.Count, activeOrdersOnDisconnect.Count());
        }
    }

    private void OnDisconnected()
    {
        var wasAvailable = IsLoggedIn && IsServiceAvailable;
        IsLoggedIn = false;
        IsServiceAvailable = false;
        if (wasAvailable)
        {
            Subscriber.Logger.Info(ServerName + " Adapter Down");
            NotifyStatusUpdateHandlers(null, IsAvailable);
        }

        lock (Statuses)
        {
            foreach (var kv in Statuses)
                if (kv.Value)
                {
                    Subscriber.Logger.Info(ServerName + ":" + kv.Key + " Adapter Down");
                    NotifyStatusUpdateHandlers(kv.Key, false);
                }
        }

        lock (Statuses)
        {
            Statuses.Clear();
        }

        HandleStatusFailure();
    }

    #endregion

    #region TradingFeed Members

    public event Action<string?, bool>? FeedStatusUpdate
    {
        add
        {
            AppendStatusHandler(value);
            lock (Statuses)
            {
                foreach (var kv in Statuses) value?.Invoke(kv.Key, kv.Value);
            }
        }
        remove => RemoveStatusHandler(value);
    }

    private void HandleStatusFailure()
    {
        if (!IsAvailable)
        {
            foreach (var order in ActiveOrders.Values)
            {
                Subscriber.Logger.Info(new OrderUpdateLog((ISpotOrder)order.Product!));
                OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.Error, TimeContext.UtcNow));
            }

            ActiveOrders.Clear();
        }
    }

    protected void OnOrderUpdate(IOrderUpdate orderUpdate)
    {
        OrderUpdate?.Invoke(orderUpdate);
    }

    protected void OnOrderAmend(IOrder order)
    {
        OrderAmend?.Invoke(order);
    }

    protected void OnVenueOrderUpdate(IVenueOrderUpdate venueOrderUpdate)
    {
        VenueOrderUpdated?.Invoke(venueOrderUpdate);
    }

    private void NotifyExecution(IExecutionUpdate executionUpdate)
    {
        Subscriber.Logger.Info(new ExecutionLog(executionUpdate.Execution!));
        Logger.Info(executionUpdate);
        Execution?.Invoke(executionUpdate);
    }

    public void CancelOrder(IOrder order)
    {
        if (IsAvailable && order != null)
        {
            Subscriber.Logger.Info(new CancelOrderLog(order));
            Subscriber.Send(new OrxCancelRequest(new OrxOrderId(order.OrderId)));
        }
    }

    public void SuspendOrder(IOrder order) { }

    public void ResumeOrder(IOrder order) { }

    #endregion

    #region TradingFeed Callbacks

    protected readonly Dictionary<string, bool> Statuses = new();

    private void HandleStatusUpdate(OrxStatusMessage message, object? context, ISession? cx)
    {
        if (message.ExchangeName == "general line")
        {
            Subscriber.Logger.Info(ServerName + " Adapter " + message.ExchangeStatus);
            IsServiceAvailable = message.ExchangeStatus == OrxExchangeStatus.Up;
            NotifyStatusUpdateHandlers(null, IsAvailable);
            HandleStatusFailure();
        }
        else
        {
            Subscriber.Logger.Info(ServerName + ":" + message.ExchangeName + " Adapter " + message.ExchangeStatus);
            var status = message.ExchangeStatus == OrxExchangeStatus.Up;
            lock (Statuses)
            {
                Statuses[message.ExchangeName!.ToString()] = status;
            }

            NotifyStatusUpdateHandlers(message.ExchangeName.ToString(), status);
        }
    }

    private void HandleSubmitReject(OrxSubmitReject message, object? context, ISession? cx)
    {
        IOrder? order;
        lock (ActiveOrders)
        {
            ActiveOrders.TryGetValue(message.OrderId!.ClientOrderId.ToString(), out order);
            ActiveOrders.Remove(message.OrderId.ClientOrderId.ToString());
        }

        if (order == null)
        {
            Subscriber.Logger.Warn("SubmitReject received for unknown order (Id=" + message.OrderId.ClientOrderId +
                                   ")");
            return;
        }

        Subscriber.Logger.Info(new OrderUpdateLog((ISpotOrder)order.Product!));
        OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.OrderRejected, TimeContext.UtcNow));
    }

    private void HandleCancelReject(OrxCancelReject message, object? context, ISession? cx) { }

    private void HandleAmendReject(OrxAmendReject message, object? context, ISession? cx)
    {
        Subscriber.Logger.Warn("AmendReject received for order Id={0}: {1}", message.OrderId, message.Reason);
        lock (AmendingOrders)
        {
            AmendingOrders.Remove(message.OrderId!.ClientOrderId.ToString());
        }

        CancelAfterAmendReject(message.OrderId);
    }

    private void HandleVenueOrder(OrxVenueOrderUpdate venueOrderUpdate, object? context, ISession? sessionRepo)
    {
        Subscriber.Logger.Info("*** OrderEvt {0} {1}", venueOrderUpdate.VenueOrder!.VenueOrderId, venueOrderUpdate);
        OnVenueOrderUpdate(venueOrderUpdate);
    }

    private void HandleOrderAmendResponse(OrxOrderAmendResponse amendResponse, object? context, ISession? cx)
    {
        HandleOrderUpdate(amendResponse, context, cx);
    }

    private void HandleOrderUpdate(OrxOrderUpdate update, object? context, ISession? cx)
    {
        var orderOrderId = update.Order!.OrderId.Clone();
        Subscriber.Logger.Info("*** OrderEvt {0} {1}", orderOrderId, update.OrderUpdateType);

        if (update.OrderUpdateType == OrderUpdateEventType.OrderCancelSent)
        {
            Subscriber.Logger.Warn("Adapter received cancel for order Id={0} ForceID={1} {2}", orderOrderId);
            return;
        }

        if (update.OrderUpdateType == OrderUpdateEventType.OrderCancelNotSent
            || update.OrderUpdateType == OrderUpdateEventType.OrderCancelRejected)
        {
            var adapterOrderId = orderOrderId.AdapterOrderId;
            Subscriber.Logger.Warn("CancelReject received for order Id={0} ForceID={1} {2}", orderOrderId,
                adapterOrderId, update.Info);
        }

        IOrder? order;
        lock (ActiveOrders)
        {
            ActiveOrders.TryGetValue(orderOrderId.ClientOrderId.ToString(), out order);
        }

        if (order == null)
        {
            Subscriber.Logger.Warn("OrderUpdate received for unknown order (Id=" + orderOrderId + ")");
            return;
        }

        IOrderAmend? amend;
        lock (AmendingOrders)
        {
            if (AmendingOrders.TryGetValue(orderOrderId.ClientOrderId.ToString(), out amend))
                AmendingOrders.Remove(orderOrderId.ClientOrderId.ToString());
        }

        order.OrderId = orderOrderId;
        order.Product?.CopyFrom(update.Order.Product!);

        var oldStatus = order.Status;
        if (update.OrderUpdateType == OrderUpdateEventType.OrderUnsent
            || update.OrderUpdateType == OrderUpdateEventType.OrderRejected)
        {
            if (amend != null)
            {
                Subscriber.Logger.Warn("Unsent/reject received on amend message for order (Id=" + orderOrderId +
                                       "). Order is still considered active but amend failed.");
                Subscriber.Logger.Warn("Reject message is: " + update.Info);
                CancelAfterAmendReject(orderOrderId);
                return;
            }
        }
        else if (update.OrderUpdateType == OrderUpdateEventType.Error)
        {
            order.Product!.CopyFrom(update.Order.Product!);
        }
        else
        {
            order.Status = NormalLifecycleToOrderStatus(update.OrderUpdateType);
        }

        if (order.Status != OrderStatus.Dead)
        {
            if (oldStatus != order.Status)
            {
                Subscriber.Logger.Info(new OrderUpdateLog((ISpotOrder)order.Product!));
                OnOrderUpdate(update);
            }
            else if (amend != null)
            {
                order.Product!.ApplyAmendment(amend);
                Subscriber.Logger.Info(new OrderAmendLog((ISpotOrder)order.Product));
                OnOrderAmend(order.Clone());
            }
        }
        else
        {
            HandleOrderEol(update);
        }
    }

    protected virtual void HandleExecution(OrxExecutionUpdate update, object? context, ISession? cx)
    {
        if (update.ExecutionUpdateType == ExecutionUpdateType.Created) HandleExecution(update);
    }

    protected void HandleExecution(OrxExecutionUpdate update)
    {
        if (PreTradeAsFinal && update.Execution!.ExecutionStageType == ExecutionStageType.PreTrade)
        {
            var executionId = update.Execution.ExecutionId;
            Subscriber.Logger.Info("*** PenExec {0} {1} {2}", executionId,
                update.Execution.CumlativeQuantity, update.Execution.CumlativeVwapPrice);
            IOrder? order;
            lock (ActiveOrders)
            {
                ActiveOrders.TryGetValue(update.Execution.OrderId.ClientOrderId.ToString(), out order);
            }

            if (order != null)
            {
                if (PreTradeAsFinal)
                {
                    order.Product?.RegisterExecution(update.Execution.Clone());
                    NotifyExecution(update);
                    if (order.Product?.IsComplete ?? true) order.Status = OrderStatus.Dead;
                }

                if (order.Product?.IsComplete ?? true)
                    HandleOrderEol(new OrxOrderUpdate(order, OrderUpdateEventType.Execution, TimeContext.UtcNow));
            }
            else
            {
                Subscriber.Logger.Warn("PreTrade received for unknown order on " + ServerName + " (ExecutionID=" +
                                       update.Execution.Venue + " OrderID=" + update.Execution.OrderId + ")");
                alertMgr?.SendAlert("PreTrade received for unknown order on " + ServerName,
                    "ExecutionID=" + update.Execution.Venue + "\nOrderID=" + update.Execution.OrderId,
                    "Check for position mismatch or blotter discrepancies", AlertSeverity.High);
            }
        }

        if ((update.Execution?.Quantity ?? 0) > 0)
        {
            Subscriber.Logger.Info("*** AckExec {0} {1} {2}", update.Execution!.ExecutionId, update.Execution.Quantity,
                update.Execution.Quantity);
            IOrder? order;
            lock (ActiveOrders)
            {
                ActiveOrders.TryGetValue(update.Execution.OrderId.ClientOrderId.ToString(), out order);
            }

            if (order == null)
            {
                Subscriber.Logger.Warn("Execution received for unknown order on " + ServerName + " (ExecutionID=" +
                                       update.Execution.Venue + " OrderID=" + update.Execution.OrderId + ")");
                alertMgr?.SendAlert("Execution received for unknown order on " + ServerName,
                    "ExecutionID=" + update.Execution.Venue + "\nOrderID=" + update.Execution.OrderId,
                    "Check for position mismatch or blotter discrepancies", AlertSeverity.High);
                return;
            }

            if (order.Executions == null) order.Executions = new Trading.Executions.Executions();
            order.Executions.Add(update.Execution.Clone());
            order.Product!.RegisterExecution(update.Execution);
            NotifyExecution(update);

            if (order.Product.IsComplete) order.Status = OrderStatus.Dead;
            else if (order.Product.IsError)
                order.Product.Message = (MutableString)("Overfill on order (Id=" + update.Execution.ExecutionId +
                                                        "). Please check with the exchange");
            HandleOrderEol(new OrderUpdate(order, OrderUpdateEventType.Execution, TimeContext.UtcNow));
        }
    }

    private void HandleOrderEol(IOrderUpdate orderUpdate)
    {
        if (!orderUpdate.Order!.IsPending() && !orderUpdate.Order!.HasPendingExecutions())
        {
            Subscriber.Logger.Info(new OrderUpdateLog((ISpotOrder)orderUpdate.Order!.Product!));
            OnOrderUpdate(orderUpdate);
            lock (ActiveOrders)
            {
                ActiveOrders.Remove(orderUpdate.Order.OrderId.ClientOrderId.ToString());
            }
        }
    }

    private static OrderStatus NormalLifecycleToOrderStatus(OrderUpdateEventType orderEvent)
    {
        switch (orderEvent)
        {
            case OrderUpdateEventType.OrderActive:
                return OrderStatus.Active;
            case OrderUpdateEventType.OrderCancelled:
            case OrderUpdateEventType.OrderRejected:
            case OrderUpdateEventType.OrderDead:
            case OrderUpdateEventType.OrderUnsent:
                return OrderStatus.Dead;
            case OrderUpdateEventType.OrderSent:
                return OrderStatus.PendingNew;
            default:
                return OrderStatus.Unknown;
        }
    }

    #endregion
}
