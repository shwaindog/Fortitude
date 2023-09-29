using System;
using Microsoft.Win32;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region RegistryRecurse enumeration

    public enum RegistryRecurse
    {
        None,
        Flattened,
        Namespacing
    }

    #endregion

    public class RegistryConfigSource : ConfigSourceBase
    {
        #region Private variables

        public RegistryConfigSource()
        {
            DefaultKey = null;
        }

        #endregion

        #region Public properties

        public RegistryKey DefaultKey { get; set; }

        #endregion

        #region Public methods

        public override IConfig AddConfig(string name)
        {
            if (DefaultKey == null)
            {
                throw new ApplicationException("You must set DefaultKey");
            }

            return AddConfig(name, DefaultKey);
        }

        public IConfig AddConfig(string name, RegistryKey key)
        {
            var result = new RegistryConfig(name, this) {Key = key, ParentKey = true};

            Configs.Add(result);

            return result;
        }

        public void AddMapping(RegistryKey registryKey, string path)
        {
            var key = registryKey.OpenSubKey(path, true);

            if (key == null)
            {
                throw new ArgumentException("The specified key does not exist");
            }

            LoadKeyValues(key, ShortKeyName(key));
        }

        public void AddMapping(RegistryKey registryKey,
            string path,
            RegistryRecurse recurse)
        {
            var key = registryKey.OpenSubKey(path, true);

            if (key == null)
            {
                throw new ArgumentException("The specified key does not exist");
            }

            LoadKeyValues(key, recurse == RegistryRecurse.Namespacing ? path : ShortKeyName(key));

            var subKeys = key.GetSubKeyNames();
            foreach (var t in subKeys)
            {
                switch (recurse)
                {
                    case RegistryRecurse.None:
                        break;
                    case RegistryRecurse.Namespacing:
                        AddMapping(registryKey, path + "\\" + t, recurse);
                        break;
                    case RegistryRecurse.Flattened:
                        AddMapping(key, t, recurse);
                        break;
                }
            }
        }

        public override void Save()
        {
            MergeConfigsIntoDocument();

            for (var i = 0; i < Configs.Count; i++)
            {
                var registryConfig = Configs[i] as RegistryConfig;
                if (registryConfig != null)
                {
                    var config = registryConfig;
                    var keys = config.GetKeys();

                    foreach (var t in keys)
                    {
                        config.Key.SetValue(t, config.Get(t));
                    }
                }
            }
        }

        public override void Reload()
        {
            ReloadKeys();
        }

        #endregion

        #region Private methods
        private void LoadKeyValues(RegistryKey key, string keyName)
        {
            var config = new RegistryConfig(keyName, this) {Key = key};

            var values = key.GetValueNames();
            foreach (var value in values)
            {
                config.Add(value, key.GetValue(value).ToString());
            }
            Configs.Add(config);
        }
        
        private void MergeConfigsIntoDocument()
        {
            foreach (IConfig config in Configs)
            {
                var config1 = config as RegistryConfig;
                if (config1 != null)
                {
                    var registryConfig = config1;

                    if (registryConfig.ParentKey)
                    {
                        registryConfig.Key =
                            registryConfig.Key.CreateSubKey(registryConfig.Name);
                    }
                    RemoveKeys(registryConfig);

                    var keys = config.GetKeys();
                    foreach (var t in keys)
                    {
                        registryConfig.Key.SetValue(t, config.Get(t));
                    }
                    registryConfig.Key.Flush();
                }
            }
        }
        private void ReloadKeys()
        {
            var keys = new RegistryKey[Configs.Count];

            for (var i = 0; i < keys.Length; i++)
            {
                keys[i] = ((RegistryConfig) Configs[i]).Key;
            }

            Configs.Clear();
            foreach (var t in keys)
            {
                LoadKeyValues(t, ShortKeyName(t));
            }
        }
        
        private void RemoveKeys(RegistryConfig config)
        {
            foreach (var valueName in config.Key.GetValueNames())
            {
                if (!config.Contains(valueName))
                {
                    config.Key.DeleteValue(valueName);
                }
            }
        }
        
        private string ShortKeyName(RegistryKey key)
        {
            var index = key.Name.LastIndexOf("\\", StringComparison.Ordinal);

            return (index == -1) ? key.Name : key.Name.Substring(index + 1);
        }

        #region RegistryConfig class
        private class RegistryConfig : ConfigBase
        {
            #region Private variables
            public RegistryConfig(string name, IConfigSource source)
                : base(name, source)
            {
                ParentKey = false;
                Key = null;
            }

            #endregion

            #region Public properties
            public bool ParentKey { get; set; }
            public RegistryKey Key { get; set; }

            #endregion
        }

        #endregion

        #endregion
    }
}