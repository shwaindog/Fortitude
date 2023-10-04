#region

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Subscription;

[TestClass]
[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public class UdpSubscriberTests
{
    private readonly int wholeMessagesPerReceive = 20;
    private DummyUdpSubscriber dummyTcpSocketClient = null!;
    private int expectedAdapterIpv4Index;

    private string expectedHost = null!;
    private IPAddress expectedIpAddress = null!;

    private byte[] expectedIpAddressNumber = null!;
    private IPEndPoint expectedIpEndPoint = null!;
    private int expectedPort;
    private Mock<IBinaryStreamPublisher> moqBinStreamPublisher = null!;
    private Mock<IBinaryDeserializationFactory> moqBinUnserialFac = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<NetworkInterface> moqNoInterNetworkUnicastAddressesInterface = null!;

    private Mock<NetworkInterface> moqNoMultiCastAddressInterface = null!;
    private Mock<NetworkInterface> moqNoSupportsMulticastInterface = null!;
    private Mock<NetworkInterface> moqNotUpInterface = null!;
    private Mock<IMap<uint, IBinaryDeserializer>> moqSerialzierCache = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<IOSSocket> moqSocket = null!;

    private Mock<IPInterfaceProperties> moqValidIPInterfaceProperties = null!;
    private Mock<IPv4InterfaceProperties> moqValidIpv4InterfaceProperties = null!;
    private Mock<NetworkInterface> moqValidMulticastInterface = null!;
    private string multicastInterface = null!;
    private MulticastIPAddressInformationCollection noValidMlticstAddrsInfoCllctn = null!;
    private UnicastIPAddressInformationCollection noValidUnicastAddressCollection = null!;

    private int recvBufferSize;

    private NetworkInterface[] returnedNetworkInterfaces = null!;
    private string sessionDescription = null!;

    private IPAddress successfulMulticaseIpAddress = null!;
    private IPAddress unSuccessfulMulticaseIpAddress = null!;

    private MulticastIPAddressInformationCollection validMlticstIpAddrsInfoCollctn = null!;

    private UnicastIPAddressInformationCollection validUnicastAddressCollection = null!;

    [TestInitialize]
    public void SetUp()
    {
        sessionDescription = "testSessionDescription";
        recvBufferSize = 1234567;
        multicastInterface = "239.0.0.222";
        expectedHost = "TestHostname";
        expectedPort = 1979;
        expectedIpAddressNumber = new byte[] { 04, 164, 30, 172 };
        expectedIpAddress = new IPAddress(expectedIpAddressNumber);
        expectedIpEndPoint = new IPEndPoint(expectedIpAddress, expectedPort);
        expectedAdapterIpv4Index = 2;

        successfulMulticaseIpAddress = new IPAddress(new byte[] { 239, 0, 0, 222 });
        unSuccessfulMulticaseIpAddress = expectedIpAddress;

        moqFLogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqBinStreamPublisher = new Mock<IBinaryStreamPublisher>();
        moqBinUnserialFac = new Mock<IBinaryDeserializationFactory>();
        moqSocket = new Mock<IOSSocket>();
        moqSocket.SetupAllProperties();
        moqSerialzierCache = new Mock<IMap<uint, IBinaryDeserializer>>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(multicastInterface))
            .Returns(successfulMulticaseIpAddress).Verifiable();

        moqFLogger.Setup(fl => fl.Info("Attempting UDPsub connection on {0} {1}:{2}={3}:{4}",
                sessionDescription, successfulMulticaseIpAddress, expectedHost, expectedIpAddress, expectedPort))
            .Verifiable();
        moqNetworkingController.Setup(nc => nc.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp)).Returns(moqSocket.Object).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(expectedHost)).Returns(
            expectedIpAddress).Verifiable();
        moqSocket.SetupSet(oss => oss.ExclusiveAddressUse = false).Verifiable();
        moqSocket.Setup(oss => oss.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true))
            .Verifiable();
        moqSocket.Setup(oss => oss.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true))
            .Verifiable();
        SetupNetworkInterfaces();
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(returnedNetworkInterfaces)
            .Verifiable();
        moqSocket.Setup(oss => oss.Bind(expectedIpEndPoint)).Verifiable();
        moqSocket.Setup(oss => oss.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            It.IsAny<MulticastOption>())).Verifiable();

        dummyTcpSocketClient = new DummyUdpSubscriber(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, multicastInterface, wholeMessagesPerReceive, moqSerialzierCache.Object,
            moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);
    }

    [TestMethod]
    public void NewUdpSubscriberWithValidMulticastAddress_CreateAndConnect_ReturnsSocketBoundToEndPoint()
    {
        var returnedSocket = dummyTcpSocketClient.CallCreateAndConnect(expectedHost, expectedPort);

        Assert.IsNotNull(returnedSocket);
        Assert.AreSame(moqSocket.Object, returnedSocket);
        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqSocket.Verify();
        moqNoMultiCastAddressInterface.Verify();
        moqNoSupportsMulticastInterface.Verify();
        moqNotUpInterface.Verify();
        moqNoInterNetworkUnicastAddressesInterface.Verify();
        moqValidMulticastInterface.Verify();
        moqValidIPInterfaceProperties.Verify();
        moqValidIpv4InterfaceProperties.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void NoValidMulticastNic_CreateAndConnect_ThrowsException()
    {
        returnedNetworkInterfaces = new[]
        {
            moqNoMultiCastAddressInterface.Object, moqNoSupportsMulticastInterface.Object, moqNotUpInterface.Object
            , moqNoInterNetworkUnicastAddressesInterface.Object
        };
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(returnedNetworkInterfaces)
            .Verifiable();
        dummyTcpSocketClient.CallCreateAndConnect(expectedHost, expectedPort);
    }

    [TestMethod]
    public void InValidMulticastNiAddress_New_DefaultsToHostnameResolution()
    {
        var invalidMulticastAddress = "BadMulticastAddress";
        moqNetworkingController = new Mock<IOSNetworkingController>();

        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(
            expectedIpAddress).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(invalidMulticastAddress))
            .Throws<ArgumentException>().Verifiable();
        dummyTcpSocketClient = new DummyUdpSubscriber(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, invalidMulticastAddress, wholeMessagesPerReceive, moqSerialzierCache.Object,
            moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);

        moqNetworkingController.Verify();
    }

    [TestMethod]
    public void NoMulticastNiAddress_New_DefaultsToHostnameResolution()
    {
        moqNetworkingController = new Mock<IOSNetworkingController>();

        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(
            expectedIpAddress).Verifiable();
        dummyTcpSocketClient = new DummyUdpSubscriber(moqFLogger.Object,
            moqDispatcher.Object, moqNetworkingController.Object, moqServerConnectionConfig.Object,
            sessionDescription, null, wholeMessagesPerReceive, moqSerialzierCache.Object,
            moqBinUnserialFac.Object, recvBufferSize, moqBinStreamPublisher.Object);

        moqNetworkingController.Verify();
    }

    private void SetupNetworkInterfaces()
    {
        validMlticstIpAddrsInfoCollctn = NonPublicInvocator.GetInstance<MulticastIPAddressInformationCollection>();
        NonPublicInvocator.RunInstanceMethod<MulticastIPAddressInformationCollection>(
            validMlticstIpAddrsInfoCollctn, "InternalAdd", new DummyMulticastIPAddressInformation(expectedIpAddress));

        noValidMlticstAddrsInfoCllctn = NonPublicInvocator.GetInstance<MulticastIPAddressInformationCollection>();

        validUnicastAddressCollection = NonPublicInvocator.GetInstance<UnicastIPAddressInformationCollection>();

        NonPublicInvocator.RunInstanceMethod<UnicastIPAddressInformationCollection>(
            validUnicastAddressCollection, "InternalAdd", new DummyUnicastIPAddressInformation(expectedIpAddress));
        noValidUnicastAddressCollection = NonPublicInvocator.GetInstance<UnicastIPAddressInformationCollection>();

        moqNoMultiCastAddressInterface = new Mock<NetworkInterface>();
        moqNoSupportsMulticastInterface = new Mock<NetworkInterface>();
        moqNotUpInterface = new Mock<NetworkInterface>();
        moqNoInterNetworkUnicastAddressesInterface = new Mock<NetworkInterface>();
        moqValidMulticastInterface = new Mock<NetworkInterface>();

        moqValidIPInterfaceProperties = new Mock<IPInterfaceProperties>();
        moqValidIPInterfaceProperties.SetupGet(ipip => ipip.MulticastAddresses)
            .Returns(validMlticstIpAddrsInfoCollctn).Verifiable();
        moqValidIpv4InterfaceProperties = new Mock<IPv4InterfaceProperties>();
        moqValidIPInterfaceProperties.Setup(ipip => ipip.GetIPv4Properties())
            .Returns(moqValidIpv4InterfaceProperties.Object).Verifiable();
        moqValidIPInterfaceProperties.SetupGet(ipip => ipip.UnicastAddresses)
            .Returns(validUnicastAddressCollection).Verifiable();
        moqValidIpv4InterfaceProperties.SetupGet(ipv4IP => ipv4IP.Index).Returns(expectedAdapterIpv4Index)
            .Verifiable();

        MakeInterfaceValidMulticastInterface(moqNoMultiCastAddressInterface);
        MakeInterfaceValidMulticastInterface(moqNoSupportsMulticastInterface);
        MakeInterfaceValidMulticastInterface(moqNotUpInterface);
        MakeInterfaceValidMulticastInterface(moqNoInterNetworkUnicastAddressesInterface);
        MakeInterfaceValidMulticastInterface(moqValidMulticastInterface);

        var moqNoMulticastAddressesIpProperties = new Mock<IPInterfaceProperties>();
        moqNoMultiCastAddressInterface.Setup(ni => ni.GetIPProperties())
            .Returns(moqNoMulticastAddressesIpProperties.Object).Verifiable();
        moqNoMulticastAddressesIpProperties.SetupGet(ipip => ipip.MulticastAddresses)
            .Returns(noValidMlticstAddrsInfoCllctn).Verifiable();

        moqNoSupportsMulticastInterface.Reset();
        moqNoSupportsMulticastInterface.SetupGet(ni => ni.SupportsMulticast).Returns(false).Verifiable();
        moqNotUpInterface.SetupGet(ni => ni.OperationalStatus).Returns(OperationalStatus.Dormant).Verifiable();

        var noValidUnicastAddressesIpProperties = new Mock<IPInterfaceProperties>();
        noValidUnicastAddressesIpProperties.SetupGet(ipip => ipip.MulticastAddresses)
            .Returns(validMlticstIpAddrsInfoCollctn).Verifiable();
        moqNoInterNetworkUnicastAddressesInterface.Setup(ni => ni.GetIPProperties())
            .Returns(noValidUnicastAddressesIpProperties.Object).Verifiable();
        noValidUnicastAddressesIpProperties.Setup(ipip => ipip.GetIPv4Properties())
            .Returns(moqValidIpv4InterfaceProperties.Object).Verifiable();
        noValidUnicastAddressesIpProperties.SetupGet(ipip => ipip.UnicastAddresses)
            .Returns(noValidUnicastAddressCollection).Verifiable();

        returnedNetworkInterfaces = new[]
        {
            moqNoMultiCastAddressInterface.Object, moqNoSupportsMulticastInterface.Object, moqNotUpInterface.Object
            , moqNoInterNetworkUnicastAddressesInterface.Object
            , moqValidMulticastInterface.Object
        };
    }

    private void MakeInterfaceValidMulticastInterface(Mock<NetworkInterface> moqInterface)
    {
        moqInterface.Setup(ni => ni.GetIPProperties()).Returns(moqValidIPInterfaceProperties.Object)
            .Verifiable();
        moqInterface.SetupGet(ni => ni.SupportsMulticast).Returns(true);
        moqInterface.SetupGet(ni => ni.OperationalStatus).Returns(OperationalStatus.Up);
    }

    internal class DummyUdpSubscriber : UdpSubscriber
    {
        private readonly IBinaryDeserializationFactory binaryDeserializationFactory;

        public DummyUdpSubscriber(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string sessionDescription, string? multicastInterface,
            int wholeMessagesPerReceive, IMap<uint, IBinaryDeserializer> serializerCache,
            IBinaryDeserializationFactory binaryDeserializationFactory, int recvBuffrSize,
            IBinaryStreamPublisher streamToPublisher)
            : base(logger, dispatcher, networkingController, connectionConfig, sessionDescription,
                multicastInterface, wholeMessagesPerReceive, serializerCache)
        {
            RecvBufferSize = recvBuffrSize;
            StreamToPublisher = streamToPublisher;
            this.binaryDeserializationFactory = binaryDeserializationFactory;
        }

        public override int RecvBufferSize { get; }

        public override IBinaryStreamPublisher StreamToPublisher { get; }

        protected override IBinaryDeserializationFactory GetFactory() => binaryDeserializationFactory;

        public IOSSocket CallCreateAndConnect(string host, int port) => CreateAndConnect(host, port);

        public override IStreamDecoder? GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers) => null;
    }

    internal class DummyUnicastIPAddressInformation : UnicastIPAddressInformation
    {
        public DummyUnicastIPAddressInformation(IPAddress address)
        {
            Address = address;
            IPv4Mask = address;
        }

        public override IPAddress Address { get; }
        public override bool IsDnsEligible { get; }
        public override bool IsTransient { get; }
        public override long AddressPreferredLifetime { get; }
        public override long AddressValidLifetime { get; }
        public override long DhcpLeaseLifetime { get; }
        public override DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }
        public override PrefixOrigin PrefixOrigin { get; }
        public override SuffixOrigin SuffixOrigin { get; }
        public override IPAddress IPv4Mask { get; }
    }

    internal class DummyMulticastIPAddressInformation : MulticastIPAddressInformation
    {
        public DummyMulticastIPAddressInformation(IPAddress address) => Address = address;

        public override IPAddress Address { get; }
        public override bool IsDnsEligible { get; }
        public override bool IsTransient { get; }
        public override long AddressPreferredLifetime { get; }
        public override long AddressValidLifetime { get; }
        public override long DhcpLeaseLifetime { get; }
        public override DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }
        public override PrefixOrigin PrefixOrigin { get; }
        public override SuffixOrigin SuffixOrigin { get; }
    }
}
