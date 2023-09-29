using System.Collections.Generic;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;

namespace FortitudeTests.FortitudeIO.Protocols.Serialization
{
    public class SimpleStreamDecoder : IStreamDecoder
    {
        private IDictionary<uint, IBinaryDeserializer> deserializers = new Dictionary<uint, IBinaryDeserializer>();

        public bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer)
        {
            deserializers.Add(msgId, deserializer);
            return true;
        }

        public bool ZeroByteReadIsDisconnection { get; } = true;
        public int ExpectedSize { get; } = 1;
        public int NumberOfReceivesPerPoll { get; } = 1;
        public unsafe int Process(DispatchContext dispatchContext)
        {
            var read = dispatchContext.EncodedBuffer.ReadCursor;
            var originalRead = read;
            ushort messageId;
            while (read < dispatchContext.EncodedBuffer.WrittenCursor)
            {
                fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                {
                    byte* ptr = fptr + read;
                    dispatchContext.MessageVersion = *(ptr++);
                    messageId = StreamByteOps.ToUShort(ref ptr);
                    dispatchContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
                }
                read += 5;
                IBinaryDeserializer u;
                if (deserializers.TryGetValue(messageId, out u))
                {
                    dispatchContext.EncodedBuffer.ReadCursor = read;
                    u.Deserialize(dispatchContext);
                }
                read += dispatchContext.MessageSize;
            }
            dispatchContext.DispatchLatencyLogger.Dedent();
            int amountRead = read - originalRead;
            dispatchContext.EncodedBuffer.ReadCursor = read;
            return amountRead;
        }

        public class SimpleDeserializerFactory : IStreamDecoderFactory
        {
            private readonly IDictionary<uint, IBinaryDeserializer> deserializers;

            public SimpleDeserializerFactory(IDictionary<uint, IBinaryDeserializer> deserializers)
            {
                this.deserializers = deserializers;
            }

            public IStreamDecoder Supply()
            {
                var streamDecoder = new SimpleStreamDecoder();
                foreach (var binaryDeserializerEntry in deserializers)
                {
                    streamDecoder.AddMessageDecoder(binaryDeserializerEntry.Key, binaryDeserializerEntry.Value);
                }
                return streamDecoder;
            }
        }
    }
}
