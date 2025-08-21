// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeTests.FortitudeIO.Protocols.Serialization;

public class SimpleVersionedMessage : ReusableObject<IVersionedMessage>, IVersionedMessage
{
    public SimpleVersionedMessage() { }

    private SimpleVersionedMessage(SimpleVersionedMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public int    Payload   { get; set; }
    public double Payload2  { get; set; }
    public uint   MessageId { get; set; } = 2345;
    public byte   Version   { get; set; } = 2;

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is SimpleVersionedMessage simpleVersionedMessage)
        {
            Payload  = simpleVersionedMessage.Payload;
            Payload2 = simpleVersionedMessage.Payload2;
        }

        return this;
    }

    public override IVersionedMessage Clone() => Recycler?.Borrow<SimpleVersionedMessage>().CopyFrom(this) ?? new SimpleVersionedMessage(this);


    public class SimpleDeserializer : MessageDeserializer<SimpleVersionedMessage>
    {
        public override unsafe SimpleVersionedMessage Deserialize(ISerdeContext readContext)
        {
            if (readContext is IMessageBufferContext messageBufferContext)
            {
                var       simpleMessage = new SimpleVersionedMessage();
                using var fixedBuffer   = messageBufferContext.EncodedBuffer!;
                var       readOffset    = fixedBuffer.BufferRelativeReadCursor;
                var       version       = messageBufferContext.MessageHeader.Version;
                var       messageSize   = messageBufferContext.MessageHeader.MessageSize;

                simpleMessage.MessageId = messageBufferContext.MessageHeader.MessageId;
                simpleMessage.Version   = messageBufferContext.MessageHeader.Version;
                if (version == 1 || messageSize == 14)
                {
                    var currPtr = fixedBuffer.ReadBuffer + readOffset;
                    simpleMessage.Payload = StreamByteOps.ToInt(ref currPtr);
                }
                else
                {
                    var currPtr = fixedBuffer.ReadBuffer + readOffset;
                    simpleMessage.Payload2 = StreamByteOps.ToDouble(ref currPtr);
                }

                OnNotify(simpleMessage, messageBufferContext);

                return simpleMessage;
            }

            throw new ArgumentException("Expected readContext to be of type IBufferContext");
        }

        public override IMessageDeserializer Clone() => this;
    }

    public class SimpleSerializer : IMessageSerializer<SimpleVersionedMessage>
    {
        public MarshalType MarshalType => MarshalType.Binary;

        public bool AddMessageHeader { get; set; } = true; // ignored for this Serializer

        public void Serialize(IVersionedMessage message, IBufferContext writeContext)
        {
            Serialize((SimpleVersionedMessage)message, (ISerdeContext)writeContext);
        }

        public void Serialize(SimpleVersionedMessage obj, ISerdeContext writeContext)
        {
            if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected readContext to support writing");
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

        public unsafe int Serialize(IBuffer buffer, IVersionedMessage message)
        {
            var       simpleVersionedMsg = (SimpleVersionedMessage)message;
            using var fixedBuffer        = buffer;
            var       currPtr            = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            *currPtr++ = message.Version;
            *currPtr++ = 0;
            StreamByteOps.ToBytes(ref currPtr, message.MessageId);
            var bytesSerialized = (uint)(message.Version > 1 ? 18 : 14);
            StreamByteOps.ToBytes(ref currPtr, bytesSerialized);
            if (message.Version == 1)
                StreamByteOps.ToBytes(ref currPtr, simpleVersionedMsg.Payload);
            else
                StreamByteOps.ToBytes(ref currPtr, simpleVersionedMsg.Payload2);

            message.DecrementRefCount();

            return (int)bytesSerialized;
        }
    }
}
