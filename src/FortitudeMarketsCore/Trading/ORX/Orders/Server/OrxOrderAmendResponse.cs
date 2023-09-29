using System;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server
{
    public class OrxOrderAmendResponse : OrxOrderUpdate, IOrderAmendResponse
    {
        public override uint MessageId => (uint) TradingMessageIds.Amend;

        public OrxOrderAmendResponse()
        {
        }

        public OrxOrderAmendResponse(IOrderAmendResponse toClone) : base(toClone)
        {
            AmendType = toClone.AmendType;
            OldOrderId = toClone.OldOrderId != null ? new OrxOrderId(toClone.OldOrderId) : null;
        }

        public OrxOrderAmendResponse(OrxOrder order, OrderUpdateEventType reason, DateTime adapterUpdateTime, 
            AmendType amendType, OrxOrderId oldOrderId) : base(order, reason, adapterUpdateTime)
        {
            AmendType = amendType;
            OldOrderId = oldOrderId;
        }

        [OrxMandatoryField(20)]
        public AmendType AmendType { get; set; }
        [OrxOptionalField(21)]
        public OrxOrderId OldOrderId { get; set; }

        IOrderId IOrderAmendResponse.OldOrderId
        {
            get => OldOrderId;
            set => OldOrderId = (OrxOrderId)value;
        }

        public override void CopyFrom(IVersionedMessage source, IRecycler orxRecyclingFactory)
        {
            base.CopyFrom(source, orxRecyclingFactory);
            if (source is IOrderAmendResponse amendResponse)
            {
                AmendType = amendResponse.AmendType;
                if (amendResponse.OldOrderId != null)
                {
                    var newOrderId = orxRecyclingFactory.Borrow<OrxOrderId>();
                    newOrderId.CopyFrom(amendResponse.OldOrderId, orxRecyclingFactory);
                    OldOrderId = newOrderId;
                }
            }
        }

        public new IOrderAmendResponse Clone()
        {
            return new OrxOrderAmendResponse(this);
        }
    }
}