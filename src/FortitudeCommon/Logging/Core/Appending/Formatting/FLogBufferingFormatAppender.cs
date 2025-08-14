// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IFLogBufferingFormatAppender : IFLogFormattingAppender
{
    bool BufferingEnabled { get; }

    int CharBufferSize { get; }

    int FlushWhenBufferLength { get; }

    TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; }

    TimeSpan AutoFlushIntervalTimeSpan { get; }

    DateTime LastFlushAt { get; }

    void FlushBufferToAppender(IBufferedFormatWriter toFlush);
}

public interface IMutableFLogBufferingFormatAppender : IFLogBufferingFormatAppender, IMutableFLogFormattingAppender
{
    new bool BufferingEnabled { get; set; }

    new int CharBufferSize { get; set; }

    new int FlushWhenBufferLength { get; set; }

    new TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; set; }

    new TimeSpan AutoFlushIntervalTimeSpan { get; set; }

    new DateTime LastFlushAt { get; set; }
}

public abstract class FLogBufferingFormatAppender : FLogFormattingAppender, IMutableFLogBufferingFormatAppender
{
    private IBufferedFormatWriter? onCreateBuffer1Instance;
    private IBufferedFormatWriter? onCreateBuffer2Instance;

    private int currentBufferCounter;
    private int maxBuffers;

    private IBufferedFormatWriter? buffer1;
    private IBufferedFormatWriter? buffer2;
    // protected IBufferedFormatWriter? ToFlush;

    private TimeSpan autoFlushIntervalTimeSpan;

    protected ITimerUpdate? AutoFlushTimer;

    protected ManualResetEvent AllBuffersFlushedEvent = new(true);

    private bool bufferingEnabled;


    protected Action CheckWaitingFormatWriterRequests;
    protected Action TimerCheckWaitingFormatWriterRequests;

    protected FLogBufferingFormatAppender(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig, IFLogContext context)
        : base(bufferingFormatAppenderConfig, context)
    {
        BufferingEnabled = !bufferingFormatAppenderConfig.DisableBuffering;

        FlushWhenBufferLength = (int)(bufferingFormatAppenderConfig.FlushConfig.WriteTriggeredAtBufferPercentage *
                                      bufferingFormatAppenderConfig.CharBufferSize);
        AutoFlushIntervalTimeSpan = bufferingFormatAppenderConfig.FlushConfig.AutoTriggeredAfterTimeSpan.ToTimeSpan();

        if (autoFlushIntervalTimeSpan > TimeSpan.Zero)
        {
            AutoFlushTimer?.Cancel();
            AutoFlushTimer = context.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
        }

        WriteTriggeredFlushIntervalTimeSpan = bufferingFormatAppenderConfig.FlushConfig.WriteTriggeredAfterTimeSpan.ToTimeSpan();


        CheckWaitingFormatWriterRequests      = CheckHasFormatWriterRequests;
        TimerCheckWaitingFormatWriterRequests = TimerTriggeredCheckFormatWriterRequests;

        CreateFormatWriters(bufferingFormatAppenderConfig);
    }

    public int CharBufferSize { get; set; }

    public bool UsingDoubleBuffering { get; set; }

    public int FlushWhenBufferLength { get; set; }

    public DateTime LastFlushAt { get; set; }

    private bool HasUnflushedBufferedWriters => buffer1?.Buffered > 0 || buffer2?.Buffered > 0;

