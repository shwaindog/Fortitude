#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenuePriceQuoteId : ReusableObject<IVenuePriceQuoteId>, IVenuePriceQuoteId
{
    public VenuePriceQuoteId() { }

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

    public override void StateReset()
    {
        SourceId = 0;
        TickerId = 0;
        SourceRefId = 0;
        PQSequenceNumber = 0;
        VenueQuoteTime = DateTimeConstants.UnixEpoch;
        base.StateReset();
    }

    public override IVenuePriceQuoteId CopyFrom(IVenuePriceQuoteId source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = source.SourceId;
        TickerId = source.TickerId;
        SourceRefId = source.SourceRefId;
        PQSequenceNumber = source.PQSequenceNumber;
        VenueQuoteTime = source.VenueQuoteTime;
        return this;
    }

    public override IVenuePriceQuoteId Clone() =>
        Recycler?.Borrow<VenuePriceQuoteId>().CopyFrom(this) ?? new VenuePriceQuoteId(this);
}
