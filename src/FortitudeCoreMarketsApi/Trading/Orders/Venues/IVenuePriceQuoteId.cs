using System;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenuePriceQuoteId
    {
        ushort SourceId { get; set; }
        ushort TickerId { get; set; }
        uint SourceRefId { get; set; }
        uint PQSequenceNumber { get; set; }
        DateTime VenueQuoteTime { get; set; }
        IVenuePriceQuoteId Clone();
    }
}