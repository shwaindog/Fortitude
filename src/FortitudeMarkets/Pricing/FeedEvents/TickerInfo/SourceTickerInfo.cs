// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Protocols;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

public interface ISourceTickerInfo : IPricingInstrumentId, IInterfacesComparable<ISourceTickerInfo>, IVersionedMessage
{
    [JsonIgnore] TickerQuoteDetailLevel PublishedTickerQuoteDetailLevel { get; set; }

    [JsonIgnore] decimal RoundingPrecision { get; set; }
    [JsonIgnore] decimal Pip { get; set; }
    [JsonIgnore] decimal MinSubmitSize { get; set; }
    [JsonIgnore] decimal MaxSubmitSize { get; set; }
    [JsonIgnore] decimal IncrementSize { get; set; }
    [JsonIgnore] ushort MaximumPublishedLayers { get; set; }
    [JsonIgnore] ushort MinimumQuoteLife { get; set; }
    [JsonIgnore] uint DefaultMaxValidMs { get; set; }
    [JsonIgnore] bool SubscribeToPrices { get; set; }
    [JsonIgnore] bool TradingEnabled { get; set; }


    LayerFlags LayerFlags { get; set; }

    LastTradedFlags LastTradedFlags { get; set; }

    PublishableQuoteInstantBehaviorFlags QuoteBehaviorFlags { get; set; }

    [JsonIgnore] string FormatPrice { get; }

    ISourceTickerInfo CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new ISourceTickerInfo Clone();
}

public interface ICanHaveSourceTickerDefinition
{
    ISourceTickerInfo? SourceTickerInfo { get; }
}

public interface IMutableCanHaveSourceTickerDefinition : ICanHaveSourceTickerDefinition
{
    new ISourceTickerInfo? SourceTickerInfo { get; set; }
}

public class SourceTickerInfo : PricingInstrumentId, ISourceTickerInfo, ICloneable<SourceTickerInfo>
{
    public const decimal DefaultPip                    = 0.0001m;
    public const decimal DefaultRoundingPrecision      = 0.00001m;
    public const uint    DefaultDefaultMaxValidMs      = 10_000;
    public const bool    DefaultSubscribeToPrices      = true;
    public const bool    DefaultTradingEnabled         = false;
    public const ushort  DefaultMaximumPublishedLayers = 1;
    public const decimal DefaultMinSubmitSize          = 1m;
    public const decimal DefaultMaxSubmitSize          = 1_000_000m;
    public const decimal DefaultIncrementSize          = 1m;
    public const ushort  DefaultMinimumQuoteLife       = 100;

    public const LayerFlags             DefaultLayerFlags      = LayerFlags.None;
    public const LastTradedFlags        DefaultLastTradedFlags = LastTradedFlags.None;
    public const TickerQuoteDetailLevel DefaultQuoteLevel      = TickerQuoteDetailLevel.Level1Quote;

    public const PublishableQuoteInstantBehaviorFlags DefaultQuoteBehaviorFlags = PublishableQuoteInstantBehaviorFlags.DefaultAdapterFlags;

    public const LayerFlags PriceVolumeFlags = LayerFlags.Price | LayerFlags.Volume;

    private string? formatPrice;

    public SourceTickerInfo()
    {
        PublishedTickerQuoteDetailLevel = DefaultQuoteLevel;

        Pip = DefaultPip;

        RoundingPrecision      = DefaultRoundingPrecision;
        DefaultMaxValidMs      = DefaultDefaultMaxValidMs;
        SubscribeToPrices      = DefaultSubscribeToPrices;
        TradingEnabled         = DefaultTradingEnabled;
        MaximumPublishedLayers = DefaultMaximumPublishedLayers;
        MinSubmitSize          = DefaultMinSubmitSize;
        MaxSubmitSize          = DefaultMaxSubmitSize;
        IncrementSize          = DefaultIncrementSize;
        MinimumQuoteLife       = DefaultMinimumQuoteLife;
        LayerFlags             = DefaultLayerFlags;
        LastTradedFlags        = DefaultLastTradedFlags;
        QuoteBehaviorFlags     = DefaultQuoteBehaviorFlags;
    }

