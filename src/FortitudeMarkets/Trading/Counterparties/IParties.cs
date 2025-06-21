#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public interface IParties : IReusableObject<IParties>, IInterfacesComparable<IParties>
{
    // Account - of Trader or strategy making request be the only
    uint InitiatorPartyId { get; set; }

    // if set then the destination account and Initiator is an on-behalf of request
    IPartyPortfolio? BuySide { get; set; }

    bool HasSellSideParty { get; }
    // if set the other ledger of the opposite party position
    // venue ledger or if back to back booking the other account to transfer from
    IPartyPortfolio? SellSide { get; set; }

    uint ResolveBuySideAccount();

    uint? ResolveSellSideAccount();
}
