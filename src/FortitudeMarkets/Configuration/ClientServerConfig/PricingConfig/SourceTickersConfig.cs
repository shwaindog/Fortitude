// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter<TickerAvailability>))]
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
    const decimal DefaultRoundingPrecisionValue      = 0.000001m;
    const decimal DefaultPipValue                    = 0.0001m;
    const byte    DefaultMaximumPublishedLayersValue = 1;
    const decimal DefaultMinSubmitSizeValue          = decimal.One;
    const decimal DefaultMaxSubmitSizeValue          = 10_000m;
    const decimal DefaultIncrementSizeValue          = decimal.One;
    const ushort  DefaultMinimumQuoteLifeMsMsValue   = 0;
    const uint    DefaultMaxValidMsValue             = 10_000;

    const LayerFlags      DefaultLayerFlagsValue      = LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
    const LastTradedFlags DefaultLastTradedFlagsValue = LastTradedFlags.None;

    TickerAvailability DefaultTickerAvailability       { get; set; }
    TickerQuoteDetailLevel  DefaultPublishTickerQuoteDetailLevel { get; set; }

    MarketClassificationConfig DefaultMarketClassification { get; set; }

    decimal DefaultPip           { get; set; }
    decimal DefaultMinSubmitSize { get; set; }
    decimal DefaultMaxSubmitSize { get; set; }
    decimal DefaultIncrementSize { get; set; }
    uint    DefaultMaxValidMs    { get; set; }

    decimal DefaultRoundingPrecision      { get; set; }
    ushort  DefaultMinimumQuoteLifeMs     { get; set; }
    byte    DefaultMaximumPublishedLayers { get; set; }

    LayerFlags      DefaultLayerFlags      { get; set; }
    LastTradedFlags DefaultLastTradedFlags { get; set; }

    public IDictionary<string, ITickerConfig> Tickers { get; set; }

    ISourceTickerInfo?             GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker);
    IEnumerable<ISourceTickerInfo> AllSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
}

public class SourceTickersConfig : ConfigSection, ISourceTickersConfig
{
    #pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private object? ignoreSuppressWarnings;
    #pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    private MarketClassificationConfig? lastMarketClassificationConfig;

    private IDictionary<string, TickerConfig> tickers = new Dictionary<string, TickerConfig>();
    public SourceTickersConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public SourceTickersConfig() { }

    public SourceTickersConfig
        (params ITickerConfig[] tickersConfigs) =>
        ((ISourceTickersConfig)this).Tickers = tickersConfigs.ToDictionary(tc => tc.Ticker!);

    public SourceTickersConfig(ISourceTickersConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DefaultPublishTickerQuoteDetailLevel = toClone.DefaultPublishTickerQuoteDetailLevel;
        DefaultMaximumPublishedLayers   = toClone.DefaultMaximumPublishedLayers;
        DefaultMarketClassification     = toClone.DefaultMarketClassification;

        DefaultTickerAvailability = toClone.DefaultTickerAvailability;
        DefaultMinimumQuoteLifeMs = toClone.DefaultMinimumQuoteLifeMs;
        DefaultRoundingPrecision  = toClone.DefaultRoundingPrecision;

        DefaultLastTradedFlags = toClone.DefaultLastTradedFlags;
        DefaultMinSubmitSize   = toClone.DefaultMinSubmitSize;
        DefaultMaxSubmitSize   = toClone.DefaultMaxSubmitSize;
        DefaultIncrementSize   = toClone.DefaultIncrementSize;
        DefaultMaxValidMs      = toClone.DefaultMaxValidMs;
        DefaultLayerFlags      = toClone.DefaultLayerFlags;

        DefaultPip                           = toClone.DefaultPip;
        ((ISourceTickersConfig)this).Tickers = toClone.Tickers;
    }

