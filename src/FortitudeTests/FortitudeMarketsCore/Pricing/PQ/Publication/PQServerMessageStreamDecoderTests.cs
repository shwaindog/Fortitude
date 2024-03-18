#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
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

    private Action<ISocketSessionContext, uint[]> deserializerCallBack = null!;
    private ISocketSessionContext lastReceivedSessionConnection = null!;

    private uint[] lastReceivedSnapshotRequestIds = null!;
    private PQServerMessageStreamDecoder pqServerMessageStreamDecoder = null!;
    private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer = null!;
    private ReadSocketBufferContext readSocketBufferContext = null!;
    private ReadWriteBuffer readWriteBuffer = null!;

    private Mock<ISocketSessionContext> socketSessionConnection = null!;

    [TestInitialize]
    public void SetUp()
    {
        socketSessionConnection = new Mock<ISocketSessionContext>();
        readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
        readSocketBufferContext = new ReadSocketBufferContext
        {
            EncodedBuffer = readWriteBuffer, Conversation = socketSessionConnection.Object
        };
        readWriteBuffer.ReadCursor = BufferReadWriteOffset;

        deserializerCallBack = (ssc, rsi) =>
        {
            lastReceivedSessionConnection = ssc;
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
        readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

        pqServerMessageStreamDecoder.Process(readSocketBufferContext);

        Assert.IsTrue(expectedIdsToReceive.SequenceEqual(lastReceivedSnapshotRequestIds));
        Assert.AreSame(socketSessionConnection.Object, lastReceivedSessionConnection);
        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);

        uint[] nextExpectedIdsToReceive = { 0, 77, 71, 51, 97, 98 };
        snapshotIdsRequest = new PQSnapshotIdsRequest(nextExpectedIdsToReceive);
        amtWritten = pqSnapshotIdsRequestSerializer.Serialize(readWriteBuffer.Buffer,
            readWriteBuffer.WrittenCursor, snapshotIdsRequest);
        readWriteBuffer.WrittenCursor = readWriteBuffer.WrittenCursor + amtWritten;

        pqServerMessageStreamDecoder.Process(readSocketBufferContext);

        Assert.IsTrue(lastReceivedSnapshotRequestIds.SequenceEqual(nextExpectedIdsToReceive));
        Assert.AreSame(socketSessionConnection.Object, lastReceivedSessionConnection);
        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
    }
}