    public SourceTickerInfo(ISourceTickerId sourceTickerId)
    {
        SourceId       = sourceTickerId.SourceId;
        InstrumentId   = sourceTickerId.InstrumentId;
        SourceName     = sourceTickerId.SourceName;
        InstrumentName = sourceTickerId.InstrumentName;

        PublishedTickerQuoteDetailLevel = DefaultQuoteLevel;

        Pip = DefaultPip;

        RoundingPrecision      = DefaultRoundingPrecision;
        DefaultMaxValidMs      = DefaultDefaultMaxValidMs;
        SubscribeToPrices      = DefaultSubscribeToPrices;
        TradingEnabled         = DefaultTradingEnabled;
        MaximumPublishedLayers = DefaultMaximumPublishedLayers;
        MinSubmitSize          = DefaultMinSubmitSize;
        MaxSubmitSize          = DefaultMaxSubmitSize;
        IncrementSize          = DefaultIncrementSize;
        MinimumQuoteLife       = DefaultMinimumQuoteLife;
        LayerFlags             = DefaultLayerFlags;
        LastTradedFlags        = DefaultLastTradedFlags;
        QuoteBehaviorFlags     = DefaultQuoteBehaviorFlags;
    }

    public SourceTickerInfo(SourceTickerIdentifier sourceTickerIdentifier) : base(sourceTickerIdentifier)
    {
        SourceId       = sourceTickerIdentifier.SourceId;
        InstrumentId   = sourceTickerIdentifier.InstrumentId;
        SourceName     = sourceTickerIdentifier.SourceName;
        InstrumentName = sourceTickerIdentifier.InstrumentName;

        PublishedTickerQuoteDetailLevel = DefaultQuoteLevel;

        Pip = DefaultPip;

        RoundingPrecision      = DefaultRoundingPrecision;
        DefaultMaxValidMs      = DefaultDefaultMaxValidMs;
        SubscribeToPrices      = DefaultSubscribeToPrices;
        TradingEnabled         = DefaultTradingEnabled;
        MaximumPublishedLayers = DefaultMaximumPublishedLayers;
        MinSubmitSize          = DefaultMinSubmitSize;
        MaxSubmitSize          = DefaultMaxSubmitSize;
        IncrementSize          = DefaultIncrementSize;
        MinimumQuoteLife       = DefaultMinimumQuoteLife;
        LayerFlags             = DefaultLayerFlags;
        LastTradedFlags        = DefaultLastTradedFlags;
        QuoteBehaviorFlags     = DefaultQuoteBehaviorFlags;
    }

    public SourceTickerInfo
        (IPricingInstrumentId pricingInstrumentId, TickerQuoteDetailLevel tickerQuoteDetailLevel = DefaultQuoteLevel)
        : base(pricingInstrumentId)
    {
        PublishedTickerQuoteDetailLevel = tickerQuoteDetailLevel;

        Pip = DefaultPip;

        RoundingPrecision      = DefaultRoundingPrecision;
        DefaultMaxValidMs      = DefaultDefaultMaxValidMs;
        SubscribeToPrices      = DefaultSubscribeToPrices;
        TradingEnabled         = DefaultTradingEnabled;
        MaximumPublishedLayers = DefaultMaximumPublishedLayers;
        MinSubmitSize          = DefaultMinSubmitSize;
        MaxSubmitSize          = DefaultMaxSubmitSize;
        IncrementSize          = DefaultIncrementSize;
        MinimumQuoteLife       = DefaultMinimumQuoteLife;
        LayerFlags             = tickerQuoteDetailLevel >= TickerQuoteDetailLevel.Level2Quote ? PriceVolumeFlags : DefaultLayerFlags;
        LastTradedFlags        = DefaultLastTradedFlags;
        QuoteBehaviorFlags     = DefaultQuoteBehaviorFlags;
    }

