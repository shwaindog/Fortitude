// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

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

    public bool AddMessageHeader { get; set; } = true;

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
            var writeLength = Serialize(bufferContext.EncodedBuffer!, obj);
            bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength            =  writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IVersionedMessage msg)
    {
        // We want to make sure that at least the header will fit
        if (OrxMessageHeader.HeaderSize <= buffer.RemainingStorage)
        {
            var size = Serialize(msg, buffer, OrxMessageHeader.HeaderSize) + OrxMessageHeader.HeaderSize;
            if (size >= OrxMessageHeader.HeaderSize && AddMessageHeader)
            {
                using var fixedBuffer = buffer;

                var fptr = fixedBuffer.WriteBuffer;
                var ptr  = fptr;

                *ptr++ = msg.Version;
                *ptr++ = 0;
                StreamByteOps.ToBytes(ref ptr, Id);
                StreamByteOps.ToBytes(ref ptr, (uint)size);

                msg.DecrementRefCount();
                return size;
            }
        }

        return -1;
    }
}
