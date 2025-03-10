// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public interface ISourceTickerInfo : IPricingInstrumentId, IInterfacesComparable<ISourceTickerInfo>, IVersionedMessage
{
    [JsonIgnore] TickerDetailLevel PublishedTickerDetailLevel { get; set; }

    [JsonIgnore] decimal RoundingPrecision      { get; set; }
    [JsonIgnore] decimal Pip                    { get; set; }
    [JsonIgnore] decimal MinSubmitSize          { get; set; }
    [JsonIgnore] decimal MaxSubmitSize          { get; set; }
    [JsonIgnore] decimal IncrementSize          { get; set; }
    [JsonIgnore] byte    MaximumPublishedLayers { get; set; }
    [JsonIgnore] ushort  MinimumQuoteLife       { get; set; }
    [JsonIgnore] uint    DefaultMaxValidMs      { get; set; }
    [JsonIgnore] bool    SubscribeToPrices      { get; set; }
    [JsonIgnore] bool    TradingEnabled         { get; set; }

    LayerFlags LayerFlags { get; set; }

    LastTradedFlags LastTradedFlags { get; set; }

    [JsonIgnore] string FormatPrice { get; }

    ISourceTickerInfo CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new ISourceTickerInfo Clone();
}

public class SourceTickerInfo : PricingInstrument, ISourceTickerInfo, ICloneable<SourceTickerInfo>
{
    private string? formatPrice;

