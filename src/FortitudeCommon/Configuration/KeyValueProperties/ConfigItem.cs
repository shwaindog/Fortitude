using System;
using FortitudeCommon.Chronometry;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public class ConfigItem : IConfigItem
    {
        public ConfigItem(string category, CompositeKey compositeKey, string value, string newAuxilaryValue = null)
        {
            Category = category;
            CompositeKey = compositeKey;
            Value = value;
            AuxilaryValue = newAuxilaryValue;
            LastUpdatedDateTime = TimeContext.UtcNow;
        }

        public ConfigItem(ConfigItem cloneSettings, string newValue, string newAuxilaryValue = null)
        {
            Category = cloneSettings.Category;
            CompositeKey = cloneSettings.CompositeKey;
            Value = newValue;
            AuxilaryValue = newAuxilaryValue;
            LastUpdatedDateTime = TimeContext.UtcNow;
        }

        public ConfigItem(string category, string key, string newValue, string newAuxilaryValue = null)
        {
            Category = category;
            CompositeKey = new CompositeKey(key);
            Value = newValue;
            AuxilaryValue = newAuxilaryValue;
            LastUpdatedDateTime = TimeContext.UtcNow;
        }

        public string Category { get; internal set; }
        public CompositeKey CompositeKey { get; set; }
        public string Value { get; private set; }
        public string AuxilaryValue { get; private set; }
        public string Comment { get; private set; }
        public DateTime LastUpdatedDateTime { get; private set; }
    }
}