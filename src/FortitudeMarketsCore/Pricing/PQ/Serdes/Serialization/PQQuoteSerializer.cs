// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

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

public enum PQSerializationFlags
{
    ForSocketPublish               = 0
  , ForStorage                     = 1
  , ForStorageIncludeReceiverTimes = 3
}

[Flags]
public enum StorageFlags
{
    // Start From PQMessageFlags
    None                = 0
  , Complete            = 1
  , Snapshot            = 3
  , Update              = 4
  , CompleteUpdate      = 5
  , Replay              = 8
  , NoChangeOrHeartbeat = 16
    // end from PQMessageFlags
  , OneByteMessageSize   = 32
  , TwoByteMessageSize   = 64
  , ThreeByteMessageSize = 96
  , IncludesSequenceId   = 128
}

public sealed class PQQuoteSerializer : IMessageSerializer<PQLevel0Quote>
{
    private const    int                  FieldSize = 2 * sizeof(byte) + sizeof(uint);
    private readonly PQMessageFlags       messageFlags;
    private readonly PQSerializationFlags serializationFlags;
    private          List<PQFieldUpdate>  fieldsToSerialize = new();

    // ReSharper disable once UnusedMember.Local
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteSerializer));

    private uint                      previousSerializedSequenceId;
    private List<PQFieldStringUpdate> stringUpdatesToSerialize = new();

    public PQQuoteSerializer(PQMessageFlags messageFlags, PQSerializationFlags serializationFlags = PQSerializationFlags.ForSocketPublish)
    {
        this.messageFlags       = messageFlags;
        this.serializationFlags = serializationFlags;
    }

    public MarshalType MarshalType => MarshalType.Binary;

    void IMessageSerializer.Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQLevel0Quote)message, writeContext);
    }

    public void Serialize(PQLevel0Quote obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);
            bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength            =  writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IVersionedMessage message)
    {
        if (!(message is IPQLevel0Quote pqL0Quote)) return FinishProcessingMessageReturnValue(message, -1);
        var resolvedFlags = pqL0Quote.OverrideSerializationFlags ?? messageFlags;
        resolvedFlags |= serializationFlags == PQSerializationFlags.ForStorageIncludeReceiverTimes
            ? PQMessageFlags.IncludeReceiverTimes
            : PQMessageFlags.None;
        var resolvedStorageFlags = (StorageFlags)(byte)resolvedFlags;
        var publishAll           = (resolvedFlags & PQMessageFlags.Complete) > 0;
        pqL0Quote.OverrideSerializationFlags = null;
        using var fixedBuffer = buffer;
        if ((publishAll ? sizeof(uint) : 0) + PQQuoteMessageHeader.HeaderSize > buffer.RemainingStorage)
            return FinishProcessingMessageReturnValue(message, -1);
        if (!publishAll && !pqL0Quote.HasUpdates) return FinishProcessingMessageReturnValue(message, 0);
        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var currentPtr = writeStart;
        var end        = writeStart + buffer.RemainingStorage;
        fieldsToSerialize.Clear();
        stringUpdatesToSerialize.Clear();
        pqL0Quote.Lock.Acquire();
        try
        {
            fieldsToSerialize.AddRange(pqL0Quote.GetDeltaUpdateFields(TimeContext.UtcNow, resolvedFlags));
            stringUpdatesToSerialize.AddRange(pqL0Quote.GetStringUpdates(TimeContext.UtcNow, resolvedFlags));

            if (serializationFlags == PQSerializationFlags.ForSocketPublish) *currentPtr++ = message.Version; //protocol version

            var flagsPtr = currentPtr++;
            if (serializationFlags == PQSerializationFlags.ForSocketPublish)
                StreamByteOps.ToBytes(ref currentPtr, pqL0Quote.SourceTickerQuoteInfo!.Id);

            var messageSizePtr  = currentPtr;
            var sequenceIdBytes = 4;
            if (serializationFlags == PQSerializationFlags.ForSocketPublish)
            {
                currentPtr += 4;
            }
            else if (stringUpdatesToSerialize.Any())
            {
                sequenceIdBytes      =  pqL0Quote.PQSequenceId == previousSerializedSequenceId + 1 && !publishAll ? 0 : 4;
                resolvedStorageFlags |= sequenceIdBytes > 0 ? StorageFlags.IncludesSequenceId : StorageFlags.None;
                resolvedStorageFlags |= StorageFlags.ThreeByteMessageSize;
                currentPtr           += 3;
            }
            else
            {
                sequenceIdBytes      =  pqL0Quote.PQSequenceId == previousSerializedSequenceId + 1 && !publishAll ? 0 : 4;
                resolvedStorageFlags |= sequenceIdBytes > 0 ? StorageFlags.IncludesSequenceId : StorageFlags.None;
                var countSingleByteIdUpdates = fieldsToSerialize.Count(fu => (fu.Flag & PQFieldFlags.IsExtendedFieldId) == 0);
                var countTwoByteIdUpdates    = fieldsToSerialize.Count(fu => (fu.Flag & PQFieldFlags.IsExtendedFieldId) > 0);
                var totalBytes               = countSingleByteIdUpdates * 6 + countTwoByteIdUpdates * 7 + sequenceIdBytes;
                switch (totalBytes)
                {
                    case < byte.MaxValue:
                        currentPtr++;
                        resolvedStorageFlags |= StorageFlags.OneByteMessageSize;
                        break;
                    case < ushort.MaxValue:
                        currentPtr           += 2;
                        resolvedStorageFlags |= StorageFlags.TwoByteMessageSize;
                        break;
                    default:
                        currentPtr           += 3;
                        resolvedStorageFlags |= StorageFlags.ThreeByteMessageSize;
                        break;
                }
            }

            var sequenceIdptr = currentPtr;
            currentPtr += sequenceIdBytes;

            foreach (var field in fieldsToSerialize)
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

            foreach (var fieldStringUpdate in stringUpdatesToSerialize)
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

            if (sequenceIdBytes > 0)
            {
                var isSnapshot = (resolvedFlags & PQMessageFlags.Snapshot) > 0;
                if (isSnapshot)
                {
                    previousSerializedSequenceId = pqL0Quote.PQSequenceId == uint.MaxValue ? 0 : pqL0Quote.PQSequenceId;
                    StreamByteOps.ToBytes(ref sequenceIdptr, previousSerializedSequenceId);
                }
                else
                {
                    previousSerializedSequenceId = unchecked(++pqL0Quote.PQSequenceId);
                    StreamByteOps.ToBytes(ref sequenceIdptr, previousSerializedSequenceId);
                }
            }

            *flagsPtr = serializationFlags == PQSerializationFlags.ForSocketPublish ? (byte)resolvedFlags : (byte)resolvedStorageFlags;

            pqL0Quote.HasUpdates = false;

            var written = (int)(currentPtr - writeStart);

            if ((resolvedStorageFlags & StorageFlags.OneByteMessageSize) > 0 && written > byte.MaxValue)
                throw new Exception($"Expected bytes written to be less than {byte.MaxValue}");
            if ((resolvedStorageFlags & StorageFlags.TwoByteMessageSize) > 0 && written > ushort.MaxValue)
                throw new Exception($"Expected bytes written to be less than {ushort.MaxValue}");
            if ((resolvedStorageFlags & StorageFlags.ThreeByteMessageSize) > 0 && written > 0x00FF_FFFF)
                throw new Exception($"Expected bytes written to be less than {0x00FF_FFFF}");
            // var level0Quote = (PQLevel0Quote)message;
            // logger.Debug($"{TimeContext.LocalTimeNow:O} {level0Quote.SourceTickerQuoteInfo.Source}-" +
            //              $"{level0Quote.SourceTickerQuoteInfo.Ticker}:" +
            //              $"{level0Quote.PQSequenceId}-> wrote {written} bytes for " +
            //              $"{resolvedFlags}.  ThreadName {Thread.CurrentThread.Name}");
            StreamByteOps.ToBytes(ref messageSizePtr, (uint)written);
            return FinishProcessingMessageReturnValue(message, written);
        }
        finally
        {
            pqL0Quote.Lock.Release();
        }
    }

    private int FinishProcessingMessageReturnValue(IVersionedMessage message, int response)
    {
        message.DecrementRefCount();
        return response;
    }
}
