// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

public interface IPQSourceTickerInfo : ISourceTickerInfo, IPQPricingInstrumentId, IPQPriceVolumePublicationPrecisionSettings
  , ICloneable<IPQSourceTickerInfo>
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

    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags);

    new IPQSourceTickerInfo CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new bool UpdateFieldString(PQFieldStringUpdate updates);
    new int  UpdateField(PQFieldUpdate fieldUpdate);

    new IPQSourceTickerInfo Clone();
}

public class PQSourceTickerInfo : PQPricingInstrumentId, IPQSourceTickerInfo
{
    private SourceTickerInfoBooleanFlags booleanFlags = SourceTickerInfoBooleanFlags.SubscribeToPricesSet;

    private uint    defaultMaxValidMs = SourceTickerInfo.DefaultDefaultMaxValidMs;
    private string? formatPrice;
    private decimal incrementSize = SourceTickerInfo.DefaultIncrementSize;

    private LastTradedFlags lastTradedFlags = SourceTickerInfo.DefaultLastTradedFlags;
    private LayerFlags      layerFlags      = SourceTickerInfo.DefaultLayerFlags;

    private ushort  maximumPublishedLayers = SourceTickerInfo.DefaultMaximumPublishedLayers;
    private decimal maxSubmitSize          = SourceTickerInfo.DefaultMaxSubmitSize;
    private ushort  minimumQuoteLife       = SourceTickerInfo.DefaultMinimumQuoteLife;
    private decimal minSubmitSize          = SourceTickerInfo.DefaultMinSubmitSize;
    private decimal pip                    = SourceTickerInfo.DefaultPip;

    private TickerQuoteDetailLevel publishedTickerQuoteDetailLevel = SourceTickerInfo.DefaultQuoteLevel;

    private decimal roundingPrecision = SourceTickerInfo.DefaultRoundingPrecision;

    public PQSourceTickerInfo()
    {
        if (GetType() == typeof(PQSourceTickerInfo)) SequenceId = 0;
    }

    public PQSourceTickerInfo
    (ushort sourceId, string sourceName, ushort tickerId, string ticker, TickerQuoteDetailLevel publishedTickerQuoteDetailLevel = SourceTickerInfo.DefaultQuoteLevel
      , MarketClassification marketClassification = default 
      , CountryCityCodes sourcePublishLocation = CountryCityCodes.Unknown  
      , CountryCityCodes adapterReceiveLocation = CountryCityCodes.Unknown  
      , CountryCityCodes clientReceiveLocation = CountryCityCodes.Unknown  
      , ushort maximumPublishedLayers = SourceTickerInfo.DefaultMaximumPublishedLayers
      , decimal roundingPrecision = SourceTickerInfo.DefaultRoundingPrecision
      , decimal pip = SourceTickerInfo.DefaultPip, decimal minSubmitSize = SourceTickerInfo.DefaultMinSubmitSize
      , decimal maxSubmitSize = SourceTickerInfo.DefaultMaxSubmitSize, decimal incrementSize = SourceTickerInfo.DefaultIncrementSize
      , ushort minimumQuoteLife = SourceTickerInfo.DefaultMinimumQuoteLife, uint defaultMaxValidMs = SourceTickerInfo.DefaultDefaultMaxValidMs
      , bool subscribeToPrices = SourceTickerInfo.DefaultSubscribeToPrices, bool tradingEnabled = SourceTickerInfo.DefaultTradingEnabled
      , LayerFlags layerFlags = SourceTickerInfo.DefaultLayerFlags, LastTradedFlags lastTradedFlags = SourceTickerInfo.DefaultLastTradedFlags
      , PublishableQuoteInstantBehaviorFlags quoteBehaviorFlags = SourceTickerInfo.DefaultQuoteBehaviorFlags )
        : base(sourceId, tickerId, sourceName, ticker, new DiscreetTimePeriod(TimeBoundaryPeriod.Tick), InstrumentType.Price, marketClassification, null,
               sourcePublishLocation, adapterReceiveLocation, clientReceiveLocation)
    {
        PublishedTickerQuoteDetailLevel     = publishedTickerQuoteDetailLevel;

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

        QuoteBehaviorFlags = quoteBehaviorFlags;

        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindPriceScaleFactor(Math.Min(MinSubmitSize, IncrementSize));

        if (GetType() == typeof(PQSourceTickerInfo)) SequenceId = 0;
    }

