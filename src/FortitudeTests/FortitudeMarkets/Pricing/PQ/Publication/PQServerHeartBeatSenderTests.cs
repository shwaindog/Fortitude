// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQServerHeartBeatSenderTests
{
    private Mock<IOSParallelController>        moqOsParallelController      = null!;
    private Mock<IOSThread>                    moqOsThread                  = null!;
    private Mock<IOSParallelControllerFactory> moqParallelControllerFactory = null!;

    private Mock<IDoublyLinkedList<IPQMessage>> moqQuotesList = null!;

    private Mock<ISyncLock>                       moqSyncLock             = null!;
    private Mock<IPQUpdateServer>                 moqUpdateServer         = null!;
    private PQServerHeartBeatSender               pqServerHeartBeatSender = null!;
    private PQTickInstantTests.DummyPQTickInstant tickInstant1            = null!;
    private PQTickInstantTests.DummyPQTickInstant tickInstant2            = null!;
    private PQTickInstantTests.DummyPQTickInstant tickInstant3            = null!;

    [TestInitialize]
    public void SetUp()
    {
        tickInstant1 = new PQTickInstantTests.DummyPQTickInstant();
        tickInstant2 = new PQTickInstantTests.DummyPQTickInstant();
        tickInstant3 = new PQTickInstantTests.DummyPQTickInstant();

        moqParallelControllerFactory = new Mock<IOSParallelControllerFactory>();
        moqOsParallelController      = new Mock<IOSParallelController>();
        moqParallelControllerFactory.SetupGet(pcf => pcf.GetOSParallelController)
                                    .Returns(moqOsParallelController.Object);

        OSParallelControllerFactory.Instance = moqParallelControllerFactory.Object;

        moqOsThread     = new Mock<IOSThread>();
        moqUpdateServer = new Mock<IPQUpdateServer>();
        moqSyncLock     = new Mock<ISyncLock>();
        moqQuotesList   = new Mock<IDoublyLinkedList<IPQMessage>>();

        moqOsParallelController.Setup(opc => opc.CreateNewOSThread(It.IsAny<ThreadStart>()))
                               .Returns(moqOsThread.Object)
                               .Verifiable();
        moqOsThread.SetupSet(ot => ot.IsBackground = true).Verifiable();
        moqOsThread.Setup(ot => ot.Start()).Verifiable();

        pqServerHeartBeatSender = new PQServerHeartBeatSender
        {
            UpdateServer       = moqUpdateServer.Object, ServerLinkedLock = moqSyncLock.Object
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
                          .Returns(baseTime)                        //original lastRun
                          .Returns(baseTime.AddMilliseconds(1001))  //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
                          .Returns(baseTime.AddMilliseconds(1002))  //1st set new last published
                          .Returns(baseTime.AddMilliseconds(1003))  //2nd quote check last published
                          .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                       .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            tickInstant1.LastPublicationTime = baseTime.AddMilliseconds(-2);
            tickInstant2.LastPublicationTime = baseTime.AddMilliseconds(-1);
            tickInstant3.LastPublicationTime = baseTime.AddMilliseconds(500);

            var realQuoteList = new DoublyLinkedList<IPQMessage>();
            realQuoteList.AddLast(tickInstant3);
            realQuoteList.AddFirst(tickInstant2);
            realQuoteList.AddFirst(tickInstant1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realQuoteList;

            moqUpdateServer.SetupGet(us => us.IsStarted).Returns(true);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                           .Callback<IVersionedMessage>(vmsg =>
                           {
                               var hbMsg = vmsg as IPQHeartBeatQuotesMessage;
                               Assert.IsNotNull(hbMsg);
                               Assert.AreEqual(2, hbMsg.QuotesToSendHeartBeats.Count);
                               Assert.AreEqual(tickInstant1, hbMsg.QuotesToSendHeartBeats[0]);
                               Assert.AreEqual(tickInstant2, hbMsg.QuotesToSendHeartBeats[1]);
                           }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsStarted, Times.Exactly(1));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(1));

            Assert.AreEqual(tickInstant3, realQuoteList.Head);
            Assert.AreEqual(tickInstant2, realQuoteList.Tail);
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
                          .Returns(baseTime)                        //original lastRun
                          .Returns(baseTime.AddMilliseconds(1001))  //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
                          .Returns(baseTime.AddMilliseconds(1002))  //1st set new last published
                          .Returns(baseTime.AddMilliseconds(1003))  //2nd quote check last published
                          .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                       .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            tickInstant1.LastPublicationTime = baseTime.AddMilliseconds(800);
            tickInstant2.LastPublicationTime = baseTime.AddMilliseconds(851);
            tickInstant3.LastPublicationTime = baseTime.AddMilliseconds(900);

            var realQuoteList = new DoublyLinkedList<IPQMessage>();
            realQuoteList.AddLast(tickInstant3);
            realQuoteList.AddFirst(tickInstant2);
            realQuoteList.AddFirst(tickInstant1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realQuoteList;

            moqUpdateServer.SetupGet(us => us.IsStarted).Returns(true);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                           .Callback<IVersionedMessage>(vmsg =>
                           {
                               var hbMsg = vmsg as IPQHeartBeatQuotesMessage;
                               Assert.IsNotNull(hbMsg);
                               Assert.AreEqual(2, hbMsg.QuotesToSendHeartBeats.Count);
                               Assert.AreEqual(tickInstant1, hbMsg.QuotesToSendHeartBeats[0]);
                               Assert.AreEqual(tickInstant2, hbMsg.QuotesToSendHeartBeats[1]);
                           }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsStarted, Times.Exactly(0));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(0));

            Assert.AreEqual(tickInstant1, realQuoteList.Head);
            Assert.AreEqual(tickInstant3, realQuoteList.Tail);
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
                          .Returns(baseTime)                        //original lastRun
                          .Returns(baseTime.AddMilliseconds(1001))  //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
                          .Returns(baseTime.AddMilliseconds(1002))  //1st set new last published
                          .Returns(baseTime.AddMilliseconds(1003))  //2nd quote check last published
                          .Returns(baseTime.AddMilliseconds(1003)); //2nd quote new last published

            moqSyncLock.Setup(sl => sl.Acquire());
            moqOsThread.Setup(ot => ot.Join());
            moqSyncLock.Setup(sl => sl.Release())
                       .Callback(() => { pqServerHeartBeatSender.StopAndWaitUntilFinished(); });

            tickInstant1.LastPublicationTime = baseTime.AddMilliseconds(-5);
            tickInstant2.LastPublicationTime = baseTime.AddMilliseconds(-4);
            tickInstant3.LastPublicationTime = baseTime.AddMilliseconds(900);

            var realTickInstantList = new DoublyLinkedList<IPQMessage>();
            realTickInstantList.AddLast(tickInstant3);
            realTickInstantList.AddFirst(tickInstant2);
            realTickInstantList.AddFirst(tickInstant1);
            pqServerHeartBeatSender.ServerLinkedQuotes = realTickInstantList;

            moqUpdateServer.SetupGet(us => us.IsStarted).Returns(false);
            moqUpdateServer.Setup(us => us.Send(It.IsAny<IVersionedMessage>()))
                           .Callback<IVersionedMessage>(vmsg =>
                           {
                               var hbMsg = vmsg as IPQHeartBeatQuotesMessage;
                               Assert.IsNotNull(hbMsg);
                               Assert.AreEqual(2, hbMsg.QuotesToSendHeartBeats.Count);
                               Assert.AreEqual(tickInstant1, hbMsg.QuotesToSendHeartBeats[0]);
                               Assert.AreEqual(tickInstant2, hbMsg.QuotesToSendHeartBeats[1]);
                           }).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqUpdateServer.Verify(us => us.IsStarted, Times.Exactly(1));
            moqUpdateServer.Verify(us => us.Send(It.IsAny<IVersionedMessage>()), Times.Exactly(0));

            Assert.AreEqual(tickInstant3, realTickInstantList.Head);
            Assert.AreEqual(tickInstant2, realTickInstantList.Tail);
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
                          .Returns(baseTime)                        //original lastRun
                          .Returns(baseTime.AddMilliseconds(1001))  //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
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

            var moqTickInstantQuote = new Mock<IPQMessage>();
            moqTickInstantQuote.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                               .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                               .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqTickInstantQuote.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                               .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqTickInstantQuote.Object)
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqTickInstantQuote.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqTickInstantQuote.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            Assert.AreNotSame(OSParallelControllerFactory.Instance, moqOsParallelController);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);

            Assert.AreNotSame(OSParallelControllerFactory.Instance, moqOsParallelController);
            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqQuotesList.Verify();
            moqTickInstantQuote.Verify();
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
                          .Returns(baseTime.AddMilliseconds(1001))  //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
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

            var moqTickInstant = new Mock<IPQMessage>();
            moqTickInstant.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                          .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                          .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqTickInstant.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                          .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqTickInstant.Object)
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqTickInstant.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqTickInstant.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);

            var moqLogger = new Mock<IFLogger>();
            moqLogger.Setup(l => l.Error(It.IsAny<string>(), It.IsAny<object[]>())).Verifiable();
            NonPublicInvocator.SetStaticField(pqServerHeartBeatSender, "Logger", moqLogger.Object);

            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqQuotesList.Verify();
            moqTickInstant.Verify();
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
                          .Returns(baseTime)                        //original lastRun
                          .Returns(baseTime.AddMilliseconds(1))     //checktime since lastRun
                          .Returns(baseTime.AddMilliseconds(1002))  //new lastrun
                          .Returns(baseTime.AddMilliseconds(1002))  //1st quote check last published
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

            var moqTickInstant = new Mock<IPQMessage>();
            moqTickInstant.SetupGet(lv0Q => lv0Q.LastPublicationTime)
                          .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock))
                          .Returns(baseTime.AddMilliseconds(-5)).Verifiable();
            moqTickInstant.SetupSet(lv0Q => lv0Q.LastPublicationTime = baseTime.AddMilliseconds(1002))
                          .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();

            moqQuotesList.SetupGet(ql => ql.Head).Returns(moqTickInstant.Object)
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Verifiable();
            moqQuotesList.Setup(ql => ql.Remove(moqTickInstant.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();
            moqQuotesList.Setup(ql => ql.AddLast(moqTickInstant.Object))
                         .Callback(() => Assert.IsTrue(isInHeartBeatSyncLock)).Returns(tickInstant1).Verifiable();

            moqOsParallelController.Setup(opc => opc.Sleep(999)).Verifiable();

            Assert.IsFalse(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.StartSendingHeartBeats();
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);
            
            Assert.IsTrue(pqServerHeartBeatSender.HasStarted);
            pqServerHeartBeatSender.CheckPublishHeartbeats();
            moqOsParallelController.Verify();
            moqQuotesList.Verify();
            moqTickInstant.Verify();
        }
        finally
        {
            TimeContext.Provider = preTestTimeContext;
        }
    }
}
