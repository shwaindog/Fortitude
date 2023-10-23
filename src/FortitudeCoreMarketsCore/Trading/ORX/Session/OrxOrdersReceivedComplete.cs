#region

using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Session;

public class OrxOrdersReceivedComplete : OrxTradingMessage
{
    public override uint MessageId => (uint)TradingMessageIds.OrderReplayComplete;

    [OrxOptionalField(10)] public OrxAccountEntry? OrxAccount { get; set; }
}
