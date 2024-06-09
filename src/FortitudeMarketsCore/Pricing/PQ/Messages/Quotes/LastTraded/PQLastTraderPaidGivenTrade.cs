// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQLastTraderPaidGivenTrade : IPQLastPaidGivenTrade, IMutableLastTraderPaidGivenTrade,
    IPQSupportsStringUpdates<ILastTrade>, ISupportsPQNameIdLookupGenerator
{
    int  TraderId            { get; set; }
    bool IsTraderNameUpdated { get; set; }

    new IPQNameIdLookupGenerator    NameIdLookup { get; set; }
    new IPQLastTraderPaidGivenTrade Clone();
}

public class PQLastTraderPaidGivenTrade : PQLastPaidGivenTrade, IPQLastTraderPaidGivenTrade
{
    private int traderId;

    public PQLastTraderPaidGivenTrade(IPQNameIdLookupGenerator traderIdToNameLookup) => NameIdLookup = traderIdToNameLookup;

    public PQLastTraderPaidGivenTrade(IPQNameIdLookupGenerator traderIdToNameLookup, decimal tradePrice = 0m, DateTime? tradeTime = null,
        decimal tradeVolume = 0m, bool wasPaid = false, bool wasGiven = false, string? traderName = null)
        : base(tradePrice, tradeTime,
               tradeVolume, wasPaid, wasGiven)
    {
        NameIdLookup = traderIdToNameLookup;
        TraderName   = traderName;
    }

    public PQLastTraderPaidGivenTrade(ILastTrade toClone, IPQNameIdLookupGenerator traderIdToNameLookup) : base(toClone)
    {
        NameIdLookup = traderIdToNameLookup;
        if (toClone is PQLastTraderPaidGivenTrade pqltpgt)
        {
            TraderId            = pqltpgt.TraderId;
            IsTraderNameUpdated = pqltpgt.IsTraderNameUpdated;
        }

        if (toClone is ILastTraderPaidGivenTrade lastTraderPaidGivenTrade) TraderName = lastTraderPaidGivenTrade.TraderName;
    }

    protected string PQLastTraderPaidGivenTradeToStringMembers => $"{PQLastPaidGivenTradeToStringMembers}, {nameof(TraderName)}: {TraderName}";

    public override LastTradeType   LastTradeType           => LastTradeType.PriceLastTraderPaidOrGivenVolume;
    public override LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.TraderName | base.SupportsLastTradedFlags;

    public int TraderId
    {
        get => traderId;
        set
        {
            if (value == traderId) return;
            IsTraderNameUpdated = true;
            traderId            = value;
        }
    }

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

    public bool IsTraderNameUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.TraderNameUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags                           |= LastTradeUpdated.TraderNameUpdated;
            else if (IsTraderNameUpdated) UpdatedFlags ^= LastTradeUpdated.TraderNameUpdated;
        }
    }

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

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

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

    public override void StateReset()
    {
        TraderId = 0;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, StorageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                   quotePublicationPrecisionSetting))
            yield return deltaUpdateField;
        if (!updatedOnly || IsTraderNameUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LastTraderIdOffset, traderId);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        if (pqFieldUpdate.Id >= PQFieldKeys.LastTraderIdOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LastTraderIdOffset + PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
        {
            var traderCountOrId = (int)pqFieldUpdate.Value;
            TraderId = traderCountOrId;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime,
        StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime,
                                                                   messageFlags))
            yield return stringUpdate;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFieldKeys.LastTraderDictionaryUpsertCommand) return false;
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
            NameIdLookup.CopyFrom(pqltpgt.NameIdLookup);
            if (pqltpgt.IsTraderNameUpdated || isFullReplace) TraderId = NameIdLookup.GetOrAddId(pqltpgt.TraderName);
            UpdatedFlags = pqltpgt.UpdatedFlags;
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
        return baseSame && traderNameSame;
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
