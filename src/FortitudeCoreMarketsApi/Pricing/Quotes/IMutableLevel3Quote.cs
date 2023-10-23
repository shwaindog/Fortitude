#region

using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface IMutableLevel3Quote : ILevel3Quote, IMutableLevel2Quote
{
    new uint BatchId { get; set; }
    new uint SourceQuoteReference { get; set; }
    new DateTime ValueDate { get; set; }
    new IMutableRecentlyTraded? RecentlyTraded { get; set; }
    new IMutableLevel3Quote Clone();
}
