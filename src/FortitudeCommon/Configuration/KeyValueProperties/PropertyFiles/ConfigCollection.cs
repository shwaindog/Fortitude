using System;
using System.Collections;
using System.Linq;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region ConfigEventHandler class

    public delegate void ConfigEventHandler(object sender, ConfigEventArgs e);

    public class ConfigEventArgs : EventArgs
    {
        private readonly IConfig config;

        public ConfigEventArgs(IConfig config)
        {
            this.config = config;
        }

        public IConfig Config
        {
            get { return config; }
        }
    }

    #endregion

    public class ConfigCollection : IList
    {
        #region Constructors

        public ConfigCollection(ConfigSourceBase owner)
        {
            this.owner = owner;
        }

        #endregion

        #region Private variables

        private readonly ArrayList configList = new ArrayList();
        private readonly ConfigSourceBase owner;

        #endregion

        #region Public properties

        public IConfig this[int index]
        {
            get { return (IConfig) configList[index]; }
        }

        public IConfig this[string configName]
        {
            get { return configList.Cast<IConfig>().FirstOrDefault(config => config.Name == configName); }
        }

        public int Count
        {
            get { return configList.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        object IList.this[int index]
        {
            get { return configList[index]; }
            set { }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Public methods

        public IEnumerator GetEnumerator()
        {
            return configList.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            configList.CopyTo(array, index);
        }

        int IList.Add(object config)
        {
            var newConfig = config as IConfig;

            if (newConfig == null)
            {
                throw new Exception("Must be an IConfig");
            }
            Add(newConfig);
            return IndexOf(newConfig);
        }

        public void Remove(object config)
        {
            configList.Remove(config);
            OnConfigRemoved(new ConfigEventArgs((IConfig) config));
        }

        public void RemoveAt(int index)
        {
            var config = (IConfig) configList[index];
            configList.RemoveAt(index);
            OnConfigRemoved(new ConfigEventArgs(config));
        }

        public void Clear()
        {
            configList.Clear();
        }

        public bool Contains(object config)
        {
            return configList.Contains(config);
        }

        public int IndexOf(object config)
        {
            return configList.IndexOf(config);
        }

        public void Insert(int index, object config)
        {
            configList.Insert(index, config);
        }

        public void Add(IConfig config)
        {
            if (configList.Contains(config))
            {
                throw new ArgumentException("IConfig already exists");
            }
            var existingConfig = this[config.Name];

            if (existingConfig != null)
            {
                // Set all new keys
                var keys = config.GetKeys();
                foreach (var t in keys)
                {
                    existingConfig.Set(t, config.Get(t));
                }
            }
            else
            {
                configList.Add(config);
                OnConfigAdded(new ConfigEventArgs(config));
            }
        }

        public IConfig Add(string name)
        {
            ConfigBase result;

            if (this[name] == null)
            {
                result = new ConfigBase(name, owner);
                configList.Add(result);
                OnConfigAdded(new ConfigEventArgs(result));
            }
            else
            {
                throw new ArgumentException("An IConfig of that name already exists");
            }

            return result;
        }

        public void Remove(IConfig config)
        {
            configList.Remove(config);
            OnConfigRemoved(new ConfigEventArgs(config));
        }

        public void CopyTo(IConfig[] array, int index)
        {
            ((ICollection) configList).CopyTo(array, index);
        }

        #endregion

        #region Public events

        public event ConfigEventHandler ConfigAdded;

        public event ConfigEventHandler ConfigRemoved;

        #endregion

        #region Protected methods

        protected void OnConfigAdded(ConfigEventArgs e)
        {
            if (ConfigAdded != null)
            {
                ConfigAdded(this, e);
            }
        }

        protected void OnConfigRemoved(ConfigEventArgs e)
        {
            if (ConfigRemoved != null)
            {
                ConfigRemoved(this, e);
            }
        }

        #endregion
    }
}