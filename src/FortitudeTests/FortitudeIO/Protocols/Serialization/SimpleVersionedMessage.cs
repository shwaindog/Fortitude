#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
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


    public class SimpleDeserializer : BinaryDeserializer<SimpleVersionedMessage>
    {
        public override unsafe object Deserialize(DispatchContext dispatchContext)
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
    }

    public class SimpleSerializer : IBinarySerializer<SimpleVersionedMessage>
    {
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

            return bytesSerialized;
        }
    }
}
