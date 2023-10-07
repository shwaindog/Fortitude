using System;

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    [Flags]
    public enum PQBinaryMessageFlags : byte
    {
        None = 0x00,
        ExtendedFieldId = 0x01,
        PublishAll = 0x02,
        ContainsStringUpdate = 0x04,
        IsHeartBeat = 0x08,
        IsSplitMessage = 0x10        
    }
}