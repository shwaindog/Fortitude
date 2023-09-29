using System;
using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.Orders.Venues
{
    public class VenueOrderUpdate : TradingMessage, IVenueOrderUpdate
    {
        public override uint MessageId => (uint) TradingMessageIds.VenueOrderUpdate;

        public VenueOrderUpdate(IVenueOrderUpdate toClone)
        {
            VenueOrder = toClone.VenueOrder.Clone();
            UpdateTime = toClone.UpdateTime;
            AdapterSocketReceivedTime = toClone.AdapterSocketReceivedTime;
            AdapterProcessedTime = toClone.AdapterProcessedTime;
            ClientReceivedTime = toClone.ClientReceivedTime;
        }

        public VenueOrderUpdate(IVenueOrder venueOrder, DateTime updateTime, DateTime socketReceivedTime, 
            DateTime adapterProcessedTime, DateTime? clientReceivedTime = null)
        {
            VenueOrder = venueOrder;
            UpdateTime = updateTime;
            AdapterSocketReceivedTime = socketReceivedTime;
            AdapterProcessedTime = adapterProcessedTime;
            ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
        }

        public IVenueOrder VenueOrder { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime AdapterSocketReceivedTime { get; set; }
        public DateTime AdapterProcessedTime { get; set; }
        public DateTime ClientReceivedTime { get; set; }
        public IVenueOrderUpdate Clone()
        {
            return new VenueOrderUpdate(this);
        }
    }
}