#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Logging;
using FortitudeIO.Transports.Network.Receiving;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Receiving;

[TestClass]
public class SocketSelectorTests
{
    private int firstSocketHandleNum;

    private int firstSocketHandlePtr;
    private Mock<IDirectOSNetworkingApi> moqDirectOSNetworkingApi = null!;
    private Mock<ISocketReceiver> moqFirstSocketReceiver = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IPerfLogger> moqPerfLogger = null!;
    private Mock<ISocketReceiver> moqSecondSocketReceiver = null!;
    private Mock<ITimeContext> moqTimeContext = null!;
    private int secondSocketHandleNum;
    private int secondSocketHandlePtr;
    private SocketSelector socketSelector = null!;
    private TimeValue timeValue;

    [TestInitialize]
    public void SetUp()
    {
        var timeoutMs = 1000u;
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqDirectOSNetworkingApi = new Mock<IDirectOSNetworkingApi>();

        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(moqDirectOSNetworkingApi.Object);
        socketSelector = new SocketSelector(timeoutMs, moqNetworkingController.Object);

        moqFirstSocketReceiver = new Mock<ISocketReceiver>();
        firstSocketHandleNum = 12345;
        firstSocketHandlePtr = firstSocketHandleNum;
        moqFirstSocketReceiver.SetupGet(ssc => ssc.SocketHandle).Returns(firstSocketHandlePtr).Verifiable();
        moqSecondSocketReceiver = new Mock<ISocketReceiver>();
        secondSocketHandleNum = 87654;
        secondSocketHandlePtr = secondSocketHandleNum;
        moqSecondSocketReceiver.SetupGet(ssc => ssc.SocketHandle).Returns(secondSocketHandlePtr).Verifiable();
        moqPerfLogger = new Mock<IPerfLogger>();
        moqPerfLogger.Setup(ltcsl => ltcsl.Start()).Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add(SocketDataLatencyLogger.StartDataDetection))
            .Verifiable();
        moqPerfLogger.Setup(ltcsl => ltcsl.Add(SocketDataLatencyLogger.SocketDataDetected))
            .Verifiable();
        timeValue = new TimeValue((int)timeoutMs);

