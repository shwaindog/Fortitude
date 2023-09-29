using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server
{
    public class OrxAmendReject : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.AmendReject;

        [OrxMandatoryField(10)]
        public string Reason { get; private set; }
        [OrxMandatoryField(11)]
        public OrxOrderId OrderId { get; set; }
    }
}
