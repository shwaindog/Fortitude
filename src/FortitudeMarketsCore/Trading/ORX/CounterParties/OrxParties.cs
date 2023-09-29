using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.CounterParties
{
    public class OrxParties : IParties
    {
        public OrxParties()
        {
        }

        public OrxParties(IParties toClone)
        {
            if (toClone is OrxParties orxParties)
            {
                BuySide = (OrxParty) orxParties.BuySide?.Clone();
                SellSide = (OrxParty)orxParties.SellSide?.Clone();
            }
            else
            {
                BuySide = toClone.BuySide != null ? new OrxParty(toClone.BuySide) : null;
                SellSide = toClone.SellSide != null ? new OrxParty(toClone.SellSide) : null;
            }
        }

        public OrxParties(OrxParty buySide, OrxParty sellSide)
        {
            BuySide = buySide;
            SellSide = sellSide;
        }

        [OrxOptionalField(0)]
        public OrxParty BuySide { get; set; }

        IParty IParties.BuySide
        {
            get => BuySide;
            set => BuySide = (OrxParty) value;
        }

        [OrxOptionalField(1)]
        public OrxParty SellSide { get; set; }

        IParty IParties.SellSide
        {
            get => SellSide;
            set => SellSide = (OrxParty) value;
        }

        public void CopyFrom(IParties parties, IRecycler recycler)
        {
            if (parties.BuySide != null)
            {
                var orxBuySideParty = recycler.Borrow<OrxParty>();
                orxBuySideParty.CopyFrom(parties.BuySide, recycler);
                BuySide = orxBuySideParty;
            }
            if (parties.SellSide != null)
            {
                var orxSellSideParty = recycler.Borrow<OrxParty>();
                orxSellSideParty.CopyFrom(parties.SellSide, recycler);
                SellSide = orxSellSideParty;
            }
        }

        public IParties Clone()
        {
            return new OrxParties(this);
        }

        protected bool Equals(OrxParties other)
        {
            var buySideSame = Equals(BuySide, other.BuySide);
            var sellSideSame = Equals(SellSide, other.SellSide);
            return buySideSame && sellSideSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrxParties) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((BuySide != null ? BuySide.GetHashCode() : 0) * 397) ^ 
                       (SellSide != null ? SellSide.GetHashCode() : 0);
            }
        }
    }
}
