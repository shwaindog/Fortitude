// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntrySource : ITargetingFLogEntrySource
{
    bool AddOptionalChild(IFLogEntrySink toAdd);

    bool RemoveOptionalChild(IFLogEntrySink toRemove);

    bool ReplaceOptionalChild(IFLogEntrySink oldChild, IFLogEntrySink newChild);

    void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, Predicate<IFLogEntrySink> selectChildrenPublish);

    IReadOnlyList<IFLogEntrySink> ChildLogEntryReceivers { get; }
}

public abstract class FLogEntrySource : TargetingFLogEntrySource, IFLogEntrySource
{
    protected FLogEntrySource()
    {
        currentChildReceivers = Recycler.Borrow<ReusableList<IFLogEntrySink>>();
    }

    private ReusableList<IFLogEntrySink>  currentChildReceivers;
    private ReusableList<IFLogEntrySink>? oldChildReceivers;

    protected event FLogEntryPublishHandler? AllChildrenPublishedLogEntryEvent;

    public bool AddOptionalChild(IFLogEntrySink toAdd)
    {
        if (currentChildReceivers.Contains(toAdd))
        {
            return false;
        }
        toAdd.InBound = this;

        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                currentChildReceivers.Add(toAdd);
                AllChildrenPublishedLogEntryEvent += toAdd.InBoundListener;
                break;
            }
        }
        return true;
    }

    public bool RemoveOptionalChild(IFLogEntrySink toRemove)
    {
        if (toRemove.InBound == this)
        {
            toRemove.InBound = null;
        }
        if (!currentChildReceivers.Contains(toRemove))
        {
            return false;
        }
        oldChildReceivers?.DecrementRefCount();
        var replaceAllButChildList = Recycler.Borrow<ReusableList<IFLogEntrySink>>();
        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                var childLock = toRemove.AcquireUpdateTreeLock(100);
                if (childLock == null) continue;

                for (int i = 0; i < currentChildReceivers.Count; i++)
                {
                    var checkChild = currentChildReceivers[i];
                    if (checkChild != toRemove)
                    {
                        replaceAllButChildList.Add(checkChild);
                    }
                    else
                    {
                        AllChildrenPublishedLogEntryEvent -= toRemove.InBoundListener;
                    }
                }
                oldChildReceivers     = currentChildReceivers;
                currentChildReceivers = replaceAllButChildList;
                break;
            }
        }
        return true;
    }

    public IReadOnlyList<IFLogEntrySink> ChildLogEntryReceivers => currentChildReceivers;

    public override void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        if (StopProcessing)
        {
            while (true)
            {
                using var readLock = AcquireReadTreeLock(50);
                if (readLock != null)
                {
                    SafeOnPublishLogEntryEvent(logEntryEvent);
                    AllChildrenPublishedLogEntryEvent?.Invoke(logEntryEvent, this);
                    break;
                }
            }
        }
        else
        {
            SafeOnPublishLogEntryEvent(logEntryEvent);
            AllChildrenPublishedLogEntryEvent?.Invoke(logEntryEvent, this);
        }
    }

    public void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, Predicate<IFLogEntrySink> selectChildrenPublish)
    {
        for (int i = 0; i < currentChildReceivers.Count; i++)
        {
            var checkChild = currentChildReceivers[i];
            if (selectChildrenPublish(checkChild))
            {
                checkChild.InBoundListener(logEntryEvent, this);
            }
        }
    }

    public bool ReplaceOptionalChild(IFLogEntrySink oldChild, IFLogEntrySink newChild)
    {
        if (!currentChildReceivers.Contains(oldChild))
        {
            return AddOptionalChild(newChild);
        }
        oldChildReceivers?.DecrementRefCount();
        while (true)
        {
            using var myLock = AcquireUpdateTreeLock(100);
            if (myLock != null)
            {
                var oldChildLock = oldChild.AcquireUpdateTreeLock(100);
                if (oldChildLock == null) continue;
                var newChildLock = newChild.AcquireUpdateTreeLock(100);
                if (newChildLock == null) continue;

                for (int i = 0; i < currentChildReceivers.Count; i++)
                {
                    var checkChild = currentChildReceivers[i];
                    if (checkChild == oldChild)
                    {
                        AllChildrenPublishedLogEntryEvent     -= oldChild.InBoundListener;
                        currentChildReceivers[i] =  newChild;
                        AllChildrenPublishedLogEntryEvent     += newChild.InBoundListener;
                    }
                }
                break;
            }
        }
        return true;
    }
}