        moqTimeContext = new Mock<ITimeContext>();
        TimeContext.Provider = moqTimeContext.Object;
    }

    [TestCleanup]
    public void TearDown()
    {
        TimeContext.Provider = new HighPrecisionTimeContext();
    }

    private IntPtr[] GetRegisteredSocketHandles() => NonPublicInvocator.GetInstanceField<IntPtr[]>(socketSelector, "allRegisteredSocketHandles");

    private IDictionary<IntPtr, ISocketReceiver> GetSocketsDict() =>
        NonPublicInvocator.GetInstanceField<IDictionary<IntPtr, ISocketReceiver>>(socketSelector,
            "allRegisteredSocketsDict");

    [TestMethod]
    public void NewSocketSelector_RegisterSocketSessionConnection_AddsSessionForListeningForIncomingRequests()
    {
        socketSelector.Register(moqFirstSocketReceiver.Object);

        var registeredSocketHandles = GetRegisteredSocketHandles();
        var registeredSocketsDict = GetSocketsDict();

        Assert.AreEqual(1, registeredSocketsDict.Count);
        Assert.IsTrue(registeredSocketsDict.ContainsKey(firstSocketHandlePtr));
        Assert.AreEqual(moqFirstSocketReceiver.Object, registeredSocketsDict[firstSocketHandlePtr]);
        Assert.AreEqual(2, registeredSocketHandles.Length);
        Assert.AreEqual(1, registeredSocketHandles[0]);
        Assert.AreEqual(firstSocketHandlePtr, registeredSocketHandles[1]);

        moqFirstSocketReceiver.Verify();
    }

    [TestMethod]
    public void TwoRegisteredSocketSelectors_UnregisterSocketSessionConnection_RemovesSocketSessionConnection()
    {
        socketSelector.Register(moqFirstSocketReceiver.Object);
        socketSelector.Register(moqSecondSocketReceiver.Object);

        var registeredSocketHandles = GetRegisteredSocketHandles();
        var registeredSocketsDict = GetSocketsDict();

        Assert.AreEqual(2, registeredSocketsDict.Count);
        Assert.IsTrue(registeredSocketsDict.ContainsKey(firstSocketHandlePtr));
        Assert.IsTrue(registeredSocketsDict.ContainsKey(secondSocketHandlePtr));
        Assert.AreEqual(moqFirstSocketReceiver.Object, registeredSocketsDict[firstSocketHandlePtr]);
        Assert.AreEqual(moqSecondSocketReceiver.Object, registeredSocketsDict[secondSocketHandlePtr]);
        Assert.AreEqual(3, registeredSocketHandles.Length);
        Assert.AreEqual(2, registeredSocketHandles[0]);
        Assert.AreEqual(firstSocketHandlePtr, registeredSocketHandles[1]);
        Assert.AreEqual(secondSocketHandlePtr, registeredSocketHandles[2]);

        moqFirstSocketReceiver.Verify();
        moqSecondSocketReceiver.Verify();

        socketSelector.Unregister(moqFirstSocketReceiver.Object);

        registeredSocketHandles = GetRegisteredSocketHandles();
        registeredSocketsDict = GetSocketsDict();

        Assert.AreEqual(1, registeredSocketsDict.Count);
        Assert.IsTrue(registeredSocketsDict.ContainsKey(secondSocketHandlePtr));
        Assert.AreEqual(moqSecondSocketReceiver.Object, registeredSocketsDict[secondSocketHandlePtr]);
        Assert.AreEqual(2, registeredSocketHandles.Length);
        Assert.AreEqual(1, registeredSocketHandles[0]);
        Assert.AreEqual(secondSocketHandlePtr, registeredSocketHandles[1]);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void NoRegisteredSocketSessionConnection_SelectRecv_ThrowsException()
    {
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
    }

    [TestMethod]
    public void RegisteredSocketSessionConnection_SelectRecv_ReturnsWithSocketSelected()
    {
        socketSelector.Register(moqFirstSocketReceiver.Object);
        moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));
        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new nint(1), firstSocketHandlePtr },
            null, null, ref timeValue)).Returns(1).Verifiable();


        var selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();

        Assert.AreEqual(1, selectedSockets.Count);
        Assert.AreEqual(moqFirstSocketReceiver.Object, selectedSockets[0]);
        moqDirectOSNetworkingApi.Verify();
        moqPerfLogger.Verify();
    }

    [TestMethod]
    public void FourRegisteredSocketSessionConnection_MultipleSelectRecv_RotatesOrderOfSelectedSocketsReturned()
    {
        var moqThirdSocketReceiver = new Mock<ISocketReceiver>();
        var thirdSocketHandleNum = 45678;
        var thirdSocketHandlePtr = thirdSocketHandleNum;
        moqThirdSocketReceiver.SetupGet(ssc => ssc.SocketHandle).Returns(thirdSocketHandlePtr).Verifiable();
        var moqFourthSocketReceiver = new Mock<ISocketReceiver>();
        var fourthSocketHandleNum = 9876;
        var fourthSocketHandlePtr = fourthSocketHandleNum;
        moqFourthSocketReceiver.SetupGet(ssc => ssc.SocketHandle).Returns(fourthSocketHandlePtr).Verifiable();
        moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));
        socketSelector.Register(moqFirstSocketReceiver.Object);
        socketSelector.Register(moqSecondSocketReceiver.Object);
        socketSelector.Register(moqThirdSocketReceiver.Object);
        socketSelector.Register(moqFourthSocketReceiver.Object);

        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0
            , new[]
            {
                new nint(4), firstSocketHandlePtr, secondSocketHandlePtr, thirdSocketHandlePtr, fourthSocketHandlePtr
            },
            null, null, ref timeValue)).Returns(4).Verifiable();
        var selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
        Assert.AreEqual(4, selectedSockets.Count);
        Assert.AreEqual(moqFirstSocketReceiver.Object, selectedSockets[0]);
        Assert.AreEqual(moqSecondSocketReceiver.Object, selectedSockets[1]);
        Assert.AreEqual(moqThirdSocketReceiver.Object, selectedSockets[2]);
        Assert.AreEqual(moqFourthSocketReceiver.Object, selectedSockets[3]);
        moqDirectOSNetworkingApi.Verify();

        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0
            , new[]
            {
                new nint(4), secondSocketHandlePtr, thirdSocketHandlePtr, fourthSocketHandlePtr, firstSocketHandlePtr
            },
            null, null, ref timeValue)).Returns(4).Verifiable();
        selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
        Assert.AreEqual(4, selectedSockets.Count);
        Assert.AreEqual(moqSecondSocketReceiver.Object, selectedSockets[0]);
        Assert.AreEqual(moqThirdSocketReceiver.Object, selectedSockets[1]);
        Assert.AreEqual(moqFourthSocketReceiver.Object, selectedSockets[2]);
        Assert.AreEqual(moqFirstSocketReceiver.Object, selectedSockets[3]);
        moqDirectOSNetworkingApi.Verify();

        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0
            , new[]
            {
                new nint(4), thirdSocketHandlePtr, fourthSocketHandlePtr, firstSocketHandlePtr, secondSocketHandlePtr
            },
            null, null, ref timeValue)).Returns(4).Verifiable();
        selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
        Assert.AreEqual(moqThirdSocketReceiver.Object, selectedSockets[0]);
        Assert.AreEqual(moqFourthSocketReceiver.Object, selectedSockets[1]);
        Assert.AreEqual(moqFirstSocketReceiver.Object, selectedSockets[2]);
        Assert.AreEqual(moqSecondSocketReceiver.Object, selectedSockets[3]);
        moqDirectOSNetworkingApi.Verify();


        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0
            , new[]
            {
                new nint(4), fourthSocketHandlePtr, firstSocketHandlePtr, secondSocketHandlePtr, thirdSocketHandlePtr
            },
            null, null, ref timeValue)).Returns(4).Verifiable();
        selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
        Assert.AreEqual(moqFourthSocketReceiver.Object, selectedSockets[0]);
        Assert.AreEqual(moqFirstSocketReceiver.Object, selectedSockets[1]);
        Assert.AreEqual(moqSecondSocketReceiver.Object, selectedSockets[2]);
        Assert.AreEqual(moqThirdSocketReceiver.Object, selectedSockets[3]);
        moqDirectOSNetworkingApi.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void RegisteredSocketSessionConnection_SelectRecvReturnsError_ExpectedException()
    {
        socketSelector.Register(moqFirstSocketReceiver.Object);
        moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));

        moqDirectOSNetworkingApi.Setup(dosna => dosna.GetLastCallError()).Returns(12345).Verifiable();
        moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new nint(1), firstSocketHandlePtr },
            null, null, ref timeValue)).Returns(-1).Verifiable();

        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
    }
}
