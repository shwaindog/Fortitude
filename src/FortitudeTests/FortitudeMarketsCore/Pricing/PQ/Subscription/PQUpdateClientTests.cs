#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQUpdateClientTests
{
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<ISocketDispatcherResolver> moqDispatcherResolver = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IInitiateControls> moqInitiateControls = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerRepository> moqPQQuoteSerializationRepo = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<ISocketConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private PQUpdateClient pqUpdateClient = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqDispatcherResolver = new Mock<ISocketDispatcherResolver>();
        moqInitiateControls = new Mock<IInitiateControls>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqPQQuoteSerializationRepo = new Mock<IPQQuoteSerializerRepository>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        moqOsSocket = new Mock<IOSSocket>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.PortStartRange).Returns(testHostPort);
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqSerdesFactory.Object);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqOsSocket.SetupAllProperties();

        moqSerdesFactory.SetupProperty(sf => sf.StreamDecoderFactory);
        moqSerdesFactory.SetupProperty(sf => sf.StreamEncoderFactory);

        pqUpdateClient = new PQUpdateClient(moqSocketSessionContext.Object, moqInitiateControls.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void UpdateClient_GetDecoder_ReturnsPQClientDecoder()
    {
        var decoder = pqUpdateClient.MessageStreamDecoder;

        Assert.IsInstanceOfType(decoder, typeof(PQClientMessageStreamDecoder));
    }
}
