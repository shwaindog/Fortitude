// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
        toIncrement.LogEntry?.IncrementRefCount();
        toIncrement.LogEntriesBatch?.IncrementRefCount();
    }

    public static void DecrementRefCount(this LogEntryPublishEvent toIncrement)
    {
        toIncrement.LogEntry?.DecrementRefCount();
        toIncrement.LogEntriesBatch?.DecrementRefCount();
    }

    public static uint EntriesCount(this LogEntryPublishEvent countEntries) =>
        (uint)(countEntries.LogEntriesBatch?.Count ?? 0) + (uint)(countEntries.LogEntry != null ? 1 : 0);
}
