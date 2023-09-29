using System;
using System.Globalization;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{

    #region ConfigKeyEventArgs class

    public delegate void ConfigKeyEventHandler(object sender, ConfigKeyEventArgs e);

    public class ConfigKeyEventArgs : EventArgs
    {
        private readonly string keyName;
        private readonly string keyValue;

        public ConfigKeyEventArgs(string keyName, string keyValue)
        {
            this.keyName = keyName;
            this.keyValue = keyValue;
        }

        public string KeyName
        {
            get { return keyName; }
        }

        public string KeyValue
        {
            get { return keyValue; }
        }
    }

    #endregion

    public class ConfigBase : IConfig
    {
        #region Protected variables

        protected OrderedList Keys = new OrderedList();

        #endregion

        #region Constructors

        public ConfigBase(string name, IConfigSource source)
        {
            configName = name;
            configSource = source;
            aliasText = new AliasText();
        }

        #endregion

        #region Private variables

        private readonly AliasText aliasText;
        private readonly IConfigSource configSource;
        private readonly IFormatProvider format = NumberFormatInfo.CurrentInfo;
        private string configName;

        #endregion

        #region Public properties

        public string Name
        {
            get { return configName; }
            set
            {
                if (configName != value)
                {
                    Rename(value);
                }
            }
        }

        public IConfigSource ConfigSource
        {
            get { return configSource; }
        }

        public AliasText Alias
        {
            get { return aliasText; }
        }

        #endregion

        #region Public methods

        public bool Contains(string key)
        {
            return (Get(key) != null);
        }

        public virtual string Get(string key)
        {
            string result = null;

            if (Keys.Contains(key))
            {
                result = Keys[key].ToString();
            }

            return result;
        }

        public string Get(string key, string defaultValue)
        {
            var result = Get(key);

            return result ?? defaultValue;
        }

        public string GetExpanded(string key)
        {
            return ConfigSource.GetExpanded(this, key);
        }

        public string GetString(string key)
        {
            return Get(key);
        }

        public string GetString(string key, string defaultValue)
        {
            return Get(key, defaultValue);
        }

        public int GetInt(string key)
        {
            var text = Get(key);

            if (text == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return Convert.ToInt32(text, format);
        }

        public int GetInt(string key, bool fromAlias)
        {
            if (!fromAlias)
            {
                return GetInt(key);
            }

            var result = Get(key);

            if (result == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return GetIntAlias(key, result);
        }

        public int GetInt(string key, int defaultValue)
        {
            var result = Get(key);

            return (result == null)
                ? defaultValue
                : Convert.ToInt32(result, format);
        }

        public int GetInt(string key, int defaultValue, bool fromAlias)
        {
            if (!fromAlias)
            {
                return GetInt(key, defaultValue);
            }

            var result = Get(key);

            return (result == null) ? defaultValue : GetIntAlias(key, result);
        }

        public long GetLong(string key)
        {
            var text = Get(key);

            if (text == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return Convert.ToInt64(text, format);
        }

        public long GetLong(string key, long defaultValue)
        {
            var result = Get(key);

            return (result == null)
                ? defaultValue
                : Convert.ToInt64(result, format);
        }

        public bool GetBoolean(string key)
        {
            var text = Get(key);

            if (text == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return GetBooleanAlias(text);
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            var text = Get(key);

            return (text == null) ? defaultValue : GetBooleanAlias(text);
        }

        public float GetFloat(string key)
        {
            var text = Get(key);

            if (text == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return Convert.ToSingle(text, format);
        }

        public float GetFloat(string key, float defaultValue)
        {
            var result = Get(key);

            return (result == null)
                ? defaultValue
                : Convert.ToSingle(result, format);
        }

        public double GetDouble(string key)
        {
            var text = Get(key);

            if (text == null)
            {
                throw new ArgumentException("Value not found: " + key);
            }

            return Convert.ToDouble(text, format);
        }

        public double GetDouble(string key, double defaultValue)
        {
            var result = Get(key);

            return (result == null)
                ? defaultValue
                : Convert.ToDouble(result, format);
        }

        public string[] GetKeys()
        {
            var result = new string[Keys.Keys.Count];

            Keys.Keys.CopyTo(result, 0);

            return result;
        }

        public string[] GetValues()
        {
            var result = new string[Keys.Values.Count];

            Keys.Values.CopyTo(result, 0);

            return result;
        }

        public virtual void Set(string key, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value" + " cannot be null");
            }

            if (Get(key) == null)
            {
                Add(key, value.ToString());
            }
            else
            {
                Keys[key] = value.ToString();
            }

            if (ConfigSource.AutoSave)
            {
                ConfigSource.Save();
            }

            OnKeySet(new ConfigKeyEventArgs(key, value.ToString()));
        }

        public virtual void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key " + " cannot be null");
            }

            if (Get(key) != null)
            {
                string keyValue = null;
                if (KeySet != null)
                {
                    keyValue = Get(key);
                }
                Keys.Remove(key);

                OnKeyRemoved(new ConfigKeyEventArgs(key, keyValue));
            }
        }

        public void Add(string key, string value)
        {
            Keys.Add(key, value);
        }

        #endregion

        #region Public events

        public event ConfigKeyEventHandler KeySet;

        public event ConfigKeyEventHandler KeyRemoved;

        #endregion

        #region Protected methods

        protected void OnKeySet(ConfigKeyEventArgs e)
        {
            if (KeySet != null)
            {
                KeySet(this, e);
            }
        }

        protected void OnKeyRemoved(ConfigKeyEventArgs e)
        {
            if (KeyRemoved != null)
            {
                KeyRemoved(this, e);
            }
        }

        #endregion

        #region Private methods
        
        private void Rename(string name)
        {
            ConfigSource.Configs.Remove(this);
            configName = name;
            ConfigSource.Configs.Add(this);
        }
        private int GetIntAlias(string key, string alias)
        {
            var result = aliasText.ContainsInt(key, alias)
                ? aliasText.GetInt(key, alias)
                : ConfigSource.Alias.GetInt(key, alias);

            return result;
        }
        
        private bool GetBooleanAlias(string key)
        {
            bool result;

            if (aliasText.ContainsBoolean(key))
            {
                result = aliasText.GetBoolean(key);
            }
            else
            {
                if (ConfigSource.Alias.ContainsBoolean(key))
                {
                    result = ConfigSource.Alias.GetBoolean(key);
                }
                else
                {
                    throw new ArgumentException
                        ("Alias value not found: " + key
                         + ". Add it to the Alias property.");
                }
            }

            return result;
        }

        #endregion
    }
}