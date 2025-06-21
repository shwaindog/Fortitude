#region

using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;

#endregion

namespace FortitudeMarkets.Trading;

public interface ITradingFeedWatchdog
{
    bool Enabled { get; }

    bool IsOrderValid(ITransmittableOrder actualOrder, out string? reason);

    void OnFeedStatusUpdate(string feedName, bool feedStatus);
    void OnOrderUpdate(string feedName, ITransmittableOrder order);
    void OnExecution(string feedName, IExecution execution);
    void OnDispose(string feedName);
}
