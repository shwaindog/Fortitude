// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

public interface IPQSourceTickerInfo : ISourceTickerInfo, IPQPricingInstrumentId, IPQPriceVolumePublicationPrecisionSettings
  , ICloneable<IPQSourceTickerInfo>, IPQSupportsFieldUpdates<IPQSourceTickerInfo>, IPQSupportsStringUpdates<IPQSourceTickerInfo>
{
    bool IsPublishedTickerDetailLevelUpdated { get; set; }
    bool IsRoundingPrecisionUpdated          { get; set; }
    bool IsPipUpdated                        { get; set; }
    bool IsDefaultMaxValidMsUpdated          { get; set; }
    bool IsMinSubmitSizeUpdated              { get; set; }
    bool IsMaxSubmitSizeUpdated              { get; set; }
    bool IsIncrementSizeUpdated              { get; set; }
    bool IsMinimumQuoteLifeUpdated           { get; set; }
    bool IsLayerFlagsUpdated                 { get; set; }
    bool IsMaximumPublishedLayersUpdated     { get; set; }
    bool IsLastTradedFlagsUpdated            { get; set; }
    bool IsSubscribeToPricesUpdated          { get; set; }
    bool IsTradingEnabledUpdated             { get; set; }

    new ushort SourceId   { get; set; }
    new ushort TickerId   { get; set; }
    new string Source     { get; set; }
    new string Ticker     { get; set; }
    new bool   HasUpdates { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags);

    new bool UpdateFieldString(PQFieldStringUpdate updates);
    new int  UpdateField(PQFieldUpdate fieldUpdate);

    new IPQSourceTickerInfo Clone();
}

public class PQSourceTickerInfo : PQPricingInstrument, IPQSourceTickerInfo
{
    private SourceTickerInfoBooleanFlags booleanFlags = SourceTickerInfoBooleanFlags.SubscribeToPricesSet;

    private uint    defaultMaxValidMs;
    private string? formatPrice;
    private decimal incrementSize;

    private LastTradedFlags lastTradedFlags;
    private LayerFlags      layerFlags;

    private byte    maximumPublishedLayers;
    private decimal maxSubmitSize;
    private ushort  minimumQuoteLife;
    private decimal minSubmitSize;
    private decimal pip;

    private TickerDetailLevel publishedTickerDetailLevel = TickerDetailLevel.Level2Quote;

    private decimal roundingPrecision;

    public PQSourceTickerInfo()
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

    public PQSourceTickerInfo
    (ushort sourceId, string source, ushort tickerId, string ticker, TickerDetailLevel publishedTickerDetailLevel
      , MarketClassification marketClassification, byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m
      , decimal pip = 0.0001m, decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m, decimal incrementSize = 0.01m
      , ushort minimumQuoteLife = 100, uint defaultMaxValidMs = 10_000, bool subscribeToPrices = true, bool tradingEnabled = false
      , LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
        : base(sourceId, tickerId, source, ticker, new DiscreetTimePeriod(TimeBoundaryPeriod.Tick), InstrumentType.Price, marketClassification)
    {
        PublishedTickerDetailLevel = publishedTickerDetailLevel;

        Pip = pip;

        DefaultMaxValidMs = defaultMaxValidMs;
        SubscribeToPrices = subscribeToPrices;
        TradingEnabled    = tradingEnabled;

        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision      = roundingPrecision;

        MinSubmitSize    = minSubmitSize;
        MaxSubmitSize    = maxSubmitSize;
        IncrementSize    = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags       = layerFlags;
        LastTradedFlags  = lastTradedFlags;

        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindPriceScaleFactor(Math.Min(MinSubmitSize, IncrementSize));
    }

    public PQSourceTickerInfo(ISourceTickerInfo toClone) : base(toClone)
    {
        PublishedTickerDetailLevel = toClone.PublishedTickerDetailLevel;

        Pip = toClone.Pip;

        DefaultMaxValidMs = toClone.DefaultMaxValidMs;
        SubscribeToPrices = toClone.SubscribeToPrices;
        TradingEnabled    = toClone.TradingEnabled;

        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision      = toClone.RoundingPrecision;

        MinSubmitSize    = toClone.MinSubmitSize;
        MaxSubmitSize    = toClone.MaxSubmitSize;
        IncrementSize    = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags       = toClone.LayerFlags;
        LastTradedFlags  = toClone.LastTradedFlags;


        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(MinSubmitSize, IncrementSize));

        if (toClone is IPQSourceTickerInfo pubToClone)
        {
            IsPublishedTickerDetailLevelUpdated = pubToClone.IsPublishedTickerDetailLevelUpdated;

            Pip = toClone.Pip;

            DefaultMaxValidMs = toClone.DefaultMaxValidMs;
            SubscribeToPrices = toClone.SubscribeToPrices;
            TradingEnabled    = toClone.TradingEnabled;

            IsMaximumPublishedLayersUpdated = pubToClone.IsMaximumPublishedLayersUpdated;
            IsRoundingPrecisionUpdated      = pubToClone.IsRoundingPrecisionUpdated;

            NameIdLookup              = pubToClone.NameIdLookup.Clone();
            IsIncrementSizeUpdated    = pubToClone.IsIncrementSizeUpdated;
            IsLastTradedFlagsUpdated  = pubToClone.IsLastTradedFlagsUpdated;
            IsMaxSubmitSizeUpdated    = pubToClone.IsMaxSubmitSizeUpdated;
            IsMinSubmitSizeUpdated    = pubToClone.IsMinSubmitSizeUpdated;
            IsMinimumQuoteLifeUpdated = pubToClone.IsMinimumQuoteLifeUpdated;
            IsLayerFlagsUpdated       = pubToClone.IsLayerFlagsUpdated;
        }
    }

