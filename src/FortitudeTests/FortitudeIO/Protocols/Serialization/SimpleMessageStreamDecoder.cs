#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Protocols.Serialization;

public class SimpleMessageStreamDecoder : IMessageStreamDecoder
{
    public SimpleMessageStreamDecoder(IMessageDeserializationRepository messageDeserializationRepository) =>
        MessageDeserializationRepository = messageDeserializationRepository;

    public IMessageDeserializationRepository MessageDeserializationRepository { get; }


    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        using var fixedBuffer = socketBufferReadContext.EncodedBuffer!;
        var originalRead = fixedBuffer.ReadCursor;
        uint messageId;
        while (fixedBuffer.ReadCursor < fixedBuffer.WriteCursor)
        {
            var ptr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            var version = *ptr++;
            var flags = *ptr++;
            messageId = StreamByteOps.ToUInt(ref ptr);
            var messageSize = StreamByteOps.ToUInt(ref ptr);
            socketBufferReadContext.MessageHeader = new MessageHeader(version, 0, messageId, messageSize, socketBufferReadContext);
            fixedBuffer.ReadCursor += MessageHeader.SerializationSize;

            if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var u))
            {
                var message = u?.Deserialize(socketBufferReadContext);
                if (message is ExpectSessionCloseMessage expectSessionCloseMessage)
                    socketBufferReadContext.SocketReceiver.ExpectSessionCloseMessage = expectSessionCloseMessage;
            }

            fixedBuffer.ReadCursor
                += (int)socketBufferReadContext.MessageHeader.MessageSize - MessageHeader.SerializationSize;
        }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = fixedBuffer.ReadCursor - originalRead;
        return (int)amountRead;
    }

    public class SimpleDeserializerMessageDeserializationFactory : MessageDeserializationFactoryRepository
    {
        public SimpleDeserializerMessageDeserializationFactory(IDictionary<uint, IMessageDeserializer> deserializers) : base(
            "SimpleDeserializerMessageDeserializationFactory", new Recycler())
        {
            foreach (var msgDesKvp in deserializers) RegisterDeserializer(msgDesKvp.Key, msgDesKvp.Value);
        }

        public override IMessageStreamDecoder Supply(string name) => new SimpleMessageStreamDecoder(this);

        public override IMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) =>
            throw new NotImplementedException("Creates no new Deserializers");

        public override INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId) =>
            throw new NotImplementedException();

        public override IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType) => throw new NotImplementedException();
    }

    public class SimpleSerializerFactory : FactorySerializationRepository
    {
        public SimpleSerializerFactory(IDictionary<uint, IMessageSerializer> serializers) : base(new Recycler())
        {
            foreach (var msgDesKvp in serializers) RegisterSerializer(msgDesKvp.Key, msgDesKvp.Value);
        }

        protected override IMessageSerializer? SourceMessageSerializer<TM>(uint msgId) =>
            throw new NotImplementedException("Creates no new Deserializers");
    }
}
