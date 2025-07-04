#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.Orders.Client;

[TestClass]
public class OrxOrderSubmitRequestTests
{
    private const int    BufferSize = 2048;
    private       byte[] byteBuffer = null!;

    private SocketBufferReadContext socketBufferReadContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        byteBuffer = new byte[BufferSize];

        socketBufferReadContext = new SocketBufferReadContext
        {
            EncodedBuffer         = new CircularReadWriteBuffer(byteBuffer)
          , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
          , MessageHeader         = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, 1)
        };
    }

    [TestMethod]
    public void NewOrderSubmitRequests_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<OrderSubmitRequests>();
        var originalClientOrderId = new OrderSubmitRequests
        {
            FirstRequest = BuildSubmitRequest(), SecondRequest = BuildSubmitRequest()
          , ThirdRequest = BuildSubmitRequest(), FourthRequest = BuildSubmitRequest()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                                                                  socketBufferReadContext.EncodedBuffer, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer.ReadCursor = MessageHeader.SerializationSize;

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<OrderSubmitRequests>(new OrxDeserializerLookup(
                                                                                            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstRequest, deserializedOrxClientOrderId.FirstRequest);
        Assert.AreEqual(originalClientOrderId.SecondRequest, deserializedOrxClientOrderId.SecondRequest);
        Assert.AreEqual(originalClientOrderId.ThirdRequest, deserializedOrxClientOrderId.ThirdRequest);
        Assert.AreEqual(originalClientOrderId.FourthRequest, deserializedOrxClientOrderId.FourthRequest);
    }

    private OrxOrderSubmitRequest BuildSubmitRequest() =>
        new(new OrxSpotOrder
                (new OrxOrderId(2), 1234, 3456u, OrderSide.Bid, 1.23456m, 300_000L,
                 OrderType.PassiveLimit, displaySize: 100_000, allowedPriceSlippage: 0.00025m, allowedVolumeSlippage: 10_000m, tickerName: "TestTicker"
                ), 1,
            new DateTime(2018, 3, 30, 2, 18, 2),
            new DateTime(2018, 3, 30, 2, 18, 2), "Tag")
        {
            Version = 23, SequenceNumber = 11, IsReplay = true, SendTime = new DateTime(2018, 4, 2, 10, 58, 58)
        };

    public class OrderSubmitRequests
    {
        [OrxMandatoryField(0)] public OrxOrderSubmitRequest? FirstRequest { get; set; }

        [OrxMandatoryField(1)] public OrxOrderSubmitRequest? SecondRequest { get; set; }

        [OrxOptionalField(1)] public OrxOrderSubmitRequest? ThirdRequest { get; set; }

        [OrxOptionalField(2)] public OrxOrderSubmitRequest? FourthRequest { get; set; }
    }
}
