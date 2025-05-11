// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerOrders;

public class CounterPartyOrderLayerInfo : AnonymousOrderLayerInfo, IMutableCounterPartyOrderLayerInfo
{
    public CounterPartyOrderLayerInfo() { }

    public CounterPartyOrderLayerInfo
    (int orderId, LayerOrderFlags orderFlags, DateTime createdTime, decimal orderVolume,
        DateTime? updatedTime = null, decimal? remainingVolume = null, string? counterPartyName = null, string? traderName = null)
        : base(orderId, orderFlags, createdTime, orderVolume, updatedTime, remainingVolume)
    {
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderName       = traderName;
    }

    public CounterPartyOrderLayerInfo(IAnonymousOrderLayerInfo toClone) : base(toClone)
    {
        if (toClone is ICounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            ExternalCounterPartyName = counterPartyOrderLayer.ExternalCounterPartyName;
            ExternalTraderName       = counterPartyOrderLayer.ExternalTraderName;
        }
    }

    public override bool IsEmpty
    {
        get => ExternalTraderName == null && ExternalCounterPartyName == null && base.IsEmpty;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            ExternalTraderName       = null;
            ExternalCounterPartyName = null;
        }
    }

    public uint ExternalCounterPartyId { get; set; }

    public string? ExternalCounterPartyName { get; set; }

    public uint    ExternalTraderId { get; set; }

    public string? ExternalTraderName { get; set; }

    public override void StateReset()
    {
        ExternalCounterPartyName = null;
        ExternalTraderName       = null;
        base.StateReset();
    }

    public IReusableObject<ICounterPartyOrderLayerInfo> CopyFrom
        (IReusableObject<ICounterPartyOrderLayerInfo> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IAnonymousOrderLayerInfo)source, copyMergeFlags);


    public ICounterPartyOrderLayerInfo CopyFrom(ICounterPartyOrderLayerInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IAnonymousOrderLayerInfo)source, copyMergeFlags);

    ICounterPartyOrderLayerInfo ICounterPartyOrderLayerInfo.Clone() => Clone();

    ICounterPartyOrderLayerInfo ICloneable<ICounterPartyOrderLayerInfo>.Clone() => Clone();

    IMutableCounterPartyOrderLayerInfo IMutableCounterPartyOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(ICounterPartyOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public override bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);

        if (other is not ICounterPartyOrderLayerInfo counterPartyOrderLayer) return baseSame && !exactTypes;

        var counterPartyNameSame = string.Equals(ExternalCounterPartyName, counterPartyOrderLayer.ExternalCounterPartyName);
        var traderNameSame       = string.Equals(ExternalTraderName, counterPartyOrderLayer.ExternalTraderName);

        return baseSame && counterPartyNameSame && traderNameSame;
    }

    public override ICounterPartyOrderLayerInfo CopyFrom
    (IAnonymousOrderLayerInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ICounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            ExternalCounterPartyName = counterPartyOrderLayer.ExternalCounterPartyName;
            ExternalTraderName       = counterPartyOrderLayer.ExternalTraderName;
        }
        return this;
    }

    public override CounterPartyOrderLayerInfo Clone() =>
        Recycler?.Borrow<CounterPartyOrderLayerInfo>().CopyFrom(this) as CounterPartyOrderLayerInfo ?? new CounterPartyOrderLayerInfo(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((ExternalCounterPartyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((ExternalTraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(CounterPartyOrderLayerInfo)}{{{nameof(OrderId)}: {OrderId}, {nameof(OrderFlags)}: {OrderFlags}, " +
        $"{nameof(CreatedTime)}: {CreatedTime}, {nameof(UpdatedTime)}: {UpdatedTime}, {nameof(OrderVolume)}: {OrderVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(ExternalTraderName)}: {ExternalTraderName}, " +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}}}";
}
