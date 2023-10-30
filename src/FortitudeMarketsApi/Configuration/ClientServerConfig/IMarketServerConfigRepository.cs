namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketServerConfigRepository<out T> where T : class, IMarketServerConfig<T>
{
    IEnumerable<T> CurrentConfigs { get; }
    IObservable<IMarketServerConfigUpdate<T>> ServerConfigUpdateStream { get; }
    T? Find(string name);
}
