using System;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance
{
    public interface IPerfLoggingPoolFactory
    {
        IPerfLoggerPool GetLatencyTracingLoggerPool(string name, TimeSpan timeout,
            Type callingClass);

        IPerfLoggerPool GetLatencyTracingLoggerPool(string name, TimeSpan timeout,
            string prefixName);
    }
}