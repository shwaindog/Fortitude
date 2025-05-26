// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastExternalCounterPartyTrade : LastPaidGivenTrade, IMutableLastExternalCounterPartyTrade
{
    public LastExternalCounterPartyTrade() { }

    public LastExternalCounterPartyTrade
    (uint tradeId = 0, uint batchId = 0, decimal tradePrice = 0m, DateTime? tradeDateTime = null, decimal tradeVolume = 0m,
      int counerPartyId = 0, string? counterPartyName = null, int traderId = 0, string? traderName = null, uint orderId = 0
   ,  bool wasPaid = false , bool wasGiven = false, LastTradedTypeFlags tradeTypeFlags = LastTradedTypeFlags.None
      , LastTradedLifeCycleFlags tradeLifecycleStatus = LastTradedLifeCycleFlags.None
      , DateTime? firstNotifiedTime = null, DateTime? adapterReceivedTime = null, DateTime? updateTime = null)
        : base(tradeId, batchId, tradePrice, tradeDateTime, tradeVolume, orderId, wasPaid, wasGiven
             , tradeTypeFlags, tradeLifecycleStatus, firstNotifiedTime, adapterReceivedTime, updateTime)
    {
        ExternalCounterPartyId   = counerPartyId;
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderId         = traderId;
        ExternalTraderName       = traderName;
    }

    public LastExternalCounterPartyTrade(ILastTrade toClone) : base(toClone)
    {
        if (toClone is ILastExternalCounterPartyTrade lastExtCpTrade)
        {
            ExternalCounterPartyId = lastExtCpTrade.ExternalCounterPartyId;
            ExternalCounterPartyName = lastExtCpTrade.ExternalCounterPartyName;
            ExternalTraderId     = lastExtCpTrade.ExternalTraderId;
            ExternalTraderName     = lastExtCpTrade.ExternalTraderName;
        }
    }

    [JsonIgnore] public override LastTradeType   LastTradeType           => LastTradeType.PriceLastTraderPaidOrGivenVolume;
    [JsonIgnore] public override LastTradedFlags SupportsLastTradedFlags => LastTradedFlags.TraderName | base.SupportsLastTradedFlags;

    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int     ExternalCounterPartyId   { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalCounterPartyName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int     ExternalTraderId         { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalTraderName { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get =>
            base.IsEmpty
         && ExternalCounterPartyId == 0
         && ExternalCounterPartyName == null
         && ExternalTraderId == 0
         && ExternalTraderName == null;
        set => base.IsEmpty = value;
    }

    public override LastExternalCounterPartyTrade ResetWithTracking()
    {
        ExternalCounterPartyId = 0;
        ExternalCounterPartyName = null;
        ExternalTraderId = 0;
        ExternalTraderName = null;

        base.ResetWithTracking();

        return this;
    }

    ILastExternalCounterPartyTrade ILastExternalCounterPartyTrade.Clone() => Clone();

    IMutableLastExternalCounterPartyTrade IMutableLastExternalCounterPartyTrade.Clone() => Clone();

    public override LastExternalCounterPartyTrade Clone() =>
        Recycler?.Borrow<LastExternalCounterPartyTrade>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new LastExternalCounterPartyTrade(this);

    public override LastExternalCounterPartyTrade CopyFrom(ILastTrade source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILastExternalCounterPartyTrade lastExtCpTrade)
        {
            ExternalCounterPartyId = lastExtCpTrade.ExternalCounterPartyId;
            ExternalCounterPartyName = lastExtCpTrade.ExternalCounterPartyName;
            ExternalTraderId = lastExtCpTrade.ExternalTraderId;
            ExternalTraderName = lastExtCpTrade.ExternalTraderName;
        }
        return this;
    }

    public override bool AreEquivalent(ILastTrade? other, bool exactTypes = false)
    {
        if (!(other is ILastExternalCounterPartyTrade lastExtCpTrade)) return false;

        var baseSame       = base.AreEquivalent(other, exactTypes);
        var counterPartyIdSame = ExternalCounterPartyId == lastExtCpTrade.ExternalCounterPartyId;
        var counterPartyNameSame = ExternalCounterPartyName== lastExtCpTrade.ExternalCounterPartyName;
        var traderIdSame = ExternalTraderId == lastExtCpTrade.ExternalTraderId;
        var traderNameSame = ExternalTraderName == lastExtCpTrade.ExternalTraderName;

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
            hashCode = (hashCode * 397) ^ (ExternalCounterPartyName?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ ExternalTraderId;
            hashCode = (hashCode * 397) ^ (ExternalTraderName?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string LastExternalCounterPartyTradeToStringMembers =>
        $"{LastPaidGivenTradeToStringMembers}, {nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, " +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, {nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, " +
        $"{nameof(ExternalTraderId)}: {ExternalTraderId}, {nameof(ExternalTraderName)}: {ExternalTraderName}";

    public override string ToString() => $"{nameof(LastExternalCounterPartyTrade)}{{{LastExternalCounterPartyTradeToStringMembers}}}";
}