    public SourceTickerInfo(PricingInstrumentIdValue pricingInstrumentId, TickerQuoteDetailLevel tickerQuoteDetailLevel = DefaultQuoteLevel)
        : base(pricingInstrumentId)
    {
        PublishedTickerQuoteDetailLevel = tickerQuoteDetailLevel;

        Pip = DefaultPip;

        RoundingPrecision      = DefaultRoundingPrecision;
        DefaultMaxValidMs      = DefaultDefaultMaxValidMs;
        SubscribeToPrices      = DefaultSubscribeToPrices;
        TradingEnabled         = DefaultTradingEnabled;
        MaximumPublishedLayers = DefaultMaximumPublishedLayers;
        MinSubmitSize          = DefaultMinSubmitSize;
        MaxSubmitSize          = DefaultMaxSubmitSize;
        IncrementSize          = DefaultIncrementSize;
        MinimumQuoteLife       = DefaultMinimumQuoteLife;
        LayerFlags             = tickerQuoteDetailLevel >= TickerQuoteDetailLevel.Level2Quote ? PriceVolumeFlags : DefaultLayerFlags;
        LastTradedFlags        = DefaultLastTradedFlags;
        QuoteBehaviorFlags     = DefaultQuoteBehaviorFlags;
    }

    public SourceTickerInfo
    (ushort sourceId, string sourceName, ushort tickerId, string ticker, TickerQuoteDetailLevel publishedTickerQuoteDetailLevel = DefaultQuoteLevel
      , MarketClassification marketClassification = default
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown
      , ushort maximumPublishedLayers = DefaultMaximumPublishedLayers
      , decimal roundingPrecision = DefaultRoundingPrecision
      , decimal pip = DefaultPip, decimal minSubmitSize = DefaultMinSubmitSize, decimal maxSubmitSize = DefaultMaxSubmitSize
      , decimal incrementSize = DefaultIncrementSize
      , ushort minimumQuoteLife = DefaultMinimumQuoteLife, uint defaultMaxValidMs = DefaultDefaultMaxValidMs
      , bool subscribeToPrices = DefaultSubscribeToPrices, bool tradingEnabled = DefaultTradingEnabled
      , LayerFlags layerFlags = DefaultLayerFlags, LastTradedFlags lastTradedFlags = DefaultLastTradedFlags
      , PublishableQuoteInstantBehaviorFlags quoteBehaviorFlags = DefaultQuoteBehaviorFlags)
        : base(sourceId, tickerId, sourceName, ticker, new DiscreetTimePeriod(TimeBoundaryPeriod.Tick), InstrumentType.Price
             , marketClassification, null, sourcePublishLocation, adapterReceiveLocation, clientReceiveLocation)
    {
        PublishedTickerQuoteDetailLevel = publishedTickerQuoteDetailLevel;

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
        LayerFlags = publishedTickerQuoteDetailLevel >= TickerQuoteDetailLevel.Level2Quote
                  && layerFlags == LayerFlags.None
            ? PriceVolumeFlags
            : layerFlags;
        LastTradedFlags    = lastTradedFlags;
        QuoteBehaviorFlags = quoteBehaviorFlags;

        Category = PublishedTickerQuoteDetailLevel.ToString();
    }

    public SourceTickerInfo(ISourceTickerInfo toClone) : base(toClone)
    {
        PublishedTickerQuoteDetailLevel = toClone.PublishedTickerQuoteDetailLevel;

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
        QuoteBehaviorFlags     = toClone.QuoteBehaviorFlags;

        foreach (var instrumentFields in toClone.FilledAttributes) this[instrumentFields.Key] = instrumentFields.Value;
    }

    public SourceTickerInfo(SourceTickerInfo toClone) : this((ISourceTickerInfo)toClone) { }
    public SourceTickerInfo(PQSourceTickerInfo toClone) : this((ISourceTickerInfo)toClone) { }


    public uint MessageId => SourceInstrumentId;
    public byte Version => 1;

    public TickerQuoteDetailLevel PublishedTickerQuoteDetailLevel { get; set; }

    public ushort MaximumPublishedLayers { get; set; }

    public decimal RoundingPrecision { get; set; }
    public decimal Pip { get; set; }
    public decimal MinSubmitSize { get; set; }
    public decimal MaxSubmitSize { get; set; }
    public decimal IncrementSize { get; set; }
    public ushort MinimumQuoteLife { get; set; }
    public uint DefaultMaxValidMs { get; set; }
    public bool SubscribeToPrices { get; set; }
    public bool TradingEnabled { get; set; }

    public LayerFlags LayerFlags { get; set; }

    public LastTradedFlags LastTradedFlags { get; set; }

