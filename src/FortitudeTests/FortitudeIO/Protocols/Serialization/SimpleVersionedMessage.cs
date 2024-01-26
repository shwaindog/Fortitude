#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;

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

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<SimpleVersionedMessage>().CopyFrom(this) ?? new SimpleVersionedMessage(this);


    public class SimpleDeserializer : MessageDeserializer<SimpleVersionedMessage>
    {
        public override unsafe SimpleVersionedMessage Deserialize(ISerdeContext readContext)
        {
            if (readContext is DispatchContext dispatchContext)
            {
                var simpleMessage = new SimpleVersionedMessage();

                if (dispatchContext.MessageVersion == 1 && dispatchContext.MessageSize == 9)
                    fixed (byte* ptr = dispatchContext.EncodedBuffer!.Buffer)
                    {
                        var currPtr = ptr + 1;
                        simpleMessage.Version = dispatchContext.MessageVersion;
                        simpleMessage.MessageId = StreamByteOps.ToUShort(ref currPtr);
                        StreamByteOps.ToUShort(ref currPtr);
                        simpleMessage.PayLoad = StreamByteOps.ToInt(ref currPtr);
                    }
                else
                    fixed (byte* ptr = dispatchContext.EncodedBuffer!.Buffer)
                    {
                        var currPtr = ptr + 1;
                        simpleMessage.Version = dispatchContext.MessageVersion;
                        simpleMessage.MessageId = StreamByteOps.ToUShort(ref currPtr);
                        StreamByteOps.ToUShort(ref currPtr);
                        simpleMessage.PayLoad2 = StreamByteOps.ToDouble(ref currPtr);
                    }

                Dispatch(simpleMessage, dispatchContext.MessageHeader, dispatchContext.Conversation
                    , dispatchContext.DispatchLatencyLogger);
                return simpleMessage;
            }
            else
            {
                throw new ArgumentException("Expected readContext to be of type DispatchContext");
            }
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
                    , bufferContext.EncodedBuffer.WrittenCursor
                    , obj);
                bufferContext.EncodedBuffer.WrittenCursor += writeLength;
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
