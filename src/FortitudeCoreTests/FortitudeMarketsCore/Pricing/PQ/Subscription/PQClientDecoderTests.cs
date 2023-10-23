#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQClientDecoderTests
{
    private const int BufferReadWriteOffset = 100;
    private const uint ExpectedStreamId = uint.MaxValue;
    private const int TotalDataHeaderByteSize = 14;
    private const int MessageSizeToQuoteSerializer = 126;
    private DispatchContext dispatchContext = null!;
    private Mock<IBinaryDeserializer> moqBinaryDeserializer = null!;
    private Mock<IMap<uint, IBinaryDeserializer>> moqDeserializersMap = null!;
    private PQClientDecoder pqClientDecoder = null!;
    private ReadWriteBuffer readWriteBuffer = null!;
    private SourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        readWriteBuffer = new ReadWriteBuffer(new byte[9000]);
        dispatchContext = new DispatchContext
        {
            DetectTimestamp = new DateTime(2017, 07, 01, 18, 59, 22)
            , ReceivingTimestamp = new DateTime(2017, 07, 01, 19, 03, 22)
            , DeserializerTimestamp = new DateTime(2017, 07, 01, 19, 03, 52), EncodedBuffer = readWriteBuffer
        };
        readWriteBuffer.ReadCursor = BufferReadWriteOffset;
        sourceTickerQuoteInfo = new SourceTickerQuoteInfo(ExpectedStreamId, "TestSource", "TestTicker",
            20, 0.00001m, 30000m, 50000000m, 1000m, 1, LayerFlags.Volume | LayerFlags.Price,
            LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName |
            LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        moqDeserializersMap = new Mock<IMap<uint, IBinaryDeserializer>>();
        moqBinaryDeserializer = new Mock<IBinaryDeserializer>();
        // ReSharper disable once NotAccessedVariable -- sets the mock with the object to return.
        var binUnserialzierObj = moqBinaryDeserializer.Object;
        moqDeserializersMap.Setup(um => um.TryGetValue(ExpectedStreamId, out binUnserialzierObj)).Returns(true)
            .Verifiable();

        pqClientDecoder = new PQClientDecoder(moqDeserializersMap.Object, PQFeedType.Snapshot);
    }

    [TestMethod]
    public void TwoQuoteDataUpdates_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer.Setup(bu => bu.Deserialize(dispatchContext))
            .Callback<DispatchContext>(dc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + TotalDataHeaderByteSize, dc.EncodedBuffer!.ReadCursor);
                Assert.AreEqual(MessageSizeToQuoteSerializer, dc.MessageSize);
            })
            .Returns(null!).Verifiable();
        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };

        var quoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);
        var amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer,
            BufferReadWriteOffset, expectedL0Quote);
        readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amountWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WrittenCursor;

        amountWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer,
            readWriteBuffer.WrittenCursor, expectedL0Quote);
        readWriteBuffer.WrittenCursor += amountWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify(bu => bu.Deserialize(dispatchContext), Times.Exactly(2));
    }


    [TestMethod]
    public void OneQuoteDataUpdateOneHeartbeat_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer.Setup(bu => bu.Deserialize(dispatchContext))
            .Callback<DispatchContext>(dc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + TotalDataHeaderByteSize, dc.EncodedBuffer!.ReadCursor);
                Assert.AreEqual(0, dc.MessageSize);
            })
            .Returns(null!).Verifiable();
        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };

        var listOfHeartBeatsToUpdate = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>(2) { expectedL0Quote });

        var heartBeatSerializer = new PQHeartbeatSerializer();
        var amtWritten = heartBeatSerializer.Serialize(readWriteBuffer.Buffer, BufferReadWriteOffset,
            listOfHeartBeatsToUpdate);
        readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WrittenCursor;
        moqBinaryDeserializer.Setup(bu => bu.Deserialize(dispatchContext))
            .Callback<DispatchContext>(dc =>
            {
                Assert.AreEqual(writeStartOffset + TotalDataHeaderByteSize, dc.EncodedBuffer!.ReadCursor);
            })
            .Returns(null!).Verifiable();

        var quoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);
        amtWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer,
            readWriteBuffer.WrittenCursor, expectedL0Quote);
        readWriteBuffer.WrittenCursor += amtWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();
    }

    [TestMethod]
    public void OneHeartbeatOneQuoteDataUpdate_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer.Setup(bu => bu.Deserialize(dispatchContext))
            .Callback<DispatchContext>(dc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + TotalDataHeaderByteSize, dc.EncodedBuffer!.ReadCursor);
            })
            .Returns(null!).Verifiable();

        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };
        var quoteSerializer = new PQQuoteSerializer(UpdateStyle.FullSnapshot);
        var amtWritten = quoteSerializer.Serialize(readWriteBuffer.Buffer,
            BufferReadWriteOffset, expectedL0Quote);
        readWriteBuffer.WrittenCursor = BufferReadWriteOffset + amtWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WrittenCursor;

        moqBinaryDeserializer.Setup(bu => bu.Deserialize(dispatchContext))
            .Callback<DispatchContext>(dc =>
            {
                Assert.AreEqual(writeStartOffset + TotalDataHeaderByteSize, dc.EncodedBuffer!.ReadCursor);
                Assert.AreEqual(0, dc.MessageSize);
            })
            .Returns(null!)
            .Verifiable();

        var listOfHeartBeatsToUpdate = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>(2) { expectedL0Quote });

        var heartBeatSerializer = new PQHeartbeatSerializer();
        amtWritten = heartBeatSerializer.Serialize(readWriteBuffer.Buffer, readWriteBuffer.WrittenCursor,
            listOfHeartBeatsToUpdate);
        readWriteBuffer.WrittenCursor += amtWritten;

        pqClientDecoder.Process(dispatchContext);

        Assert.AreEqual(readWriteBuffer.WrittenCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();
    }
}
