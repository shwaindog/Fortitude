﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using Moq;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

[TestClass]
public class PQClientMessageStreamDecoderTests
{
    private const long   BufferReadWriteOffset = 3;
    private const ushort ExpectedSourceId      = ushort.MaxValue;
    private const ushort ExpectedTickerId      = ushort.MaxValue;
    private const uint   ExpectedStreamId      = uint.MaxValue;

    private const uint MessageSizeToQuoteSerializer = 142 + PQQuoteMessageHeader.HeaderSize;

    private Mock<IPQClientQuoteDeserializerRepository> clientDeserializerRepo = null!;

    private IConversation?               lastReceivedConversation;
    private List<ISourceTickerQuoteInfo> lastReceivedSourceTickerQuoteInfos = null!;
    private Mock<IMessageDeserializer>   moqBinaryDeserializer              = null!;
    private Mock<IConversation>          moqConversation                    = null!;
    private PQClientMessageStreamDecoder pqClientMessageStreamDecoder       = null!;
    private CircularReadWriteBuffer      readWriteBuffer                    = null!;

    private List<ISourceTickerQuoteInfo> sendSourceTickerQuoteInfos = new()
    {
        new SourceTickerQuoteInfo
            (0x7777, "FirstSource", 3333, "FirstTicker", Level3, Unknown
           , 7, 0.000005m, 1m, 10_000_000m, 2m, 1
           , LayerFlags.Price | LayerFlags.ValueDate, LastTradedFlags.LastTradedPrice)
      , new SourceTickerQuoteInfo
            (0x5151, "SecondSource", 7777, "SecondTicker", Level3, Unknown
           , 20, 0.05m, 10_000m, 1_000_000m, 5_000m, 2_000
           , LayerFlags.Price | LayerFlags.TraderName | LayerFlags.Executable, LastTradedFlags.PaidOrGiven)
      , new SourceTickerQuoteInfo
            (0xFFFF, "ThirdSource", 0001, "ThirdTicker", Level3, Unknown
           , 1, 5m, 100_000m, 100_000_000m, 50_000m, 1
           , LayerFlags.None)
    };

    private SocketBufferReadContext socketBufferReadContext = null!;

    private ConversationMessageReceivedHandler<PQSourceTickerInfoResponse> sourceTickerInfoResponseCallBack = null!;

    private PQSourceTickerInfoResponseSerializer sourceTickerInfoResponseSerializer = null!;

    private SourceTickerQuoteInfo sourceTickerQuoteInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqConversation = new Mock<IConversation>();
        readWriteBuffer = new CircularReadWriteBuffer(new byte[9000]);
        socketBufferReadContext = new SocketBufferReadContext
        {
            DetectTimestamp    = new DateTime(2017, 07, 01, 18, 59, 22)
          , ReceivingTimestamp = new DateTime(2017, 07, 01, 19, 03, 22)
          , DeserializerTime   = new DateTime(2017, 07, 01, 19, 03, 52), EncodedBuffer = readWriteBuffer
          , Conversation       = moqConversation.As<IConversationRequester>().Object
        };
        readWriteBuffer.ReadCursor  = BufferReadWriteOffset;
        readWriteBuffer.WriteCursor = BufferReadWriteOffset;
        sourceTickerQuoteInfo =
            new SourceTickerQuoteInfo
                (ExpectedSourceId, "TestSource", ExpectedTickerId, "TestTicker", Level3, Unknown
               , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
               , LayerFlags.Volume | LayerFlags.Price
               , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);

        clientDeserializerRepo = new Mock<IPQClientQuoteDeserializerRepository>();
        moqBinaryDeserializer  = new Mock<IMessageDeserializer>();
        // ReSharper disable once NotAccessedVariable -- sets the mock with the object to return.
        var binUnserialzierObj = moqBinaryDeserializer.Object;
        clientDeserializerRepo.Setup(um => um.TryGetDeserializer(ExpectedStreamId, out binUnserialzierObj)).Returns(true)
                              .Verifiable();
        sourceTickerInfoResponseCallBack = (sourceTickerInfoResponse, header, conversation) =>
        {
            lastReceivedConversation           = conversation;
            lastReceivedSourceTickerQuoteInfos = sourceTickerInfoResponse.SourceTickerQuoteInfos;
        };
        sourceTickerInfoResponseSerializer = new PQSourceTickerInfoResponseSerializer();

