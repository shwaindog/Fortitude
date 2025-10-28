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
public class OrxPartiesTests
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
    public void NewParties_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<TestParties>();
        var originalClientOrderId = new TestParties
        {
            FirstParties = BuildSpotOrder(), SecondParties = BuildSpotOrder(), ThirdParties = BuildSpotOrder()
            , FourthParties = BuildSpotOrder()
        };

        var messageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            socketBufferReadContext.EncodedBuffer, OrxMessageHeader.HeaderSize);
        socketBufferReadContext.MessageHeader
            = new MessageHeader(TradingVersionInfo.CurrentVersion, 0, 0, (uint)messageSize + MessageHeader.SerializationSize);
        socketBufferReadContext.EncodedBuffer.ReadCursor = MessageHeader.SerializationSize;

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<TestParties>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(socketBufferReadContext);

        Assert.AreEqual(originalClientOrderId.FirstParties, deserializedOrxClientOrderId.FirstParties);
        Assert.AreEqual(originalClientOrderId.SecondParties, deserializedOrxClientOrderId.SecondParties);
        Assert.AreEqual(originalClientOrderId.ThirdParties, deserializedOrxClientOrderId.ThirdParties);
        Assert.AreEqual(originalClientOrderId.FourthParties, deserializedOrxClientOrderId.FourthParties);
    }

    private uint partyId = 1;
    private uint portfolioId= 10001;

    private OrxParties BuildSpotOrder() =>
        new(partyId ++, new OrxPartyPortfolio(portfolioId++),
            new OrxPartyPortfolio(portfolioId++));

    public class TestParties
    {
        [OrxMandatoryField(0)] public OrxParties? FirstParties { get; set; }

        [OrxMandatoryField(1)] public OrxParties? SecondParties { get; set; }

        [OrxOptionalField(1)] public OrxParties? ThirdParties { get; set; }

        [OrxOptionalField(2)] public OrxParties? FourthParties { get; set; }
    }
}
