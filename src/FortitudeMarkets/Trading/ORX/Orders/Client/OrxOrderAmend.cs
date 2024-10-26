#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Client;

public class OrxOrderAmend : ReusableObject<IOrderAmend>, IOrderAmend
{
    public OrxOrderAmend() { }

    public OrxOrderAmend(IOrderAmend toClone)
    {
        NewDisplaySize = toClone.NewDisplaySize;
        NewQuantity = toClone.NewQuantity;
        NewPrice = toClone.NewPrice;
        NewSide = toClone.NewSide;
    }

    public OrxOrderAmend(decimal newDisplaySize, decimal newQuantity, decimal newPrice, OrderSide newSide)
    {
        NewDisplaySize = newDisplaySize;
        NewQuantity = newQuantity;
        NewPrice = newPrice;
        NewSide = newSide;
    }

    [OrxOptionalField(1)] public decimal NewDisplaySize { get; set; }

    [OrxOptionalField(2)] public decimal NewQuantity { get; set; }

    [OrxOptionalField(3)] public decimal NewPrice { get; set; }

    [OrxOptionalField(4)] public OrderSide NewSide { get; set; }


    public override void StateReset()
    {
        NewDisplaySize = 0;
        NewQuantity = 0;
        NewPrice = 0;
        NewSide = OrderSide.None;
        base.StateReset();
    }

    public override IOrderAmend CopyFrom(IOrderAmend source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        NewDisplaySize = source.NewDisplaySize;
        NewQuantity = source.NewQuantity;
        NewPrice = source.NewPrice;
        NewSide = source.NewSide;
        return this;
    }

    public override IOrderAmend Clone() => Recycler?.Borrow<OrxOrderAmend>().CopyFrom(this) ?? new OrxOrderAmend(this);

    protected bool Equals(OrxOrderAmend other)
    {
        var displaySizeSame = NewDisplaySize == other.NewDisplaySize;
        var quantitySame = NewQuantity == other.NewQuantity;
        var priceSame = NewPrice == other.NewPrice;
        var sideSame = NewSide == other.NewSide;

        return displaySizeSame && quantitySame && priceSame && sideSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxOrderAmend)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = NewDisplaySize.GetHashCode();
            hashCode = (hashCode * 397) ^ NewQuantity.GetHashCode();
            hashCode = (hashCode * 397) ^ NewPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)NewSide;
            return hashCode;
        }
    }
}
