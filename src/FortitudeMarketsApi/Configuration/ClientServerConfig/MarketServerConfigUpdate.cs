using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig
{
    public class MarketServerConfigUpdate<T> : IMarketServerConfigUpdate<T> where T : class, IMarketServerConfig<T>
    {
        public T ServerConfig { get; private set; }
        public EventType EventType { get; private set; }

        public MarketServerConfigUpdate(T serverConfig, EventType eventType)
        {
            ServerConfig = serverConfig;
            EventType = eventType;
        }
    }
}