#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;

public interface ILatencyLogger : IHierarchicalLogger
{
    LatencyLoggingLevel LatencyLoggingLevel { get; set; }
    TimeSpan MaxAcceptablePeriod { get; set; }
    CallStat LastCallStat { get; }
    DateTime StartTime { get; }
    DateTime StopTime { get; }
    void StartTiming();
    void StopTiming<T, TU, TV>(T optionalData, TU moreOptionalData, TV evenMoreOptionalData);
    void StopTiming<T, TU>(T optionalData, TU moreOptionalData);
    void StopTiming<T>(T? optionalData = default);
    void StopTiming(string? optionalData = null);
}
