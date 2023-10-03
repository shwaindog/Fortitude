#region

using System.Diagnostics;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;

public class LatencyLogger : ILatencyLogger
{
    private const string ActivationKeywordDetailed = "detailed";
    private const string ActivationKeywordReportOnMs = "reportonms";
    internal static readonly HighPrecisionTimeContext LatencyTimer = new();
    private static int instanceCounter;
    private static Action<IHierarchicalLogger, string>? settingTranslation;
    private readonly int instanceNumber;
    private readonly IFLogger logger;
    private readonly string name;

    public LatencyLogger(string name, TimeSpan maxAcceptablePeriod, string? prefixName)
    {
        this.name = name;
        MaxAcceptablePeriod = maxAcceptablePeriod;
        FullNameOfLogger = prefixName + "." + name;
        logger = FLoggerFactory.Instance.GetLogger("Latency." + FullNameOfLogger);
        instanceNumber = Interlocked.Increment(ref instanceCounter);
    }

    public LatencyLogger(string name, TimeSpan maxAcceptablePeriod, Type measuringClass)
        : this(name, maxAcceptablePeriod, measuringClass.FullName) { }

    public static IEnumerable<string> ActivationKeywords =>
        HierarchicalConfigurationUpdater
            .ActivationKeyWords.Concat(new[] { ActivationKeywordDetailed, ActivationKeywordReportOnMs });

    public string FullNameOfLogger { get; set; }

    public LatencyLoggingLevel LatencyLoggingLevel { get; set; } = LatencyLoggingLevel.None;

    public bool Enabled
    {
        get => LatencyLoggingLevel != LatencyLoggingLevel.None;
        set => LatencyLoggingLevel = value ? LatencyLoggingLevel.Log : LatencyLoggingLevel.None;
    }

    public TimeSpan MaxAcceptablePeriod { get; set; }
    public DateTime StartTime { get; private set; }
    public DateTime StopTime { get; private set; }

    public CallStat LastCallStat
    {
        get
        {
            var strtTime = StartTime;
            var stpTime = StopTime;
            if (strtTime > stpTime)
                LogMessageWithStackTrace("Start time greater than stop time for LatencyLogger Instance " +
                                         instanceNumber);
            return new CallStat(strtTime, stpTime - strtTime);
        }
    }

    public Action<IHierarchicalLogger, string> SettingTranslation
    {
        get
        {
            return settingTranslation ?? (settingTranslation = (hl, setting) =>
            {
                var ll = hl as ILatencyLogger;
                if (ll != null && setting != null)
                {
                    setting = setting.ToLower();
                    if (HierarchicalConfigurationUpdater.IsActivationKeyWord(setting))
                    {
                        ll.LatencyLoggingLevel = LatencyLoggingLevel.Log;
                        return;
                    }

                    if (setting == ActivationKeywordDetailed)
                    {
                        ll.LatencyLoggingLevel = LatencyLoggingLevel.Detailed;
                        return;
                    }

                    var newTimeSpan = setting.Split('-');
                    if (newTimeSpan.Length == 2 && newTimeSpan[0] == ActivationKeywordReportOnMs)
                    {
                        int reportOnMs;
                        if (int.TryParse(newTimeSpan[1], out reportOnMs))
                            if (reportOnMs > 0 && reportOnMs <= 1000000)
                                ll.MaxAcceptablePeriod = TimeSpan.FromMilliseconds(reportOnMs);
                        if (ll.LatencyLoggingLevel == LatencyLoggingLevel.None)
                            ll.LatencyLoggingLevel = LatencyLoggingLevel.Log;
                        return;
                    }
                }

                if (ll != null) ll.LatencyLoggingLevel = LatencyLoggingLevel.None;
            });
        }
    }

    public string DefaultStringValue => LatencyLoggingLevel.None.ToString();

    public void StartTiming()
    {
        StartTime = LatencyTimer.UtcNow;
    }

