#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

internal sealed class PQServerDecoder : IStreamDecoder
{
    private const int HeaderSize = 2 * sizeof(byte) + 2 * sizeof(ushort);
    private const int RequestSize = sizeof(uint);
    private readonly Action<ISocketSessionConnection, uint[]> requestsHandler;
    private MessageSection messageSection;

    private ushort requestsCount;

    public PQServerDecoder(Action<ISocketSessionConnection, uint[]> requestsHandler)
    {
        messageSection = MessageSection.Header;
        ExpectedSize = HeaderSize;
        this.requestsHandler = requestsHandler;
    }

    public int NumberOfReceivesPerPoll => 1;
    public int ExpectedSize { get; private set; }

    public bool ZeroByteReadIsDisconnection => false;

    public bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer) =>
        throw new NotImplementedException("No deserializers required for this stream");

    public unsafe int Process(DispatchContext dispatchContext)
    {
        var read = dispatchContext.EncodedBuffer!.ReadCursor;
        var originalRead = dispatchContext.EncodedBuffer.ReadCursor;
        byte flags = 0;
        while (ExpectedSize <= dispatchContext.EncodedBuffer.WrittenCursor - read)
            switch (messageSection)
            {
                case MessageSection.Header:
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        dispatchContext.MessageVersion = *ptr++;
                        flags = *ptr++;
                        dispatchContext.MessageSize = StreamByteOps.ToUShort(ref ptr);
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
                    fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
                    {
                        var ptr = fptr + read;
                        for (var i = 0; i < streamIDs.Length; i++) streamIDs[i] = StreamByteOps.ToUInt(ref ptr);
                    }

                    dispatchContext.EncodedBuffer.ReadCursor = read;
                    requestsHandler(dispatchContext.Session!, streamIDs);
                    read += requestsCount * RequestSize;
                    messageSection = MessageSection.Header;
                    ExpectedSize = HeaderSize;
                    break;
            }

        dispatchContext.EncodedBuffer.ReadCursor = read;
        return read - originalRead;
    }

    private enum MessageSection
    {
        Header
        , Data
    }
}
