#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes;

public static class PQQuoteMessageHeader
{
    public const int HeaderSize = 2 * sizeof(byte) + sizeof(ushort) // feedId
                                                   + sizeof(ushort) + // tickerId
                                                   sizeof(uint); // messageSize


    public static uint ReadCurrentMessageSequenceId(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , (int)bufferContext.EncodedBuffer.BufferRelativeReadCursor);
}
