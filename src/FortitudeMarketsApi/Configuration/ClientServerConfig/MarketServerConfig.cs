#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketConnectionConfig : ICloneable<IMarketConnectionConfig>, IInterfacesComparable<IMarketConnectionConfig>
{
    ushort SourceId { get; set; }
    string Name { get; set; }
    MarketConnectionType MarketConnectionType { get; set; }
    ISourceTickersConfig? SourceTickerConfig { get; set; }
    IPricingServerConfig? PricingServerConfig { get; set; }
    ITradingServerConfig? TradingServerConfig { get; set; }

    ISourceTickerQuoteInfo? GetSourceTickerInfo(string ticker);
    IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos();
    IMarketConnectionConfig ToggleProtocolDirection();
}

public class MarketConnectionConfig : ConfigSection, IMarketConnectionConfig
{
    private IPricingServerConfig? lastPricingServerConfig;
    private ITradingServerConfig? lastTradingServerConfig;
    private ISourceTickersConfig? sourceTickerPublicationConfigs;
    public MarketConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public MarketConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }


    public MarketConnectionConfig(ushort sourceId, string name, MarketConnectionType marketConnectionType, ISourceTickersConfig sourceTickersConfig
        , IPricingServerConfig? pricingServerConfig = null, ITradingServerConfig? tradingServerConfig = null) : this()
    {
        SourceId = sourceId;
        Name = name;
        MarketConnectionType = marketConnectionType;
        SourceTickerConfig = sourceTickersConfig;
        PricingServerConfig = pricingServerConfig;
        TradingServerConfig = tradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        SourceId = toClone.SourceId;
        Name = toClone.Name;
        MarketConnectionType = toClone.MarketConnectionType;
        SourceTickerConfig = toClone.SourceTickerConfig;
        PricingServerConfig = toClone.PricingServerConfig;
        TradingServerConfig = toClone.TradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort SourceId
    {
        get => ushort.Parse(this[nameof(SourceId)]!);
        set => this[nameof(SourceId)] = value.ToString();
    }

    public string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    public MarketConnectionType MarketConnectionType
    {
        get => Enum.Parse<MarketConnectionType>(this[nameof(MarketConnectionType)]!);
        set => this[nameof(MarketConnectionType)] = value.ToString();
    }

    public ISourceTickersConfig? SourceTickerConfig
    {
        get
        {
            if (GetSection(nameof(SourceTickerConfig)).GetChildren().Any())
                return sourceTickerPublicationConfigs ??= new SourceTickersConfig(ConfigRoot, Path + ":" + nameof(SourceTickerConfig));
            return null;
        }
        set =>
            sourceTickerPublicationConfigs
                = value != null ? new SourceTickersConfig(value, ConfigRoot, Path + ":" + nameof(SourceTickerConfig)) : null;
    }

    public IPricingServerConfig? PricingServerConfig
    {
        get
        {
            if (GetSection(nameof(PricingServerConfig)).GetChildren().Any())
                return lastPricingServerConfig = new PricingServerConfig(ConfigRoot, Path + ":" + nameof(PricingServerConfig));
            return null;
        }
        set => lastPricingServerConfig = value != null ? new PricingServerConfig(value, ConfigRoot, Path + ":" + nameof(PricingServerConfig)) : null;
    }

    public ITradingServerConfig? TradingServerConfig
    {
        get
        {
            if (GetSection(nameof(TradingServerConfig)).GetChildren().Any())
                return lastTradingServerConfig = new TradingServerConfig(ConfigRoot, Path + ":" + nameof(TradingServerConfig));
            return null;
        }
        set => lastTradingServerConfig = value != null ? new TradingServerConfig(value, ConfigRoot, Path + ":" + nameof(TradingServerConfig)) : null;
    }

    public ISourceTickerQuoteInfo? GetSourceTickerInfo(string ticker) => SourceTickerConfig?.GetSourceTickerInfo(SourceId, Name, ticker);

    public IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos() =>
        SourceTickerConfig?.AllSourceTickerInfos(SourceId, Name) ?? Enumerable.Empty<ISourceTickerQuoteInfo>();

    public IMarketConnectionConfig Clone() => new MarketConnectionConfig(this);

    object ICloneable.Clone() => Clone();

    public IMarketConnectionConfig ToggleProtocolDirection() =>
        new MarketConnectionConfig(this)
        {
            PricingServerConfig = PricingServerConfig?.ToggleProtocolDirection(), TradingServerConfig = TradingServerConfig?.ToggleProtocolDirection()
        };

    public bool AreEquivalent(IMarketConnectionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var idSame = SourceId == other.SourceId;
        var nameSame = string.Equals(Name, other.Name);
        var serverTypeSame = MarketConnectionType == other.MarketConnectionType;
        var sourceTickerConfigSame = SourceTickerConfig?.AreEquivalent(other.SourceTickerConfig, exactTypes) ?? other.SourceTickerConfig == null;
        var pricingServersSame = PricingServerConfig?.AreEquivalent(other.PricingServerConfig, exactTypes) ?? other.PricingServerConfig == null;
        var tradingServersSame = TradingServerConfig?.AreEquivalent(other.TradingServerConfig, exactTypes) ?? other.TradingServerConfig == null;

        return idSame && nameSame && serverTypeSame && sourceTickerConfigSame && pricingServersSame && tradingServersSame;
    }

    protected bool Equals(IMarketConnectionConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IMarketConnectionConfig)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)MarketConnectionType;
            hashCode = (hashCode * 397) ^ SourceId.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(MarketConnectionConfig)}({nameof(SourceId)}: {SourceId}, {nameof(Name)}: {Name}, " +
        $"{nameof(MarketConnectionType)}: {MarketConnectionType}, {nameof(SourceTickerConfig)}: {SourceTickerConfig}, " +
        $"{nameof(PricingServerConfig)}: {PricingServerConfig}, {nameof(TradingServerConfig)}: {TradingServerConfig}, " +
        $"{nameof(Path)}: {Path})";
}
