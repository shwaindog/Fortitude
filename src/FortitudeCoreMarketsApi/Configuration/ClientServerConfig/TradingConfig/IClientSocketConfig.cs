using System;
using FortitudeMarketsApi.Configuration.ClientServerConfig;

namespace FortitudeIO.Sockets.Config
{
    public interface IClientSocketConfig : IMarketServerConfig<IClientSocketConfig>
    {
        uint ReconnectIntervalMs { get; }

        event Action OnConfigUpdate;
    }
}