// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public interface IPricingIndicator : IIndicator, IPricingInstrumentId
{
    long IndicatorSourceTickerId { get; }

    TimePeriod CoveringPeriod { get; }
}

public class PricingIndicator : PricingInstrument, IPricingIndicator
{
    public PricingIndicator()
    {
        IndicatorName = null!;

        IndicatorDescription = null!;
    }

    public PricingIndicator
    (ushort indicatorId, ushort sourceId, ushort tickerId, string indicatorName, string source, string ticker, TimeSeriesPeriod entryPeriod
      , IndicatorType indicatorType, TimePeriod coveringPeriod, MarketClassification marketClassification
      , string instrumentDescription = "No Description Given")
        : base(sourceId, tickerId, source, ticker, entryPeriod, InstrumentType.Indicator, marketClassification)
    {
        IndicatorId    = indicatorId;
        IndicatorName  = indicatorName;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;

        IndicatorDescription = instrumentDescription;
    }

    public PricingIndicator(IPricingIndicator toClone) : base(toClone)
    {
        IndicatorId    = toClone.IndicatorId;
        IndicatorName  = toClone.IndicatorName;
        IndicatorType  = toClone.IndicatorType;
        CoveringPeriod = toClone.CoveringPeriod;

        IndicatorDescription = toClone.IndicatorDescription;
    }

    public PricingIndicator(PricingIndicatorId toClone) : base(toClone)
    {
        IndicatorId    = toClone.IndicatorId;
        IndicatorName  = toClone.IndicatorName;
        IndicatorType  = toClone.IndicatorType;
        CoveringPeriod = toClone.CoveringPeriod;

        IndicatorDescription = toClone.InstrumentDescription;
    }

    public long IndicatorSourceTickerId => ((long)SourceTickerId << 32) | ((long)IndicatorId << 8);

    public ushort IndicatorId   { get; }
    public string IndicatorName { get; }

    public string IndicatorDescription { get; set; }

    public TimePeriod CoveringPeriod { get; }

    public IndicatorType IndicatorType { get; }
}

public readonly struct PeriodIndicatorTypePair(IndicatorType indicatorType, TimePeriod coveringPeriod)
{
    public TimePeriod CoveringPeriod { get; } = coveringPeriod;

    public IndicatorType IndicatorType { get; } = indicatorType;
}

public readonly struct PricingIndicatorId // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public PricingIndicatorId(IPricingIndicator pricingIndicator)
    {
        SourceId    = pricingIndicator.SourceId;
        TickerId    = pricingIndicator.TickerId;
        EntryPeriod = pricingIndicator.EntryPeriod;
        Category    = pricingIndicator.Category;

        MarketClassification = pricingIndicator.MarketClassification;

        IndicatorId    = pricingIndicator.IndicatorId;
        IndicatorType  = pricingIndicator.IndicatorType;
        CoveringPeriod = pricingIndicator.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ushort sourceId, ushort tickerId, TimeSeriesPeriod entryPeriod, IndicatorType indicatorType
      , TimePeriod coveringPeriod, MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = sourceId;
        TickerId    = tickerId;
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod entryPeriod, IndicatorType indicatorType
      , TimePeriod coveringPeriod, MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = sourceTickerIdentifier.SourceId;
        TickerId    = sourceTickerIdentifier.TickerId;
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ushort sourceId, ushort tickerId, TimeSeriesPeriod entryPeriod, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = sourceId;
        TickerId    = tickerId;
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ISourceTickerId sourceTickerId, TimeSeriesPeriod entryPeriod, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = sourceTickerId.SourceId;
        TickerId    = sourceTickerId.TickerId;
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, SourceTickerIdentifier sourceTickerIdentifier, TimeSeriesPeriod entryPeriod, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = sourceTickerIdentifier.SourceId;
        TickerId    = sourceTickerIdentifier.TickerId;
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, IPricingInstrumentId pricingInstrumentId, PeriodIndicatorTypePair periodIndicatorTypePair)
    {
        SourceId    = pricingInstrumentId.SourceId;
        TickerId    = pricingInstrumentId.TickerId;
        EntryPeriod = pricingInstrumentId.EntryPeriod;
        Category    = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, PricingInstrumentId pricingInstrumentId, PeriodIndicatorTypePair periodIndicatorTypePair)
    {
        SourceId    = pricingInstrumentId.SourceId;
        TickerId    = pricingInstrumentId.TickerId;
        EntryPeriod = pricingInstrumentId.EntryPeriod;
        Category    = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, IPricingInstrumentId pricingInstrumentId, IndicatorType indicatorType
      , TimePeriod coveringPeriod)
    {
        SourceId    = pricingInstrumentId.SourceId;
        TickerId    = pricingInstrumentId.TickerId;
        EntryPeriod = pricingInstrumentId.EntryPeriod;
        Category    = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, PricingInstrumentId pricingInstrumentId, IndicatorType indicatorType
      , TimePeriod coveringPeriod)
    {
        SourceId    = pricingInstrumentId.SourceId;
        TickerId    = pricingInstrumentId.TickerId;
        EntryPeriod = pricingInstrumentId.EntryPeriod;
        Category    = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (long indicatorSourceTickerId, TimeSeriesPeriod entryPeriod, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId    = (ushort)(indicatorSourceTickerId >> 48);
        TickerId    = (ushort)((indicatorSourceTickerId >> 32) & 0xFFFF);
        EntryPeriod = entryPeriod;
        Category    = category;

        MarketClassification = marketClassification;

        IndicatorId    = (ushort)((indicatorSourceTickerId >> 16) & 0xFFFF);
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public long IndicatorSourceTickerId => ((long)SourceTickerId << 32) | ((long)IndicatorId << 8);
    public uint SourceTickerId          => (uint)((SourceId << 16) | TickerId);

    public ushort        IndicatorId    { get; }
    public TimePeriod    CoveringPeriod { get; }
    public IndicatorType IndicatorType  { get; }

    public ushort SourceId { get; }
    public ushort TickerId { get; }

    public TimeSeriesPeriod EntryPeriod    { get; }
    public InstrumentType   InstrumentType => InstrumentType.Indicator;

    public MarketClassification MarketClassification { get; }

    public string? Category { get; }

    public string InstrumentDescription => IndicatorSourceTickerId.GetReferenceIndicatorDescription();
    public string IndicatorName         => IndicatorSourceTickerId.GetReferenceIndicatorName();

    public string Ticker => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceTickerId);
    public string Source => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);


    public static implicit operator SourceTickerIdentifier
        (PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId.SourceId, pricingIndicatorId.TickerId);

    public static implicit operator PeriodInstrumentTypePair(PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId.InstrumentType, pricingIndicatorId.EntryPeriod);

    public static implicit operator PricingInstrumentId(PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId, pricingIndicatorId.EntryPeriod, pricingIndicatorId.InstrumentType, pricingIndicatorId.MarketClassification);
}

