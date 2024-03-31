#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders;

[TestClass]
public class OrxOrderIdTests
{
    private const int BufferSize = 2048;
    private byte[] byteBuffer = null!;
    private SocketBufferReadContext socketBufferReadContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        byteBuffer = new byte[BufferSize];

        socketBufferReadContext = new SocketBufferReadContext
        {
            EncodedBuffer = new ReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
            , MessageVersion = TradingVersionInfo.CurrentVersion
        };
    }

    [TestMethod]
    public void NewOrxClient_Serialize_DeserializesProperly()
    {
        var orxOrxOrderIdSerializer = new OrxByteSerializer<OrxOrderId>();
        var originalOrderId = new OrxOrderId(123, "Testing 123", 234, "Test234", new OrxOrderId(345, "Testing 345")
            , "TrackingId1234");

        socketBufferReadContext.MessageSize = orxOrxOrderIdSerializer.Serialize(originalOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orxOrderIdDeserializer = new OrxByteDeserializer<OrxOrderId>(
            new OrxDeserializerLookup(new Recycler()));

        var deserializedOrxClientOrderId = orxOrderIdDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalOrderId.ClientOrderId,
            deserializedOrxClientOrderId.ClientOrderId);
        Assert.AreEqual(originalOrderId.TrackingId,
            deserializedOrxClientOrderId.TrackingId);
        Assert.AreEqual(originalOrderId.ParentOrderId,
            deserializedOrxClientOrderId.ParentOrderId);
        Assert.AreEqual(originalOrderId.AdapterOrderId,
            deserializedOrxClientOrderId.AdapterOrderId);
    }
}
