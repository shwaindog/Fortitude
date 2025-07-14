// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core;

public delegate void ForwardLogEntry(IFLogEntry toSend);

public delegate void BatchForwardLogEntry(IReusableList<IFLogEntry> batchToSend);

public interface IFLogEntryForwarder
{
    void ForwardLogEntryTo(IFLogEntry logEntry);
}

public class FLogLoggerDispatcherFactory : IFLogEntryForwarder
{
    private readonly Func<IAutoRecycleEnumerable<Appending.IFLoggerAppender>> appenderRetriever;

    public void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        foreach (var appender in appenderRetriever())
        {
            appender.ForwardLogEntryTo(logEntry);
        }
    }



}
