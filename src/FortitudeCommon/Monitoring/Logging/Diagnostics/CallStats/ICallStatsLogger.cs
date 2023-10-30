namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;

public interface ICallStatsLogger : IHierarchicalLogger
{
    int BatchSize { get; set; }
    void AddCallStat(CallStat lastCallStat);
    void AddCallStat(DateTime startTime, DateTime stopTime);
    void AddContextMeasurement(double measurement);
}
