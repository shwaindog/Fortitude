// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public interface IPricingIndicator : IIndicator, IPricingInstrumentId
{
    long IndicatorSourceTickerId { get; }
}

public class PricingIndicator : PricingInstrumentId, IPricingIndicator
{
    public PricingIndicator()
    {
        IndicatorName = null!;

        IndicatorDescription = null!;
    }

    public PricingIndicator
    (ushort indicatorId, ushort sourceId, ushort tickerId, string indicatorName, string sourceName, string ticker, DiscreetTimePeriod coveringPeriod
      , IndicatorType indicatorType, MarketClassification marketClassification
      , string instrumentDescription = "No Description Given")
        : base(sourceId, tickerId, sourceName, ticker, coveringPeriod, InstrumentType.Indicator, marketClassification)
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

    public long IndicatorSourceTickerId => ((long)SourceInstrumentId << 32) | ((long)IndicatorId << 8);

    public ushort IndicatorId          { get; }
    public string IndicatorName        { get; }
    public string IndicatorDescription { get; set; }

    public IndicatorType IndicatorType { get; }
}

public readonly struct PeriodIndicatorTypePair(IndicatorType indicatorType, DiscreetTimePeriod coveringPeriod)
{
    public DiscreetTimePeriod CoveringPeriod { get; } = coveringPeriod;

    public IndicatorType IndicatorType { get; } = indicatorType;
}

public readonly struct PricingIndicatorId // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public PricingIndicatorId(IPricingIndicator pricingIndicator)
    {
        SourceId = pricingIndicator.SourceId;
        TickerId = pricingIndicator.InstrumentId;
        Category = pricingIndicator.Category;

        MarketClassification = pricingIndicator.MarketClassification;

        IndicatorId    = pricingIndicator.IndicatorId;
        IndicatorType  = pricingIndicator.IndicatorType;
        CoveringPeriod = pricingIndicator.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ushort sourceId, ushort tickerId, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod, MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, SourceTickerIdentifier sourceTickerIdentifier, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod, MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = sourceTickerIdentifier.SourceId;
        TickerId = sourceTickerIdentifier.InstrumentId;
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ushort sourceId, ushort tickerId, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, ISourceTickerId sourceTickerId, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.InstrumentId;
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, SourceTickerIdentifier sourceTickerIdentifier
      , PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = sourceTickerIdentifier.SourceId;
        TickerId = sourceTickerIdentifier.InstrumentId;
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, IPricingInstrumentId pricingInstrumentId, PeriodIndicatorTypePair periodIndicatorTypePair)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, IPricingInstrumentId pricingInstrumentId, DiscreetTimePeriod? coveringPeriod = null)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = IndicatorConstants.GetIndicatorType(indicatorId);
        CoveringPeriod = coveringPeriod ?? pricingInstrumentId.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, PricingInstrumentIdValue pricingInstrumentId, DiscreetTimePeriod? coveringPeriod = null)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = IndicatorConstants.GetIndicatorType(indicatorId);
        CoveringPeriod = coveringPeriod ?? pricingInstrumentId.CoveringPeriod;
    }

    public PricingIndicatorId
        (ushort indicatorId, PricingInstrumentIdValue pricingInstrumentId, PeriodIndicatorTypePair periodIndicatorTypePair)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, IPricingInstrumentId pricingInstrumentId, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (ushort indicatorId, PricingInstrumentIdValue pricingInstrumentId, IndicatorType indicatorType
      , DiscreetTimePeriod coveringPeriod)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.InstrumentId;
        Category = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        IndicatorId    = indicatorId;
        IndicatorType  = indicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public PricingIndicatorId
    (long indicatorSourceTickerId, PeriodIndicatorTypePair periodIndicatorTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId = (ushort)(indicatorSourceTickerId >> 48);
        TickerId = (ushort)((indicatorSourceTickerId >> 32) & 0xFFFF);
        Category = category;

        MarketClassification = marketClassification;

        IndicatorId    = (ushort)((indicatorSourceTickerId >> 16) & 0xFFFF);
        IndicatorType  = periodIndicatorTypePair.IndicatorType;
        CoveringPeriod = periodIndicatorTypePair.CoveringPeriod;
    }

    public PricingIndicatorId(PricingIndicatorId pricingIndicatorId, DiscreetTimePeriod coveringPeriod)
    {
        SourceId = pricingIndicatorId.SourceId;
        TickerId = pricingIndicatorId.TickerId;
        Category = pricingIndicatorId.Category;

        MarketClassification = pricingIndicatorId.MarketClassification;

        IndicatorId    = pricingIndicatorId.IndicatorId;
        IndicatorType  = pricingIndicatorId.IndicatorType;
        CoveringPeriod = coveringPeriod;
    }

    public long IndicatorSourceTickerId => ((long)SourceTickerId << 32) | ((long)IndicatorId << 8);
    public uint SourceTickerId          => (uint)((SourceId << 16) | TickerId);

    public ushort             IndicatorId    { get; }
    public DiscreetTimePeriod CoveringPeriod { get; }
    public IndicatorType      IndicatorType  { get; }

    public ushort SourceId { get; }
    public ushort TickerId { get; }

    public InstrumentType InstrumentType => InstrumentType.Indicator;

    public MarketClassification MarketClassification { get; }

    public string? Category { get; }

    public string InstrumentDescription => IndicatorExtensions.GetRegisteredIndicatorDescription(IndicatorId);
    public string IndicatorName         => IndicatorExtensions.GetRegisteredIndicatorName(IndicatorId);

    public string Ticker => SourceTickerIdentifierExtensions.GetRegisteredInstrumentName(SourceTickerId);
    public string Source => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);


    public static implicit operator SourceTickerIdentifier
        (PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId.SourceId, pricingIndicatorId.TickerId);

    public static implicit operator PeriodInstrumentTypePair(PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId.InstrumentType, pricingIndicatorId.CoveringPeriod);

    public static implicit operator PricingInstrumentIdValue(PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId, pricingIndicatorId.CoveringPeriod, pricingIndicatorId.InstrumentType, pricingIndicatorId.MarketClassification);

    public static implicit operator IndicatorIdentifierValue(PricingIndicatorId pricingIndicatorId) =>
        new(pricingIndicatorId.IndicatorId, pricingIndicatorId.IndicatorName, pricingIndicatorId.InstrumentDescription
          , pricingIndicatorId.IndicatorType, pricingIndicatorId.Source, pricingIndicatorId.CoveringPeriod);
}

