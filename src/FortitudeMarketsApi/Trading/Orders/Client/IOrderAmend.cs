using FortitudeMarketsApi.Trading.Orders.Products;

namespace FortitudeMarketsApi.Trading.Orders.Client
{
    public interface IOrderAmend
    {
        decimal NewDisplaySize { get; }
        decimal NewQuantity { get; }
        decimal NewPrice { get; }
        OrderSide NewSide { get; }
    }
}