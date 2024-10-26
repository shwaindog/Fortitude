#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public class Party : ReusableObject<IParty>, IParty
{
    public Party()
    {
        PartyId = null!;
        Name = null!;
        ParentParty = null!;
        ClientPartyId = null!;
        Portfolio = null!;
    }

    public Party(IParty toClone)
    {
        PartyId = toClone.PartyId;
        Name = toClone.Name;
        ParentParty = toClone.ParentParty;
        ClientPartyId = toClone.ClientPartyId;
        Portfolio = toClone.Portfolio;
    }

    public Party(string partyId, string name, IParty? parentParty, string clientPartyId, IBookingInfo portfolio)
        : this((MutableString)partyId, (MutableString)name, parentParty, (MutableString)clientPartyId, portfolio) { }

    public Party(IMutableString partyId, IMutableString name, IParty? parentParty, IMutableString clientPartyId
        , IBookingInfo portfolio)
    {
        PartyId = partyId;
        Name = name;
        ParentParty = parentParty;
        ClientPartyId = clientPartyId;
        Portfolio = portfolio;
    }

    public IMutableString PartyId { get; set; }
    public IMutableString Name { get; set; }
    public IParty? ParentParty { get; set; }
    public IMutableString ClientPartyId { get; set; }
    public IBookingInfo Portfolio { get; set; }

    public override IParty Clone() => Recycler?.Borrow<Party>().CopyFrom(this) ?? new Party(this);

    public override IParty CopyFrom(IParty source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PartyId = source.PartyId;
        Name = source.Name;
        ParentParty = source.ParentParty;
        ClientPartyId = source.ClientPartyId;
        Portfolio = source.Portfolio;
        return this;
    }
}
