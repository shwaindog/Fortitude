using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;


public interface IFLogFormattingAppender : IFLogAppender
{
    IBlockingFormatWriterResolverHandle FormatWriterResolver { get; }

    int FormatWriterRequestQueue { get; }

    long TotalBytesAppended { get; }

    long TotalCharsAppended { get; }

    FormattingAppenderSinkType FormatAppenderType { get; }

    IFLogEntryFormatter Formatter { get; }
}

public interface IMutableFLogFormattingAppender : IFLogFormattingAppender, IMutableFLogAppender
{
    new IFLogEntryFormatter Formatter { get; set; }
}

public delegate void FormatWriterReceivedHandler<in T>(T formatWriter) where T : IFormatWriter;

public abstract class FLogFormattingAppender : FLogAppender, IMutableFLogFormattingAppender
{
    [ThreadStatic] private static IRecycler? requesterThreadRecycler;

    private readonly DoublyLinkedList<BlockingFormatWriterResolverHandle> queuedRequests = new();
    
    protected IFormatWriter? DirectFormatWriter;

    protected          Action<IBlockingFormatWriterResolverHandle> RequestHandleDisposed;

    protected readonly FormatWriterReceivedHandler<IFormatWriter>  OnReturningFormatWriter;

    private readonly ISyncLock queueProtectLock = new SpinLockLight();

    protected FLogFormattingAppender(IFormattingAppenderConfig formattingAppenderConfig, IFLogContext context)
        : base(formattingAppenderConfig, context)
    {
        Formatter             = new FLogEntryFormatter(formattingAppenderConfig.LogEntryFormatLayout, this);

        RequestHandleDisposed = WriterHandleDispose;

        OnReturningFormatWriter = WriterFinishedWithBuffer;
    }

    public IFLogEntryFormatter Formatter { get; set; }

    protected IRecycler RequesterRecycler => requesterThreadRecycler ??= new Recycler();

    protected bool      HasRequests       => queuedRequests.Head != null;

    public virtual int FormatWriterRequestQueue => queuedRequests.Count;

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        if (logEntryEvent.LogEntryEventType == LogEntryEventType.SingleEntry)
        {
            var fLogEntry = logEntryEvent.LogEntry;
            if (fLogEntry != null)
            {
                Formatter.ApplyFormatting(fLogEntry);
            }
        }
        else
        {
            var logEntriesBatch = logEntryEvent.LogEntriesBatch;
            var count = logEntriesBatch?.Count ?? 0;
            for (var i = 0; i < count; i++)
            {
                var flogEntry = logEntriesBatch![i];
                Formatter.ApplyFormatting(flogEntry);
            }
        }
    }
    
    protected virtual IFormatWriter? TrgGetFormatWriter => Interlocked.CompareExchange(ref DirectFormatWriter, null, DirectFormatWriter);

    public virtual IBlockingFormatWriterResolverHandle FormatWriterResolver
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

    protected virtual void WriterFinishedWithBuffer(IFormatWriter setReadyOrFlush)
    {
        TrySendFormatWriteToNextIfAny(setReadyOrFlush);
    }

    protected virtual void WriterHandleDispose(IBlockingFormatWriterResolverHandle actualHandle)
    {
        // to do schedule this check
        if (actualHandle is { IsAvailable: true, WasTaken: false }) { }

        actualHandle.DecrementRefCount();
    }

    protected virtual ISyncLock GetFormatWriterRequesterWaitStrategy()
    {
        return RequesterRecycler.Borrow<ManualResetEventLock>();
    }

    protected BlockingFormatWriterResolverHandle AddToQueue(BlockingFormatWriterResolverHandle newRequest)
    {
        using var queueLock = queueProtectLock;
        queueProtectLock.Acquire();

        queuedRequests.AddLast(newRequest);

        return newRequest;
    }

    protected BlockingFormatWriterResolverHandle? TrySendFormatWriteToNextIfAny(IFormatWriter toSend)
    {
        BlockingFormatWriterResolverHandle? removedFirst = null;
        using (queueProtectLock)
        {
            queueProtectLock.Acquire();

            if (queuedRequests.Head != null)
            {
                removedFirst = queuedRequests.Remove(queuedRequests.Head);
            }
        }
        if (removedFirst == null)
        {
            TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        removedFirst.ReceiveFormatWriterHandler(toSend);
        return removedFirst;
    }

    protected virtual void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        DirectFormatWriter = toReturn;
    }

    public abstract FormattingAppenderSinkType FormatAppenderType { get; }

    protected abstract IFormatWriter CreatedDirectFormatWriter(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig);

    public long TotalBytesAppended { get; protected set; }

    public long TotalCharsAppended { get; protected set; }
}
