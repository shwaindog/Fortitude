using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization
{
    public sealed class OrxDecoder : IStreamDecoder
    {
        private enum State
        {
            Header, Data
        }

        public const int HeaderSize = 2 * OrxConstants.UInt16Sz + OrxConstants.UInt8Sz;

        private State state = State.Header;
        private int toRead = HeaderSize;

        public int ExpectedSize => toRead;

        private readonly IMap<uint, IBinaryDeserializer> deserializers;

        public bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer)
        {
            deserializers.Add(msgId, deserializer);
            return deserializers[msgId] == deserializer;
        }

        public OrxDecoder(IMap<uint, IBinaryDeserializer> deserializers)
        {
            this.deserializers = deserializers;
        }

        public int NumberOfReceivesPerPoll => 1;

        public bool ZeroByteReadIsDisconnection => true;

        private ushort messageId;

        public unsafe int Process(DispatchContext dispatchContext)
        {
            var read = dispatchContext.EncodedBuffer.ReadCursor;
            var originalRead = dispatchContext.EncodedBuffer.ReadCursor;
            while (toRead <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            {
                if (state == State.Header)
                {
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        byte* ptr = fptr + read;
                        dispatchContext.MessageVersion = *(ptr++);
                        messageId = StreamByteOps.ToUShort(ref ptr);
                        toRead = StreamByteOps.ToUShort(ref ptr);
                        dispatchContext.MessageSize = toRead;
                    }
                    state = State.Data;
                    read += HeaderSize;
                }
                else
                {
                    IBinaryDeserializer u;
                    if (deserializers.TryGetValue(messageId, out u))
                    {
                        dispatchContext.EncodedBuffer.ReadCursor = read;
                        u.Deserialize(dispatchContext);
                    }
                    read += toRead;
                    state = State.Header;
                    toRead = HeaderSize;
                }
            }
            dispatchContext.DispatchLatencyLogger.Dedent();
            int amountRead = read - originalRead;
            dispatchContext.EncodedBuffer.ReadCursor = read;
            return amountRead;
        }
    }
}
