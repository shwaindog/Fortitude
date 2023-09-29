using System;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues
{
    public class OrxVenueOrderUpdate : OrxTradingMessage, IVenueOrderUpdate
    {
        public override uint MessageId => (uint) TradingMessageIds.VenueOrderUpdate;

        public OrxVenueOrderUpdate()
        {
        }

        public OrxVenueOrderUpdate(IVenueOrderUpdate venueOrderUpdate)
        {
            VenueOrder = new OrxVenueOrder(venueOrderUpdate.VenueOrder);
            UpdateTime = venueOrderUpdate.UpdateTime;
            AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
            AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
            ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
        }

        public OrxVenueOrderUpdate(OrxVenueOrder venueOrder, DateTime updateTime, DateTime socketReceivedTime, 
            DateTime adapterProcessedTime, DateTime clientReceivedTime)
        {
            VenueOrder = venueOrder;
            UpdateTime = updateTime;
            AdapterSocketReceivedTime = socketReceivedTime;
            AdapterProcessedTime = adapterProcessedTime;
            ClientReceivedTime = clientReceivedTime;
        }

        [OrxMandatoryField(10)]
        public OrxVenueOrder VenueOrder { get; set; }
        IVenueOrder IVenueOrderUpdate.VenueOrder
        {
            get => VenueOrder;
            set => VenueOrder = (OrxVenueOrder)value;
        }

        [OrxMandatoryField(11)]
        public DateTime UpdateTime { get; set; }
        [OrxMandatoryField(12)]
        public DateTime AdapterSocketReceivedTime { get; set; }
        [OrxMandatoryField(13)]
        public DateTime AdapterProcessedTime { get; set; }
        public DateTime ClientReceivedTime { get; set; }

        public void CopyFrom(IVenueOrderUpdate venueOrderUpdate, IRecycler orxRecyclingFactory)
        {
            base.CopyFrom(venueOrderUpdate, orxRecyclingFactory);
            if (venueOrderUpdate.VenueOrder != null)
            {
                var orxVenueOrder = orxRecyclingFactory.Borrow<OrxVenueOrder>();
                orxVenueOrder.CopyFrom(venueOrderUpdate.VenueOrder, orxRecyclingFactory);
                VenueOrder = orxVenueOrder;
            }
            UpdateTime = venueOrderUpdate.UpdateTime;
            AdapterSocketReceivedTime = venueOrderUpdate.AdapterSocketReceivedTime;
            AdapterProcessedTime = venueOrderUpdate.AdapterProcessedTime;
            ClientReceivedTime = venueOrderUpdate.ClientReceivedTime;
        }

        public IVenueOrderUpdate Clone()
        {
            return new OrxVenueOrderUpdate(this);
        }
    }
}