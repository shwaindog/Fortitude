#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.CounterParties;

public class OrxParty : IParty
{
    public OrxParty()
    {
        PartyId = new MutableString();
        Name = new MutableString();
        ParentParty = null;
        ClientPartyId = new MutableString();
        Portfolio = new OrxBookingInfo();
    }

    public OrxParty(IParty toClone)
    {
        PartyId = new MutableString(toClone.PartyId);
        Name = new MutableString(toClone.Name);
        ParentParty = toClone.ParentParty != null ? new OrxParty(toClone.ParentParty) : null;
        ClientPartyId = new MutableString(toClone.ClientPartyId);
        Portfolio = new OrxBookingInfo(toClone.Portfolio);
    }

    public OrxParty(string partyId, string name, OrxParty? parentParty,
        string clientPartyId, OrxBookingInfo portfolio)
        : this((MutableString)partyId, name, parentParty, clientPartyId, portfolio) { }

    public OrxParty(MutableString partyId, MutableString name, OrxParty? parentParty,
        MutableString clientPartyId, OrxBookingInfo portfolio)
    {
        PartyId = partyId;
        Name = name;
        ParentParty = parentParty;
        ClientPartyId = clientPartyId;
        Portfolio = portfolio;
    }

    [OrxMandatoryField(0)] public MutableString PartyId { get; set; }

    [OrxOptionalField(1)] public MutableString Name { get; set; }

    [OrxOptionalField(2)] public OrxParty? ParentParty { get; set; }

    [OrxOptionalField(3)] public MutableString ClientPartyId { get; set; }

    [OrxOptionalField(4)] public OrxBookingInfo Portfolio { get; set; }

    IMutableString IParty.PartyId
    {
        get => PartyId;
        set => PartyId = (MutableString)value;
    }

    IMutableString IParty.Name
    {
        get => Name;
        set => Name = (MutableString)value;
    }

    IParty? IParty.ParentParty
    {
        get => ParentParty;
        set => ParentParty = value as OrxParty;
    }

    IMutableString IParty.ClientPartyId
    {
        get => ClientPartyId;
        set => ClientPartyId = (MutableString)value;
    }

    IBookingInfo IParty.Portfolio
    {
        get => Portfolio;
        set => Portfolio = (OrxBookingInfo)value;
    }

    public IParty Clone() => new OrxParty(this);

    public void CopyFrom(IParty party, IRecycler recycler)
    {
        PartyId = recycler.Borrow<MutableString>().Clear().Append(party.PartyId);
        Name = recycler.Borrow<MutableString>().Clear().Append(party.Name);
        if (party.ParentParty != null)
        {
            var orxParentParty = recycler.Borrow<OrxParty>();
            orxParentParty.CopyFrom(party.ParentParty, recycler);
            ParentParty = orxParentParty;
        }

        ClientPartyId = recycler.Borrow<MutableString>().Clear().Append(party.ClientPartyId);
        if (party.Portfolio != null)
        {
            var orxBookingInfo = recycler.Borrow<OrxBookingInfo>();
            orxBookingInfo.CopyFrom(party.Portfolio, recycler);
            Portfolio = orxBookingInfo;
        }
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

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxParty)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = PartyId != null ? PartyId.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ParentParty != null ? ParentParty.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ClientPartyId != null ? ClientPartyId.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Portfolio != null ? Portfolio.GetHashCode() : 0);
            return hashCode;
        }
    }
}
