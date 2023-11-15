#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public class Venue : IVenue
{
    private int refCount = 0;

    public Venue(IVenue toClone)
    {
        VenueId = toClone.VenueId;
        Name = toClone.Name;
    }

    public Venue(ushort venueId, string name)
        : this(venueId, (MutableString)name) { }

    public Venue(ushort venueId, IMutableString name)
    {
        VenueId = venueId;
        Name = name;
    }

    public ushort VenueId { get; set; }
    public IMutableString Name { get; set; }

    public void CopyFrom(IVenue source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueId = source.VenueId;
        Name = source.Name;
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


    public IVenue Clone() => new Venue(this);
}
