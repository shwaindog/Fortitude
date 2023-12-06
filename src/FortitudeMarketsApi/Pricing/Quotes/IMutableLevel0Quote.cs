#region

using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface IMutableLevel0Quote : ILevel0Quote
{
    new bool IsReplay { get; set; }
    new DateTime SourceTime { get; set; }
    new DateTime ClientReceivedTime { get; set; }
    new IMutableSourceTickerQuoteInfo? SourceTickerQuoteInfo { get; set; }
    new decimal SinglePrice { get; set; }
    new IMutableLevel0Quote Clone();
}
