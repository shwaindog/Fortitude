#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded;

public interface IPQRecentlyTraded : IMutableRecentlyTraded,
    IPQSupportsFieldUpdates<IRecentlyTraded>, IPQSupportsStringUpdates<IRecentlyTraded>,
    IEnumerable<IPQLastTrade>, IRelatedItem<IPQSourceTickerQuoteInfo>, IRelatedItem<IPQLastTrade>
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}
