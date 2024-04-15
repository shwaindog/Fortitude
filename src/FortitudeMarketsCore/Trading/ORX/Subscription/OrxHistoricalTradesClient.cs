#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Protocols.Serdes.Binary;
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

    private IMessageStreamDecoder? messageStreamDecoder;

    public OrxHistoricalTradesClient(IOrxClientRequester clientRequester, string serverName,
        ILoginCredentials loginCredentials, string defaultAccount)
        : base(clientRequester, serverName, loginCredentials, defaultAccount)
    {
        ClientRequester.SerializationRepository.RegisterSerializer<OrxGetOrderBookMessage>();
        ClientRequester.SerializationRepository.RegisterSerializer<OrxGetTradeBookMessage>();

        clientRequester.Connected += () =>
        {
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxReplayMessage>()
                .AddDeserializedNotifier(
                    new PassThroughDeserializedNotifier<OrxReplayMessage>($"{nameof(OrxHistoricalTradesClient)}.{nameof(HandleReplayMessage)}"
                        , HandleReplayMessage));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxOrdersReceivedComplete>()
                .AddDeserializedNotifier(
                    new PassThroughDeserializedNotifier<OrxOrdersReceivedComplete>(
                        $"{nameof(OrxHistoricalTradesClient)}.{nameof(HandleOrderReplayFinished)}", HandleOrderReplayFinished));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxExecutionsReceivedComplete>()
                .AddDeserializedNotifier(
                    new PassThroughDeserializedNotifier<OrxExecutionsReceivedComplete>(
                        $"{nameof(OrxHistoricalTradesClient)}.{nameof(HandleExecutionReplayFinished)}", HandleExecutionReplayFinished));
        };
    }

    public event Action<IReplayMessage>? ReplayMessage;

    public void FetchHistory(DateTime from, DateTime to)
    {
        HaveReceivedAllOrders = false;
        HaveReceivedAllExecutions = false;
        ClientRequester.Send(new OrxGetOrderBookMessage(DefaultAccount,
            (TimeContext.UtcNow - from).TotalSeconds > 5));
        ClientRequester.Send(new OrxGetTradeBookMessage(DefaultAccount));
    }

    public event Action<IOrderUpdate>? OnPastOrder;
    public event Action<IExecutionUpdate>? OnPastExecution;

    private void HandleReplayMessage(OrxReplayMessage update, MessageHeader messageHeader, IConversation cx)
    {
        if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            HandleOrderReplay(update.PastOrder!, messageHeader, cx);
        else if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            HandleExecutionReplay(update.PastExecutionUpdate!, messageHeader, cx);

        OnReplayMessage(update);
    }

    private void HandleOrderReplay(OrxOrderUpdate update, MessageHeader messageHeader, IConversation cx)
    {
        var order = new Order(update.Order!);

        lock (PastOrders)
        {
            if (!PastOrders.TryGetValue(update.Order!.OrderId.ClientOrderId.ToString(), out var _))
                PastOrders.Add(update.Order.OrderId.ClientOrderId.ToString(), order);
            else PastOrders[update.Order.OrderId.ClientOrderId.ToString()] = order;
        }
    }

    private void HandleOrderReplayFinished(OrxOrdersReceivedComplete orxOrder, MessageHeader messageHeader, IConversation cx)
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

    private void HandleExecutionReplay(OrxExecutionUpdate update, MessageHeader messageHeader, IConversation cx)
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

    private void HandleExecutionReplayFinished(OrxOrdersReceivedComplete orxOrder, MessageHeader messageHeader, IConversation cx)
    {
        HaveReceivedAllExecutions = true;
        PublishPastOrdersAndExecutions(HaveReceivedAllOrders);
    }

    private void OnReplayMessage(OrxReplayMessage replayMessage)
    {
        ReplayMessage?.Invoke(replayMessage);
    }
}
