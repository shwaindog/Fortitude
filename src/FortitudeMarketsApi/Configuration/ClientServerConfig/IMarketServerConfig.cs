#region

using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketServerConfig
{
    long Id { get; }
    string? Name { get; }
    MarketServerType MarketServerType { get; }
    IEnumerable<IConnectionConfig>? ServerConnections { get; }
    ITimeTable? AvailabilityTimeTable { get; }
}

public interface IMarketServerConfig<T> : ICloneable<IMarketServerConfig<T>>, IMarketServerConfig
    where T : class, IMarketServerConfig<T>
{
    IObservable<IMarketServerConfigUpdate<T>>? Updates { get; set; }
}
