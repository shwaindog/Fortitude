// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Serialization;

public class ExpectSessionCloseSerializer : IMessageSerializer<ExpectSessionCloseMessage>
{
    public MarshalType MarshalType => MarshalType.Binary;

    public bool AddMessageHeader { get; set; } = true;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((ExpectSessionCloseMessage)message, (ISerdeContext)writeContext);
    }

    public void Serialize(ExpectSessionCloseMessage obj, ISerdeContext writeContext)
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

    public unsafe int Serialize(IBuffer buffer, ExpectSessionCloseMessage expectSessionCloseMessage)
    {
        using var fixedBuffer     = buffer;
        var       remainingBytes  = buffer.RemainingStorage;
        var       closeReasonText = expectSessionCloseMessage.ReasonText;
        if (MessageHeader.SerializationSize + 1 <= remainingBytes)
        {
            var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            var currPtr    = writeStart;

            byte* messageSize = null;

            if (AddMessageHeader)
            {
                *currPtr++ = expectSessionCloseMessage.Version;
                *currPtr++ = closeReasonText != null ? (byte)1 : (byte)0;
                StreamByteOps.ToBytes(ref currPtr, expectSessionCloseMessage.MessageId);
                messageSize =  currPtr;
                currPtr     += sizeof(uint);
            }
            *currPtr++ = (byte)expectSessionCloseMessage.CloseReason;

            remainingBytes -= MessageHeader.SerializationSize + 1;
            if (closeReasonText != null) StreamByteOps.ToBytesWithSizeHeader(ref currPtr, closeReasonText!, remainingBytes);
            var amtWritten = currPtr - writeStart;
            if (AddMessageHeader) StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
            expectSessionCloseMessage.DecrementRefCount();
            return (int)amtWritten;
        }

        return -1;
    }
}
