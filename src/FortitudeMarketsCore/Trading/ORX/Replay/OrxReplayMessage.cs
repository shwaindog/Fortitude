using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Replay;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Replay
{
    public class OrxReplayMessage : OrxTradingMessage, IReplayMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.Replay;
        [OrxMandatoryField(10)]
        public ReplayMessageType ReplayMessageType { get; set; }
        [OrxOptionalField(11)]
        public OrxOrderUpdate PastOrder { get; set; }

        IOrderUpdate IReplayMessage.PastOrder
        {
            get => PastOrder;
            set => PastOrder = (OrxOrderUpdate)value;
        }

        [OrxOptionalField(12)]
        public OrxExecutionUpdate PastExecutionUpdate { get; set; }

        IExecutionUpdate IReplayMessage.PastExecutionUpdate
        {
            get => PastExecutionUpdate;
            set => PastExecutionUpdate = (OrxExecutionUpdate)value;
        }
    }
}