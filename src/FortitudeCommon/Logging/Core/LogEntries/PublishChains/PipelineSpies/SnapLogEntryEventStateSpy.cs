// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class SnapLogEntryEventStateSpy(IList<string> snappedLogEntryEvents, string spyName) : FLogEntryForkingInterceptor
{
    private static readonly PalantírReveal<IFLogEntry> SummarizedFLogEntry = (flogEntry, sbc) =>
    {
        using var tb =
            sbc.StartComplexType(nameof(FLogEntry))
               .Field.AlwaysAdd(nameof(flogEntry.IssueSequenceNumber), flogEntry.IssueSequenceNumber)
               .Field.AlwaysAdd(nameof(flogEntry.InstanceNumber), flogEntry.InstanceNumber)
               .Field.AlwaysAdd(nameof(flogEntry.RefCount), flogEntry.RefCount)
               .Field.WhenNonDefaultAdd(nameof(flogEntry.CorrelationId), flogEntry.CorrelationId)
               .Field.AlwaysAdd(nameof(flogEntry.LogDateTime), flogEntry.LogDateTime, "{0:HH:mm:ss.ffffff}")
               .Field.AlwaysAdd(nameof(flogEntry.LogLevel), flogEntry.LogLevel)
               .Field.AlwaysReveal(nameof(flogEntry.LogLocation), flogEntry.LogLocation, flogEntry.LogLocation.Styler())
               .Field.WhenNonNullAdd(nameof(flogEntry.Style), flogEntry.Style)
               .Field.WhenNonNullAdd("Thread.Id", flogEntry.Thread.ManagedThreadId)
               .Field.WhenNonNullAdd("Thread.Name", flogEntry.Thread.Name)
               .Field.WhenNonDefaultAdd(nameof(flogEntry.Logger), flogEntry.Logger?.FullName[^25..] ?? "".AsSpan())
               .Field.AlwaysAddCharSeq(nameof(flogEntry.Message), flogEntry.Message, 0, 40, "\"{0}...\"");

        return tb.Complete();
    };
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.InterceptionPoint;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override string Name => spyName;

    public IList<string> LogEntries { get; } = snappedLogEntryEvents;

    protected override void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        var snapStateAppender = Recycler!.Borrow<TheOneString>().Initialize();
        snapStateAppender
            .StartComplexType(nameof(SnapLogEntryEventStateSpy))
            .Field.AlwaysAdd("Spy", spyName)
            .Field.AlwaysAdd("CapturedAt", TimeContext.UtcNow, "{0:HH:mm:ss.ffffff}")
            .Field.AlwaysAdd("OnQueueNumber", FLogAsyncQueue.MyCallingQueueNumber)
            .Field.WhenNonNullReveal("LogEntry", logEntryEvent.LogEntry, SummarizedFLogEntry)
            .Field.WhenNonNullReveal("LogEntryBatch", logEntryEvent.LogEntriesBatch).Complete();

        var logEntryEventSnap = snapStateAppender.WriteBuffer.ToString();
        LogEntries.Add(logEntryEventSnap);

        snapStateAppender.DecrementRefCount();
        NewEntry?.Invoke(logEntryEventSnap);
        base.SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public event Action<string>? NewEntry;
}
