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
public class UdpPubSubConnectionTests
{
    private readonly Dictionary<uint, IMessageSerializer> serializers = new()
    {
        { 2345, new SimpleVersionedMessage.SimpleSerializer() }, { 159, new SimpleVersionedMessage.SimpleSerializer() }
    };

    private readonly SocketConnectionConfig serverSocketConfig = new("TestInstanceName", "TestTCPReqRespConn",
        SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast,
        1024 * 1024 * 2, 1024 * 1024 * 2,
        TestMachineConfig.LoopBackIpAddress, TestMachineConfig.NetworkSubAddress, false,
        TestMachineConfig.ServerUpdatePort, TestMachineConfig.ServerUpdatePort);

    private ConversationPublisher conversationPublisher = null!;

    private ConversationSubscriber conversationSubscriber = null!;

    private Dictionary<uint, IMessageDeserializer> deserializers = null!;

    private SimpleVersionedMessage recevedSimpleVersionedMessage = null!;

    [TestInitialize]
    public void Setup()
    {
        deserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
            , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        var streamDecoderFactory = new SimpleMessageStreamDecoder.SimpleDeserializerFactory(deserializers);
        var serdesFactory = new SerdesFactory(streamDecoderFactory, new SocketStreamMessageEncoderFactory(serializers));
        // create server
        var udpPublisherBuilder = new UdpConversationPublisherBuilder();
        conversationPublisher = udpPublisherBuilder.Build(serverSocketConfig, serdesFactory);

        // create client
        var udpSubscriberBuilder = new UdpConversationSubscriberBuilder();
        conversationSubscriber = udpSubscriberBuilder.Build(serverSocketConfig, serdesFactory);
    }

    [TestCleanup]
    public void TearDown()
    {
        conversationSubscriber.Disconnect();
        conversationPublisher.Disconnect();
    }

    [TestMethod]
    public void ClientSendMessageDecodesCorrectlyOnServer()
    {
        // client connects
        conversationPublisher.Connect();
        conversationSubscriber.Connect();

        foreach (ICallbackMessageDeserializer<SimpleVersionedMessage> deserializersValue in
                 deserializers.Values)
            deserializersValue.Deserialized2 += ReceivedFromClientDeserializerCallback;

        var v2Message = new SimpleVersionedMessage { Version = 2, PayLoad2 = 345678.0, MessageId = 2345 };
        // send message
        conversationPublisher.ConversationPublisher!.Send(v2Message);

        Thread.Sleep(20);
        // assert server receives properly
        Assert.AreEqual(v2Message.PayLoad2, recevedSimpleVersionedMessage.PayLoad2);
        Assert.AreEqual(v2Message.MessageId, recevedSimpleVersionedMessage.MessageId);
        Assert.AreEqual(v2Message.Version, recevedSimpleVersionedMessage.Version);
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, object? header
        , IConversation? selfSession)
    {
        recevedSimpleVersionedMessage = msg;
    }
}
