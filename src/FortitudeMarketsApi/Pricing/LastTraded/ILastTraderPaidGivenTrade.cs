namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface ILastTraderPaidGivenTrade : ILastPaidGivenTrade
{
    string? TraderName { get; }
    new ILastTraderPaidGivenTrade Clone();
}