    public PublishableQuoteInstantBehaviorFlags QuoteBehaviorFlags { get; set; }

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

    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPricingInstrumentId IPricingInstrumentId.Clone() => Clone();

    ISourceTickerInfo ISourceTickerInfo.Clone() => Clone();


    public override SourceTickerInfo Clone() =>
        Recycler?.Borrow<SourceTickerInfo>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new SourceTickerInfo((ISourceTickerInfo)this);

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
    (IReusableObject<IVersionedMessage> source
      , CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    ISourceTickerInfo ISourceTickerInfo.CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override SourceTickerInfo CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISourceTickerInfo srcTkrQuoteInfo)
        {
            Pip = srcTkrQuoteInfo.Pip;

            PublishedTickerQuoteDetailLevel = srcTkrQuoteInfo.PublishedTickerQuoteDetailLevel;
            MaximumPublishedLayers          = srcTkrQuoteInfo.MaximumPublishedLayers;

            RoundingPrecision = srcTkrQuoteInfo.RoundingPrecision;
            SubscribeToPrices = srcTkrQuoteInfo.SubscribeToPrices;
            TradingEnabled    = srcTkrQuoteInfo.TradingEnabled;
            RoundingPrecision = srcTkrQuoteInfo.RoundingPrecision;
            MinSubmitSize     = srcTkrQuoteInfo.MinSubmitSize;
            MaxSubmitSize     = srcTkrQuoteInfo.MaxSubmitSize;
            IncrementSize     = srcTkrQuoteInfo.IncrementSize;
            MinimumQuoteLife  = srcTkrQuoteInfo.MinimumQuoteLife;
            LayerFlags        = srcTkrQuoteInfo.LayerFlags;
            LastTradedFlags   = srcTkrQuoteInfo.LastTradedFlags;
        }
        return this;
    }

    bool IInterfacesComparable<ISourceTickerInfo>.AreEquivalent(ISourceTickerInfo? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other is not ISourceTickerInfo srcTickerInfo) return false;
        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var detailLevelSame        = PublishedTickerQuoteDetailLevel == srcTickerInfo.PublishedTickerQuoteDetailLevel;
        var maxPublishedLayersSame = MaximumPublishedLayers == srcTickerInfo.MaximumPublishedLayers;
        var roundingPrecisionSame  = RoundingPrecision == srcTickerInfo.RoundingPrecision;

        var pipSame = Pip == srcTickerInfo.Pip;

        var minSubmitSizeSame   = MinSubmitSize == srcTickerInfo.MinSubmitSize;
        var maxSubmitSizeSame   = MaxSubmitSize == srcTickerInfo.MaxSubmitSize;
        var incrmntSizeSame     = IncrementSize == srcTickerInfo.IncrementSize;
        var minQuoteLifeSame    = MinimumQuoteLife == srcTickerInfo.MinimumQuoteLife;
        var defaultMaxValidSame = DefaultMaxValidMs == srcTickerInfo.DefaultMaxValidMs;
        var subscribeSame       = SubscribeToPrices == srcTickerInfo.SubscribeToPrices;
        var tradingEnabledSame  = TradingEnabled == srcTickerInfo.TradingEnabled;
        var layerFlagsSame      = LayerFlags == srcTickerInfo.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == srcTickerInfo.LastTradedFlags;

        var allAreSame = baseIsSame && detailLevelSame && maxPublishedLayersSame && roundingPrecisionSame && pipSame && minSubmitSizeSame
                      && maxSubmitSizeSame && incrmntSizeSame && minQuoteLifeSame && defaultMaxValidSame && subscribeSame && tradingEnabledSame
                      && layerFlagsSame && lastTradedFlagsSame;
        return allAreSame;
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

    protected string SourceTickerInfoToStringMembers =>
        $"{PricingInstrumentIdToStringMembers}, {nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, " +
        $"{nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, {nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, " +
        $"{nameof(SubscribeToPrices)}: {SubscribeToPrices}, {nameof(TradingEnabled)}: {TradingEnabled}, {nameof(LayerFlags)}: {LayerFlags:F}, " +
        $"{nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags}, " +
        $"{nameof(QuoteBehaviorFlags)}: {QuoteBehaviorFlags}";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(RoundingPrecision), RoundingPrecision)
            .Field.AlwaysAdd(nameof(Pip), Pip)
            .Field.AlwaysAdd(nameof(MinSubmitSize), MinSubmitSize)
            .Field.AlwaysAdd(nameof(MaxSubmitSize), MaxSubmitSize)
            .Field.AlwaysAdd(nameof(IncrementSize), IncrementSize)
            .Field.AlwaysAdd(nameof(MinimumQuoteLife), MinimumQuoteLife)
            .Field.AlwaysAdd(nameof(DefaultMaxValidMs), DefaultMaxValidMs)
            .Field.AlwaysAdd(nameof(SubscribeToPrices), SubscribeToPrices)
            .Field.AlwaysAdd(nameof(TradingEnabled), TradingEnabled)
            .Field.AlwaysAdd(nameof(LayerFlags), LayerFlags)
            .Field.AlwaysAdd(nameof(MaximumPublishedLayers), MaximumPublishedLayers)
            .Field.AlwaysAdd(nameof(LastTradedFlags), LastTradedFlags)
            .Field.AlwaysAdd(nameof(QuoteBehaviorFlags), QuoteBehaviorFlags)
            .Complete();

    public override string ToString() => $"{nameof(SourceTickerInfo)}{{{SourceTickerInfoToStringMembers}}}";


    public static implicit operator PricingInstrumentIdValue(SourceTickerInfo sourceTickerId) =>
        new(sourceTickerId, sourceTickerId.CoveringPeriod, sourceTickerId.InstrumentType);

    public static implicit operator SourceTickerIdentifier(SourceTickerInfo sourceTickerId) => new(sourceTickerId);
}

