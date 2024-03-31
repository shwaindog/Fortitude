#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
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

    public int PayLoad { get; set; }
    public double PayLoad2 { get; set; }
    public uint MessageId { get; set; } = 2345;
    public byte Version { get; set; } = 2;

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is SimpleVersionedMessage simpleVersionedMessage)
        {
            PayLoad = simpleVersionedMessage.PayLoad;
            PayLoad2 = simpleVersionedMessage.PayLoad2;
        }

        return this;
    }

    public override IVersionedMessage Clone() => Recycler?.Borrow<SimpleVersionedMessage>().CopyFrom(this) ?? new SimpleVersionedMessage(this);


    public class SimpleDeserializer : MessageDeserializer<SimpleVersionedMessage>
    {
        public override unsafe SimpleVersionedMessage Deserialize(ISerdeContext readContext)
        {
            if (readContext is IBufferContext bufferContext)
            {
                var simpleMessage = new SimpleVersionedMessage();
                var readOffset = bufferContext.EncodedBuffer!.ReadCursor;
                var version = bufferContext.EncodedBuffer!.Buffer[bufferContext.EncodedBuffer.ReadCursor];
                var messageId = StreamByteOps.ToUShort(bufferContext.EncodedBuffer.Buffer
                    , bufferContext.EncodedBuffer.ReadCursor + 1);
                var messageSize = StreamByteOps.ToUShort(bufferContext.EncodedBuffer.Buffer
                    , bufferContext.EncodedBuffer.ReadCursor + 3);

                if (version == 1 && messageSize == 9)
                    fixed (byte* ptr = bufferContext.EncodedBuffer!.Buffer)
                    {
                        simpleMessage.Version = version;
                        simpleMessage.MessageId = messageId;
                        var currPtr = ptr + readOffset + 5;
                        simpleMessage.PayLoad = StreamByteOps.ToInt(ref currPtr);
                    }
                else
                    fixed (byte* ptr = bufferContext.EncodedBuffer!.Buffer)
                    {
                        simpleMessage.Version = version;
                        simpleMessage.MessageId = messageId;
                        var currPtr = ptr + readOffset + 5;
                        simpleMessage.PayLoad2 = StreamByteOps.ToDouble(ref currPtr);
                    }

                OnNotify(simpleMessage, bufferContext);

                return simpleMessage;
            }

            throw new ArgumentException("Expected readContext to be of type IBufferContext");
        }
    }

    public class SimpleSerializer : IMessageSerializer<SimpleVersionedMessage>
    {
        public MarshalType MarshalType => MarshalType.Binary;

        public void Serialize(IVersionedMessage message, IBufferContext writeContext)
        {
            Serialize((SimpleVersionedMessage)message, (ISerdeContext)writeContext);
        }

        public void Serialize(SimpleVersionedMessage obj, ISerdeContext writeContext)
        {
            if ((writeContext.Direction & ContextDirection.Write) == 0)
                throw new ArgumentException("Expected readContext to support writing");
            if (writeContext is IBufferContext bufferContext)
            {
                var writeLength = Serialize(bufferContext.EncodedBuffer!.Buffer
                    , bufferContext.EncodedBuffer.WriteCursor
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
            var simpleVersionedMsg = (SimpleVersionedMessage)message;
            ushort bytesSerialized;
            fixed (byte* fptr = buffer)
            {
                var currPtr = fptr;
                *currPtr++ = message.Version;
                StreamByteOps.ToBytes(ref currPtr, (ushort)message.MessageId);
                bytesSerialized = (ushort)(message.Version > 1 ? 13 : 9);
                StreamByteOps.ToBytes(ref currPtr, bytesSerialized);
                if (message.Version == 1)
                    StreamByteOps.ToBytes(ref currPtr, simpleVersionedMsg.PayLoad);
                else
                    StreamByteOps.ToBytes(ref currPtr, simpleVersionedMsg.PayLoad2);
            }

            message?.DecrementRefCount();

            return bytesSerialized;
        }
    }
}
