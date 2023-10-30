#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketServerConfigUpdate<out T> where T : class, IMarketServerConfig<T>
{
    T? ServerConfig { get; }
    EventType EventType { get; }
}
