// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IFLogBufferingFormatAppender : IFLogFormattingAppender
{
    bool BufferingEnabled { get; }

    int CharBufferSize { get; }

    int FlushWhenBufferLength { get; }

    TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; }

    TimeSpan AutoFlushIntervalTimeSpan { get; }

    DateTime LastFlushAt { get; }
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

public interface IFLogAsyncTargetFlushBufferAppender : IMutableFLogBufferingFormatAppender
{
    IBufferedFormatWriter? ReadyForSwap { get; set; }

    IBufferedFormatWriter? ToFlushBuffer { get; set; }

    void FlushBufferToAppender(IBufferedFormatWriter toFlush);
}

public abstract class FLogBufferingFormatAppender : FLogFormattingAppender, IFLogAsyncTargetFlushBufferAppender
{
    [ThreadStatic] private static IRecycler? requesterThreadRecycler;

    protected IBufferedFormatWriter? CurrentBufferedWriter;
    protected IBufferedFormatWriter? ReadyForToggle;
    protected IBufferedFormatWriter? ToFlush;

    protected IFormatWriter? DirectFormatWriter;

    private int outStandingRequestsCount;

    private int charBufferSize;

    private TimeSpan autoFlushIntervalTimeSpan;

    protected ITimerUpdate? AutoFlushTimer;

    protected ManualResetEvent AllWritersEvent = new(true);

    private bool bufferingEnabled;

    private ISyncLock currentFormatWriterAvailableSyncLock = new AutoResetEventLock(false);

    protected readonly FormatWriterReceivedHandler<IBufferedFormatWriter> OnReturningFormatWriter;

    private DoublyLinkedList<BlockingFormatWriterResolverHandle> queuedRequests = new();


    protected Action<IBlockingFormatWriterResolverHandle> RequestHandleDisposed;

    protected FLogBufferingFormatAppender(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig, IFLogContext context)
        : base(bufferingFormatAppenderConfig, context)
    {
        BufferingEnabled        = !bufferingFormatAppenderConfig.DisableBuffering;
        OnReturningFormatWriter = WriterFinishedWithBuffer;
        RequestHandleDisposed   = WriterHandleDispose;
    }

    protected virtual void CreateBufferedFormatWriters(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig)
    {
        CharBufferSize = bufferingFormatAppenderConfig.CharBufferSize;
        if (bufferingFormatAppenderConfig.EnableDoubleBufferToggling)
        {
            if (ReadyForToggle == null || ReadyForToggle.Buffered < CharBufferSize)
            {
                if (ReadyForToggle == null)
                {
                    ReadyForToggle = new BufferedFormatWriter(this, OnReturningFormatWriter);
                }
                else
                {
                    if (CharBufferSize > ReadyForToggle.Buffered)
                    {
                        ReadyForToggle.EnsureCapacity(charBufferSize);
                    }
                }
            }
        }
    }

    protected abstract IFormatWriter CreatedImmediateFormatWriter(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig);

    public int CharBufferSize
    {
        get => charBufferSize;
        set
        {
            charBufferSize = value;
            if (CurrentBufferedWriter == null || CurrentBufferedWriter.Buffered < charBufferSize)
            {
                if (CurrentBufferedWriter == null)
                {
                    CurrentBufferedWriter = new BufferedFormatWriter(this, OnReturningFormatWriter);
                }
                else
                {
                    if (charBufferSize > CurrentBufferedWriter.Buffered)
                    {
                        CurrentBufferedWriter.EnsureCapacity(charBufferSize);
                    }
                }
            }
        }
    }

    protected void WriterHandleDispose(IBlockingFormatWriterResolverHandle actualHandle)
    {
        // to do schedule this check
        if (actualHandle is { IsAvailable: true, WasTaken: false }) { }
    }

    protected void WriterFinishedWithBuffer(IBufferedFormatWriter setReadyOrFlush)
    {
        if (setReadyOrFlush.Buffered > FlushWhenBufferLength && ToFlushBuffer == null)
        {
            if (ToFlushBuffer == null)
            {
                ToFlushBuffer = setReadyOrFlush;

                var nextBuffer = Interlocked.CompareExchange(ref ReadyForToggle, null, ReadyForToggle);
                if (nextBuffer != null)
                {
                    SendFormatWriteToNextIfAny(nextBuffer);
                    return;
                }
            }
            // To DO Flush then replace
        }
        else
        {
            SendFormatWriteToNextIfAny(setReadyOrFlush);
        }
    }

