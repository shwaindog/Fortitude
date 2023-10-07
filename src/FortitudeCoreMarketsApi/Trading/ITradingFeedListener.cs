using System;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsApi.Trading
{
    public interface ITradingFeedListener : IDisposable
    {
        event Action<string, bool> FeedStatusUpdate;
        event Action<IOrderUpdate> OrderUpdate;
        event Action<IOrderAmendResponse> OrderAmendResponse;
        event Action<IExecutionUpdate> Execution;
        event Action<IVenueOrderUpdate> VenueOrderUpdated;

        event Action Closed;
    }
}