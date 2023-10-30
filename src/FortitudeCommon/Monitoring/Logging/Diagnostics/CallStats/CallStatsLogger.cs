#region

using System.Collections.Concurrent;
using System.Globalization;
using FortitudeCommon.DataStructures;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;

public class CallStatsLogger : ICallStatsLogger
{
    public const int DefaultReportAfter = 10_000;

    private static readonly ConcurrentDictionary<string, CallStatsLogger> InstanceTracker = new();

    private static readonly object StaticSyncCntx = new();
    private static Action<IHierarchicalLogger, string>? settingTranslation;
    private readonly ObjectPool<List<CallStat>> listCallStatPool;
    private readonly IFLogger logger;
    private SpinLock addCallStatSpinLock;
    private int batchSize;
    private volatile List<CallStat>? currentBatch;
    private double currentMeasurement;

    private CallStatsLogger(string name, string prefixName)
    {
        FullNameOfLogger = GenKey(prefixName, name);
        logger = FLoggerFactory.Instance.GetLogger("CallStats." + FullNameOfLogger);
        listCallStatPool = new ObjectPool<List<CallStat>>(() => new List<CallStat>(DefaultReportAfter));
        batchSize = -1;
    }

    public static IEnumerable<string> ActivationKeywords => HierarchicalConfigurationUpdater.ActivationKeyWords;

    public bool Enabled
    {
        get => BatchSize > 1;
        set => BatchSize = value ? BatchSize > 2 ? BatchSize : DefaultReportAfter : -1;
    }

    public int BatchSize
    {
        get => batchSize;
        set
        {
            if (value == batchSize) return;
            if (value > 1000 * 1000) value = 1000 * 1000;
            var gotLock = false;
            try
            {
                addCallStatSpinLock.Enter(ref gotLock);
                if (value < 2)
                {
                    listCallStatPool.DiscardPooledObjects();
                    currentBatch = null;
                }
                else if (currentBatch != null)
                {
                    if (currentBatch.Count < value) currentBatch.Capacity = value;
                }
                else
                {
                    currentBatch = listCallStatPool.Borrow();
                    currentBatch.Capacity = value;
                }
            }
            finally
            {
                if (gotLock) addCallStatSpinLock.Exit();
            }

            batchSize = value;
        }
    }

    public void AddCallStat(CallStat lastCallStat)
    {
        if (!Enabled || currentBatch == null) return;
        var gotLock = false;
        List<CallStat>? logStats = null;
        try
        {
            addCallStatSpinLock.Enter(ref gotLock);
            if (currentBatch.Count >= BatchSize)
            {
                logStats = currentBatch;
                currentBatch = listCallStatPool.Borrow();
                var currentCapacity = currentBatch.Capacity;
                if (BatchSize > 1 && (currentCapacity < BatchSize || BatchSize * 2 <= currentCapacity))
                    currentBatch.Capacity = BatchSize;
            }

            lastCallStat.Measurement = currentMeasurement;
            currentMeasurement = 0;
            currentBatch.Add(lastCallStat);
        }
        catch (Exception e)
        {
            logger.Warn("CallStatsLogger.AddCallStat got {0}", e);
        }
        finally
        {
            if (gotLock) addCallStatSpinLock.Exit();
        }

        if (logStats != null) QueueCalculationOnThreadPool(logStats);
    }

    public void AddContextMeasurement(double measurement)
    {
        currentMeasurement = measurement;
    }

    public string DefaultStringValue => (-1).ToString(CultureInfo.InvariantCulture);

    public Action<IHierarchicalLogger, string> SettingTranslation
    {
        get
        {
            return settingTranslation ?? (settingTranslation = (hl, setting) =>
            {
                if (!(hl is ICallStatsLogger csl) || setting == null) return;

                if (HierarchicalConfigurationUpdater.IsActivationKeyWord(setting))
                {
                    if (csl.BatchSize < 2) csl.BatchSize = DefaultReportAfter;
                    return;
                }

                if (int.TryParse(setting, out var btchSize))
                {
                    csl.BatchSize = btchSize;
                    return;
                }

                csl.BatchSize = -1;
            });
        }
    }

    public string FullNameOfLogger { get; set; }

    public void AddCallStat(DateTime startTime, DateTime stopTime)
    {
        if (!Enabled) return;
        if (startTime > stopTime)
        {
            logger.Warn("Start time greater than stop time for CallStatsLogger");
            return;
        }

        AddCallStat(new CallStat(startTime, stopTime - startTime));
    }

    public static CallStatsLogger GetCallStatsLogger(string name, Type callingClass) =>
        GetCallStatsLogger(name, callingClass.FullName!);

    public static CallStatsLogger GetCallStatsLogger(string name, string prefixName)
    {
        var key = GenKey(prefixName, name);
        if (InstanceTracker.TryGetValue(key, out var getInstance)) return getInstance;
        lock (StaticSyncCntx)
        {
            if (InstanceTracker.TryGetValue(key, out getInstance)) return getInstance;
            getInstance = new CallStatsLogger(name, prefixName);
            InstanceTracker.TryAdd(key, getInstance);
        }

        return getInstance;
    }

    private static string GenKey(string prefixName, string name) => prefixName + "." + name;

    public void QueueCalculationOnThreadPool(List<CallStat> callStats)
    {
        ThreadPool.QueueUserWorkItem(delegate { CalculateStats(callStats); });
    }

    public void CalculateStats(List<CallStat> callStats)
    {
        if (!Enabled) return;
        try
        {
            var earliestTime = callStats[0].StartTime + callStats[0].ExecutionTime;
            var latestTime = callStats[callStats.Count - 1].StartTime + callStats[callStats.Count - 1].ExecutionTime;
            ZeroAllocationArrayTools.QuicksortCallStatTimeSpan(callStats, 0, callStats.Count - 1);
            var quickestCall = callStats[0].ExecutionTime;
            var quickest50ThPercentile = callStats.QuickestPercentile(0.5);
            var quickest90ThPercentile = callStats.QuickestPercentile(0.9);
            var quickest99ThPercentile = callStats.QuickestPercentile(0.99);
            var allCallStats = callStats.QuickestPercentile(1);
            var slowest5ThPercentile = callStats.SlowestPercentile(0.05);

            var batchStats = new CallStatsForBatch(earliestTime, latestTime, quickestCall, quickest50ThPercentile,
                quickest90ThPercentile, quickest99ThPercentile, slowest5ThPercentile, allCallStats);

            logger.Debug("{0}", batchStats);

            callStats.Clear();
            listCallStatPool.Return(callStats);
        }
        catch (Exception e)
        {
            logger.Warn("Error inside of Call Stats Logger {0}", e);
        }
    }
}
