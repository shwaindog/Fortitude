// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core;

public delegate void ForwardLogEntry(IFLogEntry toSend);

public delegate void BatchForwardLogEntry(IReusableList<IFLogEntry> batchToSend);

public interface IFLogEntrySink
{
    void ForwardLogEntryTo(IFLogEntry logEntry);
}

public class FLogLoggerDispatcherFactory : IFLogEntrySink
{
    private readonly Func<IAutoRecycleEnumerable<Appending.IFLogAppender>> appenderRetriever;

    public void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        foreach (var appender in appenderRetriever())
        {
            appender.ForwardLogEntryTo(logEntry);
        }
    }
}
