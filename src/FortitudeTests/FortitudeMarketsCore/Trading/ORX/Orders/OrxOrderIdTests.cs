#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders;

[TestClass]
public class OrxOrderIdTests
{
    private const int BufferSize = 2048;
    private byte[] byteBuffer = null!;
    private DispatchContext dispatchContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        byteBuffer = new byte[BufferSize];

        dispatchContext = new DispatchContext
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

        dispatchContext.MessageSize = orxOrxOrderIdSerializer.Serialize(originalOrderId,
            byteBuffer, 0, 0);

        var orxOrderIdDeserializer = new OrxByteDeserializer<OrxOrderId>(
            new OrxDeserializerLookup(new Recycler()));

        var deserializedOrxClientOrderId = (OrxOrderId)orxOrderIdDeserializer
            .Deserialize(dispatchContext);

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
