#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Executions;

public class OrxExecutionId : IExecutionId
{
    private int refCount = 0;

    public OrxExecutionId()
    {
        VenueExecutionId = new MutableString();
        BookingSystemId = new MutableString();
    }

    public OrxExecutionId(IExecutionId toClone)
    {
        VenueExecutionId = new MutableString(toClone.VenueExecutionId);
        AdapterExecutionId = toClone.AdapterExecutionId;
        BookingSystemId = new MutableString(toClone.BookingSystemId);
    }

    public OrxExecutionId(string venueExecutionId, int adapterExecutionId, string bookingSystemId)
        : this((MutableString)venueExecutionId, adapterExecutionId, bookingSystemId) { }

    public OrxExecutionId(MutableString venueExecutionId, int adapterExecutionId, MutableString bookingSystemId)
    {
        VenueExecutionId = venueExecutionId;
        AdapterExecutionId = adapterExecutionId;
        BookingSystemId = bookingSystemId;
    }

    [OrxMandatoryField(0)] public MutableString VenueExecutionId { get; set; }

    [OrxOptionalField(2)] public MutableString BookingSystemId { get; set; }

    IMutableString IExecutionId.VenueExecutionId
    {
        get => VenueExecutionId;
        set => VenueExecutionId = (MutableString)value;
    }

    [OrxMandatoryField(1)] public int AdapterExecutionId { get; set; }

    IMutableString IExecutionId.BookingSystemId
    {
        get => BookingSystemId;
        set => BookingSystemId = (MutableString)value;
    }

    public IExecutionId Clone() => new OrxExecutionId(this);

    public void CopyFrom(IExecutionId executionId, CopyMergeFlags copyMergeFlags)
    {
        VenueExecutionId = Recycler!.Borrow<MutableString>().Clear().Append(executionId.VenueExecutionId);
        AdapterExecutionId = executionId.AdapterExecutionId;
        BookingSystemId = Recycler!.Borrow<MutableString>().Clear().Append(executionId.BookingSystemId);
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IExecutionId)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }

    protected bool Equals(OrxExecutionId other)
    {
        var venueExeuctionIdSame = Equals(VenueExecutionId, other.VenueExecutionId);
        var adapterExecutionIdSame = AdapterExecutionId == other.AdapterExecutionId;
        var bookingSystemIdSame = Equals(BookingSystemId, other.BookingSystemId);

        return venueExeuctionIdSame && adapterExecutionIdSame && bookingSystemIdSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxExecutionId)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = VenueExecutionId != null ? VenueExecutionId.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ AdapterExecutionId;
            hashCode = (hashCode * 397) ^ (BookingSystemId != null ? BookingSystemId.GetHashCode() : 0);
            return hashCode;
        }
    }
}
