#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueCriteria : IVenueCriteria
{
    private int refCount = 0;
    private IList<IVenue> venues;

    public VenueCriteria(IVenueCriteria toClone)
    {
        venues = toClone.Select(v => v.Clone()).ToList();
        VenueSelectionMethod = toClone.VenueSelectionMethod;
    }

    public VenueCriteria(IList<IVenue> venues, VenueSelectionMethod venueSelectionMethod)
    {
        this.venues = venues;
        VenueSelectionMethod = venueSelectionMethod;
    }

    public IVenue this[int index]
    {
        get => venues[index];
        set => venues[index] = value;
    }

    public int Count => venues.Count;
    public VenueSelectionMethod VenueSelectionMethod { get; set; }

    public void CopyFrom(IVenueCriteria source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        venues = source.ToList();
        VenueSelectionMethod = source.VenueSelectionMethod;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IVenueCriteria)source, copyMergeFlags);
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

    public IVenueCriteria Clone() => new VenueCriteria(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenue> GetEnumerator() => venues.GetEnumerator();
}
