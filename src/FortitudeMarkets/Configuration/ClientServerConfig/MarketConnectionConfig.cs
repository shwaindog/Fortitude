// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig;

public interface IMarketConnectionConfig : IConnection, ICloneable<IMarketConnectionConfig>, IInterfacesComparable<IMarketConnectionConfig>
{
    public const ushort DefaultEmptySourceIdValue   = 0;
    public const string DefaultEmptySourceNameValue = "";

    ushort SourceId   { get; set; }
    string SourceName { get; set; }

    MarketConnectionType  MarketConnectionType { get; set; }
    ISourceTickersConfig? SourceTickerConfig   { get; set; }
    IPricingServerConfig? PricingServerConfig  { get; set; }
    ITradingServerConfig? TradingServerConfig  { get; set; }

    IEnumerable<ISourceTickerInfo> AllSourceTickerInfos            { get; }
    IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos { get; }
    IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos { get; }

    IMarketConnectionConfig ShiftPortsBy(ushort deltaPorts);

    ISourceTickerInfo?      GetSourceTickerInfo(string ticker);
    IMarketConnectionConfig ToggleProtocolDirection(string connectionName);
}

public class MarketConnectionConfig : ConfigSection, IMarketConnectionConfig
{
    private IPricingServerConfig? lastPricingServerConfig;
    private ITradingServerConfig? lastTradingServerConfig;
    private ISourceTickersConfig? sourceTickerPublicationConfigs;
    public MarketConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public MarketConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MarketConnectionConfig
    (ushort sourceId, string sourceName, MarketConnectionType marketConnectionType, ISourceTickersConfig sourceTickersConfig
      , IPricingServerConfig? pricingServerConfig = null, ITradingServerConfig? tradingServerConfig = null) : this()
    {
        SourceId   = sourceId;
        SourceName = sourceName;

        MarketConnectionType = marketConnectionType;
        SourceTickerConfig   = sourceTickersConfig;
        PricingServerConfig  = pricingServerConfig;
        TradingServerConfig  = tradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ConnectionName = ConnectionName;

        SourceId   = toClone.SourceId;
        SourceName = toClone.SourceName;

        MarketConnectionType = toClone.MarketConnectionType;
        SourceTickerConfig   = toClone.SourceTickerConfig;
        PricingServerConfig  = toClone.PricingServerConfig;
        TradingServerConfig  = toClone.TradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConnectionName
    {
        get => this[nameof(SourceName)]!;

        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            if (PricingServerConfig != null) PricingServerConfig.ConnectionName = value + "Pricing";
            if (TradingServerConfig != null) TradingServerConfig.ConnectionName = value + "Trading";
        }
    }

    public ushort SourceId
    {
        get => ushort.Parse(this[nameof(SourceId)] ?? $"{IMarketConnectionConfig.DefaultEmptySourceIdValue}");
        set => this[nameof(SourceId)] = value.ToString();
    }

    public string SourceName
    {
        get => this[nameof(SourceName)] ?? IMarketConnectionConfig.DefaultEmptySourceNameValue;
        set => this[nameof(SourceName)] = value;
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
        set =>
            lastPricingServerConfig = value != null
                ? new PricingServerConfig(value, ConfigRoot, Path + ":" + nameof(PricingServerConfig))
                {
                    ConnectionName = ConnectionName
                }
                : null;
    }

    public ITradingServerConfig? TradingServerConfig
    {
        get
        {
            if (GetSection(nameof(TradingServerConfig)).GetChildren().Any())
                return lastTradingServerConfig = new TradingServerConfig(ConfigRoot, Path + ":" + nameof(TradingServerConfig));
            return null;
        }
        set =>
            lastTradingServerConfig = value != null
                ? new TradingServerConfig(value, ConfigRoot, Path + ":" + nameof(TradingServerConfig))
                {
                    ConnectionName = ConnectionName
                }
                : null;
    }

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos =>
        SourceTickerConfig?.AllSourceTickerInfos(SourceId, SourceName) ?? Enumerable.Empty<ISourceTickerInfo>();

