// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config.Availability;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Config.TradingConfig;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config;

public interface IMarketConnectionConfig : IConnection, ICloneable<IMarketConnectionConfig>, IInterfacesComparable<IMarketConnectionConfig>
  , IStringBearer
{
    public const ushort DefaultEmptySourceIdValue   = 0;
    public const string DefaultEmptySourceNameValue = "";

    ushort SourceId { get; set; }
    string SourceName { get; set; }

    ITimeTableConfig? VenueOperatingTimeTableConfig { get; set; }

    MarketConnectionType MarketConnectionType { get; set; }
    ISourceTickersConfig? SourceTickerConfig { get; set; }
    IPricingServerConfig? PricingServerConfig { get; set; }
    ITradingServerConfig? TradingServerConfig { get; set; }
    CountryCityCodes MyLocation { get; set; }

    IEnumerable<ISourceTickerInfo> AllSourceTickerInfos { get; }
    IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos { get; }
    IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos { get; }

    IMarketConnectionConfig ShiftPortsBy(ushort deltaPorts);

    ISourceTickerInfo? GetSourceTickerInfo(string ticker);
    IMarketConnectionConfig ToggleProtocolDirection(string connectionName);
}

public class MarketConnectionConfig : ConfigSection, IMarketConnectionConfig
{
    private const string PricingConnectionNameSuffix = "Pricing";
    private const string TradingConnectionNameSuffix = "Trading";

    public MarketConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MarketConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MarketConnectionConfig
    (ushort sourceId, string sourceName, MarketConnectionType marketConnectionType, CountryCityCodes myLocation
      , ISourceTickersConfig sourceTickersConfig
      , IPricingServerConfig? pricingServerConfig = null, ITradingServerConfig? tradingServerConfig = null) : this()
    {
        SourceId   = sourceId;
        SourceName = sourceName;
        MyLocation = myLocation;

        ConnectionName = sourceName;

        MarketConnectionType = marketConnectionType;
        SourceTickerConfig   = sourceTickersConfig;
        PricingServerConfig  = pricingServerConfig;
        TradingServerConfig  = tradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        SourceName = toClone.SourceName;
        if (toClone is MarketConnectionConfig marketConnectionConfig)
        {
            ConnectionName           = marketConnectionConfig[nameof(ConnectionName)];
            ParentConnectionName     = marketConnectionConfig.ParentConnectionName;
            this[nameof(MyLocation)] = marketConnectionConfig[nameof(MyLocation)];
            ParentLocation           = marketConnectionConfig.ParentLocation;
        }
        else
        {
            ConnectionName = toClone.ConnectionName;
            MyLocation     = toClone.MyLocation;
        }

        SourceId = toClone.SourceId;

        MarketConnectionType = toClone.MarketConnectionType;
        SourceTickerConfig   = toClone.SourceTickerConfig;
        PricingServerConfig  = toClone.PricingServerConfig;
        TradingServerConfig  = toClone.TradingServerConfig;
    }

