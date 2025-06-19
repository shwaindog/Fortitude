#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;

#endregion

namespace FortitudeMarkets.Trading.ORX.CounterParties;

public class OrxParties : ReusableObject<IParties>, IParties
{
    public OrxParties() { }

    public OrxParties(IParties toClone)
    {
        InitiatorPartyId = toClone.InitiatorPartyId;
        if (toClone is OrxParties orxParties)
        {
            BuySide = orxParties.BuySide?.Clone();
            SellSide = orxParties.SellSide?.Clone();
        }
        else
        {
            BuySide = toClone.BuySide != null ? new OrxPartyPortfolio(toClone.BuySide) : null;
            SellSide = toClone.SellSide != null ? new OrxPartyPortfolio(toClone.SellSide) : null;
        }
    }

    public OrxParties(uint initiatorPartyId, OrxPartyPortfolio buySide, OrxPartyPortfolio sellSide)
    {
        InitiatorPartyId = initiatorPartyId;

        BuySide          = buySide;
        SellSide         = sellSide;
    }

    [OrxMandatoryField(0)] public uint InitiatorPartyId  { get; set; }

    [OrxOptionalField(0)] public OrxPartyPortfolio? BuySide { get; set; }

    [OrxOptionalField(1)] public OrxPartyPortfolio? SellSide { get; set; }
    
    public uint ResolveBuySideAccount() => BuySide?.PortfolioId ?? BuySide?.PartyId ?? InitiatorPartyId;

    public uint? ResolveSellSideAccount() => SellSide?.PortfolioId ?? SellSide?.PartyId;

    public bool HasSellSideParty        => SellSide != null && (SellSide.PartyId != 0 || SellSide.PortfolioId != 0);

    IPartyPortfolio? IParties.BuySide
    {
        get => BuySide;
        set => BuySide = value as OrxPartyPortfolio;
    }

    IPartyPortfolio? IParties.SellSide
    {
        get => SellSide;
        set => SellSide = value as OrxPartyPortfolio;
    }

    public override IParties Clone() => Recycler?.Borrow<OrxParties>().CopyFrom(this) ?? new OrxParties(this);

    public override IParties CopyFrom(IParties source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        InitiatorPartyId = source.InitiatorPartyId;

        BuySide  = source.BuySide.SyncOrRecycle(BuySide);
        SellSide = source.SellSide.SyncOrRecycle(SellSide);
        return this;
    }
    
    public bool AreEquivalent(IParties? other, bool exactTypes = false)
    {
        if (other == null) return false;
        
        var initiatorSame = InitiatorPartyId == other.InitiatorPartyId;
        var buySideSame   = Equals(BuySide, other.BuySide);
        var sellSideSame  = Equals(SellSide, other.SellSide);

        var allAreSame = initiatorSame && buySideSame && sellSideSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IParties, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)InitiatorPartyId;
            hashCode = (hashCode * 397) ^ (BuySide != null ? BuySide.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (SellSide != null ? SellSide.GetHashCode() : 0);
            return hashCode;
        }
    }

    protected string PartiesToStringMembers
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(nameof(InitiatorPartyId)).Append(": ").Append(InitiatorPartyId);
            if(BuySide != null) sb.Append(", ").Append(nameof(BuySide)).Append(": ").Append(BuySide);
            if(SellSide != null) sb.Append(", ").Append(nameof(SellSide)).Append(": ").Append(SellSide);

            return sb.ToString();
        }
    }

    public override string ToString() => $"{nameof(OrxParties)}{{{PartiesToStringMembers}}}";
}
