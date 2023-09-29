using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.CounterParties
{
    public class OrxParty : IParty
    {
        public OrxParty()
        {
        }

        public OrxParty(IParty toClone)
        {
            PartyId = toClone.PartyId != null ? new MutableString(toClone.PartyId) : null;
            Name = toClone.Name != null ? new MutableString(toClone.Name) : null;
            ParentParty = toClone.ParentParty != null ? new OrxParty(toClone.ParentParty) : null;
            ClientPartyId = toClone.ClientPartyId != null ? new MutableString(toClone.ClientPartyId) : null;
            Portfolio = toClone.Portfolio != null ? new OrxBookingInfo(toClone.Portfolio) : null;
        }

        public OrxParty(string partyId, string name, OrxParty parentParty, 
            string clientPartyId, OrxBookingInfo portfolio)
        : this((MutableString)partyId, name, parentParty, clientPartyId, portfolio)
        {
        }

        public OrxParty(MutableString partyId, MutableString name, OrxParty parentParty, 
            MutableString clientPartyId, OrxBookingInfo portfolio)
        {
            PartyId = partyId;
            Name = name;
            ParentParty = parentParty;
            ClientPartyId = clientPartyId;
            Portfolio = portfolio;
        }

        [OrxMandatoryField(0)]
        public MutableString PartyId { get; set; }

        IMutableString IParty.PartyId
        {
            get => PartyId;
            set => PartyId = (MutableString)value;
        }

        [OrxOptionalField(1)]
        public MutableString Name { get; set; }

        IMutableString IParty.Name
        {
            get => Name;
            set => Name = (MutableString)value;
        }

        [OrxOptionalField(2)]
        public OrxParty ParentParty { get; set; }

        IParty IParty.ParentParty
        {
            get => ParentParty;
            set => ParentParty = (OrxParty)value;
        }

        [OrxOptionalField(3)]
        public MutableString ClientPartyId { get; set; }

        IMutableString IParty.ClientPartyId
        {
            get => ClientPartyId;
            set => ClientPartyId = (MutableString)value;
        }

        [OrxOptionalField(4)]
        public OrxBookingInfo Portfolio { get; set; }

        IBookingInfo IParty.Portfolio
        {
            get => Portfolio;
            set => Portfolio = (OrxBookingInfo)value;
        }

        public void CopyFrom(IParty party, IRecycler recycler)
        {
            PartyId = party.PartyId != null
                ? recycler.Borrow<MutableString>().Clear().Append(party.PartyId) as MutableString
                : null;
            Name = party.Name != null
                ? recycler.Borrow<MutableString>().Clear().Append(party.Name) as MutableString
                : null;
            if (party.ParentParty != null)
            {
                var orxParentParty = recycler.Borrow<OrxParty>();
                orxParentParty.CopyFrom(party.ParentParty, recycler);
                ParentParty = orxParentParty;
            }
            ClientPartyId = party.ClientPartyId != null
                ? recycler.Borrow<MutableString>().Clear().Append(party.ClientPartyId) as MutableString
                : null;
            if (party.Portfolio != null)
            {
                var orxBookingInfo = recycler.Borrow<OrxBookingInfo>();
                orxBookingInfo.CopyFrom(party.Portfolio, recycler);
                Portfolio = orxBookingInfo;
            }
        }

        public IParty Clone()
        {
            return new OrxParty(this);
        }

        protected bool Equals(OrxParty other)
        {
            var partyIdSame = Equals(PartyId, other.PartyId);
            var nameSame = Equals(Name, other.Name);
            var parentPartySame = Equals(ParentParty, other.ParentParty);
            var clientPartyIdSame = Equals(ClientPartyId, other.ClientPartyId);
            var portfolioSame = Equals(Portfolio, other.Portfolio);
            return partyIdSame && nameSame && parentPartySame && clientPartyIdSame && portfolioSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxParty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PartyId != null ? PartyId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentParty != null ? ParentParty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ClientPartyId != null ? ClientPartyId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Portfolio != null ? Portfolio.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}