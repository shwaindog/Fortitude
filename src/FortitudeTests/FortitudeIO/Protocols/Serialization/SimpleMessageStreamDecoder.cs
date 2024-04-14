#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
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
        var read = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = read;
        ushort messageId;
        while (read < socketBufferReadContext.EncodedBuffer.WriteCursor)
        {
            fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
            {
                var ptr = fptr + read;
                var version = *ptr++;
                messageId = StreamByteOps.ToUShort(ref ptr);
                var messageSize = StreamByteOps.ToUShort(ref ptr);
                socketBufferReadContext.MessageHeader = new MessageHeader(version, 0, messageId, messageSize, socketBufferReadContext);
            }

            if (MessageDeserializationRepository.TryGetDeserializer(messageId, out var u))
            {
                socketBufferReadContext.EncodedBuffer.ReadCursor = read;
                u?.Deserialize(socketBufferReadContext);
            }

            read += (int)socketBufferReadContext.MessageHeader.MessageSize;
        }

        socketBufferReadContext.DispatchLatencyLogger?.Dedent();
        var amountRead = read - originalRead;
        socketBufferReadContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    public class SimpleDeserializerFactory : FactoryDeserializationRepository
    {
        public SimpleDeserializerFactory(IDictionary<uint, IMessageDeserializer> deserializers) : base(new Recycler())
        {
            foreach (var msgDesKvp in deserializers) RegisterDeserializer(msgDesKvp.Key, msgDesKvp.Value);
        }

        public override IMessageStreamDecoder Supply() => new SimpleMessageStreamDecoder(this);

        protected override IMessageDeserializer? SourceMessageDeserializer<TM>(uint msgId) =>
            throw new NotImplementedException("Creates no new Deserializers");
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
