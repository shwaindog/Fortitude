#region

using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface IMutableLevel1Quote : ILevel1Quote, IMutableLevel0Quote
{
    new DateTime AdapterSentTime { get; set; }
    new DateTime AdapterReceivedTime { get; set; }
    new DateTime SourceBidTime { get; set; }
    new decimal BidPriceTop { get; set; }
    new bool IsBidPriceTopUpdated { get; set; }
    new DateTime SourceAskTime { get; set; }
    new decimal AskPriceTop { get; set; }
    new bool IsAskPriceTopUpdated { get; set; }
    new bool Executable { get; set; }
    new IMutablePricePeriodSummary? SummaryPeriod { get; set; }
    new IMutableLevel1Quote Clone();
}
