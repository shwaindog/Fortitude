// Licensed under the MIT license.
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
[JsonDerivedType(typeof(PQLastExternalCounterPartyTrade))]
public interface IPQLastTrade : IReusableObject<IPQLastTrade>, IMutableLastTrade, IPQSupportsNumberPrecisionFieldUpdates<ILastTrade>,
    ITrackableReset<IPQLastTrade>
{
    [JsonIgnore] bool IsTradeIdUpdated                    { get; set; }
    [JsonIgnore] bool IsBatchIdUpdated                    { get; set; }
    [JsonIgnore] bool IsFirstNotifiedDateUpdated          { get; set; }
    [JsonIgnore] bool IsFirstNotifiedSub2MinTimeUpdated   { get; set; }
    [JsonIgnore] bool IsAdapterReceivedDateUpdated        { get; set; }
    [JsonIgnore] bool IsAdapterReceivedSub2MinTimeUpdated { get; set; }
    [JsonIgnore] bool IsUpdatedDateUpdated                { get; set; }
    [JsonIgnore] bool IsUpdateSub2MinTimeUpdated          { get; set; }
    [JsonIgnore] bool IsTradeTypeFlagsUpdated             { get; set; }
    [JsonIgnore] bool IsTradeLifeCycleStatusUpdated       { get; set; }
    [JsonIgnore] bool IsTradeTimeSub2MinUpdated           { get; set; }
    [JsonIgnore] bool IsTradeTimeDateUpdated              { get; set; }
    [JsonIgnore] bool IsTradePriceUpdated                 { get; set; }

    new IPQLastTrade Clone();

    new IPQLastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new IPQLastTrade ResetWithTracking();
}

public class PQLastTrade : ReusableObject<IPQLastTrade>, IPQLastTrade
{
    protected uint SequenceId = uint.MaxValue;

    private decimal  tradePrice;
    private DateTime tradeTime = DateTime.MinValue;

    LastTradedTypeFlags      tradeTypeFlags;
    LastTradedLifeCycleFlags tradeLifeCycleStatus;

    uint     tradeId;
    uint     batchId;
    DateTime firstNotifiedTime   = DateTime.MinValue;
    DateTime adapterReceivedTime = DateTime.MinValue;
    DateTime updateTime          = DateTime.MinValue;

    protected LastTradeUpdated UpdatedFlags;

    public PQLastTrade()
    {
        if (GetType() == typeof(PQLastTrade)) SequenceId = 0;
    }

    public PQLastTrade
    (uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeTime = null
      , LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None, LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null)
    {
        TradeId        = tradeId;
        BatchId        = batchId;
        TradeTime      = tradeTime ?? DateTime.MinValue;
        TradePrice     = tradePrice;
        TradeTypeFlags = tradeTypeFlags;
        UpdateTime     = updateTime ?? default;

        TradeLifeCycleStatus = tradeLifecycleStatus;
        FirstNotifiedTime    = firstNotifiedTime ?? default;
        AdapterReceivedTime  = adapterReceivedTime ?? default;

        if (GetType() == typeof(PQLastTrade)) SequenceId = 0;
    }

