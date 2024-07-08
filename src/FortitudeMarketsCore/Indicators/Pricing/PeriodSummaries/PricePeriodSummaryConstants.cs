// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public static class PricePeriodSummaryConstants
{
    public const string PeriodSummaryBase                      = $"{IndicatorServiceConstants.IndicatorsBase}.PricePeriodSummaries";
    public const string PeriodSummaryLiveTemplate              = $"{PeriodSummaryBase}.Live.{{0}}.{{1}}.{{2}}";
    public const string PeriodSummaryCompleteTemplate          = $"{PeriodSummaryBase}.Complete.{{0}}.{{1}}.{{2}}";
    public const string PeriodSummaryPersistAppendPublish      = $"{PeriodSummaryBase}.Persist.Append.{{0}}.{{1}}.{{2}}";
    public const string PeriodSummaryPersistRepublish          = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.Republish";
    public const string PeriodSummaryHistoricalStreamRequest   = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.Stream.Request";
    public const string PeriodSummaryHistoricalResponseRequest = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.RequestResponse";


    public const TimeSeriesPeriod PersistPeriodsFrom = TimeSeriesPeriod.FifteenSeconds;
    public const TimeSeriesPeriod PersistPeriodsTo   = TimeSeriesPeriod.OneYear;

    public static string LivePeriodSummaryAddress(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryLiveTemplate, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static string CompletePeriodSummaryAddress(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryCompleteTemplate, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static string PersistAppendPeriodSummaryPublish(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryPersistAppendPublish, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static string PeriodSummaryRepublish(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryPersistRepublish, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static string HistoricalPeriodSummaryStreamRequest(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryHistoricalStreamRequest, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static string HistoricalPeriodSummaryResponseRequest(this ISourceTickerId sourceTickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryHistoricalResponseRequest, sourceTickerId.Source, sourceTickerId.Ticker, period.ShortName());

    public static TimeSeriesPeriod RoundNonPersistPeriodsToTick(this TimeSeriesPeriod checkSmallPeriod)
    {
        if (checkSmallPeriod < PersistPeriodsFrom) return TimeSeriesPeriod.Tick;
        return checkSmallPeriod;
    }
}
