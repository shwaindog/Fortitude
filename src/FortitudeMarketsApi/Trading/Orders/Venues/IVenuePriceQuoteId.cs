#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenuePriceQuoteId : IReusableObject<IVenuePriceQuoteId>
{
    ushort SourceId { get; set; }
    ushort TickerId { get; set; }
    uint SourceRefId { get; set; }
    uint PQSequenceNumber { get; set; }
    DateTime VenueQuoteTime { get; set; }
}
