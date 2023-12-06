#region

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableLastTrade : ILastTrade
{
    new DateTime TradeTime { get; set; }
    new decimal TradePrice { get; set; }

    new IMutableLastTrade Clone();
}