    public void StopTiming<T, TU, TV>(T? optionalData = default, TU? moreOptionalData
        = default, TV? evenMoreOptionalData = default)
    {
        if (!Enabled) return;
        StopTime = LatencyTimer.UtcNow;
        var period = StopTime - StartTime;
        if (period > MaxAcceptablePeriod)
            LogMaxPeriodExceeded(period, optionalData, moreOptionalData, evenMoreOptionalData);
    }

    public void StopTiming<T, TU>(T? optionalData = default, TU? moreOptionalData
        = default)
    {
        if (!Enabled) return;
        StopTime = LatencyTimer.UtcNow;
        var period = StopTime - StartTime;
        if (period > MaxAcceptablePeriod) LogMaxPeriodExceeded(period, optionalData, moreOptionalData);
    }

    public void StopTiming<T>(T? optionalData = default)
    {
        if (!Enabled) return;
        StopTime = LatencyTimer.UtcNow;
        var period = StopTime - StartTime;
        if (period > MaxAcceptablePeriod) LogMaxPeriodExceeded(period, optionalData);
    }

    public void StopTiming(string? optionalData = null)
    {
        if (!Enabled) return;
        StopTime = LatencyTimer.UtcNow;
        var period = StopTime - StartTime;
        if (period > MaxAcceptablePeriod) LogMaxPeriodExceeded(period, optionalData);
    }

    public void StopTiming<T, TU, TV, TW>(T optionalData, TU moreOptionalData, TV evenMoreOptionalData,
        TW andEvenMoreOptionalData)
    {
        if (!Enabled) return;
        StopTime = LatencyTimer.UtcNow;
        var period = StopTime - StartTime;
        if (period > MaxAcceptablePeriod)
            LogMaxPeriodExceeded(period, optionalData, moreOptionalData, evenMoreOptionalData,
                andEvenMoreOptionalData);
    }

    private void LogMaxPeriodExceeded<T, TU, TV, TW>(TimeSpan period, T optionalData, TU moreOptionalData,
        TV evenMoreOptionalData, TW andEvenMoreOptionalData)
    {
        logger.Debug(
            $"{name} exceeded maximum allowable period of {MaxAcceptablePeriod.TotalMilliseconds:N3} ms " +
            $"it took {period.TotalMilliseconds:N3} ms." +
            $" {optionalData}, {moreOptionalData}, {evenMoreOptionalData}, {andEvenMoreOptionalData}");

        if (LatencyLoggingLevel == LatencyLoggingLevel.Detailed) LogMessageWithStackTrace(name);
    }

    private void LogMaxPeriodExceeded<T, TU, TV>(TimeSpan period, T optionalData, TU moreOptionalData,
        TV evenMoreOptionalData)
    {
        logger.Debug(
            $"{name} exceeded maximum allowable period of {MaxAcceptablePeriod.TotalMilliseconds:N3} ms " +
            $"it took {period.TotalMilliseconds:N3} ms." +
            $" {optionalData}, {moreOptionalData}, {evenMoreOptionalData}");

        if (LatencyLoggingLevel == LatencyLoggingLevel.Detailed) LogMessageWithStackTrace(name);
    }

    private void LogMaxPeriodExceeded<T, TU>(TimeSpan period, T optionalData, TU moreOptionalData)
    {
        logger.Debug(
            $"{name} exceeded maximum allowable period of {MaxAcceptablePeriod.TotalMilliseconds:N3} ms " +
            $"it took {period.TotalMilliseconds:N3} ms. {optionalData}, {moreOptionalData}");

        if (LatencyLoggingLevel == LatencyLoggingLevel.Detailed) LogMessageWithStackTrace(name);
    }

    private void LogMaxPeriodExceeded<T>(TimeSpan period, T optionalData)
    {
        logger.Debug(
            $"{name} exceeded maximum allowable period of {MaxAcceptablePeriod.TotalMilliseconds:N3} ms " +
            $"it took {period.TotalMilliseconds:N3} ms. {optionalData}");

        if (LatencyLoggingLevel == LatencyLoggingLevel.Detailed) LogMessageWithStackTrace(name);
    }

    internal void LogMessageWithStackTrace(object msg)
    {
        logger.Debug($"{msg} - stack trace {new StackTrace()}");
    }
}
