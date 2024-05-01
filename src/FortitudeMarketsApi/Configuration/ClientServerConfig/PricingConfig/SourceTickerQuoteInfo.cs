#region

using System.Globalization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISourceTickerQuoteInfo : IInterfacesComparable<ISourceTickerQuoteInfo>, ICloneable<ISourceTickerQuoteInfo>
{
    uint Id { get; }
    ushort SourceId { get; set; }
    ushort TickerId { get; set; }
    string Source { get; set; }
    string Ticker { get; set; }
    QuoteLevel PublishedQuoteLevel { get; set; }
    decimal RoundingPrecision { get; set; }
    decimal MinSubmitSize { get; set; }
    decimal MaxSubmitSize { get; set; }
    decimal IncrementSize { get; set; }
    ushort MinimumQuoteLife { get; set; }
    LayerFlags LayerFlags { get; set; }
    byte MaximumPublishedLayers { get; set; }
    LastTradedFlags LastTradedFlags { get; set; }
    string FormatPrice { get; }
}

public class SourceTickerQuoteInfo : ReusableObject<ISourceTickerQuoteInfo>, ISourceTickerQuoteInfo
{
    private string? formatPrice;

    public SourceTickerQuoteInfo()
    {
        Source = null!;
        Ticker = null!;
    }

    public SourceTickerQuoteInfo(ushort sourceId, string source, ushort tickerId, string ticker, QuoteLevel publishedQuoteLevel,
        byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m, decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m,
        decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source = source;
        Ticker = ticker;
        PublishedQuoteLevel = publishedQuoteLevel;
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision = roundingPrecision;
        MinSubmitSize = minSubmitSize;
        MaxSubmitSize = maxSubmitSize;
        IncrementSize = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags = layerFlags;
        LastTradedFlags = lastTradedFlags;
    }

    public SourceTickerQuoteInfo(ISourceTickerQuoteInfo toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source = toClone.Source;
        Ticker = toClone.Ticker;
        PublishedQuoteLevel = toClone.PublishedQuoteLevel;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        RoundingPrecision = toClone.RoundingPrecision;
        MinSubmitSize = toClone.MinSubmitSize;
        MaxSubmitSize = toClone.MaxSubmitSize;
        IncrementSize = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags = toClone.LayerFlags;
        LastTradedFlags = toClone.LastTradedFlags;
    }

    object ICloneable.Clone() => Clone();

    public override ISourceTickerQuoteInfo Clone() => Recycler?.Borrow<SourceTickerQuoteInfo>()?.CopyFrom(this) ?? new SourceTickerQuoteInfo(this);

    public uint Id => (uint)((SourceId << 16) | TickerId);
    public ushort SourceId { get; set; }
    public ushort TickerId { get; set; }
    public string Source { get; set; }
    public string Ticker { get; set; }
    public QuoteLevel PublishedQuoteLevel { get; set; }
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

    public virtual bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        var sourceIdSame = SourceId == other?.SourceId;
        var tickerIdSame = TickerId == other?.TickerId;
        var sourceSame = Source == other?.Source;
        var tickerSame = Ticker == other?.Ticker;
        var quoteLevelSame = PublishedQuoteLevel == other?.PublishedQuoteLevel;
        var maxPublishedLayersSame = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var roundingPrecisionSame = RoundingPrecision == other?.RoundingPrecision;
        var minSubmitSizeSame = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame = MaxSubmitSize == other?.MaxSubmitSize;
        var incrmntSizeSame = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame = MinimumQuoteLife == other?.MinimumQuoteLife;
        var layerFlagsSame = LayerFlags == other?.LayerFlags;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;

        return sourceIdSame && tickerIdSame && sourceSame && tickerSame && quoteLevelSame && maxPublishedLayersSame && roundingPrecisionSame
               && minSubmitSizeSame && maxSubmitSizeSame && incrmntSizeSame && minQuoteLifeSame && layerFlagsSame && lastTradedFlagsSame;
    }

    public override ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = source.SourceId;
        TickerId = source.TickerId;
        Source = source.Source;
        Ticker = source.Ticker;
        PublishedQuoteLevel = source.PublishedQuoteLevel;
        RoundingPrecision = source.RoundingPrecision;
        MinSubmitSize = source.MinSubmitSize;
        MaxSubmitSize = source.MaxSubmitSize;
        IncrementSize = source.IncrementSize;
        MinimumQuoteLife = source.MinimumQuoteLife;
        LayerFlags = source.LayerFlags;
        MaximumPublishedLayers = MaximumPublishedLayers;
        LastTradedFlags = LastTradedFlags;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISourceTickerQuoteInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ Source.GetHashCode();
            hashCode = (hashCode * 397) ^ Ticker.GetHashCode();
            hashCode = (hashCode * 397) ^ PublishedQuoteLevel.GetHashCode();
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
        $"SourceTickerQuoteInfo {{{nameof(Id)}: {Id}, {nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, " +
        $"{nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker},  {nameof(PublishedQuoteLevel)}: {PublishedQuoteLevel},  " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(MinSubmitSize)}: {MinSubmitSize}, " +
        $"{nameof(MaxSubmitSize)}: {MaxSubmitSize}, {nameof(IncrementSize)}: {IncrementSize}, " +
        $"{nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, {nameof(LayerFlags)}: {LayerFlags:F}, " +
        $"{nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, {nameof(LastTradedFlags)}: {LastTradedFlags} }}";
}
