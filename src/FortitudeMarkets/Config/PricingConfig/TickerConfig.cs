// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeMarkets.Config.Availability;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.PricingConfig;

public interface ITickerConfig : IInterfacesComparable<ITickerConfig>, IWeeklyAvailability
{
    ushort  InstrumentId          { get; set; }
    string  InstrumentName        { get; set; }
    string? InstrumentDescription { get; set; }

    ITradingTimeTableConfig? TradingTimeTableConfig { get; set; }

    TickerAvailability?     TickerAvailability   { get; set; }
    TickerQuoteDetailLevel? PublishedDetailLevel { get; set; }

    MarketClassificationConfig? MarketClassificationConfig { get; set; }

    decimal? RoundingPrecision      { get; set; }
    decimal? Pip                    { get; set; }
    decimal? MinSubmitSize          { get; set; }
    decimal? MaxSubmitSize          { get; set; }
    decimal? IncrementSize          { get; set; }
    ushort?  MinimumQuoteLife       { get; set; }
    uint?    DefaultMaxValidMs      { get; set; }
    ushort?  MaximumPublishedLayers { get; set; }

    LayerFlags?      LayerFlags      { get; set; }
    LastTradedFlags? LastTradedFlags { get; set; }

    PublishableQuoteInstantBehaviorFlags? QuoteBehaviorFlags { get; set; }

    string VenuePricingSymbol { get; set; }
    string VenueTradingSymbol { get; set; }
}

public class TickerConfig : ConfigSection, ITickerConfig
{
    public TickerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public TickerConfig() { }

    public TickerConfig
    (ushort instrumentId, string instrumentName, TickerAvailability? tickerAvailability = null, TickerQuoteDetailLevel? publishedQuoteLevel = null
      , MarketClassification? marketClassification = null, decimal? roundingPrecision = null, decimal? pip = null, decimal? minSubmitSize = null
      , decimal? maxSubmitSize = null, decimal? incrementSize = null, ushort? minimumQuoteLife = null, uint? defaultMaxValidMs = null
      , LayerFlags? layerFlags = null, byte? maximumPublisherLayers = null, LastTradedFlags? lastTradedFlags = null
      , string venuePricingSymbol = "", string venueTradingSymbol = "", ITradingTimeTableConfig? tradingTimeTableConfig = null
      , PublishableQuoteInstantBehaviorFlags? quoteInstantBehaviorFlags = null)
    {
        InstrumentId         = instrumentId;
        InstrumentName       = instrumentName;
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
        QuoteBehaviorFlags     = quoteInstantBehaviorFlags;
        VenuePricingSymbol     = venuePricingSymbol;
        VenueTradingSymbol     = venueTradingSymbol;
        TradingTimeTableConfig = tradingTimeTableConfig;
    }

    public TickerConfig(ITickerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        InstrumentId               = toClone.InstrumentId;
        InstrumentName             = toClone.InstrumentName;
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
        QuoteBehaviorFlags         = toClone.QuoteBehaviorFlags;
        VenuePricingSymbol         = toClone.VenuePricingSymbol;
        VenueTradingSymbol         = toClone.VenueTradingSymbol;
        TradingTimeTableConfig     = toClone.TradingTimeTableConfig;
    }

