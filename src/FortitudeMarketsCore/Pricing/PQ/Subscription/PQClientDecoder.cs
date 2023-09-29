using System;
using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    internal sealed class PQClientDecoder : IStreamDecoder
    {
        private enum MessageSection
        {
            TransmissionHeader, HeartBeats, MessageData
        }
        
        private const int TransmissionHeaderSize = sizeof(byte)*2 + sizeof(uint);
        private const int TickerSourceIdSequenceNumberBodySize = 2 * sizeof(uint);

        public int ExpectedSize => toRead;

        public bool ZeroByteReadIsDisconnection => true;

        private readonly IMap<uint, IBinaryDeserializer> deserializers;
        private MessageSection messageSection;
        private int toRead;

        public PQClientDecoder(IMap<uint, IBinaryDeserializer> deserializers, PQFeedType feed)
        {
            this.deserializers = deserializers;
            messageSection = MessageSection.TransmissionHeader;
            toRead = TransmissionHeaderSize;

            msgHeader = new PQQuoteTransmissionHeader(feed);
        }

        public bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer)
        {
            throw new NotImplementedException("No deserializers required for this stream");
        }

        private uint messagesTotalSize;
        private PQBinaryMessageFlags messageFlags;
        private uint streamId;

        private readonly PQQuoteTransmissionHeader msgHeader;

        private static readonly IFLogger Logger = 
            FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public int NumberOfReceivesPerPoll => 50;

        public unsafe int Process(DispatchContext dispatchContext)
        {
            var read = dispatchContext.EncodedBuffer.ReadCursor;
            var originalRead = dispatchContext.EncodedBuffer.ReadCursor;
            dispatchContext.MessageHeader = msgHeader;
            while (toRead <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            {
                switch (messageSection)
                {
                    case MessageSection.TransmissionHeader:
                        fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                        {
                            byte* ptr = fptr + read;
                            dispatchContext.MessageVersion = *ptr++;
                            messageFlags = (PQBinaryMessageFlags)(*ptr++);
                            messagesTotalSize = StreamByteOps.ToUInt(ref ptr);
                            read += toRead;
                            messageSection = (messageFlags & PQBinaryMessageFlags.IsHeartBeat) 
                                == PQBinaryMessageFlags.IsHeartBeat 
                                    ? MessageSection.HeartBeats 
                                    : MessageSection.MessageData;
                            if (messagesTotalSize > 0)
                                toRead = (int)messagesTotalSize - TransmissionHeaderSize;
                        }
                        break;
                    case MessageSection.HeartBeats:
                        var noBeats = toRead / TickerSourceIdSequenceNumberBodySize;
                        fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                        {
                            byte* ptr = fptr + read;
                            for (int i = 0; i < noBeats; i++)
                            {
                                streamId = StreamByteOps.ToUInt(ref ptr);
                                msgHeader.SequenceId = StreamByteOps.ToUInt(ref ptr);
                                read += TickerSourceIdSequenceNumberBodySize;
                                dispatchContext.EncodedBuffer.ReadCursor = read;
                                IBinaryDeserializer bu;
                                dispatchContext.MessageSize = 0;
                                if (deserializers.TryGetValue(streamId, out bu))
                                {
                                    dispatchContext.EncodedBuffer.ReadCursor = read;
                                    bu.Deserialize(dispatchContext);
                                    OnResponse?.Invoke();
                                }
                            }
                        }
                        toRead = TransmissionHeaderSize;
                        messageSection = MessageSection.TransmissionHeader;
                        break;
                    case MessageSection.MessageData:
                        fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                        {
                            byte* ptr = fptr + read;
                            streamId = StreamByteOps.ToUInt(ref ptr);
                            msgHeader.SequenceId = StreamByteOps.ToUInt(ref ptr);

                        }
                        read += TickerSourceIdSequenceNumberBodySize;
                        toRead -= TickerSourceIdSequenceNumberBodySize;
                        dispatchContext.MessageSize = toRead;
                        IBinaryDeserializer u;
                        if (deserializers.TryGetValue(streamId, out u))
                        {
                            dispatchContext.EncodedBuffer.ReadCursor = read;
                            u.Deserialize(dispatchContext);
                            OnResponse?.Invoke();
                        }
                        read += toRead;
                        toRead = TransmissionHeaderSize;
                        messageSection = MessageSection.TransmissionHeader;
                        break;
                }
            }
            if (toRead > 1048576/2)
            {
                Logger.Error($"The value to read from the socket {toRead:N0}B is larger than any PQ message is " +
                             "expected to be.  Resetting socket read location as it is probably corrupt.");
                read = dispatchContext.EncodedBuffer.WrittenCursor;
                messageSection = MessageSection.TransmissionHeader;
                toRead = TransmissionHeaderSize;
            }
            int amountRead = read - originalRead;
            dispatchContext.EncodedBuffer.ReadCursor = read;
            return amountRead;
        }

        public event Action OnResponse;
    }
}
