// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public interface IBufferedTargetDestinationFormatWriterRequestCache : IBufferedFormatWriterRequestCache
{
    string TargetName { get; }

    bool HasUnflushedBufferedWriters { get; }

    int FlushWhenBufferLength { get; set; }

    TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; set; }

    TimeSpan AutoFlushIntervalTimeSpan { get; set; }

    DateTime LastFlushAt { get; set; }
}

public class SingleDestBufferedFormatWriterRequestCache : SingleDestDirectFormatWriterRequestCache, IBufferedTargetDestinationFormatWriterRequestCache
{
    private const string SingleTargetFirstBufferedAppenderName  = "SingleTargetBuffer1";
    private const string SingleTargetSecondBufferedAppenderName = "SingleTargetBuffer2";

    private readonly ManualResetEvent allBuffersFlushedEvent = new(true);

    private readonly object timerSyncLock = new();

    private TimeSpan autoFlushIntervalTimeSpan;

    private ITimerUpdate? autoFlushTimer;

    private IBufferedFormatWriter? buffer1;
    private IBufferedFormatWriter? buffer2;

    private bool bufferingEnabled;

    private Action checkWaitingFormatWriterRequests = null!;

    private int currentBufferCounter;
    private int maxBuffers;

    private IBufferedFormatWriter? onCreateBuffer1Instance;
    private IBufferedFormatWriter? onCreateBuffer2Instance;

    protected Action TimerCheckWaitingFormatWriterRequests = null!;

    public SingleDestBufferedFormatWriterRequestCache Initialize(FLogBufferingFormatAppender owningAppender, IFLogContext context
      , string targetName = SIngleDestinationTarget)
    {
        base.Initialize(owningAppender, context);
        TargetName = targetName;

        bufferingEnabled = !owningAppender.GetAppenderConfig().DisableBuffering;


        checkWaitingFormatWriterRequests      = CheckHasFormatWriterRequests;
        TimerCheckWaitingFormatWriterRequests = TimerTriggeredCheckFormatWriterRequests;

        CreateFormatWriters(targetName, context);

        return this;
    }

    protected IBufferFlushingFormatWriter BufferFlushingFormatWriter => (IBufferFlushingFormatWriter)DirectFormatWriter!;

    protected virtual IMutableFLogBufferingFormatAppender OwningTypeAppender => (IMutableFLogBufferingFormatAppender)OwningAppender;

    public string TargetName { get; private set; } = null!;

    public bool BufferingEnabled
    {
        get => bufferingEnabled;
        set
        {
            var wasEnabled = bufferingEnabled;
            bufferingEnabled = value;

            if (BufferingEnabled)
            {
                if (AutoFlushIntervalTimeSpan > TimeSpan.Zero && autoFlushTimer == null)
                    autoFlushTimer = FLogContext.Context.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
                CreateFormatWriters(TargetName, FLogContext.Context);
            }
            else
            {
                if (wasEnabled)
                {
                    allBuffersFlushedEvent.Reset();
                    DrainCurrentBufferAndRetire();
                }
            }
        }
    }

    public TimeSpan AutoFlushIntervalTimeSpan
    {
        get => autoFlushIntervalTimeSpan;
        set
        {
            if (autoFlushIntervalTimeSpan == value) return;
            autoFlushIntervalTimeSpan = value;
            if (autoFlushIntervalTimeSpan > TimeSpan.Zero)
            {
                autoFlushTimer?.Cancel();
                autoFlushTimer
                    = FLogContext.NullOnUnstartedContext?.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
            }
        }
    }

    public int FlushWhenBufferLength { get; set; }

    public DateTime LastFlushAt { get; set; }

