// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public interface IVaryingIndicatorBidAskInstantChain : IDoublyLinkedList<IIndicatorValidRangeBidAskPeriod>, IRecyclableObject
{
    IIndicatorValidRangeBidAskPeriod RefIncAddFirst(IIndicatorValidRangeBidAskPeriod node);
    IIndicatorValidRangeBidAskPeriod RefIncAddLast(IIndicatorValidRangeBidAskPeriod node);
    IIndicatorValidRangeBidAskPeriod RefDecRemove(IIndicatorValidRangeBidAskPeriod node);
}

public class VaryingIndicatorBidAskInstantChain : DoublyLinkedList<IIndicatorValidRangeBidAskPeriod>, IVaryingIndicatorBidAskInstantChain
{
    private const int NumRecentRecycleTimes = 5;

    private int isInRecycler;

    protected DateTime[] LastRecycleTimes = new DateTime[NumRecentRecycleTimes];

    protected int RecycleCount;
    private   int refCount = 1;

    public IRecycler? Recycler { get; set; }

    public int RefCount => refCount;

    public bool AutoRecycleAtRefCountZero { get; set; }
    public bool IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
    }

    public IIndicatorValidRangeBidAskPeriod RefIncAddFirst(IIndicatorValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddFirst(node);
    }

    public IIndicatorValidRangeBidAskPeriod RefIncAddLast(IIndicatorValidRangeBidAskPeriod node)
    {
        node.IncrementRefCount();
        return AddLast(node);
    }

    public IIndicatorValidRangeBidAskPeriod RefDecRemove(IIndicatorValidRangeBidAskPeriod node)
    {
        Remove(node);
        node.DecrementRefCount();
        return node;
    }

    public virtual int DecrementRefCount()
    {
        var shouldRecycle = !IsInRecycler && Interlocked.Decrement(ref refCount) <= 0 && AutoRecycleAtRefCountZero;
        var currentNode   = Head;
        while (currentNode != null)
        {
            if (shouldRecycle) Remove(currentNode);
            currentNode.DecrementRefCount();
            currentNode = currentNode.Next;
        }
        if (shouldRecycle) Recycle();
        return refCount;
    }

    public virtual int IncrementRefCount()
    {
        var currentNode = Head;
        while (currentNode != null)
        {
            currentNode.IncrementRefCount();
            currentNode = currentNode.Next;
        }
        return Interlocked.Increment(ref refCount);
    }

    public virtual bool Recycle()
    {
        if (IsInRecycler) return true;
        if (Recycler == null) return false;
        if (Interlocked.CompareExchange(ref isInRecycler, 1, 0) != 0) return false;

        LastRecycleTimes[RecycleCount % NumRecentRecycleTimes] = DateTime.UtcNow;
        Interlocked.Increment(ref RecycleCount);
        Recycler.Recycle(this);

        return true;
    }

    public virtual void StateReset()
    {
        refCount = 0;
    }
}
