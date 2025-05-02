#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Venues;

public class OrxVenueCriteria : ReusableObject<IVenueCriteria>, IVenueCriteria
{
    public OrxVenueCriteria() { }

    public OrxVenueCriteria(IVenueCriteria toClone)
    {
        VenueList = toClone.Select(v => new OrxVenue(v)).ToList();
        VenueSelectionMethod = toClone.VenueSelectionMethod;
    }

    public OrxVenueCriteria(IEnumerable<OrxVenue> venues,
        VenueSelectionMethod venueSelectionMethod)
    {
        VenueList = venues.ToList();
        VenueSelectionMethod = venueSelectionMethod;
    }

    [OrxMandatoryField(0)] public List<OrxVenue> VenueList { get; set; } = new();

    [OrxMandatoryField(1)] public VenueSelectionMethod VenueSelectionMethod { get; set; }

    public IVenue this[int index]
    {
        get => VenueList[index];
        set => VenueList[index] = (OrxVenue)value;
    }

    public int Count => VenueList.Count;

    public override IVenueCriteria Clone() => Recycler?.Borrow<OrxVenueCriteria>().CopyFrom(this) ?? new OrxVenueCriteria(this);

    public IEnumerator<IVenue> GetEnumerator() => VenueList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override IVenueCriteria CopyFrom(IVenueCriteria venueCriteria
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueSelectionMethod = venueCriteria.VenueSelectionMethod;
        var venueCriteriaCount = venueCriteria.Count;
        if (venueCriteriaCount > 0)
        {
            var orxVenueList = Recycler?.Borrow<List<OrxVenue>>() ?? new List<OrxVenue>();
            orxVenueList.Clear();
            for (var i = 0; i < venueCriteriaCount; i++)
            {
                var orxVenue = Recycler?.Borrow<OrxVenue>() ?? new OrxVenue();
                orxVenue.CopyFrom(venueCriteria[i], copyMergeFlags);
                orxVenueList.Add(orxVenue);
            }

            VenueList = orxVenueList;
        }

        return this;
    }

    protected bool Equals(OrxVenueCriteria other)
    {
        var venuesSame = VenueList?.SequenceEqual(other.VenueList) ?? other.VenueList == null;
        var venueSelectionMethodSame = VenueSelectionMethod == other.VenueSelectionMethod;
        return venuesSame && venueSelectionMethodSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenueCriteria)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((VenueList != null ? VenueList.GetHashCode() : 0) * 397) ^ (int)VenueSelectionMethod;
        }
    }
}
