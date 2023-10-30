using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.Conflation
{
    public interface IPQPeriodSummary : IMutablePeriodSummary, IPQSupportsFieldUpdates<IPeriodSummary>
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
    }
}