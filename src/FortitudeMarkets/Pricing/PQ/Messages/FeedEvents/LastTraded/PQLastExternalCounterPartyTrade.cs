// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQLastExternalCounterPartyTrade : IPQLastPaidGivenTrade, IMutableLastExternalCounterPartyTrade,
    IPQSupportsStringUpdates<ILastTrade>, ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQLastExternalCounterPartyTrade>, ICloneable<IPQLastExternalCounterPartyTrade>
{
    bool IsExternalCounterPartyIdUpdated   { get; set; }
    int  ExternalCounterPartyNameId        { get; set; }
    bool IsExternalCounterPartyNameUpdated { get; set; }
    bool IsExternalTraderIdUpdated         { get; set; }
    int  ExternalTraderNameId              { get; set; }
    bool IsExternalTraderNameUpdated       { get; set; }

    new IPQNameIdLookupGenerator         NameIdLookup { get; set; }

    new IPQLastExternalCounterPartyTrade Clone();

    new IPQLastExternalCounterPartyTrade ResetWithTracking();
}

public class PQLastExternalCounterPartyTrade : PQLastPaidGivenTrade, IPQLastExternalCounterPartyTrade
{
    private IPQNameIdLookupGenerator nameIdLookup;

    private int externalTraderId;
    private int externalCounterPartyId;

    private int externalTraderNameId;
    private int externalCounterPartyNameId;