    public PQSourceTickerInfo(ISourceTickerInfo toClone) : base(toClone)
    {
        PublishedTickerQuoteDetailLevel = toClone.PublishedTickerQuoteDetailLevel;

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

        QuoteBehaviorFlags = toClone.QuoteBehaviorFlags;

        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(MinSubmitSize, IncrementSize));

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQSourceTickerInfo)) SequenceId = 0;
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

    public PQFieldFlags PriceScalingPrecision  { get; private set; } = (PQFieldFlags)3;
    public PQFieldFlags VolumeScalingPrecision { get; private set; } = (PQFieldFlags)6;

    public uint MessageId => SourceInstrumentId;
    public byte Version   => 1;

    public TickerQuoteDetailLevel PublishedTickerQuoteDetailLevel
    {
        get => publishedTickerQuoteDetailLevel;
        set
        {
            IsPublishedTickerDetailLevelUpdated |= publishedTickerQuoteDetailLevel != value || SequenceId == 0;
            publishedTickerQuoteDetailLevel     =  value;
        }
    }

    public decimal RoundingPrecision
    {
        get => roundingPrecision;
        set
        {
            IsRoundingPrecisionUpdated |= roundingPrecision != value || SequenceId == 0;
            formatPrice                =  null;
            roundingPrecision          =  value;
        }
    }

    public decimal Pip
    {
        get => pip;
        set
        {
            IsPipUpdated |= pip != value || SequenceId == 0;
            pip          =  value;
        }
    }

    public decimal MinSubmitSize
    {
        get => minSubmitSize;
        set
        {
            IsMinSubmitSizeUpdated |= minSubmitSize != value || SequenceId == 0;
            minSubmitSize          =  value;
        }
    }

    public decimal MaxSubmitSize
    {
        get => maxSubmitSize;
        set
        {
            IsMaxSubmitSizeUpdated |= maxSubmitSize != value || SequenceId == 0;
            maxSubmitSize          =  value;
        }
    }

    public decimal IncrementSize
    {
        get => incrementSize;
        set
        {
            IsIncrementSizeUpdated |= incrementSize != value || SequenceId == 0;
            incrementSize          =  value;
        }
    }

    public ushort MinimumQuoteLife
    {
        get => minimumQuoteLife;
        set
        {
            IsMinimumQuoteLifeUpdated |= minimumQuoteLife != value || SequenceId == 0;
            minimumQuoteLife          =  value;
        }
    }

    public uint DefaultMaxValidMs
    {
        get => defaultMaxValidMs;
        set
        {
            IsDefaultMaxValidMsUpdated |= defaultMaxValidMs != value || SequenceId == 0;
            defaultMaxValidMs          =  value;
        }
    }

    public bool SubscribeToPrices
    {
        get => (booleanFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesSet) > 0;
        set
        {
            IsSubscribeToPricesUpdated |= SubscribeToPrices != value || SequenceId == 0;

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
            IsTradingEnabledUpdated |= TradingEnabled || SequenceId == 0;

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
            IsLayerFlagsUpdated |= layerFlags != value || SequenceId == 0;
            layerFlags          =  value;
        }
    }

    public ushort MaximumPublishedLayers
    {
        get => maximumPublishedLayers;
        set
        {
            IsMaximumPublishedLayersUpdated |= maximumPublishedLayers != value || SequenceId == 0;
            maximumPublishedLayers          =  value;
        }
    }

    public LastTradedFlags LastTradedFlags
    {
        get => lastTradedFlags;
        set
        {
            IsLastTradedFlagsUpdated |= lastTradedFlags != value || SequenceId == 0;
            lastTradedFlags          =  value;
        }
    }

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

    protected virtual bool IsBooleanFlagsChanged() => IsTradingEnabledUpdated || IsSubscribeToPricesUpdated;

    protected virtual SourceTickerInfoBooleanFlags GenerateBooleanFlags(bool fullUpdate) =>
        (IsSubscribeToPricesUpdated || fullUpdate ? SourceTickerInfoBooleanFlags.SubscribeToPricesUpdated : 0)
      | (SubscribeToPrices ? SourceTickerInfoBooleanFlags.SubscribeToPricesSet : 0)
      | (IsTradingEnabledUpdated || fullUpdate ? SourceTickerInfoBooleanFlags.TradingEnabledUpdated : 0)
      | (TradingEnabled ? SourceTickerInfoBooleanFlags.TradingEnabledSet : 0);

    protected virtual void SetBooleanFields(SourceTickerInfoBooleanFlags boolFlags)
    {
        IsSubscribeToPricesUpdated = (boolFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesUpdated) > 0;
        if (IsSubscribeToPricesUpdated)
            SubscribeToPrices = (boolFlags & SourceTickerInfoBooleanFlags.SubscribeToPricesSet) ==
                                SourceTickerInfoBooleanFlags.SubscribeToPricesSet;
        IsTradingEnabledUpdated = (boolFlags & SourceTickerInfoBooleanFlags.TradingEnabledUpdated) > 0;
        if (IsTradingEnabledUpdated)
            TradingEnabled = (boolFlags & SourceTickerInfoBooleanFlags.TradingEnabledSet) == SourceTickerInfoBooleanFlags.TradingEnabledSet;
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPQSourceTickerInfo ICloneable<IPQSourceTickerInfo>.Clone() => Clone();

    IPQSourceTickerInfo IPQSourceTickerInfo.Clone() => Clone();

    ISourceTickerInfo ISourceTickerInfo.Clone() => Clone();

    public override PQSourceTickerInfo Clone() =>
        Recycler?.Borrow<PQSourceTickerInfo>().CopyFrom(this) as PQSourceTickerInfo ?? new PQSourceTickerInfo(this);

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle, quotePublicationPrecisionSettings))
            yield return deltaUpdateField;
        var updatedOnly = (updateStyle & PQMessageFlags.Complete) == 0;

        if (!updatedOnly || IsPublishedTickerDetailLevelUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.TickerDetailLevelType, (byte)PublishedTickerQuoteDetailLevel);
        if (!updatedOnly || IsRoundingPrecisionUpdated)
        {
            var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(RoundingPrecision)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * RoundingPrecision);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.PriceRoundingPrecision, roundingNoDecimal, (PQFieldFlags)decimalPlaces);
        }
        if (!updatedOnly || IsPipUpdated)
        {
            var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(Pip)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * Pip);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.Pip, roundingNoDecimal, (PQFieldFlags)decimalPlaces);
        }
        if (!updatedOnly || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(!updatedOnly);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.TickerDetailBooleanFlags, (uint)booleanFields);
        }
        if (!updatedOnly || IsMaximumPublishedLayersUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MaximumPublishedLayers, MaximumPublishedLayers);

        if (!updatedOnly || IsMinSubmitSizeUpdated)
        {
            var decimalPlaces      = BitConverter.GetBytes(decimal.GetBits(MinSubmitSize)[3])[2];
            var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MinSubmitSize);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MinSubmitSize, minSubmitNoDecimal, (PQFieldFlags)decimalPlaces);
        }

        if (!updatedOnly || IsMaxSubmitSizeUpdated)
        {
            var decimalPlaces      = BitConverter.GetBytes(decimal.GetBits(MaxSubmitSize)[3])[2];
            var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MaxSubmitSize);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MaxSubmitSize, maxSubmitNoDecimal, (PQFieldFlags)decimalPlaces);
        }

        if (!updatedOnly || IsIncrementSizeUpdated)
        {
            var decimalPlaces          = BitConverter.GetBytes(decimal.GetBits(IncrementSize)[3])[2];
            var incrementSizeNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * IncrementSize);
            yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.IncrementSize, incrementSizeNoDecimal, (PQFieldFlags)decimalPlaces);
        }

        if (!updatedOnly || IsDefaultMaxValidMsUpdated) yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.DefaultMaxValidMs, DefaultMaxValidMs);
        if (!updatedOnly || IsMinimumQuoteLifeUpdated) yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.MinimumQuoteLifeMs, MinimumQuoteLife);
        if (!updatedOnly || IsLayerFlagsUpdated) yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.QuoteLayerFlags, (uint)LayerFlags);
        if (!updatedOnly || IsLastTradedFlagsUpdated) yield return new PQFieldUpdate(PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.LastTradedFlags, (uint)LastTradedFlags);
    }

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        if (fieldUpdate.Id == PQFeedFields.SourceTickerDefinition)
        {
            switch (fieldUpdate.DefinitionSubId)
            {
                case PQTickerDefSubFieldKeys.TickerDetailLevelType:
                    IsPublishedTickerDetailLevelUpdated = true;
                    PublishedTickerQuoteDetailLevel     = (TickerQuoteDetailLevel)fieldUpdate.Payload;
                    return 0;
                case PQTickerDefSubFieldKeys.PriceRoundingPrecision:
                    IsRoundingPrecisionUpdated = true;
                    var decimalPlaces              = (byte)(fieldUpdate.Flag & PQFieldFlags.DecimalScaleBits);
                    var convertedRoundingPrecision = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Payload;
                    RoundingPrecision = convertedRoundingPrecision;
                    return 0;
                case PQTickerDefSubFieldKeys.Pip:
                    IsPipUpdated  = true;
                    decimalPlaces = (byte)(fieldUpdate.Flag & PQFieldFlags.DecimalScaleBits);
                    var convertedPip = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Payload;
                    Pip = convertedPip;
                    return 0;
                case PQTickerDefSubFieldKeys.TickerDetailBooleanFlags:
                    SetBooleanFields((SourceTickerInfoBooleanFlags)fieldUpdate.Payload);
                    return 0;
                case PQTickerDefSubFieldKeys.MaximumPublishedLayers:
                    IsMaximumPublishedLayersUpdated = true;
                    MaximumPublishedLayers          = (byte)fieldUpdate.Payload;
                    return 0;
                case PQTickerDefSubFieldKeys.MinSubmitSize:
                    IsMinSubmitSizeUpdated = true;
                    decimalPlaces          = (byte)(fieldUpdate.Flag & PQFieldFlags.DecimalScaleBits);
                    var convertedMinSubmitSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Payload;
                    MinSubmitSize = convertedMinSubmitSize;
                    return 0;
                case PQTickerDefSubFieldKeys.MaxSubmitSize:
                    IsMaxSubmitSizeUpdated = true;

                    decimalPlaces = (byte)(fieldUpdate.Flag & PQFieldFlags.DecimalScaleBits);
                    var convertedMaxSubmitSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Payload;
                    MaxSubmitSize = convertedMaxSubmitSize;
                    return 0;
                case PQTickerDefSubFieldKeys.IncrementSize:
                    IsIncrementSizeUpdated = true;

                    decimalPlaces = (byte)(fieldUpdate.Flag & PQFieldFlags.DecimalScaleBits);
                    var convertedIncrementSize = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Payload;
                    IncrementSize = convertedIncrementSize;
                    return 0;
                case PQTickerDefSubFieldKeys.DefaultMaxValidMs:
                    IsDefaultMaxValidMsUpdated = true;

                    DefaultMaxValidMs = fieldUpdate.Payload;
                    return 0;
                case PQTickerDefSubFieldKeys.MinimumQuoteLifeMs:
                    IsMinimumQuoteLifeUpdated = true;

                    MinimumQuoteLife = (ushort)fieldUpdate.Payload;
                    return 0;
                case PQTickerDefSubFieldKeys.QuoteLayerFlags:
                    IsLayerFlagsUpdated = true;

                    LayerFlags = (LayerFlags)fieldUpdate.Payload;
                    return 0;
                case PQTickerDefSubFieldKeys.LastTradedFlags:
                    IsLastTradedFlagsUpdated = true;

                    LastTradedFlags = (LastTradedFlags)fieldUpdate.Payload;
                    return 0;
            }
        }

        return base.UpdateField(fieldUpdate);
    }
    
    public override bool IsEmpty
    {
        get =>
            base.IsEmpty
         && PublishedTickerQuoteDetailLevel == SourceTickerInfo.DefaultQuoteLevel
         && Pip == SourceTickerInfo.DefaultPip
         && RoundingPrecision == SourceTickerInfo.DefaultRoundingPrecision
         && DefaultMaxValidMs == SourceTickerInfo.DefaultDefaultMaxValidMs
         && SubscribeToPrices == SourceTickerInfo.DefaultSubscribeToPrices
         && TradingEnabled == SourceTickerInfo.DefaultTradingEnabled
         && MaximumPublishedLayers == SourceTickerInfo.DefaultMaximumPublishedLayers
         && MinSubmitSize == SourceTickerInfo.DefaultMinSubmitSize
         && MaxSubmitSize == SourceTickerInfo.DefaultMaxSubmitSize
         && IncrementSize == SourceTickerInfo.DefaultIncrementSize
         && MinimumQuoteLife == SourceTickerInfo.DefaultMinimumQuoteLife
         && LayerFlags == SourceTickerInfo.DefaultLayerFlags
         && LastTradedFlags == SourceTickerInfo.DefaultLastTradedFlags
         && QuoteBehaviorFlags == SourceTickerInfo.DefaultQuoteBehaviorFlags;
        set => base.IsEmpty = value;
    }

    public override PQSourceTickerId ResetWithTracking()
    {
        PublishedTickerQuoteDetailLevel = SourceTickerInfo.DefaultQuoteLevel;
        
        Pip = SourceTickerInfo.DefaultPip;

        RoundingPrecision      = SourceTickerInfo.DefaultRoundingPrecision;
        DefaultMaxValidMs      = SourceTickerInfo.DefaultDefaultMaxValidMs;
        SubscribeToPrices      = SourceTickerInfo.DefaultSubscribeToPrices;
        TradingEnabled         = SourceTickerInfo.DefaultTradingEnabled;
        MaximumPublishedLayers = SourceTickerInfo.DefaultMaximumPublishedLayers;
        MinSubmitSize          = SourceTickerInfo.DefaultMinSubmitSize;
        MaxSubmitSize          = SourceTickerInfo.DefaultMaxSubmitSize;
        IncrementSize          = SourceTickerInfo.DefaultIncrementSize;
        MinimumQuoteLife       = SourceTickerInfo.DefaultMinimumQuoteLife;
        LayerFlags             = SourceTickerInfo.DefaultLayerFlags;
        LastTradedFlags        = SourceTickerInfo.DefaultLastTradedFlags;
        QuoteBehaviorFlags     = SourceTickerInfo.DefaultQuoteBehaviorFlags;

        PriceScalingPrecision  = PQFieldFlags.None;
        VolumeScalingPrecision = PQFieldFlags.None;
        base.ResetWithTracking();

        return this;
    }

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerId)source, copyMergeFlags);

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerId)source, copyMergeFlags);

    ISourceTickerInfo ISourceTickerInfo.CopyFrom(ISourceTickerInfo source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IPQSourceTickerInfo IPQSourceTickerInfo.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags) => 
       CopyFrom(source, copyMergeFlags);

    public override PQSourceTickerInfo CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQSourceTickerInfo pqSrcTickerInfo)
        {
            var hasFullReplace = copyMergeFlags.HasFullReplace();

            if (pqSrcTickerInfo.IsPublishedTickerDetailLevelUpdated || hasFullReplace)
            {
                IsPublishedTickerDetailLevelUpdated = true;

                PublishedTickerQuoteDetailLevel     = pqSrcTickerInfo.PublishedTickerQuoteDetailLevel;
            }
            if (pqSrcTickerInfo.IsMaximumPublishedLayersUpdated || hasFullReplace)
            {
                IsMaximumPublishedLayersUpdated = true;

                MaximumPublishedLayers          = pqSrcTickerInfo.MaximumPublishedLayers;
            }

            if (pqSrcTickerInfo.IsPipUpdated || hasFullReplace)
            {
                IsPipUpdated = true;

                Pip = pqSrcTickerInfo.Pip;
            }

            if (pqSrcTickerInfo.IsDefaultMaxValidMsUpdated || hasFullReplace)
            {
                IsDefaultMaxValidMsUpdated = true;
                
                DefaultMaxValidMs = pqSrcTickerInfo.DefaultMaxValidMs;
            }
            if (pqSrcTickerInfo.IsSubscribeToPricesUpdated || hasFullReplace)
            {
                IsSubscribeToPricesUpdated = true;

                SubscribeToPrices = pqSrcTickerInfo.SubscribeToPrices;
            }

            if (pqSrcTickerInfo.IsRoundingPrecisionUpdated || hasFullReplace)
            {
                IsRoundingPrecisionUpdated = true;

                RoundingPrecision = pqSrcTickerInfo.RoundingPrecision;
            }
            if (pqSrcTickerInfo.IsMinimumQuoteLifeUpdated || hasFullReplace)
            {
                IsMinimumQuoteLifeUpdated = true;

                MinimumQuoteLife   = pqSrcTickerInfo.MinimumQuoteLife;
            }

            if (pqSrcTickerInfo.IsTradingEnabledUpdated || hasFullReplace)
            {
                IsTradingEnabledUpdated = true;

                TradingEnabled   = pqSrcTickerInfo.TradingEnabled;
            }
            if (pqSrcTickerInfo.IsMinSubmitSizeUpdated || hasFullReplace)
            {
                IsMinSubmitSizeUpdated = true;

                MinSubmitSize     = pqSrcTickerInfo.MinSubmitSize;
            }
            if (pqSrcTickerInfo.IsMaxSubmitSizeUpdated || hasFullReplace)
            {
                IsMaxSubmitSizeUpdated = true;

                MaxSubmitSize     = pqSrcTickerInfo.MaxSubmitSize;
            }
            if (pqSrcTickerInfo.IsIncrementSizeUpdated || hasFullReplace)
            {
                IsIncrementSizeUpdated = true;

                IncrementSize     = pqSrcTickerInfo.IncrementSize;
            }
            if (pqSrcTickerInfo.IsLayerFlagsUpdated || hasFullReplace)
            {
                IsLayerFlagsUpdated = true;
                
                LayerFlags           = pqSrcTickerInfo.LayerFlags;
            }
            if (pqSrcTickerInfo.IsLastTradedFlagsUpdated || hasFullReplace)
            {
                IsLastTradedFlagsUpdated = true;

                LastTradedFlags = pqSrcTickerInfo.LastTradedFlags;
            }

            if (hasFullReplace)
            {
                PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
                VolumeScalingPrecision = PQScaling.FindPriceScaleFactor(Math.Min(MinSubmitSize, IncrementSize));
                SetFlagsSame(pqSrcTickerInfo);
            }
        }
        else if (source is ISourceTickerInfo srcTickerInfo)
        {
            PublishedTickerQuoteDetailLevel = srcTickerInfo.PublishedTickerQuoteDetailLevel;
            MaximumPublishedLayers          = srcTickerInfo.MaximumPublishedLayers;

            Pip = srcTickerInfo.Pip;

            RoundingPrecision = srcTickerInfo.RoundingPrecision;
            SubscribeToPrices = srcTickerInfo.SubscribeToPrices;
            DefaultMaxValidMs = srcTickerInfo.DefaultMaxValidMs;
            MinimumQuoteLife  = srcTickerInfo.MinimumQuoteLife;

            TradingEnabled         = srcTickerInfo.TradingEnabled;
            MinSubmitSize          = srcTickerInfo.MinSubmitSize;
            MaxSubmitSize          = srcTickerInfo.MaxSubmitSize;
            IncrementSize          = srcTickerInfo.IncrementSize;
            LayerFlags             = srcTickerInfo.LayerFlags;
            LastTradedFlags        = srcTickerInfo.LastTradedFlags;

            PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindPriceScaleFactor(Math.Min(MinSubmitSize, IncrementSize));
        }
        return this;
    }

    bool IInterfacesComparable<ISourceTickerInfo>.AreEquivalent(ISourceTickerInfo? other, bool exactTypes) => 
        AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other is not ISourceTickerInfo srcTickerInfo) return false;
        if ((exactTypes && srcTickerInfo is not IPQSourceTickerInfo)) return false;

        var baseIsSame = base.AreEquivalent(other, exactTypes);

        var tickerDetailLevelSame = PublishedTickerQuoteDetailLevel == srcTickerInfo.PublishedTickerQuoteDetailLevel;
        var maxPublishLayersSame  = MaximumPublishedLayers == srcTickerInfo.MaximumPublishedLayers;
        var roundingPrecisionSame = RoundingPrecision == srcTickerInfo.RoundingPrecision;

        var pipSame = Pip == srcTickerInfo.Pip;

        var minSubmitSizeSame   = MinSubmitSize == srcTickerInfo.MinSubmitSize;
        var maxSubmitSizeSame   = MaxSubmitSize == srcTickerInfo.MaxSubmitSize;
        var incrementSizeSame   = IncrementSize == srcTickerInfo.IncrementSize;
        var minQuoteLifeSame    = MinimumQuoteLife == srcTickerInfo.MinimumQuoteLife;
        var defaultMaxValidSame = DefaultMaxValidMs == srcTickerInfo.DefaultMaxValidMs;
        var subscribeSame       = SubscribeToPrices == srcTickerInfo.SubscribeToPrices;
        var tradingEnabledSame  = TradingEnabled == srcTickerInfo.TradingEnabled;

        var layerFlagsSame      = LayerFlags == srcTickerInfo.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == srcTickerInfo.LastTradedFlags;

        var updatesSame = true;
        if (exactTypes)
        {
            var pqUniqSrcTrkId = other as PQSourceTickerInfo;
            updatesSame = UpdatedFlags == pqUniqSrcTrkId?.UpdatedFlags;
        }

        var allAreSame = baseIsSame && tickerDetailLevelSame && roundingPrecisionSame && pipSame && minSubmitSizeSame && maxSubmitSizeSame
                      && incrementSizeSame && minQuoteLifeSame && layerFlagsSame && maxPublishLayersSame && defaultMaxValidSame && subscribeSame
                      && tradingEnabledSame && lastTradedFlagsSame && updatesSame;
        if (!allAreSame)
        {
            Console.Out.WriteLine("");
        }
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourceTickerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ InstrumentId;
            return hashCode;
        }
    }

    protected string PQSourceTickerInfoToStringMembers =>
        $"{PQPricingInstrumentIdToStringMembers}, {nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(Pip)}: {Pip}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, " +
        $"{nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, {nameof(DefaultMaxValidMs)}: {DefaultMaxValidMs}, " +
        $"{nameof(SubscribeToPrices)}: {SubscribeToPrices}, {nameof(TradingEnabled)}: {TradingEnabled}, {nameof(LayerFlags)}: {LayerFlags:F}, " +
        $"{nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags}, " +
        $"{nameof(QuoteBehaviorFlags)}: {QuoteBehaviorFlags}";

    public override string ToString() => $"{nameof(PQSourceTickerInfo)}{{{PQSourceTickerInfoToStringMembers}, {UpdateFlagsToString}}}";
}

