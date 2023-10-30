#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection;

[TestClass]
public class SocketSessionSenderTests
{
    private bool calledApiSend;
    private DirectOSNetworkingStub directOSNetworkingStub = null!;
    private Mock<IBinarySerializer> moqBinMock = null!;
    private Mock<IOSSocket> moqSocket = null!;
    private Mock<ISyncLock> moqSyncLock = null!;
    private Func<IntPtr, byte[], int, SocketFlags, int> sendCallBack = null!;
    private SocketSessionSender socketSessionSender = null!;
    private string testSessionDescription = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqSocket = new Mock<IOSSocket>();
        moqSyncLock = new Mock<ISyncLock>();
        moqBinMock = new Mock<IBinarySerializer>();
        moqSocket.SetupGet(oss => oss.SendBufferSize).Returns(1000);
        calledApiSend = false;
        sendCallBack = (scktHndl, buffPtr, len, flags) =>
        {
            Assert.AreEqual(10, len);
            for (var i = 0; i < 10; i++) Assert.AreEqual(i + 1, buffPtr[i]);
            calledApiSend = true;
            return 10;
        };

        directOSNetworkingStub = new DirectOSNetworkingStub(null, sendCallBack);
        testSessionDescription = "Test Session Description";

        socketSessionSender = new SocketSessionSender(moqSocket.Object,
            directOSNetworkingStub, testSessionDescription);
    }

    [TestMethod]
    public void NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock()
    {
        NonPublicInvocator.SetInstanceField(socketSessionSender, "sendLock", moqSyncLock.Object);
        var sendLockCalled = 0;
        moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => { sendLockCalled++; }).Verifiable();

        var encoders =
            NonPublicInvocator.GetInstanceField<StaticRing<SocketSessionSender.SocketEncoder>>(socketSessionSender,
                "encoders");
        moqSyncLock.Setup(sl => sl.Release()).Verifiable();
        var message = new OrxLogonResponse();

        Assert.AreEqual(0, encoders.ConsumerCursor);
        Assert.AreEqual(0, encoders.PublisherCursor);
        socketSessionSender.Enqueue(message, moqBinMock.Object);

        Assert.AreEqual(0, encoders.ConsumerCursor);
        Assert.AreEqual(1, encoders.PublisherCursor);
        Assert.AreEqual(1, sendLockCalled);
        moqSyncLock.Verify();
    }

    [TestMethod]
    public void OneQueuedMessage_SendData_SerializesMessageSendsToDirectOsNetworkingApi()
    {
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IVersionedMessage>()))
            .Callback<byte[], int, object>((byteArry, startPos, obj) =>
            {
                for (var i = 0; i < 10; i++) byteArry[startPos + i] = (byte)(i + 1);
            }).Returns(10).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        var sendSuccess = socketSessionSender.SendData();
        Assert.IsTrue(sendSuccess);
        Assert.IsTrue(calledApiSend);
        moqBinMock.Verify();
    }

    [TestMethod]
    public void NoQueuedMessages_SendData_ReturnsTrue()
    {
        var sendSuccess = socketSessionSender.SendData();
        Assert.IsTrue(sendSuccess);
    }

    [TestMethod]
    public void MultipleQueuedMessages_SendData_CallsSendForAllMessages()
    {
        sendCallBack = (scktHndl, buffPtr, len, flags) =>
        {
            for (var i = 0; i < 30; i++) Assert.AreEqual(i + 1, buffPtr[i]);
            calledApiSend = true;
            return 30;
        };
        directOSNetworkingStub = new DirectOSNetworkingStub(null, sendCallBack);
        socketSessionSender = new SocketSessionSender(moqSocket.Object,
            directOSNetworkingStub, testSessionDescription);

        var serializeNum = 0;
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IVersionedMessage>()))
            .Callback<byte[], int, object>((byteArry, startPos, obj) =>
            {
                if (serializeNum == 0)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++) byteArry[startPos + i] = (byte)(i + 1);
                }
                else if (serializeNum == 1)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++) byteArry[startPos + i] = (byte)(i + 11);
                }
                else if (serializeNum == 2)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++) byteArry[startPos + i] = (byte)(i + 21);
                }
            }).Returns(10).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        var message = new OrxLogonResponse();
        socketSessionSender.Enqueue(message, moqBinMock.Object);
        socketSessionSender.Enqueue(message, moqBinMock.Object);
        var sendSuccess = socketSessionSender.SendData();
        Assert.IsTrue(sendSuccess);
        Assert.IsTrue(calledApiSend);
        moqBinMock.Verify();
    }

    [TestMethod]
    public void TwoMsgsCantSerialize2ndMsgFullBuffer_SendData_SendsFirstMsgToClearBuffThenSends2ndMsg()
    {
        var calledSerializeAtBufferStartNum = 0;
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), 0, It.IsAny<IVersionedMessage>()))
            .Returns(10)
            .Callback<byte[], int, object>((byteArry, startPos, obj) =>
            {
                calledSerializeAtBufferStartNum++;
                for (var i = 0; i < 10; i++) byteArry[startPos + i] = (byte)(i + 1);
            }).Verifiable();

        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), 10, It.IsAny<IVersionedMessage>()))
            .Returns(-1).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        var message = new OrxLogonResponse();
        socketSessionSender.Enqueue(message, moqBinMock.Object);
        var sendSuccess = socketSessionSender.SendData();
        Assert.IsTrue(sendSuccess);
        Assert.IsTrue(calledApiSend);
        Assert.AreEqual(2, calledSerializeAtBufferStartNum);
        moqBinMock.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void OneMsgCantSerializeToBuffer_SendData_ThrowsExceptionMsgTooBig()
    {
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IVersionedMessage>()))
            .Returns(-1).Verifiable();
        socketSessionSender.Enqueue(new OrxLogonResponse(), moqBinMock.Object);
        socketSessionSender.SendData();
        Assert.Fail("Show not reach here");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void OneMsgQueued_SendDataError_ThrowsExceptionWithError()
    {
        sendCallBack = (scktHndl, buffPtr, len, flags) => -1;

        var hasCalledLastWin32Error = false;
        var moqLastWin32Error = () =>
        {
            hasCalledLastWin32Error = true;
            return 20;
        };
        directOSNetworkingStub = new DirectOSNetworkingStub(moqLastWin32Error, sendCallBack);
        socketSessionSender = new SocketSessionSender(moqSocket.Object,
            directOSNetworkingStub, testSessionDescription);

        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IVersionedMessage>()))
            .Callback<byte[], int, object>((byteArry, startPos, obj) =>
            {
                for (var i = 0; i < 20; i++) byteArry[startPos + i] = (byte)(i + 1);
            }).Returns(20).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        try
        {
            socketSessionSender.SendData();
        }
        catch (Exception)
        {
            Assert.IsTrue(hasCalledLastWin32Error);
            throw;
        }

        Assert.Fail("Show not reach here");
    }

    [TestMethod]
    public void OneMsgQueued_SendDataNotAllDataSent_ReturnsFalse()
    {
        sendCallBack = (scktHndl, buffPtr, len, flags) => 10;

        directOSNetworkingStub = new DirectOSNetworkingStub(null, sendCallBack);
        socketSessionSender = new SocketSessionSender(moqSocket.Object,
            directOSNetworkingStub, testSessionDescription);

        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IVersionedMessage>()))
            .Callback<byte[], int, object>((byteArry, startPos, obj) =>
            {
                for (var i = 0; i < 20; i++) byteArry[startPos + i] = (byte)(i + 1);
            }).Returns(20).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        var sendData = socketSessionSender.SendData();
        Assert.IsFalse(sendData);
    }
}
