namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface IPricingServersConfigRepository : IMarketServerConfigRepository<ISnapshotUpdatePricingServerConfig>
{
    IPricingClientConfigRepository ToPricingClientConfigRepository();
}

public interface IPricingClientConfigRepository : IMarketServerConfigRepository<ISnapshotUpdatePricingServerConfig>
{
    IPricingServersConfigRepository ToPricingServerConfigRepository();
}
