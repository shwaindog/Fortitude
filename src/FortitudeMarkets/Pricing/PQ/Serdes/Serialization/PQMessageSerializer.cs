// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public enum PQSerializationFlags
{
    ForSocketPublish               = 0
  , ForStorage                     = 1
  , ForStorageIncludeReceiverTimes = 3
}

[Flags]
public enum PQMessageFlags
{
    // Start From PQMessageFlags
    None     = 0
  , Complete = 1
  , Snapshot = 3
  , Update   = 4

  , CompleteUpdate           = 5
  , IncludeReceiverTimes     = 8
  , IsNotLastMessageOfUpdate = 16
    // end from PQMessageFlags
  , OneByteMessageSize   = 32
  , TwoByteMessageSize   = 64
  , ThreeByteMessageSize = 96
  , IncludesSequenceId   = 128
}

public static class PQMessageFlagsExtensions
{
    public static bool HasCompleteFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.Complete) > 0;

    public static bool HasSnapshotFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.Snapshot) > 0;

    public static bool HasUpdateFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.Update) > 0;

    public static bool HasCompleteUpdateFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.CompleteUpdate) > 0;

    public static bool HasIncludeReceiverTimesFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.IncludeReceiverTimes) > 0;

    public static bool HasIsNotLastMessageOfUpdateFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.IsNotLastMessageOfUpdate) > 0;

    public static bool HasOneByteMessageSizeFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.OneByteMessageSize) > 0;

    public static bool HasTwoByteMessageSizeFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.TwoByteMessageSize) > 0;

    public static bool HasThreeByteMessageSizeFlag
        (this PQMessageFlags flags) =>
        (flags & PQMessageFlags.ThreeByteMessageSize) == PQMessageFlags.ThreeByteMessageSize;

    public static bool HasIncludesSequenceIdFlag(this PQMessageFlags flags) => (flags & PQMessageFlags.IncludesSequenceId) > 0;
}

