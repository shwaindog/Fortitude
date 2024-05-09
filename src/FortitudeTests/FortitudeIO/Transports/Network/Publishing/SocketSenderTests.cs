#region

using System.Net.Sockets;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.Sockets;
using FortitudeIO.Transports.Network.State;
using FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Publishing;

[TestClass]
public class SocketSenderTests
{
    private const uint MessageId = 12;
    private const int SerializeWriteLength = 99;
    private const int SocketSendBufferSize = 150;
    private const int SendErrorCode = 71;
    private int calledApiSendCount;
    private DirectOSNetworkingStub directOsNetworkingStub = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
    private Mock<IMessageSerializationRepository> moqMessageSerializationRepo = null!;
    private Mock<IMessageSerializer> moqMessageSerializer = null!;
    private Mock<IOSNetworkingController> moqNetworkingController = null!;
    private Mock<IOSSocket> moqOsSocket = null!;
    private Mock<ISocketConnection> moqSocketConnection = null!;
    private Mock<ISocketDispatcher> moqSocketDispatcher = null!;
    private Mock<ISocketDispatcherSender> moqSocketDispatcherSender = null!;
    private Mock<ISocketFactoryResolver> moqSocketFactoryResolver = null!;
    private Mock<ISocketSessionContext> moqSocketSessionContext = null!;
    private Mock<IVersionedMessage> moqVersionedMessage = null!;
    private Func<IntPtr, byte[], int, SocketFlags, int> sendCallBack = null!;
    private SocketSender socketSender = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqFlogger = new Mock<IFLogger>();
        moqFloggerFactory = new Mock<IFLoggerFactory>();
        moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqVersionedMessage = new Mock<IVersionedMessage>();
        moqMessageSerializer = new Mock<IMessageSerializer>();
        moqSocketFactoryResolver = new Mock<ISocketFactoryResolver>();
        moqSocketConnection = new Mock<ISocketConnection>();
        moqOsSocket = new Mock<IOSSocket>();
        moqOsSocket.SetupAllProperties();
        moqSocketDispatcher = new Mock<ISocketDispatcher>();
        moqSocketDispatcherSender = new Mock<ISocketDispatcherSender>();
        moqNetworkingController = new Mock<IOSNetworkingController>();
        moqMessageSerializationRepo = new Mock<IMessageSerializationRepository>();

        calledApiSendCount = 0;
        sendCallBack = (_, buffPtr, len, _) =>
        {
            Assert.AreEqual(SerializeWriteLength, len);
            for (var i = 0; i < SerializeWriteLength; i++) Assert.AreEqual(i + 1, buffPtr[i]);
            calledApiSendCount++;
            return SerializeWriteLength;
        };

        unsafe void SerializeCallback(IVersionedMessage _, IBufferContext bc)
        {
            using var fixedBuffer = bc.EncodedBuffer!;
            if (fixedBuffer.RemainingStorage < SerializeWriteLength)
            {
                bc.LastWriteLength = 0;
                return;
            }

            var ptr = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            for (var i = 0; i < SerializeWriteLength; i++) *ptr++ = (byte)(i + 1);

            bc.LastWriteLength = SerializeWriteLength;
            fixedBuffer.WriteCursor += SerializeWriteLength;
        }

