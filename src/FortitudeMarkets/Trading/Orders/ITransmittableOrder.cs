#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface ITransmittableOrder : IOrder, ICloneable<ITransmittableOrder>
{
    IOrder AsOrder { get; }
    
    IOrderPublisher? OrderPublisher { get; set; }
    IMutableString?  MutableTicker  { get; set; }
    IMutableString?  MutableMessage { get; set; }


    void ApplyAmendment(IOrderAmend amendment);
    bool RequiresAmendment(IOrderAmend amendment);
    void RegisterExecution(IExecution execution);

    OrxOrder AsOrxOrder { get; }

    ITransmittableOrder   AsTransmittableOrder { get; }

    new ITransmittableOrder Clone();
}