public sealed class PQMessageSerializer : IMessageSerializer<PQPublishableTickInstant>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQMessageSerializer));

    private const int ThreeByteInt = 0xFF_FF_FF;

    private readonly Messages.FeedEvents.Quotes.PQMessageFlags messageFlags;

    private readonly PQSerializationFlags      serializationFlags;
    private readonly List<PQFieldUpdate>       fieldsToSerialize        = new();
    private readonly List<PQFieldStringUpdate> stringUpdatesToSerialize = new();

    //                                          version + flags +    streamId   + messageSize  + sequenceId
    private const int SocketPublishHeaderSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint) + sizeof(uint);

    private uint previousSerializedSequenceId;

    public PQMessageSerializer
        (Messages.FeedEvents.Quotes.PQMessageFlags messageFlags, PQSerializationFlags serializationFlags = PQSerializationFlags.ForSocketPublish)
    {
        this.messageFlags       = messageFlags;
        this.serializationFlags = serializationFlags;
    }

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    void IMessageSerializer.Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQPublishableTickInstant)message, writeContext);
    }

    public void Serialize(PQPublishableTickInstant obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IMessageBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer, obj);

            if (writeLength > 0) bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IMessageBufferContext");
        }
    }

    public unsafe int Serialize(IMessageQueueBuffer mesgBuffer, IVersionedMessage message)
    {
        if (!(message is IPQMessage pqMessage)) return FinishProcessingMessageReturnValue(message, -1);
        var resolvedFlags = (PQMessageFlags)(byte)(pqMessage.OverrideSerializationFlags ?? messageFlags);
        resolvedFlags |= serializationFlags == PQSerializationFlags.ForStorageIncludeReceiverTimes
            ? PQMessageFlags.IncludeReceiverTimes
            : PQMessageFlags.None;
        var resolvedStorageFlags = (PQMessageFlags)(byte)resolvedFlags;
        var publishAll           = (resolvedFlags & PQMessageFlags.Complete) > 0;
        pqMessage.OverrideSerializationFlags = null;
        using var fixedBuffer = mesgBuffer;
        if ((publishAll ? sizeof(uint) : 0) + PQQuoteMessageHeader.HeaderSize > mesgBuffer.RemainingStorage)
            return FinishProcessingMessageReturnValue(message, -1);
        if (!publishAll && !pqMessage.HasUpdates) return FinishProcessingMessageReturnValue(message, 0);
        var writeStart  = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var writeCursor = fixedBuffer.BufferRelativeWriteCursor;
        var currentPtr  = writeStart;
        var end         = writeStart + mesgBuffer.RemainingStorage;
        fieldsToSerialize.Clear();
        stringUpdatesToSerialize.Clear();
        pqMessage.Lock.Acquire();
        var estimatedTotalBytes = 0;
        var fieldIndex          = 0;
        var stringIndex         = 0;
        var totalWritten        = 0;
        try
        {
            fieldsToSerialize.AddRange(pqMessage.GetDeltaUpdateFields(TimeContext.UtcNow, resolvedFlags));
            stringUpdatesToSerialize.AddRange(pqMessage.GetStringUpdates(TimeContext.UtcNow, resolvedFlags));
            var countFieldsBytes  = fieldsToSerialize.Sum(fu => fu.RequiredBytes());
            var countStringBytes  = stringUpdatesToSerialize.Sum(fsu => fsu.RequiredBytes());
            var socketPublishSize = SocketPublishHeaderSize + countFieldsBytes + countStringBytes;

            bool isPossibleMultiMessage = mesgBuffer.EnforceCappedMessageSize && mesgBuffer.MaximumMessageSize < socketPublishSize;
            var  messageSizeBytes       = 4;
            var  publishMessageSize     = isPossibleMultiMessage ? mesgBuffer.MaximumMessageSize : socketPublishSize;
            if (serializationFlags != PQSerializationFlags.ForSocketPublish)
            {
                switch (publishMessageSize)
                {
                    case <= byte.MaxValue:
                        messageSizeBytes     =  1;
                        resolvedStorageFlags |= PQMessageFlags.OneByteMessageSize;
                        break;
                    case <= ushort.MaxValue:
                        messageSizeBytes     =  2;
                        resolvedStorageFlags |= PQMessageFlags.TwoByteMessageSize;
                        break;
                    case <= ThreeByteInt:
                        messageSizeBytes     =  3;
                        resolvedStorageFlags |= PQMessageFlags.ThreeByteMessageSize;
                        break;
                }
            }
            do
            {
                var messageEnd = isPossibleMultiMessage ? currentPtr + Math.Min((int)mesgBuffer.MaximumMessageSize, end - currentPtr) : end;

                if (serializationFlags == PQSerializationFlags.ForSocketPublish) *currentPtr++ = message.Version; //protocol version

                var flagsPtr = currentPtr++;
                if (serializationFlags == PQSerializationFlags.ForSocketPublish) StreamByteOps.ToBytes(ref currentPtr, pqMessage.StreamId);

                var messageSizePtr  = currentPtr;
                var sequenceIdBytes = 4;
                var sequenceId      = previousSerializedSequenceId + 1;
                if (serializationFlags == PQSerializationFlags.ForSocketPublish)
                {
                    currentPtr += 4;
                }
                else
                {
                    sequenceIdBytes =  pqMessage.PQSequenceId == sequenceId && !publishAll ? 0 : 4;
                    currentPtr      += messageSizeBytes;
                }

                if (sequenceIdBytes > 0)
                {
                    resolvedStorageFlags |= PQMessageFlags.IncludesSequenceId;
                    var isSnapshot = (resolvedFlags & PQMessageFlags.Snapshot) > 0;
                    if (isSnapshot) // should never be a multi-update message
                    {
                        if (isPossibleMultiMessage)
                            throw new ArgumentException("Snapshots should be done via TCP and not limited to multi-message limits");
                        sequenceId = pqMessage.PQSequenceId == uint.MaxValue ? 0 : pqMessage.PQSequenceId;
                        StreamByteOps.ToBytes(ref currentPtr, sequenceId);
                    }
                    else
                    {
                        sequenceId = unchecked(pqMessage.PQSequenceId++);
                        StreamByteOps.ToBytes(ref currentPtr, sequenceId);
                    }
                }
                else
                {
                    pqMessage.PQSequenceId++;
                }

                while (fieldIndex < fieldsToSerialize.Count)
                {
                    var field = fieldsToSerialize[fieldIndex];
                    // logger.Info("se-{0}-{1}", sequenceId, field);
                    // Console.Out.WriteLine("se-{0}-{1}", sequenceId, field);

                    if (currentPtr + field.RequiredBytes() > messageEnd)
                    {
                        if (currentPtr + field.RequiredBytes() + SocketPublishHeaderSize > end)
                        {
                            return FinishProcessingMessageReturnValue(message, -1);
                        }
                        resolvedFlags        |= PQMessageFlags.IsNotLastMessageOfUpdate;
                        resolvedStorageFlags |= PQMessageFlags.IsNotLastMessageOfUpdate;
                        break;
                    }
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
                    if (field.Flag.HasSubIdFlag()) *currentPtr++ = field.SubIdByte;
                    if (field.Flag.HasAuxiliaryPayloadFlag()) StreamByteOps.ToBytes(ref currentPtr, field.AuxiliaryPayload);

                    StreamByteOps.ToBytes(ref currentPtr, field.Payload);
                    fieldIndex++;
                }

                while (stringIndex < stringUpdatesToSerialize.Count)
                {
                    var fieldStringUpdate = stringUpdatesToSerialize[stringIndex];
                    if (currentPtr + fieldStringUpdate.RequiredBytes() > messageEnd)
                    {
                        if (currentPtr + fieldStringUpdate.RequiredBytes() + SocketPublishHeaderSize > end)
                        {
                            return FinishProcessingMessageReturnValue(message, -1);
                        }
                        resolvedFlags        |= PQMessageFlags.IsNotLastMessageOfUpdate;
                        resolvedStorageFlags |= PQMessageFlags.IsNotLastMessageOfUpdate;
                        break;
                    }

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
                    if (fieldStringUpdate.Field.Flag.HasSubIdFlag()) *currentPtr++ = fieldStringUpdate.Field.SubIdByte;
                    if (fieldStringUpdate.Field.Flag.HasAuxiliaryPayloadFlag())
                        StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.Field.AuxiliaryPayload);
                    var stringSizePtr = currentPtr;
                    currentPtr += 4;
                    StreamByteOps.ToBytes(ref currentPtr, fieldStringUpdate.StringUpdate.DictionaryId);
                    var bytesUsed = StreamByteOps.ToBytes(ref currentPtr,
                                                          fieldStringUpdate.StringUpdate.Value, (int)(end - currentPtr));
                    StreamByteOps.ToBytes(ref stringSizePtr, (uint)bytesUsed);

                    // var finalStringUpdate = fieldStringUpdate;
                    // logger.Info("se-{0}-{1}, size-{2}", sequenceId, finalStringUpdate, bytesUsed);
                    // Console.Out.WriteLine("se-{0}-{1}, size-{2}", sequenceId, finalStringUpdate, bytesUsed);
                    stringIndex++;
                }
                // Console.Out.WriteLine("");
                // Console.Out.WriteLine("");

                *flagsPtr = serializationFlags == PQSerializationFlags.ForSocketPublish ? (byte)resolvedFlags : (byte)resolvedStorageFlags;

                previousSerializedSequenceId = sequenceId;

                var written = (int)(currentPtr - writeStart);

                if ((resolvedStorageFlags & (PQMessageFlags.OneByteMessageSize | PQMessageFlags.ThreeByteMessageSize)) ==
                    PQMessageFlags.OneByteMessageSize && written > byte.MaxValue)
                    throw new Exception($"Expected bytes written would be {estimatedTotalBytes} bytes but it was {written}");
                if ((resolvedStorageFlags & (PQMessageFlags.TwoByteMessageSize | PQMessageFlags.ThreeByteMessageSize)) ==
                    PQMessageFlags.TwoByteMessageSize && written > ushort.MaxValue)
                    throw new Exception($"Expected bytes written to be less than {ushort.MaxValue}");
                if ((resolvedStorageFlags & PQMessageFlags.ThreeByteMessageSize) ==
                    PQMessageFlags.ThreeByteMessageSize && written > 0x00FF_FFFF)
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
                if (mesgBuffer.EnforceCappedMessageSize)
                {
                    mesgBuffer.QueueMessage(new MessageBufferEntry(writeCursor + totalWritten, written));
                    writeStart = currentPtr;
                }
                totalWritten += written;
            } while (fieldIndex < fieldsToSerialize.Count || stringIndex < stringUpdatesToSerialize.Count);
            pqMessage.HasUpdates = false;
            return FinishProcessingMessageReturnValue(message, totalWritten);
        }
        finally
        {
            pqMessage.Lock.Release();
        }
    }

    private int FinishProcessingMessageReturnValue(IVersionedMessage message, int response)
    {
        message.DecrementRefCount();
        return response;
    }
}
