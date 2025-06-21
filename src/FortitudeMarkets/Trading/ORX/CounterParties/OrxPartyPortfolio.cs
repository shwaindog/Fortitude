// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;

#endregion

namespace FortitudeMarkets.Trading.ORX.CounterParties;

public class OrxPartyPortfolio : ReusableObject<IPartyPortfolio>, IPartyPortfolio
{
    public OrxPartyPortfolio()
    {
    }

    public OrxPartyPortfolio(IPartyPortfolio toClone)
    {
        PartyId = toClone.PartyId;
        PortfolioId = toClone.PortfolioId;
    }


    public OrxPartyPortfolio(uint partyId, uint? portfolioId = 0)
    {
        PartyId     = partyId;
        PortfolioId = portfolioId;
    }

    [OrxMandatoryField(0)] public uint PartyId { get; set; }

    [OrxOptionalField(4)] public uint? PortfolioId { get; set; }

    public bool AutoRecycledByProducer { get; set; }

    public override OrxPartyPortfolio Clone() => Recycler?.Borrow<OrxPartyPortfolio>().CopyFrom(this) ?? new OrxPartyPortfolio(this);

    public override OrxPartyPortfolio CopyFrom(IPartyPortfolio partyPortfolio, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PartyId     = partyPortfolio.PartyId;
        PortfolioId = partyPortfolio.PortfolioId;
        return this;
    }

    protected bool Equals(OrxPartyPortfolio other)
    {
        var partyIdSame = PartyId == other.PartyId;
        var portfolioSame = PortfolioId == other.PortfolioId;

        return partyIdSame && portfolioSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxPartyPortfolio)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)PartyId;
            hashCode = (hashCode * 397) ^ (PortfolioId != null ? (int)PortfolioId!.Value : 0);
            return hashCode;
        }
    }
}
