using System.Collections.Generic;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public interface IConfigItemValue
    {
        string LookupKey { get; set; }
        IConfigItem ConfigItemValue { get; set; }
    }
    
    public class MainKeyValue : IConfigItemValue
    {
        public MainKeyValue(string lookupKey, IConfigItem configItemValue)
        {
            LookupKey = lookupKey;
            ConfigItemValue = configItemValue;
        }

        public string LookupKey { get; set; }
        public IConfigItem ConfigItemValue { get; set; }
        public IDictionary<string, SubKeyValue> SubKeys { get; set; }
    }

    public class SubKeyValue : IConfigItemValue
    {
        public SubKeyValue(string lookupKey, IConfigItem configItemValue)
        {
            LookupKey = lookupKey;
            ConfigItemValue = configItemValue;
        }

        public string LookupKey { get; set; }
        public IConfigItem ConfigItemValue { get; set; }
        public IDictionary<string, SubKeyModifierValue> SubKeyModifers { get; set; }
    }

    public class SubKeyModifierValue : IConfigItemValue
    {
        public SubKeyModifierValue(string lookupKey, IConfigItem configItemValue)
        {
            LookupKey = lookupKey;
            ConfigItemValue = configItemValue;
        }

        public string LookupKey { get; set; }
        public IConfigItem ConfigItemValue { get; set; }
    }
}