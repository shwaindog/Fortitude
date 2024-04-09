#region

using System.Globalization;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

public class PQSourceTickerQuoteInfo : PQUniqueSourceTickerIdentifier, IPQSourceTickerQuoteInfo
{
    private string? formatPrice;
    private decimal incrementSize;
    private LastTradedFlags lastTradedFlags;
    private LayerFlags layerFlags;
    private byte maximumPublishedLayers;
    private decimal maxSubmitSize;
    private ushort minimumQuoteLife;
    private decimal minSubmitSize;
    private decimal roundingPrecision;

    public PQSourceTickerQuoteInfo() => formatPrice = "";

    public PQSourceTickerQuoteInfo(ISourceTickerQuoteInfo toClone) : base(toClone)
    {
        RoundingPrecision = toClone.RoundingPrecision;
        MinSubmitSize = toClone.MinSubmitSize;
        MaxSubmitSize = toClone.MaxSubmitSize;
        IncrementSize = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        LayerFlags = toClone.LayerFlags;
        LastTradedFlags = toClone.LastTradedFlags;
        if (toClone is IPQSourceTickerQuoteInfo pubToClone)
        {
            SourceNameIdLookup = pubToClone.SourceNameIdLookup;
            TraderNameIdLookup = pubToClone.TraderNameIdLookup;
            LastTraderNameLookup = pubToClone.LastTraderNameLookup;
            IsIncrementSizeUpdated = pubToClone.IsIncrementSizeUpdated;
            IsLastTradedFlagsUpdated = pubToClone.IsLastTradedFlagsUpdated;
            IsMaxSubmitSizeUpdated = pubToClone.IsMaxSubmitSizeUpdated;
            IsMinSubmitSizeUpdated = pubToClone.IsMinSubmitSizeUpdated;
            IsRoundingPrecisionUpdated = pubToClone.IsRoundingPrecisionUpdated;
            IsMinimumQuoteLifeUpdated = pubToClone.IsMinimumQuoteLifeUpdated;
            IsMaximumPublishedLayersUpdated = pubToClone.IsMaximumPublishedLayersUpdated;
            IsLayerFlagsUpdated = pubToClone.IsLayerFlagsUpdated;
        }
    }

    public byte PriceScalingPrecision { get; set; } = 1;
    public byte VolumeScalingPrecision { get; } = 6;

