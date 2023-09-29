using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Subscription
{
    [TestClass]
    public class SocketSelectorTests
    {
        private SocketSelector socketSelector;
        private Mock<IOSNetworkingController> moqNetworkingController;
        private Mock<IDirectOSNetworkingApi> moqDirectOSNetworkingApi;
        private Mock<ISocketSessionConnection> moqFirstSocketSessionConnection;
        private Mock<ISocketSessionConnection> moqSecondSocketSessionConnection;
        private Mock<IPerfLogger> moqPerfLogger;
        private Mock<ITimeContext> moqTimeContext;

        private IntPtr firstSocketHandlePtr;
        private IntPtr secondSocketHandlePtr;
        private int firstSocketHandleNum;
        private int secondSocketHandleNum;
        private TimeValue timeValue;

        [TestInitialize]
        public void SetUp()
        {
            int timeoutMs = 1000;
            moqNetworkingController = new Mock<IOSNetworkingController>();
            moqDirectOSNetworkingApi = new Mock<IDirectOSNetworkingApi>();

            moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(moqDirectOSNetworkingApi.Object);
            socketSelector = new SocketSelector(timeoutMs, moqNetworkingController.Object);

            moqFirstSocketSessionConnection = new Mock<ISocketSessionConnection>();
            firstSocketHandleNum = 12345;
            firstSocketHandlePtr = new IntPtr(firstSocketHandleNum);
            moqFirstSocketSessionConnection.SetupGet(ssc => ssc.Handle).Returns(firstSocketHandlePtr).Verifiable();
            moqSecondSocketSessionConnection = new Mock<ISocketSessionConnection>();
            secondSocketHandleNum = 87654;
            secondSocketHandlePtr = new IntPtr(secondSocketHandleNum);
            moqSecondSocketSessionConnection.SetupGet(ssc => ssc.Handle).Returns(secondSocketHandlePtr).Verifiable();
            moqPerfLogger = new Mock<IPerfLogger>();
            moqPerfLogger.Setup(ltcsl => ltcsl.Start()).Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add(SocketDataLatencyLogger.StartDataDetection))
                .Verifiable();
            moqPerfLogger.Setup(ltcsl => ltcsl.Add(SocketDataLatencyLogger.SocketDataDetected))
                .Verifiable();
            timeValue = new TimeValue(timeoutMs);

            moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;
        }

        [TestCleanup]
        public void TearDown()
        {
            TimeContext.Provider = new HighPrecisionTimeContext();
        }

        private IntPtr[] GetRegisteredSocketHandles()
        {
            return NonPublicInvocator.GetInstanceField<IntPtr[]>(socketSelector, "allRegisteredSocketHandles");
        }

        private IDictionary<IntPtr, ISocketSessionConnection> GetSocketsDict()
        {
            return NonPublicInvocator.GetInstanceField<IDictionary<IntPtr, ISocketSessionConnection>>(socketSelector, 
                "allRegisteredSocketsDict");
        }

        [TestMethod]
        public void NewSocketSelector_RegisterSocketSessionConnection_AddsSessionForListeningForIncomingRequests()
        {
            socketSelector.Register(moqFirstSocketSessionConnection.Object);

            var registeredSocketHandles = GetRegisteredSocketHandles();
            var registeredSocketsDict = GetSocketsDict();
            
            Assert.AreEqual(1, registeredSocketsDict.Count);
            Assert.IsTrue(registeredSocketsDict.ContainsKey(firstSocketHandlePtr));
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, registeredSocketsDict[firstSocketHandlePtr]);
            Assert.AreEqual(2, registeredSocketHandles.Length);
            Assert.AreEqual(new IntPtr(1), registeredSocketHandles[0] );
            Assert.AreEqual(firstSocketHandlePtr, registeredSocketHandles[1] );

            moqFirstSocketSessionConnection.Verify();
        }

        [TestMethod]
        public void TwoRegisteredSocketSelectors_UnregisterSocketSessionConnection_RemovesSocketSessionConnection()
        {
            socketSelector.Register(moqFirstSocketSessionConnection.Object);
            socketSelector.Register(moqSecondSocketSessionConnection.Object);

            var registeredSocketHandles = GetRegisteredSocketHandles();
            var registeredSocketsDict = GetSocketsDict();

            Assert.AreEqual(2, registeredSocketsDict.Count);
            Assert.IsTrue(registeredSocketsDict.ContainsKey(firstSocketHandlePtr));
            Assert.IsTrue(registeredSocketsDict.ContainsKey(secondSocketHandlePtr));
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, registeredSocketsDict[firstSocketHandlePtr]);
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, registeredSocketsDict[secondSocketHandlePtr]);
            Assert.AreEqual(3, registeredSocketHandles.Length);
            Assert.AreEqual(new IntPtr(2), registeredSocketHandles[0]);
            Assert.AreEqual(firstSocketHandlePtr, registeredSocketHandles[1]);
            Assert.AreEqual(secondSocketHandlePtr, registeredSocketHandles[2]);

            moqFirstSocketSessionConnection.Verify();
            moqSecondSocketSessionConnection.Verify();

            socketSelector.Unregister(moqFirstSocketSessionConnection.Object);

            registeredSocketHandles = GetRegisteredSocketHandles();
            registeredSocketsDict = GetSocketsDict();

            Assert.AreEqual(1, registeredSocketsDict.Count);
            Assert.IsTrue(registeredSocketsDict.ContainsKey(secondSocketHandlePtr));
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, registeredSocketsDict[secondSocketHandlePtr]);
            Assert.AreEqual(2, registeredSocketHandles.Length);
            Assert.AreEqual(new IntPtr(1), registeredSocketHandles[0]);
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
            socketSelector.Register(moqFirstSocketSessionConnection.Object);
            moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));
            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] {new IntPtr(1), firstSocketHandlePtr}, 
                null,null, ref timeValue)).Returns(1).Verifiable();


            var selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();

            Assert.AreEqual(1, selectedSockets.Count);
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, selectedSockets[0]);
            moqDirectOSNetworkingApi.Verify();
            moqPerfLogger.Verify();
        }

        [TestMethod]
        public void FourRegisteredSocketSessionConnection_MultipleSelectRecv_RotatesOrderOfSelectedSocketsReturned()
        {
            var moqThirdSocketSessionConnection = new Mock<ISocketSessionConnection>();
            var thirdSocketHandleNum = 45678;
            var thirdSocketHandlePtr = new IntPtr(thirdSocketHandleNum);
            moqThirdSocketSessionConnection.SetupGet(ssc => ssc.Handle).Returns(thirdSocketHandlePtr).Verifiable();
            var moqFourthSocketSessionConnection = new Mock<ISocketSessionConnection>();
            var fourthSocketHandleNum = 9876;
            var fourthSocketHandlePtr = new IntPtr(fourthSocketHandleNum);
            moqFourthSocketSessionConnection.SetupGet(ssc => ssc.Handle).Returns(fourthSocketHandlePtr).Verifiable();
            moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));
            socketSelector.Register(moqFirstSocketSessionConnection.Object);
            socketSelector.Register(moqSecondSocketSessionConnection.Object);
            socketSelector.Register(moqThirdSocketSessionConnection.Object);
            socketSelector.Register(moqFourthSocketSessionConnection.Object);

            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new IntPtr(4), firstSocketHandlePtr,
                secondSocketHandlePtr, thirdSocketHandlePtr, fourthSocketHandlePtr },
                null, null, ref timeValue)).Returns(4).Verifiable();
            var selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
            Assert.AreEqual(4, selectedSockets.Count);
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, selectedSockets[0]);
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, selectedSockets[1]);
            Assert.AreEqual(moqThirdSocketSessionConnection.Object, selectedSockets[2]);
            Assert.AreEqual(moqFourthSocketSessionConnection.Object, selectedSockets[3]);
            moqDirectOSNetworkingApi.Verify();

            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new IntPtr(4), secondSocketHandlePtr,
                thirdSocketHandlePtr, fourthSocketHandlePtr, firstSocketHandlePtr },
                null, null, ref timeValue)).Returns(4).Verifiable();
            selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
            Assert.AreEqual(4, selectedSockets.Count);
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, selectedSockets[0]);
            Assert.AreEqual(moqThirdSocketSessionConnection.Object, selectedSockets[1]);
            Assert.AreEqual(moqFourthSocketSessionConnection.Object, selectedSockets[2]);
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, selectedSockets[3]);
            moqDirectOSNetworkingApi.Verify();
            
            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new IntPtr(4), thirdSocketHandlePtr,
                fourthSocketHandlePtr, firstSocketHandlePtr, secondSocketHandlePtr },
                null, null, ref timeValue)).Returns(4).Verifiable();
            selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
            Assert.AreEqual(moqThirdSocketSessionConnection.Object, selectedSockets[0]);
            Assert.AreEqual(moqFourthSocketSessionConnection.Object, selectedSockets[1]);
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, selectedSockets[2]);
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, selectedSockets[3]);
            moqDirectOSNetworkingApi.Verify();


            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new IntPtr(4), fourthSocketHandlePtr,
                firstSocketHandlePtr, secondSocketHandlePtr, thirdSocketHandlePtr },
                null, null, ref timeValue)).Returns(4).Verifiable();
            selectedSockets = socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
            Assert.AreEqual(moqFourthSocketSessionConnection.Object, selectedSockets[0]);
            Assert.AreEqual(moqFirstSocketSessionConnection.Object, selectedSockets[1]);
            Assert.AreEqual(moqSecondSocketSessionConnection.Object, selectedSockets[2]);
            Assert.AreEqual(moqThirdSocketSessionConnection.Object, selectedSockets[3]);
            moqDirectOSNetworkingApi.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RegisteredSocketSessionConnection_SelectRecvReturnsError_ExpectedException()
        {
            socketSelector.Register(moqFirstSocketSessionConnection.Object);
            moqTimeContext.Setup(tc => tc.UtcNow).Returns(new DateTime(2017, 04, 24, 14, 21, 56));

            moqDirectOSNetworkingApi.Setup(dosna => dosna.GetLastWin32Error()).Returns(12345).Verifiable();
            moqDirectOSNetworkingApi.Setup(dosna => dosna.Select(0, new[] { new IntPtr(1), firstSocketHandlePtr },
                null, null, ref timeValue)).Returns(-1).Verifiable();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            socketSelector.WatchSocketsForRecv(moqPerfLogger.Object).ToList();
        }
    }
}