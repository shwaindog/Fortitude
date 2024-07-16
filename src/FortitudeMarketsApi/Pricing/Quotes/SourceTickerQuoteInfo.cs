// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ISourceTickerQuoteInfo : IPricingInstrumentId, IInterfacesComparable<ISourceTickerQuoteInfo>, IVersionedMessage
{
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

    ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new ISourceTickerQuoteInfo Clone();
}

public class SourceTickerQuoteInfo : PricingInstrument, ISourceTickerQuoteInfo, ICloneable<SourceTickerQuoteInfo>
{
    private string? formatPrice;

    public SourceTickerQuoteInfo()
    {
        PublishedQuoteLevel    = QuoteLevel.Level1;
        MarketClassification   = MarketClassificationExtensions.Unknown;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerQuoteInfo(ISourceTickerId sourceTickerId)
    {
        SourceId = sourceTickerId.SourceId;
        TickerId = sourceTickerId.TickerId;
        Source   = sourceTickerId.Source;
        Ticker   = sourceTickerId.Ticker;

        PublishedQuoteLevel    = QuoteLevel.Level1;
        MarketClassification   = MarketClassificationExtensions.Unknown;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerQuoteInfo(SourceTickerIdentifier sourceTickerIdentifier)
    {
        SourceId = sourceTickerIdentifier.SourceId;
        TickerId = sourceTickerIdentifier.TickerId;
        Source   = sourceTickerIdentifier.Source;
        Ticker   = sourceTickerIdentifier.Ticker;

        PublishedQuoteLevel    = QuoteLevel.Level1;
        MarketClassification   = MarketClassificationExtensions.Unknown;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerQuoteInfo(IPricingInstrumentId pricingInstrumentId, QuoteLevel quoteLevel = QuoteLevel.Level1)
    {
        SourceId       = pricingInstrumentId.SourceId;
        TickerId       = pricingInstrumentId.TickerId;
        Source         = pricingInstrumentId.Source;
        Ticker         = pricingInstrumentId.Ticker;
        InstrumentType = pricingInstrumentId.InstrumentType;
        EntryPeriod    = pricingInstrumentId.EntryPeriod;
        Category       = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        PublishedQuoteLevel    = quoteLevel;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerQuoteInfo(PricingInstrumentId pricingInstrumentId, QuoteLevel quoteLevel = QuoteLevel.Level1)
    {
        SourceId       = pricingInstrumentId.SourceId;
        TickerId       = pricingInstrumentId.TickerId;
        Source         = pricingInstrumentId.Source;
        Ticker         = pricingInstrumentId.Ticker;
        InstrumentType = pricingInstrumentId.InstrumentType;
        EntryPeriod    = pricingInstrumentId.EntryPeriod;
        Category       = pricingInstrumentId.Category;

        MarketClassification = pricingInstrumentId.MarketClassification;

        PublishedQuoteLevel    = quoteLevel;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
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

    public override SourceTickerQuoteInfo Clone() =>
        Recycler?.Borrow<SourceTickerQuoteInfo>()?.CopyFrom(this) as SourceTickerQuoteInfo ?? new SourceTickerQuoteInfo((ISourceTickerQuoteInfo)this);

    object ICloneable.Clone() => Clone();

    public uint MessageId => SourceTickerId;
    public byte Version   => 1;

    public QuoteLevel PublishedQuoteLevel { get; set; }

    public byte       MaximumPublishedLayers { get; set; }
    public decimal    RoundingPrecision      { get; set; }
    public decimal    MinSubmitSize          { get; set; }
    public decimal    MaxSubmitSize          { get; set; }
    public decimal    IncrementSize          { get; set; }
    public ushort     MinimumQuoteLife       { get; set; }
    public LayerFlags LayerFlags             { get; set; }

    public LastTradedFlags LastTradedFlags { get; set; }


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

    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.    Clone() => Clone();
    ISourceTickerQuoteInfo ISourceTickerQuoteInfo.Clone() => Clone();

    ISourceTickerId IStoreState<ISourceTickerId>.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags)
    {
        if (source is ISourceTickerQuoteInfo srcTkrQuoteInfo) return CopyFrom(srcTkrQuoteInfo, copyMergeFlags);
        PublishedQuoteLevel    = QuoteLevel.Level1;
        MarketClassification   = MarketClassificationExtensions.Unknown;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
        return base.CopyFrom(source, copyMergeFlags);
    }

    public ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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
        $"SourceTickerQuoteInfo {{{nameof(SourceTickerId)}: {SourceTickerId}, {nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, " +
        $"{nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker},  {nameof(PublishedQuoteLevel)}: {PublishedQuoteLevel},  " +
        $"{nameof(MarketClassification)}: {MarketClassification}, {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, " +
        $"{nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, {nameof(LayerFlags)}: {LayerFlags:F}, " +
        $"{nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags} }}";


    public static implicit operator PricingInstrumentId(SourceTickerQuoteInfo sourceTickerId) =>
        new(sourceTickerId, sourceTickerId.EntryPeriod, sourceTickerId.InstrumentType);

    public static implicit operator SourceTickerIdentifier(SourceTickerQuoteInfo sourceTickerId) => new(sourceTickerId);
}
