using System;
using System.Collections.Generic;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public interface IConfigurationRepository : IMuteableRepository<IConfigItem>
    {
        string CategoryName { get; }
        IConfigItem GetConfigItem(string key);
        IConfigItem GetConfigItem(CompositeKey compositeKey);
        IEnumerable<IConfigItem> GetConfigItems(string matchKey);
        IEnumerable<IConfigItem> GetConfigItems(CompositeKey matchCompositeKey);
        IEnumerable<IConfigItem> GetConfigItems(Func<IConfigItem, bool> filterPredicate);
        IObservable<ConfigItemEvent> WatchConfig(string key);
        IObservable<ConfigItemEvent> WatchConfig(CompositeKey compositeKey);
        IObservable<ConfigItemEvent> WatchConfig(Func<IConfigItem, bool> filterPredicate);
        bool ContainsKey(CompositeKey compositeKey);
    }
}