    public PQLastExternalCounterPartyTrade() 
    {
        nameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

        if (GetType() == typeof(PQLastExternalCounterPartyTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastExternalCounterPartyTrade(IPQNameIdLookupGenerator traderIdToNameLookup) 
    {
        nameIdLookup = traderIdToNameLookup;

        if (GetType() == typeof(PQLastExternalCounterPartyTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastExternalCounterPartyTrade
    (IPQNameIdLookupGenerator traderIdToNameLookup, uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeDateTime = null
      , decimal tradeVolume = 0m,
        int counerPartyId = 0, string? counterPartyName = null, int traderId = 0, string? traderName = null, uint orderId = 0
      , bool wasPaid = false, bool wasGiven = false, LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None
      , LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null)
        : base(tradeId, batchId, tradePrice, tradeDateTime, tradeVolume, orderId, wasPaid, wasGiven
             , tradeTypeFlags, tradeLifecycleStatus, firstNotifiedTime, adapterReceivedTime, updateTime)
    {
        nameIdLookup             = traderIdToNameLookup;
        ExternalCounterPartyId   = counerPartyId;
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderId         = traderId;
        ExternalTraderName       = traderName;

        if (GetType() == typeof(PQLastExternalCounterPartyTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastExternalCounterPartyTrade(ILastTrade toClone, IPQNameIdLookupGenerator traderIdToNameLookup) : base(toClone)
    {
        nameIdLookup = traderIdToNameLookup;
        if (toClone is ILastExternalCounterPartyTrade lastExtCpTrade)
        {
            ExternalCounterPartyId   = lastExtCpTrade.ExternalCounterPartyId;
            ExternalCounterPartyName = lastExtCpTrade.ExternalCounterPartyName;
            ExternalTraderId         = lastExtCpTrade.ExternalTraderId;
            ExternalTraderName       = lastExtCpTrade.ExternalTraderName;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLastExternalCounterPartyTrade)) NumUpdatesSinceEmpty = 0;
    }


    [JsonIgnore] public override LastTradeType   LastTradeType           => LastTradeType.PriceLastTraderPaidOrGivenVolume;
    [JsonIgnore] public override LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.TraderName | base.SupportsLastTradedFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExternalCounterPartyId
    {
        get => externalCounterPartyId;
        set
        {
            IsExternalCounterPartyIdUpdated |= externalCounterPartyId != value || NumUpdatesSinceEmpty == 0;
            externalCounterPartyId          =  value;
        }
    }


    public int ExternalCounterPartyNameId
    {
        get => externalCounterPartyNameId;
        set
        {
            IsExternalCounterPartyNameUpdated |= externalCounterPartyNameId != value || NumUpdatesSinceEmpty == 0;

            externalCounterPartyNameId = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalCounterPartyName
    {
        get => externalCounterPartyNameId == 0 ? null : NameIdLookup[externalCounterPartyNameId];
        set
        {
            if (value == null)
            {
                externalCounterPartyNameId = 0;
                return;
            }

            var convertedSourceId = NameIdLookup[value];
            if (convertedSourceId > 0)
            {
                ExternalCounterPartyNameId = convertedSourceId;
                return;
            }

            ExternalCounterPartyNameId = NameIdLookup.GetOrAddId(value);
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExternalTraderId
    {
        get => externalTraderId;
        set
        {
            IsExternalTraderIdUpdated |= externalTraderId != value || NumUpdatesSinceEmpty == 0;
            externalTraderId          =  value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExternalTraderNameId
    {
        get => externalTraderNameId;
        set
        {
            IsExternalTraderNameUpdated |= externalTraderNameId != value || NumUpdatesSinceEmpty == 0;

            externalTraderNameId = value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalTraderName
    {
        get => ExternalTraderNameId == 0 ? null : NameIdLookup[externalTraderNameId];
        set
        {
            if (value == null)
            {
                ExternalTraderNameId = 0;
                return;
            }

            var convertedSourceId = NameIdLookup[value];
            if (convertedSourceId > 0)
            {
                ExternalTraderNameId = convertedSourceId;
                return;
            }

            ExternalTraderNameId = NameIdLookup.GetOrAddId(value);
        }
    }

    public bool IsExternalCounterPartyIdUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.ExternalCounterPartyIdUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.ExternalCounterPartyIdUpdated;

            else if (IsExternalCounterPartyIdUpdated) UpdatedFlags ^= LastTradeUpdated.ExternalCounterPartyIdUpdated;
        }
    }

    public bool IsExternalCounterPartyNameUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.ExternalCounterPartyNameUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.ExternalCounterPartyNameUpdated;

            else if (IsExternalCounterPartyNameUpdated) UpdatedFlags ^= LastTradeUpdated.ExternalCounterPartyNameUpdated;
        }
    }

    public bool IsExternalTraderIdUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.ExternalTraderIdUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.ExternalTraderIdUpdated;

            else if (IsExternalTraderIdUpdated) UpdatedFlags ^= LastTradeUpdated.ExternalTraderIdUpdated;
        }
    }

    [JsonIgnore]
    public bool IsExternalTraderNameUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.ExternalTraderNameUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.ExternalTraderNameUpdated;

            else if (IsExternalTraderNameUpdated) UpdatedFlags ^= LastTradeUpdated.ExternalTraderNameUpdated;
        }
    }

    [JsonIgnore] INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    [JsonIgnore]
    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;

            string? cacheTraderName                                   = null;
            if (externalTraderNameId > 0) cacheTraderName             = ExternalTraderName;
            string? cacheCounterPartyName                             = null;
            if (externalCounterPartyNameId > 0) cacheCounterPartyName = ExternalCounterPartyName;
            nameIdLookup = value;
            if (cacheCounterPartyName != null && externalCounterPartyNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(externalCounterPartyNameId, cacheCounterPartyName);
                }
                catch
                {
                    ExternalCounterPartyNameId = nameIdLookup.GetOrAddId(cacheCounterPartyName);
                }
            if (cacheTraderName != null && externalTraderNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(externalTraderNameId, cacheTraderName);
                }
                catch
                {
                    ExternalTraderNameId = nameIdLookup.GetOrAddId(cacheTraderName);
                }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty
         && externalCounterPartyId == 0
         && externalCounterPartyNameId == 0
         && externalTraderId == 0
         && externalTraderNameId ==0;
        set => base.IsEmpty = value;
    }

    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set
        {
            NameIdLookup.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        NameIdLookup.UpdateComplete();
        base.UpdateComplete();
    }

    IMutableLastTrade ITrackableReset<IMutableLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade ITrackableReset<IPQLastTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTrade IPQLastTrade.ResetWithTracking() => ResetWithTracking();

    IPQLastExternalCounterPartyTrade ITrackableReset<IPQLastExternalCounterPartyTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastExternalCounterPartyTrade IPQLastExternalCounterPartyTrade.ResetWithTracking() => ResetWithTracking();

    public override PQLastExternalCounterPartyTrade ResetWithTracking()
    {
        externalCounterPartyId     = 0;
        externalCounterPartyNameId = 0;
        externalTraderId           = 0;
        externalTraderNameId       = 0;
        base.ResetWithTracking();
        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return deltaUpdateField;

        if (!updatedOnly || IsExternalCounterPartyIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyId
                                         , (uint)externalCounterPartyId);
        if (!updatedOnly || IsExternalCounterPartyNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyNameId
                                         , (uint)externalCounterPartyNameId);
        if (!updatedOnly || IsExternalTraderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderId
                                         , (uint)externalTraderId);
        if (!updatedOnly || IsExternalTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderNameId
                                         , (uint)externalTraderNameId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.TradingSubId)
        {
            case PQTradingSubFieldKeys.LastTradedExternalCounterPartyId:
                IsExternalCounterPartyIdUpdated = true;

                var counterPartyId = (int)pqFieldUpdate.Payload;
                ExternalCounterPartyId = counterPartyId;
                return 0;
            case PQTradingSubFieldKeys.LastTradedExternalCounterPartyNameId:
                IsExternalCounterPartyNameUpdated = true;

                var counterPartyNameId = (int)pqFieldUpdate.Payload;
                ExternalCounterPartyNameId = counterPartyNameId;
                return 0;
            case PQTradingSubFieldKeys.LastTradedExternalTraderId:
                IsExternalTraderIdUpdated = true;
                var traderId = (int)pqFieldUpdate.Payload;
                ExternalTraderId = traderId;
                return 0;
            case PQTradingSubFieldKeys.LastTradedExternalTraderNameId:
                IsExternalTraderNameUpdated = true;

                var traderNameId = (int)pqFieldUpdate.Payload;
                ExternalTraderNameId = traderNameId;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates
    (DateTime snapShotTime,
        StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.LastTradedStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    ILastExternalCounterPartyTrade ILastExternalCounterPartyTrade.Clone() => Clone();

    IMutableLastExternalCounterPartyTrade IMutableLastExternalCounterPartyTrade.Clone() => Clone();

    IPQLastExternalCounterPartyTrade IPQLastExternalCounterPartyTrade.Clone() => Clone();

    IPQLastExternalCounterPartyTrade ICloneable<IPQLastExternalCounterPartyTrade>.Clone() => Clone();

    public override PQLastExternalCounterPartyTrade Clone() => 
        Recycler?.Borrow<PQLastExternalCounterPartyTrade>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLastExternalCounterPartyTrade(this, NameIdLookup);

    public override PQLastExternalCounterPartyTrade CopyFrom(ILastTrade? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;
        base.CopyFrom(source, copyMergeFlags);
        var pqlecpt = source as PQLastExternalCounterPartyTrade;
        if (pqlecpt == null && source is ILastExternalCounterPartyTrade lecpt)
        {
            ExternalCounterPartyId   = lecpt.ExternalCounterPartyId;
            ExternalCounterPartyName = lecpt.ExternalCounterPartyName;
            ExternalTraderId         = lecpt.ExternalTraderId;
            ExternalTraderName       = lecpt.ExternalTraderName;
        }
        else if (pqlecpt != null)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            NameIdLookup.CopyFrom(pqlecpt.NameIdLookup, copyMergeFlags);

            if (pqlecpt.IsExternalCounterPartyIdUpdated || isFullReplace)
            {
                IsExternalCounterPartyIdUpdated = true;

                ExternalCounterPartyId = pqlecpt.ExternalCounterPartyId;
            }
            if (pqlecpt.IsExternalCounterPartyNameUpdated || isFullReplace)
            {
                IsExternalCounterPartyNameUpdated = true;

                ExternalCounterPartyNameId = pqlecpt.ExternalCounterPartyNameId;
            }

            if (pqlecpt.IsExternalTraderIdUpdated || isFullReplace)
            {
                IsExternalTraderIdUpdated = true;

                ExternalTraderId = pqlecpt.ExternalTraderId;
            }
            if (pqlecpt.IsExternalTraderNameUpdated || isFullReplace)
            {
                IsExternalTraderNameUpdated = true;

                ExternalTraderNameId = pqlecpt.ExternalTraderNameId;
            }
            if (isFullReplace) SetFlagsSame(pqlecpt);
        }

        return this;
    }

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastExternalCounterPartyTrade lastExtCpTrade)) return false;

        var baseSame             = base.AreEquivalent(other, exactTypes);
        var counterPartyIdSame   = ExternalCounterPartyId == lastExtCpTrade.ExternalCounterPartyId;
        var counterPartyNameSame = ExternalCounterPartyName == lastExtCpTrade.ExternalCounterPartyName;
        var traderIdSame         = ExternalTraderId == lastExtCpTrade.ExternalTraderId;
        var traderNameSame       = ExternalTraderName == lastExtCpTrade.ExternalTraderName;

        var allAreSame = baseSame && counterPartyIdSame && counterPartyNameSame && traderIdSame && traderNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ ExternalCounterPartyId;
            hashCode = (hashCode * 397) ^ externalCounterPartyNameId;
            hashCode = (hashCode * 397) ^ ExternalTraderId;
            hashCode = (hashCode * 397) ^ externalTraderNameId;
            return hashCode;
        }
    }

    protected string PQLastExternalCounterPartyTradeToStringMembers =>
        $"{PQLastPaidGivenTradeToStringMembers}, {nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, " +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, {nameof(ExternalCounterPartyNameId)}: {ExternalCounterPartyNameId}, " +
        $"{nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, {nameof(ExternalTraderId)}: {ExternalTraderId}, " +
        $"{nameof(ExternalTraderName)}: {ExternalTraderName}, {nameof(ExternalTraderNameId)}: {ExternalTraderNameId}";

    public override string ToString() => $"{GetType().Name}({PQLastExternalCounterPartyTradeToStringMembers}, {UpdatedFlagsToString})";
}
