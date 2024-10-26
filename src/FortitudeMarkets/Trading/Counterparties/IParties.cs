#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public interface IParties : IReusableObject<IParties>
{
    IParty? BuySide { get; set; }
    IParty? SellSide { get; set; }
}
