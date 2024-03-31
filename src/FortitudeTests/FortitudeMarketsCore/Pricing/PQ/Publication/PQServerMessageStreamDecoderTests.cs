#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQServerMessageStreamDecoderTests
{
    private const int BufferReadWriteOffset = 5;

    private Action<IConversationRequester, uint[]> deserializerCallBack = null!;
    private IConversationRequester? lastReceivedConversation;

    private uint[] lastReceivedSnapshotRequestIds = null!;
    private Mock<IConversation> moqConversation = null!;
    private Mock<ISocketConversation> moqSocketConversation = null!;
    private Mock<IConversationRequester> moqSocketConversationRequester = null!;

    private Mock<ISocketSessionContext> moqSocketSessionConnection = null!;
    private PQServerMessageStreamDecoder pqServerMessageStreamDecoder = null!;
    private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer = null!;
    private ReadSocketBufferContext readSocketBufferContext = null!;
    private ReadWriteBuffer readWriteBuffer = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqSocketSessionConnection = new Mock<ISocketSessionContext>();
        moqSocketConversationRequester = new Mock<IConversationRequester>();
        moqConversation = new Mock<IConversation>();
        moqSocketConversation = moqSocketConversationRequester.As<ISocketConversation>();
        moqSocketSessionConnection.SetupGet(x => x.OwningConversation).Returns(moqSocketConversation.Object);
        readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
        readSocketBufferContext = new ReadSocketBufferContext
        {
            EncodedBuffer = readWriteBuffer, Conversation = moqSocketConversation.Object
        };
        readWriteBuffer.ReadCursor = BufferReadWriteOffset;

        deserializerCallBack = (ssc, rsi) =>
        {
            lastReceivedConversation = ssc;
            lastReceivedSnapshotRequestIds = rsi;
        };

        pqSnapshotIdsRequestSerializer = new PQSnapshotIdsRequestSerializer();

        pqServerMessageStreamDecoder = new PQServerMessageStreamDecoder(deserializerCallBack);
    }

    [TestMethod]
    public void NewServerDecoder_New_PropertiesInitializedAsExpected()
    {
        Assert.AreEqual(6, pqServerMessageStreamDecoder.ExpectedSize);
        Assert.AreEqual(1, pqServerMessageStreamDecoder.NumberOfReceivesPerPoll);
    }

    [TestMethod]
    public void TwoSnapshotRequests_ProcessTwice_DecodesStreamAndCompletes()
    {
        uint[] expectedIdsToReceive = { 0, 77, 95, 23, 11, 51 };
        var snapshotIdsRequest = new PQSnapshotIdsRequest(expectedIdsToReceive);
        var amtWritten = pqSnapshotIdsRequestSerializer.Serialize(readWriteBuffer.Buffer,
            BufferReadWriteOffset, snapshotIdsRequest);
        readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

        pqServerMessageStreamDecoder.Process(readSocketBufferContext);

        Assert.IsTrue(expectedIdsToReceive.SequenceEqual(lastReceivedSnapshotRequestIds));
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);

        uint[] nextExpectedIdsToReceive = { 0, 77, 71, 51, 97, 98 };
        snapshotIdsRequest = new PQSnapshotIdsRequest(nextExpectedIdsToReceive);
        amtWritten = pqSnapshotIdsRequestSerializer.Serialize(readWriteBuffer.Buffer,
            readWriteBuffer.WriteCursor, snapshotIdsRequest);
        readWriteBuffer.WriteCursor = readWriteBuffer.WriteCursor + amtWritten;
        lastReceivedConversation = null;

        pqServerMessageStreamDecoder.Process(readSocketBufferContext);

        Assert.IsTrue(lastReceivedSnapshotRequestIds.SequenceEqual(nextExpectedIdsToReceive));
        Assert.AreSame(moqSocketConversationRequester.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
    }
}
