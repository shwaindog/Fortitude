using System;
using System.Collections;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public abstract class ConfigSourceBase : IConfigSource
    {
        #region Constructors

        protected ConfigSourceBase()
        {
            AutoSave = false;
            configList = new ConfigCollection(this);
        }

        #endregion

        #region Private variables

        private readonly AliasText alias = new AliasText();
        private readonly ConfigCollection configList;
        private readonly ArrayList sourceList = new ArrayList();

        #endregion

        #region Public properties

        public ConfigCollection Configs
        {
            get { return configList; }
        }

        public bool AutoSave { get; set; }

        public AliasText Alias
        {
            get { return alias; }
        }

        #endregion

        #region Public methods

        public void Merge(IConfigSource source)
        {
            if (!sourceList.Contains(source))
            {
                sourceList.Add(source);
            }

            foreach (IConfig config in source.Configs)
            {
                Configs.Add(config);
            }
        }

        public virtual IConfig AddConfig(string name)
        {
            return configList.Add(name);
        }

        public string GetExpanded(IConfig config, string key)
        {
            return Expand(config, key, false);
        }

        public virtual void Save()
        {
            OnSaved(new EventArgs());
        }

        public virtual void Reload()
        {
            OnReloaded(new EventArgs());
        }

        public void ExpandKeyValues()
        {
            foreach (IConfig config in configList)
            {
                var keys = config.GetKeys();
                foreach (var t in keys)
                {
                    Expand(config, t, true);
                }
            }
        }

        public void ReplaceKeyValues()
        {
            ExpandKeyValues();
        }

        #endregion

        #region Public events

        public event EventHandler Reloaded;

        public event EventHandler Saved;

        #endregion

        #region Protected methods

        protected void OnReloaded(EventArgs e)
        {
            if (Reloaded != null)
            {
                Reloaded(this, e);
            }
        }

        protected void OnSaved(EventArgs e)
        {
            if (Saved != null)
            {
                Saved(this, e);
            }
        }

        #endregion

        #region Private methods	

        /// <summary>
        ///     Expands key values from the given IConfig.
        /// </summary>
        private string Expand(IConfig config, string key, bool setValue)
        {
            var result = config.Get(key);
            if (result == null)
            {
                throw new ArgumentException(string.Format("[{0}] not found in [{1}]",
                    key, config.Name));
            }

            while (true)
            {
                var startIndex = result.IndexOf("${", 0, StringComparison.Ordinal);
                if (startIndex == -1)
                {
                    break;
                }

                var endIndex = result.IndexOf("}", startIndex + 2, StringComparison.Ordinal);
                if (endIndex == -1)
                {
                    break;
                }

                var search = result.Substring(startIndex + 2,
                    endIndex - (startIndex + 2));

                if (search == key)
                {
                    // Prevent infinite recursion
                    throw new ArgumentException
                        ("Key cannot have a expand value of itself: " + key);
                }

                var replace = ExpandValue(config, search);

                result = result.Replace("${" + search + "}", replace);
            }

            if (setValue)
            {
                config.Set(key, result);
            }

            return result;
        }

        /// <summary>
        ///     Returns the replacement value of a config.
        /// </summary>
        private string ExpandValue(IConfig config, string search)
        {
            string result;

            var replaces = search.Split('|');

            if (replaces.Length > 1)
            {
                var newConfig = Configs[replaces[0]];
                if (newConfig == null)
                {
                    throw new ArgumentException("Expand config not found: "
                                                + replaces[0]);
                }
                result = newConfig.Get(replaces[1]);
                if (result == null)
                {
                    throw new ArgumentException("Expand key not found: "
                                                + replaces[1]);
                }
            }
            else
            {
                result = config.Get(search);

                if (result == null)
                {
                    throw new ArgumentException("Key not found: " + search);
                }
            }

            return result;
        }

        #endregion
    }
}