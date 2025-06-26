// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueCriteria : ReusableObject<IVenueCriteria>, IVenueCriteria
{
    private IList<IVenue> venues;

    public VenueCriteria() => venues = new List<IVenue>();

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

    public override void StateReset()
    {
        base.StateReset();
        venues.Clear();
    }

    public override IVenueCriteria Clone() => Recycler?.Borrow<VenueCriteria>().CopyFrom(this) ?? new VenueCriteria(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenue> GetEnumerator() => venues.GetEnumerator();

    public override IVenueCriteria CopyFrom
        (IVenueCriteria source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        venues = source.ToList();

        VenueSelectionMethod = source.VenueSelectionMethod;
        return this;
    }
}


public static class VenueCriteriaExtensions
{
    public static VenueCriteria? ToVenueCriteria(this IVenueCriteria? toConvert) =>
        toConvert != null ? new VenueCriteria(toConvert) : null;
}