#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Sockets;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Construction;

[TestClass]
public class SocketFactoryTests
{
    private NetworkInterface[] candidateInterfaces = null!;
    private string expectedHost = "SocketFactoryTestsHostName";

    private IPAddress expectedIpAddress = IPAddress.Parse("127.0.0.1");
    private DummyIPInterfaceProperties expectedIpv4InterfaceProperties = null!;
    private ushort expectedPort = 3333;
    private string expectedSubnetMask = "127.0.0.1";
    private Mock<IEndpointConfig> moqEndpointConfig = null!;
    private Mock<IFLogger> moqFLogger = null!;
    private Mock<IFLoggerFactory> moqFLoggerFactory = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<INetworkTopicConnectionConfig> moqNetworkTopicConnectionConfig = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private uint receiveTimeout = 10_000;
    private int recvBufferSize = 2000;
    private int sendBufferSize = 1000;
    private SocketFactory socketFactory = null!;


    [TestInitialize]
    public void SetUp()
    {
        moqFLogger = new Mock<IFLogger>();
        moqFLoggerFactory = new Mock<IFLoggerFactory>();
        moqOsSocket = new Mock<IOSSocket>();
        moqOsSocket.SetupAllProperties();
        moqNetworkTopicConnectionConfig = new Mock<INetworkTopicConnectionConfig>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqEndpointConfig = new Mock<IEndpointConfig>();
        recvBufferSize = 1234567;

        var emptyMulticastCollection
            = (MulticastIPAddressInformationCollection)NonPublicInvocator.GetInstance(typeof(MulticastIPAddressInformationCollection));
        var populatedMulticastCollection
            = (MulticastIPAddressInformationCollection)NonPublicInvocator.GetInstance(typeof(MulticastIPAddressInformationCollection));
        var addMulticastAction
            = NonPublicInvocator.GetInstanceMethodAction<MulticastIPAddressInformation>(populatedMulticastCollection, "InternalAdd");
        addMulticastAction(new DummyMulticastIPAddressInformation());
        addMulticastAction(new DummyMulticastIPAddressInformation());

        var emptyUnicastCollection
            = (UnicastIPAddressInformationCollection)NonPublicInvocator.GetInstance(typeof(UnicastIPAddressInformationCollection));
        var populatedButNotMatchingUnicastCollection
            = (UnicastIPAddressInformationCollection)NonPublicInvocator.GetInstance(typeof(UnicastIPAddressInformationCollection));
        var populatedWithMatchingUnicastCollection
            = (UnicastIPAddressInformationCollection)NonPublicInvocator.GetInstance(typeof(UnicastIPAddressInformationCollection));
        var addUnicastAction
            = NonPublicInvocator.GetInstanceMethodAction<UnicastIPAddressInformation>(populatedButNotMatchingUnicastCollection, "InternalAdd");
        addUnicastAction(new DummyUnicastIPAddressInformation(IPAddress.None));
        addUnicastAction(new DummyUnicastIPAddressInformation(IPAddress.Parse("192.168.1.1")));
        addUnicastAction
            = NonPublicInvocator.GetInstanceMethodAction<UnicastIPAddressInformation>(populatedWithMatchingUnicastCollection, "InternalAdd");
        addUnicastAction(new DummyUnicastIPAddressInformation(IPAddress.Parse("10.1.1.1")));
        addUnicastAction(new DummyUnicastIPAddressInformation(expectedIpAddress));
        addUnicastAction(new DummyUnicastIPAddressInformation(IPAddress.None));

        var noMulticastAdapterProperties = new DummyIPInterfaceProperties(emptyMulticastCollection, populatedButNotMatchingUnicastCollection
            , new DummyIPv4InterfaceProperties(0));
        var noMulticastAddressesAdapterProperties = new DummyIPInterfaceProperties(emptyMulticastCollection, populatedButNotMatchingUnicastCollection
            , new DummyIPv4InterfaceProperties(1));
        var downAdapterAdapterProperties = new DummyIPInterfaceProperties(populatedMulticastCollection, populatedButNotMatchingUnicastCollection
            , new DummyIPv4InterfaceProperties(2));
        var matchIpNotFoundAdapterProperties = new DummyIPInterfaceProperties(populatedMulticastCollection, populatedButNotMatchingUnicastCollection
            , new DummyIPv4InterfaceProperties(3));
        var noUnicastAdapterProperties
            = new DummyIPInterfaceProperties(populatedMulticastCollection, emptyUnicastCollection, new DummyIPv4InterfaceProperties(4));
        expectedIpv4InterfaceProperties = new DummyIPInterfaceProperties(populatedMulticastCollection, populatedWithMatchingUnicastCollection
            , new DummyIPv4InterfaceProperties(5));

        candidateInterfaces = new[]
        {
            new DummyNetworkInterface(noMulticastAdapterProperties, "NoMulticastSupported", "First Returned", OperationalStatus.Down, false)
            , new DummyNetworkInterface(noMulticastAddressesAdapterProperties, "NoMulticastAddresses", "Second Returned", OperationalStatus.Down
                , true)
            , new DummyNetworkInterface(downAdapterAdapterProperties, "AdapterIsDown", "Third Returned", OperationalStatus.Down, true)
            , new DummyNetworkInterface(matchIpNotFoundAdapterProperties, "MatchingIpNotFound", "Fourth Returned", OperationalStatus.Up, true)
            , new DummyNetworkInterface(noUnicastAdapterProperties, "NoUnicastAddresses", "Fifth Returned", OperationalStatus.Up, true)
            , new DummyNetworkInterface(expectedIpv4InterfaceProperties, "ExpectedInterface", "Last Returned", OperationalStatus.Up, true)
        };

        moqFLoggerFactory.Setup(flf => flf.GetLogger(It.IsAny<Type>())).Returns(moqFLogger.Object);

        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.TcpClient);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ReceiveBufferSize).Returns(recvBufferSize);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.SendBufferSize).Returns(sendBufferSize);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.None);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ResponseTimeoutMs).Returns(receiveTimeout);
        moqEndpointConfig.SetupGet(ec => ec.Hostname).Returns(expectedHost);
        moqEndpointConfig.SetupGet(ec => ec.Port).Returns(expectedPort);
        moqEndpointConfig.SetupGet(ec => ec.SubnetMask).Returns(expectedSubnetMask);
        moqEndpointConfig.SetupGet(ec => ec.SubnetMaskIpAddress).Returns(expectedIpAddress);

        moqNetworkingController.Setup(nc =>
                nc.CreateOSSocket(It.IsAny<AddressFamily>(), It.IsAny<SocketType>(), It.IsAny<ProtocolType>()))
            .Returns(moqOsSocket.Object).Verifiable();

        expectedHost = "TestHostname";
        expectedPort = 1979;


        FLoggerFactory.Instance = moqFLoggerFactory.Object;

        socketFactory = new SocketFactory(moqNetworkingController.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        FLoggerFactory.Instance = new FLoggerFactory();
    }

    [TestMethod]
    public void RequestTcpClient_NoKeepAliveCreatesSocket_WithTcpClientPropertiesSetWithoutKeepAlive()
    {
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Attempting TCP connection on {0}:{1}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.SetupSet(os => os.NoDelay = false).Verifiable();
        moqOsSocket.SetupSet(os => os.ReceiveTimeout = (int)receiveTimeout).Verifiable();
        moqOsSocket.SetupSet(os => os.SendTimeout = (int)receiveTimeout).Verifiable();
        moqOsSocket.SetupSet(os => os.SendBufferSize = sendBufferSize).Verifiable();
        moqOsSocket.SetupSet(os => os.ReceiveBufferSize = recvBufferSize).Verifiable();
        moqOsSocket.Setup(os => os.Connect(It.IsAny<IPEndPoint>())).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);

        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void RequestTcpClient_WithKeepAliveAndFastCreatesSocket_WithTcpClientProperties()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast | SocketConnectionAttributes.KeepAlive);
        moqFLogger.Setup(fl => fl.Info("Attempting TCP connection on {0}:{1}", It.IsAny<object[]>())).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqOsSocket.SetupSet(os => os.NoDelay = true).Verifiable();


        var expectedKeepAliveValues = new KeepAlive
        {
            OnOff = 1, KeepAliveTimeMillisecs = 14_000, KeepAliveIntervalMillisecs = 2_000
        };
        var expectedKeepAliveByteValues = expectedKeepAliveValues.ToByteArray();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