    public decimal RoundingPrecision
    {
        get => roundingPrecision;
        set
        {
            if (roundingPrecision == value) return;
            IsRoundingPrecisionUpdated = true;
            formatPrice = null;
            roundingPrecision = value;
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

    public decimal MinSubmitSize
    {
        get => minSubmitSize;
        set
        {
            if (minSubmitSize == value) return;

            IsMinSubmitSizeUpdated = true;
            minSubmitSize = value;
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

    public decimal MaxSubmitSize
    {
        get => maxSubmitSize;
        set
        {
            if (maxSubmitSize == value) return;

            IsMaxSubmitSizeUpdated = true;
            maxSubmitSize = value;
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

    public decimal IncrementSize
    {
        get => incrementSize;
        set
        {
            if (incrementSize == value) return;

            IsIncrementSizeUpdated = true;
            incrementSize = value;
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

    public ushort MinimumQuoteLife
    {
        get => minimumQuoteLife;
        set
        {
            if (minimumQuoteLife == value) return;

            IsMinimumQuoteLifeUpdated = true;
            minimumQuoteLife = value;
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

    public LayerFlags LayerFlags
    {
        get => layerFlags;
        set
        {
            if (layerFlags == value) return;

            IsLayerFlagsUpdated = true;
            layerFlags = value;
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

    public byte MaximumPublishedLayers
    {
        get => maximumPublishedLayers;
        set
        {
            if (maximumPublishedLayers == value) return;

            IsMaximumPublishedLayersUpdated = true;
            maximumPublishedLayers = value;
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

    public LastTradedFlags LastTradedFlags
    {
        get => lastTradedFlags;
        set
        {
            if (lastTradedFlags == value) return;
            IsLastTradedFlagsUpdated = true;
            lastTradedFlags = value;
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

    public IPQNameIdLookupGenerator? SourceNameIdLookup { get; set; }
    public IPQNameIdLookupGenerator? TraderNameIdLookup { get; set; }
    public IPQNameIdLookupGenerator? LastTraderNameLookup { get; set; }

    public string FormatPrice =>
        formatPrice ?? (formatPrice = RoundingPrecision
            .ToString(CultureInfo.InvariantCulture)
            .Replace('1', '0')
            .Replace('2', '0')
            .Replace('3', '0')
            .Replace('4', '0')
            .Replace('5', '0')
            .Replace('6', '0')
            .Replace('7', '0')
            .Replace('8', '0')
            .Replace('9', '0'));

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSettings))
            yield return updatedField;
        if (!updatedOnly || IsRoundingPrecisionUpdated)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(RoundingPrecision)[3])[2];
            var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * RoundingPrecision);
            yield return new PQFieldUpdate(PQFieldKeys.RoundingPrecision, roundingNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsMinSubmitSizeUpdated)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(MinSubmitSize)[3])[2];
            var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MinSubmitSize);
            yield return new PQFieldUpdate(PQFieldKeys.MinSubmitSize, minSubmitNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsMaxSubmitSizeUpdated)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(MaxSubmitSize)[3])[2];
            var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * MaxSubmitSize);
            yield return new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, maxSubmitNoDecimal, decimalPlaces);
        }

        if (!updatedOnly || IsIncrementSizeUpdated)
        {
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(IncrementSize)[3])[2];
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

    public override int UpdateField(PQFieldUpdate fieldUpdate)
    {
        var idResult = base.UpdateField(fieldUpdate);
        if (idResult > 0) return idResult;
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.RoundingPrecision:
                var decimalPlaces = fieldUpdate.Flag;
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

    public override IUniqueSourceTickerIdentifier CopyFrom(IUniqueSourceTickerIdentifier source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);

        if (source == null) return this;

        if (!(source is ISourceTickerQuoteInfo src)) return this;
        RoundingPrecision = src.RoundingPrecision;
        MinSubmitSize = src.MinSubmitSize;
        MaxSubmitSize = src.MaxSubmitSize;
        IncrementSize = src.IncrementSize;
        MinimumQuoteLife = src.MinimumQuoteLife;
        LayerFlags = src.LayerFlags;
        MaximumPublishedLayers = src.MaximumPublishedLayers;
        LastTradedFlags = src.LastTradedFlags;
        if (source is PQSourceTickerQuoteInfo pqSrcTkrQtInfo)
        {
            if (pqSrcTkrQtInfo.IsRoundingPrecisionUpdated) RoundingPrecision = pqSrcTkrQtInfo.RoundingPrecision;
            if (pqSrcTkrQtInfo.IsMinSubmitSizeUpdated) MinSubmitSize = pqSrcTkrQtInfo.MinSubmitSize;
            if (pqSrcTkrQtInfo.IsMaxSubmitSizeUpdated) MaxSubmitSize = pqSrcTkrQtInfo.MaxSubmitSize;
            if (pqSrcTkrQtInfo.IsIncrementSizeUpdated) IncrementSize = pqSrcTkrQtInfo.IncrementSize;
            if (pqSrcTkrQtInfo.IsMinimumQuoteLifeUpdated) MinimumQuoteLife = pqSrcTkrQtInfo.MinimumQuoteLife;
            if (pqSrcTkrQtInfo.IsLayerFlagsUpdated) LayerFlags = pqSrcTkrQtInfo.LayerFlags;
            if (pqSrcTkrQtInfo.IsMaximumPublishedLayersUpdated)
                MaximumPublishedLayers = pqSrcTkrQtInfo.MaximumPublishedLayers;
            if (pqSrcTkrQtInfo.IsLastTradedFlagsUpdated) LastTradedFlags = pqSrcTkrQtInfo.LastTradedFlags;
        }

        return this;
    }

    public override object Clone() => new PQSourceTickerQuoteInfo(this);

    ISourceTickerQuoteInfo ISourceTickerQuoteInfo.Clone() => (ISourceTickerQuoteInfo)Clone();

    IMutableSourceTickerQuoteInfo IMutableSourceTickerQuoteInfo.Clone() => (IMutableSourceTickerQuoteInfo)Clone();

    public bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);
        var roundingPrecisionSame = RoundingPrecision == other?.RoundingPrecision;
        var minSubmitSizeSame = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame = MaxSubmitSize == other?.MaxSubmitSize;
        var incrementSizeSame = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame = MinimumQuoteLife == other?.MinimumQuoteLife;
        var layerFlagsSame = LayerFlags == other?.LayerFlags;
        var maxPublishLayersSame = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        return baseSame && roundingPrecisionSame && minSubmitSizeSame && maxSubmitSizeSame && incrementSizeSame
               && minQuoteLifeSame && layerFlagsSame && maxPublishLayersSame && lastTradedFlagsSame;
    }

    public override bool AreEquivalent(IUniqueSourceTickerIdentifier? other, bool exactTypes = false)
    {
        if (!(other is ISourceTickerQuoteInfo srcTkrQtInfo)) return false;

        return AreEquivalent(srcTkrQtInfo, exactTypes);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourceTickerQuoteInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (formatPrice != null ? formatPrice.GetHashCode() : 0);
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
        $"PQSourceTickerQuoteInfo {{{nameof(roundingPrecision)}: {roundingPrecision}, " +
        $"{nameof(minSubmitSize)}: {minSubmitSize}, {nameof(maxSubmitSize)}: {maxSubmitSize}, " +
        $"{nameof(incrementSize)}: {incrementSize}, {nameof(minimumQuoteLife)}: {minimumQuoteLife}, " +
        $"{nameof(layerFlags)}: {layerFlags}, {nameof(maximumPublishedLayers)}: {maximumPublishedLayers}, " +
        $"{nameof(lastTradedFlags)}: {lastTradedFlags} }}";
}