public static class SourceTickerInfoExtensions
{
    public static SourceTickerInfo WithRoundingPrecision(this SourceTickerInfo toCopy, decimal roundingPrecision) =>
        new(toCopy) { RoundingPrecision = roundingPrecision };

    public static SourceTickerInfo WithTickerDetailLevel(this SourceTickerInfo toCopy, TickerQuoteDetailLevel tickerQuoteDetailLevel) =>
        new(toCopy) { PublishedTickerQuoteDetailLevel = tickerQuoteDetailLevel };

    public static SourceTickerInfo WithPip(this SourceTickerInfo toCopy, decimal pip) => new(toCopy) { Pip = pip };

    public static SourceTickerInfo WithDefaultMaxValidMs(this SourceTickerInfo toCopy, uint defaultMaxValidMs) =>
        new(toCopy) { DefaultMaxValidMs = defaultMaxValidMs };

    public static SourceTickerInfo WithSubscribeToPrices(this SourceTickerInfo toCopy, bool subscribeToPrice) =>
        new(toCopy) { SubscribeToPrices = subscribeToPrice };

    public static SourceTickerInfo WithTradingEnabled(this SourceTickerInfo toCopy, bool tradingEnabled) => new(toCopy) { TradingEnabled = tradingEnabled };

    public static SourceTickerInfo WithMaximumPublishedLayers(this SourceTickerInfo toCopy, ushort maxPublishedLayers) =>
        new(toCopy) { MaximumPublishedLayers = maxPublishedLayers };

    public static SourceTickerInfo WithMinSubmitSize(this SourceTickerInfo toCopy, decimal minSubmitSize) => new(toCopy) { MinSubmitSize = minSubmitSize };

    public static SourceTickerInfo WithMaxSubmitSize(this SourceTickerInfo toCopy, decimal maxSubmitSize) => new(toCopy) { MaxSubmitSize = maxSubmitSize };

    public static SourceTickerInfo WithIncrementSize(this SourceTickerInfo toCopy, decimal incrementSize) => new(toCopy) { IncrementSize = incrementSize };

    public static SourceTickerInfo WithMinimumQuoteLife(this SourceTickerInfo toCopy, ushort minQuoteLifeMs) =>
        new(toCopy) { MinimumQuoteLife = minQuoteLifeMs };

    public static SourceTickerInfo WithLayerFlags(this SourceTickerInfo toCopy, LayerFlags layerFlags) => new(toCopy) { LayerFlags = layerFlags };

    public static SourceTickerInfo WithLastTradedFlags(this SourceTickerInfo toCopy, LastTradedFlags lastTradedFlags) =>
        new(toCopy) { LastTradedFlags = lastTradedFlags };
}
