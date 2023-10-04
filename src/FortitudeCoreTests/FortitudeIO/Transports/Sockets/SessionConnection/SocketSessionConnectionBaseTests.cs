#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Sockets.SessionConnection;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection;

[TestClass]
public class SocketSessionConnectionBaseTests
{
    private Mock<IDirectOSNetworkingApi> moqDirectOsNetworkingApi = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private string sessionDescription = null!;
    private IntPtr socketHandle;
    private SocketSessionConnectionBase socketSessionConnectionBase = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqOsSocket = new Mock<IOSSocket>();
        socketHandle = new IntPtr(12345);
        moqOsSocket.SetupGet(oss => oss.Handle).Returns(socketHandle);
        moqDirectOsNetworkingApi = new Mock<IDirectOSNetworkingApi>();
        sessionDescription = "TestSessionDescription";
        socketSessionConnectionBase = new SocketSessionConnectionBase(moqOsSocket.Object,
            moqDirectOsNetworkingApi.Object, sessionDescription);
    }

    [TestMethod]
    public void NewSocketSession_New_IntializesPropertiesAsExpected()
    {
        Assert.IsTrue(socketSessionConnectionBase.Id > 0);
        Assert.AreEqual(false, socketSessionConnectionBase.Active);
        Assert.AreEqual(moqOsSocket.Object, socketSessionConnectionBase.Socket);
        Assert.AreEqual(socketHandle, socketSessionConnectionBase.Handle);
        Assert.AreEqual(sessionDescription, socketSessionConnectionBase.SessionDescription);
    }

    [TestMethod]
    public void NewSocketSession_ToString_ContainsIdAndSessionDescription()
    {
        var asString = socketSessionConnectionBase.ToString();
        Assert.IsTrue(asString.Contains(socketSessionConnectionBase.Id.ToString()));
        Assert.IsTrue(asString.Contains(socketSessionConnectionBase.SessionDescription));
    }

    [TestMethod]
    public void TwoSocketSessions_EqualsGetHashcode_DifferentiateWhenDifferent()
    {
        var firstSocketSession = new SocketSessionConnectionBase(moqOsSocket.Object,
            moqDirectOsNetworkingApi.Object, sessionDescription);
        var secondSocketSession = new SocketSessionConnectionBase(moqOsSocket.Object,
            moqDirectOsNetworkingApi.Object, sessionDescription);

        Assert.AreNotEqual(firstSocketSession, secondSocketSession);
        Assert.AreEqual(firstSocketSession, firstSocketSession);
        Assert.AreEqual(firstSocketSession.GetHashCode(), firstSocketSession.GetHashCode());
        Assert.AreNotEqual(firstSocketSession.GetHashCode(), secondSocketSession.GetHashCode());
    }
}