    public bool IsPipUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.Pip) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.Pip;

            else if (IsPipUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.Pip;
        }
    }

    public byte PriceScalingPrecision  { get; } = 3;
    public byte VolumeScalingPrecision { get; } = 6;

    public uint MessageId => SourceTickerId;
    public byte Version   => 1;

    public TickerDetailLevel PublishedTickerDetailLevel
    {
        get => publishedTickerDetailLevel;
        set
        {
            if (publishedTickerDetailLevel == value) return;
            IsPublishedTickerDetailLevelUpdated = true;
            publishedTickerDetailLevel          = value;
        }
    }

    public decimal RoundingPrecision
    {
        get => roundingPrecision;
        set
        {
            if (roundingPrecision == value) return;
            IsRoundingPrecisionUpdated = true;
            formatPrice                = null;
            roundingPrecision          = value;
        }
    }

    public decimal Pip
    {
        get => pip;
        set
        {
            if (pip == value) return;
            IsPipUpdated = true;
            pip          = value;
        }
    }

    public decimal MinSubmitSize
    {
        get => minSubmitSize;
        set
        {
            if (minSubmitSize == value) return;

            IsMinSubmitSizeUpdated = true;
            minSubmitSize          = value;
        }
    }

    public decimal MaxSubmitSize
    {
        get => maxSubmitSize;
        set
        {
            if (maxSubmitSize == value) return;

            IsMaxSubmitSizeUpdated = true;
            maxSubmitSize          = value;
        }
    }

    public decimal IncrementSize
    {
        get => incrementSize;
        set
        {
            if (incrementSize == value) return;

            IsIncrementSizeUpdated = true;
            incrementSize          = value;
        }
    }

    public ushort MinimumQuoteLife
    {
        get => minimumQuoteLife;
        set
        {
            if (minimumQuoteLife == value) return;

            IsMinimumQuoteLifeUpdated = true;
            minimumQuoteLife          = value;
        }
    }

    public uint DefaultMaxValidMs
    {
        get => defaultMaxValidMs;
        set
        {
            if (defaultMaxValidMs == value) return;

            IsDefaultMaxValidMsUpdated = true;
            defaultMaxValidMs          = value;
        }
    }

    public bool SubscribeToPrices
    {
        get => (booleanFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesSet) > 0;
        set
        {
            if (SubscribeToPrices == value) return;

            IsSubscribeToPricesUpdated = true;

            if (value)
                booleanFlags |= SourceTickerInfoBooleanFlags.SubscribeToPricesSet;
            else
                booleanFlags &= ~SourceTickerInfoBooleanFlags.SubscribeToPricesSet;
        }
    }

    public bool TradingEnabled
    {
        get => (booleanFlags & SourceTickerInfoBooleanFlags.TradingEnabledSet) > 0;
        set
        {
            if (TradingEnabled == value) return;

            IsTradingEnabledUpdated = true;

            if (value)
                booleanFlags |= SourceTickerInfoBooleanFlags.TradingEnabledSet;
            else
                booleanFlags &= ~SourceTickerInfoBooleanFlags.TradingEnabledSet;
        }
    }

    public LayerFlags LayerFlags
    {
        get => layerFlags;
        set
        {
            if (layerFlags == value) return;

            IsLayerFlagsUpdated = true;
            layerFlags          = value;
        }
    }

    public byte MaximumPublishedLayers
    {
        get => maximumPublishedLayers;
        set
        {
            if (maximumPublishedLayers == value) return;

            IsMaximumPublishedLayersUpdated = true;
            maximumPublishedLayers          = value;
        }
    }

    public LastTradedFlags LastTradedFlags
    {
        get => lastTradedFlags;
        set
        {
            if (lastTradedFlags == value) return;
            IsLastTradedFlagsUpdated = true;
            lastTradedFlags          = value;
        }
    }

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


    public bool IsPublishedTickerDetailLevelUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.PublishedQuoteLevel) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.PublishedQuoteLevel;

            else if (IsPublishedTickerDetailLevelUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.PublishedQuoteLevel;
        }
    }

    public bool IsRoundingPrecisionUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.RoundingPrecision) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.RoundingPrecision;

            else if (IsRoundingPrecisionUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.RoundingPrecision;
        }
    }

    public bool IsMinSubmitSizeUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MinSubmitSize) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MinSubmitSize;

            else if (IsMinSubmitSizeUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MinSubmitSize;
        }
    }

    public bool IsMaxSubmitSizeUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MaxSubmitSize) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MaxSubmitSize;

            else if (IsMaxSubmitSizeUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MaxSubmitSize;
        }
    }

    public bool IsIncrementSizeUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.IncrementSize) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.IncrementSize;

            else if (IsIncrementSizeUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.IncrementSize;
        }
    }

    public bool IsMinimumQuoteLifeUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MinimumQuoteLife) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MinimumQuoteLife;

            else if (IsMinimumQuoteLifeUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MinimumQuoteLife;
        }
    }

    public bool IsDefaultMaxValidMsUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.DefaultMaxValidMs) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.DefaultMaxValidMs;

            else if (IsDefaultMaxValidMsUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.DefaultMaxValidMs;
        }
    }

    public bool IsSubscribeToPricesUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SubscribeToPrices) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SubscribeToPrices;

            else if (IsSubscribeToPricesUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SubscribeToPrices;
        }
    }

    public bool IsTradingEnabledUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.TradingEnabled) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.TradingEnabled;

            else if (IsTradingEnabledUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.TradingEnabled;
        }
    }

    public bool IsLayerFlagsUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.LayerFlags) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.LayerFlags;

            else if (IsLayerFlagsUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.LayerFlags;
        }
    }

    public bool IsMaximumPublishedLayersUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MaximumPublishedLayers) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MaximumPublishedLayers;

            else if (IsMaximumPublishedLayersUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MaximumPublishedLayers;
        }
    }

    public bool IsLastTradedFlagsUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.LastTradedFlags) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.LastTradedFlags;

            else if (IsLastTradedFlagsUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.LastTradedFlags;
        }
    }

    public override bool AreEquivalent(ISourceTickerInfo? other, bool exactTypes = false)
    {
        if (other == null || (exactTypes && other is not IPQSourceTickerInfo)) return false;

        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var tickerDetailLevelSame = PublishedTickerDetailLevel == other.PublishedTickerDetailLevel;
        var maxPublishLayersSame  = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var roundingPrecisionSame = RoundingPrecision == other?.RoundingPrecision;

        var pipSame = Pip == other?.Pip;

        var minSubmitSizeSame   = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame   = MaxSubmitSize == other?.MaxSubmitSize;
        var incrementSizeSame   = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame    = MinimumQuoteLife == other?.MinimumQuoteLife;
        var defaultMaxValidSame = DefaultMaxValidMs == other?.DefaultMaxValidMs;
        var subscribeSame       = SubscribeToPrices == other?.SubscribeToPrices;
        var tradingEnabledSame  = TradingEnabled == other?.TradingEnabled;

        var layerFlagsSame      = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        var updatesSame = true;
        if (exactTypes)
        {
            var pqUniqSrcTrkId = other as PQSourceTickerInfo;
            updatesSame = UpdatedFlags == pqUniqSrcTrkId?.UpdatedFlags;
        }

        var allAreSame = baseIsSame && tickerDetailLevelSame && roundingPrecisionSame && pipSame && minSubmitSizeSame && maxSubmitSizeSame
                      && incrementSizeSame && minQuoteLifeSame && layerFlagsSame && maxPublishLayersSame && defaultMaxValidSame && subscribeSame
                      && tradingEnabledSame && lastTradedFlagsSame && updatesSame;
        return allAreSame;
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPQSourceTickerInfo ICloneable<IPQSourceTickerInfo>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public IReusableObject<IVersionedMessage> CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    public IVersionedMessage CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ISourceTickerInfo)source, copyMergeFlags);

    ISourceTickerInfo ISourceTickerInfo.Clone() => Clone();

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle, quotePublicationPrecisionSettings))
            yield return deltaUpdateField;
        var updatedOnly = (updateStyle & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsPublishedTickerDetailLevelUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.TickerDetailLevelType, (byte)PublishedTickerDetailLevel);
        if (!updatedOnly || IsRoundingPrecisionUpdated)
        {
            var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(RoundingPrecision)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * RoundingPrecision);
            yield return new PQFieldUpdate(PQFieldKeys.PriceRoundingPrecision, roundingNoDecimal, decimalPlaces);
        }
        if (!updatedOnly || IsPipUpdated)
        {
            var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(Pip)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * Pip);
            yield return new PQFieldUpdate(PQFieldKeys.Pip, roundingNoDecimal, decimalPlaces);
        }
        if (!updatedOnly || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(!updatedOnly);
            yield return new PQFieldUpdate(PQFieldKeys.TickerDetailBooleanFlags, (uint)booleanFields);
        }
        if (!updatedOnly || IsMaximumPublishedLayersUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.MaximumPublishedLayers, MaximumPublishedLayers);

        if (!updatedOnly || IsMinSubmitSizeUpdated)
        {
            var decimalPlaces      = BitConverter.GetBytes(decimal.GetBits(MinSubmitSize)[3])[2];
            var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MinSubmitSize);
            yield return new PQFieldUpdate(PQFieldKeys.MinSubmitSize, minSubmitNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsMaxSubmitSizeUpdated)
        {
            var decimalPlaces      = BitConverter.GetBytes(decimal.GetBits(MaxSubmitSize)[3])[2];
            var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MaxSubmitSize);
            yield return new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, maxSubmitNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsIncrementSizeUpdated)
        {
            var decimalPlaces          = BitConverter.GetBytes(decimal.GetBits(IncrementSize)[3])[2];
            var incrementSizeNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * IncrementSize);
            yield return new PQFieldUpdate(PQFieldKeys.IncrementSize, incrementSizeNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsDefaultMaxValidMsUpdated) yield return new PQFieldUpdate(PQFieldKeys.DefaultMaxValidMs, DefaultMaxValidMs);
        if (!updatedOnly || IsMinimumQuoteLifeUpdated) yield return new PQFieldUpdate(PQFieldKeys.MinimumQuoteLifeMs, MinimumQuoteLife);
        if (!updatedOnly || IsLayerFlagsUpdated) yield return new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)LayerFlags);
        if (!updatedOnly || IsLastTradedFlagsUpdated) yield return new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)LastTradedFlags);
    }

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.TickerDetailLevelType:
                PublishedTickerDetailLevel = (TickerDetailLevel)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.PriceRoundingPrecision:
                var decimalPlaces              = fieldUpdate.Flag;
                var convertedRoundingPrecision = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                RoundingPrecision = convertedRoundingPrecision;
                return 0;
            case PQFieldKeys.Pip:
                decimalPlaces = fieldUpdate.Flag;
                var convertedPip = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                Pip = convertedPip;
                return 0;
            case PQFieldKeys.TickerDetailBooleanFlags:
                SetBooleanFields((SourceTickerInfoBooleanFlags)fieldUpdate.Value);
                return 0;
            case PQFieldKeys.MaximumPublishedLayers:
                MaximumPublishedLayers = (byte)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.MinSubmitSize:
                decimalPlaces = fieldUpdate.Flag;
                var convertedMinSubmitSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                MinSubmitSize = convertedMinSubmitSize;
                return 0;
            case PQFieldKeys.MaxSubmitSize:
                decimalPlaces = fieldUpdate.Flag;
                var convertedMaxSubmitSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                MaxSubmitSize = convertedMaxSubmitSize;
                return 0;
            case PQFieldKeys.IncrementSize:
                decimalPlaces = fieldUpdate.Flag;
                var convertedIncrementSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                IncrementSize = convertedIncrementSize;
                return 0;
            case PQFieldKeys.DefaultMaxValidMs:
                DefaultMaxValidMs = fieldUpdate.Value;
                return 0;
            case PQFieldKeys.MinimumQuoteLifeMs:
                MinimumQuoteLife = (ushort)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.LayerFlags:
                LayerFlags = (LayerFlags)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.LastTradedFlags:
                LastTradedFlags = (LastTradedFlags)fieldUpdate.Value;
                return 0;
        }

        return base.UpdateField(fieldUpdate);
    }

    public override IPQSourceTickerId CopyFrom(IPQSourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQSourceTickerInfo pqSrcTickerInfo) return CopyFrom(pqSrcTickerInfo, copyMergeFlags);
        if (source is ISourceTickerInfo srcTickerInfo) return (IPQSourceTickerId)CopyFrom(srcTickerInfo, copyMergeFlags);
        return base.CopyFrom(source, copyMergeFlags);
    }

    public ISourceTickerInfo CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ((IPQPricingInstrumentId)this).CopyFrom(source, copyMergeFlags);
        if (source is PQSourceTickerInfo pqSrcTkrInfo && copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            var hasFullReplace = copyMergeFlags.HasFullReplace();

            if (pqSrcTkrInfo.IsPublishedTickerDetailLevelUpdated || hasFullReplace)
                PublishedTickerDetailLevel = pqSrcTkrInfo.PublishedTickerDetailLevel;
            if (pqSrcTkrInfo.IsMaximumPublishedLayersUpdated || hasFullReplace) MaximumPublishedLayers = pqSrcTkrInfo.MaximumPublishedLayers;

            if (pqSrcTkrInfo.IsPipUpdated || hasFullReplace) Pip = pqSrcTkrInfo.Pip;

            if (pqSrcTkrInfo.IsDefaultMaxValidMsUpdated || hasFullReplace) DefaultMaxValidMs = pqSrcTkrInfo.DefaultMaxValidMs;
            if (pqSrcTkrInfo.IsSubscribeToPricesUpdated || hasFullReplace) SubscribeToPrices = pqSrcTkrInfo.SubscribeToPrices;
            if (pqSrcTkrInfo.IsRoundingPrecisionUpdated || hasFullReplace) RoundingPrecision = pqSrcTkrInfo.RoundingPrecision;
            if (pqSrcTkrInfo.IsMinimumQuoteLifeUpdated || hasFullReplace) MinimumQuoteLife   = pqSrcTkrInfo.MinimumQuoteLife;

            if (pqSrcTkrInfo.IsTradingEnabledUpdated || hasFullReplace) TradingEnabled   = pqSrcTkrInfo.TradingEnabled;
            if (pqSrcTkrInfo.IsMinSubmitSizeUpdated || hasFullReplace) MinSubmitSize     = pqSrcTkrInfo.MinSubmitSize;
            if (pqSrcTkrInfo.IsMaxSubmitSizeUpdated || hasFullReplace) MaxSubmitSize     = pqSrcTkrInfo.MaxSubmitSize;
            if (pqSrcTkrInfo.IsIncrementSizeUpdated || hasFullReplace) IncrementSize     = pqSrcTkrInfo.IncrementSize;
            if (pqSrcTkrInfo.IsLayerFlagsUpdated || hasFullReplace) LayerFlags           = pqSrcTkrInfo.LayerFlags;
            if (pqSrcTkrInfo.IsLastTradedFlagsUpdated || hasFullReplace) LastTradedFlags = pqSrcTkrInfo.LastTradedFlags;
        }
        else
        {
            PublishedTickerDetailLevel = source.PublishedTickerDetailLevel;
            MaximumPublishedLayers     = source.MaximumPublishedLayers;

            Pip = source.Pip;

            RoundingPrecision = source.RoundingPrecision;
            SubscribeToPrices = source.SubscribeToPrices;
            DefaultMaxValidMs = source.DefaultMaxValidMs;
            MinimumQuoteLife  = source.MinimumQuoteLife;

            TradingEnabled  = source.TradingEnabled;
            MinSubmitSize   = source.MinSubmitSize;
            MaxSubmitSize   = source.MaxSubmitSize;
            IncrementSize   = source.IncrementSize;
            LayerFlags      = source.LayerFlags;
            LastTradedFlags = source.LastTradedFlags;
        }

        return this;
    }

    IPricingInstrumentId ICloneable<IPricingInstrumentId>.Clone() => Clone();

    IPQSourceTickerInfo IPQSourceTickerInfo.Clone() => Clone();

    public IPQSourceTickerInfo CopyFrom(IPQSourceTickerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var hasFullReplace = copyMergeFlags.HasFullReplace();
        ((IPricingInstrumentId)this).CopyFrom(source, copyMergeFlags);

        if (source.IsPublishedTickerDetailLevelUpdated || hasFullReplace) PublishedTickerDetailLevel = source.PublishedTickerDetailLevel;
        if (source.IsMaximumPublishedLayersUpdated || hasFullReplace) MaximumPublishedLayers         = source.MaximumPublishedLayers;

        if (source.IsPipUpdated) Pip = source.Pip;

        if (source.IsDefaultMaxValidMsUpdated || hasFullReplace) DefaultMaxValidMs = source.DefaultMaxValidMs;
        if (source.IsSubscribeToPricesUpdated || hasFullReplace) SubscribeToPrices = source.SubscribeToPrices;
        if (source.IsRoundingPrecisionUpdated || hasFullReplace) RoundingPrecision = source.RoundingPrecision;
        if (source.IsMinimumQuoteLifeUpdated || hasFullReplace) MinimumQuoteLife   = source.MinimumQuoteLife;

        if (source.IsTradingEnabledUpdated || hasFullReplace) TradingEnabled   = source.TradingEnabled;
        if (source.IsMinSubmitSizeUpdated || hasFullReplace) MinSubmitSize     = source.MinSubmitSize;
        if (source.IsMaxSubmitSizeUpdated || hasFullReplace) MaxSubmitSize     = source.MaxSubmitSize;
        if (source.IsIncrementSizeUpdated || hasFullReplace) IncrementSize     = source.IncrementSize;
        if (source.IsLayerFlagsUpdated || hasFullReplace) LayerFlags           = source.LayerFlags;
        if (source.IsLastTradedFlagsUpdated || hasFullReplace) LastTradedFlags = source.LastTradedFlags;
        return this;
    }

    protected virtual bool IsBooleanFlagsChanged() => IsTradingEnabledUpdated || IsSubscribeToPricesUpdated;

    protected virtual SourceTickerInfoBooleanFlags GenerateBooleanFlags(bool fullUpdate) =>
        (IsSubscribeToPricesUpdated || fullUpdate ? SourceTickerInfoBooleanFlags.SubscribeToPricesUpdated : 0)
      | (SubscribeToPrices ? SourceTickerInfoBooleanFlags.SubscribeToPricesSet : 0)
      | (IsTradingEnabledUpdated || fullUpdate ? SourceTickerInfoBooleanFlags.TradingEnabledUpdated : 0)
      | (TradingEnabled ? SourceTickerInfoBooleanFlags.TradingEnabledSet : 0);

    protected virtual void SetBooleanFields(SourceTickerInfoBooleanFlags booleanFlags)
    {
        IsSubscribeToPricesUpdated = (booleanFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesUpdated) > 0;
        if (IsSubscribeToPricesUpdated)
            SubscribeToPrices = (booleanFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesSet) ==
                                SourceTickerInfoBooleanFlags.SubscribeToPricesSet;
        IsTradingEnabledUpdated = (booleanFlags & SourceTickerInfoBooleanFlags.TradingEnabledUpdated) > 0;
        if (IsTradingEnabledUpdated)
            TradingEnabled = (booleanFlags & SourceTickerInfoBooleanFlags.TradingEnabledSet) == SourceTickerInfoBooleanFlags.TradingEnabledSet;
    }

    public override PQSourceTickerInfo Clone() =>
        Recycler?.Borrow<PQSourceTickerInfo>().CopyFrom(this) as PQSourceTickerInfo ?? new PQSourceTickerInfo(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourceTickerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ TickerId;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQSourceTickerInfo)}({nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, {nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker},  " +
        $"{nameof(PublishedTickerDetailLevel)}: {PublishedTickerDetailLevel},  {nameof(MarketClassification)}: {MarketClassification}, " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, {nameof(MinSubmitSize)}: {MinSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, {nameof(SubscribeToPrices)}: {SubscribeToPrices}, {nameof(TradingEnabled)}: {TradingEnabled}, " +
        $"{nameof(LayerFlags)}: {LayerFlags:F}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags})";
}