        pqClientMessageStreamDecoder
            = new PQClientMessageStreamDecoder(clientDeserializerRepo.Object);
    }

    [TestMethod]
    public void TwoQuoteDataUpdates_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer
            .Setup(bu => bu.Deserialize(socketBufferReadContext))
            .Callback<IBufferContext>(bc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + PQQuoteMessageHeader.HeaderSize, bc.EncodedBuffer!.ReadCursor);
                if (bc is ISocketBufferReadContext socketBufferReadContext)
                    Assert.AreEqual(MessageSizeToQuoteSerializer, socketBufferReadContext.MessageHeader.MessageSize);
                else
                    Assert.Fail("Expected bufferContext to be an ISocketBufferReadContext");
            })
            .Returns(null!).Verifiable();
        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amountWritten   = quoteSerializer.Serialize(readWriteBuffer, expectedL0Quote);
        readWriteBuffer.WriteCursor = BufferReadWriteOffset + amountWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WriteCursor;

        amountWritten = quoteSerializer.Serialize(readWriteBuffer, expectedL0Quote);

        readWriteBuffer.WriteCursor += amountWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify(bu => bu.Deserialize(socketBufferReadContext), Times.Exactly(2));
    }


    [TestMethod]
    public void OneQuoteDataUpdateOneHeartbeat_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer
            .Setup(bu => bu.Deserialize(socketBufferReadContext))
            .Callback<IBufferContext>(bc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + PQQuoteMessageHeader.HeaderSize, bc.EncodedBuffer!.ReadCursor);
                if (bc is ISocketBufferReadContext socketBufferReadContext)
                    Assert.AreEqual(14U, socketBufferReadContext.MessageHeader.MessageSize);
                else
                    Assert.Fail("Expected bufferContext to be an ISocketBufferReadContext");
            })
            .Returns(null!).Verifiable();
        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };

        var listOfHeartBeatsToUpdate = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>(2) { expectedL0Quote });

        var heartBeatSerializer = new PQHeartbeatSerializer();
        var amtWritten          = heartBeatSerializer.Serialize(readWriteBuffer, listOfHeartBeatsToUpdate);
        readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WriteCursor;
        moqBinaryDeserializer
            .Setup(bu => bu.Deserialize(socketBufferReadContext))
            .Callback<IBufferContext>(dc =>
            {
                Assert.AreEqual(writeStartOffset + PQQuoteMessageHeader.HeaderSize
                              , dc.EncodedBuffer!.ReadCursor);
            })
            .Returns(null!).Verifiable();

        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        amtWritten                  =  quoteSerializer.Serialize(readWriteBuffer, expectedL0Quote);
        readWriteBuffer.WriteCursor += amtWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();
    }

    [TestMethod]
    public void OneHeartbeatOneQuoteDataUpdate_ProcessTwice_DecodesStreamAndCompletes()
    {
        var writeStartOffset = BufferReadWriteOffset;
        moqBinaryDeserializer
            .Setup(bu => bu.Deserialize(socketBufferReadContext))
            .Callback<IBufferContext>(bc =>
            {
                // ReSharper disable once AccessToModifiedClosure
                Assert.AreEqual(writeStartOffset + PQQuoteMessageHeader.HeaderSize, bc.EncodedBuffer!.ReadCursor);
            })
            .Returns(null!).Verifiable();

        var expectedL0Quote = new PQLevel0Quote(sourceTickerQuoteInfo)
        {
            SinglePrice = 0.78568m, SourceTime = new DateTime(2017, 07, 01, 19, 35, 00), IsReplay = true
        };
        var quoteSerializer = new PQQuoteSerializer(PQMessageFlags.Snapshot);
        var amtWritten      = quoteSerializer.Serialize(readWriteBuffer, expectedL0Quote);
        readWriteBuffer.WriteCursor = BufferReadWriteOffset + amtWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();

        writeStartOffset = readWriteBuffer.WriteCursor;

        moqBinaryDeserializer
            .Setup(bu => bu.Deserialize(socketBufferReadContext))
            .Callback<IBufferContext>(bc =>
            {
                Assert.AreEqual(writeStartOffset + PQQuoteMessageHeader.HeaderSize, bc.EncodedBuffer!.ReadCursor);
                if (bc is IMessageBufferContext messageBufferContext)
                    Assert.AreEqual(14U, messageBufferContext.MessageHeader.MessageSize);
                else
                    Assert.Fail("Expected bufferContext to be an ISocketBufferReadContext");
            })
            .Returns(null!)
            .Verifiable();

        var listOfHeartBeatsToUpdate = new PQHeartBeatQuotesMessage(new List<IPQLevel0Quote>(2) { expectedL0Quote });

        var heartBeatSerializer = new PQHeartbeatSerializer();
        amtWritten                  =  heartBeatSerializer.Serialize(readWriteBuffer, listOfHeartBeatsToUpdate);
        readWriteBuffer.WriteCursor += amtWritten;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
        moqBinaryDeserializer.Verify();
    }

    [TestMethod]
    public void TwoSourceTickerInfoRequest_ProcessTwice_DecodesStreamAndCompletes()
    {
        pqClientMessageStreamDecoder
            = new PQClientMessageStreamDecoder(new PQClientQuoteDeserializerRepository("PQClientTest", new Recycler()));
        pqClientMessageStreamDecoder
            .MessageDeserializationRepository
            .RegisterDeserializer<PQSourceTickerInfoResponse>()
            .AddDeserializedNotifier
                (new PassThroughDeserializedNotifier<PQSourceTickerInfoResponse>
                    ($"{nameof(PQClientMessageStreamDecoderTests)}.{nameof(sourceTickerInfoResponseCallBack)}"
                   , sourceTickerInfoResponseCallBack));
        var sourceTickerInfoResponse = new PQSourceTickerInfoResponse(sendSourceTickerQuoteInfos);
        sourceTickerInfoResponseSerializer.Serialize(sourceTickerInfoResponse, (ISerdeContext)socketBufferReadContext);
        var amtWritten = socketBufferReadContext.LastWriteLength;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsTrue(sendSourceTickerQuoteInfos.SequenceEqual(lastReceivedSourceTickerQuoteInfos));
        Assert.AreSame(moqConversation.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);

        sendSourceTickerQuoteInfos = new List<ISourceTickerQuoteInfo>
        {
            new SourceTickerQuoteInfo
                (1111, "FourthSource", 5555, "FourthTicker", Level3, Unknown
               , 7, 0.000005m, 1m, 10_000_000m, 2m, 1
               , LayerFlags.Price | LayerFlags.ValueDate, LastTradedFlags.LastTradedPrice)
          , new SourceTickerQuoteInfo
                (0xAAAA, "FifthSource", 3333, "FifthTicker", Level3, Unknown
               , 20, 0.05m, 10_000m, 1_000_000m, 5_000m, 2_000
               , LayerFlags.Price | LayerFlags.TraderName | LayerFlags.Executable, LastTradedFlags.PaidOrGiven)
          , new SourceTickerQuoteInfo
                (0x2222, "SixthSource", 7777, "SixthTicker", Level3, Unknown
               , 1, 5m, 100_000m, 100_000_000m, 50_000m, 1
               , LayerFlags.None, LastTradedFlags.None)
        };

        sourceTickerInfoResponse = new PQSourceTickerInfoResponse(sendSourceTickerQuoteInfos);
        sourceTickerInfoResponseSerializer.Serialize(sourceTickerInfoResponse, (ISerdeContext)socketBufferReadContext);
        amtWritten                         = socketBufferReadContext.LastWriteLength;
        lastReceivedSourceTickerQuoteInfos = null!;

        pqClientMessageStreamDecoder.Process(socketBufferReadContext);

        Assert.IsTrue(lastReceivedSourceTickerQuoteInfos!.SequenceEqual(sendSourceTickerQuoteInfos));
        Assert.AreSame(moqConversation.Object, lastReceivedConversation);
        Assert.AreEqual(readWriteBuffer.WriteCursor, readWriteBuffer.ReadCursor);
    }
}
