using System;
using System.Collections;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class AliasText
    {
        #region Constructors

        public AliasText()
        {
            intAlias = InsensitiveHashtable();
            booleanAlias = InsensitiveHashtable();
            DefaultAliasLoad();
        }

        #endregion

        #region Private variables

        private readonly Hashtable booleanAlias;
        private readonly Hashtable intAlias;

        #endregion

        #region Public methods

        public void AddAlias(string key, string alias, int value)
        {
            if (intAlias.Contains(key))
            {
                var keys = (Hashtable) intAlias[key];

                keys[alias] = value;
            }
            else
            {
                var keys = InsensitiveHashtable();
                keys[alias] = value;
                intAlias.Add(key, keys);
            }
        }

        public void AddAlias(string alias, bool value)
        {
            booleanAlias[alias] = value;
        }

        public void AddAlias(string key, Enum enumAlias)
        {
            SetAliasTypes(key, enumAlias);
        }

        public bool ContainsBoolean(string key)
        {
            return booleanAlias.Contains(key);
        }

        public bool ContainsInt(string key, string alias)
        {
            var result = false;

            if (intAlias.Contains(key))
            {
                var keys = (Hashtable) intAlias[key];
                result = (keys.Contains(alias));
            }

            return result;
        }

        public bool GetBoolean(string key)
        {
            if (!booleanAlias.Contains(key))
            {
                throw new ArgumentException("Alias does not exist for text");
            }

            return (bool) booleanAlias[key];
        }

        public int GetInt(string key, string alias)
        {
            if (!intAlias.Contains(key))
            {
                throw new ArgumentException("Alias does not exist for key");
            }

            var keys = (Hashtable) intAlias[key];

            if (!keys.Contains(alias))
            {
                throw new ArgumentException("Config value does not match a " +
                                            "supplied alias");
            }

            return (int) keys[alias];
        }

        #endregion

        #region Private methods
        
        private void DefaultAliasLoad()
        {
            AddAlias("true", true);
            AddAlias("false", false);
        }
        
        private void SetAliasTypes(string key, Enum enumAlias)
        {
            var names = Enum.GetNames(enumAlias.GetType());
            var values = (int[]) Enum.GetValues(enumAlias.GetType());

            for (var i = 0; i < names.Length; i++)
            {
                AddAlias(key, names[i], values[i]);
            }
        }
        
        private Hashtable InsensitiveHashtable()
        {
            // ReSharper disable once CSharpWarnings::CS0618
            return new Hashtable(StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}