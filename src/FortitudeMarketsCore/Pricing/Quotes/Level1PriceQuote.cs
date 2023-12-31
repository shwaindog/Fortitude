﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Conflation;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class Level1PriceQuote : Level0PriceQuote, IMutableLevel1Quote
{
    public Level1PriceQuote() { }

    public Level1PriceQuote(ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null,
        bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null,
        DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null,
        DateTime? sourceBidTime = null, decimal bidPriceTop = 0m, bool isBidPriceTopChanged = false,
        DateTime? sourceAskTime = null, decimal askPriceTop = 0m, bool isAskPriceTopChanged = false,
        bool executable = false, IPeriodSummary? periodSummary = null)
        : base(sourceTickerQuoteInfo, sourceTime, isReplay, singlePrice, clientReceivedTime)
    {
        AdapterReceivedTime = adapterReceivedTime ?? DateTimeConstants.UnixEpoch;
        AdapterSentTime = adapterSentTime ?? DateTimeConstants.UnixEpoch;
        SourceBidTime = sourceBidTime ?? DateTimeConstants.UnixEpoch;
        IsBidPriceTopUpdated = isBidPriceTopChanged;
        SourceAskTime = sourceAskTime ?? DateTimeConstants.UnixEpoch;
        IsAskPriceTopUpdated = isAskPriceTopChanged;
        Executable = executable;
        if (periodSummary is PeriodSummary periodSummaryInstance)
            PeriodSummary = periodSummaryInstance.Clone();
        else if (periodSummary != null) PeriodSummary = new PeriodSummary(periodSummary);
        if (!(this is Level2PriceQuote))
        {
            BidPriceTop = bidPriceTop;
            AskPriceTop = askPriceTop;
        }
    }

    public Level1PriceQuote(ILevel0Quote toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote lvl1Quote)
        {
            AdapterReceivedTime = lvl1Quote.AdapterReceivedTime;
            AdapterSentTime = lvl1Quote.AdapterSentTime;
            SourceBidTime = lvl1Quote.SourceBidTime;
            IsBidPriceTopUpdated = lvl1Quote.IsBidPriceTopUpdated;
            SourceAskTime = lvl1Quote.SourceAskTime;
            IsAskPriceTopUpdated = lvl1Quote.IsAskPriceTopUpdated;
            Executable = lvl1Quote.Executable;
            if (lvl1Quote.PeriodSummary is PeriodSummary periodSummary)
                PeriodSummary = periodSummary.Clone();
            else if (lvl1Quote.PeriodSummary != null) PeriodSummary = new PeriodSummary(lvl1Quote.PeriodSummary);

            if (!(this is Level2PriceQuote))
            {
                BidPriceTop = lvl1Quote.BidPriceTop;
                AskPriceTop = lvl1Quote.AskPriceTop;
            }
        }
    }

    public DateTime AdapterReceivedTime { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime AdapterSentTime { get; set; } = DateTimeConstants.UnixEpoch;
    public DateTime SourceBidTime { get; set; } = DateTimeConstants.UnixEpoch;
    public virtual decimal BidPriceTop { get; set; }
    public bool IsBidPriceTopUpdated { get; set; }
    public DateTime SourceAskTime { get; set; } = DateTimeConstants.UnixEpoch;
    public virtual decimal AskPriceTop { get; set; }
    public bool IsAskPriceTopUpdated { get; set; }
    public bool Executable { get; set; }
    public IMutablePeriodSummary? PeriodSummary { get; set; }
    IPeriodSummary? ILevel1Quote.PeriodSummary => PeriodSummary;

    public override DateTime SourceTime
    {
        get =>
            new(Math.Max(base.SourceTime.Ticks,
                Math.Max(SourceBidTime.Ticks, SourceAskTime.Ticks)));
        set => base.SourceTime = value;
    }

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel1Quote level1Quote)
        {
            AdapterReceivedTime = level1Quote.AdapterReceivedTime;
            AdapterSentTime = level1Quote.AdapterSentTime;
            SourceBidTime = level1Quote.SourceBidTime;
            SourceAskTime = level1Quote.SourceAskTime;
            BidPriceTop = level1Quote.BidPriceTop;
            AskPriceTop = level1Quote.AskPriceTop;
            IsAskPriceTopUpdated = level1Quote.IsAskPriceTopUpdated;
            IsBidPriceTopUpdated = level1Quote.IsBidPriceTopUpdated;
            Executable = level1Quote.Executable;
            if (level1Quote.PeriodSummary != null)
            {
                if (PeriodSummary != null)
                    PeriodSummary.CopyFrom(level1Quote.PeriodSummary);
                else
                    PeriodSummary = new PeriodSummary(level1Quote.PeriodSummary);
            }
            else if (PeriodSummary != null)
            {
                PeriodSummary = null;
            }
        }

        return this;
    }

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => (ILevel1Quote)Clone();

    ILevel1Quote ILevel1Quote.Clone() => (ILevel1Quote)Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => (IMutableLevel1Quote)Clone();

    public override IMutableLevel0Quote Clone() =>
        (IMutableLevel0Quote?)Recycler?.Borrow<Level1PriceQuote>().CopyFrom(this) ?? new Level1PriceQuote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (!(other is ILevel1Quote otherL1)) return false;
        var baseIsSame = base.AreEquivalent(otherL1, exactTypes);

        var adapterReceivedTimeSame = AdapterReceivedTime.Equals(otherL1.AdapterReceivedTime);
        var adapterSentTimeSame = AdapterSentTime.Equals(otherL1.AdapterSentTime);
        var sourceBidTimeSame = SourceBidTime.Equals(otherL1.SourceBidTime);
        var bidPriceTopSame = BidPriceTop == otherL1.BidPriceTop;
        var sourceAskTimeSame = SourceAskTime.Equals(otherL1.SourceAskTime);
        var askPriceTopSame = AskPriceTop == otherL1.AskPriceTop;
        var executableSame = Executable == otherL1.Executable;
        var periodSummarySame = PeriodSummary?.AreEquivalent(otherL1.PeriodSummary, exactTypes)
                                ?? otherL1.PeriodSummary == null;
        var isBidPriceTopChangedSame = IsBidPriceTopUpdated == otherL1.IsBidPriceTopUpdated;
        var isAskPriceTopChangedSame = IsAskPriceTopUpdated == otherL1.IsAskPriceTopUpdated;

        return baseIsSame && adapterReceivedTimeSame && adapterSentTimeSame && sourceBidTimeSame &&
               bidPriceTopSame && isBidPriceTopChangedSame && sourceAskTimeSame && askPriceTopSame &&
               isAskPriceTopChangedSame && executableSame && periodSummarySame;
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
            hashCode = (hashCode * 397) ^ BidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsBidPriceTopUpdated.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ AskPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ IsAskPriceTopUpdated.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ PeriodSummary?.GetHashCode() ?? 0;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"Level1PriceQuote {{{nameof(SourceTickerQuoteInfo)}: {SourceTickerQuoteInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SinglePrice)}: " +
        $"{SinglePrice:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
        $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: {BidPriceTop:N5}, " +
        $"{nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(PeriodSummary)}: {PeriodSummary} }}";
}
