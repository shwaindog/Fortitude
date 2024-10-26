namespace FortitudeMarkets.Trading.Orders.Products;

public static class OrderSideHelpers
{
    public static OrderSide Opposite(this OrderSide side)
    {
        if (side != OrderSide.Offer && side != OrderSide.Bid) return OrderSide.None;
        return side == OrderSide.Offer ? OrderSide.Bid : OrderSide.Offer;
    }
}