    public SourceTickerInfo()
    {
        PublishedTickerDetailLevel = TickerDetailLevel.Level1Quote;

        Pip = 0.0001m;

        RoundingPrecision      = 0.00001m;
        DefaultMaxValidMs      = 10_000;
        SubscribeToPrices      = true;
        TradingEnabled         = false;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerInfo(ISourceTickerId sourceTickerId)
    {
        SourceId       = sourceTickerId.SourceId;
        InstrumentId   = sourceTickerId.InstrumentId;
        SourceName     = sourceTickerId.SourceName;
        InstrumentName = sourceTickerId.InstrumentName;

        PublishedTickerDetailLevel = TickerDetailLevel.Level1Quote;

        Pip = 0.0001m;

        RoundingPrecision      = 0.00001m;
        DefaultMaxValidMs      = 10_000;
        SubscribeToPrices      = true;
        TradingEnabled         = false;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerInfo(SourceTickerIdentifier sourceTickerIdentifier) : base(sourceTickerIdentifier)
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        InstrumentId   = sourceTickerIdentifier.TickerId;
        SourceName     = sourceTickerIdentifier.Source;
        InstrumentName = sourceTickerIdentifier.Ticker;

        PublishedTickerDetailLevel = TickerDetailLevel.Level1Quote;

        Pip = 0.0001m;

        RoundingPrecision      = 0.00001m;
        DefaultMaxValidMs      = 10_000;
        SubscribeToPrices      = true;
        TradingEnabled         = false;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerInfo(IPricingInstrumentId pricingInstrumentId, TickerDetailLevel tickerDetailLevel = TickerDetailLevel.Level1Quote)
        : base(pricingInstrumentId)
    {
        PublishedTickerDetailLevel = tickerDetailLevel;

        Pip = 0.0001m;

        RoundingPrecision      = 0.00001m;
        DefaultMaxValidMs      = 10_000;
        SubscribeToPrices      = true;
        TradingEnabled         = false;
        RoundingPrecision      = 0.00001m;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerInfo(PricingInstrumentId pricingInstrumentId, TickerDetailLevel tickerDetailLevel = TickerDetailLevel.Level1Quote)
        : base(pricingInstrumentId)
    {
        PublishedTickerDetailLevel = tickerDetailLevel;

        Pip = 0.0001m;

        RoundingPrecision      = 0.00001m;
        DefaultMaxValidMs      = 10_000;
        SubscribeToPrices      = true;
        TradingEnabled         = false;
        MaximumPublishedLayers = 1;
        MinSubmitSize          = 0.01m;
        MaxSubmitSize          = 1_000_000m;
        IncrementSize          = 0.01m;
        MinimumQuoteLife       = 100;
        LayerFlags             = LayerFlags.None;
        LastTradedFlags        = LastTradedFlags.None;
    }

    public SourceTickerInfo
    (ushort sourceId, string sourceName, ushort tickerId, string ticker, TickerDetailLevel publishedTickerDetailLevel
      , MarketClassification marketClassification, byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.00001m
      , decimal pip = 0.0001m, decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m, decimal incrementSize = 0.01m
      , ushort minimumQuoteLife = 100, uint defaultMaxValidMs = 10_000, bool subscribeToPrices = true, bool tradingEnabled = false
      , LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
        : base(sourceId, tickerId, sourceName, ticker, new DiscreetTimePeriod(TimeBoundaryPeriod.Tick), InstrumentType.Price, marketClassification)
    {
        PublishedTickerDetailLevel = publishedTickerDetailLevel;

        Pip = pip;

        DefaultMaxValidMs      = defaultMaxValidMs;
        SubscribeToPrices      = subscribeToPrices;
        TradingEnabled         = tradingEnabled;
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision      = roundingPrecision;
        MinSubmitSize          = minSubmitSize;
        MaxSubmitSize          = maxSubmitSize;
        IncrementSize          = incrementSize;
        MinimumQuoteLife       = minimumQuoteLife;
        LayerFlags             = layerFlags;
        LastTradedFlags        = lastTradedFlags;

        Category = PublishedTickerDetailLevel.ToString();
    }

    public SourceTickerInfo(ISourceTickerInfo toClone) : base(toClone)
    {
        PublishedTickerDetailLevel = toClone.PublishedTickerDetailLevel;

        Pip = toClone.Pip;

        DefaultMaxValidMs      = toClone.DefaultMaxValidMs;
        SubscribeToPrices      = toClone.SubscribeToPrices;
        TradingEnabled         = toClone.TradingEnabled;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision      = toClone.RoundingPrecision;
        MinSubmitSize          = toClone.MinSubmitSize;
        MaxSubmitSize          = toClone.MaxSubmitSize;
        IncrementSize          = toClone.IncrementSize;
        MinimumQuoteLife       = toClone.MinimumQuoteLife;
        LayerFlags             = toClone.LayerFlags;
        LastTradedFlags        = toClone.LastTradedFlags;

        foreach (var instrumentFields in toClone.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;
    }

    public override SourceTickerInfo Clone() =>
        Recycler?.Borrow<SourceTickerInfo>()?.CopyFrom(this) as SourceTickerInfo ?? new SourceTickerInfo((ISourceTickerInfo)this);

    object ICloneable.Clone() => Clone();

    public uint MessageId => SourceTickerId;
    public byte Version   => 1;

    public TickerDetailLevel PublishedTickerDetailLevel { get; set; }

    public byte MaximumPublishedLayers { get; set; }

    public decimal RoundingPrecision { get; set; }
    public decimal Pip               { get; set; }
    public decimal MinSubmitSize     { get; set; }
    public decimal MaxSubmitSize     { get; set; }
    public decimal IncrementSize     { get; set; }
    public ushort  MinimumQuoteLife  { get; set; }
    public uint    DefaultMaxValidMs { get; set; }
    public bool    SubscribeToPrices { get; set; }
    public bool    TradingEnabled    { get; set; }

    public LayerFlags LayerFlags { get; set; }

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

    public virtual bool AreEquivalent(ISourceTickerInfo? other, bool exactTypes = false)
    {
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var detailLevelSame        = PublishedTickerDetailLevel == other?.PublishedTickerDetailLevel;
        var maxPublishedLayersSame = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var roundingPrecisionSame  = RoundingPrecision == other?.RoundingPrecision;

        var pipSame = Pip == other?.Pip;

        var minSubmitSizeSame   = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame   = MaxSubmitSize == other?.MaxSubmitSize;
        var incrmntSizeSame     = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame    = MinimumQuoteLife == other?.MinimumQuoteLife;
        var defaultMaxValidSame = DefaultMaxValidMs == other?.DefaultMaxValidMs;
        var subscribeSame       = SubscribeToPrices == other?.SubscribeToPrices;
        var tradingEnabledSame  = TradingEnabled == other?.TradingEnabled;
        var layerFlagsSame      = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        var allAreSame = baseIsSame && detailLevelSame && maxPublishedLayersSame && roundingPrecisionSame && pipSame && minSubmitSizeSame
                      && maxSubmitSizeSame && incrmntSizeSame && minQuoteLifeSame && defaultMaxValidSame && subscribeSame && tradingEnabledSame
                      && layerFlagsSame && lastTradedFlagsSame;
        return allAreSame;
    }


    IVersionedMessage IStoreState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    IReusableObject<IVersionedMessage> IStoreState<IReusableObject<IVersionedMessage>>.CopyFrom
    (IReusableObject<IVersionedMessage> source
      , CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.Clone() => Clone();
    ISourceTickerInfo ISourceTickerInfo.      Clone() => Clone();

    ISourceTickerId IStoreState<ISourceTickerId>.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags)
    {
        if (source is ISourceTickerInfo srcTkrQuoteInfo) return CopyFrom(srcTkrQuoteInfo, copyMergeFlags);
        PublishedTickerDetailLevel = TickerDetailLevel.Level1Quote;
        MarketClassification       = MarketClassificationExtensions.Unknown;
        RoundingPrecision          = 0.00001m;
        Pip                        = 0.0001m;
        MaximumPublishedLayers     = 1;
        MinSubmitSize              = 0.01m;
        MaxSubmitSize              = 1_000_000m;
        IncrementSize              = 0.01m;
        MinimumQuoteLife           = 100;
        DefaultMaxValidMs          = 10_000;
        SubscribeToPrices          = true;
        TradingEnabled             = false;
        LayerFlags                 = LayerFlags.None;
        LastTradedFlags            = LastTradedFlags.None;
        return base.CopyFrom(source, copyMergeFlags);
    }

    public ISourceTickerInfo CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        MaximumPublishedLayers = source.MaximumPublishedLayers;

        Pip = source.Pip;

        SubscribeToPrices = source.SubscribeToPrices;
        TradingEnabled    = source.TradingEnabled;
        RoundingPrecision = source.RoundingPrecision;
        MinSubmitSize     = source.MinSubmitSize;
        MaxSubmitSize     = source.MaxSubmitSize;
        IncrementSize     = source.IncrementSize;
        MinimumQuoteLife  = source.MinimumQuoteLife;
        LayerFlags        = source.LayerFlags;
        LastTradedFlags   = source.LastTradedFlags;

        foreach (var instrumentFields in source.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISourceTickerInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ InstrumentId;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(SourceTickerInfo)}({nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, {nameof(InstrumentId)}: {InstrumentId}, {nameof(InstrumentName)}: {InstrumentName},  " +
        $"{nameof(PublishedTickerDetailLevel)}: {PublishedTickerDetailLevel},  {nameof(MarketClassification)}: {MarketClassification}, " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, {nameof(MinSubmitSize)}: {MinSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(SubscribeToPrices)}: {SubscribeToPrices}, {nameof(TradingEnabled)}: {TradingEnabled}, " +
        $"{nameof(LayerFlags)}: {LayerFlags:F}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags})";


    public static implicit operator PricingInstrumentId(SourceTickerInfo sourceTickerId) =>
        new(sourceTickerId, sourceTickerId.CoveringPeriod, sourceTickerId.InstrumentType);

    public static implicit operator SourceTickerIdentifier(SourceTickerInfo sourceTickerId) => new(sourceTickerId);
}
