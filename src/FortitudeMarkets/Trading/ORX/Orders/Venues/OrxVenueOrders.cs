#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Venues;

public class OrxVenueOrders : ReusableObject<IVenueOrders>, IVenueOrders
{
    public OrxVenueOrders()
    {
        VenueOrdersList = [];
    }

    public OrxVenueOrders(IVenueOrders toClone)
    {
        VenueOrdersList = toClone.Select(vo => new OrxVenueOrder(vo)).ToList();
    }

    public OrxVenueOrders(IList<OrxVenueOrder> venueOrders)
    {
        VenueOrdersList = venueOrders.Select(vo => new OrxVenueOrder(vo)).ToList();
    }

    [OrxMandatoryField(0)] public List<OrxVenueOrder> VenueOrdersList { get; set; }

    public int Count => VenueOrdersList.Count;

    public IVenueOrder this[int index]
    {
        get => VenueOrdersList[index];
        set => VenueOrdersList[index] = (OrxVenueOrder)value;
    }

    public void Add(IVenueOrder item)
    {
        if (item is OrxVenueOrder orxVenueOrder)
        {
            VenueOrdersList.Add(orxVenueOrder);
        }
        else
        {
            VenueOrdersList.Add(new OrxVenueOrder(item));
        }
    }

    public void Clear()
    {
        VenueOrdersList.Clear();
    }

    public bool Contains(IVenueOrder item) => VenueOrdersList.Contains(item);

    public void CopyTo(IVenueOrder[] array, int arrayIndex)
    {
        int myIndex = 0;
        for (int i = arrayIndex; i < array.Length && myIndex < VenueOrdersList.Count; i++)
        {
            array[i] = VenueOrdersList[myIndex++];
        }
    }

    public int  IndexOf(IVenueOrder item) => VenueOrdersList.IndexOf((OrxVenueOrder)item);

    public void Insert(int index, IVenueOrder item)
    {
        if (item is OrxVenueOrder orxVenueOrder)
        {
            VenueOrdersList.Insert(index, orxVenueOrder);
        }
        else
        {
            VenueOrdersList.Insert(index, new OrxVenueOrder(item));
        }
    }

    public bool IsReadOnly => false;

    public bool Remove(IVenueOrder item) => VenueOrdersList.Remove((OrxVenueOrder)item);

    public void RemoveAt(int index)
    {
        VenueOrdersList.RemoveAt(index);
    }

    public override void StateReset()
    {
        base.StateReset();
        VenueOrdersList.Clear();
    }

    public override IVenueOrders Clone() => Recycler?.Borrow<OrxVenueOrders>().CopyFrom(this) ?? new OrxVenueOrders(this);

    public IEnumerator<IVenueOrder> GetEnumerator() => VenueOrdersList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override IVenueOrders CopyFrom(IVenueOrders venueOrders
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var venueOrderCount = venueOrders.Count;
        if (venueOrderCount > 0)
        {
            var orxVenueList = Recycler!.Borrow<List<OrxVenueOrder>>();
            orxVenueList.Clear();
            for (var i = 0; i < venueOrderCount; i++)
            {
                var orxVenueOrder = Recycler!.Borrow<OrxVenueOrder>();
                orxVenueOrder.CopyFrom(venueOrders[i], copyMergeFlags);
                orxVenueList.Add(orxVenueOrder);
            }

            VenueOrdersList = orxVenueList;
        }

        return this;
    }

    protected bool Equals(OrxVenueOrders other) => VenueOrdersList.SequenceEqual(other.VenueOrdersList);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenueOrders)obj);
    }

    public override int GetHashCode() => VenueOrdersList.GetHashCode();
}


public static class OrxVenueOrdersExtensions
{
    public static OrxVenueOrders? ToOrxVenueOrders(this IVenueOrders? toConvert) =>
        toConvert != null ? new OrxVenueOrders(toConvert) : null;
}