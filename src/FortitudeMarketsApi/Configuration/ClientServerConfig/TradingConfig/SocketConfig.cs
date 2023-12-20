namespace FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;

public interface IOrxSocketConfig : IClientSocketConfig
{
    bool IocSupported { get; }
    bool FokSupported { get; }
    bool IceSupported { get; }
    bool AmendSupported { get; }
    bool PreTradeSupported { get; }
    bool PreTradeAsFinal { get; }
}
