using System;
using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded
{
    public class PQLastTrade : IPQLastTrade
    {
        private DateTime tradeTime = DateTimeConstants.UnixEpoch;
        private decimal tradePrice;
        protected LastTradeUpdated UpdatedFlags;
        
        public PQLastTrade(decimal tradePrice = 0m, DateTime?  tradeTime = null)
        {
            TradeTime = tradeTime ?? DateTimeConstants.UnixEpoch;
            TradePrice = tradePrice;
        }

        public PQLastTrade(ILastTrade toClone)
        {
            TradePrice = toClone.TradePrice;
            TradeTime = toClone.TradeTime;
            if (toClone is IPQLastTrade pqLastTrade)
            {
                IsTradeTimeDateUpdated = pqLastTrade.IsTradeTimeDateUpdated;
                IsTradeTimeSubHourUpdated = pqLastTrade.IsTradeTimeSubHourUpdated;
                IsTradePriceUpdated = pqLastTrade.IsTradePriceUpdated;
            }
        }

        public DateTime TradeTime
        {
            get => tradeTime;
            set
            {
                if (value == tradeTime) return;
                IsTradeTimeDateUpdated |= tradeTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
                IsTradeTimeSubHourUpdated |= tradeTime.GetSubHourComponent() != value.GetSubHourComponent();
                tradeTime = value;
            }
        }

        public bool IsTradeTimeDateUpdated
        {
            get => (UpdatedFlags & LastTradeUpdated.TradeTimeDateUpdated) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= LastTradeUpdated.TradeTimeDateUpdated;
                }
                else if(IsTradeTimeDateUpdated)
                {
                    UpdatedFlags ^= LastTradeUpdated.TradeTimeDateUpdated;
                }
            }
        }

        public bool IsTradeTimeSubHourUpdated
        {
            get => (UpdatedFlags & LastTradeUpdated.TradeTimeSubHourUpdated) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= LastTradeUpdated.TradeTimeSubHourUpdated;
                }
                else if (IsTradeTimeSubHourUpdated)
                {
                    UpdatedFlags ^= LastTradeUpdated.TradeTimeSubHourUpdated;
                }
            }
        }

        public decimal TradePrice
        {
            get => tradePrice;
            set
            {
                if (value == tradePrice) return;
                IsTradePriceUpdated = true;
                tradePrice = value;
            }
        }

        public bool IsTradePriceUpdated
        {
            get => (UpdatedFlags & LastTradeUpdated.TradePriceUpdated) > 0;
            set
            {
                if (value)
                {
                    UpdatedFlags |= LastTradeUpdated.TradePriceUpdated;
                }
                else if(IsTradePriceUpdated)
                {
                    UpdatedFlags ^= LastTradeUpdated.TradePriceUpdated;
                }
            }
        }

        public virtual bool HasUpdates
        {
            get => UpdatedFlags != LastTradeUpdated.None;
            set => IsTradePriceUpdated = IsTradeTimeDateUpdated = IsTradeTimeSubHourUpdated = value;
        }

        public virtual bool IsEmpty => TradeTime == DateTimeConstants.UnixEpoch && TradePrice == 0m;

        public virtual void Reset()
        {

            TradeTime = DateTimeConstants.UnixEpoch;
            TradePrice = 0m;
            UpdatedFlags = LastTradeUpdated.None;
        }

        public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
            if (!updatedOnly || IsTradeTimeDateUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.LastTradeTimeHourOffset,
                    TradeTime.GetHoursFromUnixEpoch());
            }
            if (!updatedOnly || IsTradeTimeSubHourUpdated)
            {
                byte flag = TradeTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
                yield return new PQFieldUpdate(PQFieldKeys.LastTradeTimeSubHourOffset, value, flag);
            }
            if (!updatedOnly || IsTradePriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.LastTradePriceOffset, TradePrice,
                    quotePublicationPrecisionSetting?.PriceScalingPrecision ?? 1);
            }
        }

        public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
        {// assume the recentlytraded has already forwarded this through to the correct lasttrade
            if (pqFieldUpdate.Id >= PQFieldKeys.LastTradeTimeHourOffset &&
                pqFieldUpdate.Id < (PQFieldKeys.LastTradeTimeHourOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades))
            {
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref tradeTime, pqFieldUpdate.Value);
                IsTradeTimeDateUpdated = true;
                return 0;
            }
            if (pqFieldUpdate.Id >= PQFieldKeys.LastTradeTimeSubHourOffset &&
                pqFieldUpdate.Id < (PQFieldKeys.LastTradeTimeSubHourOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades))
            {
                PQFieldConverters.UpdateSubHourComponent(ref tradeTime, 
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                IsTradeTimeSubHourUpdated = true;
                return 0;
            }
            if (pqFieldUpdate.Id >= PQFieldKeys.LastTradePriceOffset &&
                pqFieldUpdate.Id < (PQFieldKeys.LastTradePriceOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades))
            {
                TradePrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            }
            return -1;
        }

        public virtual void CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (source == null)
            {
                Reset();
                return;
            }
            PQLastTrade pqlt = source as PQLastTrade;
            if (pqlt == null && source is ILastTrade lastTrade)
            {
                TradePrice = lastTrade.TradePrice;
                TradeTime = lastTrade.TradeTime;
            }
            if (pqlt != null)
            {
                if (pqlt.IsTradeTimeDateUpdated || pqlt.IsTradeTimeSubHourUpdated)
                {
                    TradeTime = pqlt.TradeTime;
                }
                if (pqlt.IsTradePriceUpdated)
                {
                    TradePrice = pqlt.TradePrice;
                }
                UpdatedFlags = pqlt.UpdatedFlags;
            }
        }

        public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerQuoteInfo referenceInstance)
        {
        }

        public virtual void EnsureRelatedItemsAreConfigured(IPQLastTrade referenceInstance)
        {
        }

        IPQLastTrade IPQLastTrade.Clone()
        {
            return (IPQLastTrade)Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        ILastTrade ICloneable<ILastTrade>.Clone()
        {
            return Clone();
        }

        public virtual IMutableLastTrade Clone()
        {
            return new PQLastTrade(this);
        }

        public virtual bool AreEquivalent(ILastTrade other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var tradeDateSame = TradeTime.Equals(other.TradeTime);
            var tradePriceSame = TradePrice == other.TradePrice;

            var updatesSame = true;
            if (exactTypes)
            {
                var pqLastTrade = other as PQLastTrade;
                updatesSame = UpdatedFlags == pqLastTrade.UpdatedFlags;
            }

            return tradeDateSame && tradePriceSame && updatesSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((ILastTrade)obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TradeTime.GetHashCode() * 397) ^ TradePrice.GetHashCode();
            }
        }
        public override string ToString()
        {
            return $"PQLastTrade {{ {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: {TradeTime:O} }}";
        }
    }
}