public static class PricingIndicatorExtensions
{
    public const string NoPricingIndicatorShortNameValue = "Pricing Indicator Not Registered";

    private static readonly ConcurrentMap<long, PricingIndicatorId> PricingIndicatorLookup = new();

    private static readonly ConcurrentMap<long, string> InstrumentNameLookup             = new();
    private static readonly ConcurrentMap<long, string> InstrumentDescriptionLookup      = new();
    private static readonly ConcurrentMap<long, string> PricingInstrumentShortNameLookup = new();

    public static bool Register(this IPricingIndicator pricingIndicator)
    {
        var returnResult = false;
        if (!InstrumentNameLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId) && pricingIndicator.IndicatorName.IsNotNullOrEmpty())
            InstrumentNameLookup.AddOrUpdate(pricingIndicator.IndicatorSourceTickerId, pricingIndicator.IndicatorName);
        if (!InstrumentDescriptionLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId) &&
            pricingIndicator.IndicatorDescription.IsNotNullOrEmpty())
            InstrumentDescriptionLookup.AddOrUpdate(pricingIndicator.IndicatorSourceTickerId, pricingIndicator.IndicatorDescription);
        if (!PricingInstrumentShortNameLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
            if (pricingIndicator.Source != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                pricingIndicator.Ticker != SourceTickerIdentifierExtensions.NoTickerNameValue
             && pricingIndicator.IndicatorName.IsNotNullOrEmpty())
            {
                var shortName
                    = $"{pricingIndicator.Source}-{pricingIndicator.Ticker}_{pricingIndicator.IndicatorName}-{pricingIndicator.CoveringPeriod.ShortName()}";
                InstrumentNameLookup.Add(pricingIndicator.IndicatorSourceTickerId, shortName);
                returnResult = true;
            }
        if (!PricingIndicatorLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
        {
            var pricingIndicatorId = new PricingIndicatorId(pricingIndicator);
            PricingIndicatorLookup.Add(pricingIndicator.IndicatorSourceTickerId, pricingIndicatorId);
        }

        return returnResult;
    }

    public static string GetReferenceShortName
        (this IPricingIndicator pricingIndicator) =>
        GetReferenceShortName(pricingIndicator.IndicatorSourceTickerId);

    public static string GetReferenceIndicatorName(this PricingIndicatorId pricingInstrument)
    {
        if (!InstrumentNameLookup.TryGetValue(pricingInstrument.IndicatorSourceTickerId, out var shortName))
            shortName
                = $"{SourceTickerIdentifierExtensions.NoSourceTickerShortNameValue}_{pricingInstrument.IndicatorName}-{pricingInstrument.CoveringPeriod.ShortName()}";
        return shortName!;
    }

    public static string GetReferenceIndicatorDescription(this PricingIndicatorId pricingInstrument)
    {
        if (!InstrumentDescriptionLookup.TryGetValue(pricingInstrument.IndicatorSourceTickerId, out var shortName))
            shortName
                = $"{SourceTickerIdentifierExtensions.NoSourceTickerShortNameValue}_{pricingInstrument.IndicatorName}-{pricingInstrument.CoveringPeriod.ShortName()}";
        return shortName!;
    }

    public static PricingIndicatorId GetReferencePricingIndicator
        (this long indicatorSourceTickerId) =>
        PricingIndicatorLookup[indicatorSourceTickerId];

    public static string GetReferenceIndicatorName(this long indicatorSourceTickerId)
    {
        if (!InstrumentNameLookup.TryGetValue(indicatorSourceTickerId, out var shortName)) shortName = NoPricingIndicatorShortNameValue;
        return shortName!;
    }

    public static string GetReferenceIndicatorDescription(this long indicatorSourceTickerId)
    {
        if (!InstrumentDescriptionLookup.TryGetValue(indicatorSourceTickerId, out var shortName)) shortName = NoPricingIndicatorShortNameValue;
        return shortName!;
    }

    public static string GetReferenceShortName(this long indicatorSourceTickerId)
    {
        if (!PricingInstrumentShortNameLookup.TryGetValue(indicatorSourceTickerId, out var shortName)) shortName = NoPricingIndicatorShortNameValue;
        return shortName!;
    }

    public static PricingIndicatorId ToPricingInstrumentId(this IPricingInstrumentId pricingInstrumentId, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)pricingInstrumentId.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this PricingInstrumentId pricingInstrumentId, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)pricingInstrumentId.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this ISourceTickerId id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this SourceTickerIdentifier id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this SourceTickerIdValue id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }
}
