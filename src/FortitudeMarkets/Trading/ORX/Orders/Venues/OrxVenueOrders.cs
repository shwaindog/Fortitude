#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Venues;

public class OrxVenueOrders : ReusableObject<IVenueOrders>, IVenueOrders
{
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

    public override IVenueOrders Clone() => Recycler?.Borrow<OrxVenueOrders>().CopyFrom(this) ?? new OrxVenueOrders(this);

    public IEnumerator<IVenueOrder> GetEnumerator() => VenueOrdersList?.GetEnumerator() ?? Enumerable.Empty<IVenueOrder>().GetEnumerator();

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
                orxVenueOrder.CopyFrom(venueOrders[i]!, copyMergeFlags);
                orxVenueList.Add(orxVenueOrder);
            }

            VenueOrdersList = orxVenueList;
        }

        return this;
    }

    protected bool Equals(OrxVenueOrders other) => VenueOrdersList?.SequenceEqual(other.VenueOrdersList!) ?? other.VenueOrdersList == null;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxVenueOrders)obj);
    }

    public override int GetHashCode() => VenueOrdersList != null ? VenueOrdersList.GetHashCode() : 0;
}
