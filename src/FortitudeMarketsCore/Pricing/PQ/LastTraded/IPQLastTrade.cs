using FortitudeCommon.DataStructures.Collections;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded
{
    public interface IPQLastTrade : IMutableLastTrade, IPQSupportsFieldUpdates<ILastTrade>,
        IRelatedItem<ISourceTickerQuoteInfo>, IRelatedItem<IPQLastTrade>
    {
        bool IsTradeTimeSubHourUpdated { get; set; }
        bool IsTradeTimeDateUpdated { get; set; }
        bool IsTradePriceUpdated { get; set; }
        new IPQLastTrade Clone();
    }
}
