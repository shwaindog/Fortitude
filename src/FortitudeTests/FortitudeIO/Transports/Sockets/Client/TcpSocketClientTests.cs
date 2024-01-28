#region

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Client;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Client;

[TestClass]
public class TcpSocketClientTests
{
    private readonly int wholeMessagesPerReceive = 20;
    private DummyTcpSocketClient dummyTcpSocketClient = null!;

    private string expectedHost = null!;
    private byte[] expectedIpAddress = null!;
    private int expectedPort;
    private Mock<IBinaryStreamPublisher> moqBinStreamPublisher = null!;
    private Mock<IMessageIdDeserializationRepository> moqBinUnserialFac = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerialzierCache = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<IOSSocket> moqSocket = null!;

    private int recvBufferSize;
    private string sessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFLogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        sessionDescription = "testSessionDescription";
        moqBinStreamPublisher = new Mock<IBinaryStreamPublisher>();
        moqBinUnserialFac = new Mock<IMessageIdDeserializationRepository>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqSerialzierCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqNetworkingController = new Mock<IOSNetworkingController>();

        recvBufferSize = 1234567;

        dummyTcpSocketClient = new DummyTcpSocketClient(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, wholeMessagesPerReceive, moqSerialzierCache.Object, true,
            moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);

        expectedHost = "TestHostname";
        expectedPort = 1979;
        expectedIpAddress = new byte[] { 61, 26, 5, 6 };

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
            OnOff = 1, KeepAliveTimeMillisecs = 14_000, KeepAliveIntervalMillisecs = 2_000
        };
        var expectedKeepAliveByteValues = expectedKeepAliveValues.ToByteArray();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
#pragma warning disable CA1416
            moqSocket.Setup(oss => oss.IOControl(IOControlCode.KeepAliveValues, expectedKeepAliveByteValues, null))
                .Returns(0).Verifiable();
#pragma warning restore CA1416
        }

        var receivedSocket = dummyTcpSocketClient.CallCreateAndConnect(expectedHost, expectedPort);

        Assert.AreSame(moqSocket.Object, receivedSocket);

        moqFLogger.Verify();
        moqNetworkingController.Verify();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) moqSocket.Verify();
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
            OnOff = 1, KeepAliveTimeMillisecs = 14_000, KeepAliveIntervalMillisecs = 2_000
        };
        var expectedKeepAliveByteValues = expectedKeepAliveValues.ToByteArray();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
#pragma warning disable CA1416
            moqSocket.Verify(oss => oss.IOControl(IOControlCode.KeepAliveValues, expectedKeepAliveByteValues, null),
                Times.Never());
#pragma warning restore CA1416
        }

        moqSocket.Verify(oss => oss.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1),
            Times.Never);
    }

    internal class DummyTcpSocketClient : TcpSocketClient
    {
        private readonly IMessageIdDeserializationRepository messageIdDeserializationRepository;

        public DummyTcpSocketClient(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string sessionDescription,
            int wholeMessagesPerReceive, IMap<uint, IMessageDeserializer> serializerCache, bool keepalive,
            IMessageIdDeserializationRepository messageIdDeserializationRepository, int recvBuffrSize,
            IBinaryStreamPublisher streamToPublisher)
            : base(logger, dispatcher, networkingController, connectionConfig, sessionDescription,
                wholeMessagesPerReceive, serializerCache, keepalive)
        {
            RecvBufferSize = recvBuffrSize;
            StreamToPublisher = streamToPublisher;
            this.messageIdDeserializationRepository = messageIdDeserializationRepository;
        }

        public override int RecvBufferSize { get; }

        public override IBinaryStreamPublisher StreamToPublisher { get; }

        protected override IMessageIdDeserializationRepository GetFactory() => messageIdDeserializationRepository;

        public IOSSocket CallCreateAndConnect(string host, int port) => CreateAndConnect(host, port);

        public override IMessageStreamDecoder?
            GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers) =>
            null;
    }
}
