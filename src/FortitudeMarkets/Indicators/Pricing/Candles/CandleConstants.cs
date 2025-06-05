// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles;

public static class CandleConstants
{
    public const string CandleBase                      = $"{IndicatorServiceConstants.IndicatorsBase}.Candles";
    public const string CandleLiveTemplate              = $"{CandleBase}.Live.{{0}}.{{1}}.{{2}}";
    public const string CandleCompleteTemplate          = $"{CandleBase}.Complete.{{0}}.{{1}}.{{2}}";
    public const string CandlePersistAppendPublish      = $"{CandleBase}.Persist.Append";
    public const string CandlePersistPreparePublish     = $"{CandleBase}.Persist.{{0}}.{{1}}.{{2}}.Prepare";
    public const string CandleDefaultPublishAddress     = $"{CandleBase}.Historical.{{0}}.{{1}}.{{2}}.Publish";
    public const string CandleHistoricalStreamRequest   = $"{CandleBase}.Historical.{{0}}.{{1}}.{{2}}.Stream.Request";
    public const string CandleHistoricalResponseRequest = $"{CandleBase}.Historical.{{0}}.{{1}}.{{2}}.RequestResponse";


    public const TimeBoundaryPeriod PersistPeriodsFrom = TimeBoundaryPeriod.FifteenSeconds;
    public const TimeBoundaryPeriod PersistPeriodsTo   = TimeBoundaryPeriod.OneYear;

    public static string LiveCandleAddress(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandleLiveTemplate, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string CompleteCandleAddress(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandleCompleteTemplate, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string CompleteCandleAddress(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(CandleCompleteTemplate, sourceTickerIdentifier.SourceName, sourceTickerIdentifier.InstrumentName, period.ShortName());

    public static string PersistPrepareCandlePublish(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandlePersistPreparePublish, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string PersistAppendCandlePublish() => string.Format(CandlePersistAppendPublish);

    public static string CandlePublish(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandleDefaultPublishAddress, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalCandleStreamRequest(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandleHistoricalStreamRequest, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalCandleStreamRequest(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(CandleHistoricalStreamRequest, sourceTickerIdentifier.SourceName, sourceTickerIdentifier.InstrumentName, period.ShortName());

    public static string HistoricalCandleResponseRequest(this PricingInstrumentIdValue pricingInstrumentId) =>
        string.Format(CandleHistoricalResponseRequest, pricingInstrumentId.SourceName, pricingInstrumentId.InstrumentName
                    , pricingInstrumentId.CoveringPeriod.ShortName());

    public static string HistoricalCandleResponseRequest(this SourceTickerIdentifier sourceTickerIdentifier, TimeBoundaryPeriod period) =>
        string.Format(CandleHistoricalResponseRequest, sourceTickerIdentifier.SourceName, sourceTickerIdentifier.InstrumentName, period.ShortName());

    public static TimeBoundaryPeriod RoundNonPersistPeriodsToTick(this TimeBoundaryPeriod checkSmallPeriod)
    {
        if (checkSmallPeriod < PersistPeriodsFrom) return TimeBoundaryPeriod.Tick;
        return checkSmallPeriod;
    }
}
