#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public sealed class OrxSerializer<Tm> : OrxByteSerializer<Tm>, IMessageSerializer<Tm>
    where Tm : class, IVersionedMessage, new()
{
    public readonly uint Id;

    public OrxSerializer(uint id) => Id = id;

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IVersionedMessage message, IBufferContext writeContext)
    {
        Serialize((Tm)message, (ISerdeContext)writeContext);
    }

    public void Serialize(Tm obj, ISerdeContext writeContext)
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

    public unsafe int Serialize(byte[] buffer, int writeOffset, IVersionedMessage msg)
    {
        // We want to make sure that at least the header will fit
        if (OrxMessageHeader.HeaderSize <= buffer.Length - writeOffset)
        {
            var size = Serialize(msg, buffer, writeOffset, OrxMessageHeader.HeaderSize) + OrxMessageHeader.HeaderSize;
            if (size >= OrxMessageHeader.HeaderSize)
            {
                fixed (byte* fptr = buffer)
                {
                    var ptr = fptr + writeOffset;
                    *ptr++ = msg.Version;
                    *ptr++ = 0;
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, (uint)size);
                }

                msg.DecrementRefCount();
                return (int)size;
            }
        }

        return -1;
    }
}
