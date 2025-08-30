// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending.LogEntryMemory;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.LogEntryMemory;

public abstract class InMemoryLogEntryEventAppender : FLogAppender
{
    protected InMemoryLogEntryEventAppender(string appenderName) : base(new SizedMemoryAppenderConfig(appenderName), FLogContext.Context) { }

    protected InMemoryLogEntryEventAppender(string appenderName, int initialCapacity)
        : base(new SizedMemoryAppenderConfig(appenderName, initialCapacity), FLogContext.Context) { }

    public override ISizedMemoryAppenderConfig GetAppenderConfig() => (ISizedMemoryAppenderConfig)AppenderConfig;


    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        logEntryEvent.IncrementRefCount();
        AppendToMemory(logEntryEvent);
    }

    protected abstract void AppendToMemory(LogEntryPublishEvent logEntryEvent);
}
