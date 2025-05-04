// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public class Level3PriceQuote : Level2PriceQuote, IMutableLevel3Quote, ICloneable<Level3PriceQuote>, IDoublyLinkedListNode<Level3PriceQuote>
{
    public Level3PriceQuote() { }

    public Level3PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m
      , DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null
      , bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null
      , bool isAskPriceTopChanged = false
      , bool executable = false, IPricePeriodSummary? periodSummary = null, IOrderBook? orderBook = null, 
        IRecentlyTraded? recentlyTraded = null, uint batchId = 0u, uint sourceQuoteRef = 0u
      , DateTime? valueDate = null)
        : base(sourceTickerInfo, sourceTime, isReplay, feedSyncStatus, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, validFrom, validTo, isAskPriceTopChanged, executable,
               periodSummary, orderBook ?? new OrderBook(sourceTickerInfo))
    {
        if (recentlyTraded is RecentlyTraded mutableRecentlyTraded)
            RecentlyTraded = mutableRecentlyTraded;

        else if (recentlyTraded != null)
            RecentlyTraded = new RecentlyTraded(recentlyTraded);

        else if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None) RecentlyTraded = new RecentlyTraded(sourceTickerInfo);

        BatchId              = batchId;
        SourceQuoteReference = sourceQuoteRef;
        ValueDate            = valueDate ?? DateTimeConstants.UnixEpoch;
    }

    public Level3PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel3Quote level3ToClone)
        {
            if (level3ToClone.RecentlyTraded is RecentlyTraded pqRecentlyTraded)
                RecentlyTraded                                            = pqRecentlyTraded.Clone();
            else if (level3ToClone.RecentlyTraded != null) RecentlyTraded = new RecentlyTraded(level3ToClone.RecentlyTraded);

            BatchId              = level3ToClone.BatchId;
            SourceQuoteReference = level3ToClone.SourceQuoteReference;
            ValueDate            = level3ToClone.ValueDate;
        }
    }

    public override Level3PriceQuote Clone() => Recycler?.Borrow<Level3PriceQuote>().CopyFrom(this) as Level3PriceQuote ?? new Level3PriceQuote(this);

    [JsonIgnore]
    public new Level3PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as Level3PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new Level3PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as Level3PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel3Quote? ILevel3Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel3Quote? ILevel3Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel3Quote? IDoublyLinkedListNode<ILevel3Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel3Quote? IDoublyLinkedListNode<ILevel3Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level3Quote;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IMutableRecentlyTraded? RecentlyTraded { get; set; }

    [JsonIgnore] IRecentlyTraded? ILevel3Quote.RecentlyTraded => RecentlyTraded;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel3Quote level3Quote)
        {
            if (level3Quote.RecentlyTraded != null)
            {
                if (RecentlyTraded != null)
                    RecentlyTraded.CopyFrom(level3Quote.RecentlyTraded);
                else
                    RecentlyTraded = new RecentlyTraded(level3Quote.RecentlyTraded);
            }
            else if (RecentlyTraded != null)
            {
                RecentlyTraded = null;
            }

            BatchId              = level3Quote.BatchId;
            SourceQuoteReference = level3Quote.SourceQuoteReference;
            ValueDate            = level3Quote.ValueDate;
        }

        return this;
    }

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel3Quote otherL3)) return false;
        var baseIsSame = base.AreEquivalent(otherL3, exactTypes);

        var lastTradesSame = exactTypes
            ? Equals(RecentlyTraded, otherL3.RecentlyTraded)
            : RecentlyTraded?.AreEquivalent(otherL3.RecentlyTraded) ?? otherL3.RecentlyTraded == null;
        var batchIdSame          = BatchId == otherL3.BatchId;
        var sourceSequenceIdSame = SourceQuoteReference == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;

        return baseIsSame && lastTradesSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (RecentlyTraded?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ BatchId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceQuoteReference;
            hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"Level3PriceQuote{{{nameof(SourceTickerInfo)}: {SourceTickerInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SingleTickValue)}: " +
        $"{SingleTickValue:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
        $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: " +
        $"{BidPriceTop:N5}, {nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, {nameof(SourceAskTime)}: " +
        $"{SourceAskTime:O}, {nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopChanged)}: " +
        $"{IsAskPriceTopChanged}, {nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: " +
        $"{SummaryPeriod}, {nameof(OrderBook)}: {OrderBook}, {nameof(RecentlyTraded)}: {RecentlyTraded}, " +
        $"{nameof(BatchId)}: {BatchId}, {nameof(SourceQuoteReference)}: {SourceQuoteReference}, {nameof(ValueDate)}: {ValueDate:u} }}";
}
