#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders.Products.General;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.Products.General;

[TestClass]
public class OrxSpotOrderTests
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
    public void NewSpotOrders_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<SpotOrders>();
        var originalClientOrderId = new SpotOrders
        {
            FirstSpotOrder = BuildSpotOrder(), SecondSpotOrder = BuildSpotOrder(), ThirdSpotOrder = BuildSpotOrder()
            , FourthSpotOrder = BuildSpotOrder()
        };

        dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<SpotOrders>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(dispatchContext);

        Assert.AreEqual(originalClientOrderId.FirstSpotOrder, deserializedOrxClientOrderId.FirstSpotOrder);
        Assert.AreEqual(originalClientOrderId.SecondSpotOrder, deserializedOrxClientOrderId.SecondSpotOrder);
        Assert.AreEqual(originalClientOrderId.ThirdSpotOrder, deserializedOrxClientOrderId.ThirdSpotOrder);
        Assert.AreEqual(originalClientOrderId.FourthSpotOrder, deserializedOrxClientOrderId.FourthSpotOrder);
    }

    private OrxSpotOrder BuildSpotOrder() =>
        new("TestTicker", OrderSide.Bid, 1.234567m, 988_765_123,
            OrderType.Limit, 100_000m, 0.0001m, 1000m,
            FillExpectation.Complete, new OrxVenuePriceQuoteId(123, 234, 1234567u, 123456u,
                new DateTime(2018, 4, 1, 19, 11, 21)), 1.23456m, 100_000m);

    public class SpotOrders
    {
        [OrxMandatoryField(0)] public OrxSpotOrder? FirstSpotOrder { get; set; }

        [OrxMandatoryField(1)] public OrxSpotOrder? SecondSpotOrder { get; set; }

        [OrxOptionalField(1)] public OrxSpotOrder? ThirdSpotOrder { get; set; }

        [OrxOptionalField(2)] public OrxSpotOrder? FourthSpotOrder { get; set; }
    }
}
