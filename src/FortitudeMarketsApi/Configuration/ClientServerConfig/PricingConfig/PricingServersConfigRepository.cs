using System.Collections.Generic;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig
{
    public class PricingServersConfigRepository : MarketServerConfigRepository<ISnapshotUpdatePricingServerConfig>, 
        IPricingServersConfigRepository
    {
        public PricingServersConfigRepository(IEnumerable<ISnapshotUpdatePricingServerConfig> serverConfigs)
            : base(serverConfigs)
        {
        }
    }
}