using System;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig
{
    [Flags]
    public enum MarketServerType
    {
        Unknown = 0x0000,
        ConfigServer = 0x0001,
        MarketData = 0x0002,
        Trading = 0x0004,
        TestOnly = 0x0008
    }
}