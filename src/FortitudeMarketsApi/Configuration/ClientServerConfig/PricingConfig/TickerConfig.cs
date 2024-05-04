#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ITickerConfig : IInterfacesComparable<ITickerConfig>
{
    ushort TickerId { get; set; }
    string Ticker { get; set; }
    TickerAvailability? TickerAvailability { get; set; }
    QuoteLevel? PublishedQuoteLevel { get; set; }
    decimal? RoundingPrecision { get; }
    decimal? MinSubmitSize { get; }
    decimal? MaxSubmitSize { get; }
    decimal? IncrementSize { get; }
    ushort? MinimumQuoteLife { get; }
    LayerFlags? LayerFlags { get; }
    byte? MaximumPublishedLayers { get; }
    LastTradedFlags? LastTradedFlags { get; }
}

public class TickerConfig : ConfigSection, ITickerConfig
{
    public TickerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public TickerConfig() { }

    public TickerConfig(ushort tickerId, string ticker, TickerAvailability? tickerAvailability = null, QuoteLevel? publishedQuoteLevel = null,
        decimal? roundingPrecision = null, decimal? minSubmitSize = null, decimal? maxSubmitSize = null, decimal? incrementSize = null,
        ushort? minimumQuoteLife = null, LayerFlags? layerFlags = null, byte? maximumPublisherLayers = null, LastTradedFlags? lastTradedFlags = null)
    {
        TickerId = tickerId;
        Ticker = ticker;
        TickerAvailability = tickerAvailability;
        PublishedQuoteLevel = publishedQuoteLevel;
        RoundingPrecision = roundingPrecision;
        MinSubmitSize = minSubmitSize;
        MaxSubmitSize = maxSubmitSize;
        IncrementSize = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags = layerFlags;
        MaximumPublishedLayers = maximumPublisherLayers;
        LastTradedFlags = lastTradedFlags;
    }

    public TickerConfig(ITickerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TickerId = toClone.TickerId;
        Ticker = toClone.Ticker;
        TickerAvailability = toClone.TickerAvailability;
        PublishedQuoteLevel = toClone.PublishedQuoteLevel;
        RoundingPrecision = toClone.RoundingPrecision;
        MinSubmitSize = toClone.MinSubmitSize;
        MaxSubmitSize = toClone.MaxSubmitSize;
        IncrementSize = toClone.IncrementSize;
        MinimumQuoteLife = toClone.MinimumQuoteLife;
        LayerFlags = toClone.LayerFlags;
        MaximumPublishedLayers = toClone.MaximumPublishedLayers;
        LastTradedFlags = toClone.LastTradedFlags;
    }

