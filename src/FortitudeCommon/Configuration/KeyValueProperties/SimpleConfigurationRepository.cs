using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.EventProcessing;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public class SimpleConfigurationRepository : IConfigurationRepository
    {
        private readonly SortedDictionary<string, MainKeyValue> configItemsDictionary = 
                                                                        new SortedDictionary<string, MainKeyValue>();
        private readonly ISubject<ConfigItemEvent> configItemEvent = new Subject<ConfigItemEvent>();

        public SimpleConfigurationRepository(string categoryName)
        {
            CategoryName = categoryName;
        }
        
        public SimpleConfigurationRepository(string categoryName, List<ConfigItem> configItems) : this(categoryName)
        {
            LoadConfigItems(configItems);
        }

        private IEnumerable<IConfigItemValue> ConfigItemValues()
        {
            foreach (MainKeyValue mainKeyValue in configItemsDictionary.Values)
            {
                yield return mainKeyValue;
                if (mainKeyValue.SubKeys == null) continue;
                foreach (var subKeyValue in mainKeyValue.SubKeys.Values)
                {
                    yield return subKeyValue;
                    if (subKeyValue.SubKeyModifers == null) continue;

                    foreach (var subKeyModifierValue in subKeyValue.SubKeyModifers.Values)
                    {
                        yield return subKeyModifierValue;
                    }
                }
            }
        }

        private void LoadConfigItems(IEnumerable<IConfigItem> loadConfigItems)
        {
            foreach (var configItem in loadConfigItems)
            {
                if (configItem is ConfigItem configI)
                {
                    configI.Category = CategoryName;
                }
                MainKeyValue mainKeyValue = GetMainKeyValue(true, configItem.CompositeKey.Key);
                if (mainKeyValue == null)
                {
                    continue;
                }
                SubKeyValue subKeyValue = GetSubKeyValue(true, mainKeyValue, configItem.CompositeKey.SubKey);
                if (subKeyValue == null)
                {
                    mainKeyValue.ConfigItemValue = configItem;
                    continue;
                }
                SubKeyModifierValue subKeyModifierValue = GetSubKeyModifierValue(
                                                        true, subKeyValue, configItem.CompositeKey.SubKeyModifier);
                if (subKeyModifierValue == null)
                {
                    subKeyValue.ConfigItemValue = configItem;
                    continue;
                }
                subKeyModifierValue.ConfigItemValue = configItem;
            }
        }

        private MainKeyValue GetMainKeyValue(bool create, string key)
        {
            if (key == null) return null;
            if (configItemsDictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            if (!create) return null;
            value = new MainKeyValue(key, null);
            configItemsDictionary.Add(key, value);
            return value;
        }

        private SubKeyValue GetSubKeyValue(bool create, MainKeyValue mainKeyValue, string subKey)
        {
            if (subKey == null || mainKeyValue == null) return null;
            if (mainKeyValue.SubKeys == null)
            {
                mainKeyValue.SubKeys = new SortedDictionary<string, SubKeyValue>();
            }
            if (mainKeyValue.SubKeys.TryGetValue(subKey, out var value))
            {
                return value;
            }
            if (!create) return null;
            value = new SubKeyValue(mainKeyValue.LookupKey + "." + subKey, null);
            mainKeyValue.SubKeys.Add(subKey, value);
            return value;
        }

        private SubKeyModifierValue GetSubKeyModifierValue(bool create, SubKeyValue subKeyValue, string subKeyModifier)
        {
            if (subKeyModifier == null || subKeyValue == null) return null;
            if (subKeyValue.SubKeyModifers.TryGetValue(subKeyModifier, out var value))
            {
                return value;
            }
            if (!create) return null;
            value = new SubKeyModifierValue(subKeyValue.LookupKey + "." + subKeyModifier, null);
            subKeyValue.SubKeyModifers.Add(subKeyModifier, value);
            return value;
        }

        public string CategoryName { get; }
        
        public IConfigItem GetConfigItem(string key)
        {
            var getMainKeyValue = GetMainKeyValue(false, key);
            return getMainKeyValue?.ConfigItemValue;
        }

        public IConfigItem GetConfigItem(CompositeKey compositeKey)
        {
            var getMainKeyValue = GetMainKeyValue(false, compositeKey.Key);
            if (compositeKey.SubKey == null)
            {
                return getMainKeyValue?.ConfigItemValue;
            }
            var getSubKeyValue = GetSubKeyValue(false, getMainKeyValue, compositeKey.SubKey);
            if (compositeKey.SubKeyModifier == null)
            {
                return getSubKeyValue?.ConfigItemValue;
            }
            var getSubKeyModifierValue = GetSubKeyModifierValue(false, getSubKeyValue, compositeKey.SubKey);
            return getSubKeyModifierValue?.ConfigItemValue;
        }

        public IEnumerable<IConfigItem> GetConfigItems(string matchKey)
        {
            return GetConfigItems(ci => ci.CompositeKey.MatchesQueryKey(new CompositeKey(matchKey)));
        }

        public IEnumerable<IConfigItem> GetConfigItems(CompositeKey matchCompositeKey)
        {
            return GetConfigItems(ci => ci.CompositeKey.MatchesQueryKey(matchCompositeKey));
        }

        public IEnumerable<IConfigItem> GetConfigItems(Func<IConfigItem, bool> filterPredicate)
        {
            return ConfigItemValues().Select(civ => civ.ConfigItemValue)
                    .Where(filterPredicate);
        }

        public IObservable<ConfigItemEvent> WatchConfig(string key)
        {
            return WatchConfig(ci => ci.CompositeKey.MatchesQueryKey(new CompositeKey(key)));
        }

        public IObservable<ConfigItemEvent> WatchConfig(CompositeKey compositeKey)
        {
            return WatchConfig(ci => ci.CompositeKey.MatchesQueryKey(compositeKey));
        }

        public IObservable<ConfigItemEvent> WatchConfig(Func<IConfigItem, bool> predicate)
        {
            return Observable.Create<ConfigItemEvent>(observer =>
            {
                var mostRecentItems = new ReplaySubject<ConfigItemEvent>();
                var currentReplaySubjectSubscription = configItemEvent
                    .Subscribe(configEvent => mostRecentItems.OnNext(configEvent));
                foreach (var configItemValue in ConfigItemValues()
                    .Where(configItemValue => predicate(configItemValue.ConfigItemValue)))
                {
                    observer.OnNext(new ConfigItemEvent(EventType.Retrieved, configItemValue.ConfigItemValue));
                }
                var observerReplaySubjectSubscription = mostRecentItems.Subscribe(observer.OnNext);
                currentReplaySubjectSubscription.Dispose();
                var observerCurrentSubscription = configItemEvent.Subscribe(observer.OnNext);
                observerReplaySubjectSubscription.Dispose();
                return () =>
                {
                    observerCurrentSubscription.Dispose();
                };
            });
        }

        public bool ContainsKey(CompositeKey compositeKey)
        {
            return GetConfigItem(compositeKey) != null;
        }

        public bool ContainsItem(IConfigItem checkItem)
        {
            return ContainsKey(checkItem.CompositeKey);
        }

        public bool Update(IConfigItem item)
        {
            if (!ContainsItem(item))
            {
                return false;
            }
            return Upsert(item, EventType.Updated);
        }

        private bool Upsert(IConfigItem item, EventType eventType)
        {
            var compositeKey = item.CompositeKey;
            var getMainKeyValue = GetMainKeyValue(false, compositeKey.Key);
            if (compositeKey.SubKey == null)
            {
                if (getMainKeyValue == null) return false;
                getMainKeyValue.ConfigItemValue = item;
                configItemEvent.OnNext(new ConfigItemEvent(eventType, item));
                return true;
            }
            var getSubKeyValue = GetSubKeyValue(false, getMainKeyValue, compositeKey.SubKey);
            if (compositeKey.SubKeyModifier == null)
            {
                if (getSubKeyValue == null) return false;
                getSubKeyValue.ConfigItemValue = item;
                configItemEvent.OnNext(new ConfigItemEvent(eventType, item));
                return true;
            }
            var getSubKeyModifierValue = GetSubKeyModifierValue(false, getSubKeyValue, compositeKey.SubKey);
            if (getSubKeyModifierValue == null) return false;
            getSubKeyModifierValue.ConfigItemValue = item;
            configItemEvent.OnNext(new ConfigItemEvent(eventType, item));
            return true;
        }

        public bool Add(IConfigItem item)
        {
            if (ContainsItem(item))
            {
                return false;
            }
            return Upsert(item, EventType.Created);
        }

        public bool AddOrUpdate(IConfigItem item)
        {
            return Update(item) || Add(item);
        }

        public bool Delete(IConfigItem item)
        {
            if (!ContainsItem(item))
            {
                return false;
            }
            var compositeKey = item.CompositeKey;
            var getMainKeyValue = GetMainKeyValue(false, compositeKey.Key);
            IConfigItem deletedItem;
            if (compositeKey.SubKey == null)
            {
                if (getMainKeyValue == null) return false;
                deletedItem = getMainKeyValue.ConfigItemValue;
                getMainKeyValue.ConfigItemValue = null;
                configItemEvent.OnNext(new ConfigItemEvent(EventType.Deleted, deletedItem));
                return true;
            }
            var getSubKeyValue = GetSubKeyValue(false, getMainKeyValue, compositeKey.SubKey);
            if (compositeKey.SubKeyModifier == null)
            {
                if (getSubKeyValue == null) return false;
                deletedItem = getSubKeyValue.ConfigItemValue;
                getSubKeyValue.ConfigItemValue = item;
                configItemEvent.OnNext(new ConfigItemEvent(EventType.Deleted, deletedItem));
                return true;
            }
            var getSubKeyModifierValue = GetSubKeyModifierValue(false, getSubKeyValue, compositeKey.SubKey);
            if (getSubKeyModifierValue == null) return false;
            deletedItem = getSubKeyModifierValue.ConfigItemValue;
            getSubKeyModifierValue.ConfigItemValue = item;
            configItemEvent.OnNext(new ConfigItemEvent(EventType.Deleted, deletedItem));
            return true;
        }
    }
}
