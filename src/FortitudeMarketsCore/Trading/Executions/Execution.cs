using System;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Executions
{
    public class Execution : IExecution
    {
        public Execution()
        {
        }

        public Execution(IExecution toClone)
        {
            ExecutionId = toClone.ExecutionId;
            Venue = toClone.Venue;
            VenueOrderId = toClone.VenueOrderId;
            OrderId = toClone.OrderId;
            ExecutionTime = toClone.ExecutionTime;
            Price = toClone.Price;
            Quantity = toClone.Quantity;
            CumlativeQuantity = toClone.CumlativeQuantity;
            CumlativeVwapPrice = toClone.CumlativeVwapPrice;
            CounterParty = toClone.CounterParty;
            ValueDate = toClone.ValueDate;
            Type = toClone.Type;
            ExecutionStageType = toClone.ExecutionStageType;
        }

        public Execution(IExecutionId executionId, IVenue venue, IVenueOrderId venueOrderId, IOrderId orderId, 
            DateTime executionTime, decimal price, decimal quantity, decimal cumlativeQuantity, 
            decimal cumlativeVwapPrice, IParty counterParty, DateTime valueDate, ExecutionType type, ExecutionStageType stageType)
        {
            ExecutionId = executionId;
            Venue = venue;
            VenueOrderId = venueOrderId;
            OrderId = orderId;
            ExecutionTime = executionTime;
            Price = price;
            Quantity = quantity;
            CumlativeQuantity = cumlativeQuantity;
            CumlativeVwapPrice = cumlativeVwapPrice;
            CounterParty = counterParty;
            ValueDate = valueDate;
            Type = type;
            ExecutionStageType = stageType;
        }

        public IExecutionId ExecutionId { get; set; }
        public IVenue Venue { get; set; }
        public IVenueOrderId VenueOrderId { get; set; }
        public IOrderId OrderId { get; set; }
        public DateTime ExecutionTime { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal CumlativeQuantity { get; set; }
        public decimal CumlativeVwapPrice { get; set; }
        public IParty CounterParty { get; set; }
        public DateTime ValueDate { get; set; }
        public ExecutionType Type { get; set; }
        public ExecutionStageType ExecutionStageType { get; set; }

        public IExecution Clone()
        {
            return new Execution(this);
        }
    }
}