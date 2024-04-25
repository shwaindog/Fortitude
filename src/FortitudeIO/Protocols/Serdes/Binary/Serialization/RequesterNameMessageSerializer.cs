#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Serialization;

public class RequesterNameMessageSerializer : IMessageSerializer<RequesterNameMessage>
{
    public MarshalType MarshalType => MarshalType.Binary;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WriteCursor
                , obj);
            if (writeLength > 0) bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(byte[] buffer, int writeOffset, RequesterNameMessage requesterNameMessage)
    {
        var remainingBytes = buffer.Length - writeOffset;
        var requesterConnectionName = requesterNameMessage.RequesterConnectionName;
        if (MessageHeader.SerializationSize <= buffer.Length - writeOffset)
            fixed (byte* bufStrt = buffer)
            {
                var writeStart = bufStrt + writeOffset;
                var currPtr = writeStart;
                *currPtr++ = requesterNameMessage.Version;
                *currPtr++ = 0;
                StreamByteOps.ToBytes(ref currPtr, requesterNameMessage.MessageId);
                var messageSize = currPtr;
                currPtr += OrxConstants.UInt32Sz;
                remainingBytes -= MessageHeader.SerializationSize;
                StreamByteOps.ToBytesWithSizeHeader(ref currPtr, requesterConnectionName!, remainingBytes);
                var amtWritten = currPtr - writeStart;
                StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);

                requesterNameMessage.DecrementRefCount();
                return (int)amtWritten;
            }

        return -1;
    }
}
