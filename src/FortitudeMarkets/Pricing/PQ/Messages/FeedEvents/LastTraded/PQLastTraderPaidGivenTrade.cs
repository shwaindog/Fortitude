// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQLastTraderPaidGivenTrade : IPQLastPaidGivenTrade, IMutableLastTraderPaidGivenTrade,
    IPQSupportsStringUpdates<ILastTrade>, ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQLastTraderPaidGivenTrade>
{
    int  TraderId            { get; set; }
    bool IsTraderNameUpdated { get; set; }

    new IPQNameIdLookupGenerator    NameIdLookup { get; set; }
    new IPQLastTraderPaidGivenTrade Clone();

    new IPQLastTraderPaidGivenTrade ResetWithTracking();
}

public class PQLastTraderPaidGivenTrade : PQLastPaidGivenTrade, IPQLastTraderPaidGivenTrade
{
    private IPQNameIdLookupGenerator nameIdLookup;

    private int traderId;

    public PQLastTraderPaidGivenTrade(IPQNameIdLookupGenerator traderIdToNameLookup)
    {
        nameIdLookup = traderIdToNameLookup;

        if (GetType() == typeof(PQLastTraderPaidGivenTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTraderPaidGivenTrade
    (IPQNameIdLookupGenerator traderIdToNameLookup, decimal tradePrice = 0m, DateTime? tradeTime = null
      , decimal tradeVolume = 0m, bool wasPaid = false, bool wasGiven = false, string? traderName = null)
        : base(tradePrice, tradeTime, tradeVolume, wasPaid, wasGiven)
    {
        nameIdLookup = traderIdToNameLookup;
        TraderName   = traderName;

        if (GetType() == typeof(PQLastTraderPaidGivenTrade)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTraderPaidGivenTrade(ILastTrade toClone, IPQNameIdLookupGenerator traderIdToNameLookup) : base(toClone)
    {
        nameIdLookup = traderIdToNameLookup;
        if (toClone is PQLastTraderPaidGivenTrade pqltpgt)
        {
            TraderId            = pqltpgt.TraderId;
            IsTraderNameUpdated = pqltpgt.IsTraderNameUpdated;
        }

        if (toClone is ILastTraderPaidGivenTrade lastTraderPaidGivenTrade) TraderName = lastTraderPaidGivenTrade.TraderName;

        if (GetType() == typeof(PQLastTraderPaidGivenTrade)) NumUpdatesSinceEmpty = 0;
    }

    protected string PQLastTraderPaidGivenTradeToStringMembers => $"{PQLastPaidGivenTradeToStringMembers}, {nameof(TraderName)}: {TraderName}";

    [JsonIgnore] public override LastTradeType   LastTradeType           => LastTradeType.PriceLastTraderPaidOrGivenVolume;
    [JsonIgnore] public override LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.TraderName | base.SupportsLastTradedFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int TraderId
    {
        get => traderId;
        set
        {
            IsTraderNameUpdated |= traderId != value || NumUpdatesSinceEmpty == 0;

            traderId = value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraderName
    {
        get => TraderId == 0 ? null : NameIdLookup[traderId];
        set
        {
            if (value == null)
            {
                TraderId = 0;
                return;
            }

            var convertedSourceId = NameIdLookup[value];
            if (convertedSourceId > 0)
            {
                TraderId = convertedSourceId;
                return;
            }

            TraderId = NameIdLookup.GetOrAddId(value);
        }
    }

    [JsonIgnore]
    public bool IsTraderNameUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TraderNameUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.TraderNameUpdated;

            else if (IsTraderNameUpdated) UpdatedFlags ^= LastTradeUpdated.TraderNameUpdated;
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || NameIdLookup.HasUpdates;
        set
        {
            NameIdLookup.HasUpdates = value;
            IsTraderNameUpdated     = value;
            base.HasUpdates         = value;
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

            string? cacheTraderName           = null;
            if (traderId > 0) cacheTraderName = TraderName;
            nameIdLookup = value;
            if (traderId > 0) traderId = nameIdLookup[cacheTraderName];
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && TraderName == null;
        set
        {
            if (!value) return;

            TraderName = null;

            base.IsEmpty = true;
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

    IPQLastTraderPaidGivenTrade ITrackableReset<IPQLastTraderPaidGivenTrade>.ResetWithTracking() => ResetWithTracking();

    IPQLastTraderPaidGivenTrade IPQLastTraderPaidGivenTrade.ResetWithTracking() => ResetWithTracking();

    public override PQLastTraderPaidGivenTrade ResetWithTracking()
    {
        traderId = 0;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        traderId = 0;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return deltaUpdateField;
        if (!updatedOnly || IsTraderNameUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedTraderNameId, (uint)traderId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        if (pqFieldUpdate.TradingSubId == PQTradingSubFieldKeys.LastTradedTraderNameId)
        {
            var traderCountOrId = (int)pqFieldUpdate.Payload;
            TraderId = traderCountOrId;
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

    public override ILastTrade CopyFrom(ILastTrade? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source == null) return this;
        var pqltpgt = source as PQLastTraderPaidGivenTrade;
        if (pqltpgt == null && source is ILastTraderPaidGivenTrade ltpgt)
        {
            TraderName = ltpgt.TraderName;
        }
        else if (pqltpgt != null)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            NameIdLookup.CopyFrom(pqltpgt.NameIdLookup, copyMergeFlags);
            if (pqltpgt.IsTraderNameUpdated || isFullReplace) TraderId = pqltpgt.TraderId;
            if (isFullReplace) UpdatedFlags                            = pqltpgt.UpdatedFlags;
        }

        return this;
    }

    public override IMutableLastTrade Clone() => new PQLastTraderPaidGivenTrade(this, NameIdLookup);

    ILastTraderPaidGivenTrade ILastTraderPaidGivenTrade.Clone() => (ILastTraderPaidGivenTrade)Clone();

    IMutableLastTraderPaidGivenTrade IMutableLastTraderPaidGivenTrade.Clone() => (IMutableLastTraderPaidGivenTrade)Clone();

    IPQLastTraderPaidGivenTrade IPQLastTraderPaidGivenTrade.Clone() => (IPQLastTraderPaidGivenTrade)Clone();

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastTraderPaidGivenTrade pqLastTraderPaidGivenTrade)) return false;

        var baseSame       = base.AreEquivalent(other, exactTypes);
        var traderNameSame = TraderName == pqLastTraderPaidGivenTrade.TraderName;
        var allSame        = baseSame && traderNameSame;
        return allSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ TraderName?.GetHashCode() ?? 0;
        }
    }

    public override string ToString() => $"{GetType().Name}({PQLastTraderPaidGivenTradeToStringMembers})";
}
