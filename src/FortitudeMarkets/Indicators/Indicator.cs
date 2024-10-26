// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarkets.Indicators;

public enum IndicatorType : byte
{
    Unknown
  , MarketPrice
  , MarketPriceSummary
  , Oscillator
  , Averaging
  , Momentum
  , SignalAggregating
  , SignalFiltering
  , MarketBehaviorChange
  , TrendReversing
  , MeanReverting
  , RangeChange
  , CorrelatedInstrument
  , BreakoutDetecting
  , VolatilityStatus
  , EconomicCalendar
  , MarketAnnouncement
  , MarketOutlook
  , CurrentSentiment
  , MarketEvent
  , MarketNews
  , MarketAnalysis
  , AggregateUpdate
  , SupportResistance
  , KeyPriceLevel
  , TickerActivity
  , TradingSize
  , TradingStatus
  , SignalModification
  , SignalMonitoring
}

public interface IIndicator : IInstrument
{
    ushort IndicatorId   { get; }
    string IndicatorName { get; }

    string IndicatorDescription { get; }

    IndicatorType IndicatorType { get; }
}

public class Indicator : Instrument, IIndicator
{
    public Indicator
    (ushort indicatorId, string indicatorName, string instrumentSource, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod, string description = "No Description Given")
        : base(indicatorName, instrumentSource, InstrumentType.Indicator, coveringPeriod)
    {
        IndicatorId   = indicatorId;
        IndicatorType = indicatorType;

        IndicatorDescription = description;
    }

    public Indicator
    (ushort indicatorId, string instrumentName, string instrumentSource, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod, string description = "No Description Given", params KeyValuePair<string, string>[] requiredValues)
        : base(instrumentName, instrumentSource, InstrumentType.Indicator, coveringPeriod, requiredValues)
    {
        IndicatorId   = indicatorId;
        IndicatorType = indicatorType;

        IndicatorDescription = description;
    }

    public ushort IndicatorId   { get; set; }
    public string IndicatorName => InstrumentName;

    public string IndicatorDescription { get; set; }

    public IndicatorType IndicatorType { get; set; }
}

public readonly struct IndicatorIdentifier
{
    public IndicatorIdentifier(ushort indicatorId, string source, DiscreetTimePeriod? coveringPeriod = null)
    {
        IndicatorId      = indicatorId;
        InstrumentSource = source;
        CoveringPeriod   = coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
    }

    public IndicatorIdentifier(IIndicator toClone)
    {
        IndicatorId      = toClone.IndicatorId;
        InstrumentSource = toClone.InstrumentSource;
        CoveringPeriod   = toClone.CoveringPeriod;
    }

    public ushort IndicatorId      { get; }
    public string InstrumentSource { get; }

    public DiscreetTimePeriod CoveringPeriod { get; }

    public string IndicatorName => IndicatorExtensions.GetRegisteredIndicatorName(IndicatorId);

    public string IndicatorDescription => IndicatorExtensions.GetRegisteredIndicatorDescription(IndicatorId);

    public IndicatorType IndicatorType => IndicatorExtensions.GetRegisteredIndicatorType(IndicatorId);

    public InstrumentType InstrumentType =>
        IndicatorExtensions.GetRegisteredIndicatorIdentifierValue(IndicatorId)?.InstrumentType ?? InstrumentType.Unknown;

    public static implicit operator IndicatorIdentifier(Indicator indicator) => new(indicator);
}

