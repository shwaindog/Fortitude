#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public static class OrderHelpers
{
    public static bool IsBid(this IMutableSpotOrder order) => order.Side == OrderSide.Bid;
    public static bool IsOffer(this IMutableSpotOrder order) => order.Side == OrderSide.Offer;
    public static bool IsInError(this ITransmittableOrder order) => !MutableString.IsNullOrEmpty(order.MutableMessage);
    public static bool IsPending(this ITransmittableOrder order) => order.Status != OrderStatus.Dead;

    public static bool HasPendingExecutions(this ITransmittableOrder order)
    {
        return (order.VenueOrders?.Any(vo => vo.Status != OrderStatus.Dead) ?? false) &&
               (order.Executions?.Sum(e => e.Quantity) ?? 0) < (order?.VenueOrders.Sum(vo => vo.Quantity) ?? 0);
    }
}
