#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public interface IOrderAmend : IReusableObject<IOrderAmend>
{
    decimal NewDisplaySize { get; }
    decimal NewQuantity { get; }
    decimal NewPrice { get; }
    OrderSide NewSide { get; }
}
