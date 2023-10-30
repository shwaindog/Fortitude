namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenueOrderUpdate : ITradingMessage
{
    IVenueOrder? VenueOrder { get; set; }
    DateTime UpdateTime { get; set; }
    DateTime AdapterSocketReceivedTime { get; set; }
    DateTime AdapterProcessedTime { get; set; }
    DateTime ClientReceivedTime { get; set; }
    IVenueOrderUpdate Clone();
}
