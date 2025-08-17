// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public class PQSnapshotIdsRequestSerializer : IMessageSerializer<PQSnapshotIdsRequest>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);

    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQSnapshotIdsRequest)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQSnapshotIdsRequest obj, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected readContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength                                               = Serialize(bufferContext.EncodedBuffer!, obj);
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
        var ids = ((IPQSnapshotIdsRequest)message).RequestSourceTickerIds;

        using var fixedBuffer = buffer;

        if (FixedSize + ids.Count * sizeof(uint) > buffer.RemainingStorage) return -1;

        var   writeStart  = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var   currPtr     = writeStart;
        byte* messageSize = null;

        if (AddMessageHeader)
        {
            *currPtr++ = message.Version;
            *currPtr++ = (byte)Messages.FeedEvents.Quotes.PQMessageFlags.None; // header flags
            StreamByteOps.ToBytes(ref currPtr, message.MessageId);
            messageSize =  currPtr;
            currPtr     += sizeof(uint);
        }
        StreamByteOps.ToBytes(ref currPtr, (ushort)ids.Count);
        for (var i = 0; i < ids.Count; i++) StreamByteOps.ToBytes(ref currPtr, ids[i]);

        var amtWritten = currPtr - writeStart;
        if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        message.DecrementRefCount();
        return (int)amtWritten;
    }
}
