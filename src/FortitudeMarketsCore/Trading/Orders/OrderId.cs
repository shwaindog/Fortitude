using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

namespace FortitudeMarketsCore.Trading.Orders
{
    public class OrderId : IOrderId
    {
        public OrderId(IOrderId  toClone)
        {
            ClientOrderId = toClone.ClientOrderId;
            VenueClientOrderId = toClone.VenueClientOrderId != null 
                ? new MutableString(toClone.VenueClientOrderId) 
                : null;
            AdapterOrderId = toClone.AdapterOrderId;
            VenueAdapterOrderId = toClone.VenueAdapterOrderId != null 
                ? new MutableString(toClone.VenueAdapterOrderId) 
                : null ;
            ParentOrderId = toClone.ParentOrderId != null 
                ? new OrxOrderId(toClone.ParentOrderId) 
                : null;
            TrackingId = toClone.TrackingId;
        }

        public OrderId(long clientOrderId, string venueClientOrderId = null, long adapterOrderId = 0,
            string venueAdapterOrderId = null, IOrderId parentOrderId = null, string trackingId = null)
        : this(clientOrderId, (MutableString)venueClientOrderId, adapterOrderId, (MutableString)venueAdapterOrderId, 
            parentOrderId, (MutableString)trackingId)
        { }

        public OrderId(long clientOrderId, IMutableString venueClientOrderId = null, long adapterOrderId = 0,
            IMutableString venueAdapterOrderId = null, IOrderId parentOrderId = null, IMutableString trackingId = null)
        {
            ClientOrderId = clientOrderId;
            AdapterOrderId = adapterOrderId;
            VenueClientOrderId = venueClientOrderId;
            VenueAdapterOrderId = venueAdapterOrderId;
            ParentOrderId = parentOrderId;
            TrackingId = trackingId;
        }

        public long AdapterOrderId { get; set; }
        public IMutableString VenueAdapterOrderId { get; set; }
        public long ClientOrderId { get; set; }
        public IMutableString VenueClientOrderId { get; set; }
        public IOrderId ParentOrderId { get; set; }
        public IMutableString TrackingId { get; set; }

        public IOrderId Clone()
        {
            return new OrderId(this);
        }
    }
}