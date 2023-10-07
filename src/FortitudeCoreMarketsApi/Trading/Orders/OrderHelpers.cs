using System.Linq;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;

namespace FortitudeMarketsApi.Trading.Orders
{
    public static class OrderHelpers
    {
        public static bool IsBid(this ISpotOrder order) { return order.Side == OrderSide.Bid; }
        public static bool IsOffer(this ISpotOrder order) { return order.Side == OrderSide.Offer; }
        public static bool IsInError(this IOrder order) { return !MutableString.IsNullOrEmpty(order.Message); }
        public static bool IsPending(this IOrder order) { return order.Status != OrderStatus.Dead; }

        public static bool HasPendingExecutions(this IOrder order)
        {
            return order.VenueOrders.Any(vo => vo.Status != OrderStatus.Dead) &&
                order.Executions.Sum(e => e.Quantity) < order.VenueOrders.Sum(vo => vo.Quantity);
        }
    }
}