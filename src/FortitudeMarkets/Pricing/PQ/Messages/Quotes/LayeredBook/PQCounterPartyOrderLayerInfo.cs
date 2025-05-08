// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQCounterPartyOrderLayerInfo : IPQAnonymousOrderLayerInfo, IMutableCounterPartyOrderLayerInfo, ISupportsPQNameIdLookupGenerator
  , IPQSupportsStringUpdates<IPQCounterPartyOrderLayerInfo>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int OrderCounterPartyNameId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int OrderTraderNameId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsCounterPartyNameUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsTraderNameUpdated { get; set; }

    new bool HasUpdates { get; set; }

    new IPQCounterPartyOrderLayerInfo Clone();
}

public class PQCounterPartyOrderLayerInfo : PQAnonymousOrderLayerInfo, IPQCounterPartyOrderLayerInfo
{
    private int counterPartyNameId;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int traderNameId;

    public PQCounterPartyOrderLayerInfo()
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo(IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo
    (IPQNameIdLookupGenerator lookupDict, int orderId = 0, LayerOrderFlags orderFlags = LayerOrderFlags.None, DateTime createdTime = default,
        decimal orderVolume = 0m, DateTime? updatedTime = null, decimal? remainingVolume = null, string? counterPartyName = null
      , string? traderName = null)
        : base(orderId, orderFlags, createdTime, orderVolume, updatedTime, remainingVolume)
    {
        NameIdLookup     = lookupDict;
        CounterPartyName = counterPartyName;
        TraderName       = traderName;
        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    public PQCounterPartyOrderLayerInfo(IAnonymousOrderLayerInfo toClone, IPQNameIdLookupGenerator pqNameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (toClone is ICounterPartyOrderLayerInfo counterPartyOrderLayerInfo)
        {
            CounterPartyName = counterPartyOrderLayerInfo.CounterPartyName;
            TraderName       = counterPartyOrderLayerInfo.TraderName;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQCounterPartyOrderLayerInfo)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQCounterPartyOrderLayerInfoToStringMembers =>
        $"{PQAnonymousOrderLayerInfoToStringMembers}, {nameof(TraderName)}: {TraderName}, {nameof(CounterPartyName)}: {CounterPartyName}";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrderTraderNameId
    {
        get => traderNameId;
        set
        {
            IsTraderNameUpdated |= traderNameId != value || NumUpdatesSinceEmpty == 0;
            traderNameId        =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrderCounterPartyNameId
    {
        get => counterPartyNameId;
        set
        {
            IsCounterPartyNameUpdated |= value != counterPartyNameId || NumUpdatesSinceEmpty == 0;
            counterPartyNameId        =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraderName
    {
        get => NameIdLookup[traderNameId];
        set
        {
            var convertedTraderId = NameIdLookup.GetOrAddId(value);
            if (convertedTraderId <= 0 && value != null)
                throw new Exception("Error attempted to set the Trader Name to something " +
                                    "not defined in the source name to Id table.");
            OrderTraderNameId = convertedTraderId;
        }
    }

    public bool IsTraderNameUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.TraderNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.TraderNameIdUpdatedFlag;

            else if (IsTraderNameUpdated) UpdatedFlags ^= OrderLayerInfoFlags.TraderNameIdUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CounterPartyName
    {
        get => NameIdLookup[counterPartyNameId];
        set
        {
            var convertedCounterPartyId = NameIdLookup.GetOrAddId(value);
            if (convertedCounterPartyId <= 0 && value != null)
                throw new Exception("Error attempted to set the CounterParty Name to something " +
                                    "not defined in the source name to Id table.");
            OrderCounterPartyNameId = convertedCounterPartyId;
        }
    }

    public bool IsCounterPartyNameUpdated
    {
        get => (UpdatedFlags & OrderLayerInfoFlags.CounterPartyNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderLayerInfoFlags.CounterPartyNameIdUpdatedFlag;

            else if (IsCounterPartyNameUpdated) UpdatedFlags ^= OrderLayerInfoFlags.CounterPartyNameIdUpdatedFlag;
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
            if (traderNameId > 0) cacheCounterPartyName = CounterPartyName;
            string? cacheTraderName                     = null;
            if (traderNameId > 0) cacheTraderName       = TraderName;
            nameIdLookup = value;
            if (cacheCounterPartyName != null && counterPartyNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(counterPartyNameId, cacheCounterPartyName);
                }
                catch
                {
                    OrderCounterPartyNameId = nameIdLookup.GetOrAddId(cacheCounterPartyName);
                }
            if (cacheTraderName != null && traderNameId > 0)
                try
                {
                    nameIdLookup.SetIdToName(traderNameId, cacheTraderName);
                }
                catch
                {
                    OrderTraderNameId = nameIdLookup.GetOrAddId(cacheTraderName);
                }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => traderNameId == 0 && counterPartyNameId == 0 && base.IsEmpty;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrderTraderNameId       = 0;
            OrderCounterPartyNameId = 0;
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
        if (!updatedOnly || IsCounterPartyNameUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderCounterPartyNameId, (uint)OrderCounterPartyNameId);

        if (!updatedOnly || IsTraderNameUpdated) yield return new PQFieldUpdate(PQQuoteFields.LayerOrders, PQSubFieldKeys.OrderTraderNameId, (uint)OrderTraderNameId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.SubId == PQSubFieldKeys.OrderCounterPartyNameId)
        {
            IsCounterPartyNameUpdated = true; // incase of reset and sending 0;
            OrderCounterPartyNameId   = (int)pqFieldUpdate.Payload;
            return 0;
        }
        else if (pqFieldUpdate.SubId == PQSubFieldKeys.OrderTraderNameId)
        {
            IsTraderNameUpdated = true; // incase of reset and sending 0;
            OrderTraderNameId   = (int)pqFieldUpdate.Payload;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override void StateReset()
    {
        traderNameId       = 0;
        counterPartyNameId = 0;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IPQAnonymousOrderLayerInfo IPQAnonymousOrderLayerInfo.Clone() => Clone();

    public IReusableObject<ICounterPartyOrderLayerInfo> CopyFrom
    (IReusableObject<ICounterPartyOrderLayerInfo> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IAnonymousOrderLayerInfo?)source, copyMergeFlags);


    public ICounterPartyOrderLayerInfo CopyFrom(ICounterPartyOrderLayerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IAnonymousOrderLayerInfo?)source, copyMergeFlags);

    IMutableCounterPartyOrderLayerInfo IMutableCounterPartyOrderLayerInfo.Clone() => Clone();

    ICounterPartyOrderLayerInfo ICloneable<ICounterPartyOrderLayerInfo>.Clone() => Clone();

    ICounterPartyOrderLayerInfo ICounterPartyOrderLayerInfo.Clone() => Clone();

    IPQCounterPartyOrderLayerInfo IPQCounterPartyOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(ICounterPartyOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent(other as IAnonymousOrderLayerInfo, exactTypes);

    public override bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (!(other is ICounterPartyOrderLayerInfo counterPartyOther)) return false;
        var baseSame         = base.AreEquivalent(other, exactTypes);
        var traderNameSame   = TraderName == counterPartyOther.TraderName;
        var counterPartySame = CounterPartyName == counterPartyOther.CounterPartyName;

        var updatedSame = true;
        if (exactTypes)
            updatedSame = counterPartyOther is PQCounterPartyOrderLayerInfo pqCounterPartyOther && UpdatedFlags == pqCounterPartyOther.UpdatedFlags;

        return baseSame && traderNameSame && counterPartySame && updatedSame;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQQuoteFields.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    public IPQCounterPartyOrderLayerInfo CopyFrom(IPQCounterPartyOrderLayerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IAnonymousOrderLayerInfo?)source, copyMergeFlags);

    public override PQCounterPartyOrderLayerInfo CopyFrom
        (IAnonymousOrderLayerInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQCounterPartyOrderLayerInfo pqCpOrderLyrInfo)
        {
            NameIdLookup.CopyFrom(pqCpOrderLyrInfo.NameIdLookup, copyMergeFlags);

            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqCpOrderLyrInfo.IsCounterPartyNameUpdated || isFullReplace)
            {
                IsCounterPartyNameUpdated = true;

                OrderCounterPartyNameId = pqCpOrderLyrInfo.OrderCounterPartyNameId;
            }
            if (pqCpOrderLyrInfo.IsTraderNameUpdated || isFullReplace)
            {
                IsTraderNameUpdated = true;

                OrderTraderNameId             = pqCpOrderLyrInfo.OrderTraderNameId;
            }

            if (isFullReplace && pqCpOrderLyrInfo is PQCounterPartyOrderLayerInfo pqCounterPartyOrder)
                UpdatedFlags = pqCounterPartyOrder.UpdatedFlags;
        }
        else if (source is ICounterPartyOrderLayerInfo counterPartyOrderLayerInfo)
        {
            CounterPartyName = counterPartyOrderLayerInfo.CounterPartyName;
            TraderName       = counterPartyOrderLayerInfo.TraderName;
        }

        return this;
    }

    public override PQCounterPartyOrderLayerInfo Clone() =>
        Recycler?.Borrow<PQCounterPartyOrderLayerInfo>().CopyFrom(this) as PQCounterPartyOrderLayerInfo ??
        new PQCounterPartyOrderLayerInfo(this, NameIdLookup);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ICounterPartyOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = base.GetHashCode();
            hash = (traderNameId * 397) ^ hash;
            hash = (counterPartyNameId * 397) ^ hash;
            return hash;
        }
    }

    public override string ToString() =>
        $"{nameof(PQCounterPartyOrderLayerInfo)}({PQCounterPartyOrderLayerInfoToStringMembers}, {UpdatedFlagsToString})";
}
