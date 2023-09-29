using FortitudeCommon.EventProcessing;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public class ConfigItemEvent
    {
        public ConfigItemEvent(EventType eventType, IConfigItem configItem)
        {
            EventType = eventType;
            ConfigItem = configItem;
        }

        public EventType EventType { get; private set; }
        public IConfigItem ConfigItem { get; private set; }
    }
}