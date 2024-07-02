// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IBidAskPeriod : IDoublyLinkedListNode<IBidAskPeriod>
{
    TimeSeriesPeriod Period               { get; }
    DateTime         StartTime            { get; }
    DateTime         EndTime              { get; }
    BidAskPair       BidAskPair           { get; }
    bool             SweepCanBeAggregated { get; set; }
    bool             SweepCanBeTrimmed    { get; set; }

    DateTime       ContainingPeriodStart(TimeSeriesPeriod forPeriod);
    DateTime       CappedEndTime(DateTime maxDateTime);
    decimal        CappedMillis(DateTime maxDateTime);
    TimeSpan       CappedTimeSpan(DateTime maxDateTime);
    IBidAskPeriod? SwapOut(IDoublyLinkedList<IBidAskPeriod> replacements);

    IDoublyLinkedList<IBidAskPeriod> ReplaceRange(IBidAskPeriod endNode, IDoublyLinkedList<IBidAskPeriod> replacements);

    BidAskPair     TimeWeightedMsBidAskPair(DateTime maxDateTime);
    IBidAskPeriod? ContainingPeriodEndNode(TimeSeriesPeriod forPeriod);
    IBidAskPeriod  LastContiguousPeriodEndNode();
}

[Flags]
public enum PeriodListFlags : byte
{
    None              = 0
  , SweepAggregateSet = 1
  , SweepTrim         = 2
  , TicksIncomplete   = 4
}

public class BidAskPeriod : ReusableObject<IBidAskPeriod>, IBidAskPeriod
{
    private PeriodListFlags flags;

    public BidAskPeriod() { }

    public BidAskPeriod(BidAskPair bidAskPair, DateTime startTime, TimeSeriesPeriod period)
    {
        BidAskPair = bidAskPair;
        StartTime  = startTime;
        Period     = period;
    }

    public BidAskPeriod(IBidAskPeriod toClone)
    {
        BidAskPair = toClone.BidAskPair;
        StartTime  = toClone.StartTime;
        Period     = toClone.Period;
    }

    public BidAskPeriod(ILevel1Quote toCapture)
    {
        BidAskPair = new BidAskPair(toCapture.BidPriceTop, toCapture.AskPriceTop);
        StartTime  = toCapture.SourceTime;
        Period     = TimeSeriesPeriod.Tick;
    }

    public bool TicksIncomplete
    {
        get => (flags & PeriodListFlags.TicksIncomplete) > 0;
        set
        {
            if (value)
                flags |= PeriodListFlags.TicksIncomplete;
            else
                flags &= ~PeriodListFlags.TicksIncomplete;
        }
    }

    public IBidAskPeriod?   Previous   { get; set; }
    public IBidAskPeriod?   Next       { get; set; }
    public TimeSeriesPeriod Period     { get; private set; }
    public DateTime         StartTime  { get; private set; }
    public DateTime         EndTime    => Period.PeriodEnd(StartTime);
    public BidAskPair       BidAskPair { get; private set; }

    public DateTime ContainingPeriodStart(TimeSeriesPeriod forPeriod) => forPeriod.ContainingPeriodBoundaryStart(StartTime);

    public DateTime CappedEndTime
        (DateTime maxDateTime) =>
        Next != null ? DateTime.FromBinary(Math.Min(Next.StartTime.Ticks, maxDateTime.Ticks)) : maxDateTime;

    public decimal CappedMillis(DateTime maxDateTime) => (decimal)CappedTimeSpan(maxDateTime).TotalMilliseconds;

    public TimeSpan CappedTimeSpan
        (DateTime maxDateTime) =>
        Next != null ? DateTime.FromBinary(Math.Min(Next.StartTime.Ticks, maxDateTime.Ticks)) - StartTime : maxDateTime - StartTime;

    public BidAskPair TimeWeightedMsBidAskPair(DateTime maxDateTime)
    {
        var periodMs = CappedMillis(maxDateTime);
        return new BidAskPair(BidAskPair.BidPrice * periodMs, BidAskPair.AskPrice * periodMs);
    }

    public IBidAskPeriod? ContainingPeriodEndNode(TimeSeriesPeriod forPeriod)
    {
        var periodStart = forPeriod.ContainingPeriodBoundaryStart(StartTime);
        var periodEnd   = forPeriod.PeriodEnd(periodStart);

        IBidAskPeriod currentNode = this;
        var           nextNode    = Next;

        while (nextNode != null)
        {
            var nextPeriodStart = nextNode.ContainingPeriodStart(forPeriod);
            var nextPeriodEnd   = forPeriod.PeriodEnd(nextPeriodStart);
            if (nextPeriodEnd != periodEnd) break;

            currentNode = nextNode;
            nextNode    = nextNode.Next;
        }
        return nextNode != null ? currentNode : null;
    }

