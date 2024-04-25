#region

using FortitudeCommon.DataStructures.Memory;
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

    public static byte ReadCurrentMessageVersion(this IBufferContext bufferContext) => bufferContext.EncodedBuffer!.Buffer[VersionOffset];

    public static uint ReadCurrentMessageId(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + MessageIdOffset);

    public static uint ReadCurrentMessageSize(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + MessageSizeOffset);

    public static MessageHeader ReadBasicMessageHeader(this IBufferContext bufferContext)
    {
        var version = bufferContext.ReadCurrentMessageVersion();
        var messageId = bufferContext.ReadCurrentMessageId();
        var messageSize = bufferContext.ReadCurrentMessageSize();
        return new MessageHeader(version, 0, messageId, messageSize, bufferContext);
    }
}
