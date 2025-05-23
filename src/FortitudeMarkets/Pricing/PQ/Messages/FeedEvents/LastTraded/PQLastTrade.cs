﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[JsonDerivedType(typeof(PQLastTrade))]
[JsonDerivedType(typeof(PQLastPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastTraderPaidGivenTrade))]
public interface IPQLastTrade : IMutableLastTrade, IPQSupportsNumberPrecisionFieldUpdates<ILastTrade>, ITrackableReset<IPQLastTrade>
{
    [JsonIgnore] bool IsTradeTimeSub2MinUpdated { get; set; }
    [JsonIgnore] bool IsTradeTimeDateUpdated    { get; set; }
    [JsonIgnore] bool IsTradePriceUpdated       { get; set; }

    new IPQLastTrade Clone();

    new IPQLastTrade ResetWithTracking();
}

public class PQLastTrade : ReusableObject<ILastTrade>, IPQLastTrade
{
    protected uint     NumUpdatesSinceEmpty = uint.MaxValue;
    private   decimal  tradePrice;
    private   DateTime tradeTime = DateTime.MinValue;

    protected LastTradeUpdated UpdatedFlags;


    public PQLastTrade()
    {
        if (GetType() == typeof(PQLastTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTrade(decimal tradePrice = 0m, DateTime? tradeTime = null)
    {
        TradeTime  = tradeTime ?? DateTime.MinValue;
        TradePrice = tradePrice;

        if (GetType() == typeof(PQLastTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTrade(ILastTrade toClone)
    {
        TradePrice = toClone.TradePrice;
        TradeTime  = toClone.TradeTime;
        if (toClone is IPQLastTrade pqLastTrade)
        {
            IsTradeTimeDateUpdated    = pqLastTrade.IsTradeTimeDateUpdated;
            IsTradeTimeSub2MinUpdated = pqLastTrade.IsTradeTimeSub2MinUpdated;
            IsTradePriceUpdated       = pqLastTrade.IsTradePriceUpdated;
        }

        if (GetType() == typeof(PQLastTrade)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQLastTradeToStringMembers =>
        $"{nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: {TradeTime:O} {nameof(UpdatedFlags)}: {UpdatedFlags}";

    [JsonIgnore] public virtual LastTradeType LastTradeType => LastTradeType.Price;

    [JsonIgnore] public virtual LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime TradeTime
    {
        get => tradeTime;
        set
        {
            IsTradeTimeDateUpdated    |= tradeTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsTradeTimeSub2MinUpdated |= tradeTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;
            tradeTime                 =  value;
        }
    }

    [JsonIgnore]
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

    [JsonIgnore]
    public bool IsTradeTimeSub2MinUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeTimeSub2MinUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeTimeSub2MinUpdated;

            else if (IsTradeTimeSub2MinUpdated) UpdatedFlags ^= LastTradeUpdated.TradeTimeSub2MinUpdated;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal TradePrice
    {
        get => tradePrice;
        set
        {
            IsTradePriceUpdated |= tradePrice != value || NumUpdatesSinceEmpty == 0;
            tradePrice          =  value;
        }
    }

    [JsonIgnore]
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

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != LastTradeUpdated.None;
        set => IsTradePriceUpdated = IsTradeTimeDateUpdated = IsTradeTimeSub2MinUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => TradeTime == DateTime.MinValue && TradePrice == 0m;
        set
        {
            if (!value) return;
            TradeTime  = DateTime.MinValue;
            TradePrice = 0m;

            NumUpdatesSinceEmpty = 0;
        }
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade ITrackableReset<IPQLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade IPQLastTrade.                 ResetWithTracking() => ResetWithTracking();

    public virtual PQLastTrade ResetWithTracking()
    {
        TradeTime  = default;
        TradePrice = 0m;

        return this;
    }

    public uint UpdateCount => NumUpdatesSinceEmpty;

    public virtual void UpdateComplete()
    {
        if (HasUpdates && !IsEmpty) NumUpdatesSinceEmpty++;
        HasUpdates = false;
    }

    public override void StateReset()
    {
        TradeTime    = DateTime.MinValue;
        TradePrice   = 0m;
        UpdatedFlags = LastTradeUpdated.None;

        NumUpdatesSinceEmpty = 0;

        base.StateReset();
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsTradeTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate, TradeTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsTradeTimeSub2MinUpdated)
        {
            var extended = TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime,  value, extended);
        }

        if (!updatedOnly || IsTradePriceUpdated)
            yield return new PQFieldUpdate
                (PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, TradePrice, quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        if (pqFieldUpdate.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeTimeDate)
        {
            PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref tradeTime, pqFieldUpdate.Payload);
            IsTradeTimeDateUpdated = true;
            if (tradeTime == DateTime.UnixEpoch) tradeTime = default;
            return 0;
        }
        if (pqFieldUpdate.TradingSubId == PQTradingSubFieldKeys.LastTradedTradeSub2MinTime)
        {
            PQFieldConverters.UpdateSub2MinComponent(ref tradeTime,
                                                     pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
            IsTradeTimeSub2MinUpdated = true;
            if (tradeTime == DateTime.UnixEpoch) tradeTime = default;
            return 0;
        }
        if (pqFieldUpdate.TradingSubId == PQTradingSubFieldKeys.LastTradedAtPrice)
        {
            TradePrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
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

            if (pqlt.IsTradeTimeDateUpdated || pqlt.IsTradeTimeSub2MinUpdated || isFullReplace) TradeTime = pqlt.TradeTime;

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

        var allAreSame = tradeDateSame && tradePriceSame && updatesSame;
        return allAreSame;
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
