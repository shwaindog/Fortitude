#region

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Trading.Orders;
using Microsoft.Extensions.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeMarkets.Trading.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Config.TradingConfig;

public interface ITradingServerConfig : IInterfacesComparable<ITradingServerConfig>, IConnection, IStringBearer
{
    INetworkTopicConnectionConfig TradingServerConnectionConfig { get; set; }
    OrderType SupportedOrderTypes { get; set; }
    TimeInForce SupportedTimeInForce { get; set; }
    VenueFeatures SupportedVenueFeatures { get; set; }

    bool IsOrderAcceptor { get; }

    ITradingServerConfig ShiftPortsBy(ushort deltaPorts);
    ITradingServerConfig ToggleProtocolDirection();
}

public class TradingServerConfig : ConfigSection, ITradingServerConfig
{
    public TradingServerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public TradingServerConfig() { }

    public TradingServerConfig(INetworkTopicConnectionConfig tradingServerConnectionConfig,
        OrderType supportedOrderTypes = OrderType.Unknown,
        TimeInForce supportedTimeInForce = TimeInForce.None,
        VenueFeatures supportedVenueFeatures = VenueFeatures.None)
    {
        TradingServerConnectionConfig = tradingServerConnectionConfig;
        SupportedOrderTypes = supportedOrderTypes;
        SupportedTimeInForce = supportedTimeInForce;
        SupportedVenueFeatures = supportedVenueFeatures;
    }

    public TradingServerConfig(ITradingServerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        if (toClone is TradingServerConfig tradingServerConfig)
        {
            ConnectionName       = tradingServerConfig[nameof(ConnectionName)];
            ParentConnectionName = tradingServerConfig.ParentConnectionName;
        }
        else
        {
            ConnectionName = toClone.ConnectionName;
        }
        TradingServerConnectionConfig = toClone.TradingServerConnectionConfig.Clone();
        SupportedOrderTypes = toClone.SupportedOrderTypes;
        SupportedTimeInForce = toClone.SupportedTimeInForce;
        SupportedVenueFeatures = toClone.SupportedVenueFeatures;
    }

    public TradingServerConfig(ITradingServerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)] ?? ParentConnectionName;
        set
        {
            this[nameof(ConnectionName)] = value;
            var tradingServerConnectionConfig = TradingServerConnectionConfig;
            if (tradingServerConnectionConfig != null!)
            {
                if (tradingServerConnectionConfig is NetworkTopicConnectionConfig tradingNetworkConnConfig)
                {
                    tradingNetworkConnConfig.ParentConnectionName = value;
                } 
                else if (tradingServerConnectionConfig.ConnectionName.IsNullOrEmpty())
                {
                    tradingServerConnectionConfig.ConnectionName = value;
                }
            }
        }
    }
    public string? ParentConnectionName { get; set; }

    public INetworkTopicConnectionConfig TradingServerConnectionConfig
    {
        get
        {
            if (GetSection(nameof(TradingServerConnectionConfig)).GetChildren().Any())
            {
                var tradingConn = new NetworkTopicConnectionConfig(ConfigRoot, $"{Path}{Split}{nameof(TradingServerConnectionConfig)}")
                    {
                        ParentConnectionName = ConnectionName
                    };
                return tradingConn;
            }
            return null!;
        }
        set
        {
            _ = new NetworkTopicConnectionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TradingServerConnectionConfig)}");
            if (value is NetworkTopicConnectionConfig networkTopicConnectionConfig)
            {
                networkTopicConnectionConfig.ParentConnectionName = ConnectionName;
            }
            else
            {
                value.ConnectionName = ConnectionName;
            }
        }
    }

    public bool IsOrderAcceptor => TradingServerConnectionConfig.ConversationProtocol == SocketConversationProtocol.TcpAcceptor;

    public OrderType SupportedOrderTypes
    {
        get
        {
            var checkValue = this[nameof(SupportedOrderTypes)];
            return checkValue != null ? Enum.Parse<OrderType>(checkValue) : OrderType.Market;
        }
        set => this[nameof(SupportedOrderTypes)] = value.ToString();
    }

    public TimeInForce SupportedTimeInForce
    {
        get
        {
            var checkValue = this[nameof(SupportedTimeInForce)];
            return checkValue != null ? Enum.Parse<TimeInForce>(checkValue) : TimeInForce.GoodTillCancelled;
        }
        set => this[nameof(SupportedTimeInForce)] = value.ToString();
    }

    public VenueFeatures SupportedVenueFeatures
    {
        get
        {
            var checkValue = this[nameof(SupportedVenueFeatures)];
            return checkValue != null ? Enum.Parse<VenueFeatures>(checkValue) : VenueFeatures.None;
        }
        set => this[nameof(SupportedVenueFeatures)] = value.ToString();
    }

    public ITradingServerConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedTradingServerConfig = new TradingServerConfig(this)
        {
            TradingServerConnectionConfig = TradingServerConnectionConfig.ShiftPortsBy(deltaPorts)
        };
        return shiftedTradingServerConfig;
    }

    public ITradingServerConfig ToggleProtocolDirection() =>
        new TradingServerConfig(this)
        {
            TradingServerConnectionConfig = TradingServerConnectionConfig.ToggleProtocolDirection()
        };

    public bool AreEquivalent(ITradingServerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var connectionNameSame = ConnectionName == other.ConnectionName;
        var tradingConnSame = Equals(TradingServerConnectionConfig, other.TradingServerConnectionConfig);
        var supportedOrderTypesSame = SupportedOrderTypes == other.SupportedOrderTypes;
        var supportedTimeInForceSame = SupportedTimeInForce == other.SupportedTimeInForce;
        var supportedVenueFeaturesSame = SupportedVenueFeatures == other.SupportedVenueFeatures;

        return connectionNameSame && tradingConnSame && supportedOrderTypesSame && supportedTimeInForceSame && supportedVenueFeaturesSame;
    }

    public ITradingServerConfig Clone() => new TradingServerConfig(this);

    protected bool Equals(TradingServerConfig other) => throw new NotImplementedException();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return AreEquivalent((ITradingServerConfig)obj, true);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(ConnectionName);
        hashCode.Add(TradingServerConnectionConfig);
        hashCode.Add(SupportedOrderTypes);
        hashCode.Add(SupportedTimeInForce);
        hashCode.Add(SupportedVenueFeatures);
        return hashCode.ToHashCode();
    }

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(ConnectionName), ConnectionName)
            .Field.AlwaysAdd(nameof(ParentConnectionName), ParentConnectionName)
            .Field.AlwaysAdd(nameof(TradingServerConnectionConfig), TradingServerConnectionConfig)
            .Field.AlwaysAdd(nameof(SupportedOrderTypes), SupportedOrderTypes)
            .Field.AlwaysAdd(nameof(SupportedTimeInForce), SupportedTimeInForce)
            .Field.AlwaysAdd(nameof(SupportedVenueFeatures), SupportedVenueFeatures)
            .Complete();
}
