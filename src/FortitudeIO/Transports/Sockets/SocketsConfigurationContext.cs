#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Transports.Sockets;

public static class SocketsConfigurationContext
{
    public const string SocketsLoggerCategory = "Sockets";
    private static IConfigurationSection? instance;

    public static IConfigurationSection Instance
    {
        get
        {
            if (instance == null)
                Instance = new PocoConfigurationSection(SocketsLoggerCategory, SocketsLoggerCategory, null)
                {
                    { "LoggerName", "FortitudeMarketsCore.Comms.Sockets" }
                };
            return instance!;
        }
        set => instance = value;
    }

    public static void ClearPreviousConfig() => instance = null;
}
