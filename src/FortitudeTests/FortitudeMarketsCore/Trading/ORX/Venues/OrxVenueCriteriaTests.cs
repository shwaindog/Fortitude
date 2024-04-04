#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Venues;

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
            EncodedBuffer = new ReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
            , MessageVersion = TradingVersionInfo.CurrentVersion
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

        socketBufferReadContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

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
