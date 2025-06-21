#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Venues;

public class OrxVenuePriceQuoteId : ReusableObject<IVenuePriceQuoteId>, IVenuePriceQuoteId
{
    public OrxVenuePriceQuoteId() { }

    public OrxVenuePriceQuoteId(IVenuePriceQuoteId toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        SourceRefId = toClone.SourceRefId;
        PQSequenceNumber = toClone.PQSequenceNumber;
        VenueQuoteTime = toClone.VenueQuoteTime;
    }

    public OrxVenuePriceQuoteId(ushort sourceId, ushort tickerId, uint sourceRefId,
        uint pqSequenceNumber, DateTime venueQuoteTime)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        SourceRefId = sourceRefId;
        PQSequenceNumber = pqSequenceNumber;
        VenueQuoteTime = venueQuoteTime;
    }

    [OrxMandatoryField(0)] public ushort SourceId { get; set; }

    [OrxMandatoryField(1)] public ushort TickerId { get; set; }

    [OrxMandatoryField(2)] public DateTime VenueQuoteTime { get; set; }

    [OrxOptionalField(3)] public uint SourceRefId { get; set; }

    [OrxOptionalField(4)] public uint PQSequenceNumber { get; set; }

    public override IVenuePriceQuoteId Clone() => new OrxVenuePriceQuoteId(this);

    public override void StateReset()
    {
        SourceId = 0;
        TickerId = 0;
        VenueQuoteTime = DateTimeConstants.UnixEpoch;
        SourceRefId = 0;
        PQSequenceNumber = 0;
        base.StateReset();
    }

    public override IVenuePriceQuoteId CopyFrom(IVenuePriceQuoteId venuePriceQuoteId
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = venuePriceQuoteId.SourceId;
        TickerId = venuePriceQuoteId.TickerId;
        SourceRefId = venuePriceQuoteId.SourceRefId;
        PQSequenceNumber = venuePriceQuoteId.PQSequenceNumber;
        VenueQuoteTime = venuePriceQuoteId.VenueQuoteTime;
        return this;
    }


    protected bool Equals(OrxVenuePriceQuoteId other)
    {
        var sourceIdSame = SourceId == other.SourceId;
        var tickerIdSame = TickerId == other.TickerId;
        var sourceRefIdSame = SourceRefId == other.SourceRefId;
        var pqSequenceNumSame = PQSequenceNumber == other.PQSequenceNumber;
        var venueQuoteTimeSame = Equals(VenueQuoteTime, other.VenueQuoteTime);
        return sourceIdSame && tickerIdSame && sourceRefIdSame && pqSequenceNumSame && venueQuoteTimeSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenuePriceQuoteId)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ TickerId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceRefId;
            hashCode = (hashCode * 397) ^ (int)PQSequenceNumber;
            hashCode = (hashCode * 397) ^ VenueQuoteTime.GetHashCode();
            return hashCode;
        }
    }
}

public static class OrxVenuePriceQuoteIdExtensions
{
    public static OrxVenuePriceQuoteId? ToOrxPriceQuoteId(this IVenuePriceQuoteId? toConvert) =>
        toConvert != null ? new OrxVenuePriceQuoteId(toConvert) : null;
}
