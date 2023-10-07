#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsCore.Pricing.PQ;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQServerHeartBeatSenderTests
{
    private PQLevel0QuoteTests.DummyPQLevel0Quote level0Quote1 = null!;
    private PQLevel0QuoteTests.DummyPQLevel0Quote level0Quote2 = null!;
    private PQLevel0QuoteTests.DummyPQLevel0Quote level0Quote3 = null!;
    private Mock<IOSParallelController> moqOsParallelController = null!;
    private Mock<IOSThread> moqOsThread = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;
    private Mock<IDoublyLinkedList<IPQLevel0Quote>> moqQuotesList = null!;
    private Mock<ISyncLock> moqSyncLock = null!;
    private Mock<IPQUpdateServer> moqUpdateServer = null!;
    private PQServerHeartBeatSender pqServerHeartBeatSender = null!;

    [TestInitialize]
    public void SetUp()
    {
        level0Quote1 = new PQLevel0QuoteTests.DummyPQLevel0Quote();
        level0Quote2 = new PQLevel0QuoteTests.DummyPQLevel0Quote();
        level0Quote3 = new PQLevel0QuoteTests.DummyPQLevel0Quote();

        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqOsParallelController = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
            .Returns(moqOsParallelController.Object);
        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;
        moqOsThread = new Mock<IOSThread>();
        moqUpdateServer = new Mock<IPQUpdateServer>();
        moqSyncLock = new Mock<ISyncLock>();
        moqQuotesList = new Mock<IDoublyLinkedList<IPQLevel0Quote>>();
        moqOsParallelController.Setup(opc => opc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Returns(moqOsThread.Object)
            .Verifiable();
        moqOsThread.SetupSet(ot => ot.IsBackground = true).Verifiable();
        moqOsThread.Setup(ot => ot.Start()).Verifiable();

        pqServerHeartBeatSender = new PQServerHeartBeatSender
        {
            UpdateServer = moqUpdateServer.Object, ServerLinkedLock = moqSyncLock.Object
            , ServerLinkedQuotes = moqQuotesList.Object
        };
    }

    [TestCleanup]
    public void TearDown()
    {
        OSParallelControllerFactory.Instance = new OSParallelControllerFactory();
    }

    [TestMethod]
    public void NewHeartBeatSender_StartServices_LaunchesNewThreadAsBackground()
    {
        moqOsParallelController.Setup(opc => opc.CreateNewOSThread(It.IsAny<ThreadStart>()))
            .Callback<ThreadStart>(threadStart =>
                Assert.AreEqual(pqServerHeartBeatSender.CheckPublishHeartbeats, threadStart))
            .Returns(moqOsThread.Object).Verifiable();
        Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
        pqServerHeartBeatSender.StartSendingHeartBeats();
        moqOsParallelController.Verify();
        moqOsThread.Verify();
        Assert.IsTrue(pqServerHeartBeatSender.HasStarted);
    }

    [TestMethod]
    public void StartedHeartBeatSender_StopAndWaitUntilFinished_WaitsForBackgroundThreadToFinish()
    {
        moqOsThread.Setup(ot => ot.Join()).Verifiable();

        Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
        pqServerHeartBeatSender.StartSendingHeartBeats();
        Assert.IsTrue(pqServerHeartBeatSender.HasStarted);
        pqServerHeartBeatSender.StopAndWaitUntilFinished();
        moqOsThread.Verify();
        Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
    }

    [TestMethod]
    public void StartedHeartBeatSender_CheckPublishHeartBeats_PublishesHeartBeatToTwoQuotesOverTolerance()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Returns(baseTime.AddMilliseconds(1001)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)) //1st set new last published
                .Returns(baseTime.AddMilliseconds(1003)) //2nd quote check last published
                .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            level0Quote1.LastPublicationTime = baseTime.AddMilliseconds(-2);
            level0Quote2.LastPublicationTime = baseTime.AddMilliseconds(-1);
            level0Quote3.LastPublicationTime = baseTime.AddMilliseconds(500);

            var realQuoteList = new DoublyLinkedList<IPQLevel0Quote>();
            realQuoteList.AddLast(level0Quote3);
            realQuoteList.AddFirst(level0Quote2);
            realQuoteList.AddFirst(level0Quote1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realQuoteList;

            moqUpdateServer.SetupGet(us => us.IsConnected).Returns(true);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                .Callback<object>(listObj =>
                {
                    var list = listObj as List<IPQLevel0Quote>;
                    Assert.IsNotNull(list);
                    Assert.AreEqual(2, list.Count);
                    Assert.AreEqual(level0Quote1, list[0]);
                    Assert.AreEqual(level0Quote2, list[1]);
                }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsConnected, Times.Exactly(1));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(1));

            Assert.AreEqual(level0Quote3, realQuoteList.Head);
            Assert.AreEqual(level0Quote2, realQuoteList.Tail);
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }


    [TestMethod]
    public void StartedHeartBeatSenderAllQuotesJustPublished_CheckPublishHeartBeats_PublishesNothing()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Returns(baseTime.AddMilliseconds(1001)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)) //1st set new last published
                .Returns(baseTime.AddMilliseconds(1003)) //2nd quote check last published
                .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            level0Quote1.LastPublicationTime = baseTime.AddMilliseconds(800);
            level0Quote2.LastPublicationTime = baseTime.AddMilliseconds(851);
            level0Quote3.LastPublicationTime = baseTime.AddMilliseconds(900);

            var realQuoteList = new DoublyLinkedList<IPQLevel0Quote>();
            realQuoteList.AddLast(level0Quote3);
            realQuoteList.AddFirst(level0Quote2);
            realQuoteList.AddFirst(level0Quote1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realQuoteList;

            moqUpdateServer.SetupGet(us => us.IsConnected).Returns(true);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                .Callback<object>(listObj =>
                {
                    var list = listObj as List<IPQLevel0Quote>;
                    Assert.IsNotNull(list);
                    Assert.AreEqual(2, list.Count);
                    Assert.AreEqual(level0Quote1, list[0]);
                    Assert.AreEqual(level0Quote2, list[1]);
                }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsConnected, Times.Exactly(0));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(0));

            Assert.AreEqual(level0Quote1, realQuoteList.Head);
            Assert.AreEqual(level0Quote3, realQuoteList.Tail);
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }

    [TestMethod]
    public void StartedHeartBeatSenderDisconnectedUpdateServer_CheckPublishHeartBeats_PublishesNothing()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Returns(baseTime.AddMilliseconds(1001)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)) //1st set new last published
                .Returns(baseTime.AddMilliseconds(1003)) //2nd quote check last published
                .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            level0Quote1.LastPublicationTime = baseTime.AddMilliseconds(-5);
            level0Quote2.LastPublicationTime = baseTime.AddMilliseconds(-4);
            level0Quote3.LastPublicationTime = baseTime.AddMilliseconds(900);

            var realQuoteList = new DoublyLinkedList<IPQLevel0Quote>();
            realQuoteList.AddLast(level0Quote3);
            realQuoteList.AddFirst(level0Quote2);
            realQuoteList.AddFirst(level0Quote1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realQuoteList;

            moqUpdateServer.SetupGet(us => us.IsConnected).Returns(false);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                .Callback<object>(listObj =>
                {
                    var list = listObj as List<IPQLevel0Quote>;
                    Assert.IsNotNull(list);
                    Assert.AreEqual(2, list.Count);
                    Assert.AreEqual(level0Quote1, list[0]);
                    Assert.AreEqual(level0Quote2, list[1]);
                }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsConnected, Times.Exactly(1));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(0));

            Assert.AreEqual(level0Quote3, realQuoteList.Head);
            Assert.AreEqual(level0Quote2, realQuoteList.Tail);
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }

    [TestMethod]
    public void StartedHeartBeatSender_CheckPublishHeartBeats_ProtectsUpdatesToQuotesList()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Returns(baseTime.AddMilliseconds(1001)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)); //1st set new last published

            moqOsThread.Setup(ot => ot.Join());
            var isInHeartBeatSyncLock = false;

            moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => isInHeartBeatSyncLock = true).Verifiable();
            moqSyncLock.Setup(sl => sl.Release()).Callback(() =>
            {
                isInHeartBeatSyncLock = false;
                pqServerHeartBeatSender.StopAndWaitUntilFinished();
            }).Verifiable();

            moqQuotesList.SetupGet(ql => ql.IsEmpty).Returns(false).Verifiable();

            var moqLevel0Quote = new Mock<IPQLevel0Quote>();
            moqLevel0Quote.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqLevel0Quote.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqLevel0Quote.Object)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqQuotesList.Verify();
            moqLevel0Quote.Verify();
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }

    [TestMethod]
    public void StartedHeartBeatSender_CheckPublishHeartBeats_RecoversAfterExceptionToPublish()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Throws(new Exception("Recover from this will you?"))
                .Returns(baseTime.AddMilliseconds(1001)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)); //1st set new last published

            moqOsThread.Setup(ot => ot.Join());
            var isInHeartBeatSyncLock = false;

            moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => isInHeartBeatSyncLock = true).Verifiable();
            moqSyncLock.Setup(sl => sl.Release()).Callback(() =>
            {
                isInHeartBeatSyncLock = false;
                pqServerHeartBeatSender.StopAndWaitUntilFinished();
            }).Verifiable();

            moqQuotesList.SetupGet(ql => ql.IsEmpty).Returns(false).Verifiable();

            var moqLevel0Quote = new Mock<IPQLevel0Quote>();
            moqLevel0Quote.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqLevel0Quote.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqLevel0Quote.Object)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            var moqLogger = new Mock<IFLogger>();
            moqLogger.Setup(l => l.Error(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();
            NonPublicInvocator.SetStaticField(pqServerHeartBeatSender, "Logger", moqLogger.Object);

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqQuotesList.Verify();
            moqLevel0Quote.Verify();
            moqLogger.Verify();
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }


    [TestMethod]
    public void StartedHeartBeatSenderJustChecked_CheckPublishHeartBeats_PausesUntilNextCheckTime()
    {
        var preTestTimeContext = TimeContext.Provider;
        try
        {
            var moqTimeContext = new Mock<ITimeContext>();
            TimeContext.Provider = moqTimeContext.Object;

            var baseTime = new DateTime(2017, 03, 18, 19, 42, 00);

            moqTimeContext.SetupSequence(tc => tc.UtcNow)
                .Returns(baseTime) //original lastRun
                .Returns(baseTime.AddMilliseconds(1)) //checktime since lastRun
                .Returns(baseTime.AddMilliseconds(1002)) //new lastrun
                .Returns(baseTime.AddMilliseconds(1002)) //1st quote check last published
                .Returns(baseTime.AddMilliseconds(1002)); //1st set new last published

            moqOsThread.Setup(ot => ot.Join());
            var isInHeartBeatSyncLock = false;

            moqSyncLock.Setup(sl => sl.Acquire()).Callback(() => isInHeartBeatSyncLock = true).Verifiable();
            moqSyncLock.Setup(sl => sl.Release()).Callback(() =>
            {
                isInHeartBeatSyncLock = false;
                pqServerHeartBeatSender.StopAndWaitUntilFinished();
            }).Verifiable();

            moqQuotesList.SetupGet(ql => ql.IsEmpty).Returns(false).Verifiable();

            var moqLevel0Quote = new Mock<IPQLevel0Quote>();
            moqLevel0Quote.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqLevel0Quote.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqLevel0Quote.Object)
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqLevel0Quote.Object))
                .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(level0Quote1).Verifiable();

            moqOsParallelController.Setup(opc => opc.Sleep(999)).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqOsParallelController.Verify();
            moqQuotesList.Verify();
            moqLevel0Quote.Verify();
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }
}
