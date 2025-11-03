// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeIO.Storage.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Candles;

public interface ICandle : IReusableObject<ICandle>, IInterfacesComparable<ICandle>, ITimeBoundaryPeriodRange
  , ITimeSeriesEntry, IDoublyLinkedListNode<ICandle>, IStringBearer
{
    CandleFlags CandleFlags { get; }

    DateTime   PeriodEndTime { get; }
    bool       IsEmpty       { get; }
    BidAskPair StartBidAsk   { get; }
    BidAskPair HighestBidAsk { get; }
    BidAskPair LowestBidAsk  { get; }
    BidAskPair EndBidAsk     { get; }
    uint       TickCount     { get; }
    long       PeriodVolume  { get; }
    BidAskPair AverageBidAsk { get; }

    double ContributingCompletePercentage(BoundedTimeRange timeRange, IRecycler recycler);
    bool   IsWhollyBoundedBy(ITimeBoundaryPeriodRange parentRange);

    new ICandle Clone();
}

public interface IMutableCandle : ICandle, ITrackableReset<IMutableCandle> 
{
    new TimeBoundaryPeriod TimeBoundaryPeriod { get; set; }

    new CandleFlags CandleFlags { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get;             set; }
    new DateTime PeriodStartTime { get; set; }

    [JsonIgnore] new DateTime PeriodEndTime { get; set; }

    [JsonIgnore] decimal StartBidPrice   { get; set; }
    [JsonIgnore] decimal StartAskPrice   { get; set; }
    [JsonIgnore] decimal HighestBidPrice { get; set; }
    [JsonIgnore] decimal HighestAskPrice { get; set; }
    [JsonIgnore] decimal LowestBidPrice  { get; set; }
    [JsonIgnore] decimal LowestAskPrice  { get; set; }
    [JsonIgnore] decimal EndBidPrice     { get; set; }
    [JsonIgnore] decimal EndAskPrice     { get; set; }
    [JsonIgnore] decimal AverageBidPrice { get; set; }
    [JsonIgnore] decimal AverageAskPrice { get; set; }

    new uint TickCount    { get; set; }
    new long PeriodVolume { get; set; }

    new IMutableCandle Clone();
}

public static class MutableCandleExtensions
{
    public static IMutableCandle OpeningState(this IMutableCandle mergeInto, BidAskPair? previousEnd)
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

    public static IMutableCandle UnchangedClosingState(this IMutableCandle mergeInto, BidAskPair? previousEnd)
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

    public static IMutableCandle MergeBoundaries(this IMutableCandle mergeInto, ICandle other)
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

    public static int AddReplace
        (this IDoublyLinkedList<ICandle> existing, IDoublyLinkedList<ICandle> toAdd)
    {
        var removedCount = 0;
        var currentAdd   = toAdd.Head;

        while (currentAdd != null)
        {
            removedCount += AddReplace(existing, currentAdd);

            currentAdd = currentAdd.Next;
        }
        return removedCount;
    }

    public static int AddReplace
        (this IDoublyLinkedList<ICandle> existing, ICandle toAddReplace)
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
                    removed.DecrementRefCount();
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
