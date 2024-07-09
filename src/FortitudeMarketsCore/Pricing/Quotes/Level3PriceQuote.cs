// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class Level3PriceQuote : Level2PriceQuote, IMutableLevel3Quote, ITimeSeriesEntry<Level3PriceQuote>
{
    public Level3PriceQuote() { }

    public Level3PriceQuote
    (ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null,
        bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null,
        DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null,
        bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, bool isAskPriceTopChanged = false,
        bool executable = false, IPricePeriodSummary? periodSummary = null, IOrderBook? bidBook = null,
        bool isBidBookChanged = false, IOrderBook? askBook = null, bool isAskBookChanged = false,
        IRecentlyTraded? recentlyTraded = null, uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null)
        : base(sourceTickerQuoteInfo, sourceTime, isReplay, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, isAskPriceTopChanged, executable,
               periodSummary, bidBook, isBidBookChanged, askBook, isAskBookChanged)
    {
        if (recentlyTraded is RecentlyTraded mutableRecentlyTraded)
            RecentlyTraded = mutableRecentlyTraded;
        else if (recentlyTraded != null)
            RecentlyTraded                                                                     = new RecentlyTraded(recentlyTraded);
        else if (sourceTickerQuoteInfo.LastTradedFlags != LastTradedFlags.None) RecentlyTraded = new RecentlyTraded(sourceTickerQuoteInfo);
        BatchId              = batchId;
        SourceQuoteReference = sourceQuoteRef;
        ValueDate            = valueDate ?? DateTimeConstants.UnixEpoch;
    }

    public Level3PriceQuote(ILevel0Quote toClone) : base(toClone)
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

    public override QuoteLevel QuoteLevel => QuoteLevel.Level3;

    public IMutableRecentlyTraded? RecentlyTraded { get; set; }

    IRecentlyTraded? ILevel3Quote.RecentlyTraded => RecentlyTraded;
    public uint                   BatchId        { get; set; }

    public uint SourceQuoteReference { get; set; }

    public DateTime ValueDate { get; set; } = DateTimeConstants.UnixEpoch;

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => (ILevel3Quote)Clone();

    DateTime ITimeSeriesEntry<ILevel3Quote>.StorageTime(IStorageTimeResolver<ILevel3Quote>? resolver) => StorageTime(resolver);

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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

    ILevel3Quote ILevel3Quote.Clone() => (ILevel3Quote)Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => (IMutableLevel3Quote)Clone();

    public override IMutableLevel0Quote Clone() =>
        (IMutableLevel0Quote?)Recycler?.Borrow<Level3PriceQuote>().CopyFrom(this) ?? new Level3PriceQuote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
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

    public DateTime StorageTime(IStorageTimeResolver<Level3PriceQuote>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILevel0Quote?)obj, true);

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
        $"Level3PriceQuote{{{nameof(SourceTickerQuoteInfo)}: {SourceTickerQuoteInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SinglePrice)}: " +
        $"{SinglePrice:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
        $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: " +
        $"{BidPriceTop:N5}, {nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(SourceAskTime)}: " +
        $"{SourceAskTime:O}, {nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopUpdated)}: " +
        $"{IsAskPriceTopUpdated}, {nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: " +
        $"{SummaryPeriod}, {nameof(BidBook)}: {BidBook}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, " +
        $"{nameof(AskBook)}: {AskBook}, {nameof(IsAskBookChanged)}: {IsAskBookChanged}, " +
        $"{nameof(RecentlyTraded)}: {RecentlyTraded}, {nameof(BatchId)}: {BatchId}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference}, {nameof(ValueDate)}: {ValueDate:u} }}";
}
