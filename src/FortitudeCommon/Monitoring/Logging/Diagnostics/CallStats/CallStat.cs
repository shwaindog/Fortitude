using System;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats
{
    public struct CallStat
    {
        public readonly TimeSpan ExecutionTime;
        public readonly DateTime StartTime;
        public double Measurement;

        public CallStat(DateTime startTime, TimeSpan executionTime, double measurement = 0)
        {
            StartTime = startTime;
            ExecutionTime = executionTime;
            Measurement = measurement;
        }
    }
}