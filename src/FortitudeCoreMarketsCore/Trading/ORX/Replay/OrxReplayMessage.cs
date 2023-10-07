#region

using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Replay;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Replay;

public class OrxReplayMessage : OrxTradingMessage, IReplayMessage
{
    [OrxOptionalField(11)] public OrxOrderUpdate? PastOrder { get; set; }

    [OrxOptionalField(12)] public OrxExecutionUpdate? PastExecutionUpdate { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.Replay;

    [OrxMandatoryField(10)] public ReplayMessageType ReplayMessageType { get; set; }

    IOrderUpdate? IReplayMessage.PastOrder
    {
        get => PastOrder;
        set => PastOrder = value as OrxOrderUpdate;
    }

    IExecutionUpdate? IReplayMessage.PastExecutionUpdate
    {
        get => PastExecutionUpdate;
        set => PastExecutionUpdate = value as OrxExecutionUpdate;
    }
}