public static class PQSourceTickerInfoExtensions
{



    public static PQSourceTickerInfo WithRoundingPrecision(this PQSourceTickerInfo toCopy, decimal roundingPrecision) =>
        new(toCopy) { RoundingPrecision = roundingPrecision };

    public static PQSourceTickerInfo WithTickerDetailLevel(this PQSourceTickerInfo toCopy, TickerQuoteDetailLevel tickerQuoteDetailLevel) =>
        new(toCopy) { PublishedTickerQuoteDetailLevel = tickerQuoteDetailLevel };

    public static PQSourceTickerInfo WithPip(this PQSourceTickerInfo toCopy, decimal pip) => new(toCopy) { Pip = pip };

    public static PQSourceTickerInfo WithDefaultMaxValidMs(this PQSourceTickerInfo toCopy, uint defaultMaxValidMs) =>
        new(toCopy) { DefaultMaxValidMs = defaultMaxValidMs };

    public static PQSourceTickerInfo WithSubscribeToPrices(this PQSourceTickerInfo toCopy, bool subscribeToPrice) =>
        new(toCopy) { SubscribeToPrices = subscribeToPrice };

    public static PQSourceTickerInfo WithTradingEnabled(this PQSourceTickerInfo toCopy, bool tradingEnabled) =>
        new(toCopy) { TradingEnabled = tradingEnabled };

