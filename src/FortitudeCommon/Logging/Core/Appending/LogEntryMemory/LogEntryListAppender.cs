// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending.LogEntryMemory;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.LogEntryMemory;

public class LogEntryEventListAppender : InMemoryLogEntryEventAppender
{
    public LogEntryEventListAppender(string appenderName) : base(appenderName) => LogEntries = new List<LogEntryPublishEvent>();

    public LogEntryEventListAppender(string appenderName, int initialCapacity) : base(appenderName, initialCapacity) =>
        LogEntries = new List<LogEntryPublishEvent>(initialCapacity);

    public List<LogEntryPublishEvent> LogEntries { get; }

    public override ISizedMemoryAppenderConfig GetAppenderConfig() => (ISizedMemoryAppenderConfig)AppenderConfig;

    protected override void AppendToMemory(LogEntryPublishEvent logEntryEvent)
    {
        LogEntries.Add(logEntryEvent);

        NewEntry?.Invoke(logEntryEvent, this);
    }


    public event Action<LogEntryPublishEvent, LogEntryEventListAppender>? NewEntry;
}

public class LogEntryListInterceptor : FLogEntryForkingInterceptor
{
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override string Name => $"{nameof(LogEntryListInterceptor)}";

    public List<LogEntryPublishEvent> LogEntries { get; } = new();

    protected override void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        logEntryEvent.IncrementRefCount();

        LogEntries.Add(logEntryEvent);

        NewEntry?.Invoke(logEntryEvent);
        base.SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }


    public event Action<LogEntryPublishEvent>? NewEntry;
}
