// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.PeriodSummaries;

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


    public const TimeBoundaryPeriod PersistPeriodsFrom = TimeBoundaryPeriod.FifteenSeconds;
    public const TimeBoundaryPeriod PersistPeriodsTo   = TimeBoundaryPeriod.OneYear;

    public static string LivePeriodSummaryAddress(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryLiveTemplate, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string CompletePeriodSummaryAddress(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryCompleteTemplate, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string CompletePeriodSummaryAddress(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(PeriodSummaryCompleteTemplate, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());

    public static string PersistPreparePeriodSummaryPublish(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryPersistPreparePublish, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string PersistAppendPeriodSummaryPublish() => string.Format(PeriodSummaryPersistAppendPublish);

    public static string PeriodSummaryPublish(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryDefaultPublishAddress, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalPeriodSummaryStreamRequest(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryHistoricalStreamRequest, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalPeriodSummaryStreamRequest(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(PeriodSummaryHistoricalStreamRequest, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());

    public static string HistoricalPeriodSummaryResponseRequest(this PricingInstrumentId pricingInstrumentId) =>
        string.Format(PeriodSummaryHistoricalResponseRequest, pricingInstrumentId.Source, pricingInstrumentId.Ticker
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalPeriodSummaryResponseRequest(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(PeriodSummaryHistoricalResponseRequest, sourceTickerIdentifier.Source, sourceTickerIdentifier.Ticker, period.ShortName());

    public static TimeBoundaryPeriod RoundNonPersistPeriodsToTick(this TimeBoundaryPeriod checkSmallPeriod)
    {
        if (checkSmallPeriod < PersistPeriodsFrom) return TimeBoundaryPeriod.Tick;
        return checkSmallPeriod;
    }
}
