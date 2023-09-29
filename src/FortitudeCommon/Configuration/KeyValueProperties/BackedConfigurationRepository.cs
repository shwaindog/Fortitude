using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public class BackedConfigurationRepository : IConfigurationRepository
    {
        private readonly IConfigurationRepository backingConfigurationRepository;
        private readonly IConfigurationRepository primaryConfigurationRepository;

        public BackedConfigurationRepository(IConfigurationRepository primaryConfigurationRepository,
            IConfigurationRepository backingConfigurationRepository)
        {
            CategoryName = primaryConfigurationRepository.CategoryName;
            this.primaryConfigurationRepository = primaryConfigurationRepository;
            this.backingConfigurationRepository = backingConfigurationRepository;
        }

        public string CategoryName { private set; get; }

        public IConfigItem GetConfigItem(string key)
        {
            var result = primaryConfigurationRepository.GetConfigItem(key);
            if (result == null)
            {
                return backingConfigurationRepository.GetConfigItem(key);
            }
            return result;
        }

        public IConfigItem GetConfigItem(CompositeKey compositeKey)
        {
            var result = primaryConfigurationRepository.GetConfigItem(compositeKey);
            if (result == null)
            {
                return backingConfigurationRepository.GetConfigItem(compositeKey);
            }
            return result;
        }

        public IEnumerable<IConfigItem> GetConfigItems(string matchKey)
        {
            var result = primaryConfigurationRepository.GetConfigItems(matchKey);
            if (result == null)
            {
                return backingConfigurationRepository.GetConfigItems(matchKey);
            }
            return result;
        }

        public IEnumerable<IConfigItem> GetConfigItems(CompositeKey matchCompositeKey)
        {
            return backingConfigurationRepository.GetConfigItems(matchCompositeKey).Where(ci => !primaryConfigurationRepository.ContainsItem(ci))
                .Concat(primaryConfigurationRepository.GetConfigItems(matchCompositeKey));
        }

        public IEnumerable<IConfigItem> GetConfigItems(Func<IConfigItem, bool> filterPredicate)
        {
            return backingConfigurationRepository.GetConfigItems(filterPredicate).Where(ci => !primaryConfigurationRepository.ContainsItem(ci))
                .Concat(primaryConfigurationRepository.GetConfigItems(filterPredicate));
        }

        public IObservable<ConfigItemEvent> WatchConfig(string key)
        {
            var backedConfig = backingConfigurationRepository.WatchConfig(key).Where(bconfig =>
                !primaryConfigurationRepository.ContainsItem(bconfig.ConfigItem));
            return primaryConfigurationRepository.WatchConfig(key).Merge(backedConfig);
        }

        public IObservable<ConfigItemEvent> WatchConfig(CompositeKey compositeKey)
        {
            var backedConfig = backingConfigurationRepository.WatchConfig(compositeKey).Where(bconfig =>
                !primaryConfigurationRepository.ContainsItem(bconfig.ConfigItem));
            return primaryConfigurationRepository.WatchConfig(compositeKey).Merge(backedConfig);
        }

        public IObservable<ConfigItemEvent> WatchConfig(Func<IConfigItem, bool> filterPredicate)
        {
            var backedConfig = backingConfigurationRepository.WatchConfig(filterPredicate).Where(bconfig =>
                !primaryConfigurationRepository.ContainsItem(bconfig.ConfigItem));
            return primaryConfigurationRepository.WatchConfig(filterPredicate).Merge(backedConfig);
        }

        public bool ContainsKey(CompositeKey compositeKey)
        {
            return primaryConfigurationRepository.ContainsKey(compositeKey) ||
                   backingConfigurationRepository.ContainsKey(compositeKey);
        }

        public bool ContainsItem(IConfigItem checkItem)
        {
            return ContainsKey(checkItem.CompositeKey);
        }

        public bool Update(IConfigItem item)
        {
            if (primaryConfigurationRepository.ContainsItem(item))
            {
                return primaryConfigurationRepository.Update(item);
            }
            return backingConfigurationRepository.Update(item);
        }

        public bool Add(IConfigItem item)
        {
            return primaryConfigurationRepository.Add(item);
        }

        public bool AddOrUpdate(IConfigItem item)
        {
            return Update(item) || Add(item);
        }

        public bool Delete(IConfigItem item)
        {
            if (primaryConfigurationRepository.ContainsItem(item))
            {
                return primaryConfigurationRepository.Delete(item);
            }
            return backingConfigurationRepository.Delete(item);
        }
    }
}