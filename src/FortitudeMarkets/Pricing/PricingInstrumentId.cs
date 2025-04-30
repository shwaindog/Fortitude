// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarkets.Configuration.ClientServerConfig;

#endregion

namespace FortitudeMarkets.Pricing;

public interface IPricingInstrumentId : IReusableObject<IPricingInstrumentId>, ISourceTickerId, IInstrument
{
    [JsonIgnore] MarketClassification MarketClassification { get; set; }

    [JsonIgnore] string? Category { get; set; }

    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DiscreetTimePeriod CoveringPeriod { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new InstrumentType InstrumentType { get; set; }

    new IPricingInstrumentId Clone();
}

public class PricingInstrument : SourceTicker, IPricingInstrumentId
{
    private DiscreetTimePeriod? coveringPeriod;

    private string[]? optionalKeys;
    private string[]? requiredKeys;

    private InstrumentType? timeSeriesType;

    public PricingInstrument()
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = MarketClassificationExtensions.Unknown;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrument
    (ushort sourceId, ushort tickerId, string sourceName, string ticker, DiscreetTimePeriod coveringPeriod, InstrumentType instrumentType
      , MarketClassification marketClassification, string? category = null) :
        base(sourceId, tickerId, sourceName, ticker)
    {
        CoveringPeriod       = coveringPeriod;
        MarketClassification = marketClassification;
        Category             = category;
        InstrumentType       = instrumentType;
    }

    public PricingInstrument(IPricingInstrumentId toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        InstrumentType       = toClone.InstrumentType;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;
    }

