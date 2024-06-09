// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQLastTrade : IMutableLastTrade, IPQSupportsFieldUpdates<ILastTrade>
{
    bool IsTradeTimeSubHourUpdated { get; set; }
    bool IsTradeTimeDateUpdated    { get; set; }
    bool IsTradePriceUpdated       { get; set; }

    new IPQLastTrade Clone();
}

public class PQLastTrade : ReusableObject<ILastTrade>, IPQLastTrade
{
    private decimal  tradePrice;
    private DateTime tradeTime = DateTimeConstants.UnixEpoch;

    protected LastTradeUpdated UpdatedFlags;

    public PQLastTrade() { }

    public PQLastTrade(decimal tradePrice = 0m, DateTime? tradeTime = null)
    {
        TradeTime  = tradeTime ?? DateTimeConstants.UnixEpoch;
        TradePrice = tradePrice;
    }

    public PQLastTrade(ILastTrade toClone)
    {
        TradePrice = toClone.TradePrice;
        TradeTime  = toClone.TradeTime;
        if (toClone is IPQLastTrade pqLastTrade)
        {
            IsTradeTimeDateUpdated    = pqLastTrade.IsTradeTimeDateUpdated;
            IsTradeTimeSubHourUpdated = pqLastTrade.IsTradeTimeSubHourUpdated;
            IsTradePriceUpdated       = pqLastTrade.IsTradePriceUpdated;
        }
    }

    protected string PQLastTradeToStringMembers => $"{nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: {TradeTime:O}";

    public virtual LastTradeType   LastTradeType           => LastTradeType.Price;
    public virtual LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;

    public DateTime TradeTime
    {
        get => tradeTime;
        set
        {
            if (value == tradeTime) return;
            IsTradeTimeDateUpdated    |= tradeTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsTradeTimeSubHourUpdated |= tradeTime.GetSubHourComponent() != value.GetSubHourComponent();
            tradeTime                 =  value;
        }
    }

    public bool IsTradeTimeDateUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeTimeDateUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeTimeDateUpdated;

            else if (IsTradeTimeDateUpdated) UpdatedFlags ^= LastTradeUpdated.TradeTimeDateUpdated;
        }
    }

    public bool IsTradeTimeSubHourUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeTimeSubHourUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeTimeSubHourUpdated;

            else if (IsTradeTimeSubHourUpdated) UpdatedFlags ^= LastTradeUpdated.TradeTimeSubHourUpdated;
        }
    }

    public decimal TradePrice
    {
        get => tradePrice;
        set
        {
            if (value == tradePrice) return;
            IsTradePriceUpdated = true;
            tradePrice          = value;
        }
    }

    public bool IsTradePriceUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradePriceUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradePriceUpdated;

            else if (IsTradePriceUpdated) UpdatedFlags ^= LastTradeUpdated.TradePriceUpdated;
        }
    }

    public virtual bool HasUpdates
    {
        get => UpdatedFlags != LastTradeUpdated.None;
        set => IsTradePriceUpdated = IsTradeTimeDateUpdated = IsTradeTimeSubHourUpdated = value;
    }

    public virtual bool IsEmpty
    {
        get => TradeTime == DateTimeConstants.UnixEpoch && TradePrice == 0m;
        set
        {
            if (!value) return;
            TradeTime  = DateTimeConstants.UnixEpoch;
            TradePrice = 0m;
        }
    }

    public override void StateReset()
    {
        TradeTime    = DateTimeConstants.UnixEpoch;
        TradePrice   = 0m;
        UpdatedFlags = LastTradeUpdated.None;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, StorageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsTradeTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LastTradeTimeHourOffset,
                                           TradeTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsTradeTimeSubHourUpdated)
        {
            var flag = TradeTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
            yield return new PQFieldUpdate(PQFieldKeys.LastTradeTimeSubHourOffset, value, flag);
        }

        if (!updatedOnly || IsTradePriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LastTradePriceOffset, TradePrice,
                                           quotePublicationPrecisionSetting?.PriceScalingPrecision ?? 1);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        if (pqFieldUpdate.Id >= PQFieldKeys.LastTradeTimeHourOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LastTradeTimeHourOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
        {
            PQFieldConverters.UpdateHoursFromUnixEpoch(ref tradeTime, pqFieldUpdate.Value);
            IsTradeTimeDateUpdated = true;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LastTradeTimeSubHourOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LastTradeTimeSubHourOffset +
            PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
        {
            PQFieldConverters.UpdateSubHourComponent(ref tradeTime,
                                                     pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
            IsTradeTimeSubHourUpdated = true;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LastTradePriceOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LastTradePriceOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
        {
            TradePrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
            return 0;
        }

        return -1;
    }

    public override ILastTrade CopyFrom(ILastTrade? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null)
        {
            StateReset();
            return this;
        }

        if (source is PQLastTrade pqlt)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqlt.IsTradeTimeDateUpdated || pqlt.IsTradeTimeSubHourUpdated || isFullReplace) TradeTime = pqlt.TradeTime;

            if (pqlt.IsTradePriceUpdated || isFullReplace) TradePrice = pqlt.TradePrice;
            if (isFullReplace) UpdatedFlags                           = pqlt.UpdatedFlags;
        }
        else
        {
            TradePrice = source.TradePrice;
            TradeTime  = source.TradeTime;
        }

        return this;
    }

    IPQLastTrade IPQLastTrade.Clone() => (IPQLastTrade)Clone();

    object ICloneable.Clone() => Clone();

    ILastTrade ICloneable<ILastTrade>.Clone() => Clone();

    public override IMutableLastTrade Clone() => (IMutableLastTrade?)Recycler?.Borrow<PQLastTrade>().CopyFrom(this) ?? new PQLastTrade(this);

    public virtual bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var tradeDateSame  = TradeTime.Equals(other.TradeTime);
        var tradePriceSame = TradePrice == other.TradePrice;

        var updatesSame = true;
        if (exactTypes)
        {
            var pqLastTrade = (PQLastTrade)other;
            updatesSame = UpdatedFlags == pqLastTrade.UpdatedFlags;
        }

        return tradeDateSame && tradePriceSame && updatesSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILastTrade?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (TradeTime.GetHashCode() * 397) ^ TradePrice.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQLastTradeToStringMembers})";
}
