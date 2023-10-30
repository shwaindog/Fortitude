using System;

namespace FortitudeIO.Transports.NewSocketAPI.SocketFactory
{
    [Flags]
    public enum SocketConnectionAttributes
    {
        None               = 0x00, 
        KeepAlive          = 0x01,
        Unicast            = 0x02,
        Multicast          = 0x04,
        Reliable           = 0x08,
        Fast               = 0x10,
        Replayable         = 0x20,
        TransportHeartBeat = 0x40
    }
}