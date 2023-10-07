using System;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Orders.Client
{
    public interface IOrderSubmitRequest : ITradingMessage
    {
        IOrder OrderDetails { get; set;  }
        DateTime OriginalAttemptTime { get; set; }
        DateTime CurrentAttemptTime { get; set; }
        int AttemptNumber { get; set; }
        IMutableString Tag { get; set; }
        IOrderSubmitRequest Clone();
    }
}