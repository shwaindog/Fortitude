#region

using System.Net.Sockets;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.NewSocketAPI.State;
using FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.Publishing;

[TestClass]
public class SocketSenderTests
{
    private const uint MessageId = 12;
    private const int SerializeWriteLength = 99;
    private const int SocketSendBuffer = 150;
    private const int SendErrorCode = 71;
    private bool calledApiSend;
    private DirectOSNetworkingStub directOsNetworkingStub = null!;
    private Mock<IFLogger> moqFlogger = null!;
    private Mock<IFLoggerFactory> moqFloggerFactory = null!;
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

        calledApiSend = false;
        sendCallBack = (_, buffPtr, len, _) =>
        {
            Assert.AreEqual(SerializeWriteLength, len);
            for (var i = 0; i < SerializeWriteLength; i++) Assert.AreEqual(i + 1, buffPtr[i]);
            calledApiSend = true;
            return SerializeWriteLength;
        };
        moqMessageSerializer.Setup(ms => ms.Serialize(moqVersionedMessage.Object, It.IsAny<IBufferContext>()))
            .Callback<IVersionedMessage, IBufferContext>(
                (_, bc) =>
                {
                    for (var i = 0; i < SerializeWriteLength; i++)
                        bc.EncodedBuffer!.Buffer[bc.EncodedBuffer.WrittenCursor + i] = (byte)(i + 1);
                    bc.LastWriteLength = SerializeWriteLength;
                    bc.EncodedBuffer!.WrittenCursor = SerializeWriteLength;
                }
            );

        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);

        moqSocketConnection.SetupGet(sc => sc.OSSocket).Returns(moqOsSocket.Object);
        moqOsSocket.SetupGet(sc => sc.SendBufferSize).Returns(SocketSendBuffer);
        moqSocketFactoryResolver.SetupGet(sfr => sfr.NetworkingController).Returns(moqNetworkingController.Object);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);

        moqSocketSessionContext.SetupGet(ssc => ssc.SocketConnection).Returns(moqSocketConnection.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketFactoryResolver).Returns(moqSocketFactoryResolver.Object);
        moqSocketSessionContext.SetupGet(ssc => ssc.SocketDispatcher).Returns(moqSocketDispatcher.Object);

        moqFloggerFactory.Setup(ff => ff.GetLogger(It.IsAny<Type>())).Returns(moqFlogger.Object);
        FLoggerFactory.Instance = moqFloggerFactory.Object;

        socketSender = new SocketSender(moqSocketSessionContext.Object);

        socketSender.RegisterSerializer(MessageId, moqMessageSerializer.Object);
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
        moqSocketDispatcherSender.Setup(sd => sd.AddToSendQueue(socketSender)).Verifiable();

        socketSender.Send(moqVersionedMessage.Object);

        moqVersionedMessage.Verify();
        moqSocketDispatcher.Verify();
        moqSocketDispatcherSender.Verify();
    }

    [TestMethod]
    public void AddToSendQueueSocketSender_SendVersionMessage_CallsEnqueueButNotAddToSendQueue()
    {
        RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue();

        socketSender.Send(moqVersionedMessage.Object);

        moqVersionedMessage.Verify(vm => vm.MessageId, Times.AtLeast(2));
        moqVersionedMessage.Verify(vm => vm.IncrementRefCount(), Times.AtLeast(2));
        moqSocketDispatcherSender.Verify(vm => vm.AddToSendQueue(socketSender), Times.Once);
    }

    [TestMethod]
    public void QueuedMessageAndSerializer_SendQueued_SerializesMessageAndCallsSend()
    {
        calledApiSend = false;
        RegisteredMessageSerializer_SendVersionMessage_CallsEnqueueAndAddToSendQueue();

        socketSender.SendQueued();

        moqMessageSerializer.Verify();
        Assert.IsTrue(calledApiSend);
    }

    [TestMethod]
    [ExpectedException(typeof(SocketSendException))]
    public void QueuedMessageAndSerializer_SendQueued_FailsLogsErrorCode()
    {
        sendCallBack = (_, _, _, _) => -1;
        directOsNetworkingStub = new DirectOSNetworkingStub(() => SendErrorCode, sendCallBack);
        moqNetworkingController.SetupGet(nc => nc.DirectOSNetworkingApi).Returns(directOsNetworkingStub);
        socketSender = new SocketSender(moqSocketSessionContext.Object);

        socketSender.RegisterSerializer(MessageId, moqMessageSerializer.Object);
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
}
