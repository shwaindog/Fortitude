#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Protocols.Serialization;

public class SimpleMessageStreamDecoder : IMessageStreamDecoder
{
    private readonly IDictionary<uint, IMessageDeserializer> deserializers
        = new Dictionary<uint, IMessageDeserializer>();

    public bool ZeroByteReadIsDisconnection { get; } = true;
    public int ExpectedSize { get; } = 1;
    public int NumberOfReceivesPerPoll { get; } = 1;

    public bool AddMessageDeserializer(uint msgId, IMessageDeserializer deserializer)
    {
        deserializers.Add(msgId, deserializer);
        return true;
    }

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers => deserializers;

    public unsafe int Process(ReadSocketBufferContext readSocketBufferContext)
    {
        var read = readSocketBufferContext.EncodedBuffer!.ReadCursor;
        var originalRead = read;
        ushort messageId;
        while (read < readSocketBufferContext.EncodedBuffer.WriteCursor)
        {
            fixed (byte* fptr = readSocketBufferContext.EncodedBuffer.Buffer)
            {
                var ptr = fptr + read;
                readSocketBufferContext.MessageVersion = *ptr++;
                messageId = StreamByteOps.ToUShort(ref ptr);
                readSocketBufferContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
            }

            if (deserializers.TryGetValue(messageId, out var u))
            {
                readSocketBufferContext.EncodedBuffer.ReadCursor = read;
                u.Deserialize(readSocketBufferContext);
            }

            read += readSocketBufferContext.MessageSize;
        }

        readSocketBufferContext.DispatchLatencyLogger?.Dedent();
        var amountRead = read - originalRead;
        readSocketBufferContext.EncodedBuffer.ReadCursor = read;
        return amountRead;
    }

    public class SimpleDeserializerFactory : IStreamDecoderFactory
    {
        private readonly IDictionary<uint, IMessageDeserializer> deserializers;

        public SimpleDeserializerFactory(IDictionary<uint, IMessageDeserializer> deserializers) => this.deserializers = deserializers;

        public int RegisteredDeserializerCount => 0;

        public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers =>
            Enumerable.Empty<KeyValuePair<uint, IMessageDeserializer>>();

        public void RegisterMessageDeserializer(uint id, IMessageDeserializer messageSerializer)
        {
            throw new NotImplementedException();
        }

        public void UnregisterMessageDeserializer(uint id)
        {
            throw new NotImplementedException();
        }

        public IMessageStreamDecoder Supply()
        {
            var streamDecoder = new SimpleMessageStreamDecoder();
            foreach (var binaryDeserializerEntry in deserializers)
                streamDecoder.AddMessageDeserializer(binaryDeserializerEntry.Key, binaryDeserializerEntry.Value);
            return streamDecoder;
        }
    }
}
