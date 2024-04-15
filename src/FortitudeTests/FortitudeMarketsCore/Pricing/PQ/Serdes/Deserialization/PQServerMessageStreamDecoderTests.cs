#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQServerMessageStreamDecoderTests
{
    private const int BufferReadWriteOffset = 5;
    private IConversation? lastReceivedConversation;

    private List<uint> lastReceivedSnapshotRequestIds = null!;
    private PQSourceTickerInfoRequest? lastReceivedSourceTickerInfoRequest = null;
    private Mock<IConversation> moqConversation = null!;
    private Mock<IMessageDeserializationRepository> moqDeserializationRepo = null!;
    private Mock<ISocketConversation> moqSocketConversation = null!;
    private Mock<IConversation> moqSocketConversationRequester = null!;

    private Mock<ISocketSessionContext> moqSocketSessionConnection = null!;
    private PQServerMessageStreamDecoder pqServerMessageStreamDecoder = null!;
    private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer = null!;
    private PQSourceTickerInfoRequestSerializer pqSourceTickerInfoRequestSerializer = null!;
    private ReadWriteBuffer readWriteBuffer = null!;

    private ConversationMessageReceivedHandler<PQSnapshotIdsRequest> snapshotIdsResponseCallBack = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;
    private ConversationMessageReceivedHandler<PQSourceTickerInfoRequest> sourceTickerInfoResponseCallBack = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqSocketSessionConnection = new Mock<ISocketSessionContext>();
        moqSocketConversationRequester = new Mock<IConversation>();
        moqConversation = new Mock<IConversation>();
        moqDeserializationRepo = new Mock<IMessageDeserializationRepository>();
        moqSocketConversation = moqSocketConversationRequester.As<ISocketConversation>();
        moqSocketSessionConnection.SetupGet(x => x.OwningConversation).Returns(moqSocketConversation.Object);

        readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
        socketBufferReadContext = new SocketBufferReadContext
        {
            EncodedBuffer = readWriteBuffer, Conversation = moqSocketConversation.Object
        };
        readWriteBuffer.ReadCursor = BufferReadWriteOffset;
        readWriteBuffer.WriteCursor = BufferReadWriteOffset;

        snapshotIdsResponseCallBack = (snapshotIdsReq, header, conversation) =>
        {
            lastReceivedConversation = conversation;
            lastReceivedSnapshotRequestIds = snapshotIdsReq.RequestSourceTickerIds;
        };
        sourceTickerInfoResponseCallBack = (srcTkrInfoRequest, header, conversation) =>
        {
            lastReceivedConversation = conversation;
            lastReceivedSourceTickerInfoRequest = srcTkrInfoRequest;
        };

        pqSnapshotIdsRequestSerializer = new PQSnapshotIdsRequestSerializer();
        pqSourceTickerInfoRequestSerializer = new PQSourceTickerInfoRequestSerializer();

        pqServerMessageStreamDecoder = new PQServerMessageStreamDecoder(new PQServerDeserializationRepository(new Recycler()));
        pqServerMessageStreamDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSnapshotIdsRequest>()
            .AddDeserializedNotifier(new PassThroughDeserializedNotifier<PQSnapshotIdsRequest>(
                $"{nameof(PQServerMessageStreamDecoderTests)}.{nameof(snapshotIdsResponseCallBack)}", snapshotIdsResponseCallBack));
        pqServerMessageStreamDecoder.MessageDeserializationRepository.RegisterDeserializer<PQSourceTickerInfoRequest>()
            .AddDeserializedNotifier(new PassThroughDeserializedNotifier<PQSourceTickerInfoRequest>(
                $"{nameof(PQServerMessageStreamDecoderTests)}.{nameof(sourceTickerInfoResponseCallBack)}", sourceTickerInfoResponseCallBack));
    }

    [TestMethod]
    public void NewServerDecoder_New_PropertiesInitializedAsExpected()
    {
        Assert.AreEqual(10U, pqServerMessageStreamDecoder.ExpectedSize);
    }

    [TestMethod]
    public void TwoSnapshotRequests_ProcessTwice_DecodesStreamAndCompletes()
    {
        uint[] expectedIdsToReceive = { 0, 77, 95, 23, 11, 51 };
        var snapshotIdsRequest = new PQSnapshotIdsRequest(expectedIdsToReceive);
        pqSnapshotIdsRequestSerializer.Serialize(snapshotIdsRequest, (ISerdeContext)socketBufferReadContext);
        var amtWritten = socketBufferReadContext.LastWriteLength;

        pqServerMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsTrue(expectedIdsToReceive.SequenceEqual(lastReceivedSnapshotRequestIds));
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);

        uint[] nextExpectedIdsToReceive = { 0, 77, 71, 51, 97, 98 };
        snapshotIdsRequest = new PQSnapshotIdsRequest(nextExpectedIdsToReceive);
        pqSnapshotIdsRequestSerializer.Serialize(snapshotIdsRequest, (ISerdeContext)socketBufferReadContext);
        amtWritten = socketBufferReadContext.LastWriteLength;
        lastReceivedConversation = null;

        pqServerMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsTrue(lastReceivedSnapshotRequestIds.SequenceEqual(nextExpectedIdsToReceive));
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
    }

    [TestMethod]
    public void TwoSourceTickerInfoRequests_ProcessTwice_DecodesStreamAndCompletes()
    {
        lastReceivedSourceTickerInfoRequest = null;
        var sourceTickerInfoRequest = new PQSourceTickerInfoRequest();
        pqSourceTickerInfoRequestSerializer.Serialize(sourceTickerInfoRequest, (ISerdeContext)socketBufferReadContext);
        var amtWritten = socketBufferReadContext.LastWriteLength;

        pqServerMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsNotNull(lastReceivedSourceTickerInfoRequest);
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);

        pqSourceTickerInfoRequestSerializer.Serialize(sourceTickerInfoRequest, (ISerdeContext)socketBufferReadContext);
        amtWritten = socketBufferReadContext.LastWriteLength;
        lastReceivedConversation = null;
        lastReceivedSourceTickerInfoRequest = null;

        pqServerMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsNotNull(lastReceivedSourceTickerInfoRequest);
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
    }
}
