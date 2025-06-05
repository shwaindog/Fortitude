// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

[Flags]
public enum PQAdditionalCounterPartyInfoFlags : byte
{
    None                            = 0x00
  , ExternalTraderIdUpdatedFlag     = 0x01
  , ExternalTraderNameIdUpdatedFlag = 0x02

  , ExternalCounterPartyIdUpdatedFlag     = 0x04
  , ExternalCounterPartyNameIdUpdatedFlag = 0x08
}

public interface IPQAdditionalExternalCounterPartyOrderInfo : IMutableAdditionalExternalCounterPartyOrderInfo
  , ISupportsPQNameIdLookupGenerator, IPQSupportsStringUpdates, IPQSupportsNumberPrecisionFieldUpdates
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

    PQAdditionalCounterPartyInfoFlags ExternalCounterPartyUpdatedFlags { get; set; }

    void SetFlagsSame(IAdditionalExternalCounterPartyOrderInfo? toCopyFlags);

    new IPQAdditionalExternalCounterPartyOrderInfo Clone();
}

public class PQAdditionalExternalCounterPartyInfo : ReusableObject<IAdditionalExternalCounterPartyOrderInfo>
  , IPQAdditionalExternalCounterPartyOrderInfo
{
    protected uint SequenceId = uint.MaxValue;

    private PQAdditionalCounterPartyInfoFlags updatedFlags;

    private int counterPartyNameId;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private int traderNameId;
    private int externalCounterPartyId;
    private int externalTraderId;

    public PQAdditionalExternalCounterPartyInfo()
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) SequenceId = 0;
    }

    public PQAdditionalExternalCounterPartyInfo(IPQNameIdLookupGenerator pqNameIdLookupGenerator)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) SequenceId = 0;
    }

    public PQAdditionalExternalCounterPartyInfo
        (IPQNameIdLookupGenerator lookupDict, int counterPartyId = 0, string? counterPartyName = null, int traderId = 0, string? traderName = null)
        //   : base(orderId, createdTime, orderVolume, orderType, genesisFlags, orderLifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        NameIdLookup             = lookupDict;
        ExternalCounterPartyId   = counterPartyId;
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderId         = traderId;
        ExternalTraderName       = traderName;
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) SequenceId = 0;
    }

    public PQAdditionalExternalCounterPartyInfo(IAdditionalExternalCounterPartyOrderInfo? toClone, IPQNameIdLookupGenerator pqNameIdLookupGenerator)
        // : base(toClone)
    {
        NameIdLookup = pqNameIdLookupGenerator;
        if (toClone != null)
        {
            ExternalCounterPartyId   = toClone.ExternalCounterPartyId;
            ExternalCounterPartyName = toClone.ExternalCounterPartyName;
            ExternalTraderId         = toClone.ExternalTraderId;
            ExternalTraderName       = toClone.ExternalTraderName;

            SetFlagsSame(toClone);
        }
        if (GetType() == typeof(PQAdditionalExternalCounterPartyInfo)) SequenceId = 0;
    }

    public PQAdditionalCounterPartyInfoFlags ExternalCounterPartyUpdatedFlags
    {
        get => updatedFlags;
        set => updatedFlags = value;
    }

    public int ExternalCounterPartyId
    {
        get => externalCounterPartyId;
        set
        {
            IsExternalCounterPartyIdUpdated |= value != externalCounterPartyId || SequenceId == 0;
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
            IsExternalCounterPartyNameUpdated |= value != counterPartyNameId || SequenceId == 0;
            counterPartyNameId                =  value;
        }
    }

    public int ExternalTraderId
    {
        get => externalTraderId;
        set
        {
            IsExternalTraderIdUpdated |= value != externalTraderId || SequenceId == 0;
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
            IsExternalTraderNameUpdated |= traderNameId != value || SequenceId == 0;
            traderNameId                =  value;
        }
    }

    public bool IsExternalTraderNameUpdated
    {
        get => (updatedFlags & PQAdditionalCounterPartyInfoFlags.ExternalTraderNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PQAdditionalCounterPartyInfoFlags.ExternalTraderNameIdUpdatedFlag;

            else if (IsExternalTraderNameUpdated) updatedFlags ^= PQAdditionalCounterPartyInfoFlags.ExternalTraderNameIdUpdatedFlag;
        }
    }
    public bool IsExternalCounterPartyNameUpdated
    {
        get => (updatedFlags & PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyNameIdUpdatedFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyNameIdUpdatedFlag;

            else if (IsExternalCounterPartyNameUpdated) updatedFlags ^= PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyNameIdUpdatedFlag;
        }
    }

    public bool IsExternalCounterPartyIdUpdated
    {
        get => (updatedFlags & PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyIdUpdatedFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyIdUpdatedFlag;

            else if (IsExternalCounterPartyIdUpdated) updatedFlags ^= PQAdditionalCounterPartyInfoFlags.ExternalCounterPartyIdUpdatedFlag;
        }
    }
    public bool IsExternalTraderIdUpdated
    {
        get => (updatedFlags & PQAdditionalCounterPartyInfoFlags.ExternalTraderIdUpdatedFlag) > 0;
        set
        {
            if (value)
                updatedFlags |= PQAdditionalCounterPartyInfoFlags.ExternalTraderIdUpdatedFlag;

            else if (IsExternalTraderIdUpdated) updatedFlags ^= PQAdditionalCounterPartyInfoFlags.ExternalTraderIdUpdatedFlag;
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

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => updatedFlags != PQAdditionalCounterPartyInfoFlags.None;
        set
        {
            if (value) return;
            NameIdLookup.HasUpdates = value;
            updatedFlags            = PQAdditionalCounterPartyInfoFlags.None;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => externalCounterPartyId == 0 && counterPartyNameId == 0 && externalTraderId == 0 && traderNameId == 0;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ITrackableReset<IMutableAdditionalExternalCounterPartyOrderInfo>.ResetWithTracking() =>
        ResetWithTracking();

    public virtual PQAdditionalExternalCounterPartyInfo ResetWithTracking()
    {
        externalCounterPartyId = 0;
        counterPartyNameId     = 0;
        externalTraderId       = 0;
        traderNameId           = 0;

        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();

        updatedFlags = PQAdditionalCounterPartyInfoFlags.None;
        base.StateReset();
    }


    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        NameIdLookup.UpdateComplete(updateSequenceId);
        HasUpdates = false;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & PQMessageFlags.Complete) > 0;
        if (fullPicture || IsExternalCounterPartyIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyId
                                         , (uint)ExternalCounterPartyId);
        if (fullPicture || IsExternalCounterPartyNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId
                                         , (uint)ExternalCounterPartyNameId);

        if (fullPicture || IsExternalTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderNameId, (uint)ExternalTraderNameId);
        if (fullPicture || IsExternalTraderIdUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQOrdersSubFieldKeys.OrderExternalTraderId, (uint)ExternalTraderId);
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.OrdersSubId)
        {
            case PQOrdersSubFieldKeys.OrderExternalCounterPartyId:
                IsExternalCounterPartyIdUpdated = true; // in-case of reset and sending 0;
                ExternalCounterPartyId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalCounterPartyNameId:
                IsExternalCounterPartyNameUpdated = true; // in-case of reset and sending 0;
                ExternalCounterPartyNameId        = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalTraderId:
                IsExternalTraderIdUpdated = true; // in-case of reset and sending 0;
                ExternalTraderId          = (int)pqFieldUpdate.Payload;
                return 0;
            case PQOrdersSubFieldKeys.OrderExternalTraderNameId:
                IsExternalTraderNameUpdated = true; // in-case of reset and sending 0;
                ExternalTraderNameId        = (int)pqFieldUpdate.Payload;
                return 0;
        }

        return -1;
    }

    IPQAdditionalExternalCounterPartyOrderInfo IPQAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo IMutableAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    public override PQAdditionalExternalCounterPartyInfo Clone() =>
        Recycler?.Borrow<PQAdditionalExternalCounterPartyInfo>().CopyFrom(this) ??
        new PQAdditionalExternalCounterPartyInfo(this, NameIdLookup);


    public virtual bool AreEquivalent(IAdditionalExternalCounterPartyOrderInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var counterPartyIdSame = ExternalCounterPartyId == other.ExternalCounterPartyId;
        var counterPartySame   = ExternalCounterPartyName == other.ExternalCounterPartyName;
        var traderIdSame       = ExternalTraderId == other.ExternalTraderId;
        var traderNameSame     = ExternalTraderName == other.ExternalTraderName;

        var updatedSame = true;
        if (exactTypes)
        {
            var pqCounterPartyOther = (PQAdditionalExternalCounterPartyInfo)other;
            updatedSame = updatedFlags == pqCounterPartyOther.updatedFlags;
        }

        return counterPartyIdSame && counterPartySame && traderIdSame && traderNameSame && updatedSame;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ITransferState<IMutableAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IMutableAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQAdditionalExternalCounterPartyInfo CopyFrom
        (IAdditionalExternalCounterPartyOrderInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQAdditionalExternalCounterPartyOrderInfo pqCpOrderLyrInfo)
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

            if (isFullReplace) SetFlagsSame(pqCpOrderLyrInfo);
        }
        else if (source is IExternalCounterPartyOrder counterPartyOrderLayerInfo)
        {
            var hasAsNew = copyMergeFlags.HasAsNew();
            if (hasAsNew)
            {
                updatedFlags = PQAdditionalCounterPartyInfoFlags.None;
                SequenceId   = int.MaxValue;
            }

            ExternalCounterPartyId   = counterPartyOrderLayerInfo.ExternalCounterPartyId;
            ExternalCounterPartyName = counterPartyOrderLayerInfo.ExternalCounterPartyName;
            ExternalTraderId         = counterPartyOrderLayerInfo.ExternalTraderId;
            ExternalTraderName       = counterPartyOrderLayerInfo.ExternalTraderName;

            if (hasAsNew)
            {
                SequenceId = 0;
            }
        }

        return this;
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ICloneable<IMutableAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    public void SetFlagsSame(IAdditionalExternalCounterPartyOrderInfo? toCopyFlags)
    {
        if (toCopyFlags is PQAdditionalExternalCounterPartyInfo pqToClone)
        {
            updatedFlags = pqToClone.updatedFlags;
        }
        else if (toCopyFlags is IPQAdditionalExternalCounterPartyOrderInfo ipqAddCpOrders)
        {
            IsExternalCounterPartyIdUpdated   = ipqAddCpOrders.IsExternalCounterPartyIdUpdated;
            IsExternalCounterPartyNameUpdated = ipqAddCpOrders.IsExternalCounterPartyNameUpdated;
            IsExternalTraderIdUpdated         = ipqAddCpOrders.IsExternalTraderIdUpdated;
            IsExternalTraderNameUpdated       = ipqAddCpOrders.IsExternalTraderNameUpdated;
        }
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IExternalCounterPartyOrder?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = externalCounterPartyId;
            hash = (counterPartyNameId * 397) ^ hash;
            hash = (externalTraderId * 397) ^ hash;
            hash = (traderNameId * 397) ^ hash;
            return hash;
        }
    }

    protected string PQCounterPartyOrderLayerInfoToStringMembers =>
        $"{nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, " +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, {nameof(ExternalTraderId)}: {ExternalTraderId}, " +
        $"{nameof(ExternalTraderName)}: {ExternalTraderName}";

    protected string UpdatedFlagsToString => $"{nameof(updatedFlags)}: {updatedFlags}";

    public override string ToString() =>
        $"{nameof(PQAdditionalExternalCounterPartyInfo)}({PQCounterPartyOrderLayerInfoToStringMembers}, {UpdatedFlagsToString})";
}
