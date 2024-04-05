#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes;

public static class PQQuoteMessageHeader
{
    public const int VersionOffset = 0;
    public const int VersionBytes = 1;
    public const int FlagsOffset = 1;
    public const int FlagsBytes = 1;
    public const int MessageSizeOffset = 2;
    public const int MessageSizeBytes = 4;
    public const int SourceTickerIdOffset = 6;
    public const int SourceTickerIdBytes = 4;
    public const int SequenceIdOffset = 10;
    public const int SequenceIdBytes = 4;

    public const int HeaderSize = 2 * sizeof(byte) + sizeof(ushort) // feedId
                                                   + sizeof(ushort) + // tickerId
                                                   sizeof(uint) + // messageSize
                                                   sizeof(uint); // sequenceId

    public static readonly Type VersionType = typeof(byte);
    public static readonly Type FlagsType = typeof(byte);
    public static readonly Type MessageSizeType = typeof(uint);
    public static readonly Type SourceTickerIdType = typeof(uint);
    public static readonly Type SequenceIdType = typeof(uint);

    public static byte ReadCurrentMessageVersion(this IBufferContext bufferContext) => bufferContext.EncodedBuffer!.Buffer[VersionOffset];

    public static ushort ReadCurrentMessageFlags(this IBufferContext bufferContext) => bufferContext.EncodedBuffer!.Buffer[FlagsOffset];

    public static uint ReadCurrentMessageSize(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + MessageSizeOffset);

    public static uint ReadCurrentMessageSourceTickerId(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + SourceTickerIdOffset);

    public static uint ReadCurrentMessageSequenceId(this IBufferContext bufferContext) =>
        StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
            , bufferContext.EncodedBuffer.ReadCursor + SequenceIdOffset);

    public static BasicMessageHeader ReadBasicMessageHeader(this IBufferContext bufferContext)
    {
        var version = bufferContext.ReadCurrentMessageVersion();
        var messageId = bufferContext.ReadCurrentMessageSequenceId();
        var messageSize = bufferContext.ReadCurrentMessageSize();
        return new BasicMessageHeader(version, messageId, messageSize, bufferContext);
    }
}
