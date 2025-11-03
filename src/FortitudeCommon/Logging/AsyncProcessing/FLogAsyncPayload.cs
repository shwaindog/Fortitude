// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.AsyncProcessing;

public enum AsyncJobRequestType : byte
{
    None
  , RunClosureJob
  , ForwardLogEntryEventToAppender
  , PublishLogEntryEventToAppenders
  , FlushCharBufferToAppender
}

public class FLogAsyncPayload : ReusableObject<FLogAsyncPayload>, ITrackableReset<FLogAsyncPayload>
{
    private readonly List<IForkingFLogEntrySink> publishToLogEntrySinks = new();
    public FLogAsyncPayload() { }

    public FLogAsyncPayload(FLogAsyncPayload toClone)
    {
        QueueRequestNumber     = toClone.QueueRequestNumber;
        AsyncRequestType       = toClone.AsyncRequestType;
        ClosureJob             = toClone.ClosureJob;
        FlogEntryEvent         = toClone.FlogEntryEvent;
        PublishSource          = toClone.PublishSource;
        PublishToLogEntrySinks = toClone.PublishToLogEntrySinks;
        BufferToFlush          = toClone.BufferToFlush;
    }

    public uint QueueRequestNumber { get; set; }

    public AsyncJobRequestType AsyncRequestType { get; set; }

    public Action? ClosureJob { get; set; }

    public LogEntryPublishEvent? FlogEntryEvent { get; set; }

    public ITargetingFLogEntrySource? PublishSource { get; set; }

    public IReadOnlyList<IForkingFLogEntrySink> PublishToLogEntrySinks
    {
        get => publishToLogEntrySinks;
        set
        {
            publishToLogEntrySinks.Clear();
            publishToLogEntrySinks.AddRange(value);
        }
    }
    public IFLogEntrySink? ForwardToLogEntrySink { get; set; }

    public IBufferedFormatWriter? BufferToFlush { get; set; }

    public FLogAsyncPayload ResetWithTracking()
    {
        QueueRequestNumber = 0;
        AsyncRequestType   = AsyncJobRequestType.None;
        ClosureJob         = null;
        FlogEntryEvent     = null;
        PublishSource      = null;
        publishToLogEntrySinks.Clear();

        BufferToFlush = null;

        return this;
    }

    public void RunAsyncRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: ClosureJob?.Invoke(); break;
        }
    }

    public void SetAsClosureJob(Action closureJob)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.RunClosureJob;
        ClosureJob       = closureJob;
    }

    public void SetAsSendLogEntryEvent
        (LogEntryPublishEvent logEntryEvent, IReadOnlyList<IForkingFLogEntrySink> logEntrySinks, ITargetingFLogEntrySource publishSource)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.PublishLogEntryEventToAppenders;

        logEntryEvent.IncrementRefCount();

        FlogEntryEvent         = logEntryEvent;
        PublishSource          = publishSource;
        PublishToLogEntrySinks = logEntrySinks;
    }

    public void SetAsSendLogEntryEvent(LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, ITargetingFLogEntrySource publishSource)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.ForwardLogEntryEventToAppender;

        logEntryEvent.IncrementRefCount();

        FlogEntryEvent        = logEntryEvent;
        PublishSource         = publishSource;
        ForwardToLogEntrySink = logEntrySink;
    }

    public void SetAsFlushAppenderBuffer(IBufferedFormatWriter bufferToFlush)
    {
        ResetWithTracking();
        AsyncRequestType = AsyncJobRequestType.FlushCharBufferToAppender;
        BufferToFlush    = bufferToFlush;
    }

    public void ReceiverExecuteRequest()
    {
        switch (AsyncRequestType)
        {
            case AsyncJobRequestType.RunClosureJob: ClosureJob?.Invoke(); break;
            case AsyncJobRequestType.PublishLogEntryEventToAppenders:
                var flogEntryToPublish = FlogEntryEvent!.Value;

                for (var i = 0; i < PublishToLogEntrySinks.Count; i++)
                {
                    var logEntrySink = PublishToLogEntrySinks[i];
                    logEntrySink.ForkingInBoundListener(flogEntryToPublish, PublishSource!);
                }
                flogEntryToPublish.LogEntry?.DecrementRefCount();
                flogEntryToPublish.LogEntriesBatch?.DecrementRefCount();
                break;
            case AsyncJobRequestType.ForwardLogEntryEventToAppender:
                var flogEntryEvent = FlogEntryEvent!.Value;

                if (PublishSource == ForwardToLogEntrySink)
                    PublishSource?.FinalTarget?.InBoundListener(flogEntryEvent, PublishSource);
                else
                    ForwardToLogEntrySink?.InBoundListener(flogEntryEvent, PublishSource!);
                break;
            case AsyncJobRequestType.FlushCharBufferToAppender: BufferToFlush!.Flush(); break;
        }
    }

    public override FLogAsyncPayload Clone() =>
        Recycler?.Borrow<FLogAsyncPayload>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogAsyncPayload(this);

    public override FLogAsyncPayload CopyFrom(FLogAsyncPayload source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        QueueRequestNumber     = source.QueueRequestNumber;
        AsyncRequestType       = source.AsyncRequestType;
        ClosureJob             = source.ClosureJob;
        FlogEntryEvent         = source.FlogEntryEvent;
        PublishSource          = source.PublishSource;
        PublishToLogEntrySinks = source.PublishToLogEntrySinks;
        BufferToFlush          = source.BufferToFlush;

        return this;
    }
}
