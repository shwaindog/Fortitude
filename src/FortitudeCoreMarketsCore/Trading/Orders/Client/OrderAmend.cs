using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;

namespace FortitudeMarketsCore.Trading.Orders.Client
{
    public class OrderAmend : IOrderAmend
    {
        public OrderAmend(IOrderAmend toClone)
        {
            NewQuantity = toClone.NewQuantity;
            NewPrice = toClone.NewPrice;
            NewSide = toClone.NewSide;
            NewDisplaySize = toClone.NewDisplaySize;
        }

        public OrderAmend(decimal newQuantity, decimal newPrice = -1m, OrderSide newSide = OrderSide.None,
            decimal newDisplaySize = -1m)
        {
            NewQuantity = newQuantity;
            NewPrice = newPrice;
            NewSide = newSide;
            NewDisplaySize = newDisplaySize;
        }

        public decimal NewDisplaySize { get; set; }

        public decimal NewQuantity { get; set; }
        public decimal NewPrice { get; set; }
        public OrderSide NewSide { get; set; }
    }
}