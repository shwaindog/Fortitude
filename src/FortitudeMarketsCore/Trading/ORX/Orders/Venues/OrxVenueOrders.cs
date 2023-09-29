using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues
{
    public class OrxVenueOrders : IVenueOrders
    {
        public OrxVenueOrders()
        {
        }

        public OrxVenueOrders(IVenueOrders toClone)
        {
            VenueOrdersList = toClone.Select(vo => new OrxVenueOrder(vo)).ToList();
        }

        public OrxVenueOrders(IList<OrxVenueOrder> venueOrders)
        {
            VenueOrdersList = venueOrders.Select(vo => new OrxVenueOrder(vo)).ToList();
        }

        [OrxMandatoryField(0)]
        public List<OrxVenueOrder> VenueOrdersList { get; set; }

        public int Count => VenueOrdersList.Count;

        public IVenueOrder this[int index]
        {
            get => VenueOrdersList[index];
            set => VenueOrdersList[index] = (OrxVenueOrder)value;
        }

        public void CopyFrom(IVenueOrders venueOrders, IRecycler recycler)
        {
            var venueOrderCount = venueOrders.Count;
            if (venueOrderCount > 0)
            {
                var orxVenueList = recycler.Borrow<List<OrxVenueOrder>>();
                orxVenueList.Clear();
                for (int i = 0; i < venueOrderCount; i++)
                {
                    var orxVenueOrder = recycler.Borrow<OrxVenueOrder>();
                    orxVenueOrder.CopyFrom(venueOrders[i], recycler);
                    orxVenueList.Add(orxVenueOrder);
                }
                VenueOrdersList = orxVenueList;
            }
        }

        public IVenueOrders Clone()
        {
            return new OrxVenueOrders(this);
        }

        protected bool Equals(OrxVenueOrders other)
        {
            return VenueOrdersList?.SequenceEqual(other.VenueOrdersList) ?? other.VenueOrdersList == null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxVenueOrders) obj);
        }

        public override int GetHashCode()
        {
            return VenueOrdersList != null ? VenueOrdersList.GetHashCode() : 0;
        }

        public IEnumerator<IVenueOrder> GetEnumerator()
        {
            return VenueOrdersList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}