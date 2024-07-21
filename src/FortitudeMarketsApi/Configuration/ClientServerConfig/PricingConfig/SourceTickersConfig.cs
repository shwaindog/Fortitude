// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

[Flags]
public enum TickerAvailability
{
    AllDisabled    = 0
  , PricingEnabled = 1
  , TradingEnabled = 2

  , PricingAndTradingEnabled = 3
}

public static class TickerAvailabilityExtensions
{
    public static bool? TradingEnabled(this TickerAvailability? tickerAvailability) =>
        tickerAvailability != null ? (tickerAvailability.Value & TickerAvailability.TradingEnabled) > 0 : null;

    public static bool? PricingEnabled(this TickerAvailability? tickerAvailability) =>
        tickerAvailability != null ? (tickerAvailability.Value & TickerAvailability.PricingEnabled) > 0 : null;

    public static bool IsTradingEnabled(this TickerAvailability tickerAvailability) => (tickerAvailability & TickerAvailability.TradingEnabled) > 0;

    public static bool IsPricingEnabled(this TickerAvailability tickerAvailability) => (tickerAvailability & TickerAvailability.PricingEnabled) > 0;
}

public interface ISourceTickersConfig : IInterfacesComparable<ISourceTickersConfig>
{
    TickerAvailability DefaultTickerAvailability       { get; set; }
    TickerDetailLevel  DefaultPublishTickerDetailLevel { get; set; }

    MarketClassificationConfig DefaultMarketClassification { get; set; }

    decimal DefaultRoundingPrecision      { get; set; }
    decimal DefaultPip                    { get; set; }
    decimal DefaultMinSubmitSize          { get; set; }
    decimal DefaultMaxSubmitSize          { get; set; }
    decimal DefaultIncrementSize          { get; set; }
    ushort  DefaultMinimumQuoteLife       { get; set; }
    byte    DefaultMaximumPublishedLayers { get; set; }
    uint    DefaultMaxValidMs             { get; set; }

    LayerFlags      DefaultLayerFlags      { get; set; }
    LastTradedFlags DefaultLastTradedFlags { get; set; }

    public IEnumerable<ITickerConfig> Tickers { get; set; }
    ISourceTickerInfo?                GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker);
    IEnumerable<ISourceTickerInfo>    AllSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerInfo>    PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerInfo>    TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
}

public class SourceTickersConfig : ConfigSection, ISourceTickersConfig
{
    #pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private object? ignoreSuppressWarnings;
    #pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    private MarketClassificationConfig? lastMarketClassificationConfig;
    public SourceTickersConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public SourceTickersConfig() { }

    public SourceTickersConfig(params ITickerConfig[] tickersConfigs) => Tickers = tickersConfigs.ToList();

    public SourceTickersConfig(ISourceTickersConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DefaultTickerAvailability       = toClone.DefaultTickerAvailability;
        DefaultPublishTickerDetailLevel = toClone.DefaultPublishTickerDetailLevel;
        DefaultMarketClassification     = toClone.DefaultMarketClassification;
        DefaultRoundingPrecision        = toClone.DefaultRoundingPrecision;
        DefaultPip                      = toClone.DefaultPip;
        DefaultMaximumPublishedLayers   = toClone.DefaultMaximumPublishedLayers;
        DefaultMinSubmitSize            = toClone.DefaultMinSubmitSize;
        DefaultMaxSubmitSize            = toClone.DefaultMaxSubmitSize;
        DefaultIncrementSize            = toClone.DefaultIncrementSize;
        DefaultMinimumQuoteLife         = toClone.DefaultMinimumQuoteLife;
        DefaultMaxValidMs               = toClone.DefaultMaxValidMs;
        DefaultLayerFlags               = toClone.DefaultLayerFlags;
        DefaultLastTradedFlags          = toClone.DefaultLastTradedFlags;
        Tickers                         = toClone.Tickers;
    }

