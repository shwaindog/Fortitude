using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Chronometry;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeIO.Sockets;
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

namespace FortitudeMarketsCore.Trading.ORX.Subscription
{
    public class OrxHistoricalTradesClient : OrxAuthenticatedClient, ITradingHistoryService
    {
        public event Action<IReplayMessage> ReplayMessage;
        public event Action<IOrderUpdate> OnPastOrder;
        public event Action<IExecutionUpdate> OnPastExecution;
        protected readonly Dictionary<string, IOrder> PastOrders = new Dictionary<string, IOrder>();

        protected readonly Dictionary<string, List<OrxExecutionUpdate>> PastExecutions =
            new Dictionary<string, List<OrxExecutionUpdate>>();
        protected bool HaveReceivedAllOrders;
        protected bool HaveReceivedAllExecutions;

        public OrxHistoricalTradesClient(IOrxSubscriber subscriber, string serverName, 
            ILoginCredentials loginCredentials, string defaultAccount) 
            : base(subscriber, serverName, loginCredentials, defaultAccount)
        {
            Subscriber.StreamToPublisher.RegisterSerializer<OrxGetOrderBookMessage>();
            Subscriber.StreamToPublisher.RegisterSerializer<OrxGetTradeBookMessage>();

            Subscriber.RegisterDeserializer<OrxReplayMessage>(HandleReplayMessage);
            Subscriber.RegisterDeserializer<OrxOrdersReceivedComplete>(HandleOrderReplayFinished);
            Subscriber.RegisterDeserializer<OrxExecutionsReceivedComplete>(HandleExecutionReplayFinished);
        }
        
        public void FetchHistory(DateTime from, DateTime to)
        {
            HaveReceivedAllOrders = false;
            HaveReceivedAllExecutions = false;
            Subscriber.Send(new OrxGetOrderBookMessage(DefaultAccount,
                (TimeContext.UtcNow - from).TotalSeconds > 5));
            Subscriber.Send(new OrxGetTradeBookMessage(DefaultAccount));
        }

        private void HandleReplayMessage(OrxReplayMessage update, object context, ISession cx)
        {
            if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            {
                HandleOrderReplay(update.PastOrder, context, cx);
            }
            else if (update.ReplayMessageType == ReplayMessageType.PastOrder)
            {
                HandleExecutionReplay(update.PastExecutionUpdate, context, cx);
            }

            OnReplayMessage(update);
        }
        private void HandleOrderReplay(OrxOrderUpdate update, object context, ISession cx)
        {
            var order = new Order(update.Order);

            lock (PastOrders)
            {
                if (!PastOrders.TryGetValue(update.Order.OrderId.ClientOrderId.ToString(), out var _))
                    PastOrders.Add(update.Order.OrderId.ClientOrderId.ToString(), order);
                else PastOrders[update.Order.OrderId.ClientOrderId.ToString()] = order;
            }
        }

        private void HandleOrderReplayFinished(OrxOrdersReceivedComplete orxOrder, object context, ISession cx)
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
                        {
                            exeMsgcallbacks?.Invoke(new ExecutionUpdate(execution, ExecutionUpdateType.Created, null, null,
                                TimeContext.UtcNow));
                        }
                    }
                }
            }
        }

        private IEnumerable<IExecution> ConvertExecutionMessageToExecution(
            IEnumerable<OrxExecutionUpdate> receivedExecutionMessages, KeyValuePair<string, IOrder> basicOrderKvP)
        {
            foreach (var exeMsg in receivedExecutionMessages
                .Where(execMsg => execMsg.Execution.OrderId.ClientOrderId.ToString() == basicOrderKvP.Key))
            {
                if (PastOrders.TryGetValue(exeMsg.Execution.OrderId.ClientOrderId.ToString(), out var _))
                    basicOrderKvP.Value.Product.RegisterExecution(exeMsg.Execution);
                yield return exeMsg.Execution;
            }
        }

        private void HandleExecutionReplay(OrxExecutionUpdate update, object context, ISession cx)
        {
            lock (PastExecutions)
            {
                if (!PastExecutions.TryGetValue(update.Execution.OrderId.ClientOrderId.ToString(), out var ordersExecutions))
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

        private void HandleExecutionReplayFinished(OrxOrdersReceivedComplete orxOrder, object context, ISession cx)
        {
            HaveReceivedAllExecutions = true;
            PublishPastOrdersAndExecutions(HaveReceivedAllOrders);
        }

        private void OnReplayMessage(OrxReplayMessage replayMessage)
        {
            ReplayMessage?.Invoke(replayMessage);
        }
    }
}