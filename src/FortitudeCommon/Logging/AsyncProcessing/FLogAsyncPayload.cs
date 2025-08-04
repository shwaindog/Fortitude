using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.AsyncProcessing;

public enum AsyncJobRequestType : byte
{
    None
  , RunClosureJob
  , ForwardLogEntryEventToAppender
  , FlushCharBufferToAppender
}

public class FLogAsyncPayload : ReusableObject<FLogAsyncPayload>, ITrackableReset<FLogAsyncPayload>
{
    private readonly List<IFLogEntrySink> logEntrySinks = new();
    public FLogAsyncPayload() { }

    public FLogAsyncPayload(FLogAsyncPayload toClone)
    {
        QueueRequestNumber = toClone.QueueRequestNumber;
        AsyncRequestType   = toClone.AsyncRequestType;
        ClosureJob         = toClone.ClosureJob;
        FlogEntryEvent     = toClone.FlogEntryEvent;
        PublishSource      = toClone.PublishSource;
        LogEntrySinks      = toClone.LogEntrySinks;
        BufferToFlush      = toClone.BufferToFlush;

        BufferingFormatAppender = toClone.BufferingFormatAppender;
    }

    public uint QueueRequestNumber { get; set; }

    public AsyncJobRequestType AsyncRequestType { get; set; }

    public Action? ClosureJob { get; set; }

    public LogEntryPublishEvent? FlogEntryEvent { get; set; }

    public ITargetingFLogEntrySource? PublishSource { get; set; }

    public IReadOnlyList<IFLogEntrySink> LogEntrySinks
    {
        get => logEntrySinks;
        set
        {
            logEntrySinks.Clear();
            logEntrySinks.AddRange(value);
        }
    }

    public IFLogBufferingFormatAppender? BufferingFormatAppender { get; set; }

    public IBufferedFormatWriter? BufferToFlush { get; set; }

    public void RunAsyncRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: ClosureJob?.Invoke(); break;
        }
    }

    public FLogAsyncPayload ResetWithTracking()
    {
        QueueRequestNumber = 0;
        AsyncRequestType   = AsyncJobRequestType.None;
        ClosureJob         = null;
        FlogEntryEvent     = null;
        PublishSource      = null;
        logEntrySinks.Clear();

        BufferToFlush = null;

        BufferingFormatAppender = null;

        return this;
    }

    public void SetAsClosureJob(Action closureJob)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.RunClosureJob;
        ClosureJob       = closureJob;
    }

    public void SetAsSendLogEntryEvent
        (LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogEntrySink> appenders, ITargetingFLogEntrySource publishSource)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryEventToAppender;
        FlogEntryEvent   = logEntryEvent;
        PublishSource    = publishSource;
        LogEntrySinks    = appenders;
    }

    public void SetAsSendLogEntryEvent(LogEntryPublishEvent logEntryEvent, IFLogEntrySink appender, ITargetingFLogEntrySource publishSource)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryEventToAppender;
        FlogEntryEvent   = logEntryEvent;
        PublishSource    = publishSource;
        logEntrySinks.Clear();
        logEntrySinks.Add(appender);
    }

    public void SetAsFlushAppenderBuffer(IBufferedFormatWriter bufferToFlush, IFLogBufferingFormatAppender destinationAppender)
    {
        ResetWithTracking();
        AsyncRequestType        = AsyncJobRequestType.FlushCharBufferToAppender;
        BufferToFlush           = bufferToFlush;
        BufferingFormatAppender = destinationAppender;
    }

    public void ReceiverExecuteRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: ClosureJob?.Invoke(); break;
            case AsyncJobRequestType.ForwardLogEntryEventToAppender:
                var flogEntryEvent = FlogEntryEvent!.Value;

                for (int i = 0; i < LogEntrySinks.Count; i++)
                {
                    var logEntrySink = LogEntrySinks[i];
                    logEntrySink.InBoundListener(flogEntryEvent, PublishSource!);
                }
                break;
            case AsyncJobRequestType.FlushCharBufferToAppender: BufferingFormatAppender!.FlushBufferToAppender(BufferToFlush!); break;
        }
    }


    public override FLogAsyncPayload Clone() =>
        Recycler?.Borrow<FLogAsyncPayload>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogAsyncPayload(this);

    public override FLogAsyncPayload CopyFrom(FLogAsyncPayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        QueueRequestNumber = source.QueueRequestNumber;
        AsyncRequestType   = source.AsyncRequestType;
        ClosureJob         = source.ClosureJob;
        FlogEntryEvent     = source.FlogEntryEvent;
        PublishSource      = source.PublishSource;
        LogEntrySinks      = source.LogEntrySinks;
        BufferToFlush      = source.BufferToFlush;

        BufferingFormatAppender = source.BufferingFormatAppender;

        return this;
    }
}
