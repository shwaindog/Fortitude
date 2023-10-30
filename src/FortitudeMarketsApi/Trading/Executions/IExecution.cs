using System;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsApi.Trading.Executions
{
    public interface IExecution
    {
        IExecutionId ExecutionId { get; set; }
        IVenue Venue { get; set; }
        IVenueOrderId VenueOrderId { get; set; }
        IOrderId OrderId { get; set; }
        DateTime ExecutionTime { get; set; }
        decimal Price { get; set; }
        decimal Quantity { get; set; }
        decimal CumlativeQuantity { get; set; }
        decimal CumlativeVwapPrice { get; set; }
        IParty CounterParty { get; set; }
        DateTime ValueDate { get; set; }
        ExecutionType Type { get; set; }
        ExecutionStageType ExecutionStageType { get; set; }
        IExecution Clone();
    }
}
