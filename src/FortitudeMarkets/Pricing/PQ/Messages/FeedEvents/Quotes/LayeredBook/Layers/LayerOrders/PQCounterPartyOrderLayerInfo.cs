// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

public interface IPQCounterPartyOrderLayerInfo : IPQAnonymousOrderLayerInfo, IMutableExternalCounterPartyOrderLayerInfo
  , ISupportsPQNameIdLookupGenerator
  , IPQSupportsStringUpdates<IPQCounterPartyOrderLayerInfo>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int ExternalCounterPartyNameId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int ExternalTraderNameId { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsExternalCounterPartyNameUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsExternalTraderNameUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsExternalCounterPartyIdUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsExternalTraderIdUpdated { get; set; }

    new bool HasUpdates { get; set; }

    new IPQCounterPartyOrderLayerInfo Clone();
}

public class PQCounterPartyOrderLayerInfo : PQAnonymousOrderLayerInfo, IPQCounterPartyOrderLayerInfo
{
    private int counterPartyNameId;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int traderNameId;
    private int externalCounterPartyId;
    private int externalTraderId;

    public PQCounterPartyOrderLayerInfo()
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo(IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo
    (IPQNameIdLookupGenerator lookupDict, int orderId = 0, DateTime createdTime = default, decimal orderVolume = 0m
      , LayerOrderFlags orderFlags = LayerOrderFlags.None, OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None
      , OrderLifeCycleState orderLifeCycleState = OrderLifeCycleState.None, string? counterPartyName = null, string? traderName = null
      , int counterPartyId = 0, int traderId = 0, DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
        : base(orderId,createdTime, orderVolume,  orderFlags, orderType, typeFlags, orderLifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        NameIdLookup             = lookupDict;
        ExternalCounterPartyId   = counterPartyId;
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderId         = traderId;
        ExternalTraderName       = traderName;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo(IAnonymousOrderLayerInfo toClone, IPQNameIdLookupGenerator pqNameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (toClone is IExternalCounterPartyOrderLayerInfo counterPartyOrderLayerInfo)
        {
            ExternalCounterPartyId   = counterPartyOrderLayerInfo.ExternalCounterPartyId;
            ExternalCounterPartyName = counterPartyOrderLayerInfo.ExternalCounterPartyName;
            ExternalTraderId         = counterPartyOrderLayerInfo.ExternalTraderId;
            ExternalTraderName       = counterPartyOrderLayerInfo.ExternalTraderName;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQCounterPartyOrderLayerInfoToStringMembers =>
        $"{PQAnonymousOrderLayerInfoToStringMembers}, {nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, " +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, {nameof(ExternalTraderId)}: {ExternalTraderId}, " +
        $"{nameof(ExternalTraderName)}: {ExternalTraderName}";


    public int ExternalCounterPartyId
    {
        get => externalCounterPartyId;
        set
        {
            IsExternalCounterPartyIdUpdated |= value != externalCounterPartyId || NumUpdatesSinceEmpty == 0;
            externalCounterPartyId          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalCounterPartyName
    {
        get => NameIdLookup[counterPartyNameId];
        set
        {
            var convertedCounterPartyId = NameIdLookup.GetOrAddId(value);
            if (convertedCounterPartyId <= 0 && value != null)
                throw new Exception("Error attempted to set the CounterParty Name to something " +
                                    "not defined in the source name to Id table.");
            ExternalCounterPartyNameId = convertedCounterPartyId;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExternalCounterPartyNameId
    {
        get => counterPartyNameId;
        set
        {
            IsExternalCounterPartyNameUpdated |= value != counterPartyNameId || NumUpdatesSinceEmpty == 0;
            counterPartyNameId                =  value;
        }
    }

    public int ExternalTraderId
    {
        get => externalTraderId;
        set
        {
            IsExternalTraderIdUpdated |= value != externalTraderId || NumUpdatesSinceEmpty == 0;
            externalTraderId          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalTraderName
    {
        get => NameIdLookup[traderNameId];
        set
        {
            var convertedTraderId = NameIdLookup.GetOrAddId(value);
            if (convertedTraderId <= 0 && value != null)
                throw new Exception("Error attempted to set the Trader Name to something " +
                                    "not defined in the source name to Id table.");
            ExternalTraderNameId = convertedTraderId;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExternalTraderNameId
    {
        get => traderNameId;
        set
        {
            IsExternalTraderNameUpdated |= traderNameId != value || NumUpdatesSinceEmpty == 0;
            traderNameId                =  value;
        }
    }

    public bool IsExternalTraderNameUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.ExternalTraderNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.ExternalTraderNameIdUpdatedFlag;

            else if (IsExternalTraderNameUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.ExternalTraderNameIdUpdatedFlag;
        }
    }
    public bool IsExternalCounterPartyNameUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.ExternalCounterPartyNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.ExternalCounterPartyNameIdUpdatedFlag;

            else if (IsExternalCounterPartyNameUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.ExternalCounterPartyNameIdUpdatedFlag;
        }
    }

    public bool IsExternalCounterPartyIdUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.ExternalCounterPartyIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.ExternalCounterPartyIdUpdatedFlag;

            else if (IsExternalCounterPartyIdUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.ExternalCounterPartyIdUpdatedFlag;
        }
    }
    public bool IsExternalTraderIdUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoUpdatedFlags.ExternalTraderIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoUpdatedFlags.ExternalTraderIdUpdatedFlag;

            else if (IsExternalTraderIdUpdated) UpdatedFlags ^= OrderLayerInfoUpdatedFlags.ExternalTraderIdUpdatedFlag;
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
            string? cacheCounterPartyName               = null;
            if (traderNameId > 0) cacheCounterPartyName = ExternalCounterPartyName;
            string? cacheTraderName                     = null;
            if (traderNameId > 0) cacheTraderName       = ExternalTraderName;
            nameIdLookup = value;
            if (cacheCounterPartyName != null && counterPartyNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(counterPartyNameId, cacheCounterPartyName);
                }
                catch
                {
                    ExternalCounterPartyNameId = nameIdLookup.GetOrAddId(cacheCounterPartyName);
                }
            if (cacheTraderName != null && traderNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(traderNameId, cacheTraderName);
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
        get => base.IsEmpty && externalCounterPartyId == 0 && counterPartyNameId == 0 && externalTraderId == 0 && traderNameId == 0;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            ExternalCounterPartyId     = 0;
            ExternalCounterPartyNameId = 0;
            ExternalTraderId           = 0;
            ExternalTraderNameId       = 0;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set
        {
            base.HasUpdates = value;
            if (value) return;
            NameIdLookup.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        NameIdLookup.UpdateComplete();
        base.UpdateComplete();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsExternalCounterPartyIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyId
                                         , (uint)ExternalCounterPartyId);
        if (!updatedOnly || IsExternalCounterPartyNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId
                                         , (uint)ExternalCounterPartyNameId);

        if (!updatedOnly || IsExternalTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderNameId, (uint)ExternalTraderNameId);
        if (!updatedOnly || IsExternalTraderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderId, (uint)ExternalTraderId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.OrdersSubId)
        {
            case PQOrdersSubFieldKeys.OrderExternalCounterPartyId:
                IsExternalCounterPartyIdUpdated = true; // incase of reset and sending 0;
                ExternalCounterPartyId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId:
                IsExternalCounterPartyNameUpdated = true; // incase of reset and sending 0;
                ExternalCounterPartyNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalTraderId:
                IsExternalTraderIdUpdated = true; // incase of reset and sending 0;
                ExternalTraderId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalTraderNameId:
                IsExternalTraderNameUpdated = true; // incase of reset and sending 0;
                ExternalTraderNameId        = (int)pqFieldUpdate.Payload;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override void StateReset()
    {
        externalCounterPartyId = 0;
        counterPartyNameId     = 0;
        externalTraderId       = 0;
        traderNameId           = 0;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQAnonymousOrderLayerInfo IPQAnonymousOrderLayerInfo.Clone() => Clone();


    IMutableExternalCounterPartyOrderLayerInfo IMutableExternalCounterPartyOrderLayerInfo.Clone() => Clone();

    IExternalCounterPartyOrderLayerInfo ICloneable<IExternalCounterPartyOrderLayerInfo>.Clone() => Clone();

    IExternalCounterPartyOrderLayerInfo IExternalCounterPartyOrderLayerInfo.Clone() => Clone();

    IPQCounterPartyOrderLayerInfo IPQCounterPartyOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(IExternalCounterPartyOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent(other as IAnonymousOrderLayerInfo, exactTypes);

    public override bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (!(other is IExternalCounterPartyOrderLayerInfo counterPartyOther)) return false;
        var baseSame         = base.AreEquivalent(other, exactTypes);
        var counterPartyIdSame = ExternalCounterPartyId == counterPartyOther.ExternalCounterPartyId;
        var counterPartySame = ExternalCounterPartyName == counterPartyOther.ExternalCounterPartyName;
        var traderIdSame   = ExternalTraderId == counterPartyOther.ExternalTraderId;
        var traderNameSame   = ExternalTraderName == counterPartyOther.ExternalTraderName;

        var updatedSame = true;
        if (exactTypes)
            updatedSame = counterPartyOther is PQCounterPartyOrderLayerInfo pqCounterPartyOther && UpdatedFlags == pqCounterPartyOther.UpdatedFlags;

        return baseSame && counterPartyIdSame && counterPartySame && traderIdSame && traderNameSame && updatedSame;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    IPQCounterPartyOrderLayerInfo ITransferState<IPQCounterPartyOrderLayerInfo>.CopyFrom
        (IPQCounterPartyOrderLayerInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQCounterPartyOrderLayerInfo CopyFrom
        (IAnonymousOrderLayerInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQCounterPartyOrderLayerInfo pqCpOrderLyrInfo)
        {
            NameIdLookup.CopyFrom(pqCpOrderLyrInfo.NameIdLookup, copyMergeFlags);

            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqCpOrderLyrInfo.IsExternalCounterPartyIdUpdated || isFullReplace)
            {
                IsExternalCounterPartyIdUpdated = true;

                ExternalCounterPartyId = pqCpOrderLyrInfo.ExternalCounterPartyId;
            }
            if (pqCpOrderLyrInfo.IsExternalCounterPartyNameUpdated || isFullReplace)
            {
                IsExternalCounterPartyNameUpdated = true;

                ExternalCounterPartyNameId = pqCpOrderLyrInfo.ExternalCounterPartyNameId;
            }
            if (pqCpOrderLyrInfo.IsExternalTraderIdUpdated || isFullReplace)
            {
                IsExternalTraderIdUpdated = true;

                ExternalTraderId = pqCpOrderLyrInfo.ExternalTraderId;
            }
            if (pqCpOrderLyrInfo.IsExternalTraderNameUpdated || isFullReplace)
            {
                IsExternalTraderNameUpdated = true;

                ExternalTraderNameId = pqCpOrderLyrInfo.ExternalTraderNameId;
            }

            if (isFullReplace && pqCpOrderLyrInfo is PQCounterPartyOrderLayerInfo pqCounterPartyOrder)
                UpdatedFlags = pqCounterPartyOrder.UpdatedFlags;
        }
        else if (source is IExternalCounterPartyOrderLayerInfo counterPartyOrderLayerInfo)
        {
            ExternalCounterPartyId = counterPartyOrderLayerInfo.ExternalCounterPartyId;
            ExternalCounterPartyName = counterPartyOrderLayerInfo.ExternalCounterPartyName;
            ExternalTraderId       = counterPartyOrderLayerInfo.ExternalTraderId;
            ExternalTraderName       = counterPartyOrderLayerInfo.ExternalTraderName;
        }

        return this;
    }

    IMutableExternalCounterPartyOrderLayerInfo ICloneable<IMutableExternalCounterPartyOrderLayerInfo>.Clone() => Clone();

    public override PQCounterPartyOrderLayerInfo Clone() =>
        Recycler?.Borrow<PQCounterPartyOrderLayerInfo>().CopyFrom(this) ??
        new PQCounterPartyOrderLayerInfo(this, NameIdLookup);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IExternalCounterPartyOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = base.GetHashCode();
            hash = (externalCounterPartyId * 397) ^ hash;
            hash = (counterPartyNameId * 397) ^ hash;
            hash = (externalTraderId * 397) ^ hash;
            hash = (traderNameId * 397) ^ hash;
            return hash;
        }
    }

    public override string ToString() =>
        $"{nameof(PQCounterPartyOrderLayerInfo)}({PQCounterPartyOrderLayerInfoToStringMembers}, {UpdatedFlagsToString})";
}