    public SourceTickersConfig(ISourceTickersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IDictionary<string, TickerConfig> Tickers
    {
        get
        {
            foreach (var configurationSection in NonEmptyConfigs)
            {
                var tickerConfig = new TickerConfig(ConfigRoot, configurationSection.Path);
                if (tickerConfig.Ticker.IsNotNullOrEmpty() && configurationSection.Key != tickerConfig.Ticker)
                    throw new
                        ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {tickerConfig.Ticker}");
                if (tickerConfig.Ticker.IsNullOrEmpty()) tickerConfig.Ticker = configurationSection.Key;
                if (!tickers.ContainsKey(configurationSection.Key))
                    tickers.TryAdd(configurationSection.Key, tickerConfig);
                else
                    tickers[configurationSection.Key] = tickerConfig;
            }
            return tickers;
        }
        set
        {
            var oldKeys = tickers.Keys.ToHashSet();
            foreach (var tickerConfigKvp in value)
            {
                ignoreSuppressWarnings = new TickerConfig(tickerConfigKvp.Value, ConfigRoot
                                                        , Path + ":" + nameof(Tickers) + $":{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.Ticker.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.Ticker)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.Ticker}");
                if (tickerConfigKvp.Value.Ticker.IsNullOrEmpty()) tickerConfigKvp.Value.Ticker = tickerConfigKvp.Key;
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) TickerConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Tickers) + $":{deletedKey}");

            tickers = value;
        }
    }

    private IEnumerable<IConfigurationSection> NonEmptyConfigs =>
        GetSection(nameof(Tickers)).GetChildren().Where(cs => cs[nameof(ITickerConfig.TickerId)] != null);

    public TickerAvailability DefaultTickerAvailability
    {
        get
        {
            var checkValue = this[nameof(DefaultTickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : TickerAvailability.PricingAndTradingEnabled;
        }
        set => this[nameof(DefaultTickerAvailability)] = value.ToString();
    }

    public TickerQuoteDetailLevel DefaultPublishTickerQuoteDetailLevel
    {
        get
        {
            var checkValue = this[nameof(DefaultPublishTickerQuoteDetailLevel)];
            return checkValue != null ? Enum.Parse<TickerQuoteDetailLevel>(checkValue) : TickerQuoteDetailLevel.Level2Quote;
        }
        set => this[nameof(DefaultPublishTickerQuoteDetailLevel)] = value.ToString();
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
            return checkValue != null ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultRoundingPrecisionValue;
        }
        set => this[nameof(DefaultRoundingPrecision)] = value.ToString();
    }

    public decimal DefaultPip
    {
        get
        {
            var checkValue = this[nameof(DefaultPip)];
            return checkValue != null ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultPipValue;
        }
        set => this[nameof(DefaultPip)] = value.ToString();
    }

    public byte DefaultMaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(DefaultMaximumPublishedLayers)];
            return checkValue != null ? byte.Parse(checkValue) : ISourceTickersConfig.DefaultMaximumPublishedLayersValue;
        }
        set => this[nameof(DefaultMaximumPublishedLayers)] = value.ToString();
    }

    public decimal DefaultMinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMinSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultMinSubmitSizeValue;
        }
        set => this[nameof(DefaultMinSubmitSize)] = value.ToString();
    }

    public decimal DefaultMaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultMaxSubmitSizeValue;
        }
        set => this[nameof(DefaultMaxSubmitSize)] = value.ToString();
    }

    public decimal DefaultIncrementSize
    {
        get
        {
            var checkValue = this[nameof(DefaultIncrementSize)];
            return checkValue != null ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultIncrementSizeValue;
        }
        set => this[nameof(DefaultIncrementSize)] = value.ToString();
    }

    public ushort DefaultMinimumQuoteLifeMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMinimumQuoteLifeMs)];
            return checkValue != null ? ushort.Parse(checkValue) : ISourceTickersConfig.DefaultMinimumQuoteLifeMsMsValue;
        }
        set => this[nameof(DefaultMinimumQuoteLifeMs)] = value.ToString();
    }

    public uint DefaultMaxValidMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxValidMs)];
            return checkValue != null ? uint.Parse(checkValue) : ISourceTickersConfig.DefaultMaxValidMsValue;
        }
        set => this[nameof(DefaultMaxValidMs)] = value.ToString();
    }

    public LayerFlags DefaultLayerFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : ISourceTickersConfig.DefaultLayerFlagsValue;
        }
        set => this[nameof(DefaultLayerFlags)] = value.ToString();
    }

    public LastTradedFlags DefaultLastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLastTradedFlags)];
            return checkValue != null ? Enum.Parse<LastTradedFlags>(checkValue) : ISourceTickersConfig.DefaultLastTradedFlagsValue;
        }
        set => this[nameof(DefaultLastTradedFlags)] = value.ToString();
    }

    [JsonIgnore]
    IDictionary<string, ITickerConfig> ISourceTickersConfig.Tickers
    {
        get
        {
            foreach (var configurationSection in NonEmptyConfigs)
            {
                var tickerConfig = new TickerConfig(ConfigRoot, configurationSection.Path);
                if (tickerConfig.Ticker.IsNotNullOrEmpty() && configurationSection.Key != tickerConfig.Ticker)
                    throw new
                        ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {tickerConfig.Ticker}");
                if (tickerConfig.Ticker.IsNullOrEmpty()) tickerConfig.Ticker = configurationSection.Key;
                if (!Tickers.ContainsKey(configurationSection.Key))
                    tickers.TryAdd(configurationSection.Key, tickerConfig);
                else
                    tickers[configurationSection.Key] = tickerConfig;
            }
            return tickers.ToDictionary(tc => tc.Key, tc => (ITickerConfig)tc.Value);
        }
        set
        {
            var oldKeys = tickers.Keys.ToHashSet();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new TickerConfig(tickerConfigKvp.Value, ConfigRoot
                                                       , Path + ":" + nameof(Tickers) + $":{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.Ticker.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.Ticker)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.Ticker}");
                if (tickerConfigKvp.Value.Ticker.IsNullOrEmpty()) checkTickerConfig.Ticker = tickerConfigKvp.Key;
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) TickerConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Tickers) + $":{deletedKey}");
        }
    }

    public ISourceTickerInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker)
    {
        if (!Tickers.TryGetValue(ticker, out var tickerConfig)) return null;
        var sourceTickerINfo =
            new SourceTickerInfo
                (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker!
               , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
               , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
               , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
               , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
               , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
               , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
               , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLifeMs, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
               , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
               , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
               , tickerConfig.LayerFlags ?? DefaultLayerFlags
               , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
        return sourceTickerINfo;
    }

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers.Values)
        {
            var sourceTickerINfo =
                new SourceTickerInfo
                    (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker!
                   , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                   , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                   , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                   , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                   , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                   , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                   , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLifeMs, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                   , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                   , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                   , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
            yield return sourceTickerINfo;
        }
    }

    public IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers.Values)
            if (tickerConfig.TickerAvailability is TickerAvailability.PricingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker!
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                       , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLifeMs, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                       , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                       , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                       , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers.Values)
            if (tickerConfig.TickerAvailability is TickerAvailability.TradingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker!
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
                       , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLifeMs, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
                       , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
                       , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
                       , tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public bool AreEquivalent(ISourceTickersConfig? other, bool exactTypes = false)
    {
        var availabilitySame         = DefaultTickerAvailability == other?.DefaultTickerAvailability;
        var quoteLevelSame           = DefaultPublishTickerQuoteDetailLevel == other?.DefaultPublishTickerQuoteDetailLevel;
        var marketClassificationSame = Equals(DefaultMarketClassification, other?.DefaultMarketClassification);
        var roundingSame             = DefaultRoundingPrecision == other?.DefaultRoundingPrecision;
        var pipSame                  = DefaultPip == other?.DefaultPip;
        var maxLayersSame            = DefaultMaximumPublishedLayers == other?.DefaultMaximumPublishedLayers;
        var minSubitSizeSame         = DefaultMinSubmitSize == other?.DefaultMinSubmitSize;
        var maxSubitSizeSame         = DefaultMaxSubmitSize == other?.DefaultMaxSubmitSize;
        var incrementSizeSame        = DefaultIncrementSize == other?.DefaultIncrementSize;
        var minQuoteLifeSizeSame     = DefaultMinimumQuoteLifeMs == other?.DefaultMinimumQuoteLifeMs;
        var maxValidMsSame           = DefaultMaxValidMs == other?.DefaultMaxValidMs;
        var layerFlagsSizeSame       = DefaultLayerFlags == other?.DefaultLayerFlags;
        var lastTradedFlagsSame      = DefaultLastTradedFlags == other?.DefaultLastTradedFlags;
        var tickerConfigsSame        = Tickers.Values.SequenceEqual(other?.Tickers.Values ?? Array.Empty<ITickerConfig>());

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
        hashCode.Add(DefaultPublishTickerQuoteDetailLevel);
        hashCode.Add(DefaultTickerAvailability);
        hashCode.Add(DefaultMarketClassification);
        hashCode.Add(DefaultRoundingPrecision);
        hashCode.Add(DefaultPip);
        hashCode.Add(DefaultMinSubmitSize);
        hashCode.Add(DefaultMaxSubmitSize);
        hashCode.Add(DefaultIncrementSize);
        hashCode.Add(DefaultMinimumQuoteLifeMs);
        hashCode.Add(DefaultMaxValidMs);
        hashCode.Add(DefaultLayerFlags);
        hashCode.Add(DefaultMaximumPublishedLayers);
        hashCode.Add(DefaultLastTradedFlags);
        hashCode.Add(Tickers);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(SourceTickersConfig)}({nameof(DefaultTickerAvailability)}: {DefaultTickerAvailability}, " +
        $"{nameof(DefaultPublishTickerQuoteDetailLevel)}: {DefaultPublishTickerQuoteDetailLevel}, {nameof(DefaultMarketClassification)}: {DefaultMarketClassification}, " +
        $"{nameof(DefaultRoundingPrecision)}: {DefaultRoundingPrecision}, {nameof(DefaultPip)}: {DefaultPip}, {nameof(DefaultMaximumPublishedLayers)}: {DefaultMaximumPublishedLayers}, " +
        $"{nameof(DefaultMinSubmitSize)}: {DefaultMinSubmitSize}, {nameof(DefaultMaxSubmitSize)}: {DefaultMaxSubmitSize}, {nameof(DefaultIncrementSize)}: {DefaultIncrementSize}, " +
        $"{nameof(DefaultMinimumQuoteLifeMs)}: {DefaultMinimumQuoteLifeMs}, {nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(DefaultLayerFlags)}: {DefaultLayerFlags}, " +
        $"{nameof(DefaultLastTradedFlags)}: {DefaultLastTradedFlags}, {nameof(Tickers)}: {Tickers}, {nameof(Path)}: {Path})";
}
