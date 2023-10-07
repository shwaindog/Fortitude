#region

using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;

#endregion

namespace FortitudeMarketsCore.Trading;

public interface ITradingFeedWatchdog
{
    bool Enabled { get; }

    bool IsOrderValid(IOrder actualOrder, out string? reason);

    void OnFeedStatusUpdate(string feedName, bool feedStatus);
    void OnOrderUpdate(string feedName, IOrder order);
    void OnExecution(string feedName, IExecution execution);
    void OnDispose(string feedName);
}
