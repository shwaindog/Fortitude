// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Globalization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISourceTickerIdentifier
{
    uint   Id       { get; }
    ushort SourceId { get; }
    ushort TickerId { get; }
    string Source   { get; }
    string Ticker   { get; }
}

public struct SourceTickerIdentifier : ISourceTickerIdentifier
{
    public SourceTickerIdentifier(ushort sourceId, ushort tickerId, string source, string ticker)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;
    }

    public uint   Id       => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; }
    public ushort TickerId { get; }
    public string Source   { get; }
    public string Ticker   { get; }
}

public interface ISourceTickerQuoteInfo : ISourceTickerIdentifier, IInterfacesComparable<ISourceTickerQuoteInfo>, IVersionedMessage, IInstrument
{
    new ushort SourceId { get; set; }
    new ushort TickerId { get; set; }
    new string Source   { get; set; }
    new string Ticker   { get; set; }

    new MarketClassification MarketClassification { get; set; }

    QuoteLevel PublishedQuoteLevel { get; set; }

    decimal    RoundingPrecision      { get; set; }
    decimal    MinSubmitSize          { get; set; }
    decimal    MaxSubmitSize          { get; set; }
    decimal    IncrementSize          { get; set; }
    ushort     MinimumQuoteLife       { get; set; }
    LayerFlags LayerFlags             { get; set; }
    byte       MaximumPublishedLayers { get; set; }

    LastTradedFlags LastTradedFlags { get; set; }
    string          FormatPrice     { get; }

    new ISourceTickerQuoteInfo Clone();
}

public class SourceTickerQuoteInfo : ReusableObject<ISourceTickerQuoteInfo>, ISourceTickerQuoteInfo
{
    private string? category;

    private TimeSeriesPeriod? entryPeriod;

    private string?   formatPrice;
    private string[]? optionalKeys;
    private string[]? requiredKeys;

    private InstrumentType? timeSeriesType;

    public SourceTickerQuoteInfo()
    {
        Source = null!;
        Ticker = null!;
    }

    public SourceTickerQuoteInfo
    (ushort sourceId, string source, ushort tickerId, string ticker, QuoteLevel publishedQuoteLevel,
        MarketClassification marketClassification, byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.00001m,
        decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m, decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;

        PublishedQuoteLevel    = publishedQuoteLevel;
        MarketClassification   = marketClassification;
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision      = roundingPrecision;

        MinSubmitSize    = minSubmitSize;
        MaxSubmitSize    = maxSubmitSize;
        IncrementSize    = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags       = layerFlags;
        LastTradedFlags  = lastTradedFlags;

        Category = PublishedQuoteLevel.ToString();
    }

    public SourceTickerQuoteInfo(ISourceTickerQuoteInfo toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source   = toClone.Source;
        Ticker   = toClone.Ticker;

        PublishedQuoteLevel    = toClone.PublishedQuoteLevel;
        MarketClassification   = toClone.MarketClassification;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision      = toClone.RoundingPrecision;

        MinSubmitSize    = toClone.MinSubmitSize;
        MaxSubmitSize    = toClone.MaxSubmitSize;
        IncrementSize    = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags       = toClone.LayerFlags;
        LastTradedFlags  = toClone.LastTradedFlags;

        foreach (var instrumentFields in toClone) this[instrumentFields.Key] = instrumentFields.Value;
    }

    public string? Category
    {
        get => category ?? PublishedQuoteLevel.ToString();
        set => category = value;
    }

    object ICloneable.Clone() => Clone();

    public uint MessageId => Id;
    public byte Version   => 1;
    public uint Id        => (uint)((SourceId << 16) | TickerId);

    public ushort SourceId { get; set; }
    public ushort TickerId { get; set; }
    public string Source   { get; set; }
    public string Ticker   { get; set; }

    public QuoteLevel           PublishedQuoteLevel  { get; set; }
    public MarketClassification MarketClassification { get; set; }

    public byte       MaximumPublishedLayers { get; set; }
    public decimal    RoundingPrecision      { get; set; }
    public decimal    MinSubmitSize          { get; set; }
    public decimal    MaxSubmitSize          { get; set; }
    public decimal    IncrementSize          { get; set; }
    public ushort     MinimumQuoteLife       { get; set; }
    public LayerFlags LayerFlags             { get; set; }

    public LastTradedFlags LastTradedFlags { get; set; }

    string IInstrument.InstrumentName => Ticker;

    public TimeSeriesPeriod EntryPeriod
    {
        get => entryPeriod ?? TimeSeriesPeriod.Tick;
        set => entryPeriod = value;
    }
    public InstrumentType Type
    {
        get => timeSeriesType ?? InstrumentType.Price;
        set => timeSeriesType = value;
    }

