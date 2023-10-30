#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;

#endregion

namespace FortitudeMarketsApi.Trading.Orders;

public static class OrderHelpers
{
    public static bool IsBid(this ISpotOrder order) => order.Side == OrderSide.Bid;
    public static bool IsOffer(this ISpotOrder order) => order.Side == OrderSide.Offer;
    public static bool IsInError(this IOrder order) => !MutableString.IsNullOrEmpty(order.Message);
    public static bool IsPending(this IOrder order) => order.Status != OrderStatus.Dead;

    public static bool HasPendingExecutions(this IOrder order)
    {
        return (order.VenueOrders?.Any(vo => vo.Status != OrderStatus.Dead) ?? false) &&
               (order.Executions?.Sum(e => e.Quantity) ?? 0) < (order?.VenueOrders.Sum(vo => vo.Quantity) ?? 0);
    }
}
