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
    public const string PeriodSummaryPersistAppendPublish      = $"{PeriodSummaryBase}.Persist.Append";
    public const string PeriodSummaryPersistPreparePublish     = $"{PeriodSummaryBase}.Persist.{{0}}.{{1}}.{{2}}.Prepare";
    public const string PeriodSummaryDefaultPublishAddress     = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.Publish";
    public const string PeriodSummaryHistoricalStreamRequest   = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.Stream.Request";
    public const string PeriodSummaryHistoricalResponseRequest = $"{PeriodSummaryBase}.Historical.{{0}}.{{1}}.{{2}}.RequestResponse";


    public const TimeSeriesPeriod PersistPeriodsFrom = TimeSeriesPeriod.FifteenSeconds;
    public const TimeSeriesPeriod PersistPeriodsTo   = TimeSeriesPeriod.OneYear;

    public static string LivePeriodSummaryAddress(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryLiveTemplate, pricingInstrumentId.Source, pricingInstrumentId.Ticker, pricingInstrumentId.EntryPeriod.ShortName());

    public static string CompletePeriodSummaryAddress(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryCompleteTemplate, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.EntryPeriod.ShortName());

    public static string CompletePeriodSummaryAddress(this SourceTickerId tickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryCompleteTemplate, tickerId.Source, tickerId.Ticker, period.ShortName());

    public static string PersistPreparePeriodSummaryPublish(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryPersistPreparePublish, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.EntryPeriod.ShortName());

    public static string PersistAppendPeriodSummaryPublish() => string.Format(PeriodSummaryPersistAppendPublish);

    public static string PeriodSummaryPublish(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryDefaultPublishAddress, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.EntryPeriod.ShortName());

    public static string HistoricalPeriodSummaryStreamRequest(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryHistoricalStreamRequest, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.EntryPeriod.ShortName());

    public static string HistoricalPeriodSummaryStreamRequest(this SourceTickerId tickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryHistoricalStreamRequest, tickerId.Source, tickerId.Ticker, period.ShortName());

    public static string HistoricalPeriodSummaryResponseRequest(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryHistoricalResponseRequest, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.EntryPeriod.ShortName());

    public static string HistoricalPeriodSummaryResponseRequest(this SourceTickerId tickerId, TimeSeriesPeriod period) =>
        string.Format(PeriodSummaryHistoricalResponseRequest, tickerId.Source, tickerId.Ticker, period.ShortName());

    public static TimeSeriesPeriod RoundNonPersistPeriodsToTick(this TimeSeriesPeriod checkSmallPeriod)
    {
        if (checkSmallPeriod < PersistPeriodsFrom) return TimeSeriesPeriod.Tick;
        return checkSmallPeriod;
    }
}
