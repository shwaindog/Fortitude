#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Client;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Products;

public interface IProductOrder : IRecyclableObject<IProductOrder>
{
    ProductType ProductType { get; }
    IMutableString? Message { get; set; }
    IOrder? Order { get; set; }
    bool IsComplete { get; set; }
    bool IsError { get; }
    void ApplyAmendment(IOrderAmend amendment);
    bool RequiresAmendment(IOrderAmend amendment);
    void RegisterExecution(IExecution execution);
    IProductOrder Clone();
}
