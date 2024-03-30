#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.NewSocketAPI.Dispatcher;
using FortitudeIO.Transports.NewSocketAPI.Publishing;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.Dispatcher;

[TestClass]
public class SimpleSocketRingPollerSenderTests
{
    private const uint NoDataPauseTimeout = 100U;
    private List<SocketSenderContainer> emptyEnumerable = null!;
    private SocketSenderContainer firstSocketContainer = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IPollingRing<SocketSenderContainer>> moqPollingRing = null!;
    private Mock<ISocketSender> moqSocketSender = null!;
    private SocketSenderContainer secondSocketContainer = null!;
    private SimpleSocketRingPollerSender simpleSocketRingPollerSender = null!;
    private SocketSenderContainer thirdSocketContainer = null!;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        firstSocketContainer = new SocketSenderContainer();
        secondSocketContainer = new SocketSenderContainer();
        thirdSocketContainer = new SocketSenderContainer();
        // ReSharper disable once CollectionNeverUpdated.Local
        emptyEnumerable = new List<SocketSenderContainer>();
        moqSocketSender = new Mock<ISocketSender>();
        moqPollingRing = new Mock<IPollingRing<SocketSenderContainer>>();
        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqOsThread = new Mock<IOSThread>();
        moqParallelController.Setup(pc => pc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(workerMethod => { workerThreadMethod = workerMethod; })
            .Returns(moqOsThread.Object).Verifiable();

        moqSocketSender.Setup(ss => ss.SendQueued()).Returns(true).Verifiable();

        moqPollingRing.Setup(pr => pr.Name).Returns("SimpleSocketRingPollerSenderTests");
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketContainer);
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondSocketContainer);
        moqPollingRing.Setup(pr => pr[2L]).Returns(thirdSocketContainer);
        moqPollingRing.SetupSequence(pr => pr.Claim()).Returns(0).Returns(1).Returns(2);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(3);
        moqPollingRing.SetupSequence(pr => pr.StartOfBatch).Returns(true).Returns(false).Returns(false);
        moqPollingRing.SetupSequence(pr => pr.EndOfBatch).Returns(false).Returns(false).Returns(true);
        moqPollingRing.SetupSequence(pr => pr.CurrentSequence).Returns(0).Returns(1).Returns(2);
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NewRingPoller_New_SetsNameWithSocketRingPollerSender()
    {
        simpleSocketRingPollerSender
            = new SimpleSocketRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);
        Assert.AreEqual("SimpleSocketRingPollerSenderTests-SocketRingPollerSender", simpleSocketRingPollerSender.Name);
    }

    [TestMethod]
    public void NewDispatcherSender_AddToSendQueue_CallsSocketSenderSendQueued()
    {
        moqPollingRing.Setup(pr => pr.StartOfBatch).Returns(true);
        moqPollingRing.Setup(pr => pr.EndOfBatch).Returns(true);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(1);
        var justFirstSocketContainer = new List<SocketSenderContainer> { firstSocketContainer };
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    simpleSocketRingPollerSender.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(justFirstSocketContainer.GetEnumerator());
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketContainer).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(0)).Verifiable();
        simpleSocketRingPollerSender
            = new SimpleSocketRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);

        simpleSocketRingPollerSender.Start();
        simpleSocketRingPollerSender.AddToSendQueue(moqSocketSender.Object);
        workerThreadMethod();

        moqPollingRing.Verify();
        moqSocketSender.Verify();
    }

    [TestMethod]
    public void NewDispatcherSender_SocketSenderSendQueuedFails_AddsSocketSenderBackIntoTheQueue()
    {
        moqPollingRing.Setup(pr => pr.StartOfBatch).Returns(true);
        moqPollingRing.Setup(pr => pr.EndOfBatch).Returns(true);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(1);
        var justFirstSocketContainer = new List<SocketSenderContainer> { firstSocketContainer };
        var justSecondSocketContainer = new List<SocketSenderContainer> { secondSocketContainer };
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                        {
                            moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                            simpleSocketRingPollerSender.Stop();
                        })
                        .Returns(emptyEnumerable.GetEnumerator());
                })
                .Returns(justSecondSocketContainer.GetEnumerator());
        }).Returns(justFirstSocketContainer.GetEnumerator());
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketContainer).Verifiable();
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondSocketContainer).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(0)).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(1)).Verifiable();

        moqSocketSender.SetupSequence(ss => ss.SendQueued()).Returns(false).Returns(true);
        simpleSocketRingPollerSender
            = new SimpleSocketRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);

        simpleSocketRingPollerSender.Start();
        simpleSocketRingPollerSender.AddToSendQueue(moqSocketSender.Object);
        workerThreadMethod();

        moqPollingRing.Verify();
    }
}
