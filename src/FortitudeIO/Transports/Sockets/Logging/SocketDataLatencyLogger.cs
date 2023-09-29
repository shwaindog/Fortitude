using System;
using System.Globalization;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

namespace FortitudeIO.Transports.Sockets.Logging
{
    public class SocketDataLatencyLogger : ISocketDataLatencyLogger
    {
        public const string StartDataDetection = "StartDataDetection";
        public const string SocketDataDetected = "SocketDataDetected";
        public const string BeforeSocketRead = "BeforeSocketRead";
        public const string AfterSocketRead = "AfterSocketRead";
        public const string EnterDeserializer = "EnterDeserializer";
        public const string BeforePublish = "BeforePublish";

        private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");
        private readonly ICallStatsLogger startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger;
        private readonly ICallStatsLogger dataDetectedToBeforeSocketReadCallStatsLogger;
        private readonly ICallStatsLogger dataDetectedToAfterSocketReadCallStatsLogger;
        private readonly ICallStatsLogger dataDetectedToEnterDeserializerCallStatsLogger;
        private readonly ICallStatsLogger dataDetectedToPublishCallStatsLogger;
        private readonly ICallStatsLogger beforeSocketReadToAfterSocketReadCallStatsLogger;
        private readonly ICallStatsLogger beforeSocketReadToPublishCallStatsLogger;
        private readonly ICallStatsLogger afterSocketReadToPublishCallStatsLogger;
        private readonly ICallStatsLogger enterDeserializerToPublishCallStatsLogger;
        private readonly string name;
        
        public SocketDataLatencyLogger(string name)
        {
            this.name = name;
            startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("StartDataDectectionToAllSocketUpdatesFinished", FullNameOfLogger);
            dataDetectedToBeforeSocketReadCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("DataDetectedToBeforeSocketRead", FullNameOfLogger);
            dataDetectedToAfterSocketReadCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("DataDetectedToAfterSocketRead", FullNameOfLogger);
            dataDetectedToEnterDeserializerCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("DataDetectedToEnterDeserializer", FullNameOfLogger);
            dataDetectedToPublishCallStatsLogger = 
                CallStatsLogger.GetCallStatsLogger("DataDetectedToPublish", FullNameOfLogger);
            beforeSocketReadToAfterSocketReadCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("BeforeSocketReadToAfterSocketRead", FullNameOfLogger);
            beforeSocketReadToPublishCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("BeforeSocketReadToPublish", FullNameOfLogger);
            afterSocketReadToPublishCallStatsLogger =
                CallStatsLogger.GetCallStatsLogger("AfterSocketReadToPublish", FullNameOfLogger);
            enterDeserializerToPublishCallStatsLogger = 
                CallStatsLogger.GetCallStatsLogger( "EnterDeserializerToPublish", FullNameOfLogger);

            HierarchialLoggingConfigurator<ICallStatsLogger>.Register(this);
        }

        public string FullNameOfLogger => GetType().FullName + "." + name;

        public Action<IHierarchialLogger, string> SettingTranslation
            => dataDetectedToBeforeSocketReadCallStatsLogger.SettingTranslation;

        public string DefaultStringValue => (-1).ToString(CultureInfo.InvariantCulture);

        public bool Enabled
        {
            get => BatchSize > 1;
            set => BatchSize = value ? CallStatsLogger.DefaultReportAfter : -1;
        }

        public int BatchSize
        {
            get => dataDetectedToBeforeSocketReadCallStatsLogger.BatchSize;
            set
            {
                startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.BatchSize = value;
                dataDetectedToBeforeSocketReadCallStatsLogger.BatchSize = value;
                dataDetectedToAfterSocketReadCallStatsLogger.BatchSize = value;
                dataDetectedToEnterDeserializerCallStatsLogger.BatchSize = value;
                dataDetectedToPublishCallStatsLogger.BatchSize = value;
                beforeSocketReadToAfterSocketReadCallStatsLogger.BatchSize = value;
                beforeSocketReadToPublishCallStatsLogger.BatchSize = value;
                afterSocketReadToPublishCallStatsLogger.BatchSize = value;
                enterDeserializerToPublishCallStatsLogger.BatchSize = value;
            }
        }