    public string? this[string key]
    {
        get
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):        return MarketClassification.MarketType.ToString();
                case nameof(RepositoryPathName.MarketProductType): return MarketClassification.ProductType.ToString();
                case nameof(RepositoryPathName.MarketRegion):      return MarketClassification.MarketRegion.ToString();
                case nameof(RepositoryPathName.SourceName):        return Source;
                case nameof(RepositoryPathName.Category):          return Category ?? PublishedQuoteLevel.ToString();
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
                case nameof(RepositoryPathName.SourceName):
                    if (Source.IsNullOrEmpty()) Source = value ?? "";
                    break;
                case nameof(RepositoryPathName.Category):
                    if (Category.IsNullOrEmpty()) Category = value ?? "";
                    break;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        if (Source.IsNotNullOrEmpty()) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.SourceName), Source);
        if (category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), category);
        if (MarketClassification.MarketType != MarketType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
        if (MarketClassification.ProductType != ProductType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
        if (MarketClassification.MarketRegion != MarketRegion.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
    }

    public IEnumerable<string> RequiredInstrumentKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public IEnumerable<string> OptionalInstrumentKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public bool HasAllRequiredKeys =>
        Source.IsNotNullOrEmpty() && MarketClassification.MarketType != MarketType.Unknown
                                  && MarketClassification.ProductType != ProductType.Unknown &&
                                     MarketClassification.MarketRegion != MarketRegion.Unknown;

    public string FormatPrice =>
        formatPrice ??= RoundingPrecision
                        .ToString(CultureInfo.InvariantCulture)
                        .Replace('1', '0')
                        .Replace('2', '0')
                        .Replace('3', '0')
                        .Replace('4', '0')
                        .Replace('5', '0')
                        .Replace('6', '0')
                        .Replace('7', '0')
                        .Replace('8', '0')
                        .Replace('9', '0');

    public virtual bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        var sourceIdSame             = SourceId == other?.SourceId;
        var tickerIdSame             = TickerId == other?.TickerId;
        var sourceSame               = Source == other?.Source;
        var tickerSame               = Ticker == other?.Ticker;
        var quoteLevelSame           = PublishedQuoteLevel == other?.PublishedQuoteLevel;
        var marketClassificationSame = Equals(MarketClassification, other?.MarketClassification);
        var maxPublishedLayersSame   = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var roundingPrecisionSame    = RoundingPrecision == other?.RoundingPrecision;
        var minSubmitSizeSame        = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame        = MaxSubmitSize == other?.MaxSubmitSize;
        var incrmntSizeSame          = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame         = MinimumQuoteLife == other?.MinimumQuoteLife;
        var layerFlagsSame           = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame      = LastTradedFlags == other?.LastTradedFlags;

        return sourceIdSame && tickerIdSame && sourceSame && tickerSame && quoteLevelSame && marketClassificationSame
            && maxPublishedLayersSame && roundingPrecisionSame && minSubmitSizeSame && maxSubmitSizeSame && incrmntSizeSame
            && minQuoteLifeSame && layerFlagsSame && lastTradedFlagsSame;
    }


    IVersionedMessage IStoreState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerQuoteInfo)source, copyMergeFlags);

    IReusableObject<IVersionedMessage> IStoreState<IReusableObject<IVersionedMessage>>.CopyFrom
    (IReusableObject<IVersionedMessage> source
      , CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerQuoteInfo)source, copyMergeFlags);

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    public override ISourceTickerQuoteInfo Clone() => Recycler?.Borrow<SourceTickerQuoteInfo>()?.CopyFrom(this) ?? new SourceTickerQuoteInfo(this);

    public override ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = source.SourceId;
        TickerId = source.TickerId;
        Source   = source.Source;
        Ticker   = source.Ticker;

        PublishedQuoteLevel    = source.PublishedQuoteLevel;
        MarketClassification   = source.MarketClassification;
        RoundingPrecision      = source.RoundingPrecision;
        MaximumPublishedLayers = source.MaximumPublishedLayers;

        MinSubmitSize    = source.MinSubmitSize;
        MaxSubmitSize    = source.MaxSubmitSize;
        IncrementSize    = source.IncrementSize;
        MinimumQuoteLife = source.MinimumQuoteLife;
        LayerFlags       = source.LayerFlags;
        LastTradedFlags  = source.LastTradedFlags;

        foreach (var instrumentFields in source) this[instrumentFields.Key] = instrumentFields.Value;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISourceTickerQuoteInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ Source.GetHashCode();
            hashCode = (hashCode * 397) ^ Ticker.GetHashCode();
            hashCode = (hashCode * 397) ^ PublishedQuoteLevel.GetHashCode();
            hashCode = (hashCode * 397) ^ MarketClassification.GetHashCode();
            hashCode = (hashCode * 397) ^ MaximumPublishedLayers.GetHashCode();
            hashCode = (hashCode * 397) ^ (formatPrice != null ? formatPrice.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ RoundingPrecision.GetHashCode();
            hashCode = (hashCode * 397) ^ MinSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ IncrementSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MinimumQuoteLife.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)LayerFlags;
            hashCode = (hashCode * 397) ^ (int)LastTradedFlags;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"SourceTickerQuoteInfo {{{nameof(Id)}: {Id}, {nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, " +
        $"{nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker},  {nameof(PublishedQuoteLevel)}: {PublishedQuoteLevel},  " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, " +
        $"{nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, {nameof(LayerFlags)}: {LayerFlags:F}, " +
        $"{nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags} }}";
}
