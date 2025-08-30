// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;

namespace FortitudeCommon.Logging.Core.ConditionalLogging;

public readonly struct ExecutionTimingStart(IFLogger logger, uint sequenceNum, DateTime startTime)
{
    public readonly uint SequenceNumber = sequenceNum;

    public readonly IFLogger StartFrom = logger;

    public readonly DateTime StartTime = startTime;
}

public readonly struct ExecutionDuration(ExecutionTimingStart executionTimeStart, DateTime stopTime, IFLogger? endLogger = null)
{
    public readonly ExecutionTimingStart ExecutionTimingStart = executionTimeStart;

    public readonly IFLogger? StoppedAt = endLogger;

    public readonly DateTime StopTime = stopTime;
}

public static class ExecutionDurationExtensions
{
    public static ExecutionDuration StopClock(this ExecutionTimingStart executionTimeStart, IFLogger? endLogger = null)
    {
        var executionDuration = new ExecutionDuration(executionTimeStart, TimeContext.UtcNow, endLogger);

        return executionDuration;
    }

    public static long GetMicros(this ExecutionDuration timingDuration)
    {
        var durationTimeSpan = timingDuration.StopTime - timingDuration.ExecutionTimingStart.StartTime;
        return (long)durationTimeSpan.TotalMicroseconds;
    }
}
