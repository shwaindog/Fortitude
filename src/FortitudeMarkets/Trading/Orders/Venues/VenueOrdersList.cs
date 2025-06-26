// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueOrdersList : ReusableObject<IVenueOrders>, IVenueOrders
{
    private IList<IVenueOrder> venueOrders;

    public VenueOrdersList() => venueOrders = new List<IVenueOrder>();

    public VenueOrdersList(IVenueOrders toClone)
    {
        venueOrders = toClone.Select(vo => vo.Clone()).ToList();
    }

    public VenueOrdersList(IList<IVenueOrder> venueOrders) => this.venueOrders = venueOrders;

    public int Count => venueOrders.Count;


    public IVenueOrder this[int index]
    {
        get => venueOrders[index];
        set => venueOrders[index] = value;
    }

    public void Add(IVenueOrder item)
    {
        venueOrders.Add(item);
    }

    public void Clear()
    {
        venueOrders.Clear();
    }

    public bool Contains(IVenueOrder item) => venueOrders.Contains(item);

    public void CopyTo(IVenueOrder[] array, int arrayIndex)
    {
        int myIndex = 0;
        for (int i = arrayIndex; i < array.Length && myIndex < venueOrders.Count; i++)
        {
            array[i] = venueOrders[myIndex++];
        }
    }

    public int  IndexOf(IVenueOrder item) => venueOrders.IndexOf(item);

    public void Insert(int index, IVenueOrder item)
    {
        venueOrders.Insert(index, item);
    }

    public bool IsReadOnly => false;

    public bool Remove(IVenueOrder item) => venueOrders.Remove(item);

    public void RemoveAt(int index)
    {
        venueOrders.RemoveAt(index);
    }

    public override void StateReset()
    {
        base.StateReset();
        venueOrders.Clear();
    }

    public override IVenueOrders Clone() => Recycler?.Borrow<VenueOrdersList>().CopyFrom(this) ?? new VenueOrdersList(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenueOrder> GetEnumerator() => venueOrders.GetEnumerator();

    public override IVenueOrders CopyFrom(IVenueOrders source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        venueOrders = source.ToList();
        return this;
    }
}

public static class VenueOrdersExtensions
{
    public static VenueOrdersList? ToVenueOrders(this IVenueOrders? toConvert) =>
        toConvert != null ? new VenueOrdersList(toConvert) : null;
}