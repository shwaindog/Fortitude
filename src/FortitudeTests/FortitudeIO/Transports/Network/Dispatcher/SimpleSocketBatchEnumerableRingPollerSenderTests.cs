#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.State;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Dispatcher;

[TestClass]
public class SimpleSocketBatchEnumerableRingPollerSenderTests
{
    private const uint NoDataPauseTimeout = 100U;
    private List<SimpleSocketSenderPayload> emptyEnumerable = null!;
    private SimpleSocketSenderPayload firstSocketPayload = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelController> moqParallelController = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IEnumerableBatchPollingRing<SimpleSocketSenderPayload>> moqPollingRing = null!;
    private Mock<ISocketSender> moqSocketSender = null!;
    private SimpleSocketSenderPayload secondSocketPayload = null!;
    private SimpleSocketBatchEnumerableRingPollerSender simpleSocketBatchEnumerableRingPollerSender = null!;
    private SimpleSocketSenderPayload thirdSocketPayload = null!;
    private ThreadStart workerThreadMethod = null!;

    [TestInitialize]
    public void SetUp()
    {
        firstSocketPayload = new SimpleSocketSenderPayload();
        secondSocketPayload = new SimpleSocketSenderPayload();
        thirdSocketPayload = new SimpleSocketSenderPayload();
        // ReSharper disable once CollectionNeverUpdated.Local
        emptyEnumerable = new List<SimpleSocketSenderPayload>();
        moqSocketSender = new Mock<ISocketSender>();
        moqPollingRing = new Mock<IEnumerableBatchPollingRing<SimpleSocketSenderPayload>>();
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
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketPayload);
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondSocketPayload);
        moqPollingRing.Setup(pr => pr[2L]).Returns(thirdSocketPayload);
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
        simpleSocketBatchEnumerableRingPollerSender
            = new SimpleSocketBatchEnumerableRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);
        Assert.AreEqual("SimpleSocketRingPollerSenderTests-SocketRingPollerSender", simpleSocketBatchEnumerableRingPollerSender.Name);
    }

    [TestMethod]
    public void NewDispatcherSender_AddToSendQueue_CallsSocketSenderSendQueued()
    {
        moqPollingRing.Setup(pr => pr.StartOfBatch).Returns(true);
        moqPollingRing.Setup(pr => pr.EndOfBatch).Returns(true);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(1);
        var justFirstSocketContainer = new List<SimpleSocketSenderPayload> { firstSocketPayload };
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                    simpleSocketBatchEnumerableRingPollerSender.Stop();
                })
                .Returns(emptyEnumerable.GetEnumerator());
        }).Returns(justFirstSocketContainer.GetEnumerator());
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketPayload).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(0)).Verifiable();
        simpleSocketBatchEnumerableRingPollerSender
            = new SimpleSocketBatchEnumerableRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);

        simpleSocketBatchEnumerableRingPollerSender.Start();
        simpleSocketBatchEnumerableRingPollerSender.EnqueueSocketSender(moqSocketSender.Object);
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
        var justFirstSocketContainer = new List<SimpleSocketSenderPayload> { firstSocketPayload };
        var justSecondSocketContainer = new List<SimpleSocketSenderPayload> { secondSocketPayload };
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                {
                    moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
                        {
                            moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                            simpleSocketBatchEnumerableRingPollerSender.Stop();
                        })
                        .Returns(emptyEnumerable.GetEnumerator());
                })
                .Returns(justSecondSocketContainer.GetEnumerator());
        }).Returns(justFirstSocketContainer.GetEnumerator());
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketPayload).Verifiable();
        moqPollingRing.Setup(pr => pr[1L]).Returns(secondSocketPayload).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(0)).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(1)).Verifiable();

        moqSocketSender.SetupSequence(ss => ss.SendQueued()).Returns(false).Returns(true);
        simpleSocketBatchEnumerableRingPollerSender
            = new SimpleSocketBatchEnumerableRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);

        simpleSocketBatchEnumerableRingPollerSender.Start();
        simpleSocketBatchEnumerableRingPollerSender.EnqueueSocketSender(moqSocketSender.Object);
        workerThreadMethod();

        moqPollingRing.Verify();
    }

    [TestMethod]
    public void NewDispatcherSender_SocketSenderSendQueuedThrowsSocketSendException_CallsOnSessionFailure()
    {
        moqPollingRing.Setup(pr => pr.StartOfBatch).Returns(true);
        moqPollingRing.Setup(pr => pr.EndOfBatch).Returns(true);
        moqPollingRing.SetupGet(pr => pr.CurrentBatchSize).Returns(1);
        var justFirstSocketContainer = new List<SimpleSocketSenderPayload> { firstSocketPayload };
        moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
        {
            moqPollingRing.Setup(pr => pr.GetEnumerator()).Callback(() =>
            {
                moqPollingRing.Setup(pr => pr.GetEnumerator()).Returns(emptyEnumerable.GetEnumerator());
                simpleSocketBatchEnumerableRingPollerSender.Stop();
            }).Returns(emptyEnumerable.GetEnumerator());
        }).Returns(justFirstSocketContainer.GetEnumerator());
        moqPollingRing.Setup(pr => pr[0L]).Returns(firstSocketPayload).Verifiable();
        moqPollingRing.Setup(pr => pr.Publish(0)).Verifiable();

        var moqSocketSessionContext = new Mock<ISocketSessionContext>();
        moqSocketSender.Setup(ss => ss.SendQueued()).Throws(new SocketSendException("Something bad has happened", moqSocketSessionContext.Object));
        moqSocketSessionContext.Setup(ssc => ssc.OnSessionFailure(It.IsAny<string>())).Verifiable();

        simpleSocketBatchEnumerableRingPollerSender
            = new SimpleSocketBatchEnumerableRingPollerSender(moqPollingRing.Object, NoDataPauseTimeout, null, moqParallelController.Object);

        simpleSocketBatchEnumerableRingPollerSender.Start();
        simpleSocketBatchEnumerableRingPollerSender.EnqueueSocketSender(moqSocketSender.Object);
        workerThreadMethod();

        moqPollingRing.Verify();
    }
}
