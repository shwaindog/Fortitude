using System.Collections.Generic;
using FortitudeCommon.Configuration.KeyValueProperties;

namespace FortitudeCommon.Monitoring.Logging.NLogAdapter
{
    public static class NLogAsyncConfigurationContext
    {
        public const string NLogAsyncLoggerCategory = "AsyncNLogFLogger";
        private static IConfigurationRepository instance;

        public static IConfigurationRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    Instance = new SimpleConfigurationRepository(NLogAsyncLoggerCategory, new List<ConfigItem>
                    {
                        new ConfigItem(NLogAsyncLoggerCategory, "QueueSize", "50000"),
                        new ConfigItem(NLogAsyncLoggerCategory, "Overflow", "Block"),
                        new ConfigItem(NLogAsyncLoggerCategory, "TimeoutMs", "500")
                    });
                    Instance = new BackedConfigurationRepository(Instance, FLoggerConfigurationContext.Instance);
                }
                return instance;
            }
            set { instance = value; }
        }
    }
}