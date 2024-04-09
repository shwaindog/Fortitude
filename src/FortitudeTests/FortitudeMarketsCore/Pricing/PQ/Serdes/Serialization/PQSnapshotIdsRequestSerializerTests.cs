#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQSnapshotIdsRequestSerializerTests
{
    private const int BufferReadWriteOffset = 100;
    private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer = null!;
    private ReadWriteBuffer readWriteBuffer = null!;

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
        readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

        fixed (byte* bufferPtr = readWriteBuffer.Buffer)
        {
            var startWritten = bufferPtr + BufferReadWriteOffset;
            var currPtr = bufferPtr + BufferReadWriteOffset;
            var protocolVersion = *currPtr++;
            Assert.AreEqual(snapshotIdsMessage.Version, protocolVersion);
            var messageFlags = *currPtr++;
            Assert.AreEqual((byte)0, messageFlags);
            var messageId = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual(snapshotIdsMessage.MessageId, messageId);
            var messagesTotalSize = StreamByteOps.ToUInt(ref currPtr);
            Assert.AreEqual((uint)amtWritten, messagesTotalSize);
            var numberOfIds = StreamByteOps.ToUShort(ref currPtr);
            Assert.AreEqual(expectedIdsToReceive.Length, numberOfIds);
            foreach (var expectedUint in expectedIdsToReceive)
                Assert.AreEqual(expectedUint, StreamByteOps.ToUInt(ref currPtr));
            Assert.AreEqual(messagesTotalSize, (uint)(currPtr - startWritten));
        }
    }
}
