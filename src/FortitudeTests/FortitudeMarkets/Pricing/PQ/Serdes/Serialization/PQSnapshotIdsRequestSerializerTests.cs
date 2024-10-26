﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQSnapshotIdsRequestSerializerTests
{
    private const int BufferReadWriteOffset = 100;
    private PQSnapshotIdsRequestSerializer pqSnapshotIdsRequestSerializer = null!;
    private CircularReadWriteBuffer readWriteBuffer = null!;

    [TestInitialize]
    public void SetUp()
    {
        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]) { ReadCursor = BufferReadWriteOffset };

        pqSnapshotIdsRequestSerializer = new PQSnapshotIdsRequestSerializer();
    }

    [TestMethod]
    public unsafe void ListOfSnapshotIds_Serialize_WritesExpectBytesToBuffer()
    {
        uint[] expectedIdsToReceive = { 0, 41, 64, 30, 17, 2 };

        var snapshotIdsMessage = new PQSnapshotIdsRequest(expectedIdsToReceive);

        readWriteBuffer.WriteCursor = BufferReadWriteOffset;
        var amtWritten = pqSnapshotIdsRequestSerializer
            .Serialize(readWriteBuffer, snapshotIdsMessage);
        readWriteBuffer.WriteCursor += amtWritten;
        using var fixedBuffer = readWriteBuffer;
        var startWritten = fixedBuffer.ReadBuffer + BufferReadWriteOffset;
        var currPtr = startWritten;
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