public readonly struct IndicatorIdentifierValue
{
    public IndicatorIdentifierValue
    (ushort indicatorId, string indicatorName, string indicatorDescription, IndicatorType indicatorType
      , string indicatorSource, DiscreetTimePeriod coveringPeriod)
    {
        IndicatorId   = indicatorId;
        IndicatorName = indicatorName;
        IndicatorType = indicatorType;

        IndicatorDescription = indicatorDescription;
        InstrumentSource     = indicatorSource;
        CoveringPeriod       = coveringPeriod;
    }

    public IndicatorIdentifierValue(IIndicator toClone)
    {
        IndicatorId   = toClone.IndicatorId;
        IndicatorName = toClone.IndicatorName;
        IndicatorType = toClone.IndicatorType;

        IndicatorDescription = toClone.IndicatorDescription;
        InstrumentSource     = toClone.InstrumentSource;
        CoveringPeriod       = toClone.CoveringPeriod;
    }

    public IndicatorIdentifierValue(IndicatorIdentifier toClone)
    {
        IndicatorId   = toClone.IndicatorId;
        IndicatorName = toClone.IndicatorName;
        IndicatorType = toClone.IndicatorType;

        IndicatorDescription = toClone.IndicatorDescription;
        InstrumentSource     = toClone.InstrumentSource;
        CoveringPeriod       = toClone.CoveringPeriod;
    }

    public ushort IndicatorId { get; }

    public string IndicatorName { get; }

    public string InstrumentSource { get; }

    public DiscreetTimePeriod CoveringPeriod { get; }

    public string IndicatorDescription { get; }

    public IndicatorType IndicatorType { get; }

    public InstrumentType InstrumentType
    {
        get
        {
            return IndicatorType switch
                   {
                       IndicatorType.MarketPrice        => InstrumentType.Price
                     , IndicatorType.MarketPriceSummary => InstrumentType.PriceSummaryPeriod
                     , _                                => InstrumentType.Indicator
                   };
        }
    }


    public static implicit operator IndicatorIdentifierValue(Indicator indicator) => new((IIndicator)indicator);
}

public static class IndicatorExtensions
{
    public const string NoIndicatorNameValue        = "Indicator Name Not Registered";
    public const string NoIndicatorSourceValue      = "Indicator Instrument Source Not Registered";
    public const string NoIndicatorDescriptionValue = "Indicator Description Not Registered";

    private static readonly ConcurrentMap<ushort, IndicatorIdentifierValue> IdToIndicatorIdentifierValueLookup = new();

    public static bool Register(this IIndicator id)
    {
        if (!IdToIndicatorIdentifierValueLookup.ContainsKey(id.IndicatorId))
        {
            IdToIndicatorIdentifierValueLookup.AddOrUpdate(id.IndicatorId, new IndicatorIdentifierValue(id));
            return true;
        }
        return false;
    }

    public static bool Register(this IndicatorIdentifierValue id)
    {
        if (!IdToIndicatorIdentifierValueLookup.ContainsKey(id.IndicatorId))
            if (id.IndicatorName != NoIndicatorNameValue && id.IndicatorDescription != NoIndicatorDescriptionValue)
            {
                IdToIndicatorIdentifierValueLookup.AddOrUpdate(id.IndicatorId, id);
                return true;
            }
        return false;
    }

    public static IndicatorIdentifierValue? GetRegisteredIndicatorIdentifierValue
        (ushort indicatorId) =>
        IdToIndicatorIdentifierValueLookup.ContainsKey(indicatorId) ? IdToIndicatorIdentifierValueLookup[indicatorId] : default;

    public static string GetRegisteredIndicatorName
        (ushort indicatorId) =>
        IdToIndicatorIdentifierValueLookup.ContainsKey(indicatorId)
            ? IdToIndicatorIdentifierValueLookup[indicatorId].IndicatorName
            : NoIndicatorNameValue;

    public static string GetRegisteredIndicatorDescription
        (ushort indicatorId) =>
        IdToIndicatorIdentifierValueLookup.ContainsKey(indicatorId)
            ? IdToIndicatorIdentifierValueLookup[indicatorId].IndicatorDescription
            : NoIndicatorDescriptionValue;

    public static IndicatorType GetRegisteredIndicatorType
        (ushort indicatorId) =>
        IdToIndicatorIdentifierValueLookup.ContainsKey(indicatorId)
            ? IdToIndicatorIdentifierValueLookup[indicatorId].IndicatorType
            : IndicatorType.Unknown;
}