#pragma warning disable CA1416
            moqOsSocket.Setup(oss => oss.IOControl(IOControlCode.KeepAliveValues, expectedKeepAliveByteValues, null)).Verifiable();
#pragma warning restore CA1416
        }

        moqOsSocket.SetupSet(os => os.ReceiveTimeout = (int)receiveTimeout).Verifiable();
        moqOsSocket.SetupSet(os => os.SendTimeout = (int)receiveTimeout).Verifiable();
        moqOsSocket.SetupSet(os => os.SendBufferSize = sendBufferSize).Verifiable();
        moqOsSocket.SetupSet(os => os.ReceiveBufferSize = recvBufferSize).Verifiable();
        moqOsSocket.Setup(os => os.Connect(It.IsAny<IPEndPoint>())).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);

        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void RequestTcpAcceptor_CreatesSocket_WithTcpAcceptorProperties()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.TcpAcceptor);
        moqOsSocket.SetupSet(os => os.SendBufferSize = sendBufferSize).Verifiable();
        moqOsSocket.SetupSet(os => os.ReceiveBufferSize = recvBufferSize).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Acceptor attempting TCP bind on {0}:{1}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.Setup(os => os.Bind(It.IsAny<IPEndPoint>())).Verifiable();
        moqOsSocket.SetupSet(os => os.NoDelay = true).Verifiable();
        moqOsSocket.Setup(os => os.Listen(10)).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);

        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    public void RequestUdpSubscriber_FindsNic_CreatesSocket()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.UdpSubscriber);
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Attempting UDP-sub connection on {0}:{1}={2}:{3}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.SetupSet(os => os.ExclusiveAddressUse = false).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(candidateInterfaces).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Subscribe will bind on network adapter {0}-{1} on {2}:{3}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.Setup(os => os.Bind(It.IsAny<IPEndPoint>())).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, It.IsAny<MulticastOption>())).Verifiable();
        moqOsSocket.SetupSet(os => os.ReceiveBufferSize = recvBufferSize).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);

        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqOsSocket.Verify();
    }


    [TestMethod]
    [ExpectedException(typeof(MatchingNicNotFoundException))]
    public void RequestUdpSubscriber_CanNotFindNic_ThrowsException()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.UdpSubscriber);
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Attempting UDP-sub connection on {0}:{1}={2}:{3}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.SetupSet(os => os.ExclusiveAddressUse = false).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)).Verifiable();
        var missingExpected = new DummyNetworkInterface[candidateInterfaces.Length - 1];
        Array.Copy(candidateInterfaces, missingExpected, missingExpected.Length);
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(missingExpected).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);
    }

    [TestMethod]
    public void RequestUdpPublisher_FindsNic_CreatesSocket()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.UdpPublisher);
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Attempting UDP-pub connection on {0} {1}:{2}={3}:{4}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.SetupSet(os => os.ExclusiveAddressUse = false).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)).Verifiable();
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(candidateInterfaces).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Publish will occur from network adapter {0}-{1} on {2}:{3}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, It.IsAny<int>())).Verifiable();
        moqOsSocket.SetupSet(os => os.Ttl = 200).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, It.IsAny<int>())).Verifiable();
        moqOsSocket.SetupSet(os => os.EnableBroadcast = true).Verifiable();
        moqOsSocket.SetupSet(os => os.MulticastLoopback = false).Verifiable();
        moqOsSocket.Setup(os => os.Connect(It.IsAny<IPEndPoint>())).Verifiable();
        moqOsSocket.SetupSet(os => os.SendBufferSize = sendBufferSize).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);

        moqFLogger.Verify();
        moqNetworkingController.Verify();
        moqOsSocket.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(MatchingNicNotFoundException))]
    public void RequestUdpPublisher_CanNotFindNic_ThrowsException()
    {
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConnectionAttributes)
            .Returns(SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast);
        moqNetworkTopicConnectionConfig.SetupGet(ntcc => ntcc.ConversationProtocol)
            .Returns(SocketConversationProtocol.UdpPublisher);
        moqNetworkingController.Setup(nc => nc.GetIpAddress(It.IsAny<string>())).Returns(expectedIpAddress).Verifiable();
        moqFLogger.Setup(fl => fl.Info("Attempting UDP-pub connection on {0} {1}:{2}={3}:{4}", It.IsAny<object[]>())).Verifiable();
        moqOsSocket.SetupSet(os => os.ExclusiveAddressUse = false).Verifiable();
        moqOsSocket.Setup(os => os.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)).Verifiable();
        var missingExpected = new DummyNetworkInterface[candidateInterfaces.Length - 1];
        Array.Copy(candidateInterfaces, missingExpected, missingExpected.Length);
        moqNetworkingController.Setup(nc => nc.GetAllNetworkInterfaces()).Returns(missingExpected).Verifiable();

        socketFactory.Create(moqNetworkTopicConnectionConfig.Object, moqEndpointConfig.Object);
    }

    private class DummyNetworkInterface
    (IPInterfaceProperties interfaceProperties, string name, string description,
        OperationalStatus opStatus, bool supportMulticast) : NetworkInterface
    {
        public override string Description => description;
        public override string Name => name;
        public override bool SupportsMulticast => supportMulticast;
        public override OperationalStatus OperationalStatus => opStatus;
        public override IPInterfaceProperties GetIPProperties() => interfaceProperties;
    }

    private class DummyIPInterfaceProperties(MulticastIPAddressInformationCollection supportedAddresses,
        UnicastIPAddressInformationCollection supportedUnicastAddresses,
        IPv4InterfaceProperties ipv4InterfaceProperties) : IPInterfaceProperties
    {
        public override IPAddressInformationCollection AnycastAddresses => throw new NotImplementedException();
        public override IPAddressCollection DhcpServerAddresses => throw new NotImplementedException();
        public override IPAddressCollection DnsAddresses => throw new NotImplementedException();
        public override string DnsSuffix => "";
        public override GatewayIPAddressInformationCollection GatewayAddresses => throw new NotImplementedException();
        public override bool IsDnsEnabled => true;
        public override bool IsDynamicDnsEnabled => true;
        public override MulticastIPAddressInformationCollection MulticastAddresses => supportedAddresses;
        public override UnicastIPAddressInformationCollection UnicastAddresses => supportedUnicastAddresses;
        public override IPAddressCollection WinsServersAddresses => throw new NotImplementedException();
        public override IPv4InterfaceProperties GetIPv4Properties() => ipv4InterfaceProperties;

        public override IPv6InterfaceProperties GetIPv6Properties() => throw new NotImplementedException();
    }

    private class DummyMulticastIPAddressInformation : MulticastIPAddressInformation
    {
        public override IPAddress Address => throw new NotImplementedException();
        public override bool IsDnsEligible => true;
        public override bool IsTransient => false;
        public override long AddressPreferredLifetime => 100;
        public override long AddressValidLifetime => 100;
        public override long DhcpLeaseLifetime => 0;
        public override DuplicateAddressDetectionState DuplicateAddressDetectionState => DuplicateAddressDetectionState.Preferred;
        public override PrefixOrigin PrefixOrigin => PrefixOrigin.WellKnown;
        public override SuffixOrigin SuffixOrigin => SuffixOrigin.Manual;
    }

    private class DummyUnicastIPAddressInformation(IPAddress ipAddress) : UnicastIPAddressInformation
    {
        public override IPAddress Address => ipAddress;
        public override bool IsDnsEligible => true;
        public override bool IsTransient => false;
        public override long AddressPreferredLifetime => 100;
        public override long AddressValidLifetime => 100;
        public override long DhcpLeaseLifetime => 0;
        public override DuplicateAddressDetectionState DuplicateAddressDetectionState => DuplicateAddressDetectionState.Preferred;
        public override IPAddress IPv4Mask => ipAddress;
        public override PrefixOrigin PrefixOrigin => PrefixOrigin.WellKnown;
        public override SuffixOrigin SuffixOrigin => SuffixOrigin.Manual;
    }

    private class DummyIPv4InterfaceProperties(int index) : IPv4InterfaceProperties
    {
        public override int Index => index;
        public override bool IsAutomaticPrivateAddressingActive => true;
        public override bool IsAutomaticPrivateAddressingEnabled => true;
        public override bool IsDhcpEnabled => true;
        public override bool IsForwardingEnabled => false;
        public override int Mtu => ushort.MaxValue;
        public override bool UsesWins => true;
    }
}
