﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueOrders : ReusableObject<IVenueOrders>, IVenueOrders
{
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

    public override void StateReset()
    {
        base.StateReset();
        venueOrders.Clear();
    }

    public override IVenueOrders Clone() => Recycler?.Borrow<VenueOrders>().CopyFrom(this) ?? new VenueOrders(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenueOrder> GetEnumerator() => venueOrders.GetEnumerator()!;

    public override IVenueOrders CopyFrom(IVenueOrders source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        venueOrders = source.ToList()!;
        return this;
    }
}
