using System.Collections.Generic;
using System.Threading;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Conversations.Builders;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.Sockets;
using FortitudeTests.FortitudeCommon.Types;
using FortitudeTests.FortitudeIO.Protocols.Serialization;
using FortitudeTests.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations
{
    [TestClass, NoMatchingProductionClass]
    public class TCPRequestResponderConnectionTests
    {
        private Dictionary<uint, IBinaryDeserializer> responderDeserializers;
        private Dictionary<uint, IBinaryDeserializer> requesterDeserializers;

        readonly Dictionary<uint, IBinarySerializer> serializers = new Dictionary<uint, IBinarySerializer>
        {
            {2345, new SimpleVersionedMessage.SimpleSerializer()},
            {159, new SimpleVersionedMessage.SimpleSerializer()}
        };

        private SocketConnectionConfig serverSocketConfig = new SocketConnectionConfig( "TestInstanceName",
            SocketConnectionAttributes.Reliable | SocketConnectionAttributes.TransportHeartBeat,
            "TestTCPReqRespConn", 1024 * 1024 * 2, 1024 * 1024 * 2,
            TestMachineConfig.LoopBackIpAddress, null, false,
            (ushort)TestMachineConfig.ServerUpdatePort, (ushort)TestMachineConfig.ServerUpdatePort);

        private RequestResponseResponder reqRespResponder;
        private RequestResponseRequester reqRespRequester;

        private SimpleVersionedMessage responderReceivedMessage;
        private SimpleVersionedMessage requesterReceivedResponseMessage;
        private SimpleVersionedMessage v2Message;

        [TestInitialize]
        public void Setup()
        {
            responderDeserializers = new Dictionary<uint, IBinaryDeserializer>
            {
                {2345, new SimpleVersionedMessage.SimpleDeserializer()},
                {159, new SimpleVersionedMessage.SimpleDeserializer()}
            };
            var responderStreamDecoderFactory = new SimpleStreamDecoder.SimpleDeserializerFactory(responderDeserializers);
            var responderSerdesFactory = new SerdesFactory(responderStreamDecoderFactory, serializers);
            // create server
            var tcpReqRespResponderBuilder = new TCPRequestResponseResponderBuilder();
            reqRespResponder = tcpReqRespResponderBuilder.Build(serverSocketConfig, responderSerdesFactory);

            requesterDeserializers = new Dictionary<uint, IBinaryDeserializer>
            {
                {2345, new SimpleVersionedMessage.SimpleDeserializer()},
                {159, new SimpleVersionedMessage.SimpleDeserializer()}
            };
            var requesterStreamDecoderFactory = new SimpleStreamDecoder.SimpleDeserializerFactory(requesterDeserializers);
            var requesterSerdesFactory = new SerdesFactory(requesterStreamDecoderFactory, serializers);
            // create client
            var tcpReqRespRequestorBuilder = new TCPRequestResponseRequesterBuilder();
            reqRespRequester = tcpReqRespRequestorBuilder.Build(serverSocketConfig, requesterSerdesFactory);
            
            v2Message = new SimpleVersionedMessage { Version = 2, PayLoad2 = 234567.0, MessageId = 2345 };
        }

        [TestCleanup]
        public void TearDown()
        {
            reqRespRequester.Stop();
            reqRespResponder.Stop();
        }

        [TestMethod]
        public void ClientSendMessageDecodesCorrectlyOnServer()
        {
            // client connects
            reqRespResponder.Start();
            reqRespRequester.Start();

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (ICallbackBinaryDeserializer<SimpleVersionedMessage> deserializersValue in responderDeserializers.Values)
            {
                deserializersValue.Deserialized2 += ReceivedFromClientDeserializerCallback;
            }

            var v1Message = new SimpleVersionedMessage {Version = 1, PayLoad = 765432, MessageId = 159};
            // send message
            reqRespRequester.ConversationPublisher.Send(v1Message);

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
            reqRespResponder.Start();
            reqRespRequester.Start();

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (ICallbackBinaryDeserializer<SimpleVersionedMessage> deserializersValue in responderDeserializers.Values)
            {
                deserializersValue.Deserialized2 += RespondToClientMessage;
            }

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (ICallbackBinaryDeserializer<SimpleVersionedMessage> deserializersValue in requesterDeserializers.Values)
            {
                deserializersValue.Deserialized2 += ReceivedFromResponderDeserializerCallback;
            }

            var v1Message = new SimpleVersionedMessage { Version = 1, PayLoad = 765432, MessageId = 159 };
            // send message
            reqRespRequester.ConversationPublisher.Send(v1Message);

            Thread.Sleep(300);
            // assert server receives properly
            Assert.AreEqual(v2Message.PayLoad2, requesterReceivedResponseMessage.PayLoad2);
            Assert.AreEqual(v2Message.MessageId, requesterReceivedResponseMessage.MessageId);
            Assert.AreEqual(v2Message.Version, requesterReceivedResponseMessage.Version);
        }

        private void RespondToClientMessage(SimpleVersionedMessage msg, object header, ISocketConversation client)
        {
            responderReceivedMessage = msg;
            client.ConversationPublisher.Send(v2Message);
        }

        private void ReceivedFromClientDeserializerCallback(SimpleVersionedMessage msg, object header, ISocketConversation client)
        {
            responderReceivedMessage = msg;
        }

        private void ReceivedFromResponderDeserializerCallback(SimpleVersionedMessage msg, object header, ISocketConversation client)
        {
            requesterReceivedResponseMessage = msg;
        }
    }
}
