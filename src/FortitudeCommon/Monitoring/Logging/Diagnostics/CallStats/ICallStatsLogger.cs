using System;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats
{
    public interface ICallStatsLogger : IHierarchialLogger
    {
        int BatchSize { get; set; }
        void AddCallStat(CallStat lastCallStat);
        void AddCallStat(DateTime startTime, DateTime stopTime);
        void AddContextMeasurement(double measurement);
    }
}