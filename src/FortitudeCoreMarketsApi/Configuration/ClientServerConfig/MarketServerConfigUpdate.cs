#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public class MarketServerConfigUpdate<T> : IMarketServerConfigUpdate<T> where T : class, IMarketServerConfig<T>
{
    public MarketServerConfigUpdate(T? serverConfig, EventType eventType)
    {
        ServerConfig = serverConfig;
        EventType = eventType;
    }

    public T? ServerConfig { get; }
    public EventType EventType { get; }
}
