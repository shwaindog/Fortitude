using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;

namespace FortitudeMarketsCore.Trading.Counterparties
{
    public class Party : IParty
    {
        public Party(IParty toClone)
        {
            PartyId = toClone.PartyId;
            Name = toClone.Name;
            ParentParty = toClone.ParentParty;
            ClientPartyId = toClone.ClientPartyId;
            Portfolio = toClone.Portfolio;
        }

        public Party(string partyId, string name, IParty parentParty, string clientPartyId, IBookingInfo portfolio)
        : this((MutableString)partyId, (MutableString)name, parentParty, (MutableString)clientPartyId, portfolio)
        {
        }

        public Party(IMutableString partyId, IMutableString name, IParty parentParty, IMutableString clientPartyId, IBookingInfo portfolio)
        {
            PartyId = partyId;
            Name = name;
            ParentParty = parentParty;
            ClientPartyId = clientPartyId;
            Portfolio = portfolio;
        }

        public IMutableString PartyId { get; set; }
        public IMutableString Name { get; set; }
        public IParty ParentParty { get; set; }
        public IMutableString ClientPartyId { get; set; }
        public IBookingInfo Portfolio { get; set; }
        public IParty Clone()
        {
            return new Party(this);
        }
    }
}