#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded;

public class PQLastTraderPaidGivenTrade : PQLastPaidGivenTrade, IPQLastTraderPaidGivenTrade
{
    private int traderId;

    public PQLastTraderPaidGivenTrade(IPQNameIdLookupGenerator traderIdToNameLookup) =>
        TraderNameIdLookup = traderIdToNameLookup ?? new PQNameIdLookupGenerator(
            PQFieldKeys.LastTraderDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);

    public PQLastTraderPaidGivenTrade(decimal tradePrice = 0m, DateTime? tradeTime = null,
        decimal tradeVolume = 0m, bool wasPaid = false, bool wasGiven = false,
        IPQNameIdLookupGenerator? traderIdToNameLookup = null, string? traderName = null)
        : base(tradePrice, tradeTime,
            tradeVolume, wasPaid, wasGiven)
    {
        TraderNameIdLookup = traderIdToNameLookup ?? new PQNameIdLookupGenerator(
            PQFieldKeys.LastTraderDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        TraderName = traderName;
    }

    public PQLastTraderPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is PQLastTraderPaidGivenTrade pqltpqt)
        {
            TraderNameIdLookup = pqltpqt.TraderNameIdLookup.Clone();
            TraderId = pqltpqt.TraderId;
            IsTraderNameUpdated = pqltpqt.IsTraderNameUpdated;
        }
        else if (toClone is ILastTraderPaidGivenTrade pqLastTrdrPaidOrGivenTrade)
        {
            TraderNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand,
                PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
            TraderName = pqLastTrdrPaidOrGivenTrade.TraderName;
        }

        if (TraderNameIdLookup == null)
            TraderNameIdLookup = new PQNameIdLookupGenerator(
                PQFieldKeys.LastTraderDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
    }

    public int TraderId
    {
        get => traderId;
        set
        {
            if (value == traderId) return;
            IsTraderNameUpdated = true;
            traderId = value;
        }
    }

    public string? TraderName
    {
        get => TraderId == 0 ? null : TraderNameIdLookup[traderId];
        set
        {
            if (value == null)
            {
                TraderId = 0;
                return;
            }

            var convertedSourceId = TraderNameIdLookup[value];
            if (convertedSourceId > 0)
            {
                TraderId = convertedSourceId;
                return;
            }

            TraderId = TraderNameIdLookup.GetOrAddId(value);
        }
    }

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

    public override bool HasUpdates
    {
        get => base.HasUpdates || TraderNameIdLookup.HasUpdates;
        set
        {
            TraderNameIdLookup.HasUpdates = value;
            IsTraderNameUpdated = value;
            base.HasUpdates = value;
        }
    }

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }

    public override bool IsEmpty => base.IsEmpty && TraderName == null;

    public override void StateReset()
    {
        TraderId = 0;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
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
        UpdateStyle updatedStyle)
    {
        if (TraderNameIdLookup is IPQNameIdLookupGenerator pqNameIdLookupGenerator)
            foreach (var stringUpdate in pqNameIdLookupGenerator.GetStringUpdates(snapShotTime,
                         updatedStyle))
                yield return stringUpdate;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id != PQFieldKeys.LastTraderDictionaryUpsertCommand) return false;
        if (TraderNameIdLookup != null) return TraderNameIdLookup.UpdateFieldString(updates);
        return false;
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
            TraderNameIdLookup.CopyFrom(pqltpgt.TraderNameIdLookup);
            if (pqltpgt.IsTraderNameUpdated) TraderId = pqltpgt.traderId;
            UpdatedFlags = pqltpgt.UpdatedFlags;
        }

        return this;
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerQuoteInfo? referenceInstance)
    {
        if (referenceInstance is IPQSourceTickerQuoteInfo pqSrcTkrQtInfo)
            if (ReferenceEquals(pqSrcTkrQtInfo.LastTraderNameLookup, TraderNameIdLookup))
                TraderNameIdLookup = pqSrcTkrQtInfo.LastTraderNameLookup.Clone();
        if (TraderNameIdLookup == null)
            TraderNameIdLookup = new PQNameIdLookupGenerator(
                PQFieldKeys.LastTraderDictionaryUpsertCommand, PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
    }

    public override void EnsureRelatedItemsAreConfigured(IPQLastTrade? referenceInstance)
    {
        if (referenceInstance is IPQLastTraderPaidGivenTrade pqTrdrPvLayer)
            TraderNameIdLookup = pqTrdrPvLayer.TraderNameIdLookup;
    }

    public override IMutableLastTrade Clone() => new PQLastTraderPaidGivenTrade(this);

    ILastTraderPaidGivenTrade ILastTraderPaidGivenTrade.Clone() => (ILastTraderPaidGivenTrade)Clone();

    IMutableLastTraderPaidGivenTrade IMutableLastTraderPaidGivenTrade.Clone() =>
        (IMutableLastTraderPaidGivenTrade)Clone();

    IPQLastTraderPaidGivenTrade IPQLastTraderPaidGivenTrade.Clone() => (IPQLastTraderPaidGivenTrade)Clone();

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastTraderPaidGivenTrade pqLastTraderPaidGivenTrade)) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
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

    public override string ToString() =>
        $"PQLastTraderPaidGivenTrade {{ {nameof(TradePrice)}: {TradePrice:N5}," +
        $" {nameof(TradeTime)}: {TradeTime:O}, {nameof(WasPaid)}: {WasPaid}, " +
        $"{nameof(WasGiven)}: {WasGiven}, {nameof(TradeVolume)}: {TradeVolume:N2}, " +
        $"{nameof(TraderName)}: {TraderName} }}";
}
