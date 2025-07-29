// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public enum LogEntryEventType
{
    SingleEntry
  , BatchEntries
}

public record struct LogEntryPublishEvent(LogEntryEventType LogEntryEventType, IFLogEntry? LogEntry, IReusableList<IFLogEntry>? LogEntriesBatch)
{
    public LogEntryPublishEvent(IFLogEntry logEntry) : this(LogEntryEventType.SingleEntry, logEntry, null) { }
    public LogEntryPublishEvent(IReusableList<IFLogEntry> logEntriesBatch) : this(LogEntryEventType.BatchEntries, null, logEntriesBatch) { }
}

public delegate void FLogEntryPublishHandler(LogEntryPublishEvent @event, ITargetingFLogEntrySource fromPublisher);
