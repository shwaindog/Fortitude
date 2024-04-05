#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes;

public static class OrxMessageHeader
{
    public const int VersionOffset = 0;
    public const int VersionBytes = 1;
    public const int MessageIdOffset = 1;
    public const int MessageIdBytes = 2;
    public const int MessageSizeOffset = 3;
    public const int MessageSizeBytes = 2;
    public const int HeaderSize = 2 * OrxConstants.UInt16Sz + OrxConstants.UInt8Sz;
    public static readonly Type VersionType = typeof(byte);
    public static readonly Type MessageIdType = typeof(ushort);
    public static readonly Type MessageSizeType = typeof(ushort);

    public static byte ReadCurrentMessageVersion(this IBufferContext bufferContext) => bufferContext.EncodedBuffer!.Buffer[VersionOffset];

    public static ushort ReadCurrentMessageId(this IBufferContext bufferContext) =>
        StreamByteOps.ToUShort(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + MessageIdOffset);

    public static ushort ReadCurrentMessageSize(this IBufferContext bufferContext) =>
        StreamByteOps.ToUShort(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + MessageSizeOffset);

    public static BasicMessageHeader ReadBasicMessageHeader(this IBufferContext bufferContext)
    {
        var version = bufferContext.ReadCurrentMessageVersion();
        var messageId = bufferContext.ReadCurrentMessageId();
        var messageSize = bufferContext.ReadCurrentMessageSize();
        return new BasicMessageHeader(version, messageId, messageSize, bufferContext);
    }
}