public static class PricingIndicatorExtensions
{
    public const string NoPricingIndicatorShortNameValue = "Pricing Indicator Not Registered";

    private static readonly ConcurrentMap<long, PricingIndicatorId> PricingIndicatorLookup = new();

    private static readonly ConcurrentMap<long, string> PricingInstrumentShortNameLookup = new();

    public static PricingIndicatorId GetOrCreatePricingIndicatorWithCoveringPeriod
        (IPricingInstrumentId pricingInstrumentId, ushort indicatorId, DiscreetTimePeriod coveringPeriod)
    {
        var instrumentId = ConstructIndicatorSourceTickerId(pricingInstrumentId.SourceInstrumentId, indicatorId);
        if (PricingIndicatorLookup.ContainsKey(instrumentId)) return new PricingIndicatorId(PricingIndicatorLookup[instrumentId], coveringPeriod);
        var register = new PricingIndicatorId(indicatorId, pricingInstrumentId, coveringPeriod);
        PricingIndicatorLookup.Add(register.IndicatorSourceTickerId, register);
        return register;
    }

    public static long ConstructIndicatorSourceTickerId(uint sourceTickerId, ushort indicatorId) =>
        ((long)sourceTickerId << 32) | ((long)indicatorId << 16);

    public static bool Register(this PricingIndicatorId pricingIndicator)
    {
        var returnResult = false;
        new IndicatorIdentifierValue
            (pricingIndicator.IndicatorId, pricingIndicator.IndicatorName, pricingIndicator.InstrumentDescription
           , pricingIndicator.IndicatorType, pricingIndicator.Source, pricingIndicator.CoveringPeriod).Register();
        if (!PricingInstrumentShortNameLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
            if (pricingIndicator.Source != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                pricingIndicator.Ticker != SourceTickerIdentifierExtensions.NoTickerNameValue
             && pricingIndicator.IndicatorName.IsNotNullOrEmpty())
            {
                var shortName
                    = $"{pricingIndicator.Source}-{pricingIndicator.Ticker}_{pricingIndicator.IndicatorName}-{pricingIndicator.CoveringPeriod.ShortName()}";
                PricingInstrumentShortNameLookup.Add(pricingIndicator.IndicatorSourceTickerId, shortName);
                returnResult = true;
            }
        if (!PricingIndicatorLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
            PricingIndicatorLookup.Add(pricingIndicator.IndicatorSourceTickerId, pricingIndicator);

        return returnResult;
    }

    public static bool Register(this IPricingIndicator pricingIndicator)
    {
        var returnResult = false;
        IndicatorExtensions.Register(pricingIndicator);
        if (!PricingInstrumentShortNameLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
            if (pricingIndicator.SourceName != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                pricingIndicator.InstrumentName != SourceTickerIdentifierExtensions.NoTickerNameValue
             && pricingIndicator.IndicatorName.IsNotNullOrEmpty())
            {
                var shortName
                    = $"{pricingIndicator.SourceName}-{pricingIndicator.InstrumentName}_{pricingIndicator.IndicatorName}-{pricingIndicator.CoveringPeriod.ShortName()}";
                PricingInstrumentShortNameLookup.Add(pricingIndicator.IndicatorSourceTickerId, shortName);
                returnResult = true;
            }
        if (!PricingIndicatorLookup.ContainsKey(pricingIndicator.IndicatorSourceTickerId))
        {
            var pricingIndicatorId = new PricingIndicatorId(pricingIndicator);
            PricingIndicatorLookup.Add(pricingIndicator.IndicatorSourceTickerId, pricingIndicatorId);
        }

        return returnResult;
    }

    public static string GetReferenceShortName(this IPricingIndicator pricingIndicator) =>
        GetReferenceShortName(pricingIndicator.IndicatorSourceTickerId);

    public static PricingIndicatorId GetReferencePricingIndicator(this long indicatorSourceTickerId) =>
        PricingIndicatorLookup[indicatorSourceTickerId];

    public static string GetReferenceShortName(this long indicatorSourceTickerId)
    {
        if (!PricingInstrumentShortNameLookup.TryGetValue(indicatorSourceTickerId, out var shortName)) shortName = NoPricingIndicatorShortNameValue;
        return shortName!;
    }

    public static PricingIndicatorId ToPricingInstrumentId(this IPricingInstrumentId pricingInstrumentId, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)pricingInstrumentId.SourceInstrumentId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this PricingInstrumentIdValue pricingInstrumentId, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)pricingInstrumentId.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this ISourceTickerId id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceInstrumentId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this SourceTickerIdentifier id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceInstrumentId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }

    public static PricingIndicatorId ToPricingInstrumentId(this SourceTickerIdValue id, ushort indicatorId)
    {
        var indicatorSourceTickerId = ((long)id.SourceTickerId << 32) | ((long)indicatorId << 8);
        return indicatorSourceTickerId.GetReferencePricingIndicator();
    }
}
