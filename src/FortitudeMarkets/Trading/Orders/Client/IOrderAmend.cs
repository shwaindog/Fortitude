#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Trading.Orders.Products;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public interface IOrderAmend : IReusableObject<IOrderAmend>
{
    decimal NewDisplaySize { get; }
    decimal NewQuantity { get; }
    decimal NewPrice { get; }
    OrderSide NewSide { get; }
}
