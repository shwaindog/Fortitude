#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.Orders.Products.General;

[TestClass]
public class OrxSpotOrderTests
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
            EncodedBuffer = new CircularReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
            , MessageHeader = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, 1)
        };
    }

    [TestMethod]
    public void NewSpotOrders_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<SpotOrders>();
        var originalClientOrderId = new SpotOrders
        {
            FirstSpotOrder = BuildSpotOrder(), SecondSpotOrder = BuildSpotOrder(), ThirdSpotOrder = BuildSpotOrder()
            , FourthSpotOrder = BuildSpotOrder()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer!, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer!.ReadCursor = MessageHeader.SerializationSize;

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<SpotOrders>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstSpotOrder, deserializedOrxClientOrderId.FirstSpotOrder);
        Assert.AreEqual(originalClientOrderId.SecondSpotOrder, deserializedOrxClientOrderId.SecondSpotOrder);
        Assert.AreEqual(originalClientOrderId.ThirdSpotOrder, deserializedOrxClientOrderId.ThirdSpotOrder);
        Assert.AreEqual(originalClientOrderId.FourthSpotOrder, deserializedOrxClientOrderId.FourthSpotOrder);
    }

    private OrxSpotOrder BuildSpotOrder() =>
        new(new OrxOrderId(2), 1234, 3456u,OrderSide.Bid, 1.23456m, 300_000L,
            OrderType.PassiveLimit, displaySize: 100_000,  allowedPriceSlippage:0.00025m, allowedVolumeSlippage: 10_000m, tickerName:  "TestTicker");

    public class SpotOrders
    {
        [OrxMandatoryField(0)] public OrxSpotOrder? FirstSpotOrder { get; set; }

        [OrxMandatoryField(1)] public OrxSpotOrder? SecondSpotOrder { get; set; }

        [OrxOptionalField(1)] public OrxSpotOrder? ThirdSpotOrder { get; set; }

        [OrxOptionalField(2)] public OrxSpotOrder? FourthSpotOrder { get; set; }
    }
}
