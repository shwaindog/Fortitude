#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeTests.TestHelpers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Publishing;

[TestClass]
public class UdpPublisherTests
{
    private static Mock<IPInterfaceProperties> moqNoMulticastAddressIPProps = null!;
    private static Mock<MulticastIPAddressInformationCollection> moqNoMulticastAddressMulticastAddress = null!;
    private static List<MulticastIPAddressInformation> noMulticastAddressMultiCastAddressResults = null!;
    private static Mock<IPInterfaceProperties> moqNoSupportsMulticastIPProps = null!;
    private static Mock<MulticastIPAddressInformationCollection> moqNoSupportsMulticastMulticastAddress = null!;
    private static List<MulticastIPAddressInformation> noSupportsMulticastMultiCastAddressResults = null!;
    private static Mock<IPInterfaceProperties> moqDownStatusIPProps = null!;
    private static Mock<MulticastIPAddressInformationCollection> moqDownStatusMulticastAddress = null!;
    private static List<MulticastIPAddressInformation> downStatusMultiCastAddressResults = null!;
    private static Mock<IPInterfaceProperties> moqAllGoodIPProps = null!;
    private static Mock<MulticastIPAddressInformationCollection> moqAllGoodMulticastAddress = null!;
    private static List<MulticastIPAddressInformation> allGoodMultiCastAddressResults = null!;
    private static Mock<NetworkInterface> moqNoMulticastAddressNic = null!;
    private static Mock<NetworkInterface> moqNoSupportsMulticastNic = null!;
    private static Mock<NetworkInterface> moqDownStatusNic = null!;
    private static Mock<NetworkInterface> moqAllGoodNic = null!;
    private DummyUdpPublisher dummyUdpPublisher = null!;
    private string hostname = null!;
    private Mock<IBinaryDeserializationFactory> moqBinaryDeserializationFactory = null!;
    private Mock<IBinarySerializationFactory> moqBinSerialFac = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IStreamDecoder> moqFeedDecoder = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private string multicaseInterface = null!;
    private IPAddress multiCastIPAddress = null!;
    private int port;
    private string receivedMulticastLookupRequest = null!;
    private string sessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFLogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        sessionDescription = "testSessionDescription";
        multicaseInterface = "testMulticastInterface";
        hostname = "testHostName";
        multiCastIPAddress = IPAddress.Parse("192.168.1.50");
        moqBinSerialFac = new Mock<IBinarySerializationFactory>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqFeedDecoder = new Mock<IStreamDecoder>();
        moqBinaryDeserializationFactory = new Mock<IBinaryDeserializationFactory>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>()))
            .Callback<string>(requestedResolve => { receivedMulticastLookupRequest = requestedResolve; })
            .Returns(multiCastIPAddress).Verifiable();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();

        port = 34567;
        dummyUdpPublisher = new DummyUdpPublisher(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, multicaseInterface, moqBinSerialFac.Object, moqFeedDecoder.Object,
            moqBinaryDeserializationFactory.Object);

        moqFLogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }


    [TestMethod]
    public void ExplicitMultiCast_NewUdpPublisher_ResolvesIPFromExplicitAddress()
    {
        Assert.AreEqual(multicaseInterface, receivedMulticastLookupRequest);

        moqNetworkingController.Verify();
    }

    [TestMethod]
    public void NoMultiCastHost_NewUdpPublisher_ResolvesIPFromHostName()
    {
        dummyUdpPublisher = new DummyUdpPublisher(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, null, moqBinSerialFac.Object, moqFeedDecoder.Object,
            moqBinaryDeserializationFactory.Object);
        Assert.AreEqual(Dns.GetHostName(), receivedMulticastLookupRequest);
        moqNetworkingController.Verify();
    }

    [TestMethod]
    public void BadMultiCastHost_NewUdpPublisher_ResolvesIPFromHostName()
    {
        var badMulticastHostName = "testBadMulticastHostname";

        moqNetworkingController.Setup(nc => nc.GetIpAddress(badMulticastHostName)).Throws<Exception>().Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(Dns.GetHostName()))
            .Callback<string>(requestedResolve => { receivedMulticastLookupRequest = requestedResolve; })
            .Returns(multiCastIPAddress).Verifiable();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();

        dummyUdpPublisher = new DummyUdpPublisher(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, badMulticastHostName, moqBinSerialFac.Object, moqFeedDecoder.Object,
            moqBinaryDeserializationFactory.Object);
        Assert.AreEqual(Dns.GetHostName(), receivedMulticastLookupRequest);
        moqNetworkingController.Verify();
    }

    [TestMethod]
    public void StreamFromSubscriber_CreateAndConnect_FindsNicBroadcastIpCreatesSocket()
    {
        var streamFromSubscriber = (DummyUdpPublisher.DummyUdpSubscriber)dummyUdpPublisher
            .SocketStreamFromSubscriber;

        moqNetworkingController.Setup(nc => nc.GetIpAddress(hostname)).Returns(multiCastIPAddress).Verifiable();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(
                AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            .Returns(moqSocket.Object).Verifiable();

        var allNics = GetNetworkInterfaces();

        moqSocket.SetupSet(s => s.ExclusiveAddressUse = false).Verifiable();
        moqSocket.Setup(s => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true))
            .Verifiable();

        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(allNics).Verifiable();

        moqSocket.Setup(s => s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface,
            IPAddress.HostToNetworkOrder(3))).Verifiable();
        moqSocket.SetupSet(s => s.Ttl = 200).Verifiable();
        moqSocket.Setup(s => s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 200))
            .Verifiable();
        moqSocket.SetupSet(s => s.EnableBroadcast = true).Verifiable();
        moqSocket.SetupSet(s => s.MulticastLoopback = false).Verifiable();
        moqSocket.Setup(s => s.Connect(new IPEndPoint(multiCastIPAddress, port)))
            .Verifiable();

        var fromSocket = streamFromSubscriber.UdpSubscriberCreateAndConnect(hostname, port);

        Assert.IsNotNull(fromSocket);
        moqSocket.Verify();
        moqNetworkingController.Verify();
        moqNoMulticastAddressMulticastAddress.Verify();
        moqNoSupportsMulticastNic.Verify();
        moqDownStatusNic.Verify();
        moqAllGoodNic.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void StreamFromSubscriber_CreateAndConnect_FindsNoNicBroadcastThrowsException()
    {
        var streamFromSubscriber = (DummyUdpPublisher.DummyUdpSubscriber)dummyUdpPublisher
            .SocketStreamFromSubscriber;

        moqNetworkingController.Setup(nc => nc.GetIpAddress(hostname)).Returns(multiCastIPAddress).Verifiable();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(
                AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            .Returns(moqSocket.Object).Verifiable();

        var allNics = GetNetworkInterfaces();
        var mockIpv4Props = new Mock<IPv4InterfaceProperties>();
        moqAllGoodNic.Reset();
        moqAllGoodNic.SetupGet(nic => nic.SupportsMulticast).Returns(false).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(allNics).Verifiable();

        streamFromSubscriber.UdpSubscriberCreateAndConnect(hostname, port);

        Assert.Fail("Should get expected exception");
    }

    [TestMethod]
    public void StreamFromSubscriber_ConnectSetsConnector_UpdatesPublisherAcceptor()
    {
        var streamFromSubscriber = (DummyUdpPublisher.DummyUdpSubscriber)dummyUdpPublisher
            .SocketStreamFromSubscriber;
        var moqNewSocketSessionConnect = new Mock<ISocketSessionConnection>();

        streamFromSubscriber.UdpSubscriberConnector = moqNewSocketSessionConnect.Object;

        Assert.AreSame(streamFromSubscriber.UdpSubscriberConnector, dummyUdpPublisher.UdpPublisherAcceptor);
    }

    [TestMethod]
    public void StreamFromSubscriber_UdpPublisher_RefersBackToReferencingPublisher()
    {
        var streamFromSubscriber = (DummyUdpPublisher.DummyUdpSubscriber)dummyUdpPublisher
            .SocketStreamFromSubscriber;

        Assert.AreSame(streamFromSubscriber.StreamToPublisher, dummyUdpPublisher);
    }

    private static NetworkInterface[] GetNetworkInterfaces()
    {
        moqNoMulticastAddressNic = BuildAllGoodNic(out moqNoMulticastAddressIPProps,
            out moqNoMulticastAddressMulticastAddress,
            out noMulticastAddressMultiCastAddressResults);
        noMulticastAddressMultiCastAddressResults.Clear();
        moqNoMulticastAddressMulticastAddress.SetupGet(nip => nip.Count).Returns(0);

        moqNoSupportsMulticastNic = BuildAllGoodNic(out moqNoSupportsMulticastIPProps,
            out moqNoSupportsMulticastMulticastAddress,
            out noSupportsMulticastMultiCastAddressResults);
        moqNoSupportsMulticastNic.Reset();
        moqNoSupportsMulticastNic.SetupGet(nic => nic.SupportsMulticast).Returns(false).Verifiable();

        moqDownStatusNic = BuildAllGoodNic(out moqDownStatusIPProps, out moqDownStatusMulticastAddress,
            out downStatusMultiCastAddressResults);
        moqDownStatusNic.SetupGet(nic => nic.OperationalStatus).Returns(OperationalStatus.Down);

        moqAllGoodNic = BuildAllGoodNic(out moqAllGoodIPProps, out moqAllGoodMulticastAddress,
            out allGoodMultiCastAddressResults);

        var allNics = new[]
        {
            moqNoMulticastAddressNic.Object, moqNoSupportsMulticastNic.Object, moqDownStatusNic.Object
            , moqAllGoodNic.Object
        };
        return allNics;
    }

    private static Mock<NetworkInterface> BuildAllGoodNic(out Mock<IPInterfaceProperties> moqNicIpProp,
        out Mock<MulticastIPAddressInformationCollection> moqMultiCastAddress,
        out List<MulticastIPAddressInformation> multicastAddresses
    )
    {
        var moqNic = new Mock<NetworkInterface>();
        moqNicIpProp = new Mock<IPInterfaceProperties>();
        moqNic.Setup(ni => ni.GetIPProperties())
            .Returns(moqNicIpProp.Object).Verifiable();
        moqMultiCastAddress = new Mock<MulticastIPAddressInformationCollection>();
        moqNicIpProp.SetupGet(nip => nip.MulticastAddresses).Returns(moqMultiCastAddress.Object);
        var moqMulticastAddressInfo = new Mock<MulticastIPAddressInformation>();
        multicastAddresses = new List<MulticastIPAddressInformation> { moqMulticastAddressInfo.Object };
        moqMultiCastAddress.SetupGet(nip => nip.Count).Returns(1);
        moqMultiCastAddress.Setup(mipaic => mipaic.GetEnumerator())
            .Returns(new DelayEnumerator<MulticastIPAddressInformation>(multicastAddresses));
        moqNic.SetupGet(nic => nic.SupportsMulticast).Returns(true);
        moqNic.SetupGet(nic => nic.OperationalStatus).Returns(OperationalStatus.Up);

        var moqIpv4Props = new Mock<IPv4InterfaceProperties>();
        moqNicIpProp.Setup(ipip => ipip.GetIPv4Properties()).Returns(moqIpv4Props.Object);
        moqIpv4Props.SetupGet(ipv4 => ipv4.Index).Returns(3);
        return moqNic;
    }


    private class DummyUdpPublisher : UdpPublisher
    {
        private readonly IBinarySerializationFactory binarySerializationFactory;
        private readonly IBinaryDeserializationFactory deserializationFactory;
        private readonly IStreamDecoder streamDecoder;

        public DummyUdpPublisher(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string sessionDescription, string? multicastInterface,
            IBinarySerializationFactory binarySerializationFactory, IStreamDecoder streamDecoder,
            IBinaryDeserializationFactory deserializationFactory)
            : base(logger, dispatcher, networkingController,
                connectionConfig, sessionDescription, multicastInterface)
        {
            this.binarySerializationFactory = binarySerializationFactory;
            this.streamDecoder = streamDecoder;
            this.deserializationFactory = deserializationFactory;
        }

        public override int SendBufferSize => 34567;

        public ISocketSessionConnection? UdpPublisherAcceptor => PublisherConnection;

        public override IBinarySerializationFactory GetFactory() => binarySerializationFactory;

        protected override UdpSubscriber BuildSubscriber(UdpPublisher publisher) =>
            new DummyUdpSubscriber(this, Logger, Dispatcher, NetworkingController, ConnectionConfig,
                SessionDescription, 1, null, streamDecoder, deserializationFactory);

        internal class DummyUdpSubscriber : UdpSubscriber
        {
            private readonly IBinaryDeserializationFactory deserializationFactory;
            private readonly IStreamDecoder streamDecoder;

            public DummyUdpSubscriber(UdpPublisher udpPublisher, IFLogger logger, ISocketDispatcher dispatcher,
                IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
                string sessionDescription, int wholeMessagesPerReceive,
                IMap<uint, IBinaryDeserializer>? serializerCache,
                IStreamDecoder streamDecoder, IBinaryDeserializationFactory deserializationFactory)
                : base(udpPublisher, logger, dispatcher, networkingController,
                    connectionConfig, sessionDescription, wholeMessagesPerReceive, serializerCache)
            {
                this.streamDecoder = streamDecoder;
                this.deserializationFactory = deserializationFactory;
            }

            public override int RecvBufferSize => 123456;

            public ISocketSessionConnection? UdpSubscriberConnector
            {
                get => Connector;
                set => Connector = value;
            }

            public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers) =>
                streamDecoder;

            protected override IBinaryDeserializationFactory GetFactory() => deserializationFactory;

            public IOSSocket UdpSubscriberCreateAndConnect(string host, int port) => CreateAndConnect(host, port);
        }
    }
}
