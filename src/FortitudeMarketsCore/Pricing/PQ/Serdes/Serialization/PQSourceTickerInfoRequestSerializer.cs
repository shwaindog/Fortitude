#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public class PQSourceTickerInfoRequestSerializer : IMessageSerializer<PQSourceTickerInfoRequest>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(uint) + sizeof(uint);

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((PQSourceTickerInfoRequest)message, (ISerdeContext)writeContext);
    }

    public void Serialize(PQSourceTickerInfoRequest obj, ISerdeContext writeContext)
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

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
    {
        fixed (byte* bufStrt = buffer)
        {
            var writeStart = bufStrt + writeOffset;
            var currPtr = writeStart;
            *currPtr++ = message.Version;
            *currPtr++ = (byte)PQMessageFlags.None;
            ; // header flags
            StreamByteOps.ToBytes(ref currPtr, message.MessageId);
            var amtWritten = currPtr - writeStart + OrxConstants.UInt32Sz;
            StreamByteOps.ToBytes(ref currPtr, (uint)amtWritten);
            message.DecrementRefCount();
            return (int)amtWritten;
        }
    }
}
