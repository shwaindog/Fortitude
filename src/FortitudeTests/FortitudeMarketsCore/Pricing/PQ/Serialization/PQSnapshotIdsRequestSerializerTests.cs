using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization
{
    [TestClass]
    public class PQSnapshotIdsRequestSerializerTests
    {
        private ReadWriteBuffer readWriteBuffer;
        private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer;
        private const int BufferReadWriteOffset = 100;

        [TestInitialize]
        public void SetUp()
        {
            readWriteBuffer = new ReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };

            pqSnapshotIdsRequestSerializer = new PQSnapshotIdsRequestSerializer();
        }

        [TestMethod]
        public unsafe void ListOfSnapshotIds_Serialize_WritesExpectBytesToBuffer()
        {
            uint[] expectedIdsToReceive = { 0, 41, 64, 30, 17, 2 };

            var snapshotIdsMessage = new PQSnapshotIdsRequest(expectedIdsToReceive);

            var amtWritten = pqSnapshotIdsRequestSerializer
                .Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset, snapshotIdsMessage);
            readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

            fixed (byte* bufferPtr = readWriteBuffer.Buffer)
            {
                byte* startWritten = bufferPtr + BufferReadWriteOffset;
                byte* currPtr = bufferPtr + BufferReadWriteOffset;
                byte protocolVersion = *currPtr++;
                Assert.AreEqual(snapshotIdsMessage.Version, protocolVersion);
                byte messageFlags = *currPtr++;
                Assert.AreEqual((byte)0, messageFlags);
                var messagesTotalSize = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual((uint)amtWritten, messagesTotalSize);
                var numberOfIds = StreamByteOps.ToUShort(ref currPtr);
                Assert.AreEqual(expectedIdsToReceive.Length, numberOfIds);
                foreach (var expectedUint in expectedIdsToReceive)
                {
                    Assert.AreEqual(expectedUint, StreamByteOps.ToUInt(ref currPtr));
                }
                Assert.AreEqual(messagesTotalSize, (uint)(currPtr - startWritten));
            }
        }
    }
}