#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.Venues;

[TestClass]
public class OrxVenueCriteriaTests
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
    public void NewVenueCriterias_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<VenueCriterias>();
        var originalClientOrderId = new VenueCriterias
        {
            FirstVenueCriteria = BuildSpotOrder(), SecondVenueCriteria = BuildSpotOrder()
            , ThirdVenueCriteria = BuildSpotOrder(), FourthVenueCriteria = BuildSpotOrder()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer!, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer!.ReadCursor = MessageHeader.SerializationSize;

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<VenueCriterias>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = (VenueCriterias)orderSubmitRequestsDeserializer
            .Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstVenueCriteria, deserializedOrxClientOrderId.FirstVenueCriteria);
        Assert.AreEqual(originalClientOrderId.SecondVenueCriteria, deserializedOrxClientOrderId.SecondVenueCriteria);
        Assert.AreEqual(originalClientOrderId.ThirdVenueCriteria, deserializedOrxClientOrderId.ThirdVenueCriteria);
        Assert.AreEqual(originalClientOrderId.FourthVenueCriteria, deserializedOrxClientOrderId.FourthVenueCriteria);
    }

    private OrxVenueCriteria BuildSpotOrder() =>
        new(new List<OrxVenue>
        {
            new(1234, "FirstOrxVenue"), new(45678, "SecondOrxVenue")
        }, VenueSelectionMethod.Default);

    public class VenueCriterias
    {
        [OrxMandatoryField(0)] public OrxVenueCriteria? FirstVenueCriteria { get; set; }

        [OrxMandatoryField(1)] public OrxVenueCriteria? SecondVenueCriteria { get; set; }

        [OrxOptionalField(1)] public OrxVenueCriteria? ThirdVenueCriteria { get; set; }

        [OrxOptionalField(2)] public OrxVenueCriteria? FourthVenueCriteria { get; set; }
    }
}
