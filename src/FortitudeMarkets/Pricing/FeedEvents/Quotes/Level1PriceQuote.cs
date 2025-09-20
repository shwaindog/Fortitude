// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public class Level1PriceQuote : TickInstant, IMutableLevel1Quote, ICloneable<Level1PriceQuote>
{
    public Level1PriceQuote() { }

    public Level1PriceQuote
    (DateTime? sourceTime = null, decimal bidPriceTop = 0m, decimal askPriceTop = 0m, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null
      , DateTime? validFrom = null, DateTime? validTo = null, bool executable = false, decimal singlePrice = 0m)
        : base(singlePrice, sourceTime)
    {
        QuoteBehavior = quoteBehavior;

        if (!QuoteBehavior.HasNoValidDateTimeUpdatesFlag())
        {
            ValidFrom = validFrom ?? DateTime.MinValue;
            ValidTo   = validTo ?? DateTime.MinValue;
        }
        if (!QuoteBehavior.HasNoSideDateTimesUpdatesFlag())
        {
            SourceBidTime = sourceBidTime ?? DateTime.MinValue;
            SourceAskTime = sourceAskTime ?? DateTime.MinValue;
        }
        IsBidPriceTopChanged = isBidPriceTopChanged;
        IsAskPriceTopChanged = isAskPriceTopChanged;
        Executable           = executable;

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
            QuoteBehavior        = lvl1Quote.QuoteBehavior;

            if (!QuoteBehavior.HasNoValidDateTimeUpdatesFlag())
            {
                ValidFrom = lvl1Quote.ValidFrom;
                ValidTo   = lvl1Quote.ValidTo;
            }
            if (!QuoteBehavior.HasNoSideDateTimesUpdatesFlag())
            {
                SourceBidTime = lvl1Quote.SourceBidTime;
                SourceAskTime = lvl1Quote.SourceAskTime;
            }
            IsBidPriceTopChanged = lvl1Quote.IsBidPriceTopChanged;
            IsAskPriceTopChanged = lvl1Quote.IsAskPriceTopChanged;
            Executable           = lvl1Quote.Executable;

            if (this is not ILevel2Quote)
            {
                BidPriceTop = lvl1Quote.BidPriceTop;
                AskPriceTop = lvl1Quote.AskPriceTop;
            }
        }
    }

    public override Level1PriceQuote Clone() => Recycler?.Borrow<Level1PriceQuote>().CopyFrom(this) ?? new Level1PriceQuote(this);

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();


    [JsonInclude]
    public override DateTime SourceTime
    {
        get =>
            new(Math.Max(base.SourceTime.Ticks,
                         Math.Max(SourceBidTime.Ticks, SourceAskTime.Ticks)));
        set => base.SourceTime = value;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceAskTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceBidTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidFrom { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidTo { get; set; }


    [JsonIgnore] public virtual decimal BidPriceTop { get; set; }
    [JsonIgnore] public virtual decimal AskPriceTop { get; set; }

    public BidAskPair BidAskTop => new(BidPriceTop, AskPriceTop);


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable { get; set; }

    public QuoteInstantBehaviorFlags QuoteBehavior { get; set; }

    [JsonIgnore] public bool IsBidPriceTopChanged { get; set; }
    [JsonIgnore] public bool IsAskPriceTopChanged { get; set; }

    [JsonIgnore] decimal IBidAskPair.    BidPrice => BidPriceTop;
    [JsonIgnore] decimal IBidAskPair.    AskPrice => BidPriceTop;
    [JsonIgnore] DateTime IBidAskInstant.AtTime   => SourceTime;


    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }
    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next     { get; set; }

    public override void IncrementTimeBy(TimeSpan toChangeBy)
    {
        base.IncrementTimeBy(toChangeBy);

        if (!QuoteBehavior.HasNoSideDateTimesUpdatesFlag())
        {
            SourceBidTime += toChangeBy;
            SourceAskTime += toChangeBy;
        }
        if (!QuoteBehavior.HasNoValidDateTimeUpdatesFlag())
        {
            ValidFrom += toChangeBy;
            ValidTo   += toChangeBy;
        }
    }

    IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITickInstant)source, copyMergeFlags);

    IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITickInstant)source, copyMergeFlags);

    public override Level1PriceQuote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel1Quote level1Quote)
        {
            if (!QuoteBehavior.HasNoSideDateTimesUpdatesFlag())
            {
                SourceBidTime = level1Quote.SourceBidTime;
                SourceAskTime = level1Quote.SourceAskTime;
            }
            if (!QuoteBehavior.HasNoValidDateTimeUpdatesFlag())
            {
                ValidFrom = level1Quote.ValidFrom;
                ValidTo   = level1Quote.ValidTo;
            }
            if (this is not ILevel2Quote)
            {
                BidPriceTop = level1Quote.BidPriceTop;
                AskPriceTop = level1Quote.AskPriceTop;
            }
            IsAskPriceTopChanged = level1Quote.IsAskPriceTopChanged;
            IsBidPriceTopChanged = level1Quote.IsBidPriceTopChanged;
            Executable           = level1Quote.Executable;
        }

        return this;
    }

    IMutableLevel1Quote ITrackableReset<IMutableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    public override IMutableLevel1Quote ResetWithTracking()
    {
        BidPriceTop   = 0m;
        AskPriceTop   = 0m;
        SourceBidTime = DateTime.MinValue;
        SourceAskTime = DateTime.MinValue;

        ValidFrom = DateTime.MinValue;
        ValidTo   = DateTime.MinValue;

        IsAskPriceTopChanged = false;
        IsBidPriceTopChanged = false;

        Executable = true;

        base.ResetWithTracking();
        return this;
    }

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
        if (other is not ILevel1Quote otherL1) return false;
        var baseIsSame = base.AreEquivalent(otherL1, exactTypes);

        var sourceBidTimeSame = true;
        var sourceAskTimeSame = true;
        if (!QuoteBehavior.HasIgnoreSideDateTimesCompareFlag())
        {
            sourceBidTimeSame = SourceBidTime.Equals(otherL1.SourceBidTime);
            sourceAskTimeSame = SourceAskTime.Equals(otherL1.SourceAskTime);
        }
        var validFromTimeSame = true;
        var validToTimeSame   = true;
        if (!QuoteBehavior.HasIgnoreValidDateTimesCompareFlag())
        {
            validFromTimeSame = ValidFrom.Equals(otherL1.ValidFrom);
            validToTimeSame   = ValidTo.Equals(otherL1.ValidTo);
        }
        var bidPriceTopSame   = BidPriceTop == otherL1.BidPriceTop;
        var askPriceTopSame   = AskPriceTop == otherL1.AskPriceTop;
        var executableSame    = Executable == otherL1.Executable;

        var isBidPriceTopChangedSame = IsBidPriceTopChanged == otherL1.IsBidPriceTopChanged;
        var isAskPriceTopChangedSame = IsAskPriceTopChanged == otherL1.IsAskPriceTopChanged;

        var isEquivalent = baseIsSame && sourceBidTimeSame && validFromTimeSame && validToTimeSame
                        && bidPriceTopSame && isBidPriceTopChangedSame && sourceAskTimeSame && askPriceTopSame && isAskPriceTopChangedSame
                        && executableSame;
        return isEquivalent;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceBidTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidFrom.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidTo.GetHashCode();
            hashCode = (hashCode * 397) ^ BidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsBidPriceTopChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ AskPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsAskPriceTopChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            return hashCode;
        }
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(SourceBidTime), SourceBidTime, "O")
            .Field.AlwaysAdd(nameof(ValidFrom), ValidFrom, "O")
            .Field.AlwaysAdd(nameof(ValidTo), ValidTo, "O")
            .Field.AlwaysAdd(nameof(BidPriceTop), BidPriceTop, "N5")
            .Field.AlwaysAdd(nameof(IsBidPriceTopChanged), IsBidPriceTopChanged)
            .Field.AlwaysAdd(nameof(SourceAskTime), SourceAskTime, "O")
            .Field.AlwaysAdd(nameof(AskPriceTop), AskPriceTop, "N5")
            .Field.AlwaysAdd(nameof(IsAskPriceTopChanged), IsAskPriceTopChanged)
            .Field.AlwaysAdd(nameof(Executable), Executable)
            .Complete();

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(ValidFrom)}: {ValidFrom:O}, {nameof(ValidTo)}: {ValidTo:O}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop:N5}, {nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopChanged)}: {IsAskPriceTopChanged}, {nameof(Executable)}: {Executable}";

    public override string ToString() => $"{nameof(Level1PriceQuote)}{{{QuoteToStringMembers}}}";
}

