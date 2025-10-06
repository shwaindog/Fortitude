// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using System.Text.Json.Serialization;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config.Availability;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.PricingConfig;

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

public interface ISourceTickersConfig : IInterfacesComparable<ISourceTickersConfig>, ITradingAvailability, IStringBearer
{
    const decimal DefaultPipValue           = SourceTickerInfo.DefaultPip;
    const decimal DefaultMinSubmitSizeValue = SourceTickerInfo.DefaultMinSubmitSize;
    const decimal DefaultMaxSubmitSizeValue = SourceTickerInfo.DefaultMaxSubmitSize;
    const decimal DefaultIncrementSizeValue = SourceTickerInfo.DefaultIncrementSize;
    const uint    DefaultMaxValidMsValue    = SourceTickerInfo.DefaultDefaultMaxValidMs;

    const decimal DefaultRoundingPrecisionValue      = SourceTickerInfo.DefaultRoundingPrecision;
    const ushort  DefaultMinimumQuoteLifeMsMsValue   = SourceTickerInfo.DefaultMinimumQuoteLife;
    const ushort  DefaultMaximumPublishedLayersValue = SourceTickerInfo.DefaultMaximumPublishedLayers;

    const LayerFlags      DefaultLayerFlagsValue      = SourceTickerInfo.DefaultLayerFlags;
    const LastTradedFlags DefaultLastTradedFlagsValue = SourceTickerInfo.DefaultLastTradedFlags;

    const PublishableQuoteInstantBehaviorFlags DefaultQuoteBehaviorFlagsValue = SourceTickerInfo.DefaultQuoteBehaviorFlags;

    TickerAvailability DefaultTickerAvailability { get; set; }

    TickerQuoteDetailLevel DefaultPublishTickerQuoteDetailLevel { get; set; }

    ITradingTimeTableConfig DefaultTickerTradingTimeTableConfig { get; set; }

    CountryCityCodes SourcePublishLocation { get; set; }

    MarketClassificationConfig DefaultMarketClassificationConfig { get; set; }

    decimal DefaultPip           { get; set; }
    decimal DefaultMinSubmitSize { get; set; }
    decimal DefaultMaxSubmitSize { get; set; }
    decimal DefaultIncrementSize { get; set; }
    uint    DefaultMaxValidMs    { get; set; }

    decimal DefaultRoundingPrecision      { get; set; }
    ushort  DefaultMinimumQuoteLifeMs     { get; set; }
    ushort  DefaultMaximumPublishedLayers { get; set; }

    LayerFlags      DefaultLayerFlags      { get; set; }
    LastTradedFlags DefaultLastTradedFlags { get; set; }

    PublishableQuoteInstantBehaviorFlags DefaultQuoteBehaviorFlags { get; set; }

    public IReadOnlyDictionary<string, ITickerConfig> Tickers { get; set; }

    ISourceTickerInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker, CountryCityCodes myLocation, bool isPricePublisher);

    IEnumerable<ISourceTickerInfo> AllSourceTickerInfos(ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isPricePublisher);

    IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos
        (ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isPricePublisher);

    IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos
        (ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isPricePublisher);
}

public class SourceTickersConfig : ConfigSection, ISourceTickersConfig
{
    private readonly IDictionary<string, ITickerConfig> tickers = new Dictionary<string, ITickerConfig>();
    public SourceTickersConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public SourceTickersConfig() { }

    public SourceTickersConfig(CountryCityCodes sourcePublishLocation, params ITickerConfig[] tickersConfigs)
    {
        SourcePublishLocation                = sourcePublishLocation;
        ((ISourceTickersConfig)this).Tickers = tickersConfigs.ToDictionary(tc => tc.InstrumentName);
    }

