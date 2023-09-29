using System;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LayeredBook;

namespace FortitudeMarketsCore.Pricing.Quotes
{
    public class Level2PriceQuote : Level1PriceQuote, IMutableLevel2Quote
    {
        public Level2PriceQuote(ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null, 
            bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null, 
            DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, 
            DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false, 
            DateTime? sourceAskTime = null, bool isAskPriceTopChanged = false, 
            bool executable = false, IPeriodSummary periodSummary = null, IOrderBook bidBook = null, 
            bool isBidBookChanged = false, IOrderBook askBook = null, bool isAskBookChanged = false) 
            : base(sourceTickerQuoteInfo, sourceTime, isReplay, singlePrice, clientReceivedTime, adapterReceivedTime, 
                  adapterSentTime, sourceBidTime, 0m, isBidPriceTopChanged, sourceAskTime, 0m, 
                  isAskPriceTopChanged, executable, periodSummary)
        {
            if (bidBook is OrderBook mutBidOrderBook)
            {
                BidBook = mutBidOrderBook;
            }
            else
            {
                BidBook = bidBook != null ? new OrderBook(bidBook) : new OrderBook(SourceTickerQuoteInfo);
            }
            IsBidBookChanged = isBidBookChanged;
            if (askBook is OrderBook mutAskOrderBook)
            {
                AskBook = mutAskOrderBook;
            }
            else
            {
                AskBook = askBook != null ? new OrderBook(askBook) : new OrderBook(SourceTickerQuoteInfo);
            }
            IsAskBookChanged = isAskBookChanged;
        }

        public Level2PriceQuote(ILevel0Quote toClone) : base(toClone)
        {
            if (toClone is ILevel2Quote level2ToClone)
            {
                if (level2ToClone.BidBook is OrderBook bidOrdBook)
                {
                    BidBook = bidOrdBook.Clone();
                }
                else
                {
                    BidBook = new OrderBook(level2ToClone.BidBook);
                }

                IsBidBookChanged = level2ToClone.IsBidBookChanged;

                if (level2ToClone.AskBook is OrderBook askOrdBook)
                {
                    AskBook = askOrdBook.Clone();
                }
                else
                {
                    AskBook = new OrderBook(level2ToClone.AskBook);
                }

                IsAskBookChanged = level2ToClone.IsAskBookChanged;
            }
        }
        public IMutableOrderBook BidBook { get; set; }
        IOrderBook ILevel2Quote.BidBook => BidBook;
        public bool IsBidBookChanged { get; set; }
        public IMutableOrderBook AskBook { get; set; }
        IOrderBook ILevel2Quote.AskBook => AskBook;
        public bool IsAskBookChanged { get; set; }

        public override decimal BidPriceTop
        {
            get => BidBook != null && BidBook.Any() ? BidBook[0].Price : 0m;
            set
            {
                if (BidBook != null && BidBook.Any())
                {
                    BidBook[0].Price = value;
                }
                else
                {
                    BidBook = new OrderBook(SourceTickerQuoteInfo) { [0] = { Price = value } };
                }
            }
        }

        public override decimal AskPriceTop
        {
            get => AskBook != null && AskBook.Any() ? AskBook[0].Price : 0m;
            set
            {
                if (AskBook != null && AskBook.Any())
                {
                    AskBook[0].Price = value;
                }
                else
                {
                    AskBook = new OrderBook(SourceTickerQuoteInfo) { [0] = { Price = value } };
                }
            }
        }

        public override void CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source, copyMergeFlags);

            if (source is ILevel2Quote level2Quote)
            {
                BidBook.CopyFrom(level2Quote.BidBook);
                AskBook.CopyFrom(level2Quote.AskBook);
                IsBidBookChanged = level2Quote.IsBidBookChanged;
                IsAskBookChanged = level2Quote.IsAskBookChanged;
            }
        }

        public override object Clone()
        {
            return new Level2PriceQuote(this);
        }

        ILevel2Quote ICloneable<ILevel2Quote>.Clone()
        {
            return (ILevel2Quote)Clone();
        }

        ILevel2Quote ILevel2Quote.Clone()
        {
            return (ILevel2Quote)Clone();
        }

        IMutableLevel2Quote IMutableLevel2Quote.Clone()
        {
            return (IMutableLevel2Quote)Clone();
        }

        public override bool AreEquivalent(ILevel0Quote other, bool exactTypes = false)
        {
            if (!(other is ILevel2Quote otherL2)) return false;
            var baseIsSame = base.AreEquivalent(otherL2, exactTypes);

            var bidBooksSame = BidBook?.AreEquivalent(otherL2.BidBook, exactTypes) ?? otherL2.BidBook == null;
            var askBooksSame = AskBook?.AreEquivalent(otherL2.AskBook, exactTypes) ?? otherL2.AskBook == null;
            var bidBookChangedSame = true;
            var askBookChangedSame = true;
            if (exactTypes)
            {
                bidBookChangedSame = IsBidBookChanged == otherL2.IsBidBookChanged;
                askBookChangedSame = IsAskBookChanged == otherL2.IsAskBookChanged;
            }

            return baseIsSame && bidBooksSame && bidBookChangedSame && askBooksSame && askBookChangedSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel2Quote, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (BidBook != null ? BidBook.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsBidBookChanged.GetHashCode();
                hashCode = (hashCode * 397) ^ (AskBook != null ? AskBook.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsAskBookChanged.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Level2PriceQuote {{{nameof(SourceTickerQuoteInfo)}: {SourceTickerQuoteInfo}, " +
                   $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SinglePrice)}: " +
                   $"{SinglePrice:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
                   $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
                   $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: {BidPriceTop:N5}, " +
                   $"{nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
                   $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, " +
                   $"{nameof(Executable)}: {Executable}, {nameof(PeriodSummary)}: {PeriodSummary}, {nameof(BidBook)}:" +
                   $" {BidBook}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskBook)}: {AskBook}, " +
                   $"{nameof(IsAskBookChanged)}: {IsAskBookChanged}}}";
        }
    }
}
