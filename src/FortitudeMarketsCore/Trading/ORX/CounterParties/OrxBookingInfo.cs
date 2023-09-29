using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsCore.Trading.Counterparties;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.CounterParties
{
    public class OrxBookingInfo : IBookingInfo
    {
        public OrxBookingInfo()
        {
        }

        public OrxBookingInfo(IBookingInfo toClone)
        {
            Portfolio = toClone.Portfolio != null ? new MutableString(toClone.Portfolio) : null;
            SubPortfolio = toClone.SubPortfolio != null ? new MutableString(toClone.SubPortfolio) : null;
        }

        public OrxBookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio)
        {
        }

        public OrxBookingInfo(IMutableString portfolio, IMutableString subPortfolio)
        {
            Portfolio = (MutableString)portfolio;
            SubPortfolio = (MutableString)subPortfolio;
        }

        [OrxMandatoryField(0)]
        public MutableString Portfolio { get; set; }

        IMutableString IBookingInfo.Portfolio
        {
            get => Portfolio;
            set => Portfolio = (MutableString)value;
        }

        [OrxOptionalField(1)]
        public MutableString SubPortfolio { get; set; }

        IMutableString IBookingInfo.SubPortfolio
        {
            get => SubPortfolio;
            set => SubPortfolio = (MutableString)value;
        }

        public void CopyFrom(IBookingInfo bookingInfo, IRecycler recycler)
        {

            Portfolio = bookingInfo.Portfolio != null
                ? recycler.Borrow<MutableString>().Clear().Append(bookingInfo.Portfolio) as MutableString
                : null;
            SubPortfolio = bookingInfo.SubPortfolio != null
                ? recycler.Borrow<MutableString>().Clear().Append(bookingInfo.SubPortfolio) as MutableString
                : null;
        }

        public IBookingInfo Clone()
        {
            return new BookingInfo(this);
        }

        protected bool Equals(OrxBookingInfo other)
        {
            var portfolioSame = string.Equals(Portfolio, other.Portfolio);
            var subPortfolioSame = string.Equals(SubPortfolio, other.SubPortfolio);

            return portfolioSame && subPortfolioSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrxBookingInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Portfolio != null ? Portfolio.GetHashCode() : 0) * 397) ^ 
                       (SubPortfolio != null ? SubPortfolio.GetHashCode() : 0);
            }
        }
    }
}