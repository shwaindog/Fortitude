// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Conversations.Builders;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeIO.Protocols.Serialization;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations;

[TestClass]
[NoMatchingProductionClass]
public class TcpRequestResponderConnectionTests
{
    private readonly AutoResetEvent autoResetEvent = new(false);

    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(TcpRequestResponderConnectionTests));

    private readonly NetworkTopicConnectionConfig originalResponderTopicConConfig =
        new("ResponderConName", "TestResponderName"
          , SocketConversationProtocol.TcpAcceptor,
            new List<IEndpointConfig>
            {
                new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                                 , TestMachineConfig.ServerUpdatePort)
            }, "TcpRequestResponderConnectionTests",
            1024 * 1024 * 2, 1024 * 1024 * 2, 50,
            SocketConnectionAttributes.Reliable |
            SocketConnectionAttributes.TransportHeartBeat
           );

    private readonly IRecycler recycler = new Recycler();

    private readonly Dictionary<uint, IMessageSerializer> serializers = new()
    {
        { 2345, new SimpleVersionedMessage.SimpleSerializer() }, { 159, new SimpleVersionedMessage.SimpleSerializer() }
    };

    private Dictionary<uint, IMessageDeserializer> requesterDeserializers           = null!;
    private SimpleVersionedMessage                 requesterReceivedResponseMessage = null!;
    private MessageSerdesRepositoryFactory         requesterSerdesFactory           = null!;

    private SimpleMessageStreamDecoder.SimpleDeserializerMessageDeserializationFactory requesterStreamDeserializerRepo = null!;

    private Dictionary<uint, IMessageDeserializer> responderDeserializers = null!;

    private SimpleVersionedMessage         responderReceivedMessage = null!;
    private MessageSerdesRepositoryFactory responderSerdesFactory   = null!;

    private SimpleMessageStreamDecoder.SimpleDeserializerMessageDeserializationFactory responderStreamDeserializerRepo = null!;

    private SimpleMessageStreamDecoder.SimpleSerializerFactory streamSerializerRepo = null!;

    private ConversationRequester tcpRequester = null!;

    private ConversationResponder tcpResponder = null!;

    private SimpleVersionedMessage v2Message = null!;

    [TestInitialize]
    public void Setup()
    {
        responderDeserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
          , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        responderStreamDeserializerRepo = new SimpleMessageStreamDecoder.SimpleDeserializerMessageDeserializationFactory(responderDeserializers);
        streamSerializerRepo            = new SimpleMessageStreamDecoder.SimpleSerializerFactory(serializers);
        responderSerdesFactory
            = new MessageSerdesRepositoryFactory(streamSerializerRepo, responderStreamDeserializerRepo, responderStreamDeserializerRepo);

        requesterDeserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
          , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        requesterStreamDeserializerRepo = new SimpleMessageStreamDecoder.SimpleDeserializerMessageDeserializationFactory(requesterDeserializers);
        requesterSerdesFactory
            = new MessageSerdesRepositoryFactory(streamSerializerRepo, requesterStreamDeserializerRepo, requesterStreamDeserializerRepo);

        v2Message = new SimpleVersionedMessage { Version = 2, Payload2 = 234567.0, MessageId = 2345 };
    }

    public ConversationRequester BuildConversationRequester(INetworkTopicConnectionConfig requesterTopicConConfig)
    {
        var tcpRequesterBuilder = new TcpConversationRequesterBuilder();
        requesterTopicConConfig.ConversationProtocol = SocketConversationProtocol.TcpClient;
        requesterTopicConConfig.ConnectionName       = "RequesterConName";
        return tcpRequesterBuilder.Build(requesterTopicConConfig, requesterSerdesFactory);
    }

    public ConversationResponder BuildConversationResponder(INetworkTopicConnectionConfig responderTopicConConfig)
    {
        var tcpResponderBuilder = new TcpConversationResponderBuilder();
        return tcpResponderBuilder.Build(responderTopicConConfig, responderSerdesFactory);
    }

    [TestCleanup]
    public void TearDown()
    {
        tcpRequester.Stop();
        tcpResponder.Stop();
        // FLoggerFactory.WaitUntilDrained();
    }

    [TestMethod]
    [Timeout(30_000)]
    public async Task ClientSendMessageDecodesCorrectlyOnServer()
    {
        tcpResponder = BuildConversationResponder(originalResponderTopicConConfig.ShiftPortsBy(10));
        tcpRequester = BuildConversationRequester(originalResponderTopicConConfig.ToggleProtocolDirection().ShiftPortsBy(10));

        var threadPoolExecutionContext = recycler.Borrow<ThreadPoolExecutionContextResult<bool, TimeSpan>>();
        // client connects
        var started = await tcpResponder.StartAsync(10_000, threadPoolExecutionContext);
        Assert.IsTrue(started);
        await Task.Delay(20);
        threadPoolExecutionContext = recycler.Borrow<ThreadPoolExecutionContextResult<bool, TimeSpan>>();
        started                    = await tcpRequester.StartAsync(10_000, threadPoolExecutionContext);
        Assert.IsTrue(started);

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 responderDeserializers.Values.Cast<INotifyingMessageDeserializer<SimpleVersionedMessage>>())
            deserializersValue.ConversationMessageDeserialized += ReceivedFromClientDeserializerCallback;

        var v1Message = new SimpleVersionedMessage { Version = 1, Payload = 765432, MessageId = 159 };
        // send message
        tcpRequester.StreamPublisher!.Send(v1Message);
        logger.Info("Sent Message to responder");
        autoResetEvent.WaitOne(500);
        // assert server receives properly
        Assert.AreEqual(v1Message.Payload, responderReceivedMessage.Payload);
        Assert.AreEqual(v1Message.MessageId, responderReceivedMessage.MessageId);
        Assert.AreEqual(v1Message.Version, responderReceivedMessage.Version);
    }

    [TestMethod]
    public void ClientSendMessageServerRespondsDecodesCorrectlyOnClient()
    {
        tcpResponder = BuildConversationResponder(originalResponderTopicConConfig.ShiftPortsBy(20));
        tcpRequester = BuildConversationRequester(originalResponderTopicConConfig.ToggleProtocolDirection().ShiftPortsBy(20));
        // client connects
        tcpResponder.Start();
        Thread.Sleep(20);
        tcpRequester.Start();

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 responderDeserializers.Values.Cast<INotifyingMessageDeserializer<SimpleVersionedMessage>>())
            deserializersValue.ConversationMessageDeserialized += RespondToClientMessage;

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 requesterDeserializers.Values.Cast<INotifyingMessageDeserializer<SimpleVersionedMessage>>())
            deserializersValue.ConversationMessageDeserialized += ReceivedFromResponderDeserializerCallback;

        var v1Message = new SimpleVersionedMessage { Version = 2, Payload2 = 1.23456, MessageId = 2345 };
        // send message
        tcpRequester.StreamPublisher!.Send(v1Message);

        logger.Info("Sent Message to responder");
        autoResetEvent.WaitOne(1000);
        // assert server receives properly
        Assert.AreEqual(v2Message.Payload2, requesterReceivedResponseMessage.Payload2);
        Assert.AreEqual(v2Message.MessageId, requesterReceivedResponseMessage.MessageId);
        Assert.AreEqual(v2Message.Version, requesterReceivedResponseMessage.Version);
    }

    private void RespondToClientMessage(SimpleVersionedMessage msg, MessageHeader messageHeader, IConversation client)
    {
        responderReceivedMessage = msg;
        if (client is IConversationRequester conversationRequester) conversationRequester.StreamPublisher!.Send(v2Message);
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, MessageHeader header, IConversation client)
    {
        responderReceivedMessage = msg;
        autoResetEvent.Set();
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, MessageHeader msgHeader)
    {
        responderReceivedMessage = msg;
        autoResetEvent.Set();
    }

    private void ReceivedFromResponderDeserializerCallback(SimpleVersionedMessage msg, MessageHeader messageHeader, IConversation client)
    {
        requesterReceivedResponseMessage = msg;
        autoResetEvent.Set();
    }
}
