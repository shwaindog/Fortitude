// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class AdditionalExternalCounterPartyInfo : ReusableObject<IAdditionalExternalCounterPartyOrderInfo>
  , IMutableAdditionalExternalCounterPartyOrderInfo
{
    public AdditionalExternalCounterPartyInfo() { }

    public AdditionalExternalCounterPartyInfo
        (int externalCounterPartyId = 0, string? externalCounterPartyName = null, int externalTraderId = 0, string? externalTraderName = null)
    {
        ExternalCounterPartyId   = externalCounterPartyId;
        ExternalCounterPartyName = externalCounterPartyName;
        ExternalTraderId         = externalTraderId;
        ExternalTraderName       = externalTraderName;
    }

    public AdditionalExternalCounterPartyInfo(IAdditionalExternalCounterPartyOrderInfo? toClone)
    {
        if (toClone != null)
        {
            ExternalCounterPartyId   = toClone.ExternalCounterPartyId;
            ExternalCounterPartyName = toClone.ExternalCounterPartyName;
            ExternalTraderId         = toClone.ExternalTraderId;
            ExternalTraderName       = toClone.ExternalTraderName;
        }
    }

    public int     ExternalCounterPartyId   { get; set; }
    public string? ExternalCounterPartyName { get; set; }

    public int     ExternalTraderId   { get; set; }
    public string? ExternalTraderName { get; set; }

    public bool IsEmpty
    {
        get => ExternalCounterPartyId == 0 && ExternalCounterPartyName == null && ExternalTraderId == 0 && ExternalTraderName == null;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ITrackableReset<IMutableAdditionalExternalCounterPartyOrderInfo>.ResetWithTracking() =>
        ResetWithTracking();

    public AdditionalExternalCounterPartyInfo ResetWithTracking()
    {
        ExternalCounterPartyId   = 0;
        ExternalCounterPartyName = null;
        ExternalTraderId         = 0;
        ExternalTraderName       = null;

        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();
        base.StateReset();
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ICloneable<IMutableAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo IMutableAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    public override AdditionalExternalCounterPartyInfo Clone() =>
        Recycler?.Borrow<AdditionalExternalCounterPartyInfo>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new AdditionalExternalCounterPartyInfo(this);


    IMutableAdditionalExternalCounterPartyOrderInfo ITransferState<IMutableAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IMutableAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override AdditionalExternalCounterPartyInfo CopyFrom
        (IAdditionalExternalCounterPartyOrderInfo? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null) return this;

        ExternalCounterPartyId   = source.ExternalCounterPartyId;
        ExternalCounterPartyName = source.ExternalCounterPartyName;
        ExternalTraderId         = source.ExternalTraderId;
        ExternalTraderName       = source.ExternalTraderName;
        return this;
    }

    public bool AreEquivalent(IAdditionalExternalCounterPartyOrderInfo? other, bool exactTypes = false)
    {
        if (other is null) return false;
        var cpIdSame       = ExternalCounterPartyId == other.ExternalCounterPartyId;
        var cpNameSame     = ExternalCounterPartyName == other.ExternalCounterPartyName;
        var traderIdSame   = ExternalTraderId == other.ExternalTraderId;
        var traderNameSame = ExternalTraderName == other.ExternalTraderName;

        var allAreSame = cpIdSame && cpNameSame && traderIdSame && traderNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAdditionalExternalCounterPartyOrderInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = ExternalCounterPartyId;
            hashCode = ((ExternalCounterPartyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = (ExternalTraderId * 397) ^ hashCode;
            hashCode = ((ExternalTraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string AddExternalCounterPartyInfoToStringMembers =>
        $"{nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, {nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, " +
        $"{nameof(ExternalTraderId)}: {ExternalTraderId}, {nameof(ExternalTraderName)}: {ExternalTraderName}";

    public override string ToString() => $"{GetType().Name}{{{AddExternalCounterPartyInfoToStringMembers}}}";
}
