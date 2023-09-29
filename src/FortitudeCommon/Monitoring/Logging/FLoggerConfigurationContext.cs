using System.Collections.Generic;
using FortitudeCommon.Configuration.KeyValueProperties;

namespace FortitudeCommon.Monitoring.Logging
{
    public static class FLoggerConfigurationContext
    {
        public const string NLogAsyncLoggerCategory = "FLogger";
        private static IConfigurationRepository instance;

        public static IConfigurationRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    Instance = new SimpleConfigurationRepository(NLogAsyncLoggerCategory, new List<ConfigItem>
                    {
                        new ConfigItem(NLogAsyncLoggerCategory, "FLoggerFactory",
                            "Common,Common.Monitoring.Logging.NLogAdapter")
                    });
                }
                return instance;
            }
            set { instance = value; }
        }
    }
}