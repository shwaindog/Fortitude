#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.ORX.Serdes.Serialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Trading;
using FortitudeMarkets.Trading.ORX.CounterParties;

#endregion

namespace FortitudeTests.FortitudeMarkets.Trading.ORX.CounterParties;

[TestClass]
public class OrxPartyPortfolioTests
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
    public void NewParty_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Parties>();
        var originalClientOrderId = new Parties
        {
            FirstParty = BuildSpotOrder(), SecondParty = BuildSpotOrder(), ThirdParty = BuildSpotOrder()
            , FourthParty = BuildSpotOrder()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer!, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer!.ReadCursor = MessageHeader.SerializationSize;

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<Parties>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstParty, deserializedOrxClientOrderId.FirstParty);
        Assert.AreEqual(originalClientOrderId.SecondParty, deserializedOrxClientOrderId.SecondParty);
        Assert.AreEqual(originalClientOrderId.ThirdParty, deserializedOrxClientOrderId.ThirdParty);
        Assert.AreEqual(originalClientOrderId.FourthParty, deserializedOrxClientOrderId.FourthParty);
    }

    private uint buyPartyId = 1;
    private uint buyPartyPortfolioId = 10001;

    private OrxPartyPortfolio BuildSpotOrder() => new(buyPartyId++, buyPartyPortfolioId++);

    public class Parties
    {
        [OrxMandatoryField(0)] public OrxPartyPortfolio? FirstParty { get; set; }

        [OrxMandatoryField(1)] public OrxPartyPortfolio? SecondParty { get; set; }

        [OrxOptionalField(1)] public OrxPartyPortfolio? ThirdParty { get; set; }

        [OrxOptionalField(2)] public OrxPartyPortfolio? FourthParty { get; set; }
    }
}
