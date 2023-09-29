using System;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client
{
    public class OrxAmendRequest : OrxOrderSubmitRequest, IOrderAmendRequest
    {
        public override uint MessageId => (uint) TradingMessageIds.Amend;

        public OrxAmendRequest()
        {
        }

        public OrxAmendRequest(IOrderAmendRequest toClone) : base(toClone)
        {
            Amendment = new OrxOrderAmend(toClone.Amendment);
        }

        public OrxAmendRequest(OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
            DateTime originalAttemptTime, string tag, OrxOrderAmend amendment) 
            : base(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, tag)
        {
            Amendment = amendment;
        }

        [OrxMandatoryField(20)]
        public OrxOrderAmend Amendment { get; set; }

        IOrderAmend IOrderAmendRequest.Amendment
        {
            get => Amendment;
            set => Amendment = (OrxOrderAmend) value;
        }
    }
}
