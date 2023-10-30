#region

using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client;

public class OrxOrderAmend : IOrderAmend
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
