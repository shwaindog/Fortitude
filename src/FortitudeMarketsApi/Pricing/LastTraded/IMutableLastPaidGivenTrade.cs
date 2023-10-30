namespace FortitudeMarketsApi.Pricing.LastTraded
{
    public interface IMutableLastPaidGivenTrade : IMutableLastTrade, ILastPaidGivenTrade
    {
        new bool WasPaid { get; set; }
        new bool WasGiven { get; set; }
        new decimal TradeVolume { get; set; }
        new IMutableLastPaidGivenTrade Clone();
    }
}