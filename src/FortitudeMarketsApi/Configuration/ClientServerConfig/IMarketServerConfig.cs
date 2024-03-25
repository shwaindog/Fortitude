#region

using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketServerConfig
{
    long Id { get; }
    string? Name { get; }
    MarketServerType MarketServerType { get; }
    IEnumerable<ISocketTopicConnectionConfig>? ServerConnections { get; }
    ITimeTable? AvailabilityTimeTable { get; }
    IMarketServerConfig Clone(bool switchToMatchingConnection);
}

public interface IMarketServerConfig<T> : ICloneable<IMarketServerConfig<T>>, IMarketServerConfig
    where T : class, IMarketServerConfig<T>
{
    IObservable<IMarketServerConfigUpdate<T>>? Updates { get; set; }
    new T Clone(bool switchToMatchingConnection);
}
