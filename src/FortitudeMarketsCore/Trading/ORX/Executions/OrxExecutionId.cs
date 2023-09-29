using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Executions
{
    public class OrxExecutionId : IExecutionId
    {
        public OrxExecutionId()
        {
        }

        public OrxExecutionId(IExecutionId toClone)
        {
            VenueExecutionId = toClone.VenueExecutionId != null ? new MutableString(toClone.VenueExecutionId) : null;
            AdapterExecutionId = toClone.AdapterExecutionId;
            BookingSystemId = toClone.BookingSystemId != null ? new MutableString(toClone.BookingSystemId) : null;
        }

        public OrxExecutionId(string venueExecutionId, int adapterExecutionId, string bookingSystemId)
        : this((MutableString)venueExecutionId, adapterExecutionId, bookingSystemId)
        {
        }

        public OrxExecutionId(MutableString venueExecutionId, int adapterExecutionId, MutableString bookingSystemId)
        {
            VenueExecutionId = venueExecutionId;
            AdapterExecutionId = adapterExecutionId;
            BookingSystemId = bookingSystemId;
        }

        [OrxMandatoryField(0)]
        public MutableString VenueExecutionId { get; set; }

        IMutableString IExecutionId.VenueExecutionId
        {
            get => VenueExecutionId;
            set => VenueExecutionId = (MutableString)value;
        }

        [OrxMandatoryField(1)]
        public int AdapterExecutionId { get; set; }

        [OrxOptionalField(2)]
        public MutableString BookingSystemId { get; set; }

        IMutableString IExecutionId.BookingSystemId
        {
            get => BookingSystemId;
            set => BookingSystemId = (MutableString)value;
        }

        public void CopyFrom(IExecutionId executionId, IRecycler recycler)
        {
            VenueExecutionId = executionId.VenueExecutionId != null
                ? recycler.Borrow<MutableString>().Clear().Append(executionId.VenueExecutionId) as MutableString
                : null;
            AdapterExecutionId = executionId.AdapterExecutionId;
            BookingSystemId = executionId.BookingSystemId != null
                ? recycler.Borrow<MutableString>().Clear().Append(executionId.BookingSystemId) as MutableString
                : null;
        }

        public IExecutionId Clone()
        {
            return new OrxExecutionId(this);
        }

        protected bool Equals(OrxExecutionId other)
        {
            var venueExeuctionIdSame = Equals(VenueExecutionId, other.VenueExecutionId);
            var adapterExecutionIdSame = AdapterExecutionId == other.AdapterExecutionId;
            var bookingSystemIdSame = Equals(BookingSystemId, other.BookingSystemId);

            return venueExeuctionIdSame && adapterExecutionIdSame && bookingSystemIdSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxExecutionId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (VenueExecutionId != null ? VenueExecutionId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AdapterExecutionId;
                hashCode = (hashCode * 397) ^ (BookingSystemId != null ? BookingSystemId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}