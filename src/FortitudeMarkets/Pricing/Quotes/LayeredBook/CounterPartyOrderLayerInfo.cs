// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class CounterPartyOrderLayerInfo : AnonymousOrderLayerInfo, IMutableCounterPartyOrderLayerInfo
{
    public CounterPartyOrderLayerInfo() { }

    public CounterPartyOrderLayerInfo
    (int orderId, LayerOrderFlags orderFlags, DateTime createdTime, decimal orderVolume,
        DateTime? updatedTime = null, decimal? remainingVolume = null, string? counterPartyName = null, string? traderName = null)
        : base(orderId, orderFlags, createdTime, orderVolume, updatedTime, remainingVolume)
    {
        CounterPartyName = counterPartyName;
        TraderName       = traderName;
    }

    public CounterPartyOrderLayerInfo(IAnonymousOrderLayerInfo toClone) : base(toClone)
    {
        if (toClone is ICounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            CounterPartyName = counterPartyOrderLayer.CounterPartyName;
            TraderName       = counterPartyOrderLayer.TraderName;
        }
    }

    public override bool IsEmpty
    {
        get => TraderName == null && CounterPartyName == null && base.IsEmpty;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            TraderName       = null;
            CounterPartyName = null;
        }
    }

    public string? TraderName { get; set; }

    public string? CounterPartyName { get; set; }

    public override void StateReset()
    {
        CounterPartyName = null;
        TraderName       = null;
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

        var counterPartyNameSame = string.Equals(CounterPartyName, counterPartyOrderLayer.CounterPartyName);
        var traderNameSame       = string.Equals(TraderName, counterPartyOrderLayer.TraderName);

        return baseSame && counterPartyNameSame && traderNameSame;
    }

    public override ICounterPartyOrderLayerInfo CopyFrom
    (IAnonymousOrderLayerInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ICounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            CounterPartyName = counterPartyOrderLayer.CounterPartyName;
            TraderName       = counterPartyOrderLayer.TraderName;
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
            hashCode = ((CounterPartyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((TraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(CounterPartyOrderLayerInfo)}{{{nameof(OrderId)}: {OrderId}, {nameof(OrderFlags)}: {OrderFlags}, " +
        $"{nameof(CreatedTime)}: {CreatedTime}, {nameof(UpdatedTime)}: {UpdatedTime}, {nameof(OrderVolume)}: {OrderVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TraderName)}: {TraderName}, " +
        $"{nameof(CounterPartyName)}: {CounterPartyName}}}";
}
