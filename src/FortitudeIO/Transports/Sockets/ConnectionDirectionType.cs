using System;

namespace FortitudeIO.Transports.Sockets
{
    [Flags]
    public enum ConnectionDirectionType
    {
        Unknown   = 0,
        Receiver  = 1,
        Publisher = 2,
        Both      = 3
    }
}