    public PQLastTrade(ILastTrade toClone)
    {
        TradeId    = toClone.TradeId;
        BatchId    = toClone.BatchId;
        TradePrice = toClone.TradePrice;
        TradeTime  = toClone.TradeTime;
        UpdateTime = toClone.UpdateTime;

        TradeTypeFlags       = toClone.TradeTypeFlags;
        TradeLifeCycleStatus = toClone.TradeLifeCycleStatus;
        AdapterReceivedTime  = toClone.AdapterReceivedTime;
        FirstNotifiedTime    = toClone.FirstNotifiedTime;

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLastTrade)) SequenceId = 0;
    }

    [JsonIgnore] public virtual LastTradeType LastTradeType => LastTradeType.Price;

    [JsonIgnore] public virtual LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint TradeId
    {
        get => tradeId;
        set
        {
            IsTradeIdUpdated |= tradeId != value || SequenceId == 0;
            tradeId          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId
    {
        get => batchId;
        set
        {
            IsBatchIdUpdated |= batchId != value || SequenceId == 0;
            batchId          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LastTradedTypeFlags TradeTypeFlags
    {
        get => tradeTypeFlags;
        set
        {
            IsTradeTypeFlagsUpdated |= tradeTypeFlags != value || SequenceId == 0;
            tradeTypeFlags          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LastTradedLifeCycleFlags TradeLifeCycleStatus
    {
        get => tradeLifeCycleStatus;
        set
        {
            IsTradeLifeCycleStatusUpdated |= tradeLifeCycleStatus != value || SequenceId == 0;
            tradeLifeCycleStatus          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime TradeTime
    {
        get => tradeTime;
        set
        {
            IsTradeTimeDateUpdated |= tradeTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsTradeTimeSub2MinUpdated |= tradeTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            tradeTime = value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal TradePrice
    {
        get => tradePrice;
        set
        {
            IsTradePriceUpdated |= tradePrice != value || SequenceId == 0;
            tradePrice          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime FirstNotifiedTime
    {
        get => firstNotifiedTime;
        set
        {
            IsFirstNotifiedDateUpdated |= firstNotifiedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                          SequenceId == 0;
            IsFirstNotifiedSub2MinTimeUpdated |= firstNotifiedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            firstNotifiedTime                 =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterReceivedTime
    {
        get => adapterReceivedTime;
        set
        {
            IsAdapterReceivedDateUpdated |= adapterReceivedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                            SequenceId == 0;
            IsAdapterReceivedSub2MinTimeUpdated
                |= adapterReceivedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            adapterReceivedTime = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime UpdateTime
    {
        get => updateTime;
        set
        {
            IsUpdatedDateUpdated |= updateTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsUpdateSub2MinTimeUpdated |= updateTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            updateTime = value;
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
    public bool IsTradeIdUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeIdUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeIdUpdated;

            else if (IsTradeIdUpdated) UpdatedFlags ^= LastTradeUpdated.TradeIdUpdated;
        }
    }

    [JsonIgnore]
    public bool IsBatchIdUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeBatchIdUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeBatchIdUpdated;

            else if (IsBatchIdUpdated) UpdatedFlags ^= LastTradeUpdated.TradeBatchIdUpdated;
        }
    }

    [JsonIgnore]
    public bool IsFirstNotifiedDateUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeFirstNotifiedDateUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeFirstNotifiedDateUpdated;

            else if (IsFirstNotifiedDateUpdated) UpdatedFlags ^= LastTradeUpdated.TradeFirstNotifiedDateUpdated;
        }
    }

    [JsonIgnore]
    public bool IsFirstNotifiedSub2MinTimeUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeFirstNotifiedSub2MinTimeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeFirstNotifiedSub2MinTimeUpdated;

            else if (IsFirstNotifiedSub2MinTimeUpdated) UpdatedFlags ^= LastTradeUpdated.TradeFirstNotifiedSub2MinTimeUpdated;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedDateUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeAdapterReceivedDateUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeAdapterReceivedDateUpdated;

            else if (IsAdapterReceivedDateUpdated) UpdatedFlags ^= LastTradeUpdated.TradeAdapterReceivedDateUpdated;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedSub2MinTimeUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeAdapterReceivedSub2MinTimeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeAdapterReceivedSub2MinTimeUpdated;

            else if (IsAdapterReceivedSub2MinTimeUpdated) UpdatedFlags ^= LastTradeUpdated.TradeAdapterReceivedSub2MinTimeUpdated;
        }
    }

    [JsonIgnore]
    public bool IsUpdatedDateUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeUpdateDateUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeUpdateDateUpdated;

            else if (IsUpdatedDateUpdated) UpdatedFlags ^= LastTradeUpdated.TradeUpdateDateUpdated;
        }
    }

    [JsonIgnore]
    public bool IsUpdateSub2MinTimeUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeUpdateSub2MinTimeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeUpdateSub2MinTimeUpdated;

            else if (IsUpdateSub2MinTimeUpdated) UpdatedFlags ^= LastTradeUpdated.TradeUpdateSub2MinTimeUpdated;
        }
    }

    [JsonIgnore]
    public bool IsTradeTypeFlagsUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeTypeFlagsUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeTypeFlagsUpdated;

            else if (IsTradeTypeFlagsUpdated) UpdatedFlags ^= LastTradeUpdated.TradeTypeFlagsUpdated;
        }
    }

    [JsonIgnore]
    public bool IsTradeLifeCycleStatusUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TradeLifeCycleStatusUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TradeLifeCycleStatusUpdated;

            else if (IsTradeLifeCycleStatusUpdated) UpdatedFlags ^= LastTradeUpdated.TradeLifeCycleStatusUpdated;
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

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != LastTradeUpdated.None;
        set => UpdatedFlags = value ? LastTradeUpdated.AllFlagsMask : LastTradeUpdated.None;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => TradeTime == DateTime.MinValue && TradePrice == 0m;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade ITrackableReset<IPQLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade IPQLastTrade.ResetWithTracking() => ResetWithTracking();

    public virtual PQLastTrade ResetWithTracking()
    {
        TradeId    = 0;
        BatchId    = 0;
        TradeTime  = default;
        UpdateTime = default;
        TradePrice = 0m;

        TradeTypeFlags       = LastTradedTypeFlags.None;
        TradeLifeCycleStatus = LastTradedLifeCycleFlags.None;
        FirstNotifiedTime    = default;
        AdapterReceivedTime  = default;

        SequenceId = 0;
        return this;
    }

    public uint UpdateSequenceId => SequenceId;

    public virtual void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates && !IsEmpty) SequenceId++;
        HasUpdates = false;
    }

    public override void StateReset()
    {
        ResetWithTracking();
        UpdatedFlags = LastTradeUpdated.None;
        base.StateReset();
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsTradeIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeId, TradeId);
        if (!updatedOnly || IsBatchIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedBatchId, BatchId);
        if (!updatedOnly || IsTradePriceUpdated)
            yield return new PQFieldUpdate
                (PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAtPrice, TradePrice
               , quotePublicationPrecisionSetting?.PriceScalingPrecision ?? (PQFieldFlags)1);
        if (!updatedOnly || IsTradeTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeTimeDate
                                         , TradeTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsTradeTimeSub2MinUpdated)
        {
            var extended = TradeTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTradeSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsTradeTypeFlagsUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTypeFlags, (uint)TradeTypeFlags);
        if (!updatedOnly || IsBatchIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedLifeCycleStatus
                                         , (uint)TradeLifeCycleStatus);
        if (!updatedOnly || IsFirstNotifiedDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedDate
                                         , FirstNotifiedTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsFirstNotifiedSub2MinTimeUpdated)
        {
            var extended = FirstNotifiedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedFirstNotifiedSub2MinTime, value
                                         , extended);
        }
        if (!updatedOnly || IsAdapterReceivedDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedDate
                                         , AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsAdapterReceivedSub2MinTimeUpdated)
        {
            var extended = AdapterReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedAdapterReceivedSub2MinTime, value
                                         , extended);
        }
        if (!updatedOnly || IsUpdatedDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateDate
                                         , UpdateTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsUpdateSub2MinTimeUpdated)
        {
            var extended = UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedUpdateSub2MinTime, value, extended);
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        switch (pqFieldUpdate.TradingSubId)
        {
            case PQTradingSubFieldKeys.LastTradedTradeId:
                IsTradeIdUpdated = true;
                TradeId          = pqFieldUpdate.Payload;
                return 0;
            case PQTradingSubFieldKeys.LastTradedBatchId:
                IsBatchIdUpdated = true;
                BatchId          = pqFieldUpdate.Payload;
                return 0;
            case PQTradingSubFieldKeys.LastTradedTradeTimeDate:
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref tradeTime, pqFieldUpdate.Payload);
                IsTradeTimeDateUpdated = true;
                if (tradeTime == DateTime.UnixEpoch) tradeTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedTradeSub2MinTime:
                PQFieldConverters.UpdateSub2MinComponent(ref tradeTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                IsTradeTimeSub2MinUpdated = true;
                if (tradeTime == DateTime.UnixEpoch) tradeTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedAtPrice:
                TradePrice = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQTradingSubFieldKeys.LastTradedTypeFlags:
                IsTradeTypeFlagsUpdated = true;
                TradeTypeFlags          = (LastTradedTypeFlags)pqFieldUpdate.Payload;
                return 0;
            case PQTradingSubFieldKeys.LastTradedLifeCycleStatus:
                IsTradeLifeCycleStatusUpdated = true;
                TradeLifeCycleStatus          = (LastTradedLifeCycleFlags)pqFieldUpdate.Payload;
                return 0;
            case PQTradingSubFieldKeys.LastTradedFirstNotifiedDate:
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref firstNotifiedTime, pqFieldUpdate.Payload);
                IsFirstNotifiedDateUpdated = true;
                if (firstNotifiedTime == DateTime.UnixEpoch) firstNotifiedTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedFirstNotifiedSub2MinTime:
                PQFieldConverters.UpdateSub2MinComponent(ref firstNotifiedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                IsFirstNotifiedSub2MinTimeUpdated = true;
                if (firstNotifiedTime == DateTime.UnixEpoch) firstNotifiedTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedAdapterReceivedDate:
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref adapterReceivedTime, pqFieldUpdate.Payload);
                IsAdapterReceivedDateUpdated = true;
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedAdapterReceivedSub2MinTime:
                PQFieldConverters.UpdateSub2MinComponent(ref adapterReceivedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                IsAdapterReceivedSub2MinTimeUpdated = true;
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedUpdateDate:
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, pqFieldUpdate.Payload);
                IsUpdatedDateUpdated = true;
                if (updateTime == DateTime.UnixEpoch) updateTime = default;
                return 0;
            case PQTradingSubFieldKeys.LastTradedUpdateSub2MinTime:
                PQFieldConverters.UpdateSub2MinComponent(ref updateTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                IsUpdateSub2MinTimeUpdated = true;
                if (updateTime == DateTime.UnixEpoch) updateTime = default;
                return 0;
        }

        return -1;
    }

    IPQLastTrade IPQLastTrade.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    ILastTrade ICloneable<ILastTrade>.Clone() => Clone();

    IMutableLastTrade IMutableLastTrade.Clone() => Clone();

    IPQLastTrade ICloneable<IPQLastTrade>.Clone() => Clone();

    IMutableLastTrade ICloneable<IMutableLastTrade>.Clone() => Clone();

    public override PQLastTrade Clone() => Recycler?.Borrow<PQLastTrade>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLastTrade(this);

    IReusableObject<IMutableLastTrade> ITransferState<IReusableObject<IMutableLastTrade>>.CopyFrom
        (IReusableObject<IMutableLastTrade> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ILastTrade)source, copyMergeFlags);

    IMutableLastTrade ITransferState<IMutableLastTrade>.CopyFrom(IMutableLastTrade source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IReusableObject<ILastTrade> ITransferState<IReusableObject<ILastTrade>>.CopyFrom
        (IReusableObject<ILastTrade> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ILastTrade)source, copyMergeFlags);

    public override IPQLastTrade CopyFrom(IPQLastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => 
        CopyFrom(source, copyMergeFlags);

    ILastTrade ITransferState<ILastTrade>.CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IPQLastTrade IPQLastTrade.CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public PQLastTrade CopyFrom(LastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => 
        CopyFrom((ILastTrade)source, copyMergeFlags);

    public virtual PQLastTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags)
    {
        if (source is PQLastTrade pqlt)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqlt.IsTradeIdUpdated || isFullReplace)
            {
                IsTradeIdUpdated = true;

                TradeId = pqlt.TradeId;
            }
            if (pqlt.IsBatchIdUpdated || isFullReplace)
            {
                IsBatchIdUpdated = true;

                BatchId = pqlt.BatchId;
            }
            if (pqlt.IsTradePriceUpdated || isFullReplace)
            {
                IsTradePriceUpdated = true;

                TradePrice = pqlt.TradePrice;
            }
            if (pqlt.IsTradeTimeDateUpdated || isFullReplace)
            {
                IsTradeTimeDateUpdated = true;

                TradeTime = pqlt.TradeTime;
            }
            if (pqlt.IsTradeTimeSub2MinUpdated || isFullReplace)
            {
                IsTradeTimeSub2MinUpdated = true;

                TradeTime = pqlt.TradeTime;
            }
            if (pqlt.IsTradeTypeFlagsUpdated || isFullReplace)
            {
                IsTradeTypeFlagsUpdated = true;

                TradeTypeFlags = pqlt.TradeTypeFlags;
            }
            if (pqlt.IsTradeLifeCycleStatusUpdated || isFullReplace)
            {
                IsTradeLifeCycleStatusUpdated = true;

                TradeLifeCycleStatus = pqlt.TradeLifeCycleStatus;
            }
            if (pqlt.IsFirstNotifiedDateUpdated || isFullReplace)
            {
                IsFirstNotifiedDateUpdated = true;

                FirstNotifiedTime = pqlt.FirstNotifiedTime;
            }
            if (pqlt.IsFirstNotifiedSub2MinTimeUpdated || isFullReplace)
            {
                IsFirstNotifiedSub2MinTimeUpdated = true;

                FirstNotifiedTime = pqlt.FirstNotifiedTime;
            }
            if (pqlt.IsAdapterReceivedDateUpdated || isFullReplace)
            {
                IsAdapterReceivedDateUpdated = true;

                AdapterReceivedTime = pqlt.AdapterReceivedTime;
            }
            if (pqlt.IsAdapterReceivedSub2MinTimeUpdated || isFullReplace)
            {
                IsAdapterReceivedSub2MinTimeUpdated = true;

                AdapterReceivedTime = pqlt.AdapterReceivedTime;
            }
            if (pqlt.IsUpdatedDateUpdated || isFullReplace)
            {
                IsUpdatedDateUpdated = true;

                UpdateTime = pqlt.UpdateTime;
            }
            if (pqlt.IsUpdateSub2MinTimeUpdated || isFullReplace)
            {
                IsUpdateSub2MinTimeUpdated = true;

                UpdateTime = pqlt.UpdateTime;
            }

            if (isFullReplace) SetFlagsSame(pqlt);
        }
        else
        {
            TradeId    = source.TradeId;
            BatchId    = source.BatchId;
            TradeTime  = source.TradeTime;
            TradePrice = source.TradePrice;
            UpdateTime = source.UpdateTime;

            TradeTypeFlags       = source.TradeTypeFlags;
            FirstNotifiedTime    = source.FirstNotifiedTime;
            TradeLifeCycleStatus = source.TradeLifeCycleStatus;
            AdapterReceivedTime  = source.AdapterReceivedTime;
        }

        if (copyMergeFlags.HasUpdateFlagsNone()) UpdatedFlags = LastTradeUpdated.None;
        if (copyMergeFlags.HasUpdateFlagsAll()) UpdatedFlags  = LastTradeUpdated.AllFlagsMask;

        return this;
    }

    public virtual bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;


        var tradeIdSame    = TradeId == other.TradeId;
        var batchIdSame    = BatchId == other.BatchId;
        var tradeDateSame  = TradeTime == other.TradeTime;
        var tradePriceSame = TradePrice == other.TradePrice;
        var updateTimeSame = UpdateTime == other.UpdateTime;

        var tradeTypeSame    = TradeTypeFlags == other.TradeTypeFlags;
        var firstNotTimeSame = FirstNotifiedTime == other.FirstNotifiedTime;
        var lifecycleSame    = TradeLifeCycleStatus == other.TradeLifeCycleStatus;
        var adapterRecvSame  = AdapterReceivedTime == other.AdapterReceivedTime;

        var updatesSame = true;
        if (exactTypes)
        {
            var pqLastTrade = (PQLastTrade)other;
            updatesSame = UpdatedFlags == pqLastTrade.UpdatedFlags;
        }

        var allAreSame = tradeIdSame && batchIdSame && tradeDateSame && tradePriceSame && updateTimeSame
                      && tradeTypeSame && firstNotTimeSame && lifecycleSame && adapterRecvSame && updatesSame;
        return allAreSame;
    }

    protected void SetFlagsSame(ILastTrade toCopyFlags)
    {
        if (toCopyFlags is PQLastTrade pqToClone)
        {
            UpdatedFlags = pqToClone.UpdatedFlags;
        }
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILastTrade?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)TradeId;
            hashCode = ((int)BatchId * 397) ^ hashCode;
            hashCode = (TradePrice.GetHashCode() * 397) ^ hashCode;
            hashCode = (TradeTime.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)TradeLifeCycleStatus * 397) ^ hashCode;
            hashCode = ((int)TradeTypeFlags * 397) ^ hashCode;
            hashCode = (UpdateTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (FirstNotifiedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (AdapterReceivedTime.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string PQLastTradeToStringMembers =>
        $"{nameof(TradeId)}: {TradeId}, {nameof(BatchId)}: {BatchId}, {nameof(TradePrice)}: {TradePrice:N5}, {nameof(TradeTime)}: {TradeTime:O}, " +
        $"{nameof(TradeTypeFlags)}: {TradeTypeFlags}, {nameof(TradeLifeCycleStatus)}: {TradeLifeCycleStatus}, {nameof(FirstNotifiedTime)}: {FirstNotifiedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(UpdateTime)}: {UpdateTime:O}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{GetType().Name}({PQLastTradeToStringMembers}, {UpdatedFlagsToString})";
}
