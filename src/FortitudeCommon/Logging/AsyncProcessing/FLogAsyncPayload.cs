using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.AsyncProcessing;

public enum AsyncJobRequestType : byte
{
    None
   , RunClosureJob
   , ForwardLogEntryToAppender
   , ForwardBatchLogEntriesToAppender
   , FlushCharBufferToAppender
}

public class FLogAsyncPayload : ReusableObject<FLogAsyncPayload>, ITrackableReset<FLogAsyncPayload>
{
    public FLogAsyncPayload() { }

    public FLogAsyncPayload(FLogAsyncPayload toClone)
    {
        QueueRequestNumber = toClone.QueueRequestNumber;
        AsyncRequestType = toClone.AsyncRequestType;
        ClosureJob       = toClone.ClosureJob;
        SingleFlogEntry  = toClone.SingleFlogEntry;
        BatchFLogEntries = toClone.BatchFLogEntries;
        Appender         = toClone.Appender;
        BufferToFlush    = toClone.BufferToFlush;
    }

    public uint QueueRequestNumber { get; set; }

    public AsyncJobRequestType AsyncRequestType { get; set; }

    public Action? ClosureJob { get; set; }

    public IFLogEntry? SingleFlogEntry { get; set; }

    public IReusableList<IFLogEntry>? BatchFLogEntries { get; set; }

    public IFLogAppender? Appender { get; set; } 

    public IFLogAsyncTargetFlushBufferAppender? BufferingFormatAppender  => Appender as IFLogAsyncTargetFlushBufferAppender;

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
        SingleFlogEntry    = null;
        BatchFLogEntries   = null;
        Appender           = null;
        BufferToFlush      = null;

        return this;
    }

    public void SetAsClosureJob(Action closureJob)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.RunClosureJob;
        ClosureJob       = closureJob;
    }

    public void SetAsSendLogEntry(IFLogEntry toSendFlogEntry, IFLogAppender destinationAppender)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryToAppender;
        SingleFlogEntry  = toSendFlogEntry;
        Appender         = destinationAppender;
    }

    public void SetAsSendBatchLogEntries(IReusableList<IFLogEntry> toSendBatchEntries, IFLogAppender destinationAppender)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardBatchLogEntriesToAppender;
        BatchFLogEntries = toSendBatchEntries;
        Appender         = destinationAppender;
    }

    public void SetAsFlushAppenderBuffer(IBufferedFormatWriter bufferToFlush, IFLogAsyncTargetFlushBufferAppender destinationAppender)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.FlushCharBufferToAppender;
        BufferToFlush    = bufferToFlush;
        Appender         = destinationAppender;
    }

    public void ReceiverExecuteRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: 
                ClosureJob?.Invoke(); 
                break;
            case AsyncJobRequestType.ForwardLogEntryToAppender: 
                Appender!.Append(SingleFlogEntry!);
                break;
            case AsyncJobRequestType.ForwardBatchLogEntriesToAppender: 
                Appender!.Append(BatchFLogEntries!);
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
        SingleFlogEntry    = source.SingleFlogEntry;
        BatchFLogEntries   = source.BatchFLogEntries;
        Appender           = source.Appender;
        BufferToFlush      = source.BufferToFlush;

        return this;
    }
}