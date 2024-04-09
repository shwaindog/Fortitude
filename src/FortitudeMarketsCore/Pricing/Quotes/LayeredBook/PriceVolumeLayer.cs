#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

public class PriceVolumeLayer : ReusableObject<IPriceVolumeLayer>, IMutablePriceVolumeLayer
{
    public PriceVolumeLayer() { }

    public PriceVolumeLayer(decimal price = 0m, decimal volume = 0m)
    {
        Price = price;
        Volume = volume;
    }

    public PriceVolumeLayer(IPriceVolumeLayer toClone)
    {
        Price = toClone.Price;
        Volume = toClone.Volume;
    }

    public decimal Price { get; set; }
    public decimal Volume { get; set; }
    public virtual bool IsEmpty => Price == 0m && Volume == 0m;

    public override void StateReset()
    {
        Price = Volume = 0m;
        base.StateReset();
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Price = source.Price;
        Volume = source.Volume;
        return this;
    }

    public override IPriceVolumeLayer Clone() => Recycler?.Borrow<PriceVolumeLayer>().CopyFrom(this) ?? new PriceVolumeLayer(this);

    object ICloneable.Clone() => Clone();

    public virtual bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var priceSame = Price == other.Price;
        var volumeSame = Volume == other.Volume;

        return priceSame && volumeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Price.GetHashCode() * 397) ^ Volume.GetHashCode();
        }
    }

    public override string ToString() =>
        $"PriceVolumeLayer {{{nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2} }}";
}
