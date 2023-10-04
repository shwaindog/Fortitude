using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Protocols.ORX.Serialization
{
    public sealed class OrxSerializer<Tm> : OrxByteSerializer<Tm>, IBinarySerializer<Tm> where Tm : class
    {
        public readonly ushort Id;
        public OrxSerializer(ushort id)
        {
            Id = id;
        }

        public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage msg)
        {
            // We want to make sure that at least the header will fit
            if (OrxDecoder.HeaderSize <= buffer.Length - writeOffset)
            {
                ushort size = (ushort)Serialize(msg, buffer, writeOffset, OrxDecoder.HeaderSize);
                if (size > 0 || msg is OrxLogonResponse)
                {
                    fixed (byte* fptr = buffer)
                    {
                        byte* ptr = fptr + writeOffset;
                        *(ptr++) = msg.Version;
                        StreamByteOps.ToBytes(ref ptr, Id);
                        StreamByteOps.ToBytes(ref ptr, size);
                    }

                    if (msg is IRecycleableObject recycleableObject && recycleableObject.ShouldAutoRecycle)
                    {
                        recycleableObject.Recycler?.Recycle(recycleableObject);
                    }
                    return size + OrxDecoder.HeaderSize;
                }
            }
            return -1;
        }
    }
}
