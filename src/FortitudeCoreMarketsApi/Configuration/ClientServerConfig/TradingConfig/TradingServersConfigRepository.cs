using System.Collections.Generic;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig
{
    public class TradingServersConfigRepository : MarketServerConfigRepository<ITradingServerConfig>
    {
        public TradingServersConfigRepository(IEnumerable<ITradingServerConfig> serverConfigs) : base(serverConfigs)
        {
        }
    }
}