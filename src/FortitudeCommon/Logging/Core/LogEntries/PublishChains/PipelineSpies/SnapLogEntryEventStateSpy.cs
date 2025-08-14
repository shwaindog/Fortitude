using FortitudeCommon.Chronometry;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class SnapLogEntryEventStateSpy(IList<string?> snappedLogEntryEvents, string spyName) : FLogEntryForkingInterceptor
{
    private IList<string?> snappedLogEntryEvents = snappedLogEntryEvents;

    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override string Name => spyName;

    protected override void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        var snapStateAppender = Recycler!.Borrow<StyledTypeStringAppender>().Initialize();
        snapStateAppender
            .StartComplexType(spyName)
            .Field.AlwaysAdd("CapturedAt", TimeContext.UtcNow)
            .Field.AlwaysAdd("OnQueueNumber", FLogAsyncQueue.MyCallingQueueNumber)
            .Field.WhenNonNullAdd("LogEntry", logEntryEvent.LogEntry)
            .Field.WhenNonDefaultAdd("LogEntryBatch", logEntryEvent.LogEntriesBatch).Complete();

        var logEntryEventSnap = snapStateAppender.WriteBuffer.ToString();
        LogEntries.Add(logEntryEventSnap);

        snapStateAppender.DecrementRefCount();
        NewEntry?.Invoke(logEntryEventSnap);
        base.SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public List<string> LogEntries { get; } = new();


    public event Action<string>? NewEntry;
}
