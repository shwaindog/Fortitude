// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Serialization;

public class RequesterNameMessageSerializer : IMessageSerializer<RequesterNameMessage>
{
    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((RequesterNameMessage)message, (ISerdeContext)writeContext);
    }

    public void Serialize(RequesterNameMessage obj, ISerdeContext writeContext)
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

    public unsafe int Serialize(IBuffer buffer, RequesterNameMessage requesterNameMessage)
    {
        using var fixedBuffer = buffer;

        var remainingBytes          = buffer.RemainingStorage;
        var requesterConnectionName = requesterNameMessage.RequesterConnectionName;

        if (MessageHeader.SerializationSize <= remainingBytes)
        {
            var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            var currPtr    = writeStart;

            byte* messageSize = null;

            if (AddMessageHeader)
            {
                *currPtr++ = requesterNameMessage.Version;
                *currPtr++ = 0;
                StreamByteOps.ToBytes(ref currPtr, requesterNameMessage.MessageId);
                messageSize =  currPtr;
                currPtr     += sizeof(uint);
            }
            remainingBytes -= MessageHeader.SerializationSize;
            StreamByteOps.ToBytesWithSizeHeader(ref currPtr, requesterConnectionName!, remainingBytes);
            var amtWritten = currPtr - writeStart;
            if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);

            requesterNameMessage.DecrementRefCount();
            return (int)amtWritten;
        }

        return -1;
    }
}
