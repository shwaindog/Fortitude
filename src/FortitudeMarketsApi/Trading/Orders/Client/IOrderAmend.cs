#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Trading.Orders.Products;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Client;

public interface IOrderAmend : IReusableObject<IOrderAmend>
{
    decimal NewDisplaySize { get; }
    decimal NewQuantity { get; }
    decimal NewPrice { get; }
    OrderSide NewSide { get; }
}
