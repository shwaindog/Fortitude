#region

using System.Net;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.NewSocketAPI.State;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.Sockets;

[TestClass]
public class SocketSessionConnectionTests
{
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<IOSParallelController> moqParallelControler = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<ISocketConnectionConfig> moqSocketConnectionConfig = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactories = null!;
    private Mock<ISocketReconnectConfig> moqSocketReconnectConfig = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;

    private SocketConnection socketConnection = null!;
    private ushort testHostPort;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqParallelControler = new Mock<IOSParallelController>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqSocketConnectionConfig = new Mock<ISocketConnectionConfig>();
        moqSocketReconnectConfig = new Mock<ISocketReconnectConfig>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelControler.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqOsSocket = new Mock<IOSSocket>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketFactories = new Mock<ISocketFactoryResolver>();

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactories.Object);
        moqSocketFactories.SetupGet(ssc => ssc.ParallelController).Returns(moqParallelControler.Object);
        testHostPort = 1979;
        moqFlogger.Setup(fl => fl.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        moqOsSocket.SetupAllProperties();

        socketConnection = new SocketConnection("SocketSessionConnectionTests", ConversationType.Requester
            , moqOsSocket.Object, new IPAddress(new byte[] { 127, 0, 0, 0 }), testHostPort);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }


    [TestMethod]
    public void NewSocketConnection_SocketIsConnectedOrBound_ReturnsTrueWhenSocketIsConnectedOrBound()
    {
        Assert.IsFalse(socketConnection.IsConnected);
        moqOsSocket.SetupGet(oss => oss.IsBound).Returns(true).Verifiable();
        Assert.IsTrue(socketConnection.IsConnected);
        moqOsSocket.SetupGet(oss => oss.IsBound).Returns(false).Verifiable();
        Assert.IsFalse(socketConnection.IsConnected);
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(true).Verifiable();
        Assert.IsTrue(socketConnection.IsConnected);
        moqOsSocket.SetupGet(oss => oss.Connected).Returns(false).Verifiable();
        Assert.IsFalse(socketConnection.IsConnected);
    }
}
