// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

public interface ITickerConfig : IInterfacesComparable<ITickerConfig>
{
    ushort TickerId { get; set; }
    string Ticker   { get; set; }

    TickerAvailability? TickerAvailability   { get; set; }
    TickerDetailLevel?  PublishedDetailLevel { get; set; }

    MarketClassificationConfig? MarketClassificationConfig { get; set; }

    decimal? RoundingPrecision      { get; set; }
    decimal? Pip                    { get; set; }
    decimal? MinSubmitSize          { get; set; }
    decimal? MaxSubmitSize          { get; set; }
    decimal? IncrementSize          { get; set; }
    ushort?  MinimumQuoteLife       { get; set; }
    uint?    DefaultMaxValidMs      { get; set; }
    byte?    MaximumPublishedLayers { get; set; }

    LayerFlags?      LayerFlags      { get; set; }
    LastTradedFlags? LastTradedFlags { get; set; }
}

public class TickerConfig : ConfigSection, ITickerConfig
{
    private MarketClassificationConfig? lastMarketClassificationConfig;

    public TickerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public TickerConfig() { }

    public TickerConfig
    (ushort tickerId, string ticker, TickerAvailability? tickerAvailability = null, TickerDetailLevel? publishedQuoteLevel = null
      , MarketClassification? marketClassification = null, decimal? roundingPrecision = null, decimal? pip = null, decimal? minSubmitSize = null
      , decimal? maxSubmitSize = null, decimal? incrementSize = null, ushort? minimumQuoteLife = null, uint? defaultMaxValidMs = null
      , LayerFlags? layerFlags = null, byte? maximumPublisherLayers = null, LastTradedFlags? lastTradedFlags = null)
    {
        TickerId             = tickerId;
        Ticker               = ticker;
        TickerAvailability   = tickerAvailability;
        PublishedDetailLevel = publishedQuoteLevel;
        if (marketClassification != null) MarketClassificationConfig = new MarketClassificationConfig(marketClassification.Value);
        RoundingPrecision      = roundingPrecision;
        Pip                    = pip;
        MinSubmitSize          = minSubmitSize;
        MaxSubmitSize          = maxSubmitSize;
        IncrementSize          = incrementSize;
        MinimumQuoteLife       = minimumQuoteLife;
        DefaultMaxValidMs      = defaultMaxValidMs;
        LayerFlags             = layerFlags;
        MaximumPublishedLayers = maximumPublisherLayers;
        LastTradedFlags        = lastTradedFlags;
    }

    public TickerConfig(ITickerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TickerId                   = toClone.TickerId;
        Ticker                     = toClone.Ticker;
        TickerAvailability         = toClone.TickerAvailability;
        PublishedDetailLevel       = toClone.PublishedDetailLevel;
        MarketClassificationConfig = toClone.MarketClassificationConfig;
        RoundingPrecision          = toClone.RoundingPrecision;
        Pip                        = toClone.Pip;
        MinSubmitSize              = toClone.MinSubmitSize;
        MaxSubmitSize              = toClone.MaxSubmitSize;
        IncrementSize              = toClone.IncrementSize;
        MinimumQuoteLife           = toClone.MinimumQuoteLife;
        DefaultMaxValidMs          = toClone.DefaultMaxValidMs;
        LayerFlags                 = toClone.LayerFlags;
        MaximumPublishedLayers     = toClone.MaximumPublishedLayers;
        LastTradedFlags            = toClone.LastTradedFlags;
    }

