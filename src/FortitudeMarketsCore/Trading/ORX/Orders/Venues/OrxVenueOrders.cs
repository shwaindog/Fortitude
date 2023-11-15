#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenueOrders : IVenueOrders
{
    private int refCount = 0;

    public OrxVenueOrders() { }

    public OrxVenueOrders(IVenueOrders toClone)
    {
        VenueOrdersList = toClone.Select(vo => new OrxVenueOrder(vo)).ToList();
    }

    public OrxVenueOrders(IList<OrxVenueOrder> venueOrders)
    {
        VenueOrdersList = venueOrders.Select(vo => new OrxVenueOrder(vo)).ToList();
    }

    [OrxMandatoryField(0)] public List<OrxVenueOrder>? VenueOrdersList { get; set; }

    public int Count => VenueOrdersList?.Count ?? 0;

    public IVenueOrder? this[int index]
    {
        get => VenueOrdersList?[index];
        set => VenueOrdersList![index] = (OrxVenueOrder)value!;
    }

    public IVenueOrders Clone() => new OrxVenueOrders(this);

    public IEnumerator<IVenueOrder> GetEnumerator() =>
        VenueOrdersList?.GetEnumerator() ?? Enumerable.Empty<IVenueOrder>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyFrom(IVenueOrders venueOrders, CopyMergeFlags copyMergeFlags)
    {
        var venueOrderCount = venueOrders.Count;
        if (venueOrderCount > 0)
        {
            var orxVenueList = Recycler!.Borrow<List<OrxVenueOrder>>();
            orxVenueList.Clear();
            for (var i = 0; i < venueOrderCount; i++)
            {
                var orxVenueOrder = Recycler!.Borrow<OrxVenueOrder>();
                orxVenueOrder.CopyFrom(venueOrders[i]!, copyMergeFlags);
                orxVenueList.Add(orxVenueOrder);
            }

            VenueOrdersList = orxVenueList;
        }
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

    protected bool Equals(OrxVenueOrders other) =>
        VenueOrdersList?.SequenceEqual(other.VenueOrdersList!) ?? other.VenueOrdersList == null;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenueOrders)obj);
    }

    public override int GetHashCode() => VenueOrdersList != null ? VenueOrdersList.GetHashCode() : 0;
}
