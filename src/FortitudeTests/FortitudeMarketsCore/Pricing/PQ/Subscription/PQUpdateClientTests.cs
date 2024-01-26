#region

using System.Reactive.Subjects;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription;

[TestClass]
public class PQUpdateClientTests
{
    private ISubject<IConnectionUpdate> configUpdateSubject = null!;
    private Mock<ISocketDispatcher> moqDispatcher = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPQQuoteSerializerFactory> moqPQQuoteSerializationFactory = null!;
    private Mock<IMap<uint, IMessageDeserializer>> moqSerializerCache = null!;
    private Mock<IConnectionConfig> moqServerConnectionConfig = null!;
    private Mock<ICallbackMessageDeserializer<PQLevel0Quote>> moqSocketBinaryDeserializer = null!;
    private PQUpdateClient pqUpdateClient = null!;
    private string testHostName = null!;
    private int testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqDispatcher = new Mock<ISocketDispatcher>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqServerConnectionConfig = new Mock<IConnectionConfig>();
        moqPQQuoteSerializationFactory = new Mock<IPQQuoteSerializerFactory>();
        moqSocketBinaryDeserializer = new Mock<ICallbackMessageDeserializer<PQLevel0Quote>>();
        moqOsSocket = new Mock<IOSSocket>();
        configUpdateSubject = new Subject<IConnectionUpdate>();

        testHostName = "TestHostname";
        moqServerConnectionConfig.SetupGet(scc => scc.Hostname).Returns(testHostName);
        testHostPort = 1979;
        moqServerConnectionConfig.SetupGet(scc => scc.Port).Returns(testHostPort);
        moqServerConnectionConfig.SetupProperty(scc => scc.Updates, configUpdateSubject);
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqSerializerCache = new Mock<IMap<uint, IMessageDeserializer>>();
        moqOsSocket.SetupAllProperties();

        moqSocketBinaryDeserializer.SetupAllProperties();

        moqPQQuoteSerializationFactory.Setup(pqqsf => pqqsf.GetDeserializer<PQLevel0Quote>(uint.MaxValue))
            .Returns(moqSocketBinaryDeserializer.Object).Verifiable();

        pqUpdateClient = new PQUpdateClient(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, "TestSocketDescription", "multicastInterfaceIP", 50,
            moqPQQuoteSerializationFactory.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void MissingSerializationFactory_New_UsesDefaultSerializationFactory()
    {
        pqUpdateClient = new PQUpdateClient(moqDispatcher.Object, moqNetworkingController.Object,
            moqServerConnectionConfig.Object, "TestSocketDescription", "multicastInterfaceIP", 50,
            new PQQuoteSerializerFactory());

        var binaryDeserializationFactory = NonPublicInvocator.RunInstanceMethod<IBinaryDeserializationFactory>(
            pqUpdateClient, "GetFactory");
        Assert.IsInstanceOfType(binaryDeserializationFactory, typeof(PQQuoteSerializerFactory));
    }

    [TestMethod]
    public void UpdateClient_GetDecoder_ReturnsPQClientDecoder()
    {
        var decoder = pqUpdateClient.GetDecoder(moqSerializerCache.Object);

        Assert.IsInstanceOfType(decoder, typeof(PQClientMessageStreamDecoder));
    }

    [TestMethod]
    public void UpdateClient_RecvBufferSize_ReturnsPQClientDecoder()
    {
        var bufferSize = pqUpdateClient.RecvBufferSize;

        Assert.AreEqual(2097152, bufferSize);
    }

    [TestMethod]
    public void UpdateClient_HasNoStreamToPublisher()
    {
        Assert.IsNull(pqUpdateClient.StreamToPublisher);
    }
}
