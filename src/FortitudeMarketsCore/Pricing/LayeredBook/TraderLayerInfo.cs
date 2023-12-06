#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.LayeredBook;

public class TraderLayerInfo : ReusableObject<ITraderLayerInfo>, IMutableTraderLayerInfo
{
    public TraderLayerInfo() { }

    public TraderLayerInfo(string? traderNm = null, decimal traderVolume = 0m)
    {
        TraderName = traderNm;
        TraderVolume = traderVolume;
    }

    public TraderLayerInfo(ITraderLayerInfo toClone)
    {
        TraderName = toClone.TraderName;
        TraderVolume = toClone.TraderVolume;
    }

    public bool IsEmpty => TraderName == null && TraderVolume == 0m;

    public string? TraderName { get; set; }

    public decimal TraderVolume { get; set; }

    public override void Reset()
    {
        TraderVolume = 0m;
        TraderName = null;
        base.Reset();
    }

    public override ITraderLayerInfo CopyFrom(ITraderLayerInfo source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TraderVolume = source.TraderVolume;
        TraderName = source.TraderName;
        return this;
    }

    IMutableTraderLayerInfo IMutableTraderLayerInfo.Clone() => (IMutableTraderLayerInfo)Clone();

    public override ITraderLayerInfo Clone() =>
        Recycler?.Borrow<TraderLayerInfo>().CopyFrom(this) ?? new TraderLayerInfo(TraderName, TraderVolume);

    object ICloneable.Clone() => Clone();

    public virtual bool AreEquivalent(ITraderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var traderNameSame = string.Equals(TraderName, other.TraderName);
        var volumeSame = TraderVolume == other.TraderVolume;

        return traderNameSame && volumeSame;
    }

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || AreEquivalent((ITraderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((TraderName?.GetHashCode() ?? 0) * 397) ^ TraderVolume.GetHashCode();
        }
    }

    public override string ToString() =>
        $"TraderLayerInfo {{ {nameof(TraderName)}: {TraderName}, {nameof(TraderVolume)}: {TraderVolume:N2} }}";
}
