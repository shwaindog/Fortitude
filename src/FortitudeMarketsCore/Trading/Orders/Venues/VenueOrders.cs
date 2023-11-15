#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueOrders : IVenueOrders
{
    private int refCount = 0;
    private IList<IVenueOrder?> venueOrders;

    public VenueOrders() => venueOrders = new List<IVenueOrder?>();

    public VenueOrders(IVenueOrders toClone)
    {
        venueOrders = toClone.Select(vo => vo?.Clone()).ToList();
    }

    public VenueOrders(IList<IVenueOrder?> venueOrders) => this.venueOrders = venueOrders;

    public int Count => venueOrders.Count;

    public IVenueOrder? this[int index]
    {
        get => venueOrders[index];
        set => venueOrders[index] = value;
    }

    public void CopyFrom(IVenueOrders source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        venueOrders = source.ToList()!;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IVenueOrders)source, copyMergeFlags);
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

    public IVenueOrders Clone() => new VenueOrders(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenueOrder> GetEnumerator() => venueOrders.GetEnumerator()!;
}
