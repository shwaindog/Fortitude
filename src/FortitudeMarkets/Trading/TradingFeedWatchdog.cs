#region

using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;

#endregion

namespace FortitudeMarkets.Trading;

public class TradingFeedWatchdog : ITradingFeedWatchdog
{
    public bool Enabled { get; set; }

    public bool IsOrderValid(ITransmittableOrder actualOrder, out string? reason)
    {
        reason = null;
        return true;
    }

    public void OnFeedStatusUpdate(string feedName, bool feedStatus)
    {
        throw new NotImplementedException();
    }

    public void OnOrderUpdate(string feedName, ITransmittableOrder order)
    {
        throw new NotImplementedException();
    }

    public void OnExecution(string feedName, IExecution execution)
    {
        throw new NotImplementedException();
    }

    public void OnDispose(string feedName)
    {
        throw new NotImplementedException();
    }
}
