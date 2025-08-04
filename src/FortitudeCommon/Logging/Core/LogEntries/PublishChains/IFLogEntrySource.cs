// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntrySource : ITargetingFLogEntrySource
{
    bool AddOptionalChild(IFLogEntrySink toAdd);

    bool RemoveOptionalChild(IFLogEntrySink toRemove);

    bool ReplaceOptionalChild(IFLogEntrySink oldChild, IFLogEntrySink newChild);

    void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, Predicate<IFLogEntrySink> selectChildrenPublish, ITargetingFLogEntrySource? fromSource = null);

    IReadOnlyList<IFLogEntrySink> ChildLogEntryReceivers { get; }
}

public abstract class FLogEntrySource : TargetingFLogEntrySource, IFLogEntrySource
{
    // created on demand to keep overhead of publishers low (~two per logger and appenders)
    private ReusableList<IFLogEntrySink>?  currentChildReceivers;

    protected event FLogEntryPublishHandler? AllChildrenPublishedLogEntryEvent;

    public bool AddOptionalChild(IFLogEntrySink toAdd)
    {
        if (currentChildReceivers?.Contains(toAdd) ?? false)
        {
            return false;
        }
        currentChildReceivers ??= Recycler.Borrow<ReusableList<IFLogEntrySink>>();

        toAdd.InBound         =   this;

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
        if (!(currentChildReceivers?.Contains(toRemove) ?? false))
        {
            return false;
        }
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
                FLogContext.Context.AsyncRegistry.ScheduleRecycleDecrement(currentChildReceivers);
                currentChildReceivers = replaceAllButChildList;
                break;
            }
        }
        return true;
    }

    public IReadOnlyList<IFLogEntrySink> ChildLogEntryReceivers => 
        currentChildReceivers ??= Recycler.Borrow<ReusableList<IFLogEntrySink>>();

    public override void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource? fromSource = null)
    {
        if (ShouldCheckLock)
        {
            while (true)
            {
                using var readLock = AcquireReadTreeLock(50);
                if (readLock != null)
                {
                    SafeOnPublishLogEntryEvent(logEntryEvent, fromSource);
                    AllChildrenPublishedLogEntryEvent?.Invoke(logEntryEvent, fromSource ?? this);
                    break;
                }
            }
        }
        else
        {
            SafeOnPublishLogEntryEvent(logEntryEvent, fromSource);
            AllChildrenPublishedLogEntryEvent?.Invoke(logEntryEvent, fromSource ?? this);
        }
    }

    public virtual void PublishLogEntryEvent(LogEntryPublishEvent logEntryEvent
      , Predicate<IFLogEntrySink> selectChildrenPublish
      , ITargetingFLogEntrySource? fromSource = null)
    {
        if (FinalTarget != null && selectChildrenPublish(FinalTarget))
        {
            SafeOnPublishLogEntryEvent(logEntryEvent, fromSource);
        }
        if (currentChildReceivers == null) return;
        for (int i = 0; i < currentChildReceivers.Count; i++)
        {
            var checkChild = currentChildReceivers[i];
            if (selectChildrenPublish(checkChild))
            {
                checkChild.InBoundListener(logEntryEvent, fromSource ?? this);
            }
        }
    }

    public bool ReplaceOptionalChild(IFLogEntrySink oldChild, IFLogEntrySink newChild)
    {
        if (!(currentChildReceivers?.Contains(oldChild) ?? false))
        {
            return AddOptionalChild(newChild);
        }
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