    public PricingInstrument(SourceTickerIdentifier toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = MarketClassificationExtensions.Unknown;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrument(SourceTickerIdValue toClone) : base(toClone)
    {
        CoveringPeriod       = new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        MarketClassification = MarketClassificationExtensions.Unknown;
        InstrumentType       = InstrumentType.Price;
    }

    public PricingInstrument(PricingInstrumentId toClone) : base(toClone)
    {
        CoveringPeriod       = toClone.CoveringPeriod;
        InstrumentType       = toClone.InstrumentType;
        MarketClassification = toClone.MarketClassification;
        Category             = toClone.Category;
    }

    public PricingInstrument(Instrument toClone)
    {
        CoveringPeriod = toClone.CoveringPeriod;
        InstrumentType = toClone.InstrumentType;
        var marketType   = toClone[nameof(RepositoryPathName.MarketType)] ?? "Unknown";
        var productType  = toClone[nameof(RepositoryPathName.MarketProductType)] ?? "Unknown";
        var marketRegion = toClone[nameof(RepositoryPathName.MarketRegion)] ?? "Unknown";
        var marketClassification = new MarketClassification
            (Enum.Parse<MarketType>(marketType), Enum.Parse<ProductType>(productType), Enum.Parse<MarketRegion>(marketRegion));
        MarketClassification = marketClassification;
        Category             = toClone[nameof(RepositoryPathName.Category)];
    }

    public DiscreetTimePeriod CoveringPeriod
    {
        get => coveringPeriod ?? new DiscreetTimePeriod(TimeBoundaryPeriod.Tick);
        set => coveringPeriod = value;
    }

    [JsonIgnore] string IInstrument.InstrumentName => InstrumentName;
    [JsonIgnore] string IInstrument.SourceName     => SourceName;

    public InstrumentType InstrumentType
    {
        get => timeSeriesType ?? InstrumentType.Price;
        set => timeSeriesType = value;
    }

    [JsonIgnore]
    public string? this[string key]
    {
        get
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):        return MarketClassification.MarketType.ToString();
                case nameof(RepositoryPathName.MarketProductType): return MarketClassification.ProductType.ToString();
                case nameof(RepositoryPathName.MarketRegion):      return MarketClassification.MarketRegion.ToString();
                case nameof(RepositoryPathName.Category):          return Category;
            }
            return null;
        }
        set
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):
                    if (Enum.TryParse<MarketType>(value, true, out var marketType))
                        if (MarketClassification.MarketType == MarketType.Unknown)
                            MarketClassification = MarketClassification.SetMarketType(marketType);
                    break;
                case nameof(RepositoryPathName.MarketProductType):
                    if (Enum.TryParse<ProductType>(value, true, out var productType))
                        if (MarketClassification.ProductType == ProductType.Unknown)
                            MarketClassification = MarketClassification.SetProductType(productType);
                    break;
                case nameof(RepositoryPathName.MarketRegion):
                    if (Enum.TryParse<MarketRegion>(value, true, out var marketRegion))
                        if (MarketClassification.MarketRegion == MarketRegion.Unknown)
                            MarketClassification = MarketClassification.SetMarketRegion(marketRegion);
                    break;
                case nameof(RepositoryPathName.Category):
                    if (Category.IsNullOrEmpty()) Category = value ?? "";
                    break;
            }
        }
    }


    public IEnumerable<KeyValuePair<string, string>> FilledAttributes
    {
        get
        {
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
            if (MarketClassification.MarketType != MarketType.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            if (MarketClassification.ProductType != ProductType.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType)
                                                            , MarketClassification.ProductType.ToString());
            if (MarketClassification.MarketRegion != MarketRegion.Unknown)
                yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
        }
    }

    public void Add(KeyValuePair<string, string> instrumentAttribute)
    {
        this[instrumentAttribute.Key] = instrumentAttribute.Value;
    }

    public void Add(string name, string value)
    {
        this[name] = value;
    }

    public bool Remove(string name)
    {
        switch (name)
        {
            case nameof(RepositoryPathName.MarketType):
                MarketClassification = MarketClassification.SetMarketType(MarketType.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketProductType):
                MarketClassification = MarketClassification.SetProductType(ProductType.Unknown);
                return true;
            case nameof(RepositoryPathName.MarketRegion):
                MarketClassification = MarketClassification.SetMarketRegion(MarketRegion.Unknown);
                return true;
            case nameof(RepositoryPathName.Category):
                Category = null;
                return true;
        }
        return false;
    }

    public IEnumerable<string> RequiredAttributeKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public IEnumerable<string> OptionalAttributeKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public IEnumerable<KeyValuePair<string, string>> AllAttributes
    {
        get
        {
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        }
    }

    public IEnumerable<KeyValuePair<string, string>> RequiredAttributes
    {
        get
        {
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
        }
    }

    public IEnumerable<KeyValuePair<string, string>> OptionalAttributes
    {
        get
        {
            if (Category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), Category);
        }
    }


    public bool HasAllRequiredKeys =>
        SourceName.IsNotNullOrEmpty() && MarketClassification.MarketType != MarketType.Unknown
                                      && MarketClassification.ProductType != ProductType.Unknown &&
                                         MarketClassification.MarketRegion != MarketRegion.Unknown;

    public MarketClassification MarketClassification { get; set; }

    public string? Category { get; set; }


    public IPricingInstrumentId CopyFrom(IPricingInstrumentId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        CoveringPeriod       = source.CoveringPeriod;
        MarketClassification = source.MarketClassification;
        InstrumentType       = source.InstrumentType;
        return this;
    }

    IReusableObject<IPricingInstrumentId> IStoreState<IReusableObject<IPricingInstrumentId>>.CopyFrom
        (IReusableObject<IPricingInstrumentId> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPricingInstrumentId)source, copyMergeFlags);

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    public override IPricingInstrumentId Clone() =>
        Recycler?.Borrow<PricingInstrument>().CopyFrom(this) ?? new PricingInstrument((IPricingInstrumentId)this);

    public override void StateReset()
    {
        MarketClassification = new MarketClassification();

        Category       = null;
        CoveringPeriod = new DiscreetTimePeriod();

        base.StateReset();
    }


    public override bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other is not IPricingInstrumentId pricingInstrumentId) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        var marketClassificationSame = Equals(MarketClassification, pricingInstrumentId?.MarketClassification);
        var coveringPeriodSame       = coveringPeriod == pricingInstrumentId?.CoveringPeriod;
        var instrumentTypeSame       = timeSeriesType == pricingInstrumentId?.InstrumentType;
        var categorySame             = true;
        if (exactTypes) categorySame = Category == pricingInstrumentId?.Category;

        return baseSame && coveringPeriodSame && marketClassificationSame && instrumentTypeSame && categorySame;
    }

    protected bool Equals(PricingInstrument other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PricingInstrument)obj);
    }

    public override int GetHashCode() => HashCode.Combine(coveringPeriod, timeSeriesType, MarketClassification);
}

