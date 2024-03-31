#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Venues;

[TestClass]
public class OrxVenueOrdersTests
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
    public void NewVenueOrders_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<VenueCriterias>();
        var originalClientOrderId = new VenueCriterias
        {
            FirstVenueOrders = BuildVenueOrders(), SecondVenueOrders = BuildVenueOrders()
            , ThirdVenueOrders = BuildVenueOrders(), FourthVenueOrders = BuildVenueOrders()
        };

        socketBufferReadContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var venueOrdersDeserializer = new OrxByteDeserializer<VenueCriterias>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedVenueOrders = (VenueCriterias)venueOrdersDeserializer
            .Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstVenueOrders, deserializedVenueOrders.FirstVenueOrders);
        Assert.AreEqual(originalClientOrderId.SecondVenueOrders, deserializedVenueOrders.SecondVenueOrders);
        Assert.AreEqual(originalClientOrderId.ThirdVenueOrders, deserializedVenueOrders.ThirdVenueOrders);
        Assert.AreEqual(originalClientOrderId.FourthVenueOrders, deserializedVenueOrders.FourthVenueOrders);
    }

    private OrxVenueOrders BuildVenueOrders() =>
        new(new List<OrxVenueOrder>
        {
            new(
                new OrxVenueOrderId("FirstVenueClientOrderId", "FirstVenuesOrderId"),
                new OrxOrderId(1234, "ClientId1234", 0, null, null, "AdapterOrderId"),
                OrderStatus.New, new OrxVenue(1234, "FirstVenue"), "FirstTicker", 1.23456m, 1_234_345m)
            , new(
                new OrxVenueOrderId("SecondVenueClientOrderId", "SecondVenuesOrderId"),
                new OrxOrderId(1234, "ClientId1234", 0, null, null, "AdapterOrderId"),
                OrderStatus.New, new OrxVenue(2345, "SecondVenue"), "SecondTicker", 1.23456m, 1_234_345m)
        });

    public class VenueCriterias
    {
        [OrxMandatoryField(0)] public OrxVenueOrders? FirstVenueOrders { get; set; }

        [OrxMandatoryField(1)] public OrxVenueOrders? SecondVenueOrders { get; set; }

        [OrxOptionalField(1)] public OrxVenueOrders? ThirdVenueOrders { get; set; }

        [OrxOptionalField(2)] public OrxVenueOrders? FourthVenueOrders { get; set; }
    }
}
