#region

using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsCore.Trading.ORX.Orders.Products.General;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Products;

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
