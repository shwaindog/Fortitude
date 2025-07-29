using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
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
    private readonly List<IFLogAsyncTargetReceiveQueueAppender> appenders = new ();
    public FLogAsyncPayload() { }

    public FLogAsyncPayload(FLogAsyncPayload toClone)
    {
        QueueRequestNumber = toClone.QueueRequestNumber;
        AsyncRequestType   = toClone.AsyncRequestType;
        ClosureJob         = toClone.ClosureJob;
        FlogEntryEvent    = toClone.FlogEntryEvent;
        BatchFLogEntries   = toClone.BatchFLogEntries;
        Appenders          = toClone.Appenders;
        BufferToFlush      = toClone.BufferToFlush;
    }

    public uint QueueRequestNumber { get; set; }

    public AsyncJobRequestType AsyncRequestType { get; set; }

    public Action? ClosureJob { get; set; }

    public LogEntryPublishEvent? FlogEntryEvent { get; set; }

    public IReusableList<IFLogEntry>? BatchFLogEntries { get; set; }

    public IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> Appenders
    {
        get => appenders;
        set
        {
            appenders.Clear();
            appenders.AddRange(value);
        }
    }

    public IFLogAsyncTargetFlushBufferAppender? BufferingFormatAppender  => Appenders[0] as IFLogAsyncTargetFlushBufferAppender;

    public IBufferedFormatWriter? BufferToFlush { get; set; }

    public void RunAsyncRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob:
                ClosureJob?.Invoke();
                break;
        }
    }

    public FLogAsyncPayload ResetWithTracking()
    {
        QueueRequestNumber = 0;
        AsyncRequestType   = AsyncJobRequestType.None;
        ClosureJob         = null;
        FlogEntryEvent    = null;
        BatchFLogEntries   = null;
        appenders.Clear();;
        BufferToFlush      = null;

        return this;
    }

    public void SetAsClosureJob(Action closureJob)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.RunClosureJob;
        ClosureJob       = closureJob;
    }

    public void SetAsSendLogEntryEvent(LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogAsyncTargetReceiveQueueAppender> appenders)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryEventToAppender;
        FlogEntryEvent   = logEntryEvent;
        Appenders        = appenders;
    }

    public void SetAsSendLogEntryEvent(LogEntryPublishEvent logEntryEvent, IFLogAsyncTargetReceiveQueueAppender appender)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryEventToAppender;
        FlogEntryEvent   = logEntryEvent;
        appenders.Clear();
        appenders.Add(appender);
    }

    public void SetAsFlushAppenderBuffer(IBufferedFormatWriter bufferToFlush, IFLogAsyncTargetFlushBufferAppender destinationAppender)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.FlushCharBufferToAppender;
        BufferToFlush    = bufferToFlush;
        appenders.Clear();
        appenders.Add(destinationAppender);
    }

    public void ReceiverExecuteRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: 
                ClosureJob?.Invoke(); 
                break;
            case AsyncJobRequestType.ForwardLogEntryEventToAppender:
                var flogEntryEvent = FlogEntryEvent!.Value;

                for (int i = 0; i < Appenders.Count; i++)
                {
                    var appender = Appenders[i];
                    appender!.ProcessReceivedLogEntryEvent(flogEntryEvent);   
                }
                break;
            case AsyncJobRequestType.FlushCharBufferToAppender: 
                BufferingFormatAppender!.FlushBufferToAppender(BufferToFlush!);
                break;
        }
    }


    public override FLogAsyncPayload Clone() => 
        Recycler?.Borrow<FLogAsyncPayload>().CopyFrom(this, CopyMergeFlags.FullReplace ) ?? new FLogAsyncPayload(this);

    public override FLogAsyncPayload CopyFrom(FLogAsyncPayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        QueueRequestNumber = source.QueueRequestNumber;
        AsyncRequestType   = source.AsyncRequestType;
        ClosureJob         = source.ClosureJob;
        FlogEntryEvent    = source.FlogEntryEvent;
        BatchFLogEntries   = source.BatchFLogEntries;
        Appenders           = source.Appenders;
        BufferToFlush      = source.BufferToFlush;

        return this;
    }
}