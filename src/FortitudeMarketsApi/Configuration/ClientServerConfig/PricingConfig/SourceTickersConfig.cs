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

public enum TickerAvailability
{
    AllDisabled = 0
  , Pricing
  , Trading
  , AllEnabled
}

public interface ISourceTickersConfig : IInterfacesComparable<ISourceTickersConfig>
{
    TickerAvailability         DefaultTickerAvailability     { get; set; }
    QuoteLevel                 DefaultPublishQuoteLevel      { get; set; }
    MarketClassificationConfig DefaultMarketClassification   { get; set; }
    decimal                    DefaultRoundingPrecision      { get; set; }
    decimal                    DefaultMinSubmitSize          { get; set; }
    decimal                    DefaultMaxSubmitSize          { get; set; }
    decimal                    DefaultIncrementSize          { get; set; }
    ushort                     DefaultMinimumQuoteLife       { get; set; }
    LayerFlags                 DefaultLayerFlags             { get; set; }
    byte                       DefaultMaximumPublishedLayers { get; set; }
    LastTradedFlags            DefaultLastTradedFlags        { get; set; }

    public IEnumerable<ITickerConfig>   Tickers { get; set; }
    ISourceTickerQuoteInfo?             GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker);
    IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerQuoteInfo> PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
    IEnumerable<ISourceTickerQuoteInfo> TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName);
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
        DefaultTickerAvailability     = toClone.DefaultTickerAvailability;
        DefaultPublishQuoteLevel      = toClone.DefaultPublishQuoteLevel;
        DefaultMarketClassification   = toClone.DefaultMarketClassification;
        DefaultRoundingPrecision      = toClone.DefaultRoundingPrecision;
        DefaultMinSubmitSize          = toClone.DefaultMinSubmitSize;
        DefaultMaxSubmitSize          = toClone.DefaultMaxSubmitSize;
        DefaultIncrementSize          = toClone.DefaultIncrementSize;
        DefaultMinimumQuoteLife       = toClone.DefaultMinimumQuoteLife;
        DefaultLayerFlags             = toClone.DefaultLayerFlags;
        DefaultMaximumPublishedLayers = toClone.DefaultMaximumPublishedLayers;
        DefaultLastTradedFlags        = toClone.DefaultLastTradedFlags;
        Tickers                       = toClone.Tickers;
    }

    public SourceTickersConfig(ISourceTickersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TickerAvailability DefaultTickerAvailability
    {
        get
        {
            var checkValue = this[nameof(DefaultTickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : TickerAvailability.AllEnabled;
        }
        set => this[nameof(DefaultTickerAvailability)] = value.ToString();
    }

    public QuoteLevel DefaultPublishQuoteLevel
    {
        get
        {
            var checkValue = this[nameof(DefaultPublishQuoteLevel)];
            return checkValue != null ? Enum.Parse<QuoteLevel>(checkValue) : QuoteLevel.Level2;
        }
        set => this[nameof(DefaultPublishQuoteLevel)] = value.ToString();
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

    public LayerFlags DefaultLayerFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        }
        set => this[nameof(DefaultLayerFlags)] = value.ToString();
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

    public ISourceTickerQuoteInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker)
    {
        var tickerConfig = Tickers.FirstOrDefault(tc => tc.Ticker == ticker);
        if (tickerConfig == null) return null;
        var sourceTickerINfo =
            new SourceTickerQuoteInfo
                (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
               , tickerConfig.PublishedQuoteLevel ?? DefaultPublishQuoteLevel
               , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
               , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
               , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
               , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
               , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.LayerFlags ?? DefaultLayerFlags
               , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
        return sourceTickerINfo;
    }

    public IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
        {
            var sourceTickerINfo =
                new SourceTickerQuoteInfo
                    (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                   , tickerConfig.PublishedQuoteLevel ?? DefaultPublishQuoteLevel
                   , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                   , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                   , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                   , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                   , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.LayerFlags ?? DefaultLayerFlags
                   , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
            yield return sourceTickerINfo;
        }
    }

    public IEnumerable<ISourceTickerQuoteInfo> PricingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
            if (tickerConfig.TickerAvailability is TickerAvailability.Pricing or TickerAvailability.AllEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerQuoteInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                       , tickerConfig.PublishedQuoteLevel ?? DefaultPublishQuoteLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.LayerFlags ?? DefaultLayerFlags
                       , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public IEnumerable<ISourceTickerQuoteInfo> TradingEnabledSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
            if (tickerConfig.TickerAvailability is TickerAvailability.Trading or TickerAvailability.AllEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerQuoteInfo
                        (sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                       , tickerConfig.PublishedQuoteLevel ?? DefaultPublishQuoteLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassification.MarketClassification
                       , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
                       , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
                       , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
                       , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife, tickerConfig.LayerFlags ?? DefaultLayerFlags
                       , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
                yield return sourceTickerINfo;
            }
    }

    public bool AreEquivalent(ISourceTickersConfig? other, bool exactTypes = false)
    {
        var availabilitySame         = DefaultTickerAvailability == other?.DefaultTickerAvailability;
        var quoteLevelSame           = DefaultPublishQuoteLevel == other?.DefaultPublishQuoteLevel;
        var marketClassificationSame = Equals(DefaultMarketClassification, other?.DefaultMarketClassification);
        var roundingSame             = DefaultRoundingPrecision == other?.DefaultRoundingPrecision;
        var minSubitSizeSame         = DefaultMinSubmitSize == other?.DefaultMinSubmitSize;
        var maxSubitSizeSame         = DefaultMaxSubmitSize == other?.DefaultMaxSubmitSize;
        var incrementSizeSame        = DefaultIncrementSize == other?.DefaultIncrementSize;
        var minQuoteLifeSizeSame     = DefaultMinimumQuoteLife == other?.DefaultMinimumQuoteLife;
        var layerFlagsSizeSame       = DefaultLayerFlags == other?.DefaultLayerFlags;
        var maxLayersSame            = DefaultMaximumPublishedLayers == other?.DefaultMaximumPublishedLayers;
        var lastTradedFlagsSame      = DefaultLastTradedFlags == other?.DefaultLastTradedFlags;
        var tickerConfigsSame        = Tickers.SequenceEqual(other?.Tickers ?? Array.Empty<ITickerConfig>());

        return availabilitySame && quoteLevelSame && marketClassificationSame && roundingSame && minSubitSizeSame
            && maxSubitSizeSame && incrementSizeSame && minQuoteLifeSizeSame && layerFlagsSizeSame && maxLayersSame
            && lastTradedFlagsSame && tickerConfigsSame;
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
        hashCode.Add(DefaultPublishQuoteLevel);
        hashCode.Add(DefaultTickerAvailability);
        hashCode.Add(DefaultMarketClassification);
        hashCode.Add(DefaultRoundingPrecision);
        hashCode.Add(DefaultMinSubmitSize);
        hashCode.Add(DefaultMaxSubmitSize);
        hashCode.Add(DefaultIncrementSize);
        hashCode.Add(DefaultMinimumQuoteLife);
        hashCode.Add(DefaultLayerFlags);
        hashCode.Add(DefaultMaximumPublishedLayers);
        hashCode.Add(DefaultLastTradedFlags);
        hashCode.Add(Tickers);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(SourceTickersConfig)}({nameof(DefaultTickerAvailability)}: {DefaultTickerAvailability}, " +
        $"{nameof(DefaultPublishQuoteLevel)}: {DefaultPublishQuoteLevel}, {nameof(DefaultMarketClassification)}: {DefaultMarketClassification}, " +
        $"{nameof(DefaultRoundingPrecision)}: {DefaultRoundingPrecision}, {nameof(DefaultMinSubmitSize)}: {DefaultMinSubmitSize}, " +
        $"{nameof(DefaultMaxSubmitSize)}: {DefaultMaxSubmitSize}, {nameof(DefaultIncrementSize)}: {DefaultIncrementSize}, " +
        $"{nameof(DefaultMinimumQuoteLife)}: {DefaultMinimumQuoteLife}, {nameof(DefaultLayerFlags)}: {DefaultLayerFlags}, " +
        $"{nameof(DefaultMaximumPublishedLayers)}: {DefaultMaximumPublishedLayers}, {nameof(DefaultLastTradedFlags)}: {DefaultLastTradedFlags}, " +
        $"{nameof(Tickers)}: {Tickers}, {nameof(Path)}: {Path})";
}
