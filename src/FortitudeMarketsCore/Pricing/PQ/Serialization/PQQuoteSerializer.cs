#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

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

internal sealed class PQQuoteSerializer : IMessageSerializer<IPQLevel0Quote>
{
    private const int FieldSize = 2 * sizeof(byte) + sizeof(uint);
    private readonly UpdateStyle updateStyle;

    // ReSharper disable once UnusedMember.Local
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteSerializer));

    public PQQuoteSerializer(UpdateStyle updateStyle) => this.updateStyle = updateStyle;

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((IPQLevel0Quote)message, (ISerdeContext)writeContext);
    }

    public void Serialize(IPQLevel0Quote obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WriteCursor
                , obj);
            bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
    {
        var publishAll = (updateStyle & UpdateStyle.FullSnapshot) > 0;
        if ((publishAll ? sizeof(uint) : 0) + PQQuoteMessageHeader.HeaderSize > buffer.Length - writeOffset)
            return FinishProcessingMessageReturnValue(message, -1);
        if (!(message is IPQLevel0Quote level0Quote)) return FinishProcessingMessageReturnValue(message, -1);
        fixed (byte* fptr = buffer)
        {
            if (!publishAll && !level0Quote.HasUpdates) return FinishProcessingMessageReturnValue(message, 0);
            var currentPtr = fptr + writeOffset;
            var end = fptr + buffer.Length;
            *currentPtr++ = message.Version; //protocol version
            var messageFlags = currentPtr++;
            var flags = publishAll ? (byte)PQBinaryMessageFlags.PublishAll : (byte)PQBinaryMessageFlags.None;
            var messageSizePtr = currentPtr;
            currentPtr += 4;
            StreamByteOps.ToBytes(ref currentPtr, level0Quote.SourceTickerQuoteInfo!.Id);
            var sequenceIdptr = currentPtr;
            currentPtr += 4;

            var fields = level0Quote.GetDeltaUpdateFields(TimeContext.UtcNow, updateStyle);
            level0Quote.Lock.Acquire();
            try
            {
                foreach (var field in fields)
                {
                    if (currentPtr + FieldSize > end) return FinishProcessingMessageReturnValue(message, -1);
                    *currentPtr++ = field.Flag;
                    if ((field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                    {
                        *currentPtr++ = (byte)field.Id;
                    }
                    else
                    {
                        flags |= (byte)PQBinaryMessageFlags.ExtendedFieldId;
                        StreamByteOps.ToBytes(ref currentPtr, field.Id);
                    }

                    StreamByteOps.ToBytes(ref currentPtr, field.Value);
                }

                var stringUpdates = level0Quote.GetStringUpdates
                    (TimeContext.UtcNow, updateStyle);
                foreach (var fieldStringUpdate in stringUpdates)
                {
                    if (currentPtr + 100 > end) return FinishProcessingMessageReturnValue(message, -1);

                    flags |= (byte)PQBinaryMessageFlags.ContainsStringUpdate;
                    *currentPtr++ = fieldStringUpdate.Field.Flag;
                    if ((fieldStringUpdate.Field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                        *currentPtr++ = (byte)fieldStringUpdate.Field.Id;
                    else
                        StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.Field.Id);
                    var stringSizePtr = currentPtr;
                    currentPtr += 4;
                    StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.StringUpdate.DictionaryId);
                    var bytesUsed = StreamByteOps.ToBytes(ref currentPtr,
                        fieldStringUpdate.StringUpdate.Value, (int)(end - currentPtr));
                    StreamByteOps.ToBytes(ref stringSizePtr, (uint)bytesUsed);
                }

                if (publishAll)
                    StreamByteOps.ToBytes(ref sequenceIdptr
                        , level0Quote.PQSequenceId == uint.MaxValue ? 0 : level0Quote.PQSequenceId);
                else
                    StreamByteOps.ToBytes(ref sequenceIdptr, unchecked(++level0Quote.PQSequenceId));
                level0Quote.HasUpdates = false;
            }
            finally
            {
                level0Quote.Lock.Release();
            }

            *messageFlags = flags;
            var written = (int)(currentPtr - fptr - writeOffset);
            /*logger.Debug($"{TimeContext.LocalTimeNow:O} {level0Quote.SourceTickerQuoteInfo.Source}-" +
                                 $"{level0Quote.SourceTickerQuoteInfo.Ticker}:" +
                                 $"{level0Quote.PQSequenceId}-> wrote {written} bytes for " +
                                 $"{updateStyle}.  ThreadName {Thread.CurrentThread.Name}");*/
            StreamByteOps.ToBytes(ref messageSizePtr, (uint)written);
            return FinishProcessingMessageReturnValue(message, written);
        }
    }

    private int FinishProcessingMessageReturnValue(IVersionedMessage message, int response)
    {
        message.DecrementRefCount();
        return response;
    }
}
