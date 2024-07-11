// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.Summaries;

public interface IPricePeriodSummary : IInterfacesComparable<IPricePeriodSummary>, ITimeSeriesPeriodRange
  , ITimeSeriesEntry<IPricePeriodSummary>, IDoublyLinkedListNode<IPricePeriodSummary>, IReusableObject<IPricePeriodSummary>
{
    PricePeriodSummaryFlags PeriodSummaryFlags { get; }
    DateTime                PeriodEndTime      { get; }

    bool       IsEmpty       { get; }
    BidAskPair StartBidAsk   { get; }
    BidAskPair HighestBidAsk { get; }
    BidAskPair LowestBidAsk  { get; }
    BidAskPair EndBidAsk     { get; }
    uint       TickCount     { get; }
    long       PeriodVolume  { get; }
    BidAskPair AverageBidAsk { get; }

    double ContributingCompletePercentage(BoundedTimeRange timeRange, IRecycler recycler);
    bool   IsWhollyBoundedBy(ITimeSeriesPeriodRange parentRange);
}

public static class PricePeriodSummaryExtensions
{
    public static int AddReplace
        (this IDoublyLinkedList<IPricePeriodSummary> existing, IDoublyLinkedList<IPricePeriodSummary> toAdd, IRecycler? recycler = null)
    {
        var removedCount = 0;
        var currentAdd   = toAdd.Head;

        while (currentAdd != null)
        {
            removedCount += AddReplace(existing, currentAdd, recycler);

            currentAdd = currentAdd.Next;
        }
        return removedCount;
    }

    public static int AddReplace
        (this IDoublyLinkedList<IPricePeriodSummary> existing, IPricePeriodSummary toAddReplace, IRecycler? recycler = null)
    {
        var currentExisting = existing.Head;
        if (currentExisting == null)
        {
            existing.AddFirst(toAddReplace);
            return 0;
        }
        var removedCount = 0;
        while (currentExisting != null)
        {
            if (ReferenceEquals(currentExisting, toAddReplace)) break;
            if (toAddReplace.IsWhollyBoundedBy(currentExisting)) break;
            if (currentExisting.PeriodStartTime > toAddReplace.PeriodStartTime)
            {
                var insertAfter = currentExisting.Previous;
                while (currentExisting != null && currentExisting.IsWhollyBoundedBy(toAddReplace))
                {
                    if (ReferenceEquals(currentExisting, toAddReplace)) return removedCount;
                    var removed = existing.Remove(currentExisting);
                    recycler?.Recycle(removed);
                    removedCount++;
                    currentExisting = currentExisting.Next;
                }
                if (ReferenceEquals(insertAfter, toAddReplace)) break;
                if (insertAfter != null)
                {
                    insertAfter.Next      = toAddReplace;
                    toAddReplace.Previous = insertAfter;
                }
                else
                {
                    existing.AddFirst(toAddReplace);
                }
                if (currentExisting != null)
                {
                    currentExisting.Previous = toAddReplace;
                    toAddReplace.Next        = currentExisting;
                }
                else
                {
                    toAddReplace.Next = null;
                }
                return removedCount;
            }
            if (currentExisting?.Next == null)
            {
                existing.AddLast(toAddReplace);
                return removedCount;
            }

            currentExisting = currentExisting.Next;
        }
        return removedCount;
    }
}

public interface IMutablePricePeriodSummary : IPricePeriodSummary
{
    new TimeSeriesPeriod TimeSeriesPeriod { get; set; }

    new PricePeriodSummaryFlags PeriodSummaryFlags { get; set; }

    new bool     IsEmpty         { get; set; }
    new DateTime PeriodStartTime { get; set; }
    new DateTime PeriodEndTime   { get; set; }
    decimal      StartBidPrice   { get; set; }
    decimal      StartAskPrice   { get; set; }
    decimal      HighestBidPrice { get; set; }
    decimal      HighestAskPrice { get; set; }
    decimal      LowestBidPrice  { get; set; }
    decimal      LowestAskPrice  { get; set; }
    decimal      EndBidPrice     { get; set; }
    decimal      EndAskPrice     { get; set; }
    new uint     TickCount       { get; set; }
    new long     PeriodVolume    { get; set; }
    decimal      AverageBidPrice { get; set; }
    decimal      AverageAskPrice { get; set; }

    new IMutablePricePeriodSummary Clone();
}

public static class MutablePricePeriodSummaryExtensions
{
    public static IMutablePricePeriodSummary OpeningState(this IMutablePricePeriodSummary mergeInto, BidAskPair? previousEnd)
    {
        if (previousEnd == null) return mergeInto;
        var open = previousEnd.Value;
        if (Equals(mergeInto.StartBidAsk, default(BidAskPair)) && Equals(mergeInto.EndBidAsk, default(BidAskPair)))
        {
            mergeInto.StartBidPrice  = open.BidPrice;
            mergeInto.StartAskPrice  = open.AskPrice;
            mergeInto.LowestBidPrice = open.BidPrice;
            mergeInto.LowestAskPrice = open.AskPrice;
        }
        return mergeInto;
    }

    public static IMutablePricePeriodSummary UnchangedClosingState(this IMutablePricePeriodSummary mergeInto, BidAskPair? previousEnd)
    {
        if (previousEnd == null) return mergeInto;
        var open = previousEnd.Value;
        if (Equals(mergeInto.StartBidAsk, previousEnd) && Equals(mergeInto.EndBidAsk, default(BidAskPair)))
        {
            mergeInto.HighestBidPrice = open.BidPrice;
            mergeInto.HighestAskPrice = open.AskPrice;
            mergeInto.EndBidPrice     = open.BidPrice;
            mergeInto.EndAskPrice     = open.AskPrice;
        }
        mergeInto.TickCount    = 0;
        mergeInto.PeriodVolume = 0;
        return mergeInto;
    }

    public static IMutablePricePeriodSummary MergeBoundaries(this IMutablePricePeriodSummary mergeInto, IPricePeriodSummary other)
    {
        if (!other.IsWhollyBoundedBy(mergeInto)) return mergeInto;

        if (Equals(mergeInto.StartBidAsk, default(BidAskPair)) && Equals(mergeInto.EndBidAsk, default(BidAskPair)))
        {
            mergeInto.StartBidPrice  = other.StartBidAsk.BidPrice;
            mergeInto.StartAskPrice  = other.StartBidAsk.AskPrice;
            mergeInto.LowestBidPrice = other.LowestBidAsk.BidPrice;
            mergeInto.LowestAskPrice = other.LowestBidAsk.AskPrice;
        }

        var highestBid = Math.Max(mergeInto.HighestBidPrice, other.HighestBidAsk.BidPrice);
        var highestAsk = Math.Max(mergeInto.HighestAskPrice, other.HighestBidAsk.AskPrice);

        var lowestBid = Math.Min(mergeInto.LowestBidPrice, other.LowestBidAsk.BidPrice);
        var lowestAsk = Math.Min(mergeInto.LowestAskPrice, other.LowestBidAsk.AskPrice);

        mergeInto.HighestBidPrice =  highestBid;
        mergeInto.HighestAskPrice =  highestAsk;
        mergeInto.LowestBidPrice  =  lowestBid;
        mergeInto.LowestAskPrice  =  lowestAsk;
        mergeInto.EndBidPrice     =  other.EndBidAsk.BidPrice;
        mergeInto.EndAskPrice     =  other.EndBidAsk.AskPrice;
        mergeInto.TickCount       += other.TickCount;
        mergeInto.PeriodVolume    += other.PeriodVolume;

        return mergeInto;
    }
}