    public TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; set; }

    public bool HasUnflushedBufferedWriters => buffer1?.BufferedChars > 0 || buffer2?.BufferedChars > 0;

    public override void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        if (toReturn is IBufferedFormatWriter returnBufferedFormatWriter)
            ReturnBufferedWriter(returnBufferedFormatWriter);
        else
            base.TryToReturnUsedFormatWriter(toReturn);
        CheckHasFormatWriterRequests();
    }

    public void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        LastFlushAt = TimeContext.UtcNow;
        OwningTypeAppender.BufferFlushingAsyncClient.SendToFlushBufferToAppender(toFlush);
        CheckAnyQueuedRequesterOrReturnWriter(toFlush);
    }

    public override void Close()
    {
        base.Close();
        DrainCurrentBufferAndRetire();
    }

    protected override void CreateFormatWriters(string targetName, IFLogContext context)
    {
        if (BufferingEnabled)
        {
            if (HasUnflushedBufferedWriters)
            {
                BufferingEnabled = false;
                DrainCurrentBufferAndRetire();
                BufferingEnabled = true;
            }
            currentBufferCounter = 1;
            maxBuffers           = 1;

            var bufferFlusherWriter = OwningTypeAppender.CreatedDirectFormatWriter(context, targetName, OnReturningFormatWriter);

            buffer1 = onCreateBuffer1Instance
                = OwningTypeAppender.CreateBufferedFormatWriter(bufferFlusherWriter, targetName, 1, OnReturningFormatWriter);
            if (OwningTypeAppender.GetAppenderConfig().EnableDoubleBufferToggling)
            {
                maxBuffers = 2;

                buffer2 = onCreateBuffer2Instance
                    = OwningTypeAppender.CreateBufferedFormatWriter(bufferFlusherWriter, targetName, 2, OnReturningFormatWriter);
            }
            WriteTriggeredFlushIntervalTimeSpan = OwningTypeAppender.GetAppenderConfig().FlushConfig.WriteTriggeredAfterTimeSpan.ToTimeSpan();

            if (autoFlushIntervalTimeSpan > TimeSpan.Zero)
            {
                autoFlushTimer?.Cancel();
                autoFlushTimer = context.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
            }
            AutoFlushIntervalTimeSpan = OwningTypeAppender.GetAppenderConfig().FlushConfig.AutoTriggeredAfterTimeSpan.ToTimeSpan();
        }
        else
        {
            base.CreateFormatWriters(targetName, context);
        }
    }

    protected override IFormatWriter? TrgGetFormatWriter(IFLogEntry fLogEntry)
    {
        if (BufferingEnabled) return TryGetSpecificBufferedWriter(currentBufferCounter);
        if (HasUnflushedBufferedWriters) allBuffersFlushedEvent.WaitOne(); // draining the previous buffered setting first
        return base.TrgGetFormatWriter(fLogEntry);
    }

    public IBufferedFormatWriter? TryGetSpecificBufferedWriter(int bufferNumber)
    {
        var bufferSlot = ActiveBufferNum(bufferNumber);
        switch (bufferSlot)
        {
            case 0: return Interlocked.CompareExchange(ref buffer2, null, buffer2);
            case 1: return Interlocked.CompareExchange(ref buffer1, null, buffer1);
        }
        return null;
    }

    private int ActiveBufferNum(int bufferNumber) => OwningTypeAppender.UsingDoubleBuffering ? bufferNumber % maxBuffers : 1;

    private void ReturnBufferedWriter(IBufferedFormatWriter returnBufferedFormatWriter)
    {
        var bufferSlot = returnBufferedFormatWriter.BufferNum % maxBuffers;
        switch (bufferSlot)
        {
            case 0:  buffer2 = returnBufferedFormatWriter; break;
            case 1:  buffer1 = returnBufferedFormatWriter; break;
            default: throw new Exception("Unknown buffer slot");
        }
    }

    protected virtual bool ShouldFlushBuffer(IBufferedFormatWriter setReadyOrFlush) =>
        setReadyOrFlush.BufferedChars > FlushWhenBufferLength
     || LastFlushAt + WriteTriggeredFlushIntervalTimeSpan < TimeContext.UtcNow
     || !BufferingEnabled;

    protected void CheckHasFormatWriterRequests()
    {
        if (HasRequests) CheckFormatWriteAvailableForNextQueued();
    }

    private void CheckAnyQueuedRequesterOrReturnWriter(IBufferedFormatWriter toReturn)
    {
        if (BufferingEnabled && TrySendFormatWriteToNextIfAny(toReturn) != null) return;
        TryToReturnUsedFormatWriter(toReturn);
    }

    protected void FlushAndCheckNextBufferAvailable(IBufferedFormatWriter toFlush)
    {
        Interlocked.Increment(ref currentBufferCounter);
        OwningTypeAppender.BufferFlushingAsyncClient.SendToFlushBufferToAppender(toFlush);
        if (OwningTypeAppender.UsingDoubleBuffering) CheckFormatWriteAvailableForNextQueued();
    }

    protected override ISyncLock GetFormatWriterRequesterWaitStrategy()
    {
        if (BufferingEnabled) return RequesterRecycler.Borrow<SpinLockLight>();
        return RequesterRecycler.Borrow<ManualResetEventLock>();
    }

    protected virtual void DrainCurrentBufferAndRetire()
    {
        for (var i = 0; i < maxBuffers; i++)
        {
            IBufferedFormatWriter? bufferedWriter;
            while ((bufferedWriter = TryGetSpecificBufferedWriter(i)) == null) Thread.Sleep(10);
            if (bufferedWriter.BufferedChars > 0)
            {
                OwningTypeAppender.BufferFlushingAsyncClient.SendToFlushBufferToAppender(bufferedWriter);
                while (TryGetSpecificBufferedWriter(i) == null) // wait for buffer to return flushed
                    Thread.Sleep(10);
            }
        }
        allBuffersFlushedEvent.Set();
    }

    protected override void WriterFinishedWithBuffer(IFormatWriter setReadyOrFlush)
    {
        if (setReadyOrFlush is IBufferedFormatWriter bufferedFormatWriter)
        {
            if (ShouldFlushBuffer(bufferedFormatWriter))
                FlushAndCheckNextBufferAvailable(bufferedFormatWriter);
            else
                TrySendFormatWriteToNextIfAny(bufferedFormatWriter);
        }
        else
        {
            base.WriterFinishedWithBuffer(setReadyOrFlush);
        }
    }

    protected void TimerTriggeredCheckFormatWriterRequests()
    {
        OwningTypeAppender.BufferFlushingAsyncClient.RunJobOnAppenderQueue(checkWaitingFormatWriterRequests);
    }

    public void AutoFlushTimerHandler(IScheduleActualTime? scheduleActualTime)
    {
        lock (timerSyncLock)
        {
            if (!BufferingEnabled)
            {
                autoFlushTimer?.Cancel();
                autoFlushTimer = null;
            }
            else if (LastFlushAt + AutoFlushIntervalTimeSpan < (scheduleActualTime?.ScheduleTime ?? TimeContext.UtcNow))
            {
                var currentBuffer = TryGetSpecificBufferedWriter(currentBufferCounter);
                if (currentBuffer is { BufferedChars: > 0 })
                {
                    FlushAndCheckNextBufferAvailable(currentBuffer);
                }
                else
                {
                    currentBuffer = TryGetSpecificBufferedWriter(currentBufferCounter + 1);
                    if (currentBuffer is { BufferedChars: > 0 })
                        FlushAndCheckNextBufferAvailable(currentBuffer);
                    else
                        CheckHasFormatWriterRequests();
                }
            }
        }
    }
}
