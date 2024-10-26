#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;

#endregion

namespace FortitudeMarkets.Trading.Orders.Products;

public interface IProductOrder : IReusableObject<IProductOrder>
{
    ProductType ProductType { get; }
    IMutableString? Message { get; set; }
    IOrder? Order { get; set; }
    bool IsComplete { get; set; }
    bool IsError { get; }
    void ApplyAmendment(IOrderAmend amendment);
    bool RequiresAmendment(IOrderAmend amendment);
    void RegisterExecution(IExecution execution);
}
