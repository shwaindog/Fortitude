﻿#region

using System.Net.Sockets;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serdes.Binary;
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
    private Mock<IMessageSerializer> moqBinMock = null!;
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
        moqBinMock = new Mock<IMessageSerializer>();
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
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, buffContext) =>
            {
                for (var i = 0; i < 10; i++)
                    buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);
                buffContext.EncodedBuffer!.WrittenCursor += 10;
                buffContext.LastWriteLength = 10;
            }).Verifiable();
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
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, buffContext) =>
            {
                if (serializeNum == 0)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++)
                        buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);
                    buffContext.EncodedBuffer!.WrittenCursor += 10;
                    buffContext.LastWriteLength = 10;
                }
                else if (serializeNum == 1)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++)
                        buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 11);
                    buffContext.EncodedBuffer!.WrittenCursor += 10;
                    buffContext.LastWriteLength = 10;
                }
                else if (serializeNum == 2)
                {
                    serializeNum++;
                    for (var i = 0; i < 10; i++)
                        buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 21);
                    buffContext.EncodedBuffer!.WrittenCursor += 10;
                    buffContext.LastWriteLength = 10;
                }
            }).Verifiable();
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
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, buffContext) =>
            {
                if (buffContext.EncodedBuffer!.WrittenCursor < 10)
                {
                    calledSerializeAtBufferStartNum++;
                    for (var i = 0; i < 10; i++)
                        buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);

                    buffContext.EncodedBuffer!.WrittenCursor += 10;
                    buffContext.LastWriteLength = 10;
                }
                else
                {
                    buffContext.LastWriteLength = -1;
                }
            }).Verifiable();

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
        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, bufferedContext) =>
            {
                bufferedContext.LastWriteLength = -1;
            }).Verifiable();
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

        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, buffContext) =>
            {
                for (var i = 0; i < 20; i++)
                {
                    buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);
                    buffContext.EncodedBuffer!.WrittenCursor += 20;
                    buffContext.LastWriteLength = 20;
                }
            }).Verifiable();
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

        moqBinMock.Setup(bs => bs.Serialize(It.IsAny<IVersionedMessage>(), It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>((obj, buffContext) =>
            {
                for (var i = 0; i < 20; i++)
                {
                    buffContext.EncodedBuffer!.Buffer[buffContext.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);
                    buffContext.EncodedBuffer!.WrittenCursor += 20;
                    buffContext.LastWriteLength = 20;
                }
            }).Verifiable();
        NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock();
        var sendData = socketSessionSender.SendData();
        Assert.IsFalse(sendData);
    }
}
