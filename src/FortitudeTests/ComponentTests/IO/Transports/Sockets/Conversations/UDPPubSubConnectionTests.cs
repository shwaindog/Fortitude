#region

using System.Net;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
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
public class UDPPubSubConnectionTests
{
    private readonly Dictionary<uint, IMessageSerializer> serializers = new()
    {
        { 2345, new SimpleVersionedMessage.SimpleSerializer() }, { 159, new SimpleVersionedMessage.SimpleSerializer() }
    };

    private Dictionary<uint, IMessageDeserializer> deserializers = null!;

    private PublisherConversation publisherConversation = null!;

    private SimpleVersionedMessage recevedSimpleVersionedMessage = null!;

    private SocketConnectionConfig serverSocketConfig = new("TestInstanceName", "TestTCPReqRespConn",
        SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast,
        1024 * 1024 * 2, 1024 * 1024 * 2,
        TestMachineConfig.LoopBackIpAddress, IPAddress.Parse(TestMachineConfig.NetworkSubAddress), false,
        (ushort)TestMachineConfig.ServerUpdatePort, (ushort)TestMachineConfig.ServerUpdatePort);

    private SubscriberConversation subscriberConversation = null!;

    [TestInitialize]
    public void Setup()
    {
        deserializers = new Dictionary<uint, IMessageDeserializer>
        {
            { 2345, new SimpleVersionedMessage.SimpleDeserializer() }
            , { 159, new SimpleVersionedMessage.SimpleDeserializer() }
        };
        var streamDecoderFactory = new SimpleMessageStreamDecoder.SimpleDeserializerFactory(deserializers);
        var serdesFactory = new SerdesFactory(streamDecoderFactory, serializers);
        // create server
        var udpPublisherBuilder = new UDPPublisherBuilder();
        publisherConversation = udpPublisherBuilder.Build(serverSocketConfig, serdesFactory);

        // create client
        var udpSubscriberBuilder = new UDPSubscriberBuilder();
        subscriberConversation = udpSubscriberBuilder.Build(serverSocketConfig, serdesFactory);
    }

    [TestCleanup]
    public void TearDown()
    {
        subscriberConversation.Disconnect();
        publisherConversation.Disconnect();
    }

    [TestMethod]
    public void ClientSendMessageDecodesCorrectlyOnServer()
    {
        // client connects
        publisherConversation.Connect();
        subscriberConversation.Connect();

        foreach (ICallbackMessageDeserializer<SimpleVersionedMessage> deserializersValue in
                 deserializers.Values)
            deserializersValue.Deserialized2 += ReceivedFromClientDeserializerCallback;

        var v2Message = new SimpleVersionedMessage { Version = 2, PayLoad2 = 345678.0, MessageId = 2345 };
        // send message
        publisherConversation.ConversationPublisher!.Send(v2Message);

        Thread.Sleep(20);
        // assert server receives properly
        Assert.AreEqual(v2Message.PayLoad2, recevedSimpleVersionedMessage.PayLoad2);
        Assert.AreEqual(v2Message.MessageId, recevedSimpleVersionedMessage.MessageId);
        Assert.AreEqual(v2Message.Version, recevedSimpleVersionedMessage.Version);
    }

    private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, object? header
        , ISocketConversation? selfSession)
    {
        recevedSimpleVersionedMessage = msg;
    }
}
