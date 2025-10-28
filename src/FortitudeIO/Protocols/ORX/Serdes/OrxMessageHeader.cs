#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes;

public static class OrxMessageHeader
{
    public const int VersionOffset = 0;
    public const int MessageFlagsOffset = 1;
    public const int MessageIdOffset = 2;
    public const int MessageSizeOffset = 6;
    public const int HeaderSize = MessageHeader.SerializationSize;

    public static unsafe byte ReadCurrentMessageVersion(this IBufferContext bufferContext)
    {
        using var fixedBuffer = bufferContext.EncodedBuffer!;
        var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
        return *ptr;
    }

    public static unsafe uint ReadCurrentMessageId(this IBufferContext bufferContext)
    {
        using var fixedBuffer = bufferContext.EncodedBuffer!;
        var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor + MessageIdOffset;
        return StreamByteOps.ToUInt(ref ptr);
    }

    public static unsafe uint ReadCurrentMessageSize(this IBufferContext bufferContext)
    {
        using var fixedBuffer = bufferContext.EncodedBuffer!;
        var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor + MessageSizeOffset;
        return StreamByteOps.ToUInt(ref ptr);
    }

    public static MessageHeader ReadBasicMessageHeader(this IBufferContext bufferContext)
    {
        var version = bufferContext.ReadCurrentMessageVersion();
        var messageId = bufferContext.ReadCurrentMessageId();
        var messageSize = bufferContext.ReadCurrentMessageSize();
        return new MessageHeader(version, 0, messageId, messageSize, bufferContext);
    }
}