    public SourceTickersConfig(ISourceTickersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TickerAvailability DefaultTickerAvailability
    {
        get
        {
            var checkValue = this[nameof(DefaultTickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : TickerAvailability.PricingAndTradingEnabled;
        }
        set => this[nameof(DefaultTickerAvailability)] = value.ToString();
    }

    public TickerDetailLevel DefaultPublishTickerDetailLevel
    {
        get
        {
            var checkValue = this[nameof(DefaultPublishTickerDetailLevel)];
            return checkValue != null ? Enum.Parse<TickerDetailLevel>(checkValue) : TickerDetailLevel.Level2Quote;
        }
        set => this[nameof(DefaultPublishTickerDetailLevel)] = value.ToString();
    }

    public MarketClassificationConfig DefaultMarketClassification
    {
        get => new(ConfigRoot, Path + ":" + nameof(DefaultMarketClassification));
        set =>
            lastMarketClassificationConfig = new MarketClassificationConfig
                (value, ConfigRoot, Path + ":" + nameof(DefaultMarketClassification));
    }

    public decimal DefaultRoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(DefaultRoundingPrecision)];
            return checkValue != null ? decimal.Parse(checkValue) : 0.000001m;
        }
        set => this[nameof(DefaultRoundingPrecision)] = value.ToString();
    }
    public decimal DefaultPip
    {
        get
        {
            var checkValue = this[nameof(DefaultPip)];
            return checkValue != null ? decimal.Parse(checkValue) : 0.0001m;
        }
        set => this[nameof(DefaultPip)] = value.ToString();
    }

