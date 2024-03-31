#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

internal sealed class PQServerMessageStreamDecoder : IMessageStreamDecoder
{
    private const int HeaderSize = 2 * sizeof(byte) + 2 * sizeof(ushort);
    private const int RequestSize = sizeof(uint);
    private readonly Action<IConversationRequester, uint[]>? requestsHandlerNew;
    private MessageSection messageSection;

    private ushort requestsCount;


    public PQServerMessageStreamDecoder(Action<IConversationRequester, uint[]> requestsHandler)
    {
        messageSection = MessageSection.Header;
        ExpectedSize = HeaderSize;
        requestsHandlerNew = requestsHandler;
    }

    public IEnumerable<KeyValuePair<uint, IMessageDeserializer>> RegisteredDeserializers => [];

    public int NumberOfReceivesPerPoll => 1;
    public int ExpectedSize { get; private set; }

    public bool ZeroByteReadIsDisconnection => false;

    public bool AddMessageDeserializer(uint msgId, IMessageDeserializer deserializer) =>
        throw new NotImplementedException("No deserializers required for this stream");

    public unsafe int Process(SocketBufferReadContext socketBufferReadContext)
    {
        var read = socketBufferReadContext.EncodedBuffer!.ReadCursor;
        var originalRead = socketBufferReadContext.EncodedBuffer.ReadCursor;
        byte flags = 0;
        while (ExpectedSize <= socketBufferReadContext.EncodedBuffer.WriteCursor - read)
            switch (messageSection)
            {
                case MessageSection.Header:
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        socketBufferReadContext.MessageVersion = *ptr++;
                        flags = *ptr++;
                        socketBufferReadContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
                        requestsCount = StreamByteOps.ToUShort(ref ptr);
                    }

                    read += HeaderSize;
                    if (requestsCount > 0)
                    {
                        messageSection = MessageSection.Data;
                        ExpectedSize = requestsCount * RequestSize;
                    }
                    else
                    {
                        messageSection = MessageSection.Header;
                        ExpectedSize = HeaderSize;
                    }

                    break;
                case MessageSection.Data:
                    var streamIDs = new uint[requestsCount];
                    fixed (byte* fptr = socketBufferReadContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        for (var i = 0; i < streamIDs.Length; i++) streamIDs[i] = StreamByteOps.ToUInt(ref ptr);
                    }

                    socketBufferReadContext.EncodedBuffer.ReadCursor = read;
                    requestsHandlerNew?.Invoke((IConversationRequester)socketBufferReadContext.Conversation!, streamIDs);
                    read += requestsCount * RequestSize;
                    messageSection = MessageSection.Header;
                    ExpectedSize = HeaderSize;
                    break;
            }

        socketBufferReadContext.EncodedBuffer.ReadCursor = read;
        return read - originalRead;
    }

    private enum MessageSection
    {
        Header
        , Data
    }
}
