#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.State;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

[TestClass]
public class PQUpdateClientTests
{
    private Mock<IPQClientMessageStreamDecoder> moqClientMessageStreamDecoder = null!;
    private Mock<IPQClientQuoteDeserializerRepository> moqDeserializationRepo = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IMessageSerdesRepositoryFactory> moqSerdesFactory = null!;
    private Mock<IMessageSerializationRepository> moqSerializationRepo = null!;
    private Mock<IEndpointConfig> moqServerConnectionConfig = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<IStreamControls> moqStreamControls = null!;

    private PQUpdateClient pqBusRulesUpdateClient = null!;
    private string testHostName = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqStreamControls = new Mock<IStreamControls>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqParallelController = new Mock<IOSParallelController>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqServerConnectionConfig = new Mock<IEndpointConfig>();
        moqSerdesFactory = new Mock<IMessageSerdesRepositoryFactory>();
        moqOsSocket = new Mock<IOSSocket>();
        moqSerializationRepo = new Mock<IMessageSerializationRepository>();
        moqDeserializationRepo = new Mock<IPQClientQuoteDeserializerRepository>();
        moqClientMessageStreamDecoder = new Mock<IPQClientMessageStreamDecoder>();

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
        moqSocketSessionContext.SetupAdd(ssc => ssc.SocketReceiverUpdated += null);
        moqSocketSessionContext.SetupGet(ssc => ssc.SerdesFactory).Returns(moqSerdesFactory.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();
        moqDeserializationRepo.Setup(mdr => mdr.Supply(It.IsAny<string>())).Returns(moqClientMessageStreamDecoder.Object);
        moqSerdesFactory.Setup(sf => sf.MessageDeserializationRepository(It.IsAny<string>())).Returns(moqDeserializationRepo.Object);
        moqSerdesFactory.SetupGet(sf => sf.MessageSerializationRepository).Returns(moqSerializationRepo.Object);

        pqBusRulesUpdateClient = new PQUpdateClient(moqSocketSessionContext.Object, moqStreamControls.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void UpdateClient_GetDecoder_ReturnsPQClientDecoder()
    {
        moqSocketSessionContext.Raise(ssc => ssc.SocketReceiverUpdated += null);

        var deserializerRepository = pqBusRulesUpdateClient.DeserializerRepository;

        Assert.IsNotNull(deserializerRepository);
    }
}
