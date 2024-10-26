#region

using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading;

public interface ITradingFeedListener : IDisposable
{
    event Action<string?, bool>? FeedStatusUpdate;
    event Action<IOrderUpdate>? OrderUpdate;
    event Action<IOrderAmendResponse>? OrderAmendResponse;
    event Action<IExecutionUpdate>? Execution;
    event Action<IVenueOrderUpdate>? VenueOrderUpdated;

    event Action? Closed;
}
