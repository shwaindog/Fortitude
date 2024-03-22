#region

using FortitudeCommon.Chronometry;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Replay;
using FortitudeMarketsCore.Trading.Executions;
using FortitudeMarketsCore.Trading.Orders;
using FortitudeMarketsCore.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Replay;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Subscription;

public class OrxHistoricalTradesClient : OrxAuthenticatedClient, ITradingHistoryService
{
    protected readonly Dictionary<string, List<OrxExecutionUpdate>> PastExecutions = new();
    protected readonly Dictionary<string, IOrder> PastOrders = new();
    protected bool HaveReceivedAllExecutions;
    protected bool HaveReceivedAllOrders;

    public OrxHistoricalTradesClient(IOrxMessageRequester messageRequester, string serverName,
        ILoginCredentials loginCredentials, string defaultAccount)
        : base(messageRequester, serverName, loginCredentials, defaultAccount)
    {
        MessageRequester.SerializationRepository.RegisterSerializer<OrxGetOrderBookMessage>();
        MessageRequester.SerializationRepository.RegisterSerializer<OrxGetTradeBookMessage>();

        MessageRequester.DeserializationRepository.RegisterDeserializer<OrxReplayMessage>(HandleReplayMessage);
        MessageRequester.DeserializationRepository.RegisterDeserializer<OrxOrdersReceivedComplete>(
            HandleOrderReplayFinished);
        MessageRequester.DeserializationRepository.RegisterDeserializer<OrxExecutionsReceivedComplete>(
            HandleExecutionReplayFinished);
    }

    public event Action<IReplayMessage>? ReplayMessage;

    public void FetchHistory(DateTime from, DateTime to)
    {
        HaveReceivedAllOrders = false;
        HaveReceivedAllExecutions = false;
        MessageRequester.Send(new OrxGetOrderBookMessage(DefaultAccount,
            (TimeContext.UtcNow - from).TotalSeconds > 5));
        MessageRequester.Send(new OrxGetTradeBookMessage(DefaultAccount));
    }

    public event Action<IOrderUpdate>? OnPastOrder;
    public event Action<IExecutionUpdate>? OnPastExecution;

    private void HandleReplayMessage(OrxReplayMessage update, object? context, IConversation? cx)
    {
        if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            HandleOrderReplay(update.PastOrder!, context, cx);
        else if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            HandleExecutionReplay(update.PastExecutionUpdate!, context, cx);

        OnReplayMessage(update);
    }

    private void HandleOrderReplay(OrxOrderUpdate update, object? context, IConversation? cx)
    {
        var order = new Order(update.Order!);

        lock (PastOrders)
        {
            if (!PastOrders.TryGetValue(update.Order!.OrderId.ClientOrderId.ToString(), out var _))
                PastOrders.Add(update.Order.OrderId.ClientOrderId.ToString(), order);
            else PastOrders[update.Order.OrderId.ClientOrderId.ToString()] = order;
        }
    }

    private void HandleOrderReplayFinished(OrxOrdersReceivedComplete orxOrder, object? context, IConversation? cx)
    {
        HaveReceivedAllOrders = true;
        PublishPastOrdersAndExecutions(HaveReceivedAllExecutions);
    }

    private void PublishPastOrdersAndExecutions(bool shouldPublishPastOrdersAndExecutions)
    {
        if (shouldPublishPastOrdersAndExecutions)
        {
            var orderCallbacks = OnPastOrder;
            if (orderCallbacks != null)
            {
                var allExecutions = PastExecutions.SelectMany(pe => pe.Value).ToList();
                var exeMsgcallbacks = OnPastExecution;
                foreach (var basicOrderKvP in PastOrders)
                {
                    var executions = ConvertExecutionMessageToExecution(allExecutions, basicOrderKvP).ToList();
                    orderCallbacks(new OrderUpdate(basicOrderKvP.Value, OrderUpdateEventType.OrderRejected,
                        TimeContext.UtcNow));
                    foreach (var execution in executions)
                        exeMsgcallbacks?.Invoke(new ExecutionUpdate(execution, ExecutionUpdateType.Created, null, null,
                            TimeContext.UtcNow));
                }
            }
        }
    }

    private IEnumerable<IExecution> ConvertExecutionMessageToExecution(
        IEnumerable<OrxExecutionUpdate> receivedExecutionMessages, KeyValuePair<string, IOrder> basicOrderKvP)
    {
        foreach (var exeMsg in receivedExecutionMessages
                     .Where(execMsg => execMsg.Execution!.OrderId.ClientOrderId.ToString() == basicOrderKvP.Key))
        {
            if (PastOrders.TryGetValue(exeMsg.Execution!.OrderId.ClientOrderId.ToString(), out var _))
                basicOrderKvP.Value.Product!.RegisterExecution(exeMsg.Execution);
            yield return exeMsg.Execution;
        }
    }

    private void HandleExecutionReplay(OrxExecutionUpdate update, object? context, IConversation? cx)
    {
        lock (PastExecutions)
        {
            if (!PastExecutions.TryGetValue(update.Execution!.OrderId.ClientOrderId.ToString()
                    , out var ordersExecutions))
            {
                ordersExecutions = new List<OrxExecutionUpdate>();
                PastExecutions.Add(update.Execution.OrderId.ClientOrderId.ToString(), ordersExecutions);
                ordersExecutions.Add(update);
            }
            else
            {
                PastExecutions[update.Execution.OrderId.ClientOrderId.ToString()].Add(update);
            }
        }
    }

    private void HandleExecutionReplayFinished(OrxOrdersReceivedComplete orxOrder, object? context, IConversation? cx)
    {
        HaveReceivedAllExecutions = true;
        PublishPastOrdersAndExecutions(HaveReceivedAllOrders);
    }

    private void OnReplayMessage(OrxReplayMessage replayMessage)
    {
        ReplayMessage?.Invoke(replayMessage);
    }
}
