using System;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Subscription
{
    [TestClass]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class UdpSubscriberTests
    {
        private DummyUdpSubscriber dummyTcpSocketClient;
        private Mock<IBinaryStreamPublisher> moqBinStreamPublisher;
        private Mock<IBinaryDeserializationFactory> moqBinUnserialFac;
        private Mock<ISocketDispatcher> moqDispatcher;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<IConnectionConfig> moqServerConnectionConfig;
        private Mock<IFLogger> moqFLogger;
        private Mock<IMap<uint, IBinaryDeserializer>> moqSerialzierCache;
        private Mock<IOSSocket> moqSocket;

        private Mock<NetworkInterface> moqNoMultiCastAddressInterface;
        private Mock<NetworkInterface> moqNoSupportsMulticastInterface;
        private Mock<NetworkInterface> moqNotUpInterface;
        private Mock<NetworkInterface> moqNoIPv4PropertiesInterface;
        private Mock<NetworkInterface> moqNoInterNetworkUnicastAddressesInterface;
        private Mock<NetworkInterface> moqValidMulticastInterface;

        private Mock<IPInterfaceProperties> moqValidIPInterfaceProperties;
        private Mock<IPv4InterfaceProperties> moqValidIpv4InterfaceProperties;
        
        private NetworkInterface[] returnedNetworkInterfaces;

        private MulticastIPAddressInformationCollection validMlticstIpAddrsInfoCollctn;
        private MulticastIPAddressInformationCollection noValidMlticstAddrsInfoCllctn;

        private UnicastIPAddressInformationCollection validUnicastAddressCollection;
        private UnicastIPAddressInformationCollection noValidUnicastAddressCollection;

        private int recvBufferSize;
        private string sessionDescription;
        private string multicastInterface;

        private IPAddress successfulMulticaseIpAddress;
        private IPAddress unSuccessfulMulticaseIpAddress;
        
        private byte[] expectedIpAddressNumber;
        private IPAddress expectedIpAddress;
        private IPEndPoint expectedIpEndPoint;

        private string expectedHost;
        private int expectedPort;
        private int expectedAdapterIpv4Index;
        private readonly int wholeMessagesPerReceive = 20;

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

            successfulMulticaseIpAddress = new IPAddress(new byte[]{ 239, 0, 0, 222});
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
            moqNoIPv4PropertiesInterface.Verify();
            moqNoInterNetworkUnicastAddressesInterface.Verify();
            moqValidMulticastInterface.Verify();
            moqValidIPInterfaceProperties.Verify();
            moqValidIpv4InterfaceProperties.Verify();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void NoValidMulticastNic_CreateAndConnect_ThrowsException()
        {
            returnedNetworkInterfaces = new[]
            {
                moqNoMultiCastAddressInterface.Object,
                moqNoSupportsMulticastInterface.Object,
                moqNotUpInterface.Object,
                moqNoIPv4PropertiesInterface.Object,
                moqNoInterNetworkUnicastAddressesInterface.Object
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
                validMlticstIpAddrsInfoCollctn, "InternalAdd", new DummyMulticastIPAddressInformation());

            noValidMlticstAddrsInfoCllctn = NonPublicInvocator.GetInstance<MulticastIPAddressInformationCollection>();

            validUnicastAddressCollection = NonPublicInvocator.GetInstance<UnicastIPAddressInformationCollection>();

            NonPublicInvocator.RunInstanceMethod<UnicastIPAddressInformationCollection>(
                validUnicastAddressCollection, "InternalAdd", new DummyUnicastIPAddressInformation(expectedIpAddress));
            noValidUnicastAddressCollection = NonPublicInvocator.GetInstance<UnicastIPAddressInformationCollection>();

            moqNoMultiCastAddressInterface = new Mock<NetworkInterface>();
            moqNoSupportsMulticastInterface = new Mock<NetworkInterface>();
            moqNotUpInterface = new Mock<NetworkInterface>();
            moqNoIPv4PropertiesInterface = new Mock<NetworkInterface>();
            moqNoInterNetworkUnicastAddressesInterface = new Mock<NetworkInterface>();
            moqValidMulticastInterface = new Mock<NetworkInterface>();

            moqValidIPInterfaceProperties = new Mock<IPInterfaceProperties>();
            moqValidIPInterfaceProperties.SetupGet( ipip => ipip.MulticastAddresses)
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
            MakeInterfaceValidMulticastInterface(moqNoIPv4PropertiesInterface);
            MakeInterfaceValidMulticastInterface(moqNoInterNetworkUnicastAddressesInterface);
            MakeInterfaceValidMulticastInterface(moqValidMulticastInterface);

            var moqNoMulticastAddressesIpProperties = new Mock<IPInterfaceProperties>();
            moqNoMultiCastAddressInterface.Setup(ni => ni.GetIPProperties())
                .Returns(moqNoMulticastAddressesIpProperties.Object).Verifiable();
            moqNoMulticastAddressesIpProperties.SetupGet(ipip => ipip.MulticastAddresses)
                .Returns(noValidMlticstAddrsInfoCllctn).Verifiable();

            moqNoSupportsMulticastInterface.SetupGet(ni => ni.SupportsMulticast).Returns(false).Verifiable();
            moqNotUpInterface.SetupGet(ni => ni.OperationalStatus).Returns(OperationalStatus.Dormant).Verifiable();
            
            var nullIpv4IpProperties = new Mock<IPInterfaceProperties>();
            moqNoIPv4PropertiesInterface.Setup(ni => ni.GetIPProperties())
                .Returns(nullIpv4IpProperties.Object).Verifiable();
            nullIpv4IpProperties.SetupGet(ipip => ipip.MulticastAddresses)
                .Returns(validMlticstIpAddrsInfoCollctn).Verifiable();
            nullIpv4IpProperties.Setup(ipip => ipip.GetIPv4Properties())
                .Returns((IPv4InterfaceProperties)null).Verifiable();

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
                moqNoMultiCastAddressInterface.Object,
                moqNoSupportsMulticastInterface.Object,
                moqNotUpInterface.Object,
                moqNoIPv4PropertiesInterface.Object,
                moqNoInterNetworkUnicastAddressesInterface.Object,
                moqValidMulticastInterface.Object
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
                string sessionDescription, string multicastInterface,
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

        internal class DummyUnicastIPAddressInformation : UnicastIPAddressInformation
        {
            public DummyUnicastIPAddressInformation(IPAddress address)
            {
                Address = address;
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
}