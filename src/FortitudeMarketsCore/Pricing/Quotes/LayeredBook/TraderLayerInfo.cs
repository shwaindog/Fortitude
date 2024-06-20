// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

public class TraderLayerInfo : ReusableObject<ITraderLayerInfo>, IMutableTraderLayerInfo
{
    private decimal traderVolume;
    public TraderLayerInfo() { }

    public TraderLayerInfo(string? traderNm = null, decimal traderVolume = 0m)
    {
        TraderName   = traderNm;
        TraderVolume = traderVolume;
    }

    public TraderLayerInfo(ITraderLayerInfo toClone)
    {
        TraderName   = toClone.TraderName;
        TraderVolume = toClone.TraderVolume;
    }

    public bool IsEmpty
    {
        get => TraderName == null && TraderVolume == 0m;
        set
        {
            if (!value) return;
            TraderName   = null;
            TraderVolume = 0m;
        }
    }

    public string? TraderName { get; set; }

    public decimal TraderVolume
    {
        get => traderVolume;
        set
        {
            // limitation of serialization is that only 6 significant digits can be sent efficiently so set the same value here.
            var abs                = Math.Abs(value);
            var beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
            var round              = Math.Max(0, 6 - beforeDecimalPoint);
            var checkValue         = decimal.Round(value, round);

            abs = Math.Abs(checkValue);

            beforeDecimalPoint = abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
            if (beforeDecimalPoint > 6)
            {
                var scale        = beforeDecimalPoint - 6;
                var scalePower10 = 10 * scale;
                checkValue -= checkValue % scalePower10;
            }
            traderVolume = checkValue;
        }
    }

    public override void StateReset()
    {
        TraderVolume = 0m;
        TraderName   = null;
        base.StateReset();
    }

    public override ITraderLayerInfo CopyFrom
    (ITraderLayerInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TraderVolume = source.TraderVolume;
        TraderName   = source.TraderName;
        return this;
    }

    IMutableTraderLayerInfo IMutableTraderLayerInfo.Clone() => (IMutableTraderLayerInfo)Clone();

    public override ITraderLayerInfo Clone() => Recycler?.Borrow<TraderLayerInfo>().CopyFrom(this) ?? new TraderLayerInfo(TraderName, TraderVolume);

    object ICloneable.Clone() => Clone();

    public virtual bool AreEquivalent(ITraderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var traderNameSame = string.Equals(TraderName, other.TraderName);
        var volumeSame     = TraderVolume == other.TraderVolume;

        return traderNameSame && volumeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITraderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((TraderName?.GetHashCode() ?? 0) * 397) ^ TraderVolume.GetHashCode();
        }
    }

    public override string ToString() => $"TraderLayerInfo {{ {nameof(TraderName)}: {TraderName}, {nameof(TraderVolume)}: {TraderVolume:N2} }}";
}
