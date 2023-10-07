namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableLastTraderPaidGivenTrade : IMutableLastPaidGivenTrade, ILastTraderPaidGivenTrade
{
    new string? TraderName { get; set; }
    new IMutableLastTraderPaidGivenTrade Clone();
}
