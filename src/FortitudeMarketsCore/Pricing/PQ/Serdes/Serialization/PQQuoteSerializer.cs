#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

internal sealed class PQQuoteSerializer : IMessageSerializer<PQLevel0Quote>
{
    private const int FieldSize = 2 * sizeof(byte) + sizeof(uint);
    private readonly PQMessageFlags messageFlags;

    // ReSharper disable once UnusedMember.Local
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteSerializer));

    public PQQuoteSerializer(PQMessageFlags messageFlags) => this.messageFlags = messageFlags;

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQLevel0Quote)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQLevel0Quote obj, ISerdeContext writeContext)
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
        if (!(message is IPQLevel0Quote pqL0Quote)) return FinishProcessingMessageReturnValue(message, -1);
        var resolvedFlags = pqL0Quote.OverrideSerializationFlags ?? messageFlags;
        pqL0Quote.OverrideSerializationFlags = null;
        var publishAll = (resolvedFlags & PQMessageFlags.Snapshot) > 0;
        if ((publishAll ? sizeof(uint) : 0) + PQQuoteMessageHeader.HeaderSize > buffer.Length - writeOffset)
            return FinishProcessingMessageReturnValue(message, -1);
        fixed (byte* fptr = buffer)
        {
            if (!publishAll && !pqL0Quote.HasUpdates) return FinishProcessingMessageReturnValue(message, 0);
            var writeStart = fptr + writeOffset;
            var currentPtr = writeStart;
            var end = fptr + buffer.Length;
            *currentPtr++ = message.Version; //protocol version
            *currentPtr++ = (byte)resolvedFlags;
            StreamByteOps.ToBytes(ref currentPtr, pqL0Quote.SourceTickerQuoteInfo!.Id);
            var messageSizePtr = currentPtr;
            currentPtr += 4;
            var sequenceIdptr = currentPtr;
            currentPtr += 4;

            var fields = pqL0Quote.GetDeltaUpdateFields(TimeContext.UtcNow, resolvedFlags);
            pqL0Quote.Lock.Acquire();
            try
            {
                foreach (var field in fields)
                {
                    // logger.Info("Writing field {0}", field);

                    if (currentPtr + FieldSize > end) return FinishProcessingMessageReturnValue(message, -1);
                    *currentPtr++ = field.Flag;
                    if ((field.Flag & PQFieldFlags.IsExtendedFieldId) == 0)
                        *currentPtr++ = (byte)field.Id;
                    else
                        StreamByteOps.ToBytes(ref currentPtr, field.Id);

                    StreamByteOps.ToBytes(ref currentPtr, field.Value);
                }

                var stringUpdates = pqL0Quote.GetStringUpdates
                    (TimeContext.UtcNow, resolvedFlags);
                foreach (var fieldStringUpdate in stringUpdates)
                {
                    if (currentPtr + 100 > end) return FinishProcessingMessageReturnValue(message, -1);

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
                    // logger.Info("Writing string update {0} and Value = {1}", fieldStringUpdate, bytesUsed);
                }

                if (publishAll)
                    StreamByteOps.ToBytes(ref sequenceIdptr
                        , pqL0Quote.PQSequenceId == uint.MaxValue ? 0 : pqL0Quote.PQSequenceId);
                else
                    StreamByteOps.ToBytes(ref sequenceIdptr, unchecked(++pqL0Quote.PQSequenceId));
                pqL0Quote.HasUpdates = false;
            }
            finally
            {
                pqL0Quote.Lock.Release();
            }

            var written = (int)(currentPtr - writeStart);
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
