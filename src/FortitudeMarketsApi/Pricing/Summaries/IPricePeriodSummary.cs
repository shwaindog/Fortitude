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
        var removedCount    = 0;
        var currentExisting = existing.Head;
        while (currentExisting != null)
        {
            if (currentExisting.PeriodStartTime > toAddReplace.PeriodStartTime)
            {
                var insertAfter = currentExisting.Previous;
                while (currentExisting != null && currentExisting.IsWhollyBoundedBy(toAddReplace))
                {
                    var removed = existing.Remove(currentExisting);
                    recycler?.Recycle(removed);
                    removedCount++;
                    currentExisting = currentExisting.Next;
                }
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
                break;
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
    public static IMutablePricePeriodSummary Merge(this IMutablePricePeriodSummary mergeInto, IPricePeriodSummary other)
    {
        if (!other.IsWhollyBoundedBy(mergeInto)) return mergeInto;

        var existingTimeSpanMs = (decimal)(other.PeriodStartTime - mergeInto.PeriodStartTime).TotalMilliseconds;
        var addTimeSpanMs      = (decimal)(other.PeriodEnd() - other.PeriodStartTime).TotalMilliseconds;

        var timeWeightedExistingBidAverage = mergeInto.AverageBidPrice * existingTimeSpanMs;
        var timeWeightedExistingAskAverage = mergeInto.AverageAskPrice * existingTimeSpanMs;

        var timeWeightedMergedBidAverage = other.AverageBidAsk.BidPrice * addTimeSpanMs;
        var timeWeightedMergedAskAverage = other.AverageBidAsk.AskPrice * addTimeSpanMs;

        var mergedBidAverage = (timeWeightedExistingBidAverage + timeWeightedMergedBidAverage) / (existingTimeSpanMs + addTimeSpanMs);
        var mergedAskAverage = (timeWeightedExistingAskAverage + timeWeightedMergedAskAverage) / (existingTimeSpanMs + addTimeSpanMs);

        mergeInto.AverageBidPrice = mergedBidAverage;
        mergeInto.AverageAskPrice = mergedAskAverage;

        var highestBid = Math.Max(mergeInto.HighestBidPrice, other.HighestBidAsk.BidPrice);
        var highestAsk = Math.Max(mergeInto.HighestAskPrice, other.HighestBidAsk.AskPrice);

        var lowestBid = Math.Min(mergeInto.LowestBidPrice, other.LowestBidAsk.BidPrice);
        var lowestAsk = Math.Min(mergeInto.LowestAskPrice, other.LowestBidAsk.AskPrice);

        mergeInto.HighestBidPrice = highestBid;
        mergeInto.HighestAskPrice = highestAsk;
        mergeInto.LowestBidPrice  = lowestBid;
        mergeInto.LowestAskPrice  = lowestAsk;
        mergeInto.EndBidPrice     = other.EndBidAsk.BidPrice;
        mergeInto.EndAskPrice     = other.EndBidAsk.AskPrice;

        return mergeInto;
    }
}
