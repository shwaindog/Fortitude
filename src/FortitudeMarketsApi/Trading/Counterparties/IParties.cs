#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Trading.Counterparties;

public interface IParties : IRecyclableObject<IParties>
{
    IParty? BuySide { get; set; }
    IParty? SellSide { get; set; }
    IParties Clone();
}
