using System;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.SessionConnection;

namespace FortitudeMarketsCore.Pricing.PQ.Publication 
{
    internal sealed class PQServerDecoder : IStreamDecoder
    {
        private enum MessageSection
        {
            Header, Data
        }

        private const int HeaderSize = 2 * sizeof(byte) +  2 * sizeof(ushort);
        private const int RequestSize = sizeof(uint);
        private MessageSection messageSection;
        private int toRead;

        private ushort requestsCount;
        private readonly Action<ISocketSessionConnection, uint[]> requestsHandler;

        public PQServerDecoder(Action<ISocketSessionConnection, uint[]> requestsHandler)
        {
            messageSection = MessageSection.Header;
            toRead = HeaderSize;
            this.requestsHandler = requestsHandler;
        }

        public bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer)
        {
            throw new NotImplementedException("No deserializers required for this stream");
        }

        public int NumberOfReceivesPerPoll => 1;
        public int ExpectedSize => toRead;

        public bool ZeroByteReadIsDisconnection => false;

        public unsafe int Process(DispatchContext dispatchContext)
        {
            int read = dispatchContext.EncodedBuffer.ReadCursor;
            int originalRead = dispatchContext.EncodedBuffer.ReadCursor;
            byte flags = 0;
            while (toRead <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            {
                switch (messageSection)
                {
                    case MessageSection.Header:
                        fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                        {
                            byte* ptr = fptr + read;
                            dispatchContext.MessageVersion = *(ptr++);
                            flags = *(ptr++);
                            dispatchContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
                            requestsCount = StreamByteOps.ToUShort(ref ptr);
                        }
                        read += HeaderSize;
                        if (requestsCount > 0)
                        {
                            messageSection = MessageSection.Data;
                            toRead = requestsCount * RequestSize;
                        }
                        else
                        {
                            messageSection = MessageSection.Header;
                            toRead = HeaderSize;
                        }
                        break;
                    case MessageSection.Data:
                        uint[] streamIDs = new uint[requestsCount];
                        fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                        {
                            byte* ptr = fptr + read;
                            for (int i = 0; i < streamIDs.Length; i++)
                            {
                                streamIDs[i] = StreamByteOps.ToUInt(ref ptr);
                            }
                        }
                        dispatchContext.EncodedBuffer.ReadCursor = read;
                        requestsHandler(dispatchContext.Session, streamIDs);
                        read += requestsCount * RequestSize;
                        messageSection = MessageSection.Header;
                        toRead = HeaderSize;
                        break;
                }
            }
            dispatchContext.EncodedBuffer.ReadCursor = read;
            return read - originalRead;
        }
    }
}
