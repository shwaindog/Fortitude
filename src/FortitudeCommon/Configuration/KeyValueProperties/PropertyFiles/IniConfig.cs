using System.Linq;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class IniConfig : ConfigBase
    {
        #region Private variables

        private readonly IniConfigSource parent;

        #endregion

        #region Constructors

        public IniConfig(string name, IConfigSource source)
            : base(name, source)
        {
            parent = (IniConfigSource) source;
        }

        #endregion

        #region Private methods
        
        private string CaseInsensitiveKeyName(string key)
        {
            var lowerKey = key.ToLower();
            var result = Keys.Keys.Cast<string>().FirstOrDefault(currentKey => currentKey.ToLower() == lowerKey);

            return result ?? key;
        }

        #endregion

        #region Public methods

        public override string Get(string key)
        {
            if (!parent.CaseSensitive)
            {
                key = CaseInsensitiveKeyName(key);
            }

            return base.Get(key);
        }

        public override void Set(string key, object value)
        {
            if (!parent.CaseSensitive)
            {
                key = CaseInsensitiveKeyName(key);
            }

            base.Set(key, value);
        }

        public override void Remove(string key)
        {
            if (!parent.CaseSensitive)
            {
                key = CaseInsensitiveKeyName(key);
            }

            base.Remove(key);
        }

        #endregion
    }
}