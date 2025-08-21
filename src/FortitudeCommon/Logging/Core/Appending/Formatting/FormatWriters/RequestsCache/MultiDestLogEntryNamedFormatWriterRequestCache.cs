using System.Runtime.InteropServices.ComTypes;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public record TargetRequestCacheExpiry
(
    SingleDestBufferedFormatWriterRequestCache DestRequestCache
  , DateTime CloseTime)
{
    public ITimerUpdate? CloseDestinationTimerHandle { get; set; }
}

public class MultiDestLogEntryNamedFormatWriterRequestCache : IFormatWriterRequestCache
{
    private IMultiDestinationFormattingAppender owningAppender = null!;

    private IFLogContext createContext;

    private int attemptLogToClosedDestinationCount;

    private object CreateFileDestSyncLock = new();

    private CharSpanAcceptingStringMap<TargetRequestCacheExpiry> targetToCacheMap = new();
    private CharSpanAcceptingStringMap<DateTime> closeTargetToCacheMap = new();

    private   Action<TargetRequestCacheExpiry?>           RunCloseDestinationAfterExpiryAction;
    
    protected Action<IBlockingFormatWriterResolverHandle> CloseRequestHandleDisposed;
    
    protected PassThroughSyncLock closedDestDummySyncLock = new();

    public MultiDestLogEntryNamedFormatWriterRequestCache Initialize(IMultiDestinationFormattingAppender owningAppender, IFLogContext context)
    {
        this.owningAppender = owningAppender;
        createContext       = context;

        RunCloseDestinationAfterExpiryAction = DestinationCloseCallback;
        
        return this;
    }

    private void DestinationCloseCallback(TargetRequestCacheExpiry? toClose)
    {
        if (toClose != null)
        {
            var targetName = toClose.DestRequestCache.TargetName;
            if (targetToCacheMap.Remove(targetName))
            {
                toClose.CloseDestinationTimerHandle?.Dispose();
                toClose.DestRequestCache.Close();
                owningAppender.ReceiveNotificationTargetClose(targetName);
                toClose.DestRequestCache.DecrementRefCount();
                closeTargetToCacheMap.AddOrUpdate(targetName, TimeContext.UtcNow);
            }
        }
    }

    private void DestinationCloseCallback(IBlockingFormatWriterResolverHandle dummyHandle)
    {
        dummyHandle.DecrementRefCount();
    }

    public IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        var pathResolver = owningAppender.PathResolver;
        var destPath     = pathResolver.ResolvePathFor(logEntry);
        if (!targetToCacheMap.TryGetValue(destPath, out var cacheEntry))
        {
            lock (CreateFileDestSyncLock)
            {
                if (closeTargetToCacheMap.TryGetValue(destPath, out var closedAt))
                {
                    
                    var requestHandle =
                        Recycler.ThreadStaticRecycler
                            .Borrow<BlockingFormatWriterResolverHandle>()
                            .Initialize(logEntry, owningAppender, CloseRequestHandleDisposed, closedDestDummySyncLock, null);
                    requestHandle.IssueRequestAborted();
                    pathResolver.DecrementRefCount();
                    attemptLogToClosedDestinationCount++;
                    return requestHandle;
                }
                if (!targetToCacheMap.TryGetValue(destPath, out cacheEntry))
                {
                    var materializedPathName = new String(destPath);

                    var destCache     = owningAppender.GetWriterRequestCache(materializedPathName);
                    var pathTimeParts = pathResolver.GetPathDateTimePartFlags();
                    var expiryTime    = pathTimeParts.NextDifferenceFlagTime(logEntry.LogDateTime) ?? DateTime.MaxValue;
                    var closeTime     = expiryTime.Add(owningAppender.ExpiryToCloseDelay);

                    cacheEntry = new TargetRequestCacheExpiry(destCache, closeTime);
                    cacheEntry.CloseDestinationTimerHandle = createContext.AsyncRegistry.LoggerTimers.RunAt(closeTime, cacheEntry, RunCloseDestinationAfterExpiryAction);
                    targetToCacheMap.AddOrUpdate(materializedPathName, cacheEntry);
                }
            }
        }
        pathResolver.DecrementRefCount();
        return cacheEntry!.DestRequestCache.FormatWriterResolver(logEntry);
    }

    public int FormatWriterRequestQueue
    {
        get
        {
            int countQueue = 0;
            foreach (var targetRequestCacheExpiry in targetToCacheMap.Values)
            {
                countQueue += targetRequestCacheExpiry.DestRequestCache.FormatWriterRequestQueue;
            }
            return countQueue;
        }
    }

    public void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        if (!targetToCacheMap.TryGetValue(toReturn.TargetName, out var cacheEntry))
        {
            Console.Out.WriteLine($"Attempted to return a format writer to a target that doesn't or no longer exists {toReturn.TargetName}");
            return;
        }
        cacheEntry!.DestRequestCache.TryToReturnUsedFormatWriter(toReturn);
    }

    public bool IsOpen => owningAppender.IsOpen;

    public void Close()
    {
        foreach (var targetRequestCacheExpiry in targetToCacheMap.Values)
        {
            DestinationCloseCallback(targetRequestCacheExpiry);
        }
    }
}
