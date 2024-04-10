#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public enum TickerAvailability
{
    AllEnabled
    , AllDisabled
    , Pricing
    , Trading
}

public interface ISourceTickersConfig : IInterfacesComparable<ISourceTickersConfig>
{
    TickerAvailability DefaultTickerAvailability { get; set; }
    decimal DefaultRoundingPrecision { get; set; }
    decimal DefaultMinSubmitSize { get; set; }
    decimal DefaultMaxSubmitSize { get; set; }
    decimal DefaultIncrementSize { get; set; }
    ushort DefaultMinimumQuoteLife { get; set; }
    LayerFlags DefaultLayerFlags { get; set; }
    byte DefaultMaximumPublishedLayers { get; set; }
    LastTradedFlags DefaultLastTradedFlags { get; set; }
    public List<ITickerConfig> Tickers { get; set; }
    ISourceTickerQuoteInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker);
    IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos(ushort sourceId, string sourceName);
}

public class SourceTickersConfig : ConfigSection, ISourceTickersConfig
{
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private readonly List<ITickerConfig> lastestTickerConfigs = new();
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
    public SourceTickersConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public SourceTickersConfig() { }

    public SourceTickersConfig(params ITickerConfig[] tickersConfigs) => Tickers = tickersConfigs.ToList();

    public SourceTickersConfig(ISourceTickersConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DefaultTickerAvailability = toClone.DefaultTickerAvailability;
        DefaultRoundingPrecision = toClone.DefaultRoundingPrecision;
        DefaultMinSubmitSize = toClone.DefaultMinSubmitSize;
        DefaultMaxSubmitSize = toClone.DefaultMaxSubmitSize;
        DefaultIncrementSize = toClone.DefaultIncrementSize;
        DefaultMinimumQuoteLife = toClone.DefaultMinimumQuoteLife;
        DefaultLayerFlags = toClone.DefaultLayerFlags;
        DefaultMaximumPublishedLayers = toClone.DefaultMaximumPublishedLayers;
        DefaultLastTradedFlags = toClone.DefaultLastTradedFlags;
        Tickers = toClone.Tickers;
    }