    public SourceTickersConfig(ISourceTickersConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        SourcePublishLocation = toClone.SourcePublishLocation;

        DefaultPublishTickerQuoteDetailLevel = toClone.DefaultPublishTickerQuoteDetailLevel;
        DefaultMaximumPublishedLayers        = toClone.DefaultMaximumPublishedLayers;
        DefaultMarketClassificationConfig    = toClone.DefaultMarketClassificationConfig;

        DefaultTickerAvailability = toClone.DefaultTickerAvailability;
        DefaultMinimumQuoteLifeMs = toClone.DefaultMinimumQuoteLifeMs;
        DefaultRoundingPrecision  = toClone.DefaultRoundingPrecision;

        DefaultLastTradedFlags = toClone.DefaultLastTradedFlags;
        DefaultMinSubmitSize   = toClone.DefaultMinSubmitSize;
        DefaultMaxSubmitSize   = toClone.DefaultMaxSubmitSize;
        DefaultIncrementSize   = toClone.DefaultIncrementSize;
        DefaultMaxValidMs      = toClone.DefaultMaxValidMs;
        DefaultLayerFlags      = toClone.DefaultLayerFlags;

        DefaultQuoteBehaviorFlags = toClone.DefaultQuoteBehaviorFlags;

        DefaultPip = toClone.DefaultPip;

        ((ISourceTickersConfig)this).Tickers = toClone.Tickers;
    }

    public SourceTickersConfig(ISourceTickersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    private IEnumerable<IConfigurationSection> NonEmptyConfigs =>
        GetSection(nameof(Tickers)).GetChildren().Where(cs => cs[nameof(ITickerConfig.InstrumentId)] != null);

    public CountryCityCodes SourcePublishLocation
    {
        get
        {
            var checkValue = this[nameof(SourcePublishLocation)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<CountryCityCodes>(checkValue) : CountryCityCodes.Unknown;
        }
        set => this[nameof(SourcePublishLocation)] = value.ToString();
    }

    public ITimeTableConfig? ParentVenueOperatingTimeTableConfig { get; set; }

    public ITradingTimeTableConfig DefaultTickerTradingTimeTableConfig
    {
        get =>
            new TradingTimeTableConfig(ConfigRoot, $"{Path}{Split}{nameof(DefaultTickerTradingTimeTableConfig)}")
            {
                VenueOperatingTimeTable = ParentVenueOperatingTimeTableConfig
            };
        set => _ = new TradingTimeTableConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DefaultTickerTradingTimeTableConfig)}");
    }

    public TickerAvailability DefaultTickerAvailability
    {
        get
        {
            var checkValue = this[nameof(DefaultTickerAvailability)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<TickerAvailability>(checkValue) : TickerAvailability.PricingAndTradingEnabled;
        }
        set => this[nameof(DefaultTickerAvailability)] = value.ToString();
    }

    public TickerQuoteDetailLevel DefaultPublishTickerQuoteDetailLevel
    {
        get
        {
            var checkValue = this[nameof(DefaultPublishTickerQuoteDetailLevel)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<TickerQuoteDetailLevel>(checkValue) : TickerQuoteDetailLevel.Level2Quote;
        }
        set => this[nameof(DefaultPublishTickerQuoteDetailLevel)] = value.ToString();
    }

    public MarketClassificationConfig DefaultMarketClassificationConfig
    {
        get => new(ConfigRoot, $"{Path}{Split}{nameof(DefaultMarketClassificationConfig)}");
        set => _ = new MarketClassificationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DefaultMarketClassificationConfig)}");
    }

