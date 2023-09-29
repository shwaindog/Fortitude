using System;
using System.Collections.Generic;
using FortitudeCommon.Configuration.Availability;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.TradingConfig
{
    public class TradingServerConfig : ITradingServerConfig
    {
        public TradingServerConfig(long id = 0L, string name = null, 
            MarketServerType marketServerType = MarketServerType.Trading,
            IEnumerable<IConnectionConfig> serverConnections = null,
            ITimeTable availabilityTimeTable = null, 
            IObservable<IMarketServerConfigUpdate<ITradingServerConfig>> updates = null,
            OrderType supportedOrderTypes = OrderType.Unset,
            TimeInForce supportedTimeInForce = TimeInForce.None,
            VenueFeatures supportedVenueFeatures = VenueFeatures.None,
            bool supportsAmends = false,
            ISnapshotUpdatePricingServerConfig supportsMarketPriceQuoteExecution = null)
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

        public TradingServerConfig(TradingServerConfig toClone)
        {
            Id = toClone.Id;
            Name = toClone.Name;
            MarketServerType = toClone.MarketServerType;
            ServerConnections = toClone.ServerConnections;
            AvailabilityTimeTable = toClone.AvailabilityTimeTable.Clone();
            Updates = toClone.Updates;
            SupportedOrderTypes = toClone.SupportedOrderTypes;
            SupportedTimeInForce = toClone.SupportedTimeInForce;
            SupportedVenueFeatures = toClone.SupportedVenueFeatures;
            SupportsMarketPriceQuoteExecution = toClone.SupportsMarketPriceQuoteExecution;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public IMarketServerConfig<ITradingServerConfig> Clone()
        {
            return new TradingServerConfig(this);
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public MarketServerType MarketServerType { get; set; }
        public IEnumerable<IConnectionConfig> ServerConnections { get; set; }
        public ITimeTable AvailabilityTimeTable { get; set; }
        public IObservable<IMarketServerConfigUpdate<ITradingServerConfig>> Updates { get; set; }
        public OrderType SupportedOrderTypes { get; set; }
        public TimeInForce SupportedTimeInForce { get; set; }
        public VenueFeatures SupportedVenueFeatures { get; set; }
        public ISnapshotUpdatePricingServerConfig SupportsMarketPriceQuoteExecution { get; set; }
    }
}