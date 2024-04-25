#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Serialization;

public class ExpectSessionCloseSerializer : IMessageSerializer<ExpectSessionCloseMessage>
{
    public MarshalType MarshalType => MarshalType.Binary;

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

    public unsafe int Serialize(byte[] buffer, int writeOffset, ExpectSessionCloseMessage expectSessionCloseMessage)
    {
        var remainingBytes = buffer.Length - writeOffset;
        var closeReasonText = expectSessionCloseMessage.ReasonText;
        if (MessageHeader.SerializationSize + 1 <= buffer.Length - writeOffset)
            fixed (byte* bufStrt = buffer)
            {
                var writeStart = bufStrt + writeOffset;
                var currPtr = writeStart;
                *currPtr++ = expectSessionCloseMessage.Version;
                *currPtr++ = closeReasonText != null ? (byte)1 : (byte)0;
                StreamByteOps.ToBytes(ref currPtr, expectSessionCloseMessage.MessageId);
                var messageSize = currPtr;
                currPtr += OrxConstants.UInt32Sz;
                *currPtr++ = (byte)expectSessionCloseMessage.CloseReason;
                remainingBytes -= MessageHeader.SerializationSize + 1;
                if (closeReasonText != null) StreamByteOps.ToBytesWithSizeHeader(ref currPtr, closeReasonText!, remainingBytes);
                var amtWritten = currPtr - writeStart;
                StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);

                expectSessionCloseMessage.DecrementRefCount();
                return (int)amtWritten;
            }

        return -1;
    }
}
