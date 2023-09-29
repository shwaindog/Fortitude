using System.Collections.Generic;
using FortitudeCommon.Configuration.KeyValueProperties;

namespace FortitudeIO.Transports.Sockets
{
    public static class SocketsConfigurationContext
    {
        public const string SocketsLoggerCategory = "Sockets";
        private static IConfigurationRepository instance;

        public static IConfigurationRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    Instance = new SimpleConfigurationRepository(SocketsLoggerCategory, new List<ConfigItem>
                    {
                        new ConfigItem(SocketsLoggerCategory, "LoggerName",
                            "FortitudeMarketsCore.Comms.Sockets")
                    });
                }
                return instance;
            }
            set => instance = value;
        }
    }
}