    public byte DefaultMaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(DefaultMaximumPublishedLayers)];
            return checkValue != null ? byte.Parse(checkValue) : (byte)1;
        }
        set => this[nameof(DefaultMaximumPublishedLayers)] = value.ToString();
    }

    public decimal DefaultMinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMinSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : decimal.One;
        }
        set => this[nameof(DefaultMinSubmitSize)] = value.ToString();
    }

    public decimal DefaultMaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : 1_000_000m;
        }
        set => this[nameof(DefaultMaxSubmitSize)] = value.ToString();
    }

    public decimal DefaultIncrementSize
    {
        get
        {
            var checkValue = this[nameof(DefaultIncrementSize)];
            return checkValue != null ? decimal.Parse(checkValue) : 1m;
        }
        set => this[nameof(DefaultIncrementSize)] = value.ToString();
    }

    public ushort DefaultMinimumQuoteLife
    {
        get
        {
            var checkValue = this[nameof(DefaultMinimumQuoteLife)];
            return checkValue != null ? ushort.Parse(checkValue) : (ushort)0;
        }
        set => this[nameof(DefaultMinimumQuoteLife)] = value.ToString();
    }
    public uint DefaultMaxValidMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxValidMs)];
            return checkValue != null ? uint.Parse(checkValue) : 10_000;
        }
        set => this[nameof(DefaultMaxValidMs)] = value.ToString();
    }

    public LayerFlags DefaultLayerFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        }
        set => this[nameof(DefaultLayerFlags)] = value.ToString();
    }

    public LastTradedFlags DefaultLastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLastTradedFlags)];
            return checkValue != null ? Enum.Parse<LastTradedFlags>(checkValue) : LastTradedFlags.None;
        }
        set => this[nameof(DefaultLastTradedFlags)] = value.ToString();
    }

    public IEnumerable<ITickerConfig> Tickers
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<ITickerConfig>>();
            foreach (var configurationSection in GetSection(nameof(Tickers)).GetChildren())
                if (configurationSection["Ticker"] != null)
                    autoRecycleList.Add(new TickerConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = Tickers.Count();
            var i        = 0;
            foreach (var tickerConfig in value)
            {
                ignoreSuppressWarnings = new TickerConfig(tickerConfig, ConfigRoot
                                                        , Path + ":" + nameof(Tickers) + $":{i}");
                i++;
            }

            for (var j = i; j < oldCount; j++) TickerConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Tickers) + $":{i}");
        }
    }

    public ISourceTickerInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker)
    {
        var tickerConfig = Tickers.FirstOrDefault(tc => tc.Ticker == ticker);
        if (tickerConfig == null) return null;
        var sourceTickerINfo =
            new SourceTickerInfo
                (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
               , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerDetailLevel
               , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
               , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
               , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
               , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
               , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
               , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
               , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
               , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
               , tickerConfig.LayerFlags ?? DefaultLayerFlags
               , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
        return sourceTickerINfo;
    }

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
        {
            var sourceTickerINfo =
                new SourceTickerInfo
                    (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                   , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerDetailLevel
                   , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                   , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                   , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                   , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                   , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                   , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                   , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                   , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                   , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
            yield return sourceTickerINfo;
        }
    }

    public IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
            if (tickerConfig.TickerAvailability is TickerAvailability.PricingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                       , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                       , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                       , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                       , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
            if (tickerConfig.TickerAvailability is TickerAvailability.TradingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                       , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                       , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                       , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                       , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public bool AreEquivalent(ISourceTickersConfig? other, bool exactTypes = false)
    {
        var availabilitySame         = DefaultTickerAvailability == other?.DefaultTickerAvailability;
        var quoteLevelSame           = DefaultPublishTickerDetailLevel == other?.DefaultPublishTickerDetailLevel;
        var marketClassificationSame = Equals(DefaultMarketClassification, other?.DefaultMarketClassification);
        var roundingSame             = DefaultRoundingPrecision == other?.DefaultRoundingPrecision;
        var pipSame                  = DefaultPip == other?.DefaultPip;
        var maxLayersSame            = DefaultMaximumPublishedLayers == other?.DefaultMaximumPublishedLayers;
        var minSubitSizeSame         = DefaultMinSubmitSize == other?.DefaultMinSubmitSize;
        var maxSubitSizeSame         = DefaultMaxSubmitSize == other?.DefaultMaxSubmitSize;
        var incrementSizeSame        = DefaultIncrementSize == other?.DefaultIncrementSize;
        var minQuoteLifeSizeSame     = DefaultMinimumQuoteLife == other?.DefaultMinimumQuoteLife;
        var maxValidMsSame           = DefaultMaxValidMs == other?.DefaultMaxValidMs;
        var layerFlagsSizeSame       = DefaultLayerFlags == other?.DefaultLayerFlags;
        var lastTradedFlagsSame      = DefaultLastTradedFlags == other?.DefaultLastTradedFlags;
        var tickerConfigsSame        = Tickers.SequenceEqual(other?.Tickers ?? Array.Empty<ITickerConfig>());

        return availabilitySame && quoteLevelSame && marketClassificationSame && roundingSame && pipSame && maxLayersSame && minSubitSizeSame
            && maxSubitSizeSame && incrementSizeSame && minQuoteLifeSizeSame && maxValidMsSame && layerFlagsSizeSame && lastTradedFlagsSame
            && tickerConfigsSame;
    }

    protected bool Equals(ISourceTickersConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ISourceTickersConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(DefaultPublishTickerDetailLevel);
        hashCode.Add(DefaultTickerAvailability);
        hashCode.Add(DefaultMarketClassification);
        hashCode.Add(DefaultRoundingPrecision);
        hashCode.Add(DefaultPip);
        hashCode.Add(DefaultMinSubmitSize);
        hashCode.Add(DefaultMaxSubmitSize);
        hashCode.Add(DefaultIncrementSize);
        hashCode.Add(DefaultMinimumQuoteLife);
        hashCode.Add(DefaultMaxValidMs);
        hashCode.Add(DefaultLayerFlags);
        hashCode.Add(DefaultMaximumPublishedLayers);
        hashCode.Add(DefaultLastTradedFlags);
        hashCode.Add(Tickers);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(SourceTickersConfig)}({nameof(DefaultTickerAvailability)}: {DefaultTickerAvailability}, " +
        $"{nameof(DefaultPublishTickerDetailLevel)}: {DefaultPublishTickerDetailLevel}, {nameof(DefaultMarketClassification)}: {DefaultMarketClassification}, " +
        $"{nameof(DefaultRoundingPrecision)}: {DefaultRoundingPrecision}, {nameof(DefaultPip)}: {DefaultPip}, {nameof(DefaultMaximumPublishedLayers)}: {DefaultMaximumPublishedLayers}, " +
        $"{nameof(DefaultMinSubmitSize)}: {DefaultMinSubmitSize}, {nameof(DefaultMaxSubmitSize)}: {DefaultMaxSubmitSize}, {nameof(DefaultIncrementSize)}: {DefaultIncrementSize}, " +
        $"{nameof(DefaultMinimumQuoteLife)}: {DefaultMinimumQuoteLife}, {nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(DefaultLayerFlags)}: {DefaultLayerFlags}, " +
        $"{nameof(DefaultLastTradedFlags)}: {DefaultLastTradedFlags}, {nameof(Tickers)}: {Tickers}, {nameof(Path)}: {Path})";
}
