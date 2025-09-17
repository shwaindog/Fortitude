#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public interface ITransmittableOrder : IMutableOrder, ICloneable<ITransmittableOrder>
{
    IMutableOrder AsOrder { get; }
    
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
