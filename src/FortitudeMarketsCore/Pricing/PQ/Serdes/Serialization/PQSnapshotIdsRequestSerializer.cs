#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public class PQSnapshotIdsRequestSerializer : IMessageSerializer<PQSnapshotIdsRequest>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(ushort);

    public MarshalType MarshalType => MarshalType.Binary;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer, bufferContext.EncodedBuffer.WriteCursor
                , obj);
            bufferContext.EncodedBuffer.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message)
    {
        var ids = ((IPQSnapshotIdsRequest)message).RequestSourceTickerIds;
        if (ids != null && FixedSize + ids.Length * sizeof(uint) <= buffer.Length - writeOffset)
            fixed (byte* bufStrt = buffer)
            {
                var writeStart = bufStrt + writeOffset;
                var currPtr = writeStart;
                *currPtr++ = message.Version;
                *currPtr++ = 0; // header flags
                var messageSize = currPtr;
                currPtr += OrxConstants.UInt16Sz;
                StreamByteOps.ToBytes(ref currPtr, (ushort)ids.Length);
                for (var i = 0; i < ids.Length; i++) StreamByteOps.ToBytes(ref currPtr, ids[i]);

                var amtWritten = currPtr - writeStart;
                StreamByteOps.ToBytes(ref messageSize, (ushort)amtWritten);
                message.DecrementRefCount();
                return (int)amtWritten;
            }

        return -1;
    }
}
