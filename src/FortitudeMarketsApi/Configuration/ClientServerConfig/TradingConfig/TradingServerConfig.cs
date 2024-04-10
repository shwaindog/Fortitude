#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;

public interface ITradingServerConfig : IInterfacesComparable<ITradingServerConfig>
{
    INetworkTopicConnectionConfig TradingServerConnectionConfig { get; set; }
    OrderType SupportedOrderTypes { get; set; }
    TimeInForce SupportedTimeInForce { get; set; }
    VenueFeatures SupportedVenueFeatures { get; set; }
    ITradingServerConfig ToggleProtocolDirection();
}

public class TradingServerConfig : ConfigSection, ITradingServerConfig
{
    private INetworkTopicConnectionConfig? lastTradingConnectionConfig;
    public TradingServerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public TradingServerConfig() { }

    public TradingServerConfig(INetworkTopicConnectionConfig tradingServerConnectionConfig,
        OrderType supportedOrderTypes = OrderType.Unset,
        TimeInForce supportedTimeInForce = TimeInForce.None,
        VenueFeatures supportedVenueFeatures = VenueFeatures.None)
    {
        TradingServerConnectionConfig = tradingServerConnectionConfig;
        SupportedOrderTypes = supportedOrderTypes;
        SupportedTimeInForce = supportedTimeInForce;
        SupportedVenueFeatures = supportedVenueFeatures;
    }

    public TradingServerConfig(ITradingServerConfig toClone, IConfigurationRoot root, string path)
    {
        TradingServerConnectionConfig = toClone.TradingServerConnectionConfig.Clone();
        SupportedOrderTypes = toClone.SupportedOrderTypes;
        SupportedTimeInForce = toClone.SupportedTimeInForce;
        SupportedVenueFeatures = toClone.SupportedVenueFeatures;
    }

    public TradingServerConfig(ITradingServerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig TradingServerConnectionConfig
    {
        get
        {
            if (GetSection(nameof(TradingServerConnectionConfig)).GetChildren().Any())
                return lastTradingConnectionConfig
                    ??= new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(TradingServerConnectionConfig));
            throw new Exception("Expected Trading Server Network config to be set");
        }
        set => lastTradingConnectionConfig = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(TradingServerConnectionConfig));
    }

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

    public ITradingServerConfig ToggleProtocolDirection() =>
        new TradingServerConfig(this)
        {
            TradingServerConnectionConfig = TradingServerConnectionConfig.ToggleProtocolDirection()
        };

    public bool AreEquivalent(ITradingServerConfig? other, bool exactTypes = false)
    {
        var tradingConnSame = Equals(TradingServerConnectionConfig, other.TradingServerConnectionConfig);
        var supportedOrderTypesSame = SupportedOrderTypes == other.SupportedOrderTypes;
        var supportedTimeInForceSame = SupportedTimeInForce == other.SupportedTimeInForce;
        var supportedVenueFeaturesSame = SupportedVenueFeatures == other.SupportedVenueFeatures;

        return tradingConnSame && supportedOrderTypesSame && supportedTimeInForceSame && supportedVenueFeaturesSame;
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
        hashCode.Add(TradingServerConnectionConfig);
        hashCode.Add(SupportedOrderTypes);
        hashCode.Add(SupportedTimeInForce);
        hashCode.Add(SupportedVenueFeatures);
        return hashCode.ToHashCode();
    }
}
