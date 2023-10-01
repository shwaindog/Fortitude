using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance
{
    public interface IPerfLogger : ITraceLogger, ILatencyLogger, ICallStatsLogger
    {
    }
}