    public TickerConfig(ITickerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool? SubscribeToPrices
    {
        get
        {
            var checkValue = this[nameof(SubscribeToPrices)];
            return checkValue != null ? bool.Parse(checkValue) : null;
        }
        set => this[nameof(SubscribeToPrices)] = value.ToString();
    }

    public bool? TradingEnabled
    {
        get
        {
            var checkValue = this[nameof(TradingEnabled)];
            return checkValue != null ? bool.Parse(checkValue) : null;
        }
        set => this[nameof(TradingEnabled)] = value.ToString();
    }

    public decimal? Pip
    {
        get
        {
            var checkValue = this[nameof(Pip)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(Pip)] = value.ToString();
    }

    public uint? DefaultMaxValidMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxValidMs)];
            return checkValue != null ? uint.Parse(checkValue) : null;
        }
        set => this[nameof(DefaultMaxValidMs)] = value.ToString();
    }

    public MarketClassificationConfig? MarketClassificationConfig
    {
        get
        {
            if (GetSection(nameof(MarketClassificationConfig))
                .GetChildren()
                .Any(c => c.Value.IsNotNullOrEmpty() && c.Value != "Unknown"))
                return lastMarketClassificationConfig = new MarketClassificationConfig
                    (ConfigRoot, Path + ":" + nameof(MarketClassificationConfig));
            return null;
        }
        set =>
            lastMarketClassificationConfig = value != null
                ? new MarketClassificationConfig(value, ConfigRoot, Path + ":" + nameof(MarketClassificationConfig))
                : new MarketClassificationConfig(new MarketClassification(0), ConfigRoot, Path + ":" + nameof(MarketClassificationConfig));
    }

    public ushort TickerId
    {
        get => ushort.Parse(this[nameof(TickerId)]!);
        set => this[nameof(TickerId)] = value.ToString();
    }

    public string Ticker
    {
        get => this[nameof(Ticker)]!;
        set => this[nameof(Ticker)] = value;
    }

    public TickerAvailability? TickerAvailability
    {
        get
        {
            var checkValue = this[nameof(TickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : null;
        }
        set => this[nameof(TickerAvailability)] = value.ToString();
    }

    public TickerDetailLevel? PublishedDetailLevel
    {
        get
        {
            var checkValue = this[nameof(PublishedDetailLevel)];
            return checkValue != null ? Enum.Parse<TickerDetailLevel>(checkValue) : null;
        }
        set => this[nameof(PublishedDetailLevel)] = value.ToString();
    }

    public decimal? RoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(RoundingPrecision)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(RoundingPrecision)] = value?.ToString();
    }

    public decimal? MinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MinSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MinSubmitSize)] = value?.ToString();
    }

    public decimal? MaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MaxSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MaxSubmitSize)] = value?.ToString();
    }

    public decimal? IncrementSize
    {
        get
        {
            var checkValue = this[nameof(IncrementSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(IncrementSize)] = value?.ToString();
    }

    public ushort? MinimumQuoteLife
    {
        get
        {
            var checkValue = this[nameof(MinimumQuoteLife)];
            return checkValue != null ? ushort.Parse(checkValue) : null;
        }
        set => this[nameof(MinimumQuoteLife)] = value?.ToString();
    }

    public LayerFlags? LayerFlags
    {
        get
        {
            var checkValue = this[nameof(LayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : null;
        }
        set => this[nameof(LayerFlags)] = value.ToString();
    }

    public byte? MaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(MaximumPublishedLayers)];
            return checkValue != null ? byte.Parse(checkValue) : null;
        }
        set => this[nameof(MaximumPublishedLayers)] = value?.ToString();
    }

    public LastTradedFlags? LastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(LastTradedFlags)];
            return checkValue != null ? Enum.Parse<LastTradedFlags>(checkValue) : null;
        }
        set => this[nameof(LastTradedFlags)] = value.ToString();
    }

    public bool AreEquivalent(ITickerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var tickerIdSame             = TickerId == other.TickerId;
        var tickerSame               = Ticker == other.Ticker;
        var quoteLevelSame           = PublishedDetailLevel == other.PublishedDetailLevel;
        var marketClassificationSame = Equals(MarketClassificationConfig, other.MarketClassificationConfig);
        var availabilitySame         = TickerAvailability == other.TickerAvailability;
        var roundingSame             = RoundingPrecision == other.RoundingPrecision;
        var pipSame                  = Pip == other.Pip;
        var minSubitSizeSame         = MinSubmitSize == other.MinSubmitSize;
        var maxSubitSizeSame         = MaxSubmitSize == other.MaxSubmitSize;
        var incrementSizeSame        = IncrementSize == other.IncrementSize;
        var minQuoteLifeSizeSame     = MinimumQuoteLife == other.MinimumQuoteLife;
        var maxValidMsSame           = DefaultMaxValidMs == other.DefaultMaxValidMs;
        var layerFlagsSizeSame       = LayerFlags == other.LayerFlags;
        var maxLayersSame            = MaximumPublishedLayers == other.MaximumPublishedLayers;
        var lastTradedFlagsSame      = LastTradedFlags == other.LastTradedFlags;

        return tickerIdSame && tickerSame && availabilitySame && quoteLevelSame && marketClassificationSame && roundingSame && pipSame
            && minSubitSizeSame && maxSubitSizeSame && incrementSizeSame && minQuoteLifeSizeSame && maxValidMsSame && layerFlagsSizeSame
            && maxLayersSame && lastTradedFlagsSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(TickerId)]                   = null;
        root[path + ":" + nameof(Ticker)]                     = null;
        root[path + ":" + nameof(TickerAvailability)]         = null;
        root[path + ":" + nameof(PublishedDetailLevel)]       = null;
        root[path + ":" + nameof(MarketClassificationConfig)] = null;
        root[path + ":" + nameof(RoundingPrecision)]          = null;
        root[path + ":" + nameof(Pip)]                        = null;
        root[path + ":" + nameof(MinSubmitSize)]              = null;
        root[path + ":" + nameof(MaxSubmitSize)]              = null;
        root[path + ":" + nameof(IncrementSize)]              = null;
        root[path + ":" + nameof(MinimumQuoteLife)]           = null;
        root[path + ":" + nameof(DefaultMaxValidMs)]          = null;
        root[path + ":" + nameof(SubscribeToPrices)]          = null;
        root[path + ":" + nameof(TradingEnabled)]             = null;
        root[path + ":" + nameof(LayerFlags)]                 = null;
        root[path + ":" + nameof(MaximumPublishedLayers)]     = null;
        root[path + ":" + nameof(LastTradedFlags)]            = null;
    }

    protected bool Equals(ITickerConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ITickerConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(TickerId);
        hashCode.Add(Ticker);
        hashCode.Add(TickerAvailability);
        hashCode.Add(PublishedDetailLevel);
        hashCode.Add(MarketClassificationConfig);
        hashCode.Add(RoundingPrecision);
        hashCode.Add(Pip);
        hashCode.Add(MinSubmitSize);
        hashCode.Add(MaxSubmitSize);
        hashCode.Add(IncrementSize);
        hashCode.Add(MinimumQuoteLife);
        hashCode.Add(LayerFlags);
        hashCode.Add(MaximumPublishedLayers);
        hashCode.Add(LastTradedFlags);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(TickerConfig)}({nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker}, " +
        $"{nameof(TickerAvailability)}: {TickerAvailability}, {nameof(PublishedDetailLevel)}: {PublishedDetailLevel}, " +
        $"{nameof(MarketClassificationConfig)}: {MarketClassificationConfig}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers},  " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, {nameof(MinSubmitSize)}: {MinSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(SubscribeToPrices)}: {SubscribeToPrices}, {nameof(TradingEnabled)}: {TradingEnabled}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(LastTradedFlags)}: {LastTradedFlags}, {nameof(Path)}: {Path})";
}
