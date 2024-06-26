﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Globalization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

public interface IPQSourceTickerQuoteInfo : ISourceTickerQuoteInfo, IPQPriceVolumePublicationPrecisionSettings, ICloneable<IPQSourceTickerQuoteInfo>
  , IHasNameIdLookup, IPQSupportsFieldUpdates<IPQSourceTickerQuoteInfo>, IPQSupportsStringUpdates<IPQSourceTickerQuoteInfo>
{
    bool IsIdUpdated                     { get; set; }
    bool IsSourceUpdated                 { get; set; }
    bool IsTickerUpdated                 { get; set; }
    bool IsPublishedQuoteLevelUpdated    { get; set; }
    bool IsMarketClassificationUpdated   { get; set; }
    bool IsRoundingPrecisionUpdated      { get; set; }
    bool IsMinSubmitSizeUpdated          { get; set; }
    bool IsMaxSubmitSizeUpdated          { get; set; }
    bool IsIncrementSizeUpdated          { get; set; }
    bool IsMinimumQuoteLifeUpdated       { get; set; }
    bool IsLayerFlagsUpdated             { get; set; }
    bool IsMaximumPublishedLayersUpdated { get; set; }
    bool IsLastTradedFlagsUpdated        { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceTickerQuoteInfo Clone();
}

public class PQSourceTickerQuoteInfo : ReusableObject<PQSourceTickerQuoteInfo>, IPQSourceTickerQuoteInfo
{
    private string? category;

    private string?         formatPrice;
    private decimal         incrementSize;
    private LastTradedFlags lastTradedFlags;
    private LayerFlags      layerFlags;

    private MarketClassification marketClassification;

    private byte       maximumPublishedLayers;
    private decimal    maxSubmitSize;
    private ushort     minimumQuoteLife;
    private decimal    minSubmitSize;
    private string[]?  optionalKeys;
    private QuoteLevel publishedQuoteLevel = QuoteLevel.Level2;
    private string[]?  requiredKeys;

    private decimal roundingPrecision;
    private string  source = "";
    private ushort  sourceId;
    private string  ticker = "";
    private ushort  tickerId;

    protected SourceTickerInfoUpdatedFlags UpdatedFlags;

    public PQSourceTickerQuoteInfo() => formatPrice = "";

    public PQSourceTickerQuoteInfo
    (ushort sourceId, string source, ushort tickerId, string ticker, QuoteLevel publishedQuoteLevel,
        MarketClassification marketClassification, byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m,
        decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m, decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume, LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;

        MarketClassification   = marketClassification;
        PublishedQuoteLevel    = publishedQuoteLevel;
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision      = roundingPrecision;

        MinSubmitSize    = minSubmitSize;
        MaxSubmitSize    = maxSubmitSize;
        IncrementSize    = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags       = layerFlags;
        LastTradedFlags  = lastTradedFlags;

        PriceScalingPrecision  = PQScaling.FindScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindScaleFactor(Math.Min(MinSubmitSize, IncrementSize));

        Category = PublishedQuoteLevel.ToString();
    }

    public PQSourceTickerQuoteInfo(ISourceTickerQuoteInfo toClone)
    {
        SourceId = toClone.SourceId;
        Source   = toClone.Source;
        TickerId = toClone.TickerId;
        Ticker   = toClone.Ticker;

        PublishedQuoteLevel    = toClone.PublishedQuoteLevel;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision      = toClone.RoundingPrecision;

        MinSubmitSize    = toClone.MinSubmitSize;
        MaxSubmitSize    = toClone.MaxSubmitSize;
        IncrementSize    = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags       = toClone.LayerFlags;
        LastTradedFlags  = toClone.LastTradedFlags;

        foreach (var instrumentFields in toClone) this[instrumentFields.Key] = instrumentFields.Value;

        PriceScalingPrecision  = PQScaling.FindScaleFactor(RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindScaleFactor(Math.Min(MinSubmitSize, IncrementSize));


        foreach (var instrumentFields in toClone) this[instrumentFields.Key] = instrumentFields.Value;

        if (toClone is IPQSourceTickerQuoteInfo pubToClone)
        {
            IsIdUpdated     = pubToClone.IsIdUpdated;
            IsSourceUpdated = pubToClone.IsSourceUpdated;
            IsTickerUpdated = pubToClone.IsTickerUpdated;

            IsPublishedQuoteLevelUpdated    = pubToClone.IsPublishedQuoteLevelUpdated;
            IsMarketClassificationUpdated   = pubToClone.IsMarketClassificationUpdated;
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

    public string? Category
    {
        get => category ?? PublishedQuoteLevel.ToString();
        set => category = value;
    }

    public byte PriceScalingPrecision  { get; } = 3;
    public byte VolumeScalingPrecision { get; } = 6;

    public virtual bool HasUpdates
    {
        get => UpdatedFlags > 0;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : SourceTickerInfoUpdatedFlags.None;
    }

    public uint Id => ((uint)SourceId << 16) | TickerId;

    public uint MessageId => Id;

    public byte Version => 1;

    public ushort SourceId
    {
        get => sourceId;
        set
        {
            if (sourceId == value) return;
            IsIdUpdated = true;
            sourceId    = value;
        }
    }

    public ushort TickerId
    {
        get => tickerId;
        set
        {
            if (tickerId == value) return;
            IsIdUpdated = true;
            tickerId    = value;
        }
    }

    public string Source
    {
        get => source;
        set
        {
            if (source == value) return;
            IsSourceUpdated = true;
            source          = value;
        }
    }

    public string Ticker
    {
        get => ticker;
        set
        {
            if (ticker == value) return;
            IsTickerUpdated = true;
            ticker          = value;
        }
    }

    public QuoteLevel PublishedQuoteLevel
    {
        get => publishedQuoteLevel;
        set
        {
            if (publishedQuoteLevel == value) return;
            IsPublishedQuoteLevelUpdated = true;
            publishedQuoteLevel          = value;
        }
    }

    public MarketClassification MarketClassification
    {
        get => marketClassification;
        set
        {
            if (Equals(marketClassification, value)) return;
            IsMarketClassificationUpdated = true;
            marketClassification          = value;
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

    public bool IsIdUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourceTickerId) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourceTickerId;

            else if (IsIdUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourceTickerId;
        }
    }

    public bool IsSourceUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourceName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourceName;

            else if (IsSourceUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourceName;
        }
    }

    public bool IsTickerUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.TickerName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.TickerName;

            else if (IsTickerUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.TickerName;
        }
    }

    public bool IsMarketClassificationUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.MarketClassification) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.MarketClassification;

            else if (IsMarketClassificationUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MarketClassification;
        }
    }

    public bool IsPublishedQuoteLevelUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.PublishedQuoteLevel) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.PublishedQuoteLevel;

            else if (IsPublishedQuoteLevelUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.PublishedQuoteLevel;
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
            else if (IsMaximumPublishedLayersUpdated)
                UpdatedFlags ^= SourceTickerInfoUpdatedFlags.MaximumPublishedLayers;
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

    string IInstrument.     InstrumentName => Ticker;
    public TimeSeriesPeriod EntryPeriod    { get; set; } = TimeSeriesPeriod.Tick;
    public InstrumentType   Type           { get; set; } = InstrumentType.Price;


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        if (Source.IsNotNullOrEmpty()) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.SourceName), Source);
        if (category != null) yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.Category), category);
        if (MarketClassification.MarketType != MarketType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketType), MarketClassification.MarketType.ToString());
        if (MarketClassification.ProductType != ProductType.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketProductType), MarketClassification.ProductType.ToString());
        if (MarketClassification.MarketRegion != MarketRegion.Unknown)
            yield return new KeyValuePair<string, string>(nameof(RepositoryPathName.MarketRegion), MarketClassification.MarketRegion.ToString());
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public string? this[string key]
    {
        get
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):        return MarketClassification.MarketType.ToString();
                case nameof(RepositoryPathName.MarketProductType): return MarketClassification.ProductType.ToString();
                case nameof(RepositoryPathName.MarketRegion):      return MarketClassification.MarketRegion.ToString();
                case nameof(RepositoryPathName.SourceName):        return Source;
                case nameof(RepositoryPathName.Category):          return Category ?? PublishedQuoteLevel.ToString();
            }
            return null;
        }
        set
        {
            switch (key)
            {
                case nameof(RepositoryPathName.MarketType):
                    if (Enum.TryParse<MarketType>(value, true, out var marketType))
                        if (MarketClassification.MarketType == MarketType.Unknown)
                            MarketClassification = MarketClassification.SetMarketType(marketType);
                    break;
                case nameof(RepositoryPathName.MarketProductType):
                    if (Enum.TryParse<ProductType>(value, true, out var productType))
                        if (MarketClassification.ProductType == ProductType.Unknown)
                            MarketClassification = MarketClassification.SetProductType(productType);
                    break;
                case nameof(RepositoryPathName.MarketRegion):
                    if (Enum.TryParse<MarketRegion>(value, true, out var marketRegion))
                        if (MarketClassification.MarketRegion == MarketRegion.Unknown)
                            MarketClassification = MarketClassification.SetMarketRegion(marketRegion);
                    break;
                case nameof(RepositoryPathName.SourceName):
                    if (Source.IsNullOrEmpty()) Source = value ?? "";
                    break;
                case nameof(RepositoryPathName.Category):
                    if (Category.IsNullOrEmpty()) Category = value ?? "";
                    break;
            }
        }
    }

    public IEnumerable<string> RequiredInstrumentKeys
    {
        get => requiredKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiRequiredInstrumentKeys;
        set => requiredKeys = value.ToArray();
    }

    public IEnumerable<string> OptionalInstrumentKeys
    {
        get => optionalKeys ??= DymwiTimeSeriesDirectoryRepository.DymwiOptionalInstrumentKeys;
        set => optionalKeys = value.ToArray();
    }

    public bool HasAllRequiredKeys =>
        Source.IsNotNullOrEmpty() && MarketClassification.MarketType != MarketType.Unknown
                                  && MarketClassification.ProductType != ProductType.Unknown &&
                                     MarketClassification.MarketRegion != MarketRegion.Unknown;


    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = new PQNameIdLookupGenerator(PQFieldKeys.SourceTickerNames);


    public bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        if (other == null || (exactTypes && other is not IPQSourceTickerQuoteInfo srcTkrQtInfo)) return false;

        var sourceIdSame   = SourceId == other.SourceId;
        var tickerIdSame   = TickerId == other.TickerId;
        var sourceNameSame = string.Equals(Source, other.Source);
        var tickerNameSame = string.Equals(Ticker, other.Ticker);
        var quoteLevelSame = PublishedQuoteLevel == other.PublishedQuoteLevel;

        var marketClassificationSame = Equals(MarketClassification, other.MarketClassification);
        var maxPublishLayersSame     = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var minQuoteLifeSame         = MinimumQuoteLife == other?.MinimumQuoteLife;
        var roundingPrecisionSame    = RoundingPrecision == other?.RoundingPrecision;

        var minSubmitSizeSame   = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame   = MaxSubmitSize == other?.MaxSubmitSize;
        var incrementSizeSame   = IncrementSize == other?.IncrementSize;
        var layerFlagsSame      = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        var updatesSame = true;
        if (exactTypes)
        {
            var pqUniqSrcTrkId = other as PQSourceTickerQuoteInfo;
            updatesSame = UpdatedFlags == pqUniqSrcTrkId!.UpdatedFlags;
        }

        var allAreSame = sourceIdSame && tickerIdSame && sourceNameSame && tickerNameSame && quoteLevelSame && marketClassificationSame
                      && roundingPrecisionSame && minSubmitSizeSame && maxSubmitSizeSame && incrementSizeSame && minQuoteLifeSame
                      && layerFlagsSame && maxPublishLayersSame && lastTradedFlagsSame && updatesSame;
        return allAreSame;
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPQSourceTickerQuoteInfo ICloneable<IPQSourceTickerQuoteInfo>.Clone() => Clone();

    IPQSourceTickerQuoteInfo IPQSourceTickerQuoteInfo.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public IReusableObject<IVersionedMessage> CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ISourceTickerQuoteInfo)source, copyMergeFlags);

    public IVersionedMessage CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ISourceTickerQuoteInfo)source, copyMergeFlags);

    ISourceTickerQuoteInfo ISourceTickerQuoteInfo.Clone() => Clone();


    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags updateStyle,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsPublishedQuoteLevelUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PublishQuoteLevelType, (byte)PublishedQuoteLevel);
        if (!updatedOnly || IsIdUpdated) yield return new PQFieldUpdate(PQFieldKeys.SourceTickerId, Id);
        if (!updatedOnly || IsMarketClassificationUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.MarketClassification, MarketClassification.CompoundedClassification);
        if (!updatedOnly || IsRoundingPrecisionUpdated)
        {
            var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(RoundingPrecision)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * RoundingPrecision);
            yield return new PQFieldUpdate(PQFieldKeys.RoundingPrecision, roundingNoDecimal, decimalPlaces);
        }

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

        if (!updatedOnly || IsMinimumQuoteLifeUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.MinimumQuoteLife, MinimumQuoteLife);
        if (!updatedOnly || IsLayerFlagsUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)LayerFlags);
        if (!updatedOnly || IsMaximumPublishedLayersUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.MaximumPublishedLayers, MaximumPublishedLayers);
        if (!updatedOnly || IsLastTradedFlagsUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)LastTradedFlags);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var isUpdateOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!isUpdateOnly || IsSourceUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 0, Value = Source, Command = CrudCommand.Upsert
                    }
            };
        if (!isUpdateOnly || IsTickerUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 1, Value = Ticker, Command = CrudCommand.Upsert
                    }
            };
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id == PQFieldKeys.SourceTickerNames)
        {
            var stringUpdt = updates.StringUpdate;
            var upsert     = stringUpdt.Command == CrudCommand.Upsert;

            if (stringUpdt.DictionaryId == 0 && upsert) Source = updates.StringUpdate.Value;
            if (stringUpdt.DictionaryId == 1 && upsert) Ticker = updates.StringUpdate.Value;
            return true;
        }

        return false;
    }

    public int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.SourceTickerId:
                SourceId = (ushort)(fieldUpdate.Value >> 16);
                TickerId = (ushort)(fieldUpdate.Value & 0xFFFF);
                return 0;
            case PQFieldKeys.PublishQuoteLevelType:
                PublishedQuoteLevel = (QuoteLevel)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.MarketClassification:
                MarketClassification = new MarketClassification(fieldUpdate.Value);
                return 0;
            case PQFieldKeys.SourceTickerNames:
                return (int)fieldUpdate.Value;
            case PQFieldKeys.RoundingPrecision:
                var decimalPlaces              = fieldUpdate.Flag;
                var convertedRoundingPrecision = (decimal)Math.Pow(10, -decimalPlaces) * fieldUpdate.Value;
                RoundingPrecision = convertedRoundingPrecision;
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
            case PQFieldKeys.MinimumQuoteLife:
                MinimumQuoteLife = (ushort)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.LayerFlags:
                LayerFlags = (LayerFlags)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.MaximumPublishedLayers:
                MaximumPublishedLayers = (byte)fieldUpdate.Value;
                return 0;
            case PQFieldKeys.LastTradedFlags:
                LastTradedFlags = (LastTradedFlags)fieldUpdate.Value;
                return 0;
        }

        return -1;
    }

    public IPQSourceTickerQuoteInfo CopyFrom(IPQSourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IPQSourceTickerQuoteInfo)CopyFrom((ISourceTickerQuoteInfo)source, copyMergeFlags);

    public override PQSourceTickerQuoteInfo Clone() =>
        Recycler?.Borrow<PQSourceTickerQuoteInfo>().CopyFrom(this) as PQSourceTickerQuoteInfo
     ?? new PQSourceTickerQuoteInfo(this);


    public override PQSourceTickerQuoteInfo CopyFrom(PQSourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var hasFullReplace = copyMergeFlags.HasFullReplace();

        if (source.IsSourceUpdated || hasFullReplace) Source = source.Source;
        if (source.IsSourceUpdated || hasFullReplace) Ticker = source.Ticker;

        if (source.IsPublishedQuoteLevelUpdated || hasFullReplace) PublishedQuoteLevel = source.PublishedQuoteLevel;
        if (source.IsMarketClassificationUpdated) MarketClassification                 = source.MarketClassification;
        if (source.IsRoundingPrecisionUpdated || hasFullReplace) RoundingPrecision     = source.RoundingPrecision;
        if (source.IsMinimumQuoteLifeUpdated || hasFullReplace) MinimumQuoteLife       = source.MinimumQuoteLife;

        if (source.IsMinSubmitSizeUpdated || hasFullReplace) MinSubmitSize = source.MinSubmitSize;
        if (source.IsMaxSubmitSizeUpdated || hasFullReplace) MaxSubmitSize = source.MaxSubmitSize;
        if (source.IsIncrementSizeUpdated || hasFullReplace) IncrementSize = source.IncrementSize;
        if (source.IsLayerFlagsUpdated || hasFullReplace) LayerFlags       = source.LayerFlags;
        if (source.IsMaximumPublishedLayersUpdated || hasFullReplace)
            MaximumPublishedLayers = source.MaximumPublishedLayers;
        if (source.IsLastTradedFlagsUpdated || hasFullReplace) LastTradedFlags = source.LastTradedFlags;
        return this;
    }

    public ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQSourceTickerQuoteInfo pqSrcTkrQtInfo && copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (pqSrcTkrQtInfo.IsSourceUpdated) Source = pqSrcTkrQtInfo.Source;
            if (pqSrcTkrQtInfo.IsSourceUpdated) Ticker = pqSrcTkrQtInfo.Ticker;

            if (pqSrcTkrQtInfo.IsPublishedQuoteLevelUpdated) PublishedQuoteLevel   = pqSrcTkrQtInfo.PublishedQuoteLevel;
            if (pqSrcTkrQtInfo.IsMarketClassificationUpdated) MarketClassification = pqSrcTkrQtInfo.MarketClassification;
            if (pqSrcTkrQtInfo.IsRoundingPrecisionUpdated) RoundingPrecision       = pqSrcTkrQtInfo.RoundingPrecision;
            if (pqSrcTkrQtInfo.IsMinimumQuoteLifeUpdated) MinimumQuoteLife         = pqSrcTkrQtInfo.MinimumQuoteLife;

            if (pqSrcTkrQtInfo.IsMinSubmitSizeUpdated) MinSubmitSize = pqSrcTkrQtInfo.MinSubmitSize;
            if (pqSrcTkrQtInfo.IsMaxSubmitSizeUpdated) MaxSubmitSize = pqSrcTkrQtInfo.MaxSubmitSize;
            if (pqSrcTkrQtInfo.IsIncrementSizeUpdated) IncrementSize = pqSrcTkrQtInfo.IncrementSize;
            if (pqSrcTkrQtInfo.IsLayerFlagsUpdated) LayerFlags       = pqSrcTkrQtInfo.LayerFlags;
            if (pqSrcTkrQtInfo.IsMaximumPublishedLayersUpdated)
                MaximumPublishedLayers = pqSrcTkrQtInfo.MaximumPublishedLayers;
            if (pqSrcTkrQtInfo.IsLastTradedFlagsUpdated) LastTradedFlags = pqSrcTkrQtInfo.LastTradedFlags;
        }
        else
        {
            SourceId = source.SourceId;
            Source   = source.Source;
            TickerId = source.TickerId;
            Ticker   = source.Ticker;

            PublishedQuoteLevel    = source.PublishedQuoteLevel;
            MaximumPublishedLayers = source.MaximumPublishedLayers;
            RoundingPrecision      = source.RoundingPrecision;

            MinSubmitSize    = source.MinSubmitSize;
            MaxSubmitSize    = source.MaxSubmitSize;
            IncrementSize    = source.IncrementSize;
            MinimumQuoteLife = source.MinimumQuoteLife;
            LayerFlags       = source.LayerFlags;
            LastTradedFlags  = source.LastTradedFlags;
        }
        foreach (var instrumentFields in source) this[instrumentFields.Key] = instrumentFields.Value;

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourceTickerQuoteInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ Source.GetHashCode();
            hashCode = (hashCode * 397) ^ Ticker.GetHashCode();
            hashCode = (hashCode * 397) ^ PublishedQuoteLevel.GetHashCode();
            hashCode = (hashCode * 397) ^ MarketClassification.GetHashCode();
            hashCode = (hashCode * 397) ^ RoundingPrecision.GetHashCode();
            hashCode = (hashCode * 397) ^ MinSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ IncrementSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MinimumQuoteLife.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)LayerFlags;
            hashCode = (hashCode * 397) ^ MaximumPublishedLayers.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)LastTradedFlags;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQSourceTickerQuoteInfo({nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, {nameof(TickerId)}: {TickerId}, " +
        $"{nameof(Ticker)}: {Ticker}, {nameof(PublishedQuoteLevel)}: {publishedQuoteLevel}, " +
        $"{nameof(MarketClassification)}: {marketClassification}, {nameof(MaximumPublishedLayers)}: {maximumPublishedLayers}, " +
        $"{nameof(RoundingPrecision)}: {roundingPrecision}, {nameof(MinSubmitSize)}: {minSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {maxSubmitSize}, {nameof(IncrementSize)}: {incrementSize}, {nameof(MinimumQuoteLife)}: {minimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {layerFlags}, {nameof(LastTradedFlags)}: {lastTradedFlags})";
}