        public void AddCallStat(CallStat lastCallStat)
        {
            throw new NotSupportedException("Do not call this for this class");
        }

        public void AddCallStat(DateTime startTime, DateTime stopTime)
        {
            throw new NotSupportedException("Do not call this for this class");
        }

        public void AddContextMeasurement(double measurement)
        {
            throw new NotSupportedException("Do not call this for this class");
        }

        public void ParseTraceLog(ITraceLogger pqTraceLog)
        {
            if (!Enabled) return;
            try
            {
                var wellKnownTraceEntries = pqTraceLog.TraceFinished();
                if (wellKnownTraceEntries.Count <= 0) return;
                var startDataDection = wellKnownTraceEntries[0];
                var startDataDectionTime = startDataDection.Time;
                var dataDetectedEntry = wellKnownTraceEntries[1];
                if (dataDetectedEntry.Identifier != SocketDataDetected) return;
                var timeDataDetected = dataDetectedEntry.Time;
                var lastParsedPublishDate = DateTime.MinValue;
                var hasPublishTime = false;
                var lastAfterSocketEntry = new TraceEntry();
                for (var currentIndex = wellKnownTraceEntries.Count - 1; currentIndex > 1; currentIndex--)
                {
                    var currentTraceEntry = wellKnownTraceEntries[currentIndex];
                    switch (currentTraceEntry.Identifier)
                    {
                        case BeforePublish:
                            hasPublishTime = true;
                            lastParsedPublishDate = currentTraceEntry.Time;
                            dataDetectedToPublishCallStatsLogger.AddCallStat(timeDataDetected, lastParsedPublishDate);
                            break;
                        case EnterDeserializer:
                            if (hasPublishTime)
                            {
                                enterDeserializerToPublishCallStatsLogger
                                    .AddCallStat(currentTraceEntry.Time, lastParsedPublishDate);
                            }
                            dataDetectedToEnterDeserializerCallStatsLogger.AddCallStat(timeDataDetected,
                                currentTraceEntry.Time);
                            break;
                        case AfterSocketRead:
                            lastAfterSocketEntry = currentTraceEntry;
                            if (hasPublishTime)
                            {
                                afterSocketReadToPublishCallStatsLogger
                                    .AddCallStat(currentTraceEntry.Time, lastParsedPublishDate);
                            }
                            dataDetectedToAfterSocketReadCallStatsLogger.AddContextMeasurement(
                                (double)currentTraceEntry.Subject);
                            dataDetectedToAfterSocketReadCallStatsLogger.AddCallStat(timeDataDetected,
                                currentTraceEntry.Time);
                            break;
                        case BeforeSocketRead:
                            if (hasPublishTime)
                            {
                                beforeSocketReadToPublishCallStatsLogger.AddCallStat(
                                    currentTraceEntry.Time, lastParsedPublishDate);
                            }
                            beforeSocketReadToAfterSocketReadCallStatsLogger.AddCallStat(
                                currentTraceEntry.Time, lastAfterSocketEntry.Time);
                            dataDetectedToBeforeSocketReadCallStatsLogger.AddCallStat(timeDataDetected,
                                currentTraceEntry.Time);
                            hasPublishTime = false;
                            break;
                    }
                }
                startDataDectectionToAllSocketUpdatesFinishedCallStatsLogger.AddCallStat(startDataDectionTime,
                    TraceLogger.ConsistentTimer.UtcNow);
            }
            catch (Exception e)
            {
                Logger.Warn("Error processing PQ latency stats got {0} when processing [{1}]", e, 
                    string.Join(", ", pqTraceLog));
            }
        }
    }
}