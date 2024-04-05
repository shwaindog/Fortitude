#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Executions;

[TestClass]
public class OrxExecutionsTests
{
    private const int BufferSize = 4096;
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
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Executions>();
        var originalClientOrderId = new Executions
        {
            FirstExecutions = BuildVenueOrders(), SecondExecutions = BuildVenueOrders()
            , ThirdExecutions = BuildVenueOrders(), FourthExecutions = BuildVenueOrders()
        };

        socketBufferReadContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var venueOrdersDeserializer = new OrxByteDeserializer<Executions>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedVenueOrders = venueOrdersDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstExecutions, deserializedVenueOrders.FirstExecutions);
        Assert.AreEqual(originalClientOrderId.SecondExecutions, deserializedVenueOrders.SecondExecutions);
        Assert.AreEqual(originalClientOrderId.ThirdExecutions, deserializedVenueOrders.ThirdExecutions);
        Assert.AreEqual(originalClientOrderId.FourthExecutions, deserializedVenueOrders.FourthExecutions);
    }

    private OrxExecutions BuildVenueOrders() =>
        new(new List<OrxExecution>
        {
            new(
                new OrxExecutionId("FirstVenueExecutionId", 1234, "FirstVenuesBookingSystemId"),
                new OrxVenue(1234, "FirstVenue"), new OrxVenueOrderId("FirstVenueClientOrderId", "FirstVenuesOrderId"),
                new OrxOrderId(123, "Testing 123", 234, "Testing 234", new OrxOrderId(345, "Testing 345")
                    , "TrackingId1234"),
                new DateTime(2018, 4, 1, 23, 44, 54), 1.23456m, 1_234_345m,
                new OrxParty("PartyId", "PartyName", null,
                    "PartyId", new OrxBookingInfo("", "")), DateTimeConstants.UnixEpoch, ExecutionType.CounterPartyGave
                , ExecutionStageType.Trade)
            , new(
                new OrxExecutionId("SecondVenueExecutionId", 1234, "SecondVenuesBookingSystemId"),
                new OrxVenue(2345, "SecondVenue")
                , new OrxVenueOrderId("SecondVenueClientOrderId", "SecondVenuesOrderId"),
                new OrxOrderId(123, "Testing 123", 234, "Testing 234", new OrxOrderId(345, "Testing 345")
                    , "TrackingId1234"),
                new DateTime(2018, 4, 1, 23, 44, 54), 1.23456m, 1_234_345m,
                new OrxParty("PartyId", "PartyName", null,
                    "PartyId", new OrxBookingInfo("", "")), DateTimeConstants.UnixEpoch, ExecutionType.CounterPartyGave
                , ExecutionStageType.Trade)
        });

    public class Executions
    {
        [OrxMandatoryField(0)] public OrxExecutions? FirstExecutions { get; set; }

        [OrxMandatoryField(1)] public OrxExecutions? SecondExecutions { get; set; }

        [OrxOptionalField(2)] public OrxExecutions? ThirdExecutions { get; set; }

        [OrxOptionalField(3)] public OrxExecutions? FourthExecutions { get; set; }
    }
}
