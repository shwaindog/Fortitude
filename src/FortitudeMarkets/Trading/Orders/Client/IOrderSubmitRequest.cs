#region

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public interface IOrderSubmitRequest : ITradingMessage
{
    ITransmittableOrder? OrderDetails { get; set; }
    DateTime OriginalAttemptTime { get; set; }
    DateTime CurrentAttemptTime { get; set; }
    int AttemptNumber { get; set; }
    IMutableString? Tag { get; set; }
}
