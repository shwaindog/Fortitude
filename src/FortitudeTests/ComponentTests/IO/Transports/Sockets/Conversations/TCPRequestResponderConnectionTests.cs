#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeIO.Protocols.Serialization;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations;

[TestClass]
[NoMatchingProductionClass]
public class TcpRequestResponderConnectionTests
{
    private readonly SocketTopicConnectionConfig responderTopicConConfig = new("TestResponderName"
        , SocketConversationProtocol.TcpAcceptor,
        new List<ISocketConnectionConfig>
        {
            new SocketConnectionConfig(TestMachineConfig.LoopBackIpAddress, TestMachineConfig.ServerUpdatePort)
        }, "TestResponderDescription",
        1024 * 1024 * 2, 1024 * 1024 * 2, 50,
        SocketConnectionAttributes.Reliable | SocketConnectionAttributes.TransportHeartBeat
    );

    private readonly Dictionary<uint, IMessageSerializer> serializers = new()
    {
        { 2345, new SimpleVersionedMessage.SimpleSerializer() }, { 159, new SimpleVersionedMessage.SimpleSerializer() }
    };

    private Dictionary<uint, IMessageDeserializer> requesterDeserializers = null!;
    private SimpleVersionedMessage requesterReceivedResponseMessage = null!;
    private Dictionary<uint, IMessageDeserializer> responderDeserializers = null!;

    private SimpleVersionedMessage responderReceivedMessage = null!;

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
        var responderStreamDecoderFactory
            = new SimpleMessageStreamDecoder.SimpleDeserializerFactory(responderDeserializers);
        var responderSerdesFactory = new SerdesFactory(responderStreamDecoderFactory
            , new SocketStreamMessageEncoderFactory(serializers));
        // create server
        var tcpResponderBuilder = new TcpConversationResponderBuilder();
        tcpResponder = tcpResponderBuilder.Build(responderTopicConConfig, responderSerdesFactory);

        requesterDeserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
            , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        var requesterStreamDecoderFactory
            = new SimpleMessageStreamDecoder.SimpleDeserializerFactory(requesterDeserializers);
        var requesterSerdesFactory = new SerdesFactory(requesterStreamDecoderFactory
            , new SocketStreamMessageEncoderFactory(serializers));
        // create client
        var tcpRequesterBuilder = new TcpConversationRequesterBuilder();
        var requesterTopicConConfig = responderTopicConConfig.Clone();
        requesterTopicConConfig.ConversationProtocol = SocketConversationProtocol.TcpClient;
        tcpRequester = tcpRequesterBuilder.Build(requesterTopicConConfig, requesterSerdesFactory);

        v2Message = new SimpleVersionedMessage { Version = 2, PayLoad2 = 234567.0, MessageId = 2345 };
    }

    [TestCleanup]
    public void TearDown()
    {
        tcpRequester.Stop();
        tcpResponder.Stop();
    }

    [TestMethod]
    public void ClientSendMessageDecodesCorrectlyOnServer()
    {
        // client connects
        tcpResponder.Start();
        tcpRequester.Start();

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 responderDeserializers.Values.Cast<ICallbackMessageDeserializer<SimpleVersionedMessage>>())
        {
            deserializersValue.Deserialized2 += ReceivedFromClientDeserializerCallback;
            deserializersValue.MessageDeserialized += ReceivedFromClientDeserializerCallback;
        }

        var v1Message = new SimpleVersionedMessage { Version = 1, PayLoad = 765432, MessageId = 159 };
        // send message
        tcpRequester.ConversationPublisher!.Send(v1Message);

        Thread.Sleep(20);
        // assert server receives properly
        Assert.AreEqual(v1Message.PayLoad, responderReceivedMessage.PayLoad);
        Assert.AreEqual(v1Message.MessageId, responderReceivedMessage.MessageId);
        Assert.AreEqual(v1Message.Version, responderReceivedMessage.Version);
    }

    [TestMethod]
    public void ClientSendMessageServerRespondsDecodesCorrectlyOnClient()
    {
        // client connects
        tcpResponder.Start();
        tcpRequester.Start();

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 responderDeserializers.Values.Cast<ICallbackMessageDeserializer<SimpleVersionedMessage>>())
            deserializersValue.Deserialized2 += RespondToClientMessage;

        // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
        foreach (var deserializersValue in
                 requesterDeserializers.Values.Cast<ICallbackMessageDeserializer<SimpleVersionedMessage>>())
            deserializersValue.Deserialized2 += ReceivedFromResponderDeserializerCallback;

        var v1Message = new SimpleVersionedMessage { Version = 1, PayLoad = 765432, MessageId = 159 };
        // send message
        tcpRequester.ConversationPublisher!.Send(v1Message);

        Thread.Sleep(300);
        // assert server receives properly
        Assert.AreEqual(v2Message.PayLoad2, requesterReceivedResponseMessage.PayLoad2);
        Assert.AreEqual(v2Message.MessageId, requesterReceivedResponseMessage.MessageId);
        Assert.AreEqual(v2Message.Version, requesterReceivedResponseMessage.Version);
    }

    private void RespondToClientMessage(SimpleVersionedMessage msg, object? header, IConversation? client)
    {
        responderReceivedMessage = msg;
        if (client is IConversationRequester conversationRequester)
            conversationRequester.ConversationPublisher!.Send(v2Message);
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, object? header
        , IConversation? client)
    {
        responderReceivedMessage = msg;
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, BasicMessageHeader msgHeader)
    {
        responderReceivedMessage = msg;
    }

    private void ReceivedFromResponderDeserializerCallback(SimpleVersionedMessage msg, object? header
        , IConversation? client)
    {
        requesterReceivedResponseMessage = msg;
    }
}
