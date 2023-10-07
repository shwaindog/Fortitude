namespace FortitudeMarketsApi.Trading.Counterparties;

public interface IParties
{
    IParty? BuySide { get; set; }
    IParty? SellSide { get; set; }
    IParties Clone();
}
