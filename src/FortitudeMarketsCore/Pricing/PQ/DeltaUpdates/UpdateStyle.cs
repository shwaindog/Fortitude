using System;

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    [Flags]
    public enum UpdateStyle : byte
    {
        NotSpecified     =    0,
        Updates          = 0x01,
        FullSnapshot     = 0x02,
        Replay           = 0x10,
        Persistence      = 0x20
    }
}
