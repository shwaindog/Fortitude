using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Executions
{
    public interface IExecutionId
    {
        IMutableString VenueExecutionId { get; set; }
        int AdapterExecutionId { get; set; }
        IMutableString BookingSystemId { get; set; }
        IExecutionId Clone();
    }
}