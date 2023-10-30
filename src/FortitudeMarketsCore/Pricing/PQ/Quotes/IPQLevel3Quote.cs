#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Quotes;

public interface IPQLevel3Quote : IPQLevel2Quote, IMutableLevel3Quote
{
    new IPQRecentlyTraded? RecentlyTraded { get; set; }
    bool IsValueDateUpdated { get; set; }
    bool IsBatchIdUpdated { get; set; }
    bool IsSourceQuoteReferenceUpdated { get; set; }
    new IPQLevel3Quote Clone();
}
