#region

using System.Globalization;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

public class SourceTickerQuoteInfo : UniqueSourceTickerIdentifier, IMutableSourceTickerQuoteInfo
{
    private string? formatPrice;

    public SourceTickerQuoteInfo(uint uniqueId, string source, string ticker, byte maximumPublishedLayers = 20,
        decimal roundingPrecision = 0.0001m, decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m,
        decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None)
        : base(uniqueId, source, ticker)
    {
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision = roundingPrecision;
        MinSubmitSize = minSubmitSize;
        MaxSubmitSize = maxSubmitSize;
        IncrementSize = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags = layerFlags;
        LastTradedFlags = lastTradedFlags;
    }

    public SourceTickerQuoteInfo(ISourceTickerQuoteInfo toClone) : base(toClone)
    {
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision = toClone.RoundingPrecision;
        MinSubmitSize = toClone.MinSubmitSize;
        MaxSubmitSize = toClone.MaxSubmitSize;
        IncrementSize = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags = toClone.LayerFlags;
        LastTradedFlags = toClone.LastTradedFlags;
    }

    public byte MaximumPublishedLayers { get; set; }
    public decimal RoundingPrecision { get; set; }
    public decimal MinSubmitSize { get; set; }
    public decimal MaxSubmitSize { get; set; }
    public decimal IncrementSize { get; set; }
    public ushort MinimumQuoteLife { get; set; }
    public LayerFlags LayerFlags { get; set; }
    public LastTradedFlags LastTradedFlags { get; set; }

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

    public override object Clone() => new SourceTickerQuoteInfo(this);

    ISourceTickerQuoteInfo ISourceTickerQuoteInfo.Clone() => (ISourceTickerQuoteInfo)Clone();

    IMutableSourceTickerQuoteInfo IMutableSourceTickerQuoteInfo.Clone() => (IMutableSourceTickerQuoteInfo)Clone();

    public virtual bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);
        var maxPublishedLayersSame = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var roundingPrecisionSame = RoundingPrecision == other?.RoundingPrecision;
        var minSubmitSizeSame = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame = MaxSubmitSize == other?.MaxSubmitSize;
        var incrmntSizeSame = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame = MinimumQuoteLife == other?.MinimumQuoteLife;
        var layerFlagsSame = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        return baseSame && maxPublishedLayersSame && roundingPrecisionSame && minSubmitSizeSame
               && maxSubmitSizeSame && incrmntSizeSame && minQuoteLifeSame && layerFlagsSame
               && lastTradedFlagsSame;
    }


    public override bool AreEquivalent(IUniqueSourceTickerIdentifier? other, bool exactTypes = false)
    {
        if (!(other is ISourceTickerQuoteInfo srcTkrQuoteInfo)) return false;
        return AreEquivalent(srcTkrQuoteInfo, exactTypes);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISourceTickerQuoteInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ MaximumPublishedLayers.GetHashCode();
            hashCode = (hashCode * 397) ^ (formatPrice != null ? formatPrice.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ RoundingPrecision.GetHashCode();
            hashCode = (hashCode * 397) ^ MinSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ IncrementSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MinimumQuoteLife.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)LayerFlags;
            hashCode = (hashCode * 397) ^ (int)LastTradedFlags;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"SourceTickerQuoteInfo {{{nameof(Id)}: {Id}, {nameof(Source)}: {Source}, " +
        $"{nameof(Ticker)}: {Ticker},  {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags} }}";
}
