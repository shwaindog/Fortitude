// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public record TargetRequestCacheExpiry(SingleDestBufferedFormatWriterRequestCache DestRequestCache, DateTime CloseTime)
{
    public ITimerUpdate? CloseDestinationTimerHandle { get; set; }
}

public class MultiDestLogEntryNamedFormatWriterRequestCache : IBufferedFormatWriterRequestCache
{
    private readonly PassThroughSyncLock closedDestDummySyncLock = new();

    private readonly CharSpanAcceptingStringMap<DateTime> closeTargetToCacheMap = new();

    private readonly object createFileDestSyncLock = new();

    private readonly CharSpanAcceptingStringMap<TargetRequestCacheExpiry> targetToCacheMap = new();

    private bool bufferingEnabled;

    protected Action<IBlockingFormatWriterResolverHandle> CloseRequestHandleDisposed;

    private IFLogContext                        createContext  = null!;
    private IMultiDestinationFormattingAppender owningAppender = null!;

    protected Action<TargetRequestCacheExpiry?> RunCloseDestinationAfterExpiryAction;

    public MultiDestLogEntryNamedFormatWriterRequestCache()
    {
        CloseRequestHandleDisposed           = FinishedWithDummyHandle;
        RunCloseDestinationAfterExpiryAction = DestinationCloseCallback;
    }

    public MultiDestLogEntryNamedFormatWriterRequestCache Initialize(IMultiDestinationFormattingAppender owningMultiDestAppender
      , IFLogContext context)
    {
        owningAppender = owningMultiDestAppender;

        createContext = context;

        RunCloseDestinationAfterExpiryAction = DestinationCloseCallback;

        return this;
    }

    public bool BufferingEnabled
    {
        get => bufferingEnabled;
        set
        {
            if (value == bufferingEnabled) return;
            bufferingEnabled = value;
            foreach (var keyValuePair in targetToCacheMap) keyValuePair.Value.DestRequestCache.BufferingEnabled = value;
        }
    }

    public void FlushBufferToAppender(IBufferedFormatWriter toFlush)
    {
        if (targetToCacheMap.TryGetValue(toFlush.TargetName, out var targetRequestCacheExpiry))
            targetRequestCacheExpiry!.DestRequestCache.FlushBufferToAppender(toFlush);
        if (closeTargetToCacheMap.TryGetValue(toFlush.TargetName, out var closeAtRequestCache))
            Console.Out.WriteLine($"Attempted to flush a format writer to a target that was closed at {closeAtRequestCache} for target : {toFlush.TargetName}");
        Console.Out.WriteLine($"Attempted to flush a format writer to a target with that no longer or never existed : {toFlush.TargetName}");
    }

    public IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        var pathResolver = owningAppender.PathResolver;
        var destPath     = pathResolver.ResolvePathFor(logEntry);
        if (!targetToCacheMap.TryGetValue(destPath, out var cacheEntry))
            lock (createFileDestSyncLock)
            {
                if (closeTargetToCacheMap.TryGetValue(destPath, out _))
                {
                    var requestHandle =
                        Recycler.ThreadStaticRecycler
                                .Borrow<BlockingFormatWriterResolverHandle>()
                                .Initialize(logEntry, Thread.CurrentThread, owningAppender, CloseRequestHandleDisposed, closedDestDummySyncLock);
                    requestHandle.IssueRequestAborted();
                    pathResolver.DecrementRefCount();

                    owningAppender.IncrementLogEntriesDropped();

                    return requestHandle;
                }
                if (!targetToCacheMap.TryGetValue(destPath, out cacheEntry))
                {
                    var materializedPathName = new string(destPath);

                    var destCache     = owningAppender.GetWriterRequestCache(materializedPathName);
                    var pathTimeParts = pathResolver.GetPathDateTimePartFlags();
                    var expiryTime    = pathTimeParts.NextDifferenceFlagTime(logEntry.LogDateTime) ?? DateTime.MaxValue;
                    var closeTime     = expiryTime != DateTime.MaxValue ? expiryTime.Add(owningAppender.ExpiryToCloseDelay) : expiryTime;

                    cacheEntry = new TargetRequestCacheExpiry(destCache, closeTime);
                    if (closeTime != DateTime.MaxValue)
                        cacheEntry.CloseDestinationTimerHandle
                            = createContext.AsyncRegistry.LoggerTimers.RunAt(closeTime, cacheEntry, RunCloseDestinationAfterExpiryAction);
                    targetToCacheMap.AddOrUpdate(materializedPathName, cacheEntry);
                }
            }
        pathResolver.DecrementRefCount();
        return cacheEntry!.DestRequestCache.FormatWriterResolver(logEntry);
    }

    public int FormatWriterRequestQueue
    {
        get
        {
            var countQueue = 0;
            foreach (var targetRequestCacheExpiry in targetToCacheMap.Values)
                countQueue += targetRequestCacheExpiry.DestRequestCache.FormatWriterRequestQueue;
            return countQueue;
        }
    }

    public void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        if (!targetToCacheMap.TryGetValue(toReturn.TargetName, out var cacheEntry))
        {
            if (!closeTargetToCacheMap.TryGetValue(toReturn.TargetName, out var closedAtRequestCache))
                Console.Out.WriteLine($"Attempted to return a format writer to a target that doesn't exist {toReturn.TargetName}");
            Console.Out.WriteLine($"Attempted to return a format writer to a target that was closed at {closedAtRequestCache} for target : {toReturn.TargetName}");
            return;
        }
        cacheEntry!.DestRequestCache.TryToReturnUsedFormatWriter(toReturn);
    }

    public bool IsOpen => owningAppender.IsOpen;

    public void Close()
    {
        foreach (var targetRequestCacheExpiry in targetToCacheMap.Values) DestinationCloseCallback(targetRequestCacheExpiry);
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

    private void FinishedWithDummyHandle(IBlockingFormatWriterResolverHandle dummyHandle)
    {
        dummyHandle.DecrementRefCount();
    }
}
