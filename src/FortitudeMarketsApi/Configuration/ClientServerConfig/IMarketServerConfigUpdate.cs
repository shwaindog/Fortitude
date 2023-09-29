using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig
{
    public interface IMarketServerConfigUpdate<out T> where T : class, IMarketServerConfig<T>
    {
        T ServerConfig { get; }
        EventType EventType { get; }
    }
}