    public MarketConnectionConfig(IMarketConnectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)] ?? ParentConnectionName ?? this[nameof(SourceName)];

        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            if (PricingServerConfig != null)
            {
                if (PricingServerConfig is PricingServerConfig pricingServerConfig)
                {
                    pricingServerConfig.ParentConnectionName = value + PricingConnectionNameSuffix;
                }
                else if (PricingServerConfig.ConnectionName.IsNullOrEmpty())
                {
                    PricingServerConfig.ConnectionName = value + PricingConnectionNameSuffix;
                }
            }
            if (TradingServerConfig != null)
            {
                if (PricingServerConfig is PricingServerConfig pricingServerConfig)
                {
                    pricingServerConfig.ParentConnectionName = value + TradingConnectionNameSuffix;
                }
                else if (TradingServerConfig.ConnectionName.IsNullOrEmpty())
                {
                    TradingServerConfig.ConnectionName = value + TradingConnectionNameSuffix;
                }
            }
        }
    }

    public string? ParentConnectionName { get; set; }

    public ushort SourceId
    {
        get => ushort.Parse(this[nameof(SourceId)] ?? $"{IMarketConnectionConfig.DefaultEmptySourceIdValue}");
        set => this[nameof(SourceId)] = value.ToString();
    }

    public string SourceName
    {
        get => this[nameof(SourceName)] ?? ParentConnectionName ?? IMarketConnectionConfig.DefaultEmptySourceNameValue;
        set => this[nameof(SourceName)] = value;
    }

    public CountryCityCodes MyLocation
    {
        get
        {
            var checkValue = this[nameof(MyLocation)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<CountryCityCodes>(checkValue) : ParentLocation ?? CountryCityCodes.Unknown;
        }
        set => this[nameof(MyLocation)] = value.ToString();
    }

    public CountryCityCodes? ParentLocation { get; set; }

    public ITimeTableConfig? VenueOperatingTimeTableConfig
    {
        get
        {
            if (GetSection(nameof(VenueOperatingTimeTableConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new TimeTableConfig(ConfigRoot, $"{Path}{Split}{nameof(VenueOperatingTimeTableConfig)}");
            }
            return null;
        }
        set => _ = value != null ? new TimeTableConfig(value, ConfigRoot, $"{Path}{Split}{nameof(VenueOperatingTimeTableConfig)}") : null;
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
            if (GetSection(nameof(SourceTickerConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var sourceTickersConfig = new SourceTickersConfig(ConfigRoot, $"{Path}{Split}{nameof(SourceTickerConfig)}")
                {
                    ParentVenueOperatingTimeTableConfig = VenueOperatingTimeTableConfig
                };
                return sourceTickersConfig;
            }
            return null;
        }
        set
        {
            if (value is SourceTickersConfig valueSourceTickersConfig)
            {
                valueSourceTickersConfig.ParentVenueOperatingTimeTableConfig = VenueOperatingTimeTableConfig;
            }
            _ = value != null ? new SourceTickersConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SourceTickerConfig)}") : null;
        }
    }

    public IPricingServerConfig? PricingServerConfig
    {
        get
        {
            if (GetSection(nameof(PricingServerConfig)).GetChildren()
                                                       .SelectMany(cs => cs.GetChildren()).Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var pricingServerConfig = new PricingServerConfig(ConfigRoot, $"{Path}{Split}{nameof(PricingServerConfig)}")
                {
                    ParentConnectionName = ConnectionName + PricingConnectionNameSuffix
                };
                return pricingServerConfig;
            }
            return null;
        }
        set
        {
            if (value is PricingServerConfig valuePricingServerConfig)
            {
                valuePricingServerConfig.ParentConnectionName = ConnectionName + PricingConnectionNameSuffix;
            }
            else if (value?.ConnectionName.IsNullOrEmpty() ?? false)
            {
                value.ConnectionName = ConnectionName + PricingConnectionNameSuffix;
            }
            _ = value != null
                ? new PricingServerConfig(value, ConfigRoot, $"{Path}{Split}{nameof(PricingServerConfig)}")
                : null;
        }
    }

    public ITradingServerConfig? TradingServerConfig
    {
        get
        {
            if (GetSection(nameof(TradingServerConfig)).GetChildren().Any())
            {
                var tradingServerConfig = new TradingServerConfig(ConfigRoot, $"{Path}{Split}{nameof(TradingServerConfig)}")
                {
                    ParentConnectionName = ConnectionName + TradingConnectionNameSuffix
                };
                return tradingServerConfig;
            }
            return null;
        }
        set
        {
            if (value?.TradingServerConnectionConfig.ConversationProtocol == SocketConversationProtocol.Unknown)
            {
                value.TradingServerConnectionConfig.ConversationProtocol = SocketConversationProtocol.TcpAcceptor;
            }

            if (value is TradingServerConfig valueTradingServerConfig)
            {
                valueTradingServerConfig.ParentConnectionName = ConnectionName + TradingConnectionNameSuffix;
            }
            else if (value?.ConnectionName.IsNullOrEmpty() ?? false)
            {
                value.ConnectionName = ConnectionName + TradingConnectionNameSuffix;
            }
            _ = value != null
                ? new TradingServerConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TradingServerConfig)}")
                : null;
        }
    }

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos =>
        SourceTickerConfig?.AllSourceTickerInfos
            (SourceId, SourceName, MyLocation, PricingServerConfig?.IsPricePublisher ?? TradingServerConfig?.IsOrderAcceptor ?? false) ?? [];

    public IEnumerable<ISourceTickerInfo> PricingEnabledSourceTickerInfos =>
        SourceTickerConfig?.PricingEnabledSourceTickerInfos(SourceId, SourceName, MyLocation, PricingServerConfig!.IsPricePublisher) ?? [];

    public IEnumerable<ISourceTickerInfo> TradingEnabledSourceTickerInfos =>
        SourceTickerConfig?.TradingEnabledSourceTickerInfos(SourceId, SourceName, MyLocation, TradingServerConfig?.IsOrderAcceptor ?? false) ?? [];

    IMarketConnectionConfig IMarketConnectionConfig.ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedMarketConnectionConfig = new MarketConnectionConfig(this)
        {
            PricingServerConfig = PricingServerConfig?.ShiftPortsBy(deltaPorts), TradingServerConfig = TradingServerConfig?.ShiftPortsBy(deltaPorts)
        };
        return shiftedMarketConnectionConfig;
    }

    public ISourceTickerInfo? GetSourceTickerInfo(string ticker) =>
        SourceTickerConfig?.GetSourceTickerInfo(SourceId, SourceName, ticker, MyLocation
                                              , (PricingServerConfig?.IsPricePublisher ?? false) || (TradingServerConfig?.IsOrderAcceptor ?? false));

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
        root[$"{path}{Split}{nameof(ConnectionName)}"]       = null;
        root[$"{path}{Split}{nameof(SourceName)}"]           = null;
        root[$"{path}{Split}{nameof(SourceId)}"]             = "0";
        root[$"{path}{Split}{nameof(MarketConnectionType)}"] = "0";
        root[$"{path}{Split}{nameof(SourceTickerConfig)}"]   = null;
        root[$"{path}{Split}{nameof(PricingServerConfig)}"]  = null;
        root[$"{path}{Split}{nameof(TradingServerConfig)}"]  = null;
    }

    protected bool Equals(IMarketConnectionConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMarketConnectionConfig, true);

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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(ConnectionName), ConnectionName)
            .Field.AlwaysAdd(nameof(SourceId), SourceId)
            .Field.AlwaysAdd(nameof(SourceName), SourceName)
            .Field.AlwaysAdd(nameof(MarketConnectionType), MarketConnectionType)
            .Field.AlwaysAdd(nameof(SourceTickerConfig), SourceTickerConfig)
            .Field.AlwaysAdd(nameof(PricingServerConfig), PricingServerConfig)
            .Field.AlwaysAdd(nameof(TradingServerConfig), TradingServerConfig)
            .Complete();

    public override string ToString() =>
        $"{nameof(MarketConnectionConfig)}({nameof(ConnectionName)}: {ConnectionName}, {nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(MarketConnectionType)}: {MarketConnectionType}, {nameof(SourceTickerConfig)}: {SourceTickerConfig}, " +
        $"{nameof(PricingServerConfig)}: {PricingServerConfig}, {nameof(TradingServerConfig)}: {TradingServerConfig}, " +
        $"{nameof(Path)}: {Path})";
}
