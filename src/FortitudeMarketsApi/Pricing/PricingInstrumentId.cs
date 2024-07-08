// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IPricingInstrumentId : IReusableObject<IPricingInstrumentId>, ISourceTickerId
{
    TimeSeriesPeriod Period { get; }

    InstrumentType InstrumentType { get; }

    new IPricingInstrumentId Clone();
}

public class PricingInstrumentIdentifier : SourceTickerIdentifier, IPricingInstrumentId
{
    public PricingInstrumentIdentifier() { }

    public PricingInstrumentIdentifier
        (ushort sourceId, ushort tickerId, string source, string ticker, TimeSeriesPeriod period, InstrumentType instrumentType) :
        base(sourceId, tickerId, source, ticker)
    {
        Period         = period;
        InstrumentType = instrumentType;
    }

    public PricingInstrumentIdentifier(IPricingInstrumentId toClone) : base(toClone) => Period = toClone.Period;

    public TimeSeriesPeriod Period { get; set; }

    public InstrumentType InstrumentType { get; set; }

    public IPricingInstrumentId CopyFrom(IPricingInstrumentId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        Period = source.Period;
        return this;
    }

    IReusableObject<IPricingInstrumentId> IStoreState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPricingInstrumentId)source, copyMergeFlags);

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    public override IPricingInstrumentId Clone() =>
        Recycler?.Borrow<PricingInstrumentIdentifier>().CopyFrom(this) ?? new PricingInstrumentIdentifier(this);

    public override void StateReset()
    {
        Period = TimeSeriesPeriod.None;
        base.StateReset();
    }
}

public readonly struct PricingInstrumentId // not inheriting from ISourceTickerId to prevent accidental boxing unboxing
{
    public PricingInstrumentId(IPricingInstrumentId pricingInstrumentId)
    {
        SourceId = pricingInstrumentId.SourceId;
        TickerId = pricingInstrumentId.TickerId;
        Period   = pricingInstrumentId.Period;

        InstrumentType = pricingInstrumentId.InstrumentType;
    }

    public PricingInstrumentId(ushort sourceId, ushort tickerId, TimeSeriesPeriod period, InstrumentType instrumentType)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Period   = period;

        InstrumentType = instrumentType;
    }

    public PricingInstrumentId(ISourceTickerId sourceTickerId, TimeSeriesPeriod period, InstrumentType instrumentType)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
        Period   = period;

        InstrumentType = instrumentType;
    }

    public PricingInstrumentId(SourceTickerId sourceTickerId, TimeSeriesPeriod period, InstrumentType instrumentType)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
        Period   = period;

        InstrumentType = instrumentType;
    }

    public PricingInstrumentId(ushort sourceId, ushort tickerId, PeriodInstrumentTypePair periodInstrumentTypePair)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Period   = periodInstrumentTypePair.Period;

        InstrumentType = periodInstrumentTypePair.InstrumentType;
    }

    public PricingInstrumentId(ISourceTickerId sourceTickerId, PeriodInstrumentTypePair periodInstrumentTypePair)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
        Period   = periodInstrumentTypePair.Period;

        InstrumentType = periodInstrumentTypePair.InstrumentType;
    }

    public PricingInstrumentId(SourceTickerId sourceTickerId, PeriodInstrumentTypePair periodInstrumentTypePair)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
        Period   = periodInstrumentTypePair.Period;

        InstrumentType = periodInstrumentTypePair.InstrumentType;
    }

    public uint   Id       => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; }
    public ushort TickerId { get; }

    public TimeSeriesPeriod Period { get; }

    public InstrumentType InstrumentType { get; }

    public string Ticker => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceId, TickerId);
    public string Source => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);


    public static implicit operator SourceTickerId(PricingInstrumentId sourceTickerId) => new(sourceTickerId.SourceId, sourceTickerId.TickerId);

    public static implicit operator PeriodInstrumentTypePair
        (PricingInstrumentId sourceTickerId) =>
        new(sourceTickerId.InstrumentType, sourceTickerId.Period);
}

public static class PricingInstrumentIdExtensions
{
    private static readonly ConcurrentMap<SourceTickerId, IMap<PeriodInstrumentTypePair, string>> SingleStringShortNameLookup = new();


    public static string GetReferenceShortName(this IPricingInstrumentId id) => GetReferenceShortName(id.ToPricingInstrumentId());

    public static string GetReferenceShortName(this PricingInstrumentId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id, out var tickerMap))
        {
            tickerMap = new ConcurrentMap<PeriodInstrumentTypePair, string>();
            SingleStringShortNameLookup.Add(id, tickerMap);
        }
        if (!tickerMap!.TryGetValue(id, out var shortName))
        {
            if (id.Source != SourceTickerIdentifierExtensions.NoSourceNameValue && id.Ticker != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}_{id.InstrumentType}_{id.Period.ShortName()}";
                tickerMap.Add(id, shortName);
            }
            else
            {
                shortName = $"{id.SourceId}-{id.TickerId}_{id.InstrumentType}_{id.Period.ShortName()}";
            }
        }
        return shortName!;
    }

    public static PricingInstrumentId ToPricingInstrumentId(this IPricingInstrumentId pricingInstrumentId) => new(pricingInstrumentId);

    public static PricingInstrumentId ToPricingInstrumentId(this ISourceTickerId id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerId id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerIdValue id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id.SourceId, id.TickerId, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this ISourceTickerId id, TimeSeriesPeriod period, InstrumentType instrumentType) =>
        new(id, period, instrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerId id, TimeSeriesPeriod period, InstrumentType instrumentType) =>
        new(id, period, instrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerIdValue id, TimeSeriesPeriod period, InstrumentType instrumentType) =>
        new(id.SourceId, id.TickerId, period, instrumentType);
}