    public TickerConfig(ITickerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort InstrumentId
    {
        get => ushort.Parse(this[nameof(InstrumentId)]!);
        set => this[nameof(InstrumentId)] = value.ToString();
    }

    public string InstrumentName
    {
        get => this[nameof(InstrumentName)]!;
        set => this[nameof(InstrumentName)] = value;
    }

    public string? InstrumentDescription
    {
        get => this[nameof(InstrumentDescription)];
        set => this[nameof(InstrumentDescription)] = value;
    }

    public bool? SubscribeToPrices
    {
        get
        {
            var checkValue = this[nameof(SubscribeToPrices)];
            return checkValue.IsNotNullOrEmpty() ? bool.Parse(checkValue) : null;
        }
        set => this[nameof(SubscribeToPrices)] = value.ToString();
    }

    public bool? TradingEnabled
    {
        get
        {
            var checkValue = this[nameof(TradingEnabled)];
            return checkValue.IsNotNullOrEmpty() ? bool.Parse(checkValue) : null;
        }
        set => this[nameof(TradingEnabled)] = value.ToString();
    }

    public decimal? Pip
    {
        get
        {
            var checkValue = this[nameof(Pip)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(Pip)] = value.ToString();
    }

    public uint? DefaultMaxValidMs
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxValidMs)];
            return checkValue.IsNotNullOrEmpty() ? uint.Parse(checkValue) : null;
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
                return new MarketClassificationConfig(ConfigRoot, $"{Path}{Split}{nameof(MarketClassificationConfig)}");
            return null;
        }
        set =>
            _ = value != null
                ? new MarketClassificationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(MarketClassificationConfig)}")
                : new MarketClassificationConfig(new MarketClassification(0), ConfigRoot, $"{Path}{Split}{nameof(MarketClassificationConfig)}");
    }

    public ITimeTableConfig? ParentVenueOperatingTimeTableConfig { get; set; }

    public ITradingTimeTableConfig? TradingTimeTableConfig
    {
        get
        {
            if (GetSection(nameof(TradingTimeTableConfig)).GetChildren().SelectMany(cs => cs.GetChildren()).Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var tradingTimeTable = new TradingTimeTableConfig(ConfigRoot, $"{Path}{Split}{nameof(TradingTimeTableConfig)}")
                {
                    VenueOperatingTimeTable = ParentVenueOperatingTimeTableConfig, 
                    ParentTradingTimeTableConfig = ParentTradingTimeTableConfig
                };
                return tradingTimeTable;
            }
            return ParentTradingTimeTableConfig;
        }
        set
        {
            if (value is not Availability.TradingTimeTableConfig and { HighLiquidityTimeTable: null })
            {
                value.HighLiquidityTimeTable = ParentTradingTimeTableConfig?.HighLiquidityTimeTable;
            } else if (value is not Availability.TradingTimeTableConfig and { TradingScheduleConfig: null })
            {
                value.TradingScheduleConfig = ParentTradingTimeTableConfig?.TradingScheduleConfig;
            }
            _ = value != null ? new TradingTimeTableConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TradingTimeTableConfig)}") : null;
            if (value is TradingTimeTableConfig valueTradingTimeTableConfig)
            {
                valueTradingTimeTableConfig.VenueOperatingTimeTable      = ParentVenueOperatingTimeTableConfig;
                valueTradingTimeTableConfig.ParentTradingTimeTableConfig = ParentTradingTimeTableConfig;
            }
        }
    }

    public ITradingTimeTableConfig? ParentTradingTimeTableConfig { get; set; }

    public TickerAvailability? TickerAvailability
    {
        get
        {
            var checkValue = this[nameof(TickerAvailability)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<TickerAvailability>(checkValue) : null;
        }
        set => this[nameof(TickerAvailability)] = value.ToString();
    }

    public TickerQuoteDetailLevel? PublishedDetailLevel
    {
        get
        {
            var checkValue = this[nameof(PublishedDetailLevel)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<TickerQuoteDetailLevel>(checkValue) : null;
        }
        set => this[nameof(PublishedDetailLevel)] = value.ToString();
    }

    public decimal? RoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(RoundingPrecision)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(RoundingPrecision)] = value?.ToString();
    }

    public decimal? MinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MinSubmitSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MinSubmitSize)] = value?.ToString();
    }

    public decimal? MaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MaxSubmitSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MaxSubmitSize)] = value?.ToString();
    }

    public decimal? IncrementSize
    {
        get
        {
            var checkValue = this[nameof(IncrementSize)];
            return checkValue.IsNotNullOrEmpty() ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(IncrementSize)] = value?.ToString();
    }

    public ushort? MinimumQuoteLife
    {
        get
        {
            var checkValue = this[nameof(MinimumQuoteLife)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : null;
        }
        set => this[nameof(MinimumQuoteLife)] = value?.ToString();
    }

    public LayerFlags? LayerFlags
    {
        get
        {
            var checkValue = this[nameof(LayerFlags)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<LayerFlags>(checkValue) : null;
        }
        set => this[nameof(LayerFlags)] = value.ToString();
    }

    public ushort? MaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(MaximumPublishedLayers)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : null;
        }
        set => this[nameof(MaximumPublishedLayers)] = value?.ToString();
    }

    public LastTradedFlags? LastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(LastTradedFlags)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<LastTradedFlags>(checkValue) : null;
        }
        set => this[nameof(LastTradedFlags)] = value.ToString();
    }

    public PublishableQuoteInstantBehaviorFlags? QuoteBehaviorFlags
    {
        get
        {
            var checkValue = this[nameof(QuoteBehaviorFlags)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<PublishableQuoteInstantBehaviorFlags>(checkValue) : null;
        }
        set => this[nameof(QuoteBehaviorFlags)] = value.ToString();
    }

    public string VenuePricingSymbol
    {
        get => this[nameof(VenuePricingSymbol)] ?? InstrumentName;
        set => this[nameof(VenuePricingSymbol)] = value;
    }

    public string VenueTradingSymbol
    {
        get => this[nameof(VenueTradingSymbol)] ?? InstrumentName;
        set => this[nameof(VenueTradingSymbol)] = value;
    }

    public WeeklyTradingSchedule WeeklySchedule(DateTimeOffset forTimeInWeek)
    {
        return TradingTimeTableConfig?.WeeklySchedule(forTimeInWeek) ?? Recycler.Borrow<WeeklyTradingSchedule>();
    }

    public bool AreEquivalent(ITickerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var tickerIdSame = InstrumentId == other.InstrumentId;
        var tickerSame   = InstrumentName == other.InstrumentName;
        var tradingTimetableSame = TradingTimeTableConfig?.AreEquivalent(other.TradingTimeTableConfig, exactTypes) ??
                                   other.TradingTimeTableConfig == null;
        var quoteLevelSame           = PublishedDetailLevel == other.PublishedDetailLevel;
        var marketClassificationSame = Equals(MarketClassificationConfig, other.MarketClassificationConfig);
        var availabilitySame         = TickerAvailability == other.TickerAvailability;
        var roundingSame             = RoundingPrecision == other.RoundingPrecision;
        var pipSame                  = Pip == other.Pip;
        var minSubmitSizeSame         = MinSubmitSize == other.MinSubmitSize;
        var maxSubmitSizeSame         = MaxSubmitSize == other.MaxSubmitSize;
        var incrementSizeSame        = IncrementSize == other.IncrementSize;
        var minQuoteLifeSizeSame     = MinimumQuoteLife == other.MinimumQuoteLife;
        var maxValidMsSame           = DefaultMaxValidMs == other.DefaultMaxValidMs;
        var layerFlagsSizeSame       = LayerFlags == other.LayerFlags;
        var maxLayersSame            = MaximumPublishedLayers == other.MaximumPublishedLayers;
        var lastTradedFlagsSame      = LastTradedFlags == other.LastTradedFlags;
        var quoteBehaviorFlagsSame   = QuoteBehaviorFlags == other.QuoteBehaviorFlags;
        var pricingSymSame           = VenuePricingSymbol == other.VenuePricingSymbol;
        var tradingSymSame           = VenueTradingSymbol == other.VenueTradingSymbol;

        return tickerIdSame && tickerSame && tradingTimetableSame && availabilitySame && quoteLevelSame && marketClassificationSame
            && roundingSame && pipSame && minSubmitSizeSame && maxSubmitSizeSame && incrementSizeSame && minQuoteLifeSizeSame
            && maxValidMsSame && layerFlagsSizeSame && maxLayersSame && lastTradedFlagsSame && quoteBehaviorFlagsSame
            && pricingSymSame && tradingSymSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(InstrumentId)}"]               = null;
        root[$"{path}{Split}{nameof(InstrumentName)}"]             = null;
        root[$"{path}{Split}{nameof(TickerAvailability)}"]         = null;
        root[$"{path}{Split}{nameof(PublishedDetailLevel)}"]       = null;
        root[$"{path}{Split}{nameof(MarketClassificationConfig)}"] = null;
        root[$"{path}{Split}{nameof(RoundingPrecision)}"]          = null;
        root[$"{path}{Split}{nameof(Pip)}"]                        = null;
        root[$"{path}{Split}{nameof(MinSubmitSize)}"]              = null;
        root[$"{path}{Split}{nameof(MaxSubmitSize)}"]              = null;
        root[$"{path}{Split}{nameof(IncrementSize)}"]              = null;
        root[$"{path}{Split}{nameof(MinimumQuoteLife)}"]           = null;
        root[$"{path}{Split}{nameof(DefaultMaxValidMs)}"]          = null;
        root[$"{path}{Split}{nameof(SubscribeToPrices)}"]          = null;
        root[$"{path}{Split}{nameof(TradingEnabled)}"]             = null;
        root[$"{path}{Split}{nameof(LayerFlags)}"]                 = null;
        root[$"{path}{Split}{nameof(MaximumPublishedLayers)}"]     = null;
        root[$"{path}{Split}{nameof(LastTradedFlags)}"]            = null;
        root[$"{path}{Split}{nameof(QuoteBehaviorFlags)}"]         = null;
    }


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickerConfig, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(InstrumentId);
        hashCode.Add(InstrumentName);
        hashCode.Add(TickerAvailability);
        hashCode.Add(TradingTimeTableConfig);
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
        hashCode.Add(QuoteBehaviorFlags);
        hashCode.Add(VenuePricingSymbol);
        hashCode.Add(VenueTradingSymbol);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(TickerConfig)}({nameof(InstrumentId)}: {InstrumentId}, {nameof(InstrumentName)}: {InstrumentName}, " +
        $"{nameof(TickerAvailability)}: {TickerAvailability}, {nameof(PublishedDetailLevel)}: {PublishedDetailLevel}, " +
        $"{nameof(MarketClassificationConfig)}: {MarketClassificationConfig}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, {nameof(MinSubmitSize)}: {MinSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(SubscribeToPrices)}: {SubscribeToPrices}, " +
        $"{nameof(TradingEnabled)}: {TradingEnabled}, {nameof(LayerFlags)}: {LayerFlags}, {nameof(LastTradedFlags)}: {LastTradedFlags}, " +
        $"{nameof(VenuePricingSymbol)}: {VenuePricingSymbol},{nameof(VenueTradingSymbol)}: {VenueTradingSymbol}, " +
        $"{nameof(TradingTimeTableConfig)}: {TradingTimeTableConfig}, {nameof(QuoteBehaviorFlags)}: {QuoteBehaviorFlags}, {nameof(Path)}: {Path})";
}
