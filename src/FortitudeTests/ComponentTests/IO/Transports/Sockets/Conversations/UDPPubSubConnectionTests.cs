#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
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
public class UdpPubSubConnectionTests
{
    private readonly Dictionary<uint, IMessageSerializer> serializers = new()
    {
        { 2345, new SimpleVersionedMessage.SimpleSerializer() }, { 159, new SimpleVersionedMessage.SimpleSerializer() }
    };

    private readonly NetworkTopicConnectionConfig udpPublisherTopicConConfig = new("TestInstanceName"
        , SocketConversationProtocol.UdpPublisher,
        new List<IEndpointConfig>(new List<IEndpointConfig>
        {
            new EndpointConfig(TestMachineConfig.LoopBackIpAddress, TestMachineConfig.ServerUpdatePort, CountryCityCodes.AUinMEL
                , subnetMask: TestMachineConfig.NetworkSubAddress)
        }), "TestTCPReqRespConn", 1024 * 1024 * 2, 1024 * 1024 * 2, 50,
        SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast);

    private AutoResetEvent autoReset = new(false);

    private ConversationPublisher conversationPublisher = null!;

    private ConversationSubscriber conversationSubscriber = null!;

    private Dictionary<uint, IMessageDeserializer> deserializers = null!;

    private SimpleVersionedMessage receivedSimpleVersionedMessage = null!;

    [TestInitialize]
    public void Setup()
    {
        deserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
            , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        var streamDecoderFactory = new SimpleMessageStreamDecoder.SimpleDeserializerMessageDeserializationFactory(deserializers);
        var streamEncoderFactory = new SimpleMessageStreamDecoder.SimpleSerializerFactory(serializers);
        var serdesFactory = new MessageSerdesRepositoryFactory(streamEncoderFactory, streamDecoderFactory, streamDecoderFactory);
        // create server
        var udpPublisherBuilder = new UdpConversationPublisherBuilder();
        conversationPublisher = udpPublisherBuilder.Build(udpPublisherTopicConConfig, serdesFactory);

        // create client
        var udpSubscriberBuilder = new UdpConversationSubscriberBuilder();
        var udpSubscriberTopicConConfig = udpPublisherTopicConConfig.Clone();
        udpSubscriberTopicConConfig.ConversationProtocol = SocketConversationProtocol.UdpSubscriber;
        conversationSubscriber = udpSubscriberBuilder.Build(udpSubscriberTopicConConfig, serdesFactory);
    }

    [TestCleanup]
    public void TearDown()
    {
        conversationSubscriber.Stop(CloseReason.Completed, "Test closing down");
        conversationPublisher.Stop(CloseReason.Completed, "Test closing down");
        // FLoggerFactory.WaitUntilDrained();
    }

    [TestMethod]
    public void ClientSendMessageDecodesCorrectlyOnServer()
    {
        // client connects
        conversationPublisher.Start();
        Thread.Sleep(20);
        conversationSubscriber.Start();

        foreach (INotifyingMessageDeserializer<SimpleVersionedMessage> deserializersValue in
                 deserializers.Values)
            deserializersValue.ConversationMessageDeserialized += ReceivedFromClientDeserializerCallback;

        var v2Message = new SimpleVersionedMessage { Version = 2, Payload2 = 345678.0, MessageId = 2345 };
        // send message
        conversationPublisher.StreamPublisher!.Send(v2Message);

        autoReset.WaitOne(50);
        // assert server receives properly
        Assert.AreEqual(v2Message.Payload2, receivedSimpleVersionedMessage.Payload2);
        Assert.AreEqual(v2Message.MessageId, receivedSimpleVersionedMessage.MessageId);
        Assert.AreEqual(v2Message.Version, receivedSimpleVersionedMessage.Version);
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, MessageHeader header, IConversation selfSession)
    {
        receivedSimpleVersionedMessage = msg;
        autoReset.Set();
    }
}
