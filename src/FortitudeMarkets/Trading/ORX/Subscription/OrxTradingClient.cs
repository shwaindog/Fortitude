// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Alerting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeMarkets.Config.TradingConfig;
using FortitudeMarkets.Monitoring.Logging;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.CounterParties;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders.Server;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;
using FortitudeMarkets.Trading.ORX.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Subscription;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public class OrxTradingClient : OrxHistoricalTradesClient, ITradingFeedListener
{
    private new static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OrxTradingClient));

    protected readonly Dictionary<string, IOrder> ActiveOrders = new();

    private readonly IAlertManager? alertMgr;

    protected readonly Dictionary<string, IOrderAmend> AmendingOrders = new();

    protected readonly bool CancelOnAmendReject;
    private readonly   bool forwardAllMessages;

    private readonly ITradingFeedWatchdog? tradingFeedWatchdog;

    private IRecycler? recycler;

    public OrxTradingClient
    (ITradingServerConfig tradingServerConfig, ISocketDispatcherResolver dispatchResolver
      , string serverName,
        ILoginCredentials loginCredentials, uint defaultAccount, bool forwardAllMessages,
        ITradingFeedWatchdog? tradingFeedWatchdog = null,
        IAlertManager? alertMgr = null, bool cancelOnAmendReject = true)
        : base(OrxClientMessaging.BuildTcpRequester(tradingServerConfig.TradingServerConnectionConfig, dispatchResolver)
             , serverName, loginCredentials, defaultAccount)
    {
        CancelOnAmendReject     = cancelOnAmendReject;
        this.forwardAllMessages = forwardAllMessages;

        this.alertMgr = alertMgr;

        ClientRequester.SerializationRepository.RegisterSerializer<OrxOrderSubmitRequest>();
        ClientRequester.SerializationRepository.RegisterSerializer<OrxAmendRequest>();
        ClientRequester.SerializationRepository.RegisterSerializer<OrxCancelRequest>();

        ClientRequester.Connected += () =>
        {
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxStatusMessage>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxStatusMessage>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleStatusUpdate)}", HandleStatusUpdate));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxVenueOrderUpdate>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxVenueOrderUpdate>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleVenueOrder)}", HandleVenueOrder));

            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxSubmitReject>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxSubmitReject>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleSubmitReject)}", HandleSubmitReject));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxCancelReject>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxCancelReject>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleCancelReject)}", HandleCancelReject));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxAmendReject>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxAmendReject>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleAmendReject)}", HandleAmendReject));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxOrderUpdate>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxOrderUpdate>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleOrderUpdate)}", HandleOrderUpdate));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxOrderAmendResponse>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxOrderAmendResponse>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleOrderAmendResponse)}", HandleOrderAmendResponse));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxExecutionUpdate>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxExecutionUpdate>
                                   ($"{nameof(OrxTradingClient)}.{nameof(HandleExecution)}", HandleExecution));

            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxTickerMessage>()
                           .AddDeserializedNotifier
                               (new PassThroughDeserializedNotifier<OrxTickerMessage>
                                   ($"{nameof(OrxTradingClient)}.ConstructorCallback",
                                    (m, _, _) =>
                                        Logger.Info("Instrument update: " + m)));
        };


        ClientRequester.Connected += () =>
        {
            SupportedTimeInForce   = tradingServerConfig.SupportedTimeInForce;
            SupportedVenueFeatures = tradingServerConfig.SupportedVenueFeatures;
        };
        ClientRequester.Started       += OnStarted;
        ClientRequester.Disconnecting += OnDisconnecting;
        ClientRequester.Disconnected += () =>
        {
            SupportedTimeInForce   = TimeInForce.None;
            SupportedVenueFeatures = VenueFeatures.None;
            PreTradeSupported      = false;
            PreTradeAsFinal        = false;
        };
        ClientRequester.Disconnected += OnDisconnected;

        ClientRequester.Start();

        this.tradingFeedWatchdog = tradingFeedWatchdog;
        OrderIdGenFunc           = _ => IdGen.Next(ServerName);
    }

    public IRecycler Recycler
    {
        get => recycler ??= new Recycler();
        set => recycler = value;
    }

    public TimeInForce   SupportedTimeInForce   { get; private set; }
    public VenueFeatures SupportedVenueFeatures { get; private set; }

    protected bool PreTradeSupported { get; private set; }
    protected bool PreTradeAsFinal   { get; private set; }

    public Func<IOrderSubmitRequest, string> OrderIdGenFunc { get; set; }

    public event Action<IOrderUpdate>? OrderUpdate;
    #pragma warning disable 67
    public event Action<IOrderAmendResponse>? OrderAmendResponse;
    #pragma warning restore 67
    public event Action<IExecutionUpdate>? Execution;

    public event Action?                    Closed;
    public event Action<IVenueOrderUpdate>? VenueOrderUpdated;

    #region IDisposable Members

    public void Dispose()
    {
        ClientRequester.Stop();
        Closed?.Invoke();
    }

    #endregion

    public event Action<IOrder>? OrderAmend;

    private void CancelAfterAmendReject(IOrderId orderId)
    {
        if (CancelOnAmendReject)
        {
            Logger.Warn("Cancel on AmendReject enabled, will cancel the order.");
            ClientRequester.Send(new OrxCancelRequest(new OrxOrderId(orderId)));
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
            Logger.Info(new NewOrderLog((ISpotOrder)order));
            lock (ActiveOrders)
            {
                order.IncrementRefCount();
                Logger.Info("Registering order {0} as an active order.", order);
                ActiveOrders.Add(order.OrderId.ClientOrderId.ToString(), order);
            }

            var orxOrderSubmitRequest = BuildSubmitRequest(orderSubmitRequest);
            ClientRequester.Send(orxOrderSubmitRequest);
            orxOrderSubmitRequest.DecrementRefCount();
            if (orderSubmitRequest is not OrxOrderSubmitRequest) orderSubmitRequest.DecrementRefCount();
            Logger.Info(order.OrderId.ClientOrderId + " Sent");
            order.Status = OrderStatus.PendingNew;
        }
    }

    public void AmendOrderRequest(IOrder order, IOrderAmend amendOrderRequest)
    {
        if (order.Status == OrderStatus.PendingNew)
        {
            Logger.Warn(
                        "Cannot amend order, confirmation from exchange not received yet " + ServerName);
            return;
        }

        if (IsAvailable)
        {
            bool isActive;
            var  orderKey = order.OrderId.ClientOrderId.ToString();
            lock (ActiveOrders)
            {
                isActive = ActiveOrders.ContainsKey(orderKey);
            }

            if (!isActive)
            {
                RaiseOrderError(order, "Cannot amend order, not found as active on " + ServerName);
                return;
            }

            lock (AmendingOrders)
            {
                if (AmendingOrders.ContainsKey(orderKey))
                {
                    Logger.Warn("Cannot execute amend on " + ServerName +
                                " as the order is already waiting for an amend reply.");
                    return;
                }

                amendOrderRequest.IncrementRefCount();
                AmendingOrders.Add(orderKey, amendOrderRequest);
            }

            if ((SupportedVenueFeatures & VenueFeatures.Amends) > 0)
            {
                if (order.RequiresAmendment(amendOrderRequest))
                {
                    Logger.Info(new AmendOrderLog(order, amendOrderRequest));
                    var orxOrder = order switch
                                   {
                                       ISpotOrder spotOrder => new OrxSpotOrder(spotOrder)
                                     , _ => new OrxSpotOrder((ISpotOrder)order)
                                   };


                    ClientRequester.Send(new OrxAmendRequest(orxOrder, 0,
                                                             TimeContext.UtcNow,
                                                             TimeContext.UtcNow, null, new OrxOrderAmend(amendOrderRequest)));
                }
                else
                {
                    Logger.Info("No need to amend order " + order.OrderId.ClientOrderId +
                                " on the exchange as nothing changes.");
                    Logger.Info(new OrderAmendLog((ISpotOrder)order));
                    lock (AmendingOrders)
                    {
                        if (AmendingOrders.TryGetValue(orderKey, out var amendOrder))
                        {
                            AmendingOrders.Remove(orderKey);
                            amendOrder.DecrementRefCount();
                        }
                    }

                    OnOrderAmend(order);
                }
            }
            else
            {
                Logger.Warn(ServerName + " cannot Amend order, will cancel.");
                CancelOrder(order);
            }
        }
        else
        {
            RaiseOrderError(order, "Cannot amend order: " + ServerName + " not available");
            Logger.Warn(new AbortedOrderLog((ISpotOrder)order));
            OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.OrderAmended, TimeContext.UtcNow));
        }
    }

    private void RaiseOrderError(IOrder order, string errorMsg)
    {
        RaiseOrderError(order, (MutableString)errorMsg);
    }

    private void RaiseOrderError(IOrder order, IMutableString _)
    {
        Logger.Warn(new AbortedOrderLog((ISpotOrder)order));
        OnOrderUpdate(new OrderUpdate(order, OrderUpdateEventType.Error, TimeContext.UtcNow));
    }

    protected virtual OrxOrderSubmitRequest BuildSubmitRequest(IOrderSubmitRequest orderSubmitRequest)
    {
        if (orderSubmitRequest.OrderDetails!.Parties!.SellSide?.PortfolioId == null)
        {
            orderSubmitRequest.OrderDetails!.Parties!.SellSide ??= new OrxPartyPortfolio(DefaultAccount);

            orderSubmitRequest.OrderDetails!.Parties!.SellSide!.PortfolioId = DefaultAccount;
        }
        if (orderSubmitRequest is not OrxOrderSubmitRequest orxOrderSubmitRequest)
        {
            orxOrderSubmitRequest = Recycler.Borrow<OrxOrderSubmitRequest>();
            orxOrderSubmitRequest.CopyFrom(orderSubmitRequest);
        }

        return orxOrderSubmitRequest;
    }

    #region OrxClient Callbacks

    private void OnDisconnecting()
    {
        Logger.Info("Disconnection detected, will try to cancel sent orders.");
        for (var disconnectionAttempt = 0; ActiveOrders.Count > 0 && disconnectionAttempt < 5; disconnectionAttempt++)
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
                    Logger.Info(
                                "Order {0} not acknowledged yet, waiting...", order);
                    sentTimeout--;
                    Thread.Sleep(20);
                }

                if (order.Status != OrderStatus.New
                 && order.Status != OrderStatus.PendingNew)
                    if (order.Status == OrderStatus.Dead)
                        Logger.Info(
                                    "Order {0} is already Dead. No need to cancel it anymore.", order);
                    else
                        CancelOrder(order);
            }

            if (ActiveOrders.Count > 0) Thread.Sleep(100);
            if (ActiveOrders.Count > 0)
                Logger.Info(
                            "Orders still not finished after disconnectionAttempt {0} from Orx. There are still {1} " +
                            "orders active. We tried to cancel {2} of them.",
                            disconnectionAttempt, ActiveOrders.Count, activeOrdersOnDisconnect.Count());
        }
    }

    private void OnDisconnected()
    {
        var wasAvailable = IsLoggedIn && IsServiceAvailable;
        IsLoggedIn         = false;
        IsServiceAvailable = false;
        if (wasAvailable)
        {
            Logger.Info(ServerName + " Adapter Down");
            NotifyStatusUpdateHandlers(null, IsAvailable);
        }

        lock (Statuses)
        {
            foreach (var kv in Statuses)
                if (kv.Value)
                {
                    Logger.Info(ServerName + ":" + kv.Key + " Adapter Down");
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
                Logger.Info(new OrderUpdateLog((ISpotOrder)order));
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
        Logger.Info(new ExecutionLog(executionUpdate.Execution!));
        Logger.Info(executionUpdate);
        Execution?.Invoke(executionUpdate);
    }

    public void CancelOrder(IOrder order)
    {
        if (IsAvailable)
        {
            Logger.Info(new CancelOrderLog(order));
            ClientRequester.Send(new OrxCancelRequest(new OrxOrderId(order.OrderId)));
        }
    }

    public void SuspendOrder(IOrder order) { }

    public void ResumeOrder(IOrder order) { }

    #endregion

    #region TradingFeed Callbacks

    protected readonly Dictionary<string, bool> Statuses = new();

    private void HandleStatusUpdate(OrxStatusMessage message, MessageHeader messageHeader, IConversation cx)
    {
        if (message.ExchangeName == "general line")
        {
            Logger.Info(ServerName + " Adapter " + message.ExchangeStatus);
            IsServiceAvailable = message.ExchangeStatus == OrxExchangeStatus.Up;
            NotifyStatusUpdateHandlers(null, IsAvailable);
            HandleStatusFailure();
        }
        else
        {
            Logger.Info(ServerName + ":" + message.ExchangeName + " Adapter " +
                        message.ExchangeStatus);
            var status = message.ExchangeStatus == OrxExchangeStatus.Up;
            lock (Statuses)
            {
                Statuses[message.ExchangeName!.ToString()] = status;
            }

            NotifyStatusUpdateHandlers(message.ExchangeName.ToString(), status);
        }
    }

    private void HandleSubmitReject(OrxSubmitReject message, MessageHeader messageHeader, IConversation cx)
    {
        var orderKey = message.OrderId!.ClientOrderId.ToString();
        var order    = GetActiveOrder(orderKey);
        RemoveActiveOrder(orderKey);

        Logger.Info(new OrderUpdateLog((ISpotOrder)order!));
        OnOrderUpdate(new OrderUpdate(order!, OrderUpdateEventType.OrderRejected, TimeContext.UtcNow));
    }

    private void HandleCancelReject(OrxCancelReject message, MessageHeader messageHeader, IConversation cx) { }

    private void HandleAmendReject(OrxAmendReject message, MessageHeader messageHeader, IConversation cx)
    {
        Logger.Warn("AmendReject received for order Id={0}: {1}", message.OrderId, message.Reason);
        RemoveActiveOrder(message.OrderId!.ClientOrderId.ToString());

        CancelAfterAmendReject(message.OrderId);
    }

    private void RemoveActiveOrder(string clientOrderId)
    {
        lock (AmendingOrders)
        {
            if (ActiveOrders.TryGetValue(clientOrderId, out var toRemove))
            {
                ActiveOrders.Remove(clientOrderId);
                toRemove.DecrementRefCount();
            }
            else
            {
                Logger.Warn("Unexpected state tried to deactivate for unknown order (Id=" +
                            clientOrderId +
                            ")");
            }
        }
    }

    private void HandleVenueOrder(OrxVenueOrderUpdate venueOrderUpdate, MessageHeader messageHeader, IConversation sessionRepo)
    {
        Logger.Info("*** OrderEvt {0} {1}", venueOrderUpdate.VenueOrder!.VenueOrderId
                  , venueOrderUpdate);
        OnVenueOrderUpdate(venueOrderUpdate);
    }

    private void HandleOrderAmendResponse(OrxOrderAmendResponse amendResponse, MessageHeader messageHeader, IConversation cx)
    {
        HandleOrderUpdate(amendResponse, messageHeader, cx);
    }

    private void HandleOrderUpdate(OrxOrderUpdate update, MessageHeader messageHeader, IConversation cx)
    {
        var orderOrderId = update.Order!.OrderId.Clone();
        Logger.Info("*** OrderEvt {0} {1}", update.Order, update.OrderUpdateType);

        if (update.OrderUpdateType == OrderUpdateEventType.OrderCancelSent)
        {
            Logger.Warn("Adapter received cancel for order Id={0} ForceID={1} {2}", orderOrderId, update.Order, update.OrderUpdateType);
            return;
        }

        if (update.OrderUpdateType == OrderUpdateEventType.OrderCancelNotSent
         || update.OrderUpdateType == OrderUpdateEventType.OrderCancelRejected)
        {
            var adapterOrderId = orderOrderId.AdapterOrderId;
            Logger.Warn("CancelReject received for order Id={0} ForceID={1} {2}", orderOrderId,
                        adapterOrderId, update.Info);
        }

        var order = GetActiveOrder(orderOrderId.ClientOrderId.ToString());
        if (order == null)
        {
            Logger.Warn("OrderUpdate received for unknown order (Id=" + orderOrderId + ")");
            return;
        }

        Logger.Info("HandleOrderUpdate cached order is {0}", order);

        IOrderAmend? amend;
        lock (AmendingOrders)
        {
            var orderKey = orderOrderId.ClientOrderId.ToString();

            if (AmendingOrders.TryGetValue(orderKey, out amend))
            {
                AmendingOrders.Remove(orderKey);
                amend.DecrementRefCount();
            }
        }

        order.OrderId = orderOrderId;

        var oldStatus = order.Status;
        order.CopyFrom(update.Order);
        if (update.OrderUpdateType == OrderUpdateEventType.OrderUnsent
         || update.OrderUpdateType == OrderUpdateEventType.OrderRejected)
        {
            if (amend != null)
            {
                Logger.Warn("Unsent/reject received on amend message for order (Id=" + orderOrderId +
                            "). Order is still considered active but amend failed.");
                Logger.Warn("Reject message is: " + update.Info);
                CancelAfterAmendReject(orderOrderId);
                return;
            }
        }
        else if (update.OrderUpdateType == OrderUpdateEventType.Error)
        {
            order.CopyFrom(update.Order);
        }
        else
        {
            order.Status = NormalLifecycleToOrderStatus(update.OrderUpdateType);
        }

        if (oldStatus != order.Status)
        {
            Logger.Info(new OrderUpdateLog((ISpotOrder)order));
            OnOrderUpdate(update);
        }
        else if (amend != null)
        {
            order.ApplyAmendment(amend);
            Logger.Info(new OrderAmendLog((ISpotOrder)order));
            OnOrderAmend(order.Clone());
        }

        if (order.Status == OrderStatus.Dead) HandleOrderEol(update);
    }

    protected virtual void HandleExecution(OrxExecutionUpdate update, MessageHeader messageHeader, IConversation cx)
    {
        if (update.ExecutionUpdateType == ExecutionUpdateType.Created) HandleExecutionUpdate(update);
    }

    protected void HandleExecutionUpdate(OrxExecutionUpdate update)
    {
        var order = GetActiveOrder(update.Execution!.OrderId.ClientOrderId.ToString());
        if (PreTradeAsFinal && update.Execution!.ExecutionStageType == ExecutionStageType.PreTrade)
        {
            var executionId = update.Execution.ExecutionId;
            Logger.Info("*** PenExec {0} {1} {2}", executionId,
                        update.Execution.CumulativeQuantity, update.Execution.CumulativeVwapPrice);
            if (order != null)
            {
                if (PreTradeAsFinal)
                {
                    order.RegisterExecution(update.Execution.Clone());
                    NotifyExecution(update);
                    if (order.IsComplete) order.Status = OrderStatus.Dead;
                }

                if (order.IsComplete) HandleOrderEol(new OrxOrderUpdate(order, OrderUpdateEventType.Execution, TimeContext.UtcNow));
            }
            else
            {
                Logger.Warn("PreTrade received for unknown order on " + ServerName + " (ExecutionID=" +
                            update.Execution.Venue + " OrderID=" + update.Execution.OrderId + ")");
                alertMgr?.SendAlert("PreTrade received for unknown order on " + ServerName,
                                    "ExecutionID=" + update.Execution.Venue + "\nOrderID=" + update.Execution.OrderId,
                                    "Check for position mismatch or blotter discrepancies", AlertSeverity.High);
            }
        }

        if ((update.Execution?.Quantity ?? 0) > 0)
        {
            Logger.Info("*** AckExec {0} {1} {2}", update.Execution!.ExecutionId
                      , update.Execution.Quantity,
                        update.Execution.Quantity);
            if (order == null)
            {
                Logger.Warn("Execution received for unknown order on " + ServerName +
                            " (ExecutionID=" +
                            update.Execution.Venue + " OrderID=" + update.Execution.OrderId + ")");
                alertMgr?.SendAlert("Execution received for unknown order on " + ServerName,
                                    "ExecutionID=" + update.Execution.Venue + "\nOrderID=" + update.Execution.OrderId,
                                    "Check for position mismatch or blotter discrepancies", AlertSeverity.High);
                return;
            }

            order.Executions ??= new Trading.Executions.Executions();
            order.Executions.Add(update.Execution.Clone());
            order.RegisterExecution(update.Execution);
            NotifyExecution(update);

            if (order.IsComplete)
                order.Status = OrderStatus.Dead;
            else if (order.IsError)
                order.Message = (MutableString)("Overfill on order (Id=" + update.Execution.ExecutionId +
                                                "). Please check with the exchange");
            HandleOrderEol(new OrderUpdate(order, OrderUpdateEventType.Execution, TimeContext.UtcNow));
        }
    }

    private IOrder? GetActiveOrder(string clientOrderId)
    {
        IOrder? order;
        lock (ActiveOrders)
        {
            ActiveOrders.TryGetValue(clientOrderId, out order);
        }

        return order;
    }

    private void HandleOrderEol(IOrderUpdate orderUpdate)
    {
        if (!orderUpdate.Order!.IsPending() && !orderUpdate.Order!.HasPendingExecutions())
        {
            Logger.Info(new OrderUpdateLog((ISpotOrder)orderUpdate.Order!));
            RemoveActiveOrder(orderUpdate.Order!.OrderId.ClientOrderId.ToString());
        }
    }

    private static OrderStatus NormalLifecycleToOrderStatus(OrderUpdateEventType orderEvent)
    {
        switch (orderEvent)
        {
            case OrderUpdateEventType.OrderActive:
            case OrderUpdateEventType.Execution:
            case OrderUpdateEventType.ExecutionAmended:
            case OrderUpdateEventType.OrderAmended:
            case OrderUpdateEventType.OrderAccepted:
            case OrderUpdateEventType.OrderResumed:
            case OrderUpdateEventType.OrderAcknowledged:
                return OrderStatus.Active;
            case OrderUpdateEventType.OrderCancelled:
            case OrderUpdateEventType.OrderRejected:
            case OrderUpdateEventType.OrderDead:
            case OrderUpdateEventType.OrderUnsent:
                return OrderStatus.Dead;
            case OrderUpdateEventType.OrderCancelSent: return OrderStatus.Cancelling;
            case OrderUpdateEventType.OrderPaused:     return OrderStatus.Frozen;
            case OrderUpdateEventType.OrderSent:       return OrderStatus.PendingNew;
            default:                                   return OrderStatus.Unknown;
        }
    }

    #endregion
}
