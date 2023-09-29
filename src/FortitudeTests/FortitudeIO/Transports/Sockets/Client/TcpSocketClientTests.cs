using System.Net;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Client;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Client
{
    [TestClass]
    public class TcpSocketClientTests
    {
        private readonly int wholeMessagesPerReceive = 20;
        private DummyTcpSocketClient dummyTcpSocketClient;
        private Mock<IBinaryStreamPublisher> moqBinStreamPublisher;
        private Mock<IBinaryDeserializationFactory> moqBinUnserialFac;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IFLogger> moqFLogger;
        private Mock<IMap<uint, IBinaryDeserializer>> moqSerialzierCache;
        private Mock<IOSSocket> moqSocket;

        private int recvBufferSize;
        private string sessionDescription;

        private string expectedHost;
        private int expectedPort;
        private byte[] expectedIpAddress;

        [TestInitialize]
        public void SetUp()
        {
            moqFLogger = new Mock<IFLogger>();
            moqDispatcher = new Mock<ISocketDispatcher>();
            sessionDescription = "testSessionDescription";
            moqBinStreamPublisher = new Mock<IBinaryStreamPublisher>();
            moqBinUnserialFac = new Mock<IBinaryDeserializationFactory>();
            moqSocket = new Mock<IOSSocket>();
            moqSocket.SetupAllProperties();
            moqSerialzierCache = new Mock<IMap<uint, IBinaryDeserializer>>();
            moqServerConnectionConfig = new Mock<IConnectionConfig>();
            moqNetworkingController = new Mock<IOSNetworkingController>();

            recvBufferSize = 1234567;

            dummyTcpSocketClient = new DummyTcpSocketClient(moqFLogger.Object,
                moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
                sessionDescription, wholeMessagesPerReceive, moqSerialzierCache.Object, true, 
                moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);

            expectedHost = "TestHostname";
            expectedPort = 1979;
            expectedIpAddress = new byte[] {61, 26, 5, 6};

            moqFLogger.Setup(fl => fl.Info("Attempting TCP connection to {0} on {1}:{2}", 
                sessionDescription, expectedHost, expectedPort)).Verifiable();
            moqNetworkingController.Setup(nc => nc.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp)).Returns(moqSocket.Object).Verifiable();
            moqNetworkingController.Setup(nc => nc.GetIpAddress(expectedHost)).Returns(
                new IPAddress(expectedIpAddress)).Verifiable();
            moqSocket.SetupSet(oss => oss.NoDelay = true).Verifiable();
            moqSocket.Setup(oss => oss.Connect(It.IsAny<IPEndPoint>())).Verifiable();
        }

        [TestMethod]
        public void KeepAlive_CreateAndConnect_ReturnsTCPOSSocket()
        {
            moqSocket.Setup(oss => oss.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1))
                .Verifiable();

            var expectedKeepAliveValues = new KeepAlive
            {
                OnOff = 1,
                KeepAliveTimeMillisecs = 14_000,
                KeepAliveIntervalMillisecs = 2_000
            };
            var expectedKeepAliveByteValues = expectedKeepAliveValues.ToByteArray();

            moqSocket.Setup(oss => oss.IOControl(IOControlCode.KeepAliveValues, expectedKeepAliveByteValues, null))
                .Returns(0).Verifiable();
            
            var receivedSocket = dummyTcpSocketClient.CallCreateAndConnect(expectedHost, expectedPort);

            Assert.AreSame(moqSocket.Object, receivedSocket);

            moqFLogger.Verify();
            moqNetworkingController.Verify();
            moqSocket.Verify();
        }

        [TestMethod]
        public void NoKeepAlive_CreateAndConnect_ReturnsTCPOSSocket()
        {
            dummyTcpSocketClient = new DummyTcpSocketClient(moqFLogger.Object,
                moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
                sessionDescription, wholeMessagesPerReceive, moqSerialzierCache.Object, false,
                moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);
            var receivedSocket = dummyTcpSocketClient.CallCreateAndConnect(expectedHost, expectedPort);

            Assert.AreSame(moqSocket.Object, receivedSocket);

            moqFLogger.Verify();
            moqNetworkingController.Verify();
            moqSocket.Verify();

            var expectedKeepAliveValues = new KeepAlive
            {
                OnOff = 1,
                KeepAliveTimeMillisecs = 14_000,
                KeepAliveIntervalMillisecs = 2_000
            };
            var expectedKeepAliveByteValues = expectedKeepAliveValues.ToByteArray();
            moqSocket.Verify(oss => oss.IOControl(IOControlCode.KeepAliveValues, expectedKeepAliveByteValues, null), 
                Times.Never());
            moqSocket.Verify(oss => oss.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1),
                Times.Never);
        }

        internal class DummyTcpSocketClient: TcpSocketClient
        {
            private readonly IBinaryDeserializationFactory binaryDeserializationFactory;

            public DummyTcpSocketClient(IFLogger logger, ISocketDispatcher dispatcher,
                IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
                string sessionDescription,
                int wholeMessagesPerReceive, IMap<uint, IBinaryDeserializer> serializerCache, bool keepalive,
                IBinaryDeserializationFactory binaryDeserializationFactory, int recvBuffrSize,
                IBinaryStreamPublisher streamToPublisher)
                : base(logger, dispatcher, networkingController, connectionConfig, sessionDescription, 
                      wholeMessagesPerReceive, serializerCache, keepalive)
            {
                RecvBufferSize = recvBuffrSize;
                StreamToPublisher = streamToPublisher;
                this.binaryDeserializationFactory = binaryDeserializationFactory;
            }

            public override int RecvBufferSize { get; }

            public override IBinaryStreamPublisher StreamToPublisher { get; }
            protected override IBinaryDeserializationFactory GetFactory()
            {
                return binaryDeserializationFactory;
            }

            public IOSSocket CallCreateAndConnect(string host, int port)
            {
                return CreateAndConnect(host, port);
            }

            public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers)
            {
                return null;
            }
        }
    }
}