    public IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos =>
        SourceTickerConfig?.PricingEnabledSourceTickerInfos(SourceId, SourceName) ?? Enumerable.Empty<ISourceTickerInfo>();

    public IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos =>
        SourceTickerConfig?.TradingEnabledSourceTickerInfos(SourceId, SourceName) ?? Enumerable.Empty<ISourceTickerInfo>();

    IMarketConnectionConfig IMarketConnectionConfig.ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedMarketConnectionConfig = new MarketConnectionConfig(this)
        {
            PricingServerConfig = PricingServerConfig?.ShiftPortsBy(deltaPorts), TradingServerConfig = TradingServerConfig?.ShiftPortsBy(deltaPorts)
        };
        return shiftedMarketConnectionConfig;
    }

    public ISourceTickerInfo? GetSourceTickerInfo(string ticker) => SourceTickerConfig?.GetSourceTickerInfo(SourceId, SourceName, ticker);

    public IMarketConnectionConfig Clone() => new MarketConnectionConfig(this);

    object ICloneable.Clone() => Clone();

    public IMarketConnectionConfig ToggleProtocolDirection(string connectionName) =>
        new MarketConnectionConfig(this)
        {
            ConnectionName      = connectionName, PricingServerConfig = PricingServerConfig?.ToggleProtocolDirection()
          , TradingServerConfig = TradingServerConfig?.ToggleProtocolDirection()
        };

    public bool AreEquivalent(IMarketConnectionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var connectionNameSame = ConnectionName == other.ConnectionName;
        var idSame             = SourceId == other.SourceId;
        var nameSame           = string.Equals(SourceName, other.SourceName);
        var serverTypeSame     = MarketConnectionType == other.MarketConnectionType;

        var sourceTickerConfigSame = SourceTickerConfig?.AreEquivalent(other.SourceTickerConfig, exactTypes) ?? other.SourceTickerConfig == null;

        var pricingServersSame = PricingServerConfig?.AreEquivalent(other.PricingServerConfig, exactTypes) ?? other.PricingServerConfig == null;
        var tradingServersSame = TradingServerConfig?.AreEquivalent(other.TradingServerConfig, exactTypes) ?? other.TradingServerConfig == null;

        return connectionNameSame && idSame && nameSame && serverTypeSame && sourceTickerConfigSame && pricingServersSame && tradingServersSame;
    }

    public MarketConnectionConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedMarketConnectionConfig = new MarketConnectionConfig(this)
        {
            PricingServerConfig = PricingServerConfig?.ShiftPortsBy(deltaPorts), TradingServerConfig = TradingServerConfig?.ShiftPortsBy(deltaPorts)
        };
        return shiftedMarketConnectionConfig;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(ConnectionName)]       = null;
        root[path + ":" + nameof(SourceName)]           = null;
        root[path + ":" + nameof(SourceId)]             = "0";
        root[path + ":" + nameof(MarketConnectionType)] = "0";
        root[path + ":" + nameof(SourceTickerConfig)]   = null;
        root[path + ":" + nameof(PricingServerConfig)]  = null;
        root[path + ":" + nameof(TradingServerConfig)]  = null;
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
            hashCode = (hashCode * 397) ^ SourceName.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)MarketConnectionType;
            hashCode = (hashCode * 397) ^ SourceId.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(MarketConnectionConfig)}({nameof(ConnectionName)}: {ConnectionName}, {nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(MarketConnectionType)}: {MarketConnectionType}, {nameof(SourceTickerConfig)}: {SourceTickerConfig}, " +
        $"{nameof(PricingServerConfig)}: {PricingServerConfig}, {nameof(TradingServerConfig)}: {TradingServerConfig}, " +
        $"{nameof(Path)}: {Path})";
}
