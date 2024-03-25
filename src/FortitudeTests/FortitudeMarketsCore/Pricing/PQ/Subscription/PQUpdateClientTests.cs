#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Controls;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQUpdateClientTests
{
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IInitiateControls> moqInitiateControls = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISerdesFactory> moqSerdesFactory = null!;
    private Mock<ISocketConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ISocketFactories> moqSocketFactories = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private PQUpdateClient pqUpdateClient = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqInitiateControls = new Mock<IInitiateControls>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqParallelController = new Mock<IOSParallelController>();
        moqSocketFactories = new Mock<ISocketFactories>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqServerConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqSerdesFactory = new Mock<ISerdesFactory>();
        moqOsSocket = new Mock<IOSSocket>();

        var stateChangedFunc = new Mock<Func<ISocketSessionContext, ISocketConnectivityChanged>>();
        var moqSocketConnectivityChange = new Mock<ISocketConnectivityChanged>();
        moqSocketConnectivityChange.Setup(scc => scc.GetOnConnectionChangedHandler())
            .Returns(new Mock<Action<SocketSessionState>>().Object);
        stateChangedFunc.Setup(scf => scf.Invoke(It.IsAny<ISocketSessionContext>()))
            .Returns(moqSocketConnectivityChange.Object);
        moqSocketFactories.SetupGet(sf => sf.ConnectionChangedHandlerResolver)
            .Returns(stateChangedFunc.Object);

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqSerdesFactory.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactories).Returns(moqSocketFactories.Object);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
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
