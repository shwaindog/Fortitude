using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class SnapLogEntryEventStateSpy(IList<string?> snappedLogEntryEvents, string spyName) : FLogEntryForkingInterceptor
{
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override string Name => spyName;

    protected override void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        var snapStateAppender = Recycler!.Borrow<StyledTypeStringAppender>().Initialize();
        snapStateAppender
            .StartComplexType(nameof(SnapLogEntryEventStateSpy))
            .Field.AlwaysAdd("Spy", spyName)
            .Field.AlwaysAdd("CapturedAt", TimeContext.UtcNow, "{0:HH:mm:ss.ffffff}")
            .Field.AlwaysAdd("OnQueueNumber", FLogAsyncQueue.MyCallingQueueNumber)
            .Field.WhenNonNullAdd("LogEntry", logEntryEvent.LogEntry, SummarizedFLogEntry)
            .Field.WhenNonDefaultAdd("LogEntryBatch", logEntryEvent.LogEntriesBatch).Complete();

        var logEntryEventSnap = snapStateAppender.WriteBuffer.ToString();
        LogEntries.Add(logEntryEventSnap);

        snapStateAppender.DecrementRefCount();
        NewEntry?.Invoke(logEntryEventSnap);
        base.SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public IList<string?> LogEntries { get; } = snappedLogEntryEvents;
    
    private static readonly CustomTypeStyler<IFLogEntry> SummarizedFLogEntry = (flogEntry, sbc) =>
    {
        using var tb =
            sbc.StartComplexType(nameof(FLogEntry))
               .Field.AlwaysAdd(nameof(flogEntry.IssueSequenceNumber), flogEntry.IssueSequenceNumber)
               .Field.AlwaysAdd(nameof(flogEntry.InstanceNumber), flogEntry.InstanceNumber)
               .Field.AlwaysAdd(nameof(flogEntry.RefCount), flogEntry.RefCount)
               .Field.WhenNonDefaultAdd(nameof(flogEntry.CorrelationId), flogEntry.CorrelationId)
               .Field.AlwaysAdd(nameof(flogEntry.LogDateTime), flogEntry.LogDateTime, "{0:HH:mm:ss.ffffff}")
               .Field.AlwaysAdd(nameof(flogEntry.LogLevel), flogEntry.LogLevel)
               .Field.AlwaysAdd(nameof(flogEntry.LogLocation), flogEntry.LogLocation, flogEntry.LogLocation.Styler())
               .Field.WhenNonNullAdd(nameof(flogEntry.Style), flogEntry.Style, flogEntry.Style.Styler())
               .Field.WhenNonNullAdd("Thread.Id", flogEntry.Thread.ManagedThreadId)
               .Field.WhenNonNullAdd("Thread.Name", flogEntry.Thread.Name)
               .Field.WhenNonDefaultAdd(nameof(flogEntry.Logger), (flogEntry.Logger?.FullName[^25..] ?? "".AsSpan()))
               .Field.AlwaysAdd(nameof(flogEntry.Message), flogEntry.Message, 0, 40, "\"{0}...\"");

        return tb.Complete();
    }; 

    public event Action<string>? NewEntry;
}
