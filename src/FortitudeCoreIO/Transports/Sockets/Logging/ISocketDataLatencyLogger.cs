using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

namespace FortitudeIO.Transports.Sockets.Logging
{
    // inherits from ICallStatsLogger so can be configured in HierarchialLoggingConfigurator<ICallStatsLogger>.Register
    public interface ISocketDataLatencyLogger : ICallStatsLogger
    {
        void ParseTraceLog(ITraceLogger pqTraceLog);
    }
}