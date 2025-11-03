#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public interface IExecutionId : IReusableObject<IExecutionId>
{
    IMutableString VenueExecutionId { get; set; }
    int AdapterExecutionId { get; set; }
    IMutableString BookingSystemId { get; set; }
}
