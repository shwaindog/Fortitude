// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public class Parties : ReusableObject<IParties>, IParties
{
    public Parties() { }

    public Parties(IParties toClone)
    {
        InitiatorPartyId = toClone.InitiatorPartyId;

        BuySide          = toClone.BuySide?.Clone();
        SellSide         = toClone.SellSide?.Clone();
    }

    public Parties(uint initiatorPartyId, IPartyPortfolio? buySide = null, IPartyPortfolio? sellSide = null)
    {
        InitiatorPartyId = initiatorPartyId;

        BuySide  = buySide;
        SellSide = sellSide;
    }

    public uint InitiatorPartyId        { get; set; }

    public IPartyPortfolio? BuySide  { get; set; }

    public IPartyPortfolio? SellSide { get; set; }
    
    public uint ResolveBuySideAccount() => BuySide?.PortfolioId ?? BuySide?.PartyId ?? InitiatorPartyId;

    public uint? ResolveSellSideAccount() => SellSide?.PortfolioId ?? SellSide?.PartyId;

    public bool HasSellSideParty        => SellSide != null && (SellSide.PartyId != 0 || SellSide.PortfolioId != 0);

    public override IParties Clone() => Recycler?.Borrow<Parties>().CopyFrom(this) ?? new Parties(this);

    public override IParties CopyFrom(IParties source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        InitiatorPartyId = source.InitiatorPartyId;
        
        BuySide  = source.BuySide?.CopyOrClone(BuySide as PartyPortfolio);
        SellSide = source.SellSide?.CopyOrClone(SellSide as PartyPortfolio);
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


    public override string ToString() => $"{nameof(Parties)}{{{PartiesToStringMembers}}}";
}
