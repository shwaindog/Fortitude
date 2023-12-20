namespace FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;

public interface IClientSocketConfig : IMarketServerConfig<IClientSocketConfig>
{
    uint ReconnectIntervalMs { get; }

    event Action OnConfigUpdate;
}