    public static PQSourceTickerInfo WithMaximumPublishedLayers(this PQSourceTickerInfo toCopy, ushort maxPublishedLayers) =>
        new(toCopy) { MaximumPublishedLayers = maxPublishedLayers };

    public static PQSourceTickerInfo WithMinSubmitSize(this PQSourceTickerInfo toCopy, decimal minSubmitSize) =>
        new(toCopy) { MinSubmitSize = minSubmitSize };

    public static PQSourceTickerInfo WithMaxSubmitSize(this PQSourceTickerInfo toCopy, decimal maxSubmitSize) =>
        new(toCopy) { MaxSubmitSize = maxSubmitSize };

    public static PQSourceTickerInfo WithIncrementSize(this PQSourceTickerInfo toCopy, decimal incrementSize) =>
        new(toCopy) { IncrementSize = incrementSize };

    public static PQSourceTickerInfo WithMinimumQuoteLife(this PQSourceTickerInfo toCopy, ushort minQuoteLifeMs) =>
        new(toCopy) { MinimumQuoteLife = minQuoteLifeMs };

    public static PQSourceTickerInfo WithLayerFlags(this PQSourceTickerInfo toCopy, LayerFlags layerFlags) => new(toCopy) { LayerFlags = layerFlags };

    public static PQSourceTickerInfo WithLastTradedFlags(this PQSourceTickerInfo toCopy, LastTradedFlags lastTradedFlags) =>
        new(toCopy) { LastTradedFlags = lastTradedFlags };
}
