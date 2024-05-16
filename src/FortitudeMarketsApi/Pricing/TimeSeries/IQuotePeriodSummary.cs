#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.TimeSeries;

public interface IQuotePeriodSummary : ICloneable<IQuotePeriodSummary>, IInterfacesComparable<IQuotePeriodSummary>
    , IStoreState<IQuotePeriodSummary>, ITimeSeriesSummary
{
    decimal StartBidPrice { get; }
    decimal StartAskPrice { get; }
    decimal HighestBidPrice { get; }
    decimal HighestAskPrice { get; }
    decimal LowestBidPrice { get; }
    decimal LowestAskPrice { get; }
    decimal EndBidPrice { get; }
    decimal EndAskPrice { get; }
    uint TickCount { get; }
    long PeriodVolume { get; }
    decimal AverageMidPrice { get; }
}

public interface IMutableQuotePeriodSummary : IQuotePeriodSummary
{
    new TimeSeriesPeriod SummaryPeriod { get; set; }
    new DateTime SummaryStartTime { get; set; }
    new DateTime SummaryEndTime { get; set; }
    new decimal StartBidPrice { get; set; }
    new decimal StartAskPrice { get; set; }
    new decimal HighestBidPrice { get; set; }
    new decimal HighestAskPrice { get; set; }
    new decimal LowestBidPrice { get; set; }
    new decimal LowestAskPrice { get; set; }
    new decimal EndBidPrice { get; set; }
    new decimal EndAskPrice { get; set; }
    new uint TickCount { get; set; }
    new long PeriodVolume { get; set; }
    new decimal AverageMidPrice { get; set; }
    new IMutableQuotePeriodSummary Clone();
}