    public TickerConfig(ITickerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort TickerId
    {
        get => ushort.Parse(this[nameof(TickerId)]!);
        set => this[nameof(TickerId)] = value.ToString();
    }

    public string Ticker
    {
        get => this[nameof(Ticker)]!;
        set => this[nameof(Ticker)] = value;
    }

    public TickerAvailability? TickerAvailability
    {
        get
        {
            var checkValue = this[nameof(TickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : null;
        }
        set => this[nameof(TickerAvailability)] = value.ToString();
    }

    public QuoteLevel? PublishedQuoteLevel
    {
        get
        {
            var checkValue = this[nameof(PublishedQuoteLevel)];
            return checkValue != null ? Enum.Parse<QuoteLevel>(checkValue) : null;
        }
        set => this[nameof(PublishedQuoteLevel)] = value.ToString();
    }

    public decimal? RoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(RoundingPrecision)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(RoundingPrecision)] = value?.ToString();
    }

    public decimal? MinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MinSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MinSubmitSize)] = value?.ToString();
    }

    public decimal? MaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(MaxSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(MaxSubmitSize)] = value?.ToString();
    }

    public decimal? IncrementSize
    {
        get
        {
            var checkValue = this[nameof(IncrementSize)];
            return checkValue != null ? decimal.Parse(checkValue) : null;
        }
        set => this[nameof(IncrementSize)] = value?.ToString();
    }

    public ushort? MinimumQuoteLife
    {
        get
        {
            var checkValue = this[nameof(MinimumQuoteLife)];
            return checkValue != null ? ushort.Parse(checkValue) : null;
        }
        set => this[nameof(MinimumQuoteLife)] = value?.ToString();
    }

    public LayerFlags? LayerFlags
    {
        get
        {
            var checkValue = this[nameof(LayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : null;
        }
        set => this[nameof(LayerFlags)] = value.ToString();
    }

    public byte? MaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(MaximumPublishedLayers)];
            return checkValue != null ? byte.Parse(checkValue) : null;
        }
        set => this[nameof(MaximumPublishedLayers)] = value?.ToString();
    }

    public LastTradedFlags? LastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(LastTradedFlags)];
            return checkValue != null ? Enum.Parse<LastTradedFlags>(checkValue) : null;
        }
        set => this[nameof(LastTradedFlags)] = value.ToString();
    }

    public bool AreEquivalent(ITickerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var tickerIdSame = TickerId == other.TickerId;
        var tickerSame = Ticker == other.Ticker;
        var quoteLevelSame = PublishedQuoteLevel == other.PublishedQuoteLevel;
        var availabilitySame = TickerAvailability == other.TickerAvailability;
        var roundingSame = RoundingPrecision == other.RoundingPrecision;
        var minSubitSizeSame = MinSubmitSize == other.MinSubmitSize;
        var maxSubitSizeSame = MaxSubmitSize == other.MaxSubmitSize;
        var incrementSizeSame = IncrementSize == other.IncrementSize;
        var minQuoteLifeSizeSame = MinimumQuoteLife == other.MinimumQuoteLife;
        var layerFlagsSizeSame = LayerFlags == other.LayerFlags;
        var maxLayersSame = MaximumPublishedLayers == other.MaximumPublishedLayers;
        var lastTradedFlagsSame = LastTradedFlags == other.LastTradedFlags;

        return tickerIdSame && tickerSame && availabilitySame && quoteLevelSame && roundingSame && minSubitSizeSame && maxSubitSizeSame &&
               incrementSizeSame && minQuoteLifeSizeSame && layerFlagsSizeSame && maxLayersSame && lastTradedFlagsSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(TickerId)] = null;
        root[path + ":" + nameof(Ticker)] = null;
        root[path + ":" + nameof(TickerAvailability)] = null;
        root[path + ":" + nameof(PublishedQuoteLevel)] = null;
        root[path + ":" + nameof(RoundingPrecision)] = null;
        root[path + ":" + nameof(MinSubmitSize)] = null;
        root[path + ":" + nameof(MaxSubmitSize)] = null;
        root[path + ":" + nameof(IncrementSize)] = null;
        root[path + ":" + nameof(MinimumQuoteLife)] = null;
        root[path + ":" + nameof(LayerFlags)] = null;
        root[path + ":" + nameof(MaximumPublishedLayers)] = null;
        root[path + ":" + nameof(LastTradedFlags)] = null;
    }

    protected bool Equals(ITickerConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ITickerConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(TickerId);
        hashCode.Add(Ticker);
        hashCode.Add(TickerAvailability);
        hashCode.Add(PublishedQuoteLevel);
        hashCode.Add(RoundingPrecision);
        hashCode.Add(MinSubmitSize);
        hashCode.Add(MaxSubmitSize);
        hashCode.Add(IncrementSize);
        hashCode.Add(MinimumQuoteLife);
        hashCode.Add(LayerFlags);
        hashCode.Add(MaximumPublishedLayers);
        hashCode.Add(LastTradedFlags);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(TickerConfig)}({nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker}, " +
        $"{nameof(TickerAvailability)}: {TickerAvailability}, {nameof(PublishedQuoteLevel)}: {PublishedQuoteLevel}, " +
        $"{nameof(RoundingPrecision)}: {RoundingPrecision}, {nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags}, {nameof(Path)}: {Path})";
}