    public SourceTickersConfig(ISourceTickersConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TickerAvailability DefaultTickerAvailability
    {
        get
        {
            var checkValue = this[nameof(DefaultTickerAvailability)];
            return checkValue != null ? Enum.Parse<TickerAvailability>(checkValue) : TickerAvailability.AllEnabled;
        }
        set => this[nameof(DefaultTickerAvailability)] = value.ToString();
    }

    public decimal DefaultRoundingPrecision
    {
        get
        {
            var checkValue = this[nameof(DefaultRoundingPrecision)];
            return checkValue != null ? decimal.Parse(checkValue) : 0.000001m;
        }
        set => this[nameof(DefaultRoundingPrecision)] = value.ToString();
    }

    public decimal DefaultMinSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMinSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : decimal.One;
        }
        set => this[nameof(DefaultMinSubmitSize)] = value.ToString();
    }

    public decimal DefaultMaxSubmitSize
    {
        get
        {
            var checkValue = this[nameof(DefaultMaxSubmitSize)];
            return checkValue != null ? decimal.Parse(checkValue) : 1_000_000m;
        }
        set => this[nameof(DefaultMaxSubmitSize)] = value.ToString();
    }

    public decimal DefaultIncrementSize
    {
        get
        {
            var checkValue = this[nameof(DefaultIncrementSize)];
            return checkValue != null ? decimal.Parse(checkValue) : 1m;
        }
        set => this[nameof(DefaultIncrementSize)] = value.ToString();
    }

    public ushort DefaultMinimumQuoteLife
    {
        get
        {
            var checkValue = this[nameof(DefaultMinimumQuoteLife)];
            return checkValue != null ? ushort.Parse(checkValue) : (ushort)0;
        }
        set => this[nameof(DefaultMinimumQuoteLife)] = value.ToString();
    }

    public LayerFlags DefaultLayerFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLayerFlags)];
            return checkValue != null ? Enum.Parse<LayerFlags>(checkValue) : LayerFlags.Price | LayerFlags.Volume | LayerFlags.Executable;
        }
        set => this[nameof(DefaultLayerFlags)] = value.ToString();
    }

    public byte DefaultMaximumPublishedLayers
    {
        get
        {
            var checkValue = this[nameof(DefaultMaximumPublishedLayers)];
            return checkValue != null ? byte.Parse(checkValue) : (byte)1;
        }
        set => this[nameof(DefaultMaximumPublishedLayers)] = value.ToString();
    }

    public LastTradedFlags DefaultLastTradedFlags
    {
        get
        {
            var checkValue = this[nameof(DefaultLastTradedFlags)];
            return checkValue != null ? Enum.Parse<LastTradedFlags>(checkValue) : LastTradedFlags.None;
        }
        set => this[nameof(DefaultLastTradedFlags)] = value.ToString();
    }

    public List<ITickerConfig> Tickers
    {
        get
        {
            if (lastestTickerConfigs.Any()) return lastestTickerConfigs;
            foreach (var serviceName in GetSection(nameof(Tickers)).GetChildren())
                lastestTickerConfigs.Add(new TickerConfig(ConfigRoot, serviceName.Path));
            return lastestTickerConfigs;
        }
        set
        {
            lastestTickerConfigs.Clear();
            for (var i = 0; i < value.Count; i++)
                lastestTickerConfigs.Add(new TickerConfig(value[i], ConfigRoot, Path + ":" + nameof(Tickers) + $":{i}"));
        }
    }


    public ISourceTickerQuoteInfo? GetSourceTickerInfo(ushort sourceId, string sourceName, string ticker)
    {
        var tickerConfig = Tickers.FirstOrDefault(tc => tc.Ticker == ticker);
        if (tickerConfig == null) return null;
        var sourceTickerINfo = new SourceTickerQuoteInfo(sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
            , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers,
            tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision,
            tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize, tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize
            , tickerConfig.IncrementSize ?? DefaultIncrementSize, tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife
            ,
            tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
        return sourceTickerINfo;
    }

    public IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos(ushort sourceId, string sourceName)
    {
        foreach (var tickerConfig in Tickers)
        {
            var sourceTickerINfo = new SourceTickerQuoteInfo(sourceId, sourceName, tickerConfig.TickerId, tickerConfig.Ticker
                , tickerConfig.MaximumPublishedLayers ?? DefaultMaximumPublishedLayers,
                tickerConfig.RoundingPrecision ?? DefaultRoundingPrecision,
                tickerConfig.MinSubmitSize ?? DefaultMinSubmitSize, tickerConfig.MaxSubmitSize ?? DefaultMaxSubmitSize
                , tickerConfig.IncrementSize ?? DefaultIncrementSize
                , tickerConfig.MinimumQuoteLife ?? DefaultMinimumQuoteLife,
                tickerConfig.LayerFlags ?? DefaultLayerFlags, tickerConfig.LastTradedFlags ?? DefaultLastTradedFlags);
            yield return sourceTickerINfo;
        }
    }

    public bool AreEquivalent(ISourceTickersConfig? other, bool exactTypes = false)
    {
        var availabilitySame = DefaultTickerAvailability == other.DefaultTickerAvailability;
        var roundingSame = DefaultRoundingPrecision == other.DefaultRoundingPrecision;
        var minSubitSizeSame = DefaultMinSubmitSize == other.DefaultMinSubmitSize;
        var maxSubitSizeSame = DefaultMaxSubmitSize == other.DefaultMaxSubmitSize;
        var incrementSizeSame = DefaultIncrementSize == other.DefaultIncrementSize;
        var minQuoteLifeSizeSame = DefaultMinimumQuoteLife == other.DefaultMinimumQuoteLife;
        var layerFlagsSizeSame = DefaultLayerFlags == other.DefaultLayerFlags;
        var maxLayersSame = DefaultMaximumPublishedLayers == other.DefaultMaximumPublishedLayers;
        var lastTradedFlagsSame = DefaultLastTradedFlags == other.DefaultLastTradedFlags;
        var tickerConfigsSame = Tickers.SequenceEqual(other.Tickers);

        return availabilitySame && roundingSame && minSubitSizeSame && maxSubitSizeSame && incrementSizeSame &&
               minQuoteLifeSizeSame && layerFlagsSizeSame && maxLayersSame && lastTradedFlagsSame && tickerConfigsSame;
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
        hashCode.Add(DefaultTickerAvailability);
        hashCode.Add(DefaultRoundingPrecision);
        hashCode.Add(DefaultMinSubmitSize);
        hashCode.Add(DefaultMaxSubmitSize);
        hashCode.Add(DefaultIncrementSize);
        hashCode.Add(DefaultMinimumQuoteLife);
        hashCode.Add(DefaultLayerFlags);
        hashCode.Add(DefaultMaximumPublishedLayers);
        hashCode.Add(DefaultLastTradedFlags);
        hashCode.Add(Tickers);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(SourceTickersConfig)}({nameof(DefaultTickerAvailability)}: {DefaultTickerAvailability}," +
        $" {nameof(DefaultRoundingPrecision)}: {DefaultRoundingPrecision}, {nameof(DefaultMinSubmitSize)}: {DefaultMinSubmitSize}, " +
        $"{nameof(DefaultMaxSubmitSize)}: {DefaultMaxSubmitSize}, {nameof(DefaultIncrementSize)}: {DefaultIncrementSize}, " +
        $"{nameof(DefaultMinimumQuoteLife)}: {DefaultMinimumQuoteLife}, {nameof(DefaultLayerFlags)}: {DefaultLayerFlags}, " +
        $"{nameof(DefaultMaximumPublishedLayers)}: {DefaultMaximumPublishedLayers}, " +
        $"{nameof(DefaultLastTradedFlags)}: {DefaultLastTradedFlags}, {nameof(Tickers)}: {Tickers}, {nameof(Path)}: {Path})";
}

public interface ITickerConfig : IInterfacesComparable<ITickerConfig>
{
    ushort TickerId { get; set; }
    string Ticker { get; set; }
    TickerAvailability? TickerAvailability { get; set; }
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

    public TickerConfig(ushort tickerId, string ticker, TickerAvailability? tickerAvailability = null, decimal? roundingPrecision = null,
        decimal? minSubmitSize = null, decimal? maxSubmitSize = null, decimal? incrementSize = null, ushort? minimumQuoteLife = null
        , LayerFlags? layerFlags = null, byte? maximumPublisherLayers = null, LastTradedFlags? lastTradedFlags = null)
    {
        TickerId = tickerId;
        Ticker = ticker;
        TickerAvailability = tickerAvailability;
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
        var availabilitySame = TickerAvailability == other.TickerAvailability;
        var roundingSame = RoundingPrecision == other.RoundingPrecision;
        var minSubitSizeSame = MinSubmitSize == other.MinSubmitSize;
        var maxSubitSizeSame = MaxSubmitSize == other.MaxSubmitSize;
        var incrementSizeSame = IncrementSize == other.IncrementSize;
        var minQuoteLifeSizeSame = MinimumQuoteLife == other.MinimumQuoteLife;
        var layerFlagsSizeSame = LayerFlags == other.LayerFlags;
        var maxLayersSame = MaximumPublishedLayers == other.MaximumPublishedLayers;
        var lastTradedFlagsSame = LastTradedFlags == other.LastTradedFlags;

        return tickerIdSame && tickerSame && availabilitySame && roundingSame && minSubitSizeSame && maxSubitSizeSame && incrementSizeSame &&
               minQuoteLifeSizeSame && layerFlagsSizeSame && maxLayersSame && lastTradedFlagsSame;
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
        $"{nameof(TickerAvailability)}: {TickerAvailability}, {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags}, {nameof(Path)}: {Path})";
}
