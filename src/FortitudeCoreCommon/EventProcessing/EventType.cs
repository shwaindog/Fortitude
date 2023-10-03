using System;

namespace FortitudeCommon.EventProcessing
{
    [Flags]
    public enum EventType
    {
        Unknown   = 0,
        Created   = 1,
        Retrieved = 2,
        Updated   = 4,
        Deleted   = 8,
    }
}