    public bool SweepCanBeAggregated
    {
        get => (flags & PeriodListFlags.SweepAggregateSet) > 0;
        set
        {
            if (value)
                flags |= PeriodListFlags.SweepAggregateSet;
            else
                flags &= ~PeriodListFlags.SweepAggregateSet;
        }
    }

    public bool SweepCanBeTrimmed
    {
        get => (flags & PeriodListFlags.SweepTrim) > 0;
        set
        {
            if (value)
                flags |= PeriodListFlags.SweepTrim;
            else
                flags &= ~PeriodListFlags.SweepTrim;
        }
    }

    public IBidAskPeriod? SwapOut(IDoublyLinkedList<IBidAskPeriod> replacements)
    {
        if (replacements.IsEmpty) return null;
        if (Previous != null)
        {
            Previous.Next               = replacements.Head;
            replacements.Head!.Previous = Previous;
        }
        if (Next != null)
        {
            Next.Previous           = replacements.Tail;
            replacements.Tail!.Next = Next;
        }
        if (Previous == null) replacements.Head!.Previous = null;
        if (Next == null) replacements.Tail!.Next         = null;
        replacements.DetachNodes();
        Next     = null;
        Previous = null;
        return this;
    }

    public IBidAskPeriod LastContiguousPeriodEndNode()
    {
        IBidAskPeriod? currentNode = this;
        var            nextNode    = Next;
        while (nextNode != null && nextNode.Period == Period)
        {
            currentNode = nextNode;
            nextNode    = nextNode.Next;
        }
        return currentNode;
    }

    public IDoublyLinkedList<IBidAskPeriod> ReplaceRange(IBidAskPeriod endNode, IDoublyLinkedList<IBidAskPeriod> replacements)
    {
        if (replacements.IsEmpty) return new DoublyLinkedList<IBidAskPeriod>();
        if (Previous != null)
        {
            Previous.Next = replacements.Head;

            replacements.Head!.Previous = Previous;
        }
        if (endNode.Next != null)
        {
            endNode.Next.Previous   = replacements.Tail;
            replacements.Tail!.Next = endNode.Next;
        }
        if (Previous == null) replacements.Head!.Previous = null;
        if (Next == null) replacements.Tail!.Next         = null;
        replacements.DetachNodes();
        Previous     = null;
        endNode.Next = null;
        IBidAskPeriod? currentNode = this;

        while (currentNode != endNode && currentNode != null)
        {
            replacements.AddLast(currentNode);
            currentNode = currentNode.Next;
        }
        if (currentNode != null) replacements.AddLast(currentNode);
        return replacements;
    }


    public void Configure(BidAskPair bidAskPair, DateTime startTime, TimeSeriesPeriod period)
    {
        BidAskPair = bidAskPair;
        StartTime  = startTime;
        Period     = period;
        flags      = PeriodListFlags.None;
    }

    public override IBidAskPeriod CopyFrom(IBidAskPeriod source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidAskPair = source.BidAskPair;
        StartTime  = source.StartTime;
        Period     = source.Period;
        return this;
    }

    public IBidAskPeriod CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BidAskPair = new BidAskPair(source.BidPriceTop, source.AskPriceTop);
        StartTime  = source.SourceTime;
        Period     = TimeSeriesPeriod.Tick;
        return this;
    }

    public override IBidAskPeriod Clone() => Recycler?.Borrow<BidAskPeriod>().CopyFrom(this) ?? new BidAskPeriod(this);
}

public static class BidAskPeriodExtensions
{
    public static IBidAskPeriod ToBidAskPeriod(this ILevel1Quote level1Quote, IRecycler? recycler = null)
    {
        var bidAskPeriod = recycler?.Borrow<BidAskPeriod>().CopyFrom(level1Quote) ?? new BidAskPeriod(level1Quote);
        return bidAskPeriod;
    }

    public static IBidAskPeriod AverageToBidAskPeriod(this IPricePeriodSummary pricePeriodSummary, IRecycler? recycler = null)
    {
        var bidAskPeriod = recycler?.Borrow<BidAskPeriod>() ?? new BidAskPeriod();
        bidAskPeriod.Configure(pricePeriodSummary.AverageBidAsk, pricePeriodSummary.PeriodStartTime, pricePeriodSummary.TimeSeriesPeriod);
        return bidAskPeriod;
    }
}
