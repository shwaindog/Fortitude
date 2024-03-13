#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public class MarketServerConfigRepository<T> : IMarketServerConfigRepository<T>, IMuteableRepository<T>
    where T : class, IMarketServerConfig<T>
{
    private readonly Dictionary<long, T> currentConfigs;
    private readonly Subject<IMarketServerConfigUpdate<T>> updateStream = new();

    public MarketServerConfigRepository() => currentConfigs = [];

    public MarketServerConfigRepository(IEnumerable<T> serverConfigs)
    {
        currentConfigs = serverConfigs.ToDictionary(item => item.Id);
    }

    public IEnumerable<IMarketServerConfig<T>> ServerConfigs => currentConfigs.Values;

    public IEnumerable<T> CurrentConfigs => currentConfigs.Values;

    public IObservable<IMarketServerConfigUpdate<T>> ServerConfigUpdateStream
    {
        get
        {
            return Observable.Create<IMarketServerConfigUpdate<T>>(observer =>
            {
                foreach (var currentServerConfig in CurrentConfigs)
                    observer.OnNext(new MarketServerConfigUpdate<T>(currentServerConfig, EventType.Retrieved));
                return updateStream.Subscribe(observer.OnNext);
            });
        }
    }

    public T? Find(string name)
    {
        return currentConfigs.Select(kvp => kvp.Value)
            .FirstOrDefault(sci => sci.Name?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }

    public bool ContainsItem(T checkItem) => currentConfigs.ContainsKey(checkItem.Id);

    public bool Update(T item)
    {
        var containsItem = ContainsItem(item);
        if (!containsItem) return false; // doesn't exist
        var currentItem = currentConfigs[item.Id];
        if (item.Equals(currentItem)) return false; //same values
        currentConfigs[item.Id] = item;
        updateStream.OnNext(new MarketServerConfigUpdate<T>(item, EventType.Updated));
        return true;
    }

    public bool Add(T item)
    {
        var containsItem = ContainsItem(item);
        if (containsItem) return false;
        currentConfigs.Add(item.Id, item);
        item.Updates = updateStream;
        updateStream.OnNext(new MarketServerConfigUpdate<T>(item, EventType.Created));
        return true;
    }

    public bool AddOrUpdate(T item) => Update(item) || Add(item);

    public bool Delete(T item)
    {
        var itemIndex = currentConfigs.ContainsKey(item.Id);
        if (!itemIndex) return false;
        currentConfigs.Remove(item.Id);
        updateStream.OnNext(new MarketServerConfigUpdate<T>(item, EventType.Deleted));
        return true;
    }
}
