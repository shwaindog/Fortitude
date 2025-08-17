#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary.Deserialization;

public class ExpectSessionCloseMessageDeserializer : BinaryMessageDeserializer<ExpectSessionCloseMessage>
{
    private readonly IRecycler recycler;

    public ExpectSessionCloseMessageDeserializer(IRecycler recycler) => this.recycler = recycler;

    public ExpectSessionCloseMessageDeserializer(ExpectSessionCloseMessageDeserializer toClone) => recycler = toClone.recycler;

    public override unsafe ExpectSessionCloseMessage Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedExpectSessionCloseMessage = recycler.Borrow<ExpectSessionCloseMessage>();
            var fixedBuffer = messageBufferContext.EncodedBuffer!;
            var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            deserializedExpectSessionCloseMessage.CloseReason = (CloseReason)(*ptr++);
            if (messageBufferContext.MessageHeader.Flags > 0)
                deserializedExpectSessionCloseMessage.ReasonText = StreamByteOps.ToStringWithSizeHeader(ref ptr);

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            return deserializedExpectSessionCloseMessage;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => this;
}
