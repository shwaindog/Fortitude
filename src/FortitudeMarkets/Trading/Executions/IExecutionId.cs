#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public interface IExecutionId : IReusableObject<IExecutionId>
{
    IMutableString VenueExecutionId { get; set; }
    int AdapterExecutionId { get; set; }
    IMutableString BookingSystemId { get; set; }
}
