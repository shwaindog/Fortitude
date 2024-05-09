#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);
            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, PQSourceTickerInfoRequest sourceTickerInfoRequest)
    {
        using var fixedBuffer = buffer;
        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var currPtr = writeStart;
        *currPtr++ = sourceTickerInfoRequest.Version;
        *currPtr++ = (byte)PQMessageFlags.None;
        StreamByteOps.ToBytes(ref currPtr, sourceTickerInfoRequest.MessageId);
        var messageSize = currPtr;
        currPtr += OrxConstants.UInt32Sz;
        StreamByteOps.ToBytes(ref currPtr, sourceTickerInfoRequest.RequestId);
        var amtWritten = currPtr - writeStart;
        StreamByteOps.ToBytes(ref messageSize, (uint)amtWritten);
        sourceTickerInfoRequest.DecrementRefCount();
        return (int)amtWritten;
    }
}