    public override IBlockingFormatWriterResolverHandle FormatWriterResolver
    {
        get
        {
            Interlocked.Increment(ref outStandingRequestsCount);
            IFormatWriter? useFormatWriter;
            if (BufferingEnabled)
            {
                useFormatWriter =
                    Interlocked.CompareExchange(ref CurrentBufferedWriter, null, CurrentWriteBuffer);
            }
            else
            {
                useFormatWriter = CurrentBufferedWriter;
                if (useFormatWriter != null)
                {
                    AllWritersEvent.WaitOne(); // draining the previous buffered setting first
                }

                useFormatWriter =
                    Interlocked.CompareExchange(ref DirectFormatWriter, null, DirectFormatWriter);
            }
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

    private readonly ISyncLock queueProtectLock = new SpinLockLight();

    protected BlockingFormatWriterResolverHandle AddToQueue(BlockingFormatWriterResolverHandle newRequest)
    {
        using var queueLock = queueProtectLock;
        queueProtectLock.Acquire();

        queuedRequests.AddLast(newRequest);

        return newRequest;
    }

    protected BlockingFormatWriterResolverHandle? SendFormatWriteToNextIfAny(IFormatWriter toSend)
    {
        BlockingFormatWriterResolverHandle? removedFirst = null;
        using (queueProtectLock)
        {
            queueProtectLock.Acquire();

            if (queuedRequests.Head != null)
            {
                removedFirst = queuedRequests.Remove(queuedRequests.Head);

                removedFirst.ReceiveFormatWriterHandler(toSend);
            }
        }
        if (removedFirst == null && Interlocked.Decrement(ref outStandingRequestsCount) > 0)
        {
            // To do schedule check again
        }

        return removedFirst;
    }

    public override int FormatWriterRequestQueue => queuedRequests.Count;

    protected ISyncLock GetFormatWriterRequesterWaitStrategy() => RequesterRecycler.Borrow<ManualResetEventLock>();

    protected IRecycler RequesterRecycler => requesterThreadRecycler ??= new Recycler();

    protected void ScheduleImmediateDrain(IBufferedFormatWriter setReadyOrFlush) { }


    public bool BufferingEnabled
    {
        get => bufferingEnabled;
        set
        {
            var wasEnabled = bufferingEnabled;
            bufferingEnabled = value;

            if (BufferingEnabled)
            {
                CreateBufferedFormatWriters(GetAppenderConfig());
            }
            else
            {
                if (wasEnabled)
                {
                    AllWritersEvent.Reset();
                    DrainCurrentBufferAndRetire();
                }
            }
        }
    }

    protected virtual void DrainCurrentBufferAndRetire()
    {
        if (CurrentBufferedWriter?.Buffered > 0)
        {
            FlushBufferToAppender(CurrentWriteBuffer!);
        }
        CurrentBufferedWriter = null;
        ReadyForToggle        = null;
        AllWritersEvent.Set();
    }

    public IBufferedFormatWriter? CurrentWriteBuffer => CurrentBufferedWriter;

    public IBufferedFormatWriter? ToFlushBuffer
    {
        get => ToFlush;
        set => ToFlush = value;
    }

    public IBufferedFormatWriter? ReadyForSwap
    {
        get => ReadyForToggle;
        set => ReadyForToggle = value;
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
                AutoFlushTimer = FLogContext.Context.LoggerTimers.RunEvery(autoFlushIntervalTimeSpan, AutoFlushTimerHandler);
            }
        }
    }

    public void AutoFlushTimerHandler(IScheduleActualTime? scheduleActualTime)
    {
        if (LastFlushAt + AutoFlushIntervalTimeSpan < (scheduleActualTime?.ScheduleTime ?? TimeContext.UtcNow))
        {
            LastFlushAt = DateTime.UtcNow;
        }
    }

    public int FlushWhenBufferLength { get; set; }

    public DateTime LastFlushAt { get; set; }

    public TimeSpan WriteTriggeredFlushIntervalTimeSpan { get; set; }

    public abstract void FlushBufferToAppender(IBufferedFormatWriter toFlush);

    public override IBufferingFormatAppenderConfig GetAppenderConfig() => (IBufferingFormatAppenderConfig)AppenderConfig;
}
