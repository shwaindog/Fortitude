#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Executions;

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
    IPartyPortfolio CounterPartyPortfolio { get; set; }
    DateTime ValueDate { get; set; }
    ExecutionType Type { get; set; }
    ExecutionStageType ExecutionStageType { get; set; }
}
