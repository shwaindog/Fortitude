namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public class PricingClientConfigRepository : MarketServerConfigRepository<ISnapshotUpdatePricingServerConfig>
    , IPricingClientConfigRepository
{
    public PricingClientConfigRepository(IEnumerable<ISnapshotUpdatePricingServerConfig> serverConfigs)
        : base(serverConfigs) { }

    public IPricingServersConfigRepository ToPricingServerConfigRepository()
    {
        return new PricingServersConfigRepository(CurrentConfigs
            .Select(supsc => supsc.Clone(true)));
    }
}

public class PricingServersConfigRepository : MarketServerConfigRepository<ISnapshotUpdatePricingServerConfig>,
    IPricingServersConfigRepository
{
    public PricingServersConfigRepository(IEnumerable<ISnapshotUpdatePricingServerConfig> serverConfigs)
        : base(serverConfigs) { }

    public IPricingClientConfigRepository ToPricingClientConfigRepository()
    {
        return new PricingClientConfigRepository(CurrentConfigs
            .Select(supsc => supsc.Clone(true)));
    }
}
