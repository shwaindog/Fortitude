#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues;

public class OrxVenueCriteria : IVenueCriteria
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

    public IVenueCriteria Clone() => new OrxVenueCriteria(this);

    public IEnumerator<IVenue> GetEnumerator() => VenueList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyFrom(IVenueCriteria venueCriteria, IRecycler recycler)
    {
        VenueSelectionMethod = venueCriteria.VenueSelectionMethod;
        var venueCriteriaCount = venueCriteria.Count;
        if (venueCriteriaCount > 0)
        {
            var orxVenueList = recycler.Borrow<List<OrxVenue>>();
            orxVenueList.Clear();
            for (var i = 0; i < venueCriteriaCount; i++)
            {
                var orxVenue = recycler.Borrow<OrxVenue>();
                orxVenue.CopyFrom(venueCriteria[i], recycler);
                orxVenueList.Add(orxVenue);
            }

            VenueList = orxVenueList;
        }
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
