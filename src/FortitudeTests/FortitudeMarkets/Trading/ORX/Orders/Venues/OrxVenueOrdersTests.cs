#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.ORX.Orders;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.Orders.Venues;

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
            EncodedBuffer = new CircularReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
            , MessageHeader = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, 1)
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

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer!, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer!.ReadCursor = MessageHeader.SerializationSize;

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
                new OrxOrderId(1234, adapterOrderId: 1234),
                OrderStatus.New, new OrxVenue(1234, "FirstVenue"), "FirstTicker", 1.23456m, 1_234_345m)
            , new(
                new OrxVenueOrderId("SecondVenueClientOrderId", "SecondVenuesOrderId"),
                new OrxOrderId(1234, adapterOrderId: 1234),
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
