// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core.ActivationProfiles;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.ConditionalLogging;

public interface IFLoggerExecutionDuration
{
    ExecutionTimingStart StartTiming();

    IMutableFLogEntry? StopTimingAllowLogEntryWhenExceeds
    (ExecutionTimingStart startTime, FLogLevel logLevel, long logThresholdMicros
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, int interval = 1
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? StopTimingAllowLogEntryWhenAverageExceeds
    (ExecutionTimingStart startTime, FLogLevel logLevel, long logThresholdAverageMicros
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, int averageEntryInterval = 10
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    TimingTraceExecutionPath? StartTraceTime
    (FLogLevel logLevel
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void StopTraceLogTraceWhenExceeds
        (TimingTraceExecutionPath? trace, long logThresholdMicros, int interval = 1);
}

public class FLoggerExecutionDuration(FLogger wrappingLogger) : IFLoggerExecutionDuration
{
    // ReSharper disable ExplicitCallerInfoArgument
    private static readonly ConcurrentDictionary<FLogCallLocation, LocationIntervalMetrics>        PerLocationIntervalMetrics        = new();
    private static readonly ConcurrentDictionary<FLogCallLocation, LocationAverageDurationMetrics> PerLocationAverageDurationMetrics = new();

    public ExecutionTimingStart StartTiming() => new(wrappingLogger, wrappingLogger.GetNextTimingDurationSequenceEvent(), DateTime.UtcNow);

    public IMutableFLogEntry? StopTimingAllowLogEntryWhenExceeds
    (ExecutionTimingStart startTime, FLogLevel logLevel, long logThresholdMicros
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, int interval = 1
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var getDuration = startTime.StopClock(wrappingLogger);
        if (getDuration.GetMicros() > logThresholdMicros)
        {
            var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
            var locationMetrics  = PerLocationIntervalMetrics.GetOrAdd(logEntryLocation, logLoc => new LocationIntervalMetrics(logLoc));
            var occurence        = locationMetrics.IncrementOccurence();
            if (occurence % interval == 0) return wrappingLogger.AtLevel(logLevel, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        }
        return null;
    }

    public IMutableFLogEntry? StopTimingAllowLogEntryWhenAverageExceeds
    (ExecutionTimingStart startTime, FLogLevel logLevel, long logThresholdAverageMicros
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, int averageEntryInterval = 10
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var getDuration      = startTime.StopClock(wrappingLogger);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var locationAvgMetrics = PerLocationAverageDurationMetrics
            .GetOrAdd(logEntryLocation, logLoc => new LocationAverageDurationMetrics(logLoc, averageEntryInterval));
        var averageTime = locationAvgMetrics.AddDurationGetMicros(getDuration);
        if (averageTime > logThresholdAverageMicros)
        {
            locationAvgMetrics.Reset();
            return wrappingLogger.AtLevel(logLevel, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        }
        return null;
    }

    public TimingTraceExecutionPath? StartTraceTime(FLogLevel logLevel
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var flogEntry        = wrappingLogger.LogEntryPool.SourceLogEntry();
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        flogEntry.Initialize(new LoggerEntryContext(wrappingLogger, wrappingLogger.PublishEndpoint, logEntryLocation, logLevel));
        var startedAt        = new ExecutionTimingStart(wrappingLogger, wrappingLogger.GetNextTimingDurationSequenceEvent(), DateTime.UtcNow);
        var timeTraceExePath = new TimingTraceExecutionPath(startedAt, flogEntry);
        return timeTraceExePath;
    }

    public void StopTraceLogTraceWhenExceeds
        (TimingTraceExecutionPath? trace, long logThresholdMicros, int interval = 1)
    {
        if (trace == null) return;
        var duration = trace.StopGetDuration();
        if (duration.GetMicros() > logThresholdMicros)
        {
            var locationMetrics = PerLocationIntervalMetrics.GetOrAdd(trace.FLogCallLocation, logLoc => new LocationIntervalMetrics(logLoc));
            var occurence       = locationMetrics.IncrementOccurence();
            if (occurence % interval == 0) trace.Dispatch();
        }
    }

    public void StopTraceLogTraceWhenAverageExceeds
        (TimingTraceExecutionPath? trace, long logThresholdAverageMicros, int averageEntryInterval = 10)
    {
        if (trace == null) return;
        var duration = trace.StopGetDuration();
        var locationAvgMetrics = PerLocationAverageDurationMetrics
            .GetOrAdd(trace.FLogCallLocation, logLoc => new LocationAverageDurationMetrics(logLoc, averageEntryInterval));
        var averageTime = locationAvgMetrics.AddDurationGetMicros(duration);
        if (averageTime > logThresholdAverageMicros)
        {
            locationAvgMetrics.Reset();
            trace.Dispatch();
        }
    }

    private class LocationIntervalMetrics(FLogCallLocation stopLocation)
    {
        protected int IntervalCount;

        public int IncrementOccurence() => Interlocked.Increment(ref IntervalCount);
    }

    private class LocationAverageDurationMetrics : LocationIntervalMetrics
    {
        private const int MinPublishAverageSamples = 5;

        protected readonly int DoubleInterval;
        protected readonly int Interval;

        protected double NextIntervalAverageHistory;

        protected double SumAllDurationsForDoubleInterval;

        public LocationAverageDurationMetrics(FLogCallLocation stopLocation, int restartInterval) : base(stopLocation)
        {
            restartInterval =  Math.Max(MinPublishAverageSamples * 2, restartInterval * 2);
            DoubleInterval  += restartInterval % 2;
            Interval        =  restartInterval / 2;
        }

        public void Reset()
        {
            SumAllDurationsForDoubleInterval = 0;
            NextIntervalAverageHistory       = 0;
            IntervalCount                    = 0;
        }

        public long? AddDurationGetMicros(ExecutionDuration executionDuration)
        {
            var currentInterval = IncrementOccurence();
            if (currentInterval >= DoubleInterval)
            {
                SumAllDurationsForDoubleInterval = NextIntervalAverageHistory;
                IntervalCount                    = Interval;

                NextIntervalAverageHistory = 0;
            }
            SumAllDurationsForDoubleInterval += executionDuration.GetMicros();
            if (currentInterval >= Interval)
            {
                NextIntervalAverageHistory += executionDuration.GetMicros();
                return (long)(SumAllDurationsForDoubleInterval / currentInterval);
            }
            return null;
        }
    }

    // ReSharper restore ExplicitCallerInfoArgument
}
