#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;

public interface ITradingServerConfig : IMarketServerConfig<ITradingServerConfig>
{
    OrderType SupportedOrderTypes { get; }
    TimeInForce SupportedTimeInForce { get; }
    VenueFeatures SupportedVenueFeatures { get; }
    ISnapshotUpdatePricingServerConfig? SupportsMarketPriceQuoteExecution { get; }
}
