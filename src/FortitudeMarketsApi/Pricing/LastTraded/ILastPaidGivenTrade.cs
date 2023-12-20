namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface ILastPaidGivenTrade : ILastTrade
{
    bool WasPaid { get; }
    bool WasGiven { get; }
    decimal TradeVolume { get; }
    new ILastPaidGivenTrade Clone();
}
