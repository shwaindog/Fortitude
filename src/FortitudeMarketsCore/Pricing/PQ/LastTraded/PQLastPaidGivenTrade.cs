#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded;

public interface IPQLastPaidGivenTrade : IPQLastTrade, IMutableLastPaidGivenTrade
{
    bool IsWasPaidUpdated { get; set; }
    bool IsWasGivenUpdated { get; set; }
    bool IsTradeVolumeUpdated { get; set; }
    new IPQLastPaidGivenTrade Clone();
}

[Flags]
public enum LastTradeBooleanFlags : byte
{
    None = 0x00
    , WasGiven = 0x01
    , WasPaid = 0x02
}

public class PQLastPaidGivenTrade : PQLastTrade, IPQLastPaidGivenTrade
{
    protected LastTradeBooleanFlags LastTradeBooleanFlags;

    private decimal tradeVolume;

    public PQLastPaidGivenTrade(decimal tradePrice = 0m, DateTime? tradeTime = null, decimal tradeVolume = 0m,
        bool wasPaid = false, bool wasGiven = false) : base(tradePrice, tradeTime)
    {
        TradeVolume = tradeVolume;
        WasPaid = wasPaid;
        WasGiven = wasGiven;
    }

    public PQLastPaidGivenTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastPaidGivenTrade lastPaidGivenTrade)
        {
            TradeVolume = lastPaidGivenTrade.TradeVolume;
            WasPaid = lastPaidGivenTrade.WasPaid;
            WasGiven = lastPaidGivenTrade.WasGiven;
        }

        if (toClone is IPQLastPaidGivenTrade pqLastPaidGivenTrade)
        {
            IsTradeVolumeUpdated = pqLastPaidGivenTrade.IsTradeVolumeUpdated;
            IsWasGivenUpdated = pqLastPaidGivenTrade.IsWasGivenUpdated;
            IsWasPaidUpdated = pqLastPaidGivenTrade.IsWasPaidUpdated;
        }

        if (toClone is PQLastPaidGivenTrade pqLastPaidGiven) UpdatedFlags = pqLastPaidGiven.UpdatedFlags;
    }

    protected string PQLastPaidGivenTradeToStringMembers =>
        $"{PQLastTradeToStringMembers}, {nameof(WasPaid)}: {WasPaid}, " +
        $"{nameof(WasGiven)}: {WasGiven}, {nameof(TradeVolume)}: {TradeVolume:N2}";

    public bool WasPaid
    {
        get => (LastTradeBooleanFlags & LastTradeBooleanFlags.WasPaid) > 0;
        set
        {
            if (WasPaid == value) return;
            IsWasPaidUpdated = true;
            if (value)
                LastTradeBooleanFlags |= LastTradeBooleanFlags.WasPaid;
            else if (WasPaid) LastTradeBooleanFlags ^= LastTradeBooleanFlags.WasPaid;
        }
    }

    public bool IsWasPaidUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.WasPaidUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.WasPaidUpdated;
            else if (IsWasPaidUpdated) UpdatedFlags ^= LastTradeUpdated.WasPaidUpdated;
        }
    }

    public bool WasGiven
    {
        get => (LastTradeBooleanFlags & LastTradeBooleanFlags.WasGiven) > 0;
        set
        {
            if (WasGiven == value) return;
            IsWasGivenUpdated = true;
            if (value)
                LastTradeBooleanFlags |= LastTradeBooleanFlags.WasGiven;
            else if (WasGiven) LastTradeBooleanFlags ^= LastTradeBooleanFlags.WasGiven;
        }
    }

    public bool IsWasGivenUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.WasGivenUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.WasGivenUpdated;
            else if (IsWasGivenUpdated) UpdatedFlags ^= LastTradeUpdated.WasGivenUpdated;
        }
    }

    public decimal TradeVolume
    {
        get => tradeVolume;
        set
        {
            if (tradeVolume == value) return;
            IsTradeVolumeUpdated = true;
            tradeVolume = value;
        }
    }

    public bool IsTradeVolumeUpdated
    {
        get => (UpdatedFlags & LastTradeUpdated.VolumeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LastTradeUpdated.VolumeUpdated;
            else if (IsTradeVolumeUpdated) UpdatedFlags ^= LastTradeUpdated.VolumeUpdated;
        }
    }

    public override bool HasUpdates
    {
        set => base.HasUpdates = IsTradeVolumeUpdated = IsWasGivenUpdated = IsWasPaidUpdated = value;
    }

    public override bool IsEmpty => base.IsEmpty && WasPaid == false && WasGiven == false && TradeVolume == 0m;

    public override void StateReset()
    {
        WasGiven = WasPaid = false;
        TradeVolume = 0m;
        base.StateReset();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var deltaUpdateField in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSetting))
            yield return deltaUpdateField;
        var flagModifier = (byte)((WasGiven ? PQFieldFlags.IsGivenFlag : 0)
                                  | (WasPaid ? PQFieldFlags.IsPaidFlag : 0));
        if (!updatedOnly || IsTradeVolumeUpdated || IsWasGivenUpdated || IsWasPaidUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LastTradeVolumeOffset, TradeVolume,
                (byte)((quotePublicationPrecisionSetting?.VolumeScalingPrecision ?? 6) | flagModifier));
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the recentlytraded has already forwarded this through to the correct lasttrade
        if (pqFieldUpdate.Id >= PQFieldKeys.LastTradeVolumeOffset &&
            pqFieldUpdate.Id < PQFieldKeys.LastTradeVolumeOffset +
            PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
        {
            TradeVolume = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
            WasGiven = (pqFieldUpdate.Flag & PQFieldFlags.IsGivenFlag) == PQFieldFlags.IsGivenFlag;
            WasPaid = (pqFieldUpdate.Flag & PQFieldFlags.IsPaidFlag) == PQFieldFlags.IsPaidFlag;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override ILastTrade CopyFrom(ILastTrade? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source == null) return this;
        var pqlpgt = source as PQLastPaidGivenTrade;
        if (pqlpgt == null && source is ILastPaidGivenTrade lpgt)
        {
            TradeVolume = lpgt.TradeVolume;
            WasGiven = lpgt.WasGiven;
            WasPaid = lpgt.WasPaid;
        }
        else if (pqlpgt != null)
        {
            if (pqlpgt.IsTradeVolumeUpdated) tradeVolume = pqlpgt.tradeVolume;
            if (pqlpgt.IsWasPaidUpdated) WasPaid = pqlpgt.WasPaid;
            if (pqlpgt.IsWasGivenUpdated) WasGiven = pqlpgt.WasGiven;

            UpdatedFlags = pqlpgt.UpdatedFlags;
        }

        return this;
    }

    public override IMutableLastTrade Clone() => new PQLastPaidGivenTrade(this);

    ILastPaidGivenTrade ILastPaidGivenTrade.Clone() => (ILastPaidGivenTrade)Clone();

    IMutableLastPaidGivenTrade IMutableLastPaidGivenTrade.Clone() => (IMutableLastPaidGivenTrade)Clone();

    IPQLastPaidGivenTrade IPQLastPaidGivenTrade.Clone() => (IPQLastPaidGivenTrade)Clone();

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastPaidGivenTrade pqLastPaidGivenTrader)) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
        var traderVolumeSame = tradeVolume == pqLastPaidGivenTrader.TradeVolume;
        var wasPaidSame = WasPaid == pqLastPaidGivenTrader.WasPaid;
        var wasGivenSame = WasGiven == pqLastPaidGivenTrader.WasGiven;
        return baseSame && traderVolumeSame && wasPaidSame && wasGivenSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTrade, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ tradeVolume.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQLastPaidGivenTradeToStringMembers})";
}