public class PublishableLevel1PriceQuote : PublishableTickInstant, IMutablePublishableLevel1Quote, ICloneable<PublishableLevel1PriceQuote>
  , IDoublyLinkedListNode<PublishableLevel1PriceQuote>
{
    public PublishableLevel1PriceQuote() { }

    public PublishableLevel1PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, decimal bidPriceTop = 0m, decimal askPriceTop = 0m,
        PublishableQuoteInstantBehaviorFlags quoteBehavior = PublishableQuoteInstantBehaviorFlags.None, bool isBidPriceTopChanged = false
      , bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null
      , bool executable = false, FeedSyncStatus syncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , decimal singlePrice = 0m, ICandle? conflationTicksCandle = null)
        : this(new Level1PriceQuote(sourceTime, bidPriceTop, askPriceTop, (QuoteInstantBehaviorFlags)quoteBehavior, isBidPriceTopChanged, isAskPriceTopChanged,
                                    sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice),
               sourceTickerInfo, syncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PublishableLevel1PriceQuote
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus syncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , ICandle? conflationTicksCandle = null)
        : base(initialisedQuoteContainer, sourceTickerInfo, syncStatus, feedConnectivityStatus)
    {
        QuoteBehavior = sourceTickerInfo.QuoteBehaviorFlags;
        if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return;
        if (conflationTicksCandle is not null) ConflatedTicksCandle = new Candle(conflationTicksCandle);
    }

    public PublishableLevel1PriceQuote(IPublishableTickInstant toClone) : this(toClone, null) { }

    public PublishableLevel1PriceQuote(IPublishableTickInstant toClone, IMutableTickInstant? initializedQuoteContainer)
        : base(toClone, initializedQuoteContainer)
    {
        if (toClone is IPublishableLevel1Quote lvl1Quote)
        {
            QuoteBehavior = lvl1Quote.QuoteBehavior;
            if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return;
            if (lvl1Quote.ConflatedTicksCandle is { IsEmpty: false }) ConflatedTicksCandle = new Candle(lvl1Quote.ConflatedTicksCandle);
        }
    }

    protected override IMutableLevel1Quote CreateEmptyQuoteContainerInstant() => new Level1PriceQuote();

    protected override IMutableLevel1Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new Level1PriceQuote(tickInstant);

    protected override IMutableLevel1Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new Level1PriceQuote();

    ILevel1Quote IPublishableLevel1Quote.AsNonPublishable => AsNonPublishable;

    public override IMutableLevel1Quote AsNonPublishable => (IMutableLevel1Quote)QuoteContainer;

    public decimal BidPrice => AsNonPublishable.BidPriceTop;

    public decimal AskPrice => AsNonPublishable.AskPriceTop;

    public DateTime AtTime => AsNonPublishable.AtTime;

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level1Quote;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceAskTime
    {
        get => AsNonPublishable.SourceAskTime;
        set => AsNonPublishable.SourceAskTime = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceBidTime
    {
        get => AsNonPublishable.SourceBidTime;
        set => AsNonPublishable.SourceBidTime = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidFrom
    {
        get => AsNonPublishable.ValidFrom;
        set => AsNonPublishable.ValidFrom = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidTo
    {
        get => AsNonPublishable.ValidTo;
        set => AsNonPublishable.ValidTo = value;
    }

    [JsonIgnore]
    public virtual decimal BidPriceTop
    {
        get => AsNonPublishable.BidPriceTop;
        set => AsNonPublishable.BidPriceTop = value;
    }

    [JsonIgnore]
    public virtual decimal AskPriceTop
    {
        get => AsNonPublishable.AskPriceTop;
        set => AsNonPublishable.AskPriceTop = value;
    }

    public BidAskPair BidAskTop => new(BidPriceTop, AskPriceTop);


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => AsNonPublishable.Executable;
        set => AsNonPublishable.Executable = value;
    }

    [JsonIgnore]
    public bool IsBidPriceTopChanged
    {
        get => AsNonPublishable.IsBidPriceTopChanged;
        set => AsNonPublishable.IsBidPriceTopChanged = value;
    }

    [JsonIgnore]
    public bool IsAskPriceTopChanged
    {
        get => AsNonPublishable.IsAskPriceTopChanged;
        set => AsNonPublishable.IsAskPriceTopChanged = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IMutableCandle? ConflatedTicksCandle { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    ICandle? IPublishableLevel1Quote.ConflatedTicksCandle => ConflatedTicksCandle;

    QuoteInstantBehaviorFlags ILevel1Quote.QuoteBehavior => AsNonPublishable.QuoteBehavior;

    QuoteInstantBehaviorFlags IMutableLevel1Quote.QuoteBehavior
    {
        get => AsNonPublishable.QuoteBehavior;
        set => AsNonPublishable.QuoteBehavior = value;
    }

    public override PublishableQuoteInstantBehaviorFlags QuoteBehavior
    {
        get => base.QuoteBehavior;
        set
        {
            if (value.HasRestoreAndOverlayOriginalQuoteFlagsFlag())
            {
                AsNonPublishable.QuoteBehavior |= (QuoteInstantBehaviorFlags)value;
            }
            base.QuoteBehavior = value;
        }
    }

    [JsonIgnore]
    public new PublishableLevel1PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PublishableLevel1PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }
    [JsonIgnore]
    public new PublishableLevel1PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PublishableLevel1PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IPublishableLevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IPublishableLevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IDoublyLinkedListNode<IPublishableLevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IDoublyLinkedListNode<IPublishableLevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous
    {
        get => AsNonPublishable.Previous;
        set => AsNonPublishable.Previous = value;
    }
    IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next
    {
        get => AsNonPublishable.Next;
        set => AsNonPublishable.Next = value;
    }

    IMutableLevel1Quote ITrackableReset<IMutableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel1Quote IMutableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel1Quote ITrackableReset<IMutablePublishableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel1Quote IMutablePublishableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    public override PublishableLevel1PriceQuote ResetWithTracking()
    {
        base.ResetWithTracking();
        return this;
    }

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    public override PublishableLevel1PriceQuote Clone() =>
        Recycler?.Borrow<PublishableLevel1PriceQuote>().CopyFrom(this) ?? new PublishableLevel1PriceQuote(this, QuoteContainer.Clone());

    IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPublishableLevel1Quote pubL1Quote)
        {
            CopyFrom(pubL1Quote, copyMergeFlags);
        }
        else
        {
            AsNonPublishable.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPublishableLevel1Quote pubL1Quote)
        {
            CopyFrom(pubL1Quote, copyMergeFlags);
        }
        else
        {
            AsNonPublishable.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    public override PublishableLevel1PriceQuote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPublishableLevel1Quote level1Quote)
        {
            if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return this;
            if (level1Quote.ConflatedTicksCandle is { IsEmpty: false })
            {
                ConflatedTicksCandle ??= new Candle();
                ConflatedTicksCandle.CopyFrom(level1Quote.ConflatedTicksCandle);
            }
            else if (ConflatedTicksCandle is { IsEmpty: false })
            {
                ConflatedTicksCandle.IsEmpty = true;
            }
        }

        return this;
    }

    IPublishableLevel1Quote ICloneable<IPublishableLevel1Quote>.Clone() => Clone();

    IPublishableLevel1Quote IPublishableLevel1Quote.Clone() => Clone();

    IMutablePublishableLevel1Quote IMutablePublishableLevel1Quote.Clone() => Clone();

    bool IInterfacesComparable<IBidAskInstant>.AreEquivalent(IBidAskInstant? other, bool exactTypes) =>
        AreEquivalent(other as IPublishableLevel1Quote, exactTypes);

    bool IMutableLevel1Quote.AreEquivalent(ITickInstant? other, bool exactTypes) => AreEquivalent(other as IPublishableLevel1Quote, exactTypes);

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other is not IPublishableLevel1Quote otherL1) return false;
        var baseIsSame = base.AreEquivalent(otherL1, exactTypes);
        var conflationTicksCandleSame
            = ((ConflatedTicksCandle == null || ConflatedTicksCandle.IsEmpty) &&
               (otherL1.ConflatedTicksCandle == null || otherL1.ConflatedTicksCandle.IsEmpty)) ||
              (ConflatedTicksCandle?.AreEquivalent(otherL1.ConflatedTicksCandle, exactTypes) ?? otherL1.ConflatedTicksCandle == null);


        var isEquivalent = baseIsSame && conflationTicksCandleSame;
        return isEquivalent;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableLevel1Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceBidTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidFrom.GetHashCode();
            hashCode = (hashCode * 397) ^ ValidTo.GetHashCode();
            hashCode = (hashCode * 397) ^ BidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsBidPriceTopChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ AskPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsAskPriceTopChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ ConflatedTicksCandle?.GetHashCode() ?? 0;
            return hashCode;
        }
    }

    public override string QuoteToStringMembers => $"{base.QuoteToStringMembers}, {nameof(ConflatedTicksCandle)}: {ConflatedTicksCandle}";

    public override StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(QuoteContainer), QuoteContainer)
            .Field.AlwaysAdd(nameof(ConflatedTicksCandle), ConflatedTicksCandle)
            .Field.AlwaysAdd(nameof(QuoteBehavior), QuoteBehavior)
            .Complete();

    public override string ToString() =>
        $"{nameof(PublishableLevel1PriceQuote)}{{{QuoteToStringMembers}, {AsNonPublishable.QuoteToStringMembers}, " +
        $"{JustFeedSyncConnectivityStatusToStringMembers}}}";
}
