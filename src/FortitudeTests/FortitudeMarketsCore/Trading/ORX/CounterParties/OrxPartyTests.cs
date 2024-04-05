#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.CounterParties;

[TestClass]
public class OrxPartyTests
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
    public void NewParty_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Parties>();
        var originalClientOrderId = new Parties
        {
            FirstParty = BuildSpotOrder(), SecondParty = BuildSpotOrder(), ThirdParty = BuildSpotOrder()
            , FourthParty = BuildSpotOrder()
        };

        socketBufferReadContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<Parties>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstParty, deserializedOrxClientOrderId.FirstParty);
        Assert.AreEqual(originalClientOrderId.SecondParty, deserializedOrxClientOrderId.SecondParty);
        Assert.AreEqual(originalClientOrderId.ThirdParty, deserializedOrxClientOrderId.ThirdParty);
        Assert.AreEqual(originalClientOrderId.FourthParty, deserializedOrxClientOrderId.FourthParty);
    }

    private OrxParty BuildSpotOrder() =>
        new("BuySidePartyId", "BuySideName",
            new OrxParty("BuySideParentPartyId", "BuySideParentPartyName", null,
                "BuySideParentClientPartyId", new OrxBookingInfo("", "")),
            "BuySideClientParty", new OrxBookingInfo("BuySidePortfolio", "BuySideSubPortfolio"));

    public class Parties
    {
        [OrxMandatoryField(0)] public OrxParty? FirstParty { get; set; }

        [OrxMandatoryField(1)] public OrxParty? SecondParty { get; set; }

        [OrxOptionalField(1)] public OrxParty? ThirdParty { get; set; }

        [OrxOptionalField(2)] public OrxParty? FourthParty { get; set; }
    }
}
