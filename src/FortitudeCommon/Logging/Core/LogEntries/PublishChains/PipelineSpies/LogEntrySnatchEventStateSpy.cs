// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class LogEntrySnatchEventStateSpy(IList<IFLogEntry> snatchedLogEntryEvents, string spyName) : FLogEntryForkingInterceptor
{
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.InterceptionPoint;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override string Name => spyName;

    public IList<IFLogEntry> LogEntries { get; } = snatchedLogEntryEvents;

    protected override void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        var singleEntry = logEntryEvent.LogEntry;
        if (singleEntry != null)
        {
            LogEntries.Add(singleEntry); // expected should incrementRefCount on add
            NewEntry?.Invoke(singleEntry);
        }
        var batchEntries = logEntryEvent.LogEntriesBatch;
        if (batchEntries != null)
            foreach (var batchEntry in batchEntries)
            {
                LogEntries.Add(batchEntry); // expected should incrementRefCount on add
                NewEntry?.Invoke(batchEntry);
            }
        base.SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public event Action<IFLogEntry>? NewEntry;
}
