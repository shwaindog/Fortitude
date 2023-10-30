#region

using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client;

public sealed class OrxCancelRequest : OrxTradingMessage
{
    public OrxCancelRequest(OrxOrderId adapterOrderId) => OrderId = adapterOrderId;

    public OrxCancelRequest() { }

    public override uint MessageId => (uint)TradingMessageIds.CancelRequest;

    [OrxMandatoryField(10)] public OrxOrderId? OrderId { get; set; }
}
