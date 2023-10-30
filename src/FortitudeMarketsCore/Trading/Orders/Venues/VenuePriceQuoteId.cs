using System;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders.Venues
{
    public class VenuePriceQuoteId : IVenuePriceQuoteId
    {
        public VenuePriceQuoteId(IVenuePriceQuoteId toClone)
        {
            SourceId = toClone.SourceId;
            TickerId = toClone.TickerId;
            VenueQuoteTime = toClone.VenueQuoteTime;
            SourceRefId = toClone.SourceRefId;
            PQSequenceNumber = toClone.PQSequenceNumber;
        }

        public VenuePriceQuoteId(ushort sourceId, ushort tickerId, DateTime venueQuoteTime, 
                                uint sourceRefId, uint pqSequenceNumber)
        {
            SourceId = sourceId;
            TickerId = tickerId;
            VenueQuoteTime = venueQuoteTime;
            SourceRefId = sourceRefId;
            PQSequenceNumber = pqSequenceNumber;
        }

        public ushort SourceId { get; set; }
        public ushort TickerId { get; set; }
        public uint SourceRefId { get; set; }
        public uint PQSequenceNumber { get; set; }
        public DateTime VenueQuoteTime { get; set; }

        public IVenuePriceQuoteId Clone()
        {
            return new VenuePriceQuoteId(this);
        }
    }
}