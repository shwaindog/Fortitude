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
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

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
    None                 = 0
  , Complete             = 1
  , Snapshot             = 3
  , Update               = 4
  , CompleteUpdate       = 5
  , IncludeReceiverTimes = 8
  , NoChangeOrHeartbeat  = 16
    // end from PQMessageFlags
  , OneByteMessageSize   = 32
  , TwoByteMessageSize   = 64
  , ThreeByteMessageSize = 96
  , IncludesSequenceId   = 128
}

public sealed class PQQuoteSerializer : IMessageSerializer<PQTickInstant>
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

    public bool AddMessageHeader { get; set; } = true;

    void IMessageSerializer.Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQTickInstant)message, writeContext);
    }

    public void Serialize(PQTickInstant obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);

            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IVersionedMessage message)
    {
        if (!(message is IPQTickInstant pqTickInstant)) return FinishProcessingMessageReturnValue(message, -1);
        var resolvedFlags = (StorageFlags)(byte)(pqTickInstant.OverrideSerializationFlags ?? messageFlags);
        resolvedFlags |= serializationFlags == PQSerializationFlags.ForStorageIncludeReceiverTimes
            ? StorageFlags.IncludeReceiverTimes
            : StorageFlags.None;
        var resolvedStorageFlags = (StorageFlags)(byte)resolvedFlags;
        var publishAll           = (resolvedFlags & StorageFlags.Complete) > 0;
        pqTickInstant.OverrideSerializationFlags = null;
        using var fixedBuffer = buffer;
        if ((publishAll ? sizeof(uint) : 0) + PQQuoteMessageHeader.HeaderSize > buffer.RemainingStorage)
            return FinishProcessingMessageReturnValue(message, -1);
        if (!publishAll && !pqTickInstant.HasUpdates) return FinishProcessingMessageReturnValue(message, 0);
        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var currentPtr = writeStart;
        var end        = writeStart + buffer.RemainingStorage;
        fieldsToSerialize.Clear();
        stringUpdatesToSerialize.Clear();
        pqTickInstant.Lock.Acquire();
        var estimatedTotalBytes = 0;
        try
        {
            fieldsToSerialize.AddRange(pqTickInstant.GetDeltaUpdateFields(TimeContext.UtcNow, resolvedFlags));
            stringUpdatesToSerialize.AddRange(pqTickInstant.GetStringUpdates(TimeContext.UtcNow, resolvedFlags));

            if (serializationFlags == PQSerializationFlags.ForSocketPublish) *currentPtr++ = message.Version; //protocol version

            var flagsPtr = currentPtr++;
            if (serializationFlags == PQSerializationFlags.ForSocketPublish)
                StreamByteOps.ToBytes(ref currentPtr, pqTickInstant.SourceTickerInfo!.SourceTickerId);

            var messageSizePtr   = currentPtr;
            var messageSizeBytes = 4;
            var sequenceIdBytes  = 4;
            var sequenceId       = previousSerializedSequenceId + 1;
            if (serializationFlags == PQSerializationFlags.ForSocketPublish)
            {
                currentPtr += 4;
            }
            else if (stringUpdatesToSerialize.Any())
            {
                sequenceIdBytes      =  pqTickInstant.PQSequenceId == sequenceId && !publishAll ? 0 : 4;
                resolvedStorageFlags |= sequenceIdBytes > 0 ? StorageFlags.IncludesSequenceId : StorageFlags.None;
                resolvedStorageFlags |= StorageFlags.ThreeByteMessageSize;
                messageSizeBytes     =  3;
                currentPtr           += messageSizeBytes;
                estimatedTotalBytes  =  (ushort.MaxValue << 8) | byte.MaxValue;
            }
            else
            {
                sequenceIdBytes      =  pqTickInstant.PQSequenceId == sequenceId && !publishAll ? 0 : 4;
                resolvedStorageFlags |= sequenceIdBytes > 0 ? StorageFlags.IncludesSequenceId : StorageFlags.None;
                var countFieldsBytes = fieldsToSerialize.Sum(fu => fu.RequiredBytes());
                estimatedTotalBytes = countFieldsBytes + sequenceIdBytes + 4; // 1 for flags, 3 for possible message size bytes
                switch (estimatedTotalBytes)
                {
                    case < byte.MaxValue:
                        messageSizeBytes = 1;
                        currentPtr++;
                        resolvedStorageFlags |= StorageFlags.OneByteMessageSize;
                        break;
                    case < ushort.MaxValue:
                        messageSizeBytes     =  2;
                        currentPtr           += 2;
                        resolvedStorageFlags |= StorageFlags.TwoByteMessageSize;
                        break;
                    default:
                        messageSizeBytes     =  3;
                        currentPtr           += 3;
                        resolvedStorageFlags |= StorageFlags.ThreeByteMessageSize;
                        break;
                }
            }

            if (sequenceIdBytes > 0)
            {
                var isSnapshot = (resolvedFlags & StorageFlags.Snapshot) > 0;
                if (isSnapshot)
                {
                    sequenceId = pqTickInstant.PQSequenceId == uint.MaxValue ? 0 : pqTickInstant.PQSequenceId;
                    StreamByteOps.ToBytes(ref currentPtr, sequenceId);
                }
                else
                {
                    sequenceId = unchecked(pqTickInstant.PQSequenceId++);
                    StreamByteOps.ToBytes(ref currentPtr, sequenceId);
                }
            }
            else
            {
                pqTickInstant.PQSequenceId++;
            }


            foreach (var field in fieldsToSerialize)
            {
                // logger.Info("se-{0}-{1}", sequenceId, field);
                // Console.Out.WriteLine("se-{0}-{1}", sequenceId, field);

                if (currentPtr + FieldSize > end) return FinishProcessingMessageReturnValue(message, -1);
                *currentPtr++ = (byte)field.Flag;
                *currentPtr++ = (byte)field.Id;

                if (field.Flag.HasDepthKeyFlag())
                {
                    if (field.DepthId.IsTwoByteDepth())
                    {
                        StreamByteOps.ToBytes(ref currentPtr, (ushort)field.DepthId);
                    }
                    else
                    {
                        var depthByte = field.DepthId.ToByte();
                        *currentPtr++ = depthByte;
                    }
                }
                if (field.Flag.HasAuxiliaryPayloadFlag()) StreamByteOps.ToBytes(ref currentPtr, field.AuxiliaryPayload);
                if (field.Flag.HasExtendedPayloadFlag()) StreamByteOps.ToBytes(ref currentPtr, field.ExtendedPayload);

                StreamByteOps.ToBytes(ref currentPtr, field.Payload);
            }

            foreach (var fieldStringUpdate in stringUpdatesToSerialize)
            {
                if (currentPtr + 100 > end) return FinishProcessingMessageReturnValue(message, -1);

                *currentPtr++ = (byte)fieldStringUpdate.Field.Flag;
                *currentPtr++ = (byte)fieldStringUpdate.Field.Id;

                if (fieldStringUpdate.Field.Flag.HasDepthKeyFlag())
                {
                    if (fieldStringUpdate.Field.DepthId.IsTwoByteDepth())
                    {
                        StreamByteOps.ToBytes(ref currentPtr, (ushort)fieldStringUpdate.Field.DepthId);
                    }
                    else
                    {
                        var depthByte = fieldStringUpdate.Field.DepthId.ToByte();
                        *currentPtr++ = depthByte;
                    }
                }
                if (fieldStringUpdate.Field.Flag.HasAuxiliaryPayloadFlag())
                    StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.Field.AuxiliaryPayload);
                if (fieldStringUpdate.Field.Flag.HasExtendedPayloadFlag())
                    StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.Field.ExtendedPayload);
                var stringSizePtr = currentPtr;
                currentPtr += 4;
                StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.StringUpdate.DictionaryId);
                var bytesUsed = StreamByteOps.ToBytes(ref currentPtr,
                                                      fieldStringUpdate.StringUpdate.Value, (int)(end - currentPtr));
                StreamByteOps.ToBytes(ref stringSizePtr, (uint)bytesUsed);

                var finalStringUpdate = fieldStringUpdate;
                // logger.Info("se-{0}-{1}, size-{2}", sequenceId, finalStringUpdate, bytesUsed);
                // Console.Out.WriteLine("se-{0}-{1}, size-{2}", sequenceId, finalStringUpdate, bytesUsed);
            }
            // Console.Out.WriteLine("");
            // Console.Out.WriteLine("");

            *flagsPtr = serializationFlags == PQSerializationFlags.ForSocketPublish ? (byte)resolvedFlags : (byte)resolvedStorageFlags;

            pqTickInstant.HasUpdates     = false;
            previousSerializedSequenceId = sequenceId;

            var written = (int)(currentPtr - writeStart);

            if ((resolvedStorageFlags & (StorageFlags.OneByteMessageSize | StorageFlags.ThreeByteMessageSize)) ==
                StorageFlags.OneByteMessageSize && written > byte.MaxValue)
                throw new Exception($"Expected bytes written would be {estimatedTotalBytes} bytes but it was {written}");
            if ((resolvedStorageFlags & (StorageFlags.TwoByteMessageSize | StorageFlags.ThreeByteMessageSize)) ==
                StorageFlags.TwoByteMessageSize && written > ushort.MaxValue)
                throw new Exception($"Expected bytes written to be less than {ushort.MaxValue}");
            if ((resolvedStorageFlags & StorageFlags.ThreeByteMessageSize) ==
                StorageFlags.ThreeByteMessageSize && written > 0x00FF_FFFF)
                throw new Exception($"Expected bytes written to be less than {0x00FF_FFFF}");
            // var tickInstant = (PQTickInstant)message;
            // logger.Debug($"{TimeContext.LocalTimeNow:O} {tickInstant.SourceTickerInfo.Source}-" +
            //              $"{tickInstant.SourceTickerInfo.Ticker}:" +
            //              $"{tickInstant.PQSequenceId}-> wrote {written} bytes for " +
            //              $"{resolvedFlags}.  ThreadName {Thread.CurrentThread.Name}");
            if (messageSizeBytes > 3)
            {
                StreamByteOps.ToBytes(ref messageSizePtr, (uint)written);
            }
            else if (messageSizeBytes > 2)
            {
                var littleByte = (byte)(written & 0xFF);
                *messageSizePtr++ = littleByte;
                var largestTwoByte = (ushort)(written >> 8);
                StreamByteOps.ToBytes(ref messageSizePtr, largestTwoByte);
            }
            else if (messageSizeBytes > 1)
            {
                StreamByteOps.ToBytes(ref messageSizePtr, (ushort)written);
            }
            else
            {
                *messageSizePtr = (byte)(written & 0xFF);
            }
            return FinishProcessingMessageReturnValue(message, written);
        }
        finally
        {
            pqTickInstant.Lock.Release();
        }
    }

    private int FinishProcessingMessageReturnValue(IVersionedMessage message, int response)
    {
        message.DecrementRefCount();
        return response;
    }
}
