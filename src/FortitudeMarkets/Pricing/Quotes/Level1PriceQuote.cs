// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public class Level1PriceQuote : TickInstant, IMutableLevel1Quote, ICloneable<Level1PriceQuote>, IDoublyLinkedListNode<Level1PriceQuote>
{
    public Level1PriceQuote() { }

    public Level1PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null,
        bool isReplay = false, FeedSyncStatus syncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null,
        DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null, DateTime? validFrom = null,
        DateTime? validTo = null, decimal bidPriceTop = 0m, bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null,
        decimal askPriceTop = 0m, bool isAskPriceTopChanged = false, bool executable = false, IPricePeriodSummary? periodSummary = null)
        : base(sourceTickerInfo, sourceTime, isReplay, syncStatus, singlePrice, clientReceivedTime)
    {
        AdapterReceivedTime  = adapterReceivedTime ?? DateTimeConstants.UnixEpoch;
        AdapterSentTime      = adapterSentTime ?? DateTimeConstants.UnixEpoch;
        ValidFrom            = validFrom ?? DateTimeConstants.UnixEpoch;
        ValidTo              = validTo ?? DateTimeConstants.UnixEpoch;
        SourceBidTime        = sourceBidTime ?? DateTimeConstants.UnixEpoch;
        IsBidPriceTopUpdated = isBidPriceTopChanged;
        SourceAskTime        = sourceAskTime ?? DateTimeConstants.UnixEpoch;
        IsAskPriceTopUpdated = isAskPriceTopChanged;
        Executable           = executable;
        if (periodSummary is not null) SummaryPeriod = new PricePeriodSummary(periodSummary);

        if (this is not ILevel2Quote)
        {
            BidPriceTop = bidPriceTop;
            AskPriceTop = askPriceTop;
        }
    }

    public Level1PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote lvl1Quote)
        {
            AdapterReceivedTime  = lvl1Quote.AdapterReceivedTime;
            AdapterSentTime      = lvl1Quote.AdapterSentTime;
            ValidFrom            = lvl1Quote.ValidFrom;
            ValidTo              = lvl1Quote.ValidTo;
            SourceBidTime        = lvl1Quote.SourceBidTime;
            IsBidPriceTopUpdated = lvl1Quote.IsBidPriceTopUpdated;
            SourceAskTime        = lvl1Quote.SourceAskTime;
            IsAskPriceTopUpdated = lvl1Quote.IsAskPriceTopUpdated;
            Executable           = lvl1Quote.Executable;
            if (lvl1Quote.SummaryPeriod is { IsEmpty: false }) SummaryPeriod = new PricePeriodSummary(lvl1Quote.SummaryPeriod);

            if (this is not ILevel2Quote)
            {
                BidPriceTop = lvl1Quote.BidPriceTop;
                AskPriceTop = lvl1Quote.AskPriceTop;
            }
        }
    }

    public override Level1PriceQuote Clone() => Recycler?.Borrow<Level1PriceQuote>().CopyFrom(this) as Level1PriceQuote ?? new Level1PriceQuote(this);

    public new Level1PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as Level1PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }
    public new Level1PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as Level1PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    ILevel1Quote? ILevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    ILevel1Quote? ILevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }
    IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next     { get; set; }

    decimal IBidAskPair.BidPrice => BidPriceTop;
    decimal IBidAskPair.AskPrice => BidPriceTop;

    DateTime IBidAskInstant.AtTime => SourceTime;

    public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level1Quote;

    public DateTime AdapterReceivedTime { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime AdapterSentTime     { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime SourceBidTime       { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ValidFrom           { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime ValidTo             { get; set; } = DateTimeConstants.UnixEpoch;

    public virtual decimal BidPriceTop { get; set; }
    public virtual decimal AskPriceTop { get; set; }

    public BidAskPair BidAskTop     => new(BidPriceTop, AskPriceTop);
    public DateTime   SourceAskTime { get; set; } = DateTimeConstants.UnixEpoch;
    public bool       Executable    { get; set; }

    public bool IsBidPriceTopUpdated { get; set; }
    public bool IsAskPriceTopUpdated { get; set; }

    public IMutablePricePeriodSummary? SummaryPeriod { get; set; }
    IPricePeriodSummary? ILevel1Quote. SummaryPeriod => SummaryPeriod;

    public override DateTime SourceTime
    {
        get =>
            new(Math.Max(base.SourceTime.Ticks,
                         Math.Max(SourceBidTime.Ticks, SourceAskTime.Ticks)));
        set => base.SourceTime = value;
    }

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel1Quote level1Quote)
        {
            AdapterReceivedTime = level1Quote.AdapterReceivedTime;
            AdapterSentTime     = level1Quote.AdapterSentTime;
            SourceBidTime       = level1Quote.SourceBidTime;
            SourceAskTime       = level1Quote.SourceAskTime;
            ValidFrom           = level1Quote.ValidFrom;
            ValidTo             = level1Quote.ValidTo;
            if (this is not ILevel2Quote)
            {
                BidPriceTop = level1Quote.BidPriceTop;
                AskPriceTop = level1Quote.AskPriceTop;
            }
            IsAskPriceTopUpdated = level1Quote.IsAskPriceTopUpdated;
            IsBidPriceTopUpdated = level1Quote.IsBidPriceTopUpdated;
            Executable           = level1Quote.Executable;
            if (level1Quote.SummaryPeriod is { IsEmpty: false })
            {
                SummaryPeriod ??= new PricePeriodSummary();
                SummaryPeriod.CopyFrom(level1Quote.SummaryPeriod);
            }
            else if (SummaryPeriod is { IsEmpty: false })
            {
                SummaryPeriod.IsEmpty = true;
            }
        }

        return this;
    }

    IReusableObject<IBidAskInstant> IStoreState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
        (ILevel1Quote)CopyFrom((ILevel1Quote)source, copyMergeFlags);

    IBidAskInstant IStoreState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) =>
        (ILevel1Quote)CopyFrom((ILevel1Quote)source, copyMergeFlags);

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();

    bool IInterfacesComparable<IBidAskInstant>.AreEquivalent(IBidAskInstant? other, bool exactTypes)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var startTimeSame       = SourceTime.Equals(other.AtTime);
        var averageBidPriceSame = BidPriceTop == other.BidPrice;
        var averageAskPriceSame = AskPriceTop == other.AskPrice;

        var allAreSame = startTimeSame && averageBidPriceSame && averageAskPriceSame;
        return allAreSame;
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel1Quote otherL1)) return false;
        var baseIsSame = base.AreEquivalent(otherL1, exactTypes);

        var adapterReceivedTimeSame = AdapterReceivedTime.Equals(otherL1.AdapterReceivedTime);
        var adapterSentTimeSame     = AdapterSentTime.Equals(otherL1.AdapterSentTime);
        var sourceBidTimeSame       = SourceBidTime.Equals(otherL1.SourceBidTime);
        var validFromTimeSame       = ValidFrom.Equals(otherL1.ValidFrom);
        var validToTimeSame         = ValidTo.Equals(otherL1.ValidTo);
        var bidPriceTopSame         = BidPriceTop == otherL1.BidPriceTop;
        var sourceAskTimeSame       = SourceAskTime.Equals(otherL1.SourceAskTime);
        var askPriceTopSame         = AskPriceTop == otherL1.AskPriceTop;
        var executableSame          = Executable == otherL1.Executable;
        var periodSummarySame
            = ((SummaryPeriod == null || SummaryPeriod.IsEmpty) && (otherL1.SummaryPeriod == null || otherL1.SummaryPeriod.IsEmpty)) ||
              (SummaryPeriod?.AreEquivalent(otherL1.SummaryPeriod, exactTypes) ?? otherL1.SummaryPeriod == null);

        var isBidPriceTopChangedSame = IsBidPriceTopUpdated == otherL1.IsBidPriceTopUpdated;
        var isAskPriceTopChangedSame = IsAskPriceTopUpdated == otherL1.IsAskPriceTopUpdated;

        var isEquivalent = baseIsSame && adapterReceivedTimeSame && adapterSentTimeSame && sourceBidTimeSame && validFromTimeSame && validToTimeSame
                        && bidPriceTopSame && isBidPriceTopChangedSame && sourceAskTimeSame && askPriceTopSame && isAskPriceTopChangedSame
                        && executableSame && periodSummarySame;
        return isEquivalent;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ AdapterReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ AdapterSentTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceBidTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidFrom.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidTo.GetHashCode();
            hashCode = (hashCode * 397) ^ BidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsBidPriceTopUpdated.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ AskPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsAskPriceTopUpdated.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ SummaryPeriod?.GetHashCode() ?? 0;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"Level1PriceQuote {{{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, " +
        $"{nameof(SingleTickValue)}: {SingleTickValue:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, " +
        $"{nameof(AdapterSentTime)}: {AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(ValidFrom)}: {ValidFrom:O}, {nameof(ValidTo)}: {ValidTo:O}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop:N5}, {nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SummaryPeriod)}: {SummaryPeriod} }}";
}