        moqMessageSerializer.Setup(ms => ms.Serialize(moqVersionedMessage.Object, It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>(SerializeCallback);

        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);

        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqOsSocket.Object);
        moqSocketConnection.SetupGet(os => os.IsConnected).Returns(true);
        moqOsSocket.SetupGet(os => os.SendBufferSize).Returns(SocketSendBufferSize);
        moqSocketFactoryResolver.SetupGet(sfr => sfr.NetworkingController).Returns(moqNetworkingController.Object);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactoryResolver.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqSocketDispatcher.Object);

        moqMessageSerializationRepo.Setup(msr => msr.GetSerializer(It.IsAny<uint>())).Returns(moqMessageSerializer.Object);

        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        socketSender = new SocketSender(moqSocketSessionContext.Object, moqMessageSerializationRepo.Object);
    }

    [TestCleanup]
    public void TearDown()
    {
        FLoggerFactory.Instance = new FLoggerFactory();
    }

    [TestMethod]
    public void RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue()
    {
        moqVersionedMessage.Setup(sc => sc.IncrementRefCount()).Returns(1).Verifiable();
        moqVersionedMessage.SetupGet(sc => sc.MessageId).Returns(MessageId).Verifiable();
        moqSocketDispatcher.SetupGet(sd => sd.Sender).Returns(moqSocketDispatcherSender.Object).Verifiable();
        moqSocketDispatcherSender.Setup(sd => sd.EnqueueSocketSender(socketSender)).Verifiable();

        socketSender.Send(moqVersionedMessage.Object);

        moqVersionedMessage.Verify();
        moqSocketDispatcher.Verify();
        moqSocketDispatcherSender.Verify();
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void UnregisterMessageSerializer_SendVersionMessage_ThrowsKeyNotFoundException()
    {
        moqVersionedMessage.Setup(sc => sc.IncrementRefCount()).Returns(1).Verifiable();
        moqVersionedMessage.SetupGet(sc => sc.MessageId).Returns(MessageId).Verifiable();
        moqSocketDispatcher.SetupGet(sd => sd.Sender).Returns(moqSocketDispatcherSender.Object).Verifiable();
        moqSocketDispatcherSender.Setup(sd => sd.EnqueueSocketSender(socketSender)).Verifiable();
        moqMessageSerializationRepo.Setup(msr => msr.GetSerializer(It.IsAny<uint>())).Returns(null as IMessageSerializer);

        socketSender.Send(moqVersionedMessage.Object);
    }

    [TestMethod]
    public void AddToSendQueueSocketSender_SendVersionMessage_CallsEnqueueButNotAddToSendQueue()
    {
        RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue();

        socketSender.Send(moqVersionedMessage.Object);

        moqVersionedMessage.Verify(vm => vm.MessageId, Times.AtLeast(2));
        moqVersionedMessage.Verify(vm => vm.IncrementRefCount(), Times.AtLeast(2));
        moqSocketDispatcherSender.Verify(vm => vm.EnqueueSocketSender(socketSender), Times.Once);
    }

    [TestMethod]
    public void QueuedMessageAndSerializer_SendQueued_SerializesMessageAndCallsSend()
    {
        calledApiSendCount = 0;
        RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue();

        socketSender.SendQueued();

        moqMessageSerializer.Verify();
        Assert.AreEqual(1, calledApiSendCount);
    }

    [TestMethod]
    [ExpectedException(typeof(SocketSendException))]
    public void QueuedMessageAndSerializer_SendQueued_FailsLogsErrorCode()
    {
        sendCallBack = (_, _, _, _) => -1;
        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);
        socketSender = new SocketSender(moqSocketSessionContext.Object, moqMessageSerializationRepo.Object);

        socketSender.MessageSerializationRepository.RegisterSerializer(MessageId, moqMessageSerializer.Object);
        RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue();

        socketSender.SendQueued();
    }

    [TestMethod]
    public void SocketSender_ResetsBufferOnSend_SendsMultipleMessagesThatExceedTheSocketBuffer()
    {
        QueuedMessageAndSerializer_SendQueued_SerializesMessageAndCallsSend();
        QueuedMessageAndSerializer_SendQueued_SerializesMessageAndCallsSend();
        QueuedMessageAndSerializer_SendQueued_SerializesMessageAndCallsSend();
    }

    [TestMethod]
    public void SocketSender_MoreDataThanCanBeWrittenToTheBuffer_SendsMultipleMessagesByRetryingAfterSend()
    {
        calledApiSendCount = 0;
        moqVersionedMessage.Setup(sc => sc.IncrementRefCount()).Returns(1).Verifiable();
        moqVersionedMessage.SetupGet(sc => sc.MessageId).Returns(MessageId).Verifiable();
        moqSocketDispatcher.SetupGet(sd => sd.Sender).Returns(moqSocketDispatcherSender.Object).Verifiable();
        moqSocketDispatcherSender.Setup(sd => sd.EnqueueSocketSender(socketSender)).Verifiable();

        unsafe void SerializeCallback(IVersionedMessage _, IBufferContext bc)
        {
            using var fixedBuffer = bc.EncodedBuffer!;
            bc.LastWriteLength = 0;
            if (fixedBuffer.RemainingStorage < SerializeWriteLength)
            {
                bc.LastWriteLength = 0;
                return;
            }

            var ptr = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            for (var i = 0; i < SerializeWriteLength; i++) *ptr++ = (byte)(i + 1);
            bc.LastWriteLength = SerializeWriteLength;
            bc.EncodedBuffer!.WriteCursor += SerializeWriteLength;
        }

        moqMessageSerializer.Setup(ms => ms.Serialize(moqVersionedMessage.Object, It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>(SerializeCallback);

        socketSender.Send(moqVersionedMessage.Object);
        socketSender.Send(moqVersionedMessage.Object);
        socketSender.Send(moqVersionedMessage.Object);

        socketSender.SendQueued();
        Assert.AreEqual(3, calledApiSendCount);
    }


    [TestMethod]
    public void SocketSender_CanNotSerializeAMessageAfter100Attempts_LogsSocketSendError()
    {
        calledApiSendCount = 0;
        moqVersionedMessage.Setup(sc => sc.IncrementRefCount()).Returns(1).Verifiable();
        moqVersionedMessage.SetupGet(sc => sc.MessageId).Returns(MessageId).Verifiable();
        moqSocketDispatcher.SetupGet(sd => sd.Sender).Returns(moqSocketDispatcherSender.Object).Verifiable();
        moqSocketDispatcherSender.Setup(sd => sd.EnqueueSocketSender(socketSender)).Verifiable();

        void SerializeCallback(IVersionedMessage _, IBufferContext bc)
        {
            bc.LastWriteLength = 0; // fail on purpose
        }

        moqMessageSerializer.Setup(ms => ms.Serialize(moqVersionedMessage.Object, It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>(
                SerializeCallback
            );
        sendCallBack = (_, _, _, _) => 0;
        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);
        socketSender = new SocketSender(moqSocketSessionContext.Object, moqMessageSerializationRepo.Object);
        socketSender.MessageSerializationRepository.RegisterSerializer(MessageId, moqMessageSerializer.Object);
        moqFlogger.Setup(fl =>
                fl.Error("Message could not be serialized or was too big for the buffer or there was no bytes to serialize. Message {0}"
                    , It.IsAny<object[]>()))
            .Verifiable();

        socketSender.Send(moqVersionedMessage.Object);


        for (var i = 0; i < 101; i++) socketSender.SendQueued();

        moqFlogger.Verify();
    }

    [TestMethod]
    public void SocketSender_MultipleEncodedMessages_SendsMultipleAsOneCallToSocket()
    {
        calledApiSendCount = 0;
        const int smallMessageSize = 50;
        moqVersionedMessage.Setup(sc => sc.IncrementRefCount()).Returns(1).Verifiable();
        moqVersionedMessage.SetupGet(sc => sc.MessageId).Returns(MessageId).Verifiable();
        moqSocketDispatcher.SetupGet(sd => sd.Sender).Returns(moqSocketDispatcherSender.Object).Verifiable();
        moqSocketDispatcherSender.Setup(sd => sd.EnqueueSocketSender(socketSender)).Verifiable();

        unsafe void SerializeCallback(IVersionedMessage _, IBufferContext bc)
        {
            using var fixedBuffer = bc.EncodedBuffer!;
            bc.LastWriteLength = 0;
            if (fixedBuffer.RemainingStorage < smallMessageSize)
            {
                bc.LastWriteLength = 0;
                return;
            }

            var ptr = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
            for (var i = 0; i < smallMessageSize; i++) *ptr++ = (byte)(i + 1);
            bc.LastWriteLength = smallMessageSize;
            bc.EncodedBuffer!.WriteCursor += smallMessageSize;
        }

        moqMessageSerializer.Setup(ms => ms.Serialize(moqVersionedMessage.Object, It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>(
                SerializeCallback
            );
        sendCallBack = (_, _, len, _) =>
        {
            calledApiSendCount++;
            return len;
        };
        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);
        socketSender = new SocketSender(moqSocketSessionContext.Object, moqMessageSerializationRepo.Object);
        socketSender.MessageSerializationRepository.RegisterSerializer(MessageId, moqMessageSerializer.Object);

        socketSender.Send(moqVersionedMessage.Object);
        socketSender.Send(moqVersionedMessage.Object);
        socketSender.Send(moqVersionedMessage.Object); // these 3 will be sent together
        socketSender.Send(moqVersionedMessage.Object);
        socketSender.Send(moqVersionedMessage.Object); // then these two

        socketSender.SendQueued();
        Assert.AreEqual(2, calledApiSendCount);
    }
}
