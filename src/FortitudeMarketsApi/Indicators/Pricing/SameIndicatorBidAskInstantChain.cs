// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public interface ISameIndicatorBidAskInstantChain : IDoublyLinkedList<IBidAskInstant>, IRecyclableObject
{
    long       IndicatorSourceTickerId { get; }
    TimePeriod CoveringPeriod          { get; }

    IBidAskInstant RefIncAddFirst(IBidAskInstant node);
    IBidAskInstant RefIncAddLast(IBidAskInstant node);
    IBidAskInstant RefDecRemove(IBidAskInstant node);
}

public class SameIndicatorBidAskInstantChain : DoublyLinkedList<IBidAskInstant>, ISameIndicatorBidAskInstantChain
{
    private const int NumRecentRecycleTimes = 5;

    private int isInRecycler;

    protected DateTime[] LastRecycleTimes = new DateTime[NumRecentRecycleTimes];

    protected int RecycleCount;
    private   int refCount = 1;

    public long IndicatorSourceTickerId { get; set; }

    public TimePeriod CoveringPeriod { get; set; }

    public IBidAskInstant RefIncAddFirst(IBidAskInstant node)
    {
        node.IncrementRefCount();
        return AddFirst(node);
    }

    public IBidAskInstant RefIncAddLast(IBidAskInstant node)
    {
        node.IncrementRefCount();
        return AddLast(node);
    }

    public IBidAskInstant RefDecRemove(IBidAskInstant node)
    {
        Remove(node);
        node.DecrementRefCount();
        return node;
    }

    public IRecycler? Recycler { get; set; }

    public int RefCount => refCount;

    public bool AutoRecycleAtRefCountZero { get; set; }
    public bool IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
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
        IndicatorSourceTickerId = 0;

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.None);

        refCount = 0;
    }


    public void Configure(long indicatorSourceTickerId, TimePeriod coveringPeriod)
    {
        IndicatorSourceTickerId = indicatorSourceTickerId;

        CoveringPeriod = coveringPeriod;
    }
}
