// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.Summaries;

public interface IPricePeriodSummary : ICloneable<IPricePeriodSummary>, IInterfacesComparable<IPricePeriodSummary>
  , IStoreState<IPricePeriodSummary>, ITimeSeriesSummary, ITimeSeriesEntry<IPricePeriodSummary>
{
    bool    IsEmpty         { get; }
    decimal StartBidPrice   { get; }
    decimal StartAskPrice   { get; }
    decimal HighestBidPrice { get; }
    decimal HighestAskPrice { get; }
    decimal LowestBidPrice  { get; }
    decimal LowestAskPrice  { get; }
    decimal EndBidPrice     { get; }
    decimal EndAskPrice     { get; }
    uint    TickCount       { get; }
    long    PeriodVolume    { get; }
    decimal AverageBidPrice { get; }
    decimal AverageAskPrice { get; }
}

public interface IMutablePricePeriodSummary : IPricePeriodSummary
{
    new TimeSeriesPeriod SummaryPeriod { get; set; }

    new bool     IsEmpty          { get; set; }
    new DateTime SummaryStartTime { get; set; }
    new DateTime SummaryEndTime   { get; set; }
    new decimal  StartBidPrice    { get; set; }
    new decimal  StartAskPrice    { get; set; }
    new decimal  HighestBidPrice  { get; set; }
    new decimal  HighestAskPrice  { get; set; }
    new decimal  LowestBidPrice   { get; set; }
    new decimal  LowestAskPrice   { get; set; }
    new decimal  EndBidPrice      { get; set; }
    new decimal  EndAskPrice      { get; set; }
    new uint     TickCount        { get; set; }
    new long     PeriodVolume     { get; set; }
    new decimal  AverageBidPrice  { get; set; }
    new decimal  AverageAskPrice  { get; set; }

    new IMutablePricePeriodSummary Clone();
}
