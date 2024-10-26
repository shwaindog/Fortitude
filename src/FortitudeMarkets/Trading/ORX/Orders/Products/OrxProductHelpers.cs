#region

using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Products.General;
using FortitudeMarkets.Trading.ORX.Orders.Products.General;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Products;

public static class OrxProductHelpers
{
    public static OrxProductOrder CreateNewOrxProductOrder(this IProductOrder convert)
    {
        switch (convert.ProductType)
        {
            case ProductType.Spot:
                return new OrxSpotOrder((ISpotOrder)convert);
            case ProductType.Forward:
                return new OrxSpotOrder((ISpotOrder)convert);
            case ProductType.Future:
                return new OrxSpotOrder((ISpotOrder)convert);
            case ProductType.Swap:
                return new OrxSpotOrder((ISpotOrder)convert);
            default:
                throw new Exception("Unexpected order type received");
        }
    }
}
