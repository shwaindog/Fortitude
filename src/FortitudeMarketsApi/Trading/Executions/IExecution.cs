#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsApi.Trading.Executions;

public interface IExecution : IReusableObject<IExecution>
{
    IExecutionId ExecutionId { get; set; }
    IVenue Venue { get; set; }
    IVenueOrderId VenueOrderId { get; set; }
    IOrderId OrderId { get; set; }
    DateTime ExecutionTime { get; set; }
    decimal Price { get; set; }
    decimal Quantity { get; set; }
    decimal CumulativeQuantity { get; set; }
    decimal CumulativeVwapPrice { get; set; }
    IParty CounterParty { get; set; }
    DateTime ValueDate { get; set; }
    ExecutionType Type { get; set; }
    ExecutionStageType ExecutionStageType { get; set; }
}
