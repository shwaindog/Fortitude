#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Orders.Products.General;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Publication;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.Client;

[TestClass]
public class OrxOrderSubmitRequestTests
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
    public void NewOrderSubmitRequests_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<OrderSubmitRequests>();
        var originalClientOrderId = new OrderSubmitRequests
        {
            FirstRequest = BuildSubmitRequest(), SecondRequest = BuildSubmitRequest()
            , ThirdRequest = BuildSubmitRequest(), FourthRequest = BuildSubmitRequest()
        };

        dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<OrderSubmitRequests>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(dispatchContext);

        Assert.AreEqual(originalClientOrderId.FirstRequest, deserializedOrxClientOrderId.FirstRequest);
        Assert.AreEqual(originalClientOrderId.SecondRequest, deserializedOrxClientOrderId.SecondRequest);
        Assert.AreEqual(originalClientOrderId.ThirdRequest, deserializedOrxClientOrderId.ThirdRequest);
        Assert.AreEqual(originalClientOrderId.FourthRequest, deserializedOrxClientOrderId.FourthRequest);
    }

    private OrxOrderSubmitRequest BuildSubmitRequest() =>
        new(new OrxOrder(
                new OrxOrderId(1234, "Test1234", 2345, "Test2345",
                    new OrxOrderId(3456, "Test3456"), "TrackingId1234"), TimeInForce.GoodTillCancelled,
                new DateTime(2018, 3, 30, 2, 4, 11), OrderStatus.New,
                new OrxSpotOrder("TestTicker", OrderSide.Bid, 1.23456m, 300_000L, OrderType.Limit, 100_000,
                    0.00025m, 10_000m),
                new DateTime(2018, 3, 30, 2, 18, 2),
                new OrxParties(new OrxParty("", "", null, "", new OrxBookingInfo("", "")),
                    new OrxParty("TestPartyId", "TestPartyName", null, "MyClientPartyId",
                        new OrxBookingInfo("TestAccount", "TestSubAccount"))),
                new DateTime(2018, 3, 30, 2, 18, 2),
                new OrxVenueCriteria(new List<OrxVenue> { new(23, "TestVenue") },
                    VenueSelectionMethod.Default),
                new OrxVenueOrders(), new OrxExecutions(), "OrderMessage", new OrxOrderPublisher()), 1,
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
