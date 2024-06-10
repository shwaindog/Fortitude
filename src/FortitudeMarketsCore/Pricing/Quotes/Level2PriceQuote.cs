// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class Level2PriceQuote : Level1PriceQuote, IMutableLevel2Quote
{
    public Level2PriceQuote()
    {
        BidBook = new OrderBook(BookSide.BidBook, 20);
        AskBook = new OrderBook(BookSide.AskBook, 20);
    }

    public Level2PriceQuote(ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null,
        bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null,
        DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null,
        DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false,
        DateTime? sourceAskTime = null, bool isAskPriceTopChanged = false,
        bool executable = false, IPricePeriodSummary? periodSummary = null, IOrderBook? bidBook = null,
        bool isBidBookChanged = false, IOrderBook? askBook = null, bool isAskBookChanged = false)
        : base(sourceTickerQuoteInfo, sourceTime, isReplay, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, 0m, isBidPriceTopChanged, sourceAskTime, 0m,
               isAskPriceTopChanged, executable, periodSummary)
    {
        if (bidBook is OrderBook mutBidOrderBook)
            BidBook = mutBidOrderBook;
        else
            BidBook = bidBook != null ? new OrderBook(bidBook) : new OrderBook(BookSide.BidBook, SourceTickerQuoteInfo!);
        IsBidBookChanged = isBidBookChanged;
        if (askBook is OrderBook mutAskOrderBook)
            AskBook = mutAskOrderBook;
        else
            AskBook = askBook != null ? new OrderBook(askBook) : new OrderBook(BookSide.AskBook, SourceTickerQuoteInfo!);
        IsAskBookChanged = isAskBookChanged;
    }

    public Level2PriceQuote(ILevel0Quote toClone) : base(toClone)
    {
        if (toClone is ILevel2Quote level2ToClone)
        {
            if (level2ToClone.BidBook is OrderBook bidOrdBook)
                BidBook = bidOrdBook.Clone();
            else
                BidBook = new OrderBook(level2ToClone.BidBook);

            IsBidBookChanged = level2ToClone.IsBidBookChanged;

            if (level2ToClone.AskBook is OrderBook askOrdBook)
                AskBook = askOrdBook.Clone();
            else
                AskBook = new OrderBook(level2ToClone.AskBook);

            IsAskBookChanged = level2ToClone.IsAskBookChanged;
        }
        else
        {
            BidBook = new OrderBook(BookSide.BidBook, 20);
            AskBook = new OrderBook(BookSide.AskBook, 20);
        }
    }

    public override QuoteLevel QuoteLevel => QuoteLevel.Level2;

    public IMutableOrderBook BidBook          { get; set; }
    IOrderBook ILevel2Quote. BidBook          => BidBook;
    public bool              IsBidBookChanged { get; set; }
    public IMutableOrderBook AskBook          { get; set; }
    IOrderBook ILevel2Quote. AskBook          => AskBook;
    public bool              IsAskBookChanged { get; set; }


    public DateTime StorageTime(IStorageTimeResolver<ILevel2Quote>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    public override decimal BidPriceTop
    {
        get => BidBook.Any() ? BidBook[0]?.Price ?? 0 : 0m;
        set
        {
            if (BidBook.Capacity > 0 && BidBook[0] != null)
            {
                BidBook[0]!.Price = value;
            }
            else
            {
                BidBook[0]
                    = OrderBook.LayerSelector.FindForLayerFlags(SourceTickerQuoteInfo!) as
                        IMutablePriceVolumeLayer;
                BidBook[0]!.Price = value;
            }
        }
    }

    public override decimal AskPriceTop
    {
        get => AskBook.Any() ? AskBook[0]?.Price ?? 0 : 0m;
        set
        {
            if (AskBook.Capacity > 0 && BidBook[0] != null)
            {
                AskBook[0]!.Price = value;
            }
            else
            {
                AskBook[0]
                    = OrderBook.LayerSelector.FindForLayerFlags(SourceTickerQuoteInfo!) as
                        IMutablePriceVolumeLayer;
                AskBook[0]!.Price = value;
            }
        }
    }

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel2Quote level2Quote)
        {
            BidBook.CopyFrom(level2Quote.BidBook);
            AskBook.CopyFrom(level2Quote.AskBook);
        }
        if (source is ILevel1Quote level1Quote)
        {
            IsBidPriceTopUpdated = level1Quote.IsBidPriceTopUpdated;
            IsBidPriceTopUpdated = level1Quote.IsBidPriceTopUpdated;
        }

        return this;
    }


    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => (ILevel2Quote)Clone();

    ILevel2Quote ILevel2Quote.Clone() => (ILevel2Quote)Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => (IMutableLevel2Quote)Clone();

    public override IMutableLevel0Quote Clone() =>
        (IMutableLevel0Quote?)Recycler?.Borrow<Level2PriceQuote>().CopyFrom(this) ?? new Level2PriceQuote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (!(other is ILevel2Quote otherL2)) return false;
        var baseIsSame = base.AreEquivalent(otherL2, exactTypes);

        var bidBooksSame       = BidBook?.AreEquivalent(otherL2.BidBook, exactTypes) ?? otherL2?.BidBook == null;
        var askBooksSame       = AskBook?.AreEquivalent(otherL2!.AskBook, exactTypes) ?? otherL2?.AskBook == null;
        var bidBookChangedSame = true;
        var askBookChangedSame = true;
        if (exactTypes)
        {
            bidBookChangedSame = IsBidBookChanged == otherL2!.IsBidBookChanged;
            askBookChangedSame = IsAskBookChanged == otherL2.IsAskBookChanged;
        }

        var allAreSame = baseIsSame && bidBooksSame && bidBookChangedSame && askBooksSame && askBookChangedSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel2Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (BidBook != null ? BidBook.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsBidBookChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ (AskBook != null ? AskBook.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsAskBookChanged.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"Level2PriceQuote {{{nameof(SourceTickerQuoteInfo)}: {SourceTickerQuoteInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SinglePrice)}: " +
        $"{SinglePrice:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
        $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: {BidPriceTop:N5}, " +
        $"{nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(BidBook)}:" +
        $" {BidBook}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskBook)}: {AskBook}, " +
        $"{nameof(IsAskBookChanged)}: {IsAskBookChanged}}}";
}
