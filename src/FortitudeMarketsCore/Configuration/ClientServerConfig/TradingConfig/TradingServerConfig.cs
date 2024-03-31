#region

using FortitudeCommon.Configuration.Availability;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.TradingConfig;

public class TradingServerConfig : ITradingServerConfig
{
    public TradingServerConfig(long id = 0L, string? name = null,
        MarketServerType marketServerType = MarketServerType.Trading,
        IEnumerable<INetworkTopicConnectionConfig>? serverConnections = null,
        ITimeTable? availabilityTimeTable = null,
        IObservable<IMarketServerConfigUpdate<ITradingServerConfig>>? updates = null,
        OrderType supportedOrderTypes = OrderType.Unset,
        TimeInForce supportedTimeInForce = TimeInForce.None,
        VenueFeatures supportedVenueFeatures = VenueFeatures.None,
        bool supportsAmends = false,
        ISnapshotUpdatePricingServerConfig? supportsMarketPriceQuoteExecution = null)
    {
        Id = id;
        Name = name;
        MarketServerType = marketServerType;
        ServerConnections = serverConnections;
        AvailabilityTimeTable = availabilityTimeTable;
        Updates = updates;
        SupportedOrderTypes = supportedOrderTypes;
        SupportedTimeInForce = supportedTimeInForce;
        SupportedVenueFeatures = supportedVenueFeatures;
        SupportsMarketPriceQuoteExecution = supportsMarketPriceQuoteExecution;
    }

    public TradingServerConfig(TradingServerConfig toClone, bool toggleProtocolDirection)
    {
        Id = toClone.Id;
        Name = toClone.Name;
        MarketServerType = toClone.MarketServerType;
        ServerConnections = toClone.ServerConnections?.Select(stcc =>
                                toggleProtocolDirection ? stcc.ToggleProtocolDirection() : stcc.Clone()) ??
                            Enumerable.Empty<INetworkTopicConnectionConfig>();
        AvailabilityTimeTable = toClone.AvailabilityTimeTable?.Clone();
        Updates = toClone.Updates;
        SupportedOrderTypes = toClone.SupportedOrderTypes;
        SupportedTimeInForce = toClone.SupportedTimeInForce;
        SupportedVenueFeatures = toClone.SupportedVenueFeatures;
        SupportsMarketPriceQuoteExecution = toClone.SupportsMarketPriceQuoteExecution;
    }

    object ICloneable.Clone() => Clone();

    public IMarketServerConfig<ITradingServerConfig> Clone() => new TradingServerConfig(this, false);

    IMarketServerConfig IMarketServerConfig.ToggleProtocolDirection() => new TradingServerConfig(this, true);

    public ITradingServerConfig ToggleProtocolDirection() => new TradingServerConfig(this, true);

    public long Id { get; set; }
    public string? Name { get; set; }
    public MarketServerType MarketServerType { get; set; }
    public IEnumerable<INetworkTopicConnectionConfig>? ServerConnections { get; set; }
    public ITimeTable? AvailabilityTimeTable { get; set; }
    public IObservable<IMarketServerConfigUpdate<ITradingServerConfig>>? Updates { get; set; }

    public OrderType SupportedOrderTypes { get; set; }
    public TimeInForce SupportedTimeInForce { get; set; }
    public VenueFeatures SupportedVenueFeatures { get; set; }
    public ISnapshotUpdatePricingServerConfig? SupportsMarketPriceQuoteExecution { get; set; }
}
