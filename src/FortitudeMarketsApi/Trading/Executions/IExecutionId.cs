#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Executions;

public interface IExecutionId : IRecyclableObject<IExecutionId>
{
    IMutableString VenueExecutionId { get; set; }
    int AdapterExecutionId { get; set; }
    IMutableString BookingSystemId { get; set; }
    IExecutionId Clone();
}
