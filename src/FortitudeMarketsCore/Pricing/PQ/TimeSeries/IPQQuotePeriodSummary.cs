#region

using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries;

public interface IPQPricePeriodSummary : IMutablePricePeriodSummary, IPQSupportsFieldUpdates<IPricePeriodSummary>
{
    bool IsStartTimeDateUpdated { get; set; }
    bool IsStartTimeSubHourUpdated { get; set; }
    bool IsEndTimeDateUpdated { get; set; }
    bool IsEndTimeSubHourUpdated { get; set; }
    bool IsStartBidPriceUpdated { get; set; }
    bool IsStartAskPriceUpdated { get; set; }
    bool IsHighestBidPriceUpdated { get; set; }
    bool IsHighestAskPriceUpdated { get; set; }
    bool IsLowestBidPriceUpdated { get; set; }
    bool IsLowestAskPriceUpdated { get; set; }
    bool IsEndBidPriceUpdated { get; set; }
    bool IsEndAskPriceUpdated { get; set; }
    bool IsTickCountUpdated { get; set; }
    bool IsPeriodVolumeLowerBytesUpdated { get; set; }
    bool IsPeriodVolumeUpperBytesUpdated { get; set; }
    bool IsAverageMidPriceUpdated { get; set; }
}
