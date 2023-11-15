#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenue : IVenue
{
    private int refCount = 0;

    public OrxVenue() => Name = new MutableString();

    public OrxVenue(IVenue toClone)
    {
        VenueId = toClone.VenueId;
        Name = new MutableString(toClone.Name);
    }

    public OrxVenue(ushort venueId, string name)
        : this(venueId, (MutableString)name) { }

    public OrxVenue(ushort venueId, MutableString name)
    {
        VenueId = venueId;
        Name = name;
    }

    [OrxOptionalField(1)] public MutableString Name { get; set; }

    [OrxMandatoryField(0)] public ushort VenueId { get; set; }

    IMutableString IVenue.Name
    {
        get => Name;
        set => Name = (MutableString)value;
    }

    public IVenue Clone() => new OrxVenue(this);

    public void CopyFrom(IVenue venue, CopyMergeFlags copyMergeFlags)
    {
        VenueId = venue.VenueId;
        Name = Recycler!.Borrow<MutableString>().Clear().Append(venue.Name);
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IVenue)source, copyMergeFlags);
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

    protected bool Equals(OrxVenue other)
    {
        var venueIdSame = VenueId == other.VenueId;
        var nameSame = Equals(Name, other.Name);
        return venueIdSame && nameSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenue)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (VenueId.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
