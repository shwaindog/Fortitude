﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.ORX.CounterParties;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Products.General;
using FortitudeMarkets.Trading.ORX.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Publication;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.Orders;

[TestClass]
public class OrxOrderTests
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
    public void NewOrders_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Orders>();
        var originalClientOrderId = new Orders
        {
            FirstOrder = BuildVenueOrders(), SecondOrder = BuildVenueOrders(), ThirdOrder = BuildVenueOrders()
            , FourthOrder = BuildVenueOrders()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer!, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer!.ReadCursor = MessageHeader.SerializationSize;

        var ordersDeserializer = new OrxByteDeserializer<Orders>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrders = ordersDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstOrder, deserializedOrders.FirstOrder);
        Assert.AreEqual(originalClientOrderId.SecondOrder, deserializedOrders.SecondOrder);
        Assert.AreEqual(originalClientOrderId.ThirdOrder, deserializedOrders.ThirdOrder);
        Assert.AreEqual(originalClientOrderId.FourthOrder, deserializedOrders.FourthOrder);
    }

    private OrxOrder BuildVenueOrders() =>
        new(new OrxOrderId(1234, "Test1234", 2345, "Test2345", new OrxOrderId(3456, "Test3456"),
                "TrackingId1234"), TimeInForce.GoodTillCancelled, new DateTime(2018, 3, 30, 2, 4, 11),
            OrderStatus.New, new OrxSpotOrder("TestTicker", OrderSide.Bid, 1.23456m, 300_000L,
                OrderType.Limit, 100_000, 0.00025m, 10_000m),
            new DateTime(2018, 3, 30, 2, 18, 2),
            new OrxParties(new OrxParty("", "", null, "", new OrxBookingInfo("", "")), new OrxParty("TestPartyId"
                , "TestPartyName", null, "MyClientPartyId",
                new OrxBookingInfo("TestAccount", "TestSubAccount"))),
            new DateTime(2018, 3, 30, 2, 18, 2),
            new OrxVenueCriteria(new List<OrxVenue> { new(23, "TestVenue") }, VenueSelectionMethod.Default),
            new OrxVenueOrders(), new OrxExecutions(), "OrderMessage", new OrxOrderPublisher());

    public class Orders
    {
        [OrxMandatoryField(0)] public OrxOrder? FirstOrder { get; set; }

        [OrxMandatoryField(1)] public OrxOrder? SecondOrder { get; set; }

        [OrxOptionalField(2)] public OrxOrder? ThirdOrder { get; set; }

        [OrxOptionalField(3)] public OrxOrder? FourthOrder { get; set; }
    }
}
