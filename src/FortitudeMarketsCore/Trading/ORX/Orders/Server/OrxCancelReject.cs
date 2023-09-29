using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server
{
    public sealed class OrxCancelReject : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.CancelRejectResponse;

        [OrxMandatoryField(10)]
        public string Reason { get; set; }
        [OrxOptionalField(11)]
        public OrxOrderId OrderId { get; set; }
    }
}
