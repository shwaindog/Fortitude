// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface ITargetingFLogEntrySource : IFLogEntryPublishChainTreeNode
{
    string Name { get; }

    IFLogEntrySink? OutBound { get; set; }

    void Insert(IFLogEntryPipelineInterceptor toInsert);

    bool Remove(IFLogEntryPipelineInterceptor toRemove);

    int RemoveAllToOutBoundTarget();

    void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource? fromSource = null);

    ITargetingFLogEntrySource Last();

    IFLogEntrySink? FinalTarget { get; }
}

public abstract class TargetingFLogEntrySource : FLogEntryPublishChainTreeNode, ITargetingFLogEntrySource
{
    private IFLogEntrySink? outBound;
    private IFLogEntrySink? finalTarget;

    public void Insert(IFLogEntryPipelineInterceptor toInsert)
    {
        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                if (OutBound == null)
                {
                    outBound               =  toInsert;
                    PublishedLogEntryEvent += toInsert.InBoundListener;
                    return;
                }
                var oldOutBound = OutBound;
                var oldToInsert = toInsert.Last();

                outBound = toInsert;

                oldToInsert.OutBound = oldOutBound;

                PublishedLogEntryEvent += toInsert.InBoundListener;
            }
            break;
        }
    }

    public IFLogEntrySink? OutBound
    {
        get => outBound;
        set
        {
            if (ReferenceEquals(value, outBound)) return;
            while (true)
            {
                using var myLock = AcquireUpdateTreeLock(100);
                if (myLock != null)
                {
                    if (outBound != null)
                    {
                        PublishedLogEntryEvent -= outBound.InBoundListener;
                        outBound.DecrementRefCount();
                    }
                    outBound = value;
                    if (outBound != null)
                    {
                        PublishedLogEntryEvent += outBound.InBoundListener;
                        outBound.IncrementRefCount();
                    }
                    break;
                }
            }
        }
    }

    public IFLogEntrySink? FinalTarget
    {
        get => finalTarget;
        set
        {
            if (ReferenceEquals(value, finalTarget)) return;
            if (outBound != null)
            {
                RemoveAllToOutBoundTarget();
            }
            finalTarget = value;
            if (finalTarget != null)
            {
                OutBound = finalTarget;
            }
        }
    }

    public ITargetingFLogEntrySource Last() =>
        OutBound is ITargetingFLogEntrySource targetingSource
            ? targetingSource != FinalTarget
                ? targetingSource.Last()
                : this
            : this;

    public abstract string Name { get; }

    public bool Remove(IFLogEntryPipelineInterceptor toRemove)
    {
        if (OutBound != toRemove)
        {
            if (OutBound is ITargetingFLogEntrySource targetingFLogEntrySource)
            {
                return targetingFLogEntrySource.Remove(toRemove);
            }
            return false;
        }
        var checkCain = toRemove as ITargetingFLogEntrySource;
        while (checkCain != null)
        {
            if (checkCain == this) return false;
            checkCain = checkCain.OutBound as ITargetingFLogEntrySource;
        }
        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                if (OutBound != toRemove)
                {
                    if (OutBound is ITargetingFLogEntrySource targetingFLogEntrySource)
                    {
                        return targetingFLogEntrySource.Remove(toRemove);
                    }
                    return false;
                }

                var nextSourceEntry = toRemove.OutBound;
                OutBound = nextSourceEntry;
                break;
            }
        }
        return true;
    }

    public int RemoveAllToOutBoundTarget()
    {
        var count = 0;
        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                var previous = this as ITargetingFLogEntrySource;
                var current  = OutBound as ITargetingFLogEntrySource;
                while (previous != null)
                {
                    var chainNext = current?.OutBound as ITargetingFLogEntrySource;
                    previous.OutBound = null;
                    count++;

                    previous = current;
                    current  = chainNext;
                }
                OutBound = FinalTarget;
                break;
            }
        }
        return count;
    }

    protected event FLogEntryPublishHandler? PublishedLogEntryEvent;

    public virtual void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource? fromSource = null)
    {
        if (ShouldCheckLock)
        {
            while (true)
            {
                using var readLock = AcquireReadTreeLock(50);
                if (readLock != null)
                {
                    SafeOnPublishLogEntryEvent(logEntryEvent);
                    break;
                }
            }
        }
        else
        {
            SafeOnPublishLogEntryEvent(logEntryEvent);
        }
    }

    protected virtual void SafeOnPublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource? fromSource = null)
    {
        PublishedLogEntryEvent?.Invoke(logEntryEvent, fromSource ?? this);
    }
}