    public decimal DefaultRoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(DefaultRoundingPrecision)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultRoundingPrecisionValue;
        }
        set => this[nameof(DefaultRoundingPrecision)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal DefaultPip
    {
        get
        {
            var checkValue = this[nameof(DefaultPip)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultPipValue;
        }
        set => this[nameof(DefaultPip)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public ushort DefaultMaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(DefaultMaximumPublishedLayers)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : ISourceTickersConfig.DefaultMaximumPublishedLayersValue;
        }
        set => this[nameof(DefaultMaximumPublishedLayers)] = value.ToString();
    }

    public decimal DefaultMinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMinSubmitSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultMinSubmitSizeValue;
        }
        set => this[nameof(DefaultMinSubmitSize)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal DefaultMaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxSubmitSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultMaxSubmitSizeValue;
        }
        set => this[nameof(DefaultMaxSubmitSize)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal DefaultIncrementSize
    {
        get
        {
            var checkValue = this[nameof(DefaultIncrementSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : ISourceTickersConfig.DefaultIncrementSizeValue;
        }
        set => this[nameof(DefaultIncrementSize)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public ushort DefaultMinimumQuoteLifeMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMinimumQuoteLifeMs)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : ISourceTickersConfig.DefaultMinimumQuoteLifeMsMsValue;
        }
        set => this[nameof(DefaultMinimumQuoteLifeMs)] = value.ToString();
    }

    public uint DefaultMaxValidMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxValidMs)];
            return checkValue.IsNotNullOrEmpty() ? uint.Parse(checkValue) : ISourceTickersConfig.DefaultMaxValidMsValue;
        }
        set => this[nameof(DefaultMaxValidMs)] = value.ToString();
    }

    public LayerFlags DefaultLayerFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLayerFlags)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<LayerFlags>(checkValue) : ISourceTickersConfig.DefaultLayerFlagsValue;
        }
        set => this[nameof(DefaultLayerFlags)] = value.ToString();
    }

    public LastTradedFlags DefaultLastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLastTradedFlags)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<LastTradedFlags>(checkValue) : ISourceTickersConfig.DefaultLastTradedFlagsValue;
        }
        set => this[nameof(DefaultLastTradedFlags)] = value.ToString();
    }

    public PublishableQuoteInstantBehaviorFlags DefaultQuoteBehaviorFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultQuoteBehaviorFlags)];
            return checkValue.IsNotNullOrEmpty()
                ? Enum.Parse<PublishableQuoteInstantBehaviorFlags>(checkValue)
                : ISourceTickersConfig.DefaultQuoteBehaviorFlagsValue;
        }
        set => this[nameof(DefaultQuoteBehaviorFlags)] = value.ToString();
    }

    [JsonIgnore]
    public IReadOnlyDictionary<string, ITickerConfig> Tickers
    {
        get
        {
            if (!tickers.Any())
            {
                foreach (var configurationSection in NonEmptyConfigs)
                {
                    var tickerConfig = new TickerConfig(ConfigRoot, configurationSection.Path);
                    if (tickerConfig.InstrumentName.IsNotNullOrEmpty() && configurationSection.Key != tickerConfig.InstrumentName)
                        throw new
                            ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {tickerConfig.InstrumentName}");
                    if (tickerConfig.InstrumentName.IsNullOrEmpty()) tickerConfig.InstrumentName = configurationSection.Key;
                    tickerConfig.ParentTradingTimeTableConfig        = DefaultTickerTradingTimeTableConfig;
                    tickerConfig.ParentVenueOperatingTimeTableConfig = ParentVenueOperatingTimeTableConfig;
                    if (!tickers.ContainsKey(configurationSection.Key))
                        tickers.TryAdd(configurationSection.Key, tickerConfig);
                    else
                        tickers[configurationSection.Key] = tickerConfig;
                }
            }
            return tickers.AsReadOnly();
        }
        set
        {
            var oldKeys = tickers.Keys.ToHashSet();
            tickers.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new TickerConfig(tickerConfigKvp.Value, ConfigRoot
                                                       , $"{Path}{Split}{nameof(Tickers)}:{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.InstrumentName.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.InstrumentName)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.InstrumentName}");
                if (tickerConfigKvp.Value.InstrumentName.IsNullOrEmpty()) checkTickerConfig.InstrumentName = tickerConfigKvp.Key;
                checkTickerConfig.ParentTradingTimeTableConfig        = DefaultTickerTradingTimeTableConfig;
                checkTickerConfig.ParentVenueOperatingTimeTableConfig = ParentVenueOperatingTimeTableConfig;
                tickers.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys)
                TickerConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(Tickers)}{Split}{deletedKey}");
        }
    }

    public WeeklyTradingSchedule? TickerWeeklyTradingSchedule(DateTimeOffset forTimeInWeek, string instrumentName)
    {
        var tickersConfig = Tickers;
        if (tickersConfig.TryGetValue(instrumentName, out var tickerConfig))
        {
            return tickerConfig.TradingTimeTableConfig!.WeeklySchedule(forTimeInWeek);
        }
        return null;
    }

    public bool HasTradingSchedule(string instrumentName)
    {
        return Tickers.ContainsKey(instrumentName);
    }

    public IRecyclableReadOnlyDictionary<string, WeeklyTradingSchedule> AllTickersWeeklyTradingSchedules(DateTime forTimeInWeek)
    {
        var tradingScheduleDict = Recycler.Borrow<ReusableDictionary<string, WeeklyTradingSchedule>>();
        foreach (var tickerConfigKvp in Tickers)
        {
            tradingScheduleDict.Add(tickerConfigKvp.Key,
                                    tickerConfigKvp.Value.TradingTimeTableConfig!.WeeklySchedule(new DateTimeOffset(forTimeInWeek)));
        }
        return tradingScheduleDict;
    }

    public ISourceTickerInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker, CountryCityCodes myLocation, bool isAdapter)
    {
        if (!Tickers.TryGetValue(ticker, out var tickerConfig)) return null;
        var sourceTickerINfo =
            new SourceTickerInfo
                (sourceId, sourceName, tickerConfig.InstrumentId, tickerConfig.InstrumentName
               , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
               , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassificationConfig.MarketClassification
               , SourcePublishLocation
               , isAdapter ? myLocation : CountryCityCodes.Unknown
               , !isAdapter ? myLocation : CountryCityCodes.Unknown
               , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers
               , tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision, tickerConfig.Pip ?? DefaultPip
               , tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize
               , tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize, tickerConfig.IncrementSize ?? DefaultIncrementSize
               , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLifeMs, tickerConfig.DefaultMaxValidMs ?? DefaultMaxValidMs
               , tickerConfig.TickerAvailability.PricingEnabled() ?? DefaultTickerAvailability.IsPricingEnabled()
               , tickerConfig.TickerAvailability.TradingEnabled() ?? DefaultTickerAvailability.IsTradingEnabled()
               , tickerConfig.LayerFlags ?? DefaultLayerFlags
               , tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags
               , tickerConfig.QuoteBehaviorFlags ?? DefaultQuoteBehaviorFlags);
        return sourceTickerINfo;
    }

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos(ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isAdapter)
    {
        foreach (var tickerConfig in Tickers.Values)
        {
            var sourceTickerINfo =
                new SourceTickerInfo
                    (sourceId, sourceName, tickerConfig.InstrumentId, tickerConfig.InstrumentName
                   , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                   , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassificationConfig.MarketClassification
                   , SourcePublishLocation
                   , isAdapter ? myLocation : CountryCityCodes.Unknown
                   , !isAdapter ? myLocation : CountryCityCodes.Unknown
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

    public IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos
        (ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isAdapter)
    {
        foreach (var tickerConfig in Tickers.Values)
            if (tickerConfig.TickerAvailability is TickerAvailability.PricingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.InstrumentId, tickerConfig.InstrumentName
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassificationConfig.MarketClassification
                       , SourcePublishLocation
                       , isAdapter ? myLocation : CountryCityCodes.Unknown
                       , !isAdapter ? myLocation : CountryCityCodes.Unknown
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

    public IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos
        (ushort sourceId, string sourceName, CountryCityCodes myLocation, bool isAdapter)
    {
        foreach (var tickerConfig in Tickers.Values)
            if (tickerConfig.TickerAvailability is TickerAvailability.TradingEnabled or TickerAvailability.PricingAndTradingEnabled)
            {
                var sourceTickerINfo =
                    new SourceTickerInfo
                        (sourceId, sourceName, tickerConfig.InstrumentId, tickerConfig.InstrumentName
                       , tickerConfig.PublishedDetailLevel ?? DefaultPublishTickerQuoteDetailLevel
                       , tickerConfig.MarketClassificationConfig?.MarketClassification ?? DefaultMarketClassificationConfig.MarketClassification
                       , SourcePublishLocation
                       , isAdapter ? myLocation : CountryCityCodes.Unknown
                       , !isAdapter ? myLocation : CountryCityCodes.Unknown
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
        var marketClassificationSame = Equals(DefaultMarketClassificationConfig, other?.DefaultMarketClassificationConfig);
        var roundingSame             = DefaultRoundingPrecision == other?.DefaultRoundingPrecision;
        var pipSame                  = DefaultPip == other?.DefaultPip;
        var maxLayersSame            = DefaultMaximumPublishedLayers == other?.DefaultMaximumPublishedLayers;
        var minSubmitSizeSame        = DefaultMinSubmitSize == other?.DefaultMinSubmitSize;
        var maxSubmitSizeSame        = DefaultMaxSubmitSize == other?.DefaultMaxSubmitSize;
        var incrementSizeSame        = DefaultIncrementSize == other?.DefaultIncrementSize;
        var minQuoteLifeSizeSame     = DefaultMinimumQuoteLifeMs == other?.DefaultMinimumQuoteLifeMs;
        var maxValidMsSame           = DefaultMaxValidMs == other?.DefaultMaxValidMs;
        var layerFlagsSizeSame       = DefaultLayerFlags == other?.DefaultLayerFlags;
        var lastTradedFlagsSame      = DefaultLastTradedFlags == other?.DefaultLastTradedFlags;
        var tickerConfigsSame        = Tickers.Values.SequenceEqual(other?.Tickers.Values ?? Array.Empty<ITickerConfig>());

        return availabilitySame && quoteLevelSame && marketClassificationSame && roundingSame && pipSame && maxLayersSame && minSubmitSizeSame
            && maxSubmitSizeSame && incrementSizeSame && minQuoteLifeSizeSame && maxValidMsSame && layerFlagsSizeSame && lastTradedFlagsSame
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
        hashCode.Add(DefaultMarketClassificationConfig);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(DefaultTickerAvailability), DefaultTickerAvailability)
            .Field.AlwaysAdd(nameof(DefaultPublishTickerQuoteDetailLevel), DefaultPublishTickerQuoteDetailLevel)
            .Field.AlwaysReveal(nameof(DefaultMarketClassificationConfig), DefaultMarketClassificationConfig)
            .Field.AlwaysAdd(nameof(DefaultRoundingPrecision), DefaultRoundingPrecision)
            .Field.AlwaysAdd(nameof(DefaultPip), DefaultPip)
            .Field.AlwaysAdd(nameof(DefaultMaximumPublishedLayers), DefaultMaximumPublishedLayers)
            .Field.AlwaysAdd(nameof(DefaultMinSubmitSize), DefaultMinSubmitSize)
            .Field.AlwaysAdd(nameof(DefaultMaxSubmitSize), DefaultMaxSubmitSize)
            .Field.AlwaysAdd(nameof(DefaultIncrementSize), DefaultIncrementSize)
            .Field.AlwaysAdd(nameof(DefaultMinimumQuoteLifeMs), DefaultMinimumQuoteLifeMs)
            .Field.AlwaysAdd(nameof(DefaultMaxValidMs), DefaultMaxValidMs)
            .Field.AlwaysAdd(nameof(DefaultLayerFlags), DefaultLayerFlags)
            .Field.AlwaysAdd(nameof(DefaultLastTradedFlags), DefaultLastTradedFlags)
            .KeyedCollectionField.AlwaysAddAll(nameof(Tickers), Tickers)
            .Complete();

    public override string ToString() =>
        $"{nameof(SourceTickersConfig)}({nameof(DefaultTickerAvailability)}: {DefaultTickerAvailability}, " +
        $"{nameof(DefaultPublishTickerQuoteDetailLevel)}: {DefaultPublishTickerQuoteDetailLevel}, {nameof(DefaultMarketClassificationConfig)}: {DefaultMarketClassificationConfig}, " +
        $"{nameof(DefaultRoundingPrecision)}: {DefaultRoundingPrecision}, {nameof(DefaultPip)}: {DefaultPip}, {nameof(DefaultMaximumPublishedLayers)}: {DefaultMaximumPublishedLayers}, " +
        $"{nameof(DefaultMinSubmitSize)}: {DefaultMinSubmitSize}, {nameof(DefaultMaxSubmitSize)}: {DefaultMaxSubmitSize}, {nameof(DefaultIncrementSize)}: {DefaultIncrementSize}, " +
        $"{nameof(DefaultMinimumQuoteLifeMs)}: {DefaultMinimumQuoteLifeMs}, {nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(DefaultLayerFlags)}: {DefaultLayerFlags}, " +
        $"{nameof(DefaultLastTradedFlags)}: {DefaultLastTradedFlags}, {nameof(Tickers)}: {Tickers}, {nameof(Path)}: {Path})";
}
