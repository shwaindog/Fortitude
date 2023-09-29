using System;
using System.Linq;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication
{
    [TestClass]
    public class PQServerDecoderTests
    {
        private ReadWriteBuffer readWriteBuffer;
        private DispatchContext dispatchContext;
        private PQServerDecoder pqServerDecoder;
        private const int BufferReadWriteOffset = 5;
        private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer;

        private Mock<ISocketSessionConnection> socketSessionConnection;

        private uint[] lastReceivedSnapshotRequestIds;
        private ISocketSessionConnection lastReceivedSessionConnection;

        private Action<ISocketSessionConnection, uint[]> deserializerCallBack;

        [TestInitialize]
        public void SetUp()
        {
            socketSessionConnection = new Mock<ISocketSessionConnection>();
            readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
            dispatchContext = new DispatchContext
            {
                EncodedBuffer = readWriteBuffer,
                Session = socketSessionConnection.Object
            };
            readWriteBuffer.ReadCursor = BufferReadWriteOffset;

            deserializerCallBack = (ssc, rsi) =>
            {
                lastReceivedSessionConnection = ssc;
                lastReceivedSnapshotRequestIds = rsi;
            };

            pqSnapshotIdsRequestSerializer = new PQSnapshotIdsRequestSerializer();

            pqServerDecoder = new PQServerDecoder(deserializerCallBack);
        }

        [TestMethod]
        public void NewServerDecoder_New_PropertiesInitializedAsExpected()
        {
            Assert.AreEqual(6, pqServerDecoder.ExpectedSize);
            Assert.AreEqual(1, pqServerDecoder.NumberOfReceivesPerPoll);
        }

        [TestMethod]
        public void TwoSnapshotRequests_ProcessTwice_DecodesStreamAndCompletes()
        {
            uint[] expectedIdsToReceive = {0, 77, 95, 23, 11, 51};
            var snapshotIdsRequest = new PQSnapshotIdsRequest(expectedIdsToReceive);
            var amtWritten = pqSnapshotIdsRequestSerializer.Serialize(readWriteBuffer.Buffer, 
                BufferReadWriteOffset, snapshotIdsRequest);
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

            pqServerDecoder.Process(dispatchContext);

            Assert.IsTrue(expectedIdsToReceive.SequenceEqual(lastReceivedSnapshotRequestIds));
            Assert.AreSame(socketSessionConnection.Object, lastReceivedSessionConnection);
            Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);

            uint[] nextExpectedIdsToReceive = { 0, 77, 71, 51, 97, 98 };
            snapshotIdsRequest = new PQSnapshotIdsRequest(nextExpectedIdsToReceive);
            amtWritten = pqSnapshotIdsRequestSerializer.Serialize(readWriteBuffer.Buffer,
                readWriteBuffer.WrittenCursor, snapshotIdsRequest);
            readWriteBuffer.WrittenCursor = readWriteBuffer.WrittenCursor + amtWritten;

            pqServerDecoder.Process(dispatchContext);

            Assert.IsTrue(lastReceivedSnapshotRequestIds.SequenceEqual(nextExpectedIdsToReceive));
            Assert.AreSame(socketSessionConnection.Object, lastReceivedSessionConnection);
            Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        }
    }
}