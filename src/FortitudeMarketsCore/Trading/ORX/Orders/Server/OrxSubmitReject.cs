using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server
{
    public sealed class OrxSubmitReject : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.SubmitRejectResponse;

        [OrxMandatoryField(10)]
        public string Reason { get; set; }
        [OrxMandatoryField(11)]
        public OrxOrderId OrderId { get; set; }
    }
}
