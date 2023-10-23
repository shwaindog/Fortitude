using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Executions;

namespace FortitudeMarketsCore.Trading.Executions
{
    public class ExecutionId : IExecutionId
    {
        public ExecutionId(IExecutionId toClone)
        {
            VenueExecutionId = toClone.VenueExecutionId;
            AdapterExecutionId = toClone.AdapterExecutionId;
            BookingSystemId = toClone.BookingSystemId;
        }

        public ExecutionId(string venueExecutionId, int adapterExecutionId, string bookingSystemId)
        : this((MutableString)venueExecutionId, adapterExecutionId, (MutableString)bookingSystemId)
        {
        }

        public ExecutionId(IMutableString venueExecutionId, int adapterExecutionId, IMutableString bookingSystemId)
        {
            VenueExecutionId = venueExecutionId;
            AdapterExecutionId = adapterExecutionId;
            BookingSystemId = bookingSystemId;
        }

        public IMutableString VenueExecutionId { get; set; }
        public int AdapterExecutionId { get; set; }
        public IMutableString BookingSystemId { get; set; }

        public IExecutionId Clone()
        {
            return new ExecutionId(this);
        }
    }
}