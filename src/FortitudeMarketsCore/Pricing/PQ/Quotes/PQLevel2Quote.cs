using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    public class PQLevel2Quote : PQLevel1Quote, IPQLevel2Quote
    {
        private IPQOrderBook bidBook;
        private IPQOrderBook askBook;

        [Obsolete]
        public PQLevel2Quote()
        {
            throw new NotSupportedException();
        }

        public PQLevel2Quote(ISourceTickerQuoteInfo sourceTickerInfo)
            : base(sourceTickerInfo)
        {
            bidBook = new PQOrderBook(PQSourceTickerQuoteInfo);
            askBook = new PQOrderBook(PQSourceTickerQuoteInfo);
            // ReSharper disable once VirtualMemberCallInConstructor
            EnsureRelatedItemsAreConfigured(this);
        }

        public PQLevel2Quote(ILevel0Quote toClone) : base(toClone)
        {
            if (toClone is IPQLevel2Quote l2QToClone)
            {
                bidBook = l2QToClone.BidBook.Clone();
                askBook = l2QToClone.AskBook.Clone();
            } else if (toClone is ILevel2Quote l2Q)
            {
                bidBook = new PQOrderBook(l2Q.BidBook);
                askBook = new PQOrderBook(l2Q.AskBook);
            }
            // ReSharper disable once VirtualMemberCallInConstructor
            EnsureRelatedItemsAreConfigured(this);
        }
        
        public bool IsBidBookChanged
        {
            get => bidBook.HasUpdates;
            set => bidBook.HasUpdates = value;
        }

        public bool IsAskBookChanged
        {
            get => askBook.HasUpdates;
            set => askBook.HasUpdates = value;
        }

        public override bool HasUpdates
        {
            get => base.HasUpdates || bidBook.HasUpdates || askBook.HasUpdates;
            set
            {
                base.HasUpdates = value;
                bidBook.HasUpdates = value;
                askBook.HasUpdates = value;
            }
        }

        IOrderBook ILevel2Quote.BidBook => BidBook;
        IOrderBook ILevel2Quote.AskBook => AskBook;

        IMutableOrderBook IMutableLevel2Quote.BidBook
        {
            get => BidBook;
            set => BidBook = (IPQOrderBook)value;
        }

        IMutableOrderBook IMutableLevel2Quote.AskBook
        {
            get => AskBook;
            set => AskBook = (IPQOrderBook)value;
        }

        public IPQOrderBook BidBook
        {
            get => bidBook;
            set
            {
                bidBook = value;
                EnsureRelatedItemsAreConfigured(this);
            }
        }

        public IPQOrderBook AskBook
        {
            get => askBook;
            set
            {
                askBook = value;
                EnsureRelatedItemsAreConfigured(this);
            }
        }

        public override decimal BidPriceTop
        {
            get => BidBook[0].Price;
            set
            {
                if (BidBook[0].Price == value) return;
                BidBook[0].Price = value;
                IsBidPriceTopUpdated = true;
            }
        }

        public override decimal AskPriceTop
        {
            get => AskBook[0].Price;
            set
            {
                if (AskBook[0].Price == value) return;
                AskBook[0].Price = value;
                IsAskPriceTopUpdated = true;
            }
        }

        public override void ResetFields()
        {
            base.ResetFields();
            bidBook.Reset();
            bidBook.HasUpdates = false;
            askBook.Reset();
            askBook.HasUpdates = false;
        }

        public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            quotePublicationPrecisionSetting = quotePublicationPrecisionSetting ?? PQSourceTickerQuoteInfo;
            foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                quotePublicationPrecisionSetting))
            {
                yield return updatedField;
            }
            foreach (var bidFields in bidBook.GetDeltaUpdateFields(snapShotTime, updateStyle,
                quotePublicationPrecisionSetting))
            {
                yield return bidFields;
            }
            foreach (var askField in askBook.GetDeltaUpdateFields(snapShotTime,
                updateStyle, quotePublicationPrecisionSetting))
            {
                yield return new PQFieldUpdate(askField.Id, askField.Value,
                    (byte)(askField.Flag | PQFieldFlags.IsAskSideFlag));
            }
        }

        protected override IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields(DateTime snapShotTime, 
            bool updatedOnly, IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSettings = null)
        {
            yield break;
        }

        public override int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            if (pqFieldUpdate.Id == PQFieldKeys.LayerNameDictionaryUpsertCommand)
            {
                return (int)pqFieldUpdate.Value;
            }
            if (pqFieldUpdate.Id >= PQFieldKeys.FirstLayersRangeStart && 
                pqFieldUpdate.Id <= PQFieldKeys.FirstLayersRangeEnd ||
                pqFieldUpdate.Id >= PQFieldKeys.SecondLayersRangeStart &&
                pqFieldUpdate.Id <= PQFieldKeys.SecondLayersRangeEnd)
            {
                var result = pqFieldUpdate.IsBid()
                    ? bidBook.UpdateField(pqFieldUpdate)
                    : askBook.UpdateField(pqFieldUpdate);
                if (pqFieldUpdate.Id == PQFieldKeys.LayerPriceOffset)
                {
                    if (pqFieldUpdate.IsBid())
                    {
                        IsBidPriceTopUpdated = true;
                    }
                    else
                    {
                        IsAskPriceTopUpdated = true;
                    }
                }
                return result;
            }
            return base.UpdateField(pqFieldUpdate);
        }

        public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, 
            UpdateStyle updatedStyle)
        {
            foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, updatedStyle))
            {
                yield return pqFieldStringUpdate;
            }
            // bid and askbook should share same dictionary so just pick either one.
            foreach (var pqFieldStringUpdate in bidBook.GetStringUpdates(snapShotTime, updatedStyle))
            {
                yield return pqFieldStringUpdate;
            }
        }

        public override bool UpdateFieldString(PQFieldStringUpdate updates)
        {
            var found = base.UpdateFieldString(updates);
            if (found) return true;
            if (updates.Field.Id == PQFieldKeys.LayerNameDictionaryUpsertCommand)
            {
                // share dictionary so just updated bidbook.
                return bidBook.UpdateFieldString(updates);
            }
            return false;
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

        IPQLevel2Quote IPQLevel2Quote.Clone()
        {
            return (IPQLevel2Quote)Clone();
        }

        public override object Clone()
        {
            var clone = new PQLevel2Quote(this);
            return clone;
        }

        public override void CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            base.CopyFrom(source);

            if (!(source is ILevel2Quote l2Q)) return;
            Type originalType = null;
            if (bidBook.AllLayers.Any())
            {
                originalType = bidBook[0].GetType();
            }
            bidBook.CopyFrom(l2Q.BidBook);
            askBook.CopyFrom(l2Q.AskBook);
            Type newType = null;
            if (bidBook.AllLayers.Any())
            {
                newType = bidBook[0].GetType();
            }
            if (newType != originalType)
            {
                EnsureRelatedItemsAreConfigured(this);
            }
        }

        public override void EnsureRelatedItemsAreConfigured(ILevel0Quote quote)
        {
            var previousSrcTkrQtInfo = PQSourceTickerQuoteInfo;
            base.EnsureRelatedItemsAreConfigured(quote);
            if (!ReferenceEquals(previousSrcTkrQtInfo, PQSourceTickerQuoteInfo))
            {
                bidBook.EnsureRelatedItemsAreConfigured(PQSourceTickerQuoteInfo);
                askBook.EnsureRelatedItemsAreConfigured(PQSourceTickerQuoteInfo);
            }
            if (bidBook?.AllLayers?.Any() ?? false)
            {
                if (!ReferenceEquals(this, quote) && quote is IPQLevel2Quote pqLevel2Quote)
                {
                    //share name lookups between many quotes
                    IPQPriceVolumeLayer otherLayer = null;
                    if (pqLevel2Quote.BidBook?.AllLayers?.Any() ?? false)
                    {
                        otherLayer = pqLevel2Quote.BidBook[0];
                    }
                    else if (pqLevel2Quote.AskBook?.AllLayers?.Any() ?? false)
                    {
                        otherLayer = pqLevel2Quote.AskBook[0];
                    }
                    if (otherLayer != null)
                    {
                        bidBook[0].EnsureRelatedItemsAreConfigured(otherLayer);
                    }
                }
                bidBook[0].EnsureRelatedItemsAreConfigured(PQSourceTickerQuoteInfo);
                bidBook.EnsureRelatedItemsAreConfigured(bidBook[0]);
                askBook?.EnsureRelatedItemsAreConfigured(bidBook[0]);
            }
            else if (askBook?.AllLayers?.Any() ?? false) // should never execute but just incase
            {
                askBook[0].EnsureRelatedItemsAreConfigured(PQSourceTickerQuoteInfo);
                askBook.EnsureRelatedItemsAreConfigured(askBook[0]);
            }
        }

        public override bool AreEquivalent(ILevel0Quote other, bool exactTypes = false)
        {
            if (!(other is ILevel2Quote otherL2)) return false;
            var baseSame = base.AreEquivalent(other, exactTypes);
            var bidBooksSame = bidBook?.AreEquivalent(otherL2.BidBook, exactTypes) ?? otherL2.BidBook == null;
            var askBookSame = askBook?.AreEquivalent(otherL2.AskBook, exactTypes) ?? otherL2.AskBook == null;
            var bidBookChangedSame = true;
            var askBookChangedSame = true;
            if (exactTypes)
            {
                bidBookChangedSame = IsBidBookChanged == otherL2.IsBidBookChanged;
                askBookChangedSame = IsAskBookChanged == otherL2.IsAskBookChanged;
            }
            return baseSame && bidBooksSame && askBookSame && bidBookChangedSame && askBookChangedSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((ILevel0Quote) obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (bidBook != null ? bidBook.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (askBook != null ? askBook.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
