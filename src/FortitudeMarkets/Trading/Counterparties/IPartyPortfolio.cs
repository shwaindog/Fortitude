#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public interface IPartyPortfolio : IReusableObject<IPartyPortfolio>
{
    // destination account to book against
    uint PartyId { get; set; }

    // if the party has multiple portfolio then the sub-portfolio/account to book to
    uint? PortfolioId { get; set; }
}
