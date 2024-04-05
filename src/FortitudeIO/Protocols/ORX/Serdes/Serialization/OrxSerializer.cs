#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public sealed class OrxSerializer<Tm> : OrxByteSerializer<Tm>, IMessageSerializer<Tm>
    where Tm : class, IVersionedMessage, new()
{
    public readonly ushort Id;

    public OrxSerializer(ushort id) => Id = id;

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
            var size = (ushort)Serialize(msg, buffer, writeOffset, OrxMessageHeader.HeaderSize);
            if (size > 0 || msg is OrxLogonResponse)
            {
                fixed (byte* fptr = buffer)
                {
                    var ptr = fptr + writeOffset;
                    *ptr++ = msg.Version;
                    StreamByteOps.ToBytes(ref ptr, Id);
                    StreamByteOps.ToBytes(ref ptr, size);
                }

                msg.DecrementRefCount();
                return size + OrxMessageHeader.HeaderSize;
            }
        }

        return -1;
    }
}
