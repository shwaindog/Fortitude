#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization;

public class PQSnapshotIdsRequestSerializer : IBinarySerializer<uint[]>
{
    private const int FixedSize = 2 * sizeof(byte) + sizeof(ushort);

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
