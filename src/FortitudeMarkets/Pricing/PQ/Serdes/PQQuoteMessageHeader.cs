#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes;

public static class PQQuoteMessageHeader
{
    public const int HeaderSize = 2 * sizeof(byte) + sizeof(ushort) // feedId
                                                   + sizeof(ushort) + // tickerId
                                                   sizeof(uint); // messageSize


    public static unsafe uint ReadCurrentMessageSequenceId(this IBufferContext bufferContext)
    {
        using var fixedBuffer = bufferContext.EncodedBuffer!;
        var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
        return StreamByteOps.ToUInt(ref ptr);
    }
}
