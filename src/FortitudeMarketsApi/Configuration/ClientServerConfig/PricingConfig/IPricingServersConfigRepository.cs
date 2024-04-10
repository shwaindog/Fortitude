namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface IPricingConnectionsConfigRepository : IMarketConnectionConfigRepository
{
    IPricingClientConfigRepository ToPricingClientConfigRepository();
}

public interface IPricingClientConfigRepository : IMarketConnectionConfigRepository
{
    IPricingConnectionsConfigRepository ToPricingServerConfigRepository();
}
