// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public class PartyPortfolio : ReusableObject<IPartyPortfolio>, IPartyPortfolio
{
    public PartyPortfolio()
    {
        PartyId = 0;
        PortfolioId     = 0;
    }

    public PartyPortfolio(IPartyPortfolio toClone)
    {
        PartyId = toClone.PartyId;
        PortfolioId     = toClone.PortfolioId;
    }

    public PartyPortfolio(uint partyId, uint portfolioId)
    {
        PartyId = partyId;
        PortfolioId     = portfolioId;
    }

    public uint PartyId { get; set; }

    public uint? PortfolioId { get; set; }

    public override IPartyPortfolio Clone() => Recycler?.Borrow<PartyPortfolio>().CopyFrom(this) ?? new PartyPortfolio(this);

    public override IPartyPortfolio CopyFrom(IPartyPortfolio source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PartyId = source.PartyId;
        PortfolioId     = source.PortfolioId;
        return this;
    }
}
