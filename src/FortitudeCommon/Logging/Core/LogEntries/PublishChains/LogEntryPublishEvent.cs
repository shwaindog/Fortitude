// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public enum LogEntryEventType
{
    SingleEntry
  , BatchEntries
}

public record struct LogEntryPublishEvent(LogEntryEventType LogEntryEventType, IFLogEntry? LogEntry, ILogEntriesBatch? LogEntriesBatch)
{
    public LogEntryPublishEvent(IFLogEntry logEntry) : this(LogEntryEventType.SingleEntry, logEntry, null) { }
    public LogEntryPublishEvent(ILogEntriesBatch logEntriesBatch) : this(LogEntryEventType.BatchEntries, null, logEntriesBatch) { }
}

public delegate void FLogEntryPublishHandler(LogEntryPublishEvent @event, ITargetingFLogEntrySource fromPublisher);

public static class LogEntryPublishEventExtensions
{
    public static void IncrementRefCount(this LogEntryPublishEvent toIncrement)
    {
        // StackTrace stackTrace = new StackTrace(0, true);
        //
        // var frames = stackTrace.GetFrames();
        //
        // for (var i = 0; i < 3; i++)
        // {
        //     var frame = frames[i];
        //     Console.Out.Write(frame.ToString());
        // }
        // Console.Out.WriteLine($"Inc {toIncrement.LogEntry.InstanceNumber} RefCount = {(toIncrement.LogEntry?.RefCount ?? 0)}");
        toIncrement.LogEntry?.IncrementRefCount();
        toIncrement.LogEntriesBatch?.IncrementRefCount();
    }

    public static void DecrementRefCount(this LogEntryPublishEvent toIncrement)
    {
        // StackTrace stackTrace = new StackTrace(0, true);
        //
        // var frames = stackTrace.GetFrames();
        //
        // for (var i = 0; i < 3; i++)
        // {
        //     var frame = frames[i];
        //     Console.Out.Write(frame.ToString());
        // }
        // Console.Out.WriteLine($"Dec {toIncrement.LogEntry.InstanceNumber} RefCount = {(toIncrement.LogEntry?.RefCount ?? 0)}");
        toIncrement.LogEntry?.DecrementRefCount();
        toIncrement.LogEntriesBatch?.DecrementRefCount();
    }

    public static uint EntriesCount(this LogEntryPublishEvent countEntries)
    {
        return (uint)(countEntries.LogEntriesBatch?.Count ?? 0) + (uint)(countEntries.LogEntry != null ? 1 : 0);
    }
}
