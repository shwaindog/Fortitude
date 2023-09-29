using System;
using System.Linq;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeTests.FortitudeCommon.Chronometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Transports.Sockets.Logging
{
    [TestClass]
    public class SocketDataLatencyLoggerTests
    {
        private SocketDataLatencyLogger testSocketDataLatencyLogger;
        private string instanceName;
        private string baseLoggerNameSpace;
        private ICallStatsLogger startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger;
        private ICallStatsLogger dataDetectedToBeforeSocketReadCallStatsLogger;
        private ICallStatsLogger dataDetectedToAfterSocketReadCallStatsLogger;
        private ICallStatsLogger dataDetectedToEnterDeserializerCallStatsLogger;
        private ICallStatsLogger dataDetectedToPublishCallStatsLogger;
        private ICallStatsLogger beforeSocketReadToAfterSocketReadCallStatsLogger;
        private ICallStatsLogger beforeSocketReadToPublishCallStatsLogger;
        private ICallStatsLogger afterSocketReadToPublishCallStatsLogger;
        private ICallStatsLogger enterDeserializerToPublishCallStatsLogger;

        private Mock<ICallStatsLogger> moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger;
        private Mock<ICallStatsLogger> moqDataDetectedToBeforeSocketReadCallStatsLogger;
        private Mock<ICallStatsLogger> moqDataDetectedToAfterSocketReadCallStatsLogger;
        private Mock<ICallStatsLogger> moqDataDetectedToEnterDeserializerCallStatsLogger;
        private Mock<ICallStatsLogger> moqDataDetectedToPublishCallStatsLogger;
        private Mock<ICallStatsLogger> moqBeforeSocketReadToAfterSocketReadCallStatsLogger;
        private Mock<ICallStatsLogger> moqBeforeSocketReadToPublishCallStatsLogger;
        private Mock<ICallStatsLogger> moqAfterSocketReadToPublishCallStatsLogger;
        private Mock<ICallStatsLogger> moqEnterDeserializerToPublishCallStatsLogger;
        private DateTime baseTime;

        private const string StartDataDectectionToAllSocketUpdatesFinishedFieldName =
            "startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger";
        private const string DataDetectedToBeforeSocketReadFieldName = "dataDetectedToBeforeSocketReadCallStatsLogger";
        private const string DataDetectedToAfterSocketReadFieldName = "dataDetectedToAfterSocketReadCallStatsLogger";
        private const string DataDetectedToEnterDeserializerFieldName =
            "dataDetectedToEnterDeserializerCallStatsLogger";
        private const string DataDetectedToPublishFieldName = "dataDetectedToPublishCallStatsLogger";
        private const string BeforeSocketReadToAfterSocketReadFieldName = 
            "beforeSocketReadToAfterSocketReadCallStatsLogger";
        private const string BeforeSocketReadToPublishFieldName = "beforeSocketReadToPublishCallStatsLogger";
        private const string AfterSocketReadToPublishFieldName = "afterSocketReadToPublishCallStatsLogger";
        private const string EnterDeserializerToPublishFieldName = "enterDeserializerToPublishCallStatsLogger";

        [TestInitialize]
        public void SetUp()
        {
            instanceName = "TestSocketDataLatencyLogger";
            testSocketDataLatencyLogger = new SocketDataLatencyLogger(instanceName);
            baseLoggerNameSpace = typeof(SocketDataLatencyLogger).FullName + "." + instanceName;

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger = new Mock<ICallStatsLogger>();
            moqDataDetectedToBeforeSocketReadCallStatsLogger = new Mock<ICallStatsLogger>();
            moqDataDetectedToAfterSocketReadCallStatsLogger = new Mock<ICallStatsLogger>();
            moqDataDetectedToEnterDeserializerCallStatsLogger = new Mock<ICallStatsLogger>();
            moqDataDetectedToPublishCallStatsLogger = new Mock<ICallStatsLogger>();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger = new Mock<ICallStatsLogger>();
            moqBeforeSocketReadToPublishCallStatsLogger = new Mock<ICallStatsLogger>();
            moqAfterSocketReadToPublishCallStatsLogger = new Mock<ICallStatsLogger>();
            moqEnterDeserializerToPublishCallStatsLogger = new Mock<ICallStatsLogger>();


            baseTime = new DateTime(2018, 1, 24, 21, 02, 11).AddTicks(500_500_0);
        }

        [TestMethod]
        public void NewSocketDataLatencyLogger_New_InitializesCallStatLoggers()
        {
            GetCallStatLoggers();

            Assert.IsNotNull(startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger);
            Assert.IsNotNull(dataDetectedToBeforeSocketReadCallStatsLogger);
            Assert.IsNotNull(dataDetectedToAfterSocketReadCallStatsLogger);
            Assert.IsNotNull(dataDetectedToEnterDeserializerCallStatsLogger);
            Assert.IsNotNull(dataDetectedToPublishCallStatsLogger);
            Assert.IsNotNull(beforeSocketReadToAfterSocketReadCallStatsLogger);
            Assert.IsNotNull(beforeSocketReadToPublishCallStatsLogger);
            Assert.IsNotNull(afterSocketReadToPublishCallStatsLogger);
            Assert.IsNotNull(enterDeserializerToPublishCallStatsLogger);

            Assert.AreEqual(baseLoggerNameSpace + "." + "StartDataDectectionToAllSocketUpdatesFinished",
                startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "DataDetectedToBeforeSocketRead",
                dataDetectedToBeforeSocketReadCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "DataDetectedToAfterSocketRead",
                dataDetectedToAfterSocketReadCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "DataDetectedToEnterDeserializer",
                dataDetectedToEnterDeserializerCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "DataDetectedToPublish",
                dataDetectedToPublishCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "BeforeSocketReadToAfterSocketRead",
                beforeSocketReadToAfterSocketReadCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "BeforeSocketReadToPublish",
                beforeSocketReadToPublishCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "AfterSocketReadToPublish",
                afterSocketReadToPublishCallStatsLogger.FullNameOfLogger);
            Assert.AreEqual(baseLoggerNameSpace + "." + "EnterDeserializerToPublish",
                enterDeserializerToPublishCallStatsLogger.FullNameOfLogger);

            Assert.AreEqual(baseLoggerNameSpace, testSocketDataLatencyLogger.FullNameOfLogger);
        }

        [TestMethod]
        public void NewSocketLatencyLogger_SettingTranslation_ParsesBatchSizeSettingsAsExpected()
        {
            Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
            Assert.IsFalse(testSocketDataLatencyLogger.Enabled);

            foreach (var activationKeyWord in HierarchialConfigurationUpdater.ActivationKeyWords
                .Concat(HierarchialConfigurationUpdater.ActivationKeyWords.Select(ak => ak.ToUpper())))
            {
                testSocketDataLatencyLogger.SettingTranslation(testSocketDataLatencyLogger, activationKeyWord);
                Assert.IsTrue(testSocketDataLatencyLogger.Enabled);
                Assert.AreEqual(CallStatsLogger.DefaultReportAfter, testSocketDataLatencyLogger.BatchSize);

                testSocketDataLatencyLogger.SettingTranslation(testSocketDataLatencyLogger, "disabled");
                Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
                Assert.IsFalse(testSocketDataLatencyLogger.Enabled);
            }

            testSocketDataLatencyLogger.SettingTranslation(testSocketDataLatencyLogger, "1000");
            Assert.AreEqual(1000, testSocketDataLatencyLogger.BatchSize);
            testSocketDataLatencyLogger.SettingTranslation(testSocketDataLatencyLogger, "55555");
            Assert.AreEqual(55555, testSocketDataLatencyLogger.BatchSize);
        }

        [TestMethod]
        public void NewSocketLatencyLogger_DefaultStringValue_EqualsDisabled()
        {
            Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
            Assert.IsFalse(testSocketDataLatencyLogger.Enabled);
            Assert.AreEqual("-1", testSocketDataLatencyLogger.DefaultStringValue);
        }

        [TestMethod]
        public void NewSocketLatencyLogger_Enabled_SetsBatchSizeToDefault()
        {
            Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
            Assert.IsFalse(testSocketDataLatencyLogger.Enabled);

            testSocketDataLatencyLogger.Enabled = true;
            Assert.IsTrue(testSocketDataLatencyLogger.Enabled);
            Assert.AreEqual(CallStatsLogger.DefaultReportAfter, testSocketDataLatencyLogger.BatchSize);

            testSocketDataLatencyLogger.Enabled = false;
            Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
            Assert.IsFalse(testSocketDataLatencyLogger.Enabled);
        }

        [TestMethod]
        public void MockedSocketLatencyLogger_BatchSize_UsesAndSetsUnderlyingLoggersValues()
        {
            Assert.AreEqual(-1, testSocketDataLatencyLogger.BatchSize);
            SetCallStatsToMocks();

            moqDataDetectedToBeforeSocketReadCallStatsLogger.SetupProperty(ddtbsr => ddtbsr.BatchSize, 23_456);
            Assert.AreEqual(23_456, testSocketDataLatencyLogger.BatchSize);

            int expectedNewBatchSize = 45_789;

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.SetupSet(ddtbsr =>
                ddtbsr.BatchSize = expectedNewBatchSize).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqDataDetectedToEnterDeserializerCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqDataDetectedToPublishCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqBeforeSocketReadToPublishCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqAfterSocketReadToPublishCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();
            moqEnterDeserializerToPublishCallStatsLogger.SetupSet(ddtbsr => ddtbsr.BatchSize = expectedNewBatchSize)
                .Verifiable();

            testSocketDataLatencyLogger.BatchSize = expectedNewBatchSize;

            Assert.AreEqual(expectedNewBatchSize, testSocketDataLatencyLogger.BatchSize);

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Verify();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Verify();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Verify();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Verify();
            moqDataDetectedToPublishCallStatsLogger.Verify();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Verify();
            moqBeforeSocketReadToPublishCallStatsLogger.Verify();
            moqAfterSocketReadToPublishCallStatsLogger.Verify();
            moqEnterDeserializerToPublishCallStatsLogger.Verify();
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SocketLatencyLogger_AddCallStat_ThrowsException()
        {
            testSocketDataLatencyLogger.AddCallStat(new CallStat());
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SocketLatencyLogger_AddCallStatTwoDates_ThrowsException()
        {
            testSocketDataLatencyLogger.AddCallStat(DateTime.Now, DateTime.Now);
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SocketLatencyLogger_AddContextMeasurement_ThrowsException()
        {
            testSocketDataLatencyLogger.AddContextMeasurement(1.23456D);
        }

        [TestMethod]
        public void EnabledMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage()
        {
            SetCallStatsToMocks();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.SetupProperty(ddtbsr => ddtbsr.BatchSize, 10_000);

            var expectedFinishTime = baseTime.AddTicks(2_000_540_1);
            var expectedDataDetectionTime = baseTime.AddTicks(2_000_500_1);
            var expectedSocket2BeforePublishTime = baseTime.AddTicks(2_000_536_1);
            var expectedSocket2EnterDeserializerTime = baseTime.AddTicks(2_000_528_1);
            var expectedSocket2AfterSocketReadTime = baseTime.AddTicks(2_000_521_1);
            var expectedSocket2BeforeSocketReadTime = baseTime.AddTicks(2_000_515_1);

            var expectedSocket1BeforePublishTime = baseTime.AddTicks(2_000_510_1);
            var expectedSocket1EnterDeserializerTime = baseTime.AddTicks(2_000_506_1);
            var expectedSocket1AfterSocketReadTime = baseTime.AddTicks(2_000_503_1);
            var expectedSocket1BeforeSocketReadTime = baseTime.AddTicks(2_000_501_1);

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Setup(
                cs => cs.AddCallStat(baseTime, expectedFinishTime)).Verifiable();

            moqDataDetectedToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2BeforePublishTime)).Verifiable();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2EnterDeserializerTime)).Verifiable();
            moqEnterDeserializerToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2EnterDeserializerTime, expectedSocket2BeforePublishTime))
                .Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqAfterSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2AfterSocketReadTime, expectedSocket2BeforePublishTime))
                .Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2BeforeSocketReadTime, expectedSocket2BeforePublishTime))
                .Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2BeforeSocketReadTime, expectedSocket2AfterSocketReadTime))
                .Verifiable();

            moqDataDetectedToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforePublishTime)).Verifiable();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1EnterDeserializerTime)).Verifiable();
            moqEnterDeserializerToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1EnterDeserializerTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqAfterSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1AfterSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1AfterSocketReadTime))
                .Verifiable();

            var traceLog = BuildMultiSocketMatchingPlacesReceive();
            
            var timeStub = new TimeContextTests.StubTimeContext();

            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = expectedFinishTime;
                testSocketDataLatencyLogger.ParseTraceLog(traceLog);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", 
                    new HighPrecisionTimeContext(), true);
            }
            AssertMoqMethodsHit();
        }

        [TestMethod]
        public void EnabledNoDeserializerMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage()
        {
            SetCallStatsToMocks();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.SetupProperty(ddtbsr => ddtbsr.BatchSize, 10_000);

            var expectedFinishTime = baseTime.AddTicks(2_000_523_1);
            var expectedDataDetectionTime = baseTime.AddTicks(2_000_500_1);
            var expectedSocket2AfterSocketReadTime = baseTime.AddTicks(2_000_521_1);
            var expectedSocket2BeforeSocketReadTime = baseTime.AddTicks(2_000_515_1);

            var expectedSocket1BeforePublishTime = baseTime.AddTicks(2_000_510_1);
            var expectedSocket1EnterDeserializerTime = baseTime.AddTicks(2_000_506_1);
            var expectedSocket1AfterSocketReadTime = baseTime.AddTicks(2_000_503_1);
            var expectedSocket1BeforeSocketReadTime = baseTime.AddTicks(2_000_501_1);

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Setup(
                cs => cs.AddCallStat(baseTime, expectedFinishTime)).Verifiable();

            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2BeforeSocketReadTime, expectedSocket2AfterSocketReadTime))
                .Verifiable();

            moqDataDetectedToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforePublishTime)).Verifiable();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1EnterDeserializerTime)).Verifiable();
            moqEnterDeserializerToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1EnterDeserializerTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqAfterSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1AfterSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1AfterSocketReadTime))
                .Verifiable();

            var traceLog = BuildNoDeserializerMultiSocketMatchingPlacesReceive();
            
            var timeStub = new TimeContextTests.StubTimeContext();

            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = expectedFinishTime;
                testSocketDataLatencyLogger.ParseTraceLog(traceLog);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", 
                    new HighPrecisionTimeContext(), true);
            }
            AssertMoqMethodsHit();
        }

        [TestMethod]
        public void EnabledNotAbleToPublishMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage()
        {
            SetCallStatsToMocks();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.SetupProperty(ddtbsr => ddtbsr.BatchSize, 10_000);
            
            var expectedFinishTime = baseTime.AddTicks(2_000_530_1);
            var expectedDataDetectionTime = baseTime.AddTicks(2_000_500_1);
            var expectedSocket2EnterDeserializerTime = baseTime.AddTicks(2_000_528_1);
            var expectedSocket2AfterSocketReadTime = baseTime.AddTicks(2_000_521_1);
            var expectedSocket2BeforeSocketReadTime = baseTime.AddTicks(2_000_515_1);

            var expectedSocket1BeforePublishTime = baseTime.AddTicks(2_000_510_1);
            var expectedSocket1EnterDeserializerTime = baseTime.AddTicks(2_000_506_1);
            var expectedSocket1AfterSocketReadTime = baseTime.AddTicks(2_000_503_1);
            var expectedSocket1BeforeSocketReadTime = baseTime.AddTicks(2_000_501_1);

            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Setup(
                cs => cs.AddCallStat(baseTime, expectedFinishTime)).Verifiable();

            moqDataDetectedToEnterDeserializerCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2EnterDeserializerTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket2BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket2BeforeSocketReadTime, expectedSocket2AfterSocketReadTime))
                .Verifiable();

            moqDataDetectedToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforePublishTime)).Verifiable();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1EnterDeserializerTime)).Verifiable();
            moqEnterDeserializerToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1EnterDeserializerTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1AfterSocketReadTime)).Verifiable();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddContextMeasurement(50)).Verifiable();
            moqAfterSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1AfterSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedDataDetectionTime, expectedSocket1BeforeSocketReadTime)).Verifiable();
            moqBeforeSocketReadToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1BeforePublishTime))
                .Verifiable();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Setup(
                cs => cs.AddCallStat(expectedSocket1BeforeSocketReadTime, expectedSocket1AfterSocketReadTime))
                .Verifiable();

            var traceLog = BuildNotValidForPublishMultiSocketMatchingPlacesReceive();
            
            var timeStub = new TimeContextTests.StubTimeContext();

            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = expectedFinishTime;
                testSocketDataLatencyLogger.ParseTraceLog(traceLog);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", 
                    new HighPrecisionTimeContext(), true);
            }
            AssertMoqMethodsHit();
        }
        
        [TestMethod]
        public void DisabledSocketLatencyLoger_ParseTraceLog_IgnoresContentsLogsNothing()
        {
            SetCallStatsToMocks();

            //not enabled

            var traceLog = BuildMultiSocketMatchingPlacesReceive();
            var expectedFinishTime = baseTime.AddTicks(2_000_540_1);

            var timeStub = new TimeContextTests.StubTimeContext();
            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = expectedFinishTime;
                testSocketDataLatencyLogger.ParseTraceLog(traceLog);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer",
                    new HighPrecisionTimeContext(), true);
            }
            
            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);

            moqDataDetectedToPublishCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqDataDetectedToEnterDeserializerCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqEnterDeserializerToPublishCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqDataDetectedToAfterSocketReadCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqDataDetectedToAfterSocketReadCallStatsLogger.Verify(
                cs => cs.AddContextMeasurement(It.IsAny<double>()), Times.Never);
            moqAfterSocketReadToPublishCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqBeforeSocketReadToPublishCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Verify(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public void Enabled_ParseTraceLogCausesException_LogsExceptionAndContinues()
        {
            SetCallStatsToMocks();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.SetupProperty(ddtbsr => ddtbsr.BatchSize, 10_000);

            moqDataDetectedToPublishCallStatsLogger.Setup(
                cs => cs.AddCallStat(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws<NullReferenceException>()
                .Verifiable();

            Mock<IFLogger> moqLogger = new Mock<IFLogger>();
            moqLogger.Setup(fl => fl.Warn("Error processing PQ latency stats got {0} when processing [{1}]", 
                It.IsAny<NullReferenceException>(), It.IsAny<string>())).Verifiable();

            NonPublicInvocator.SetStaticField(typeof(SocketDataLatencyLogger), "Logger", moqLogger.Object);
            
            var traceLog = BuildMultiSocketMatchingPlacesReceive();

            testSocketDataLatencyLogger.ParseTraceLog(traceLog);

            NonPublicInvocator.SetStaticField(typeof(SocketDataLatencyLogger), "Logger", 
                FLoggerFactory.Instance.GetLogger("DiagnosticSettings"));

            AssertMoqMethodsHit();
            moqLogger.Verify();
        }

        private ITraceLogger BuildMultiSocketMatchingPlacesReceive()
        {
            var timeStub = new TimeContextTests.StubTimeContext();

            var populatedTraceLogger = new TraceLogger("ignored", GetType())
            {
                Enabled = true,
                WriteTrace = false
            };
            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = baseTime;
                populatedTraceLogger.Add(SocketDataLatencyLogger.StartDataDetection);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(2_000_500_1);
                populatedTraceLogger.Add(SocketDataLatencyLogger.SocketDataDetected);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(5);
                populatedTraceLogger.Add("Unexpected Trace Statement");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(5);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "FirstSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 25D);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(10);
                populatedTraceLogger.Add("High outburst of incoming data received read ",
                    34567);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(10);
                populatedTraceLogger.Add("Data detected but not decoded");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(10);
                populatedTraceLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add("New introduced Message");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(50);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "SecondSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(40);
                populatedTraceLogger.Add("Another New introduced Message");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 50D);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(70);
                populatedTraceLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(40);
                populatedTraceLogger.Add("Yet Another New introduced Message");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(40);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer",
                    new HighPrecisionTimeContext(), true);
            }
            return populatedTraceLogger;
        }

        private ITraceLogger BuildNoDeserializerMultiSocketMatchingPlacesReceive()
        {
            var timeStub = new TimeContextTests.StubTimeContext();

            var populatedTraceLogger = new TraceLogger("ignored", GetType())
            {
                Enabled = true,
                WriteTrace = false
            };
            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = baseTime;
                populatedTraceLogger.Add(SocketDataLatencyLogger.StartDataDetection);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(2_000_500_1);
                populatedTraceLogger.Add(SocketDataLatencyLogger.SocketDataDetected);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(10);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "FirstSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 25D);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(30);
                populatedTraceLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(40);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(50);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "SecondSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(60);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 50D);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer",
                    new HighPrecisionTimeContext(), true);
            }
            return populatedTraceLogger;
        }

        private ITraceLogger BuildNotValidForPublishMultiSocketMatchingPlacesReceive()
        {
            var timeStub = new TimeContextTests.StubTimeContext();

            var populatedTraceLogger = new TraceLogger("ignored", GetType())
            {
                Enabled = true,
                WriteTrace = false
            };
            NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer", timeStub, true);
            try
            {
                timeStub.UtcNow = baseTime;
                populatedTraceLogger.Add(SocketDataLatencyLogger.StartDataDetection);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(2_000_500_1);
                populatedTraceLogger.Add(SocketDataLatencyLogger.SocketDataDetected);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(10);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "FirstSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(20);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 25D);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(30);
                populatedTraceLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(40);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforePublish);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(50);
                populatedTraceLogger.Add(SocketDataLatencyLogger.BeforeSocketRead, "SecondSocket");
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(60);
                populatedTraceLogger.Add(SocketDataLatencyLogger.AfterSocketRead, 50D);
                timeStub.UtcNow = timeStub.UtcNow.AddTicks(70);
                populatedTraceLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
            }
            finally
            {
                NonPublicInvocator.SetStaticField(typeof(TraceLogger), "ConsistentTimer",
                    new HighPrecisionTimeContext(), true);
            }
            return populatedTraceLogger;
        }

        private void GetCallStatLoggers()
        {
            startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger =
                NonPublicInvocator.GetInstanceField<CallStatsLogger>(testSocketDataLatencyLogger,
                    StartDataDectectionToAllSocketUpdatesFinishedFieldName);
            dataDetectedToBeforeSocketReadCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, DataDetectedToBeforeSocketReadFieldName);
            dataDetectedToAfterSocketReadCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, DataDetectedToAfterSocketReadFieldName);
            dataDetectedToEnterDeserializerCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, DataDetectedToEnterDeserializerFieldName);
            dataDetectedToPublishCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, DataDetectedToPublishFieldName);
            beforeSocketReadToAfterSocketReadCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, BeforeSocketReadToAfterSocketReadFieldName);
            beforeSocketReadToPublishCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, BeforeSocketReadToPublishFieldName);
            afterSocketReadToPublishCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, AfterSocketReadToPublishFieldName);
            enterDeserializerToPublishCallStatsLogger = NonPublicInvocator.GetInstanceField<CallStatsLogger>(
                testSocketDataLatencyLogger, EnterDeserializerToPublishFieldName);
        }

        private void AssertMoqMethodsHit()
        {
            moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Verify();
            moqDataDetectedToBeforeSocketReadCallStatsLogger.Verify();
            moqDataDetectedToAfterSocketReadCallStatsLogger.Verify();
            moqDataDetectedToEnterDeserializerCallStatsLogger.Verify();
            moqDataDetectedToPublishCallStatsLogger.Verify();
            moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Verify();
            moqBeforeSocketReadToPublishCallStatsLogger.Verify();
            moqAfterSocketReadToPublishCallStatsLogger.Verify();
            moqEnterDeserializerToPublishCallStatsLogger.Verify();
        }

        private void SetCallStatsToMocks()
        {
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, StartDataDectectionToAllSocketUpdatesFinishedFieldName,
                moqStartDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, DataDetectedToBeforeSocketReadFieldName,
                moqDataDetectedToBeforeSocketReadCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, DataDetectedToAfterSocketReadFieldName,
                moqDataDetectedToAfterSocketReadCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, DataDetectedToEnterDeserializerFieldName,
                moqDataDetectedToEnterDeserializerCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, DataDetectedToPublishFieldName,
                moqDataDetectedToPublishCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, BeforeSocketReadToAfterSocketReadFieldName,
                moqBeforeSocketReadToAfterSocketReadCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, BeforeSocketReadToPublishFieldName,
                moqBeforeSocketReadToPublishCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, AfterSocketReadToPublishFieldName, 
                moqAfterSocketReadToPublishCallStatsLogger.Object);
            NonPublicInvocator.SetInstanceField(
                testSocketDataLatencyLogger, EnterDeserializerToPublishFieldName,
                moqEnterDeserializerToPublishCallStatsLogger.Object);
        }
    }
}