    public TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; set; }

    public TimeSpan AutoFlushIntervalTimeSpan
    {
        get => autoFlushIntervalTimeSpan;
        set
        {
            if (autoFlushIntervalTimeSpan == value) return;
            autoFlushIntervalTimeSpan = value;
            if (autoFlushIntervalTimeSpan > TimeSpan.Zero)
            {
                AutoFlushTimer?.Cancel();
                AutoFlushTimer
                    = FLogContext.NullOnUnstartedContext?.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
            }
        }
    }


    protected IBufferFlushAppenderAsyncClient BufferFlushingAsyncClient => (IBufferFlushAppenderAsyncClient)AsyncClient;

    public bool BufferingEnabled
    {
        get => bufferingEnabled;
        set
        {
            var wasEnabled = bufferingEnabled;
            bufferingEnabled = value;

            if (BufferingEnabled)
            {
                if (AutoFlushIntervalTimeSpan > TimeSpan.Zero && AutoFlushTimer == null)
                {
                    AutoFlushTimer = FLogContext.Context.AsyncRegistry.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
                }
                CreateFormatWriters(GetAppenderConfig());
            }
            else
            {
                if (wasEnabled)
                {
                    AllBuffersFlushedEvent.Reset();
                    DrainCurrentBufferAndRetire();
                }
            }
        }
    }

    protected override IFormatWriter? TrgGetFormatWriter
    {
        get
        {
            if (BufferingEnabled)
            {
                return TryGetSpecificBufferedWriter(currentBufferCounter);
            }
            if (HasUnflushedBufferedWriters)
            {
                AllBuffersFlushedEvent.WaitOne(); // draining the previous buffered setting first
            }
            return base.TrgGetFormatWriter;
        }
    }

    public override IBlockingFormatWriterResolverHandle FormatWriterResolver
    {
        get
        {
            IFormatWriter? useFormatWriter = TrgGetFormatWriter;

            var requesterSync = GetFormatWriterRequesterWaitStrategy();

            var requestHandle = RequesterRecycler.Borrow<BlockingFormatWriterResolverHandle>()
                                                 .Initialize(this, RequestHandleDisposed, requesterSync, useFormatWriter);
            if (useFormatWriter == null)
            {
                requesterSync.Acquire(0);

                return AddToQueue(requestHandle);
            }
            return requestHandle;
        }
    }

    protected override IBufferFlushAppenderAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var bufferConfig = (IBufferingFormatAppenderConfig)appenderDefinitionConfig;

        var bufferAsync = bufferConfig.FlushAsyncQueueNumber;

        var bufferFlushingAsyncClient =
            new BufferFlushAppenderAsyncClient(this, processAsync, asyncRegistry, bufferAsync);
        return bufferFlushingAsyncClient;
    }

    protected virtual void CreateFormatWriters(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig)
    {
        CharBufferSize       = bufferingFormatAppenderConfig.CharBufferSize;
        UsingDoubleBuffering = bufferingFormatAppenderConfig.EnableDoubleBufferToggling;
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

            buffer1 = onCreateBuffer1Instance = new BufferedFormatWriter(this, 1, OnReturningFormatWriter);
            if (bufferingFormatAppenderConfig.EnableDoubleBufferToggling)
            {
                maxBuffers = 2;

                buffer2 = onCreateBuffer2Instance = new BufferedFormatWriter(this, 2, OnReturningFormatWriter);
            }
        }
        else
        {
            DirectFormatWriter = CreatedDirectFormatWriter(bufferingFormatAppenderConfig);
        }
    }

    protected override void WriterFinishedWithBuffer(IFormatWriter setReadyOrFlush)
    {
        if (setReadyOrFlush is IBufferedFormatWriter bufferedFormatWriter)
        {
            if (ShouldFlushBuffer(bufferedFormatWriter))
            {
                FlushAndCheckNextBufferAvailable(bufferedFormatWriter);
            }
            else
            {
                TrySendFormatWriteToNextIfAny(bufferedFormatWriter);
            }
        }
        else
        {
            base.WriterFinishedWithBuffer(setReadyOrFlush);
        }
    }

    protected virtual bool ShouldFlushBuffer(IBufferedFormatWriter setReadyOrFlush)
    {
        return setReadyOrFlush.Buffered > FlushWhenBufferLength
            || (LastFlushAt + WriteTriggeredFlushIntervalTimeSpan) < TimeContext.UtcNow
            || !BufferingEnabled;
    }

    private IBufferedFormatWriter? TryGetSpecificBufferedWriter(int bufferNumber)
    {
        var bufferSlot = UsingDoubleBuffering ? bufferNumber % maxBuffers : 1;
        switch (bufferSlot)
        {
            case 0: return Interlocked.CompareExchange(ref buffer2, null, buffer2);
            case 1: return Interlocked.CompareExchange(ref buffer1, null, buffer1);
        }
        return null;
    }

    private void ReturnBufferedWriter(IBufferedFormatWriter returnBufferedFormatWriter)
    {
        var bufferSlot = returnBufferedFormatWriter.BufferNum;
        switch (bufferSlot)
        {
            case 0: buffer2 = returnBufferedFormatWriter; break;
            case 1: buffer1 = returnBufferedFormatWriter; break;
        }
    }

    protected void TimerTriggeredCheckFormatWriterRequests()
    {
        AsyncClient.RunJobOnAppenderQueue(CheckWaitingFormatWriterRequests);
    }

    protected void CheckHasFormatWriterRequests()
    {
        if (HasRequests)
        {
            var useFormatWriter = TrgGetFormatWriter;
            if (useFormatWriter != null)
            {
                TrySendFormatWriteToNextIfAny(useFormatWriter);
            }
        }
    }

    protected override void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        if (toReturn is IBufferedFormatWriter returnBufferedFormatWriter)
        {
            ReturnBufferedWriter(returnBufferedFormatWriter);
        }
        else
        {
            base.TryToReturnUsedFormatWriter(toReturn);
        }
    }

    private void CheckAnyQueuedRequesterOrReturnWriter(IBufferedFormatWriter toReturn)
    {
        if (BufferingEnabled && TrySendFormatWriteToNextIfAny(toReturn) != null) return;
        TryToReturnUsedFormatWriter(toReturn);
    }

    protected void FlushAndCheckNextBufferAvailable(IBufferedFormatWriter toFlush)
    {
        Interlocked.Increment(ref currentBufferCounter);
        BufferFlushingAsyncClient.SendToFlushBufferToAppender(toFlush, this);
        if (UsingDoubleBuffering)
        {
            var nextBuffer = TrgGetFormatWriter;
            if (nextBuffer != null)
            {
                if (TrySendFormatWriteToNextIfAny(nextBuffer) == null)
                {
                    TryToReturnUsedFormatWriter(nextBuffer);
                }
            }
        }
    }

    protected override ISyncLock GetFormatWriterRequesterWaitStrategy()
    {
        if (BufferingEnabled)
        {
            return RequesterRecycler.Borrow<SpinLockLight>();
        }
        return RequesterRecycler.Borrow<ManualResetEventLock>();
    }

    protected virtual void DrainCurrentBufferAndRetire()
    {
        for (int i = 0; i < maxBuffers; i++)
        {
            IBufferedFormatWriter? bufferedWriter;
            while ((bufferedWriter = TryGetSpecificBufferedWriter(i)) == null)
            {
                Thread.Sleep(10);
            }
            if (bufferedWriter.Buffered > 0)
            {
                BufferFlushingAsyncClient.SendToFlushBufferToAppender(bufferedWriter, this);
                while (TryGetSpecificBufferedWriter(i) == null) // wait for buffer to return flushed
                {
                    Thread.Sleep(10);
                }
            }
        }
        AllBuffersFlushedEvent.Set();
    }

    public void AutoFlushTimerHandler(IScheduleActualTime? scheduleActualTime)
    {
        if (!BufferingEnabled)
        {
            AutoFlushTimer?.Cancel();
            AutoFlushTimer = null;
        }
        else if (LastFlushAt + AutoFlushIntervalTimeSpan < (scheduleActualTime?.ScheduleTime ?? TimeContext.UtcNow))
        {
            var currentBuffer = TryGetSpecificBufferedWriter(currentBufferCounter);
            if (currentBuffer is { Buffered: > 0 })
            {
                FlushAndCheckNextBufferAvailable(currentBuffer);
            }
        }
    }

    public void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        LastFlushAt = TimeContext.UtcNow;
        FlushBuffer(toFlush);
        CheckAnyQueuedRequesterOrReturnWriter(toFlush);
    }

    public abstract void FlushBuffer(IBufferedFormatWriter toFlush);

    public override IBufferingFormatAppenderConfig GetAppenderConfig() => (IBufferingFormatAppenderConfig)AppenderConfig;
}
