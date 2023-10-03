#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;

public class PerfLogger : IPerfLogger
{
    private static int instanceCounter;
    private readonly CallStatsLogger callStatsLogger;
    private readonly int instanceNumber;
    private readonly LatencyLogger latencyLogger;
    private readonly TraceLogger traceLogger;

    public PerfLogger(string name, TimeSpan timeoutTimeSpan, Type callingType)
        : this(name, timeoutTimeSpan, callingType.FullName!) { }

    public PerfLogger(string name, TimeSpan timeoutTimeSpan, string prefixName)
    {
        latencyLogger = new LatencyLogger(name, timeoutTimeSpan, prefixName);
        traceLogger = new TraceLogger(name, prefixName);
        callStatsLogger = CallStatsLogger.GetCallStatsLogger(name, prefixName);
        FullNameOfLogger = prefixName + "." + name;

        HierarchicalLoggingConfigurator<ILatencyLogger>.Register(latencyLogger);
        HierarchicalLoggingConfigurator<ITraceLogger>.Register(traceLogger);
        HierarchicalLoggingConfigurator<ICallStatsLogger>.Register(callStatsLogger);
        instanceNumber = Interlocked.Increment(ref instanceCounter);
    }

    public string FullNameOfLogger { get; set; }

    public LatencyLoggingLevel LatencyLoggingLevel
    {
        get => latencyLogger.LatencyLoggingLevel;
        set => latencyLogger.LatencyLoggingLevel = value;
    }

    public bool Enabled
    {
        get => traceLogger.Enabled || latencyLogger.Enabled;
        set { }
    }

    public Action<IHierarchicalLogger, string> SettingTranslation
    {
        get { return (hl, setting) => { }; }
    }

    public string DefaultStringValue => "Not used";

    public void Start()
    {
        traceLogger.Start();
        latencyLogger.StartTiming();
    }

    public void Add<TU>(string identifier, TU subject)
    {
        if (traceLogger.Enabled) traceLogger.Add(identifier, subject);
    }

    public void Add(string identifier)
    {
        if (traceLogger.Enabled) traceLogger.Add(identifier);
    }

    public void Dedent()
    {
        if (traceLogger.Enabled) traceLogger.Dedent();
    }

    public void Indent()
    {
        if (traceLogger.Enabled) traceLogger.Indent();
    }

    public DateTime StartTime => latencyLogger.StartTime;

    public DateTime StopTime => latencyLogger.StopTime;

    public CallStat LastCallStat => latencyLogger.LastCallStat;

    public TimeSpan MaxAcceptablePeriod
    {
        get => latencyLogger.MaxAcceptablePeriod;
        set => latencyLogger.MaxAcceptablePeriod = value;
    }

    public void StartTiming()
    {
        Start();
    }

    public bool WriteTrace
    {
        get => traceLogger.WriteTrace;
        set => traceLogger.WriteTrace = value;
    }

    public bool DefaultWriteTrace
    {
        get => traceLogger.DefaultWriteTrace;
        set => traceLogger.DefaultWriteTrace = value;
    }

    public void StopTiming<T1, TU, TV>(T1 optionalData, TU moreOptionalData, TV evenMoreOptionalData)
    {
        if (latencyLogger.Enabled)
            latencyLogger.StopTiming(optionalData, moreOptionalData, evenMoreOptionalData, traceLogger);
        TraceFinished();
        LogCallStats();
    }

    public void StopTiming<T1, TU>(T1 optionalData, TU moreOptionalData)
    {
        if (latencyLogger.Enabled) latencyLogger.StopTiming(optionalData, moreOptionalData, traceLogger);
        TraceFinished();
        LogCallStats();
    }

    public void StopTiming<T1>(T1? optionalData)
    {
        if (latencyLogger.Enabled) latencyLogger.StopTiming(optionalData, traceLogger);
        TraceFinished();
        LogCallStats();
    }

    public void StopTiming(string? optionalData = null)
    {
        if (latencyLogger.Enabled)
        {
            if (optionalData != null)
                latencyLogger.StopTiming(optionalData, traceLogger);
            else
                latencyLogger.StopTiming(traceLogger);
        }

        TraceFinished();
        LogCallStats();
    }

    public List<TraceEntry> TraceFinished() => traceLogger.TraceFinished();

    public int BatchSize
    {
        get => callStatsLogger.BatchSize;
        set => callStatsLogger.BatchSize = value;
    }

    public void AddCallStat(CallStat lastCallStat)
    {
        callStatsLogger.AddCallStat(lastCallStat);
    }

    public void AddCallStat(DateTime startTime, DateTime stopTime)
    {
        callStatsLogger.AddCallStat(startTime, stopTime);
    }

    public void AddContextMeasurement(double measurement)
    {
        callStatsLogger.AddContextMeasurement(measurement);
    }

    private void LogCallStats()
    {
        if (callStatsLogger.Enabled)
        {
            if (latencyLogger.Enabled)
            {
                AddCallStat(latencyLogger.LastCallStat);
            }
            else
            {
                var startTime = latencyLogger.StartTime;
                var stopTime = LatencyLogger.LatencyTimer.UtcNow;
                if (startTime > stopTime)
                    latencyLogger.LogMessageWithStackTrace(
                        "Start time greater than stop time for TraceLoggerInstance " + instanceNumber);
                callStatsLogger.AddCallStat(new CallStat(startTime, stopTime - startTime));
            }
        }
    }
}
