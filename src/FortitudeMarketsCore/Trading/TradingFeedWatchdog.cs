using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;

namespace FortitudeMarketsCore.Trading
{
    public class TradingFeedWatchdog : ITradingFeedWatchdog
    {
        public bool Enabled { get; set; }
        public bool IsOrderValid(IOrder actualOrder, out string reason)
        {
            reason = null;
            return true;
        }

        public void OnFeedStatusUpdate(string feedName, bool feedStatus)
        {
            throw new System.NotImplementedException();
        }

        public void OnOrderUpdate(string feedName, IOrder order)
        {
            throw new System.NotImplementedException();
        }

        public void OnExecution(string feedName, IExecution execution)
        {
            throw new System.NotImplementedException();
        }

        public void OnDispose(string feedName)
        {
            throw new System.NotImplementedException();
        }
    }
}