public readonly struct PricingInstrumentId // not inheriting from IPricingInstrumentId to prevent accidental boxing unboxing
{
    public PricingInstrumentId(IPricingInstrumentId pricingInstrumentId)
    {
        SourceId       = pricingInstrumentId.SourceId;
        TickerId       = pricingInstrumentId.InstrumentId;
        CoveringPeriod = pricingInstrumentId.CoveringPeriod;
        Category       = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        InstrumentType = pricingInstrumentId.InstrumentType;
    }

    public PricingInstrumentId
    (ushort sourceId, ushort tickerId, DiscreetTimePeriod coveringPeriod, InstrumentType instrumentType
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId       = sourceId;
        TickerId       = tickerId;
        CoveringPeriod = coveringPeriod;
        Category       = category;

        InstrumentType = instrumentType;

        MarketClassification = marketClassification;
    }

    public PricingInstrumentId
    (SourceTickerIdentifier sourceTickerIdentifier, DiscreetTimePeriod coveringPeriod, InstrumentType instrumentType
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        TickerId       = sourceTickerIdentifier.TickerId;
        CoveringPeriod = coveringPeriod;
        Category       = category;

        InstrumentType = instrumentType;

        MarketClassification = marketClassification;
    }

    public PricingInstrumentId
    (ushort sourceId, ushort tickerId, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId       = sourceId;
        TickerId       = tickerId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;
    }

    public PricingInstrumentId
    (ISourceTickerId sourceTickerId, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId       = sourceTickerId.SourceId;
        TickerId       = sourceTickerId.InstrumentId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;
    }

    public PricingInstrumentId
    (SourceTickerIdentifier sourceTickerIdentifier, PeriodInstrumentTypePair periodInstrumentTypePair
      , MarketClassification marketClassification = default, string? category = null)
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        TickerId       = sourceTickerIdentifier.TickerId;
        CoveringPeriod = periodInstrumentTypePair.CoveringPeriod;
        Category       = category;

        InstrumentType = periodInstrumentTypePair.InstrumentType;

        MarketClassification = marketClassification;
    }

    public uint SourceTickerId => (uint)((SourceId << 16) | TickerId);

    public ushort SourceId { get; }
    public ushort TickerId { get; }

    public DiscreetTimePeriod CoveringPeriod { get; }
    public InstrumentType     InstrumentType { get; }

    public MarketClassification MarketClassification { get; }

    public string? Category { get; }

    public string Ticker => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceTickerId);
    public string Source => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);

    public override string ToString() =>
        $"{nameof(PricingInstrumentId)}({nameof(SourceId)}: {SourceId}, {nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker}, " +
        $"{nameof(Source)}: {Source}, {nameof(CoveringPeriod)}: {CoveringPeriod}, {nameof(InstrumentType)}: {InstrumentType}, " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(Category)}: {Category})";


    public static implicit operator SourceTickerIdentifier(PricingInstrumentId sourceTickerId) =>
        new(sourceTickerId.SourceId, sourceTickerId.TickerId);

    public static implicit operator PeriodInstrumentTypePair(PricingInstrumentId sourceTickerId) =>
        new(sourceTickerId.InstrumentType, sourceTickerId.CoveringPeriod);
}

public static class PricingInstrumentIdExtensions
{
    private static readonly ConcurrentMap<uint, string> SingleStringShortNameLookup = new();

    private static readonly ConcurrentMap<uint, PricingInstrumentId> PricingInstrumentIdLookup = new();

    public static bool Register(this IPricingInstrumentId id)
    {
        SourceTickerIdentifierExtensions.Register(id);
        if (!PricingInstrumentIdLookup.ContainsKey(id.SourceTickerId))
        {
            var pricingInstrumentId
                = new PricingInstrumentId(id.SourceId, id.InstrumentId, new PeriodInstrumentTypePair(id.InstrumentType, id.CoveringPeriod));
            PricingInstrumentIdLookup.Add(id.SourceTickerId, pricingInstrumentId);
        }
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
            if (id.SourceName != SourceTickerIdentifierExtensions.NoSourceNameValue &&
                id.InstrumentName != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.SourceName}-{id.InstrumentName}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
                return true;
            }
        return false;
    }

    public static bool Register(this PricingInstrumentId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
            if (id.Source != SourceTickerIdentifierExtensions.NoSourceNameValue && id.Ticker != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
                return true;
            }
        return false;
    }

    public static string GetReferenceShortName(this IPricingInstrumentId id) => GetReferenceShortName(id.ToPricingInstrumentId());

    public static string GetReferenceShortName(this PricingInstrumentId id)
    {
        if (!SingleStringShortNameLookup.TryGetValue(id.SourceTickerId, out var shortName))
        {
            if (id.Source != SourceTickerIdentifierExtensions.NoSourceNameValue && id.Ticker != SourceTickerIdentifierExtensions.NoTickerNameValue)
            {
                shortName = $"{id.Source}-{id.Ticker}_{id.InstrumentType}-{id.CoveringPeriod.ShortName()}";
                SingleStringShortNameLookup.Add(id.SourceTickerId, shortName);
            }
            else
            {
                shortName = $"{SourceTickerIdentifierExtensions.NoSourceTickerShortNameValue}_{id.InstrumentType}_{id.CoveringPeriod.ShortName()}";
            }
        }
        return shortName!;
    }

    public static PricingInstrumentId ToPricingInstrumentId(this IPricingInstrumentId pricingInstrumentId) => new(pricingInstrumentId);

    public static PricingInstrumentId ToPricingInstrumentId(this ISourceTickerId id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerIdentifier id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerIdValue id, PeriodInstrumentTypePair periodInstrumentType) =>
        new(id.SourceId, id.TickerId, periodInstrumentType);

    public static PricingInstrumentId ToPricingInstrumentId
        (this SourceTickerIdentifier id, DiscreetTimePeriod period, InstrumentType instrumentType) =>
        new(id, period, instrumentType);

    public static PricingInstrumentId ToPricingInstrumentId(this SourceTickerIdValue id, DiscreetTimePeriod period, InstrumentType instrumentType) =>
        new(id.SourceId, id.TickerId, period, instrumentType);
}
