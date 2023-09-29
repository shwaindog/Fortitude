using System;
using System.Diagnostics;
using System.Threading;
using FortitudeCommon.Chronometry;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics
{
    public sealed class GcMonitor : IHierarchialLogger
    {
        private const string NetMemoryPerfCategoryName = ".NET CLR Memory";
        private const string NetMemoryPerfGen0CounterName = "# Gen 0 Collections";
        private const string NetMemoryPerfGen1CounterName = "# Gen 1 Collections";
        private const string NetMemoryPerfGen2CounterName = "# Gen 2 Collections";
        private const string NetMemoryPerfAllBytesInHeapCounterName = "# Bytes in all Heaps";
        private const string NetMemoryPerfPercentageInGcCounterName = "% Time in GC";
        private const string NetMemoryPerfLargeObjectHeapSizeCounterName = "Large Object Heap size";
        private const string NetMemoryPerfPinnedObjectsCounterName = "# of Pinned Objects";
        private const string NetMemoryPerfClrVmCommitedBytesCounterName = "# Total committed Bytes";
        private const string NetInteropPerfCategoryName = ".NET CLR Interop";
        private const string NetInteropPerfComCallableWrapperCounterName = "# of CCWs";
        private const string ProcessPerfCategoryName = "Process";
        private const string ProcessPerfVirtualMemoryCommittedBytesCounterName = "Private Bytes";
        private static IFLogger logger;
        private static volatile GcMonitor singletonInstance;
        private static readonly object SyncRoot = new object();
        private static Action<IHierarchialLogger, string> settingTranslation;
        private readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private int logGenCount = 1;
        private float timeSpentInGc = 20;
        private Thread monitoringThread;

        private GcMonitor(IFLogger logger)
        {
            GcMonitor.logger = logger;
        }

        private void CheckMonitoringThreadStarted()
        {
            if (monitoringThread == null)
            {
                monitoringThread = new Thread(DoGcMonitoring)
                {
                    Name = "GCMonitor",
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal
                };
                logger.Info("Requested Start for GC Notifications");
                monitoringThread.Start();
            }
        }

        public string FullNameOfLogger => "MemoryMonitoring";

        public Action<IHierarchialLogger, string> SettingTranslation
        {
            get
            {
                return settingTranslation ?? (settingTranslation = (hl, setting) =>
                {
                    if (HierarchialConfigurationUpdater.IsActivationKeyWord(setting))
                    {
                        Resume();
                        logger.Info("Will only log GC Gen 1 or 2 collections");
                        logGenCount = 1;
                        return;
                    }

                    if (setting != null)
                    {
                        switch (setting.ToLower())
                        {
                            case "gen0":
                                Resume();
                                logger.Info("Will log GC if any collection has occured");
                                logGenCount = 0;
                                break;
                            case "gen1":
                                Resume();
                                logger.Info("Will only log GC Gen 1 or 2 collections");
                                logGenCount = 1;
                                break;
                            case "gen2":
                                Resume();
                                logger.Info("Will only log GC Gen 2 collections");
                                logGenCount = 2;
                                break;
                            default:
                                var timeSpentInGcCheck = setting.ToLower().Split('.');
                                if (timeSpentInGcCheck.Length == 2 && timeSpentInGcCheck[0] == "timespentingc")
                                {
                                    int percentChange;
                                    if (int.TryParse(timeSpentInGcCheck[1], out percentChange))
                                    {
                                        if (percentChange > 0 && percentChange <= 100)
                                        {
                                            logger.Info(
                                                "Will log GC activity if time spent in GC exceeds {0}% per second",
                                                percentChange);
                                            timeSpentInGc = percentChange;
                                        }
                                    }
                                    Resume();
                                    return;
                                }
                                logger.Info("Pausing GC monitoring until further notice.");
                                Pause();
                                break;
                        }
                    }
                });
            }
        }

        public string DefaultStringValue => "deactivate";

        public bool Enabled { get; set; }
        public event Action<GcNotification> GcNotificationEvent;

        public static GcMonitor GetInstance(IFLogger flogger)
        {
            if (singletonInstance == null)
            {
                lock (SyncRoot)
                {
                    if (singletonInstance == null)
                    {
                        singletonInstance = new GcMonitor(flogger ?? FLoggerFactory.Instance.GetLogger("Memory"));
                    }
                }
            }
            return singletonInstance;
        }

        public void Pause()
        {
            Enabled = false;
            resetEvent.Reset();
        }

        public void Resume()
        {
            CheckMonitoringThreadStarted();
            Enabled = true;
            resetEvent.Set();
        }

        private void DoGcMonitoring()
        {
            try
            {
                var myInstanceName = GetProcessInstanceName(Process.GetCurrentProcess().Id);
                logger.Info("Found Process Perf Counter Instance Name as {0}", myInstanceName);
                FrameworkPerfromanceCounters(myInstanceName);
            }
            catch (Exception e)
            {
                logger?.Error("  ####################   GCMonitor Error  ######################## \r\n {0}", e);
            }
        }
        
        private void FrameworkPerfromanceCounters(string myInstanceName)
        {
            var memoryNumGen0Counter = new PerformanceCounter(NetMemoryPerfCategoryName, NetMemoryPerfGen0CounterName,
                myInstanceName, true);
            var memoryNumGen1Counter = new PerformanceCounter(NetMemoryPerfCategoryName, NetMemoryPerfGen1CounterName,
                myInstanceName, true);
            var memoryNumGen2Counter = new PerformanceCounter(NetMemoryPerfCategoryName, NetMemoryPerfGen2CounterName,
                myInstanceName, true);
            var memoryNumBytesAllHeapsCounter = new PerformanceCounter(NetMemoryPerfCategoryName,
                NetMemoryPerfAllBytesInHeapCounterName, myInstanceName, true);
            var timeSpentAllHeapsCounter = new PerformanceCounter(NetMemoryPerfCategoryName,
                NetMemoryPerfPercentageInGcCounterName, myInstanceName, true);
            var largeObjectHeapSizeCounter = new PerformanceCounter(NetMemoryPerfCategoryName,
                NetMemoryPerfLargeObjectHeapSizeCounterName, myInstanceName, true);
            var clrCommitedBytesCounter = new PerformanceCounter(NetMemoryPerfCategoryName,
                NetMemoryPerfClrVmCommitedBytesCounterName, myInstanceName, true);
            var clrPinnedObjectsCounter = new PerformanceCounter(NetMemoryPerfCategoryName,
                NetMemoryPerfPinnedObjectsCounterName, myInstanceName, true);
            var comCallableWrapperCounter = new PerformanceCounter(NetInteropPerfCategoryName,
                NetInteropPerfComCallableWrapperCounterName, myInstanceName, true);
            var totalCommitedBytesCounter = new PerformanceCounter(ProcessPerfCategoryName,
                ProcessPerfVirtualMemoryCommittedBytesCounterName, myInstanceName, true);

            float prevGen0FloatCount = 0;
            float prevGen1FloatCount = 0;
            float prevGen2FloatCount = 0;
            float prevCommitedBytesCount = 0;

            const int logAtMemoryGrowth = 10*1024*1024;

            var fromTime = TimeContext.UtcNow;
            var errorCount = 0;
            while (resetEvent.WaitOne() && errorCount < 5)
            {
                try
                {
                    var gen0Count = memoryNumGen0Counter.NextValue();
                    var gen1Count = memoryNumGen1Counter.NextValue();
                    var gen2Count = memoryNumGen2Counter.NextValue();
                    var memoryAllHeaps = memoryNumBytesAllHeapsCounter.NextValue();
                    var timePercentageSpentInGc = timeSpentAllHeapsCounter.NextValue();
                    var clrCommitedBytes = clrCommitedBytesCounter.NextValue();
                    var clrPinnedObjects = clrPinnedObjectsCounter.NextValue();
                    var comCallableWrapper = comCallableWrapperCounter.NextValue();
                    var totalCommitedBytes = totalCommitedBytesCounter.NextValue();

                    var largeObjectHeapSize = largeObjectHeapSizeCounter.NextValue();

                    var toTime = TimeContext.UtcNow;

                    if ((prevGen0FloatCount < gen0Count && logGenCount == 0)
                        || (prevGen1FloatCount < gen1Count && logGenCount <= 1)
                        || (prevGen2FloatCount < gen2Count && logGenCount <= 2)
                        || timePercentageSpentInGc > timeSpentInGc
                        || totalCommitedBytes > prevCommitedBytesCount + logAtMemoryGrowth
                        || totalCommitedBytes < prevCommitedBytesCount - logAtMemoryGrowth)
                    {
                        var gcTime =
                            TimeSpan.FromTicks((long) (TimeSpan.FromSeconds(1).Ticks*timePercentageSpentInGc/100));

                        GcNotificationEvent?.Invoke(new GcNotification(fromTime, toTime, memoryAllHeaps, gen0Count,
                            gen1Count, gen2Count, gcTime,
                            largeObjectHeapSize));
                        logger?.Debug(
                            $"GC AllHeapsMemory='{memoryAllHeaps:N0}' B,Gen0Count='{gen0Count}',Gen1Count='{gen1Count}'," +
                            $"Gen2Count='{gen2Count}',TimeSpentInGC='{gcTime.TotalMilliseconds:N6}' ms in 1s, " +
                            $"LargeObjectHeapSize='{largeObjectHeapSize:N0}' B, " +
                            $"ClrCommitedBytes='{clrCommitedBytes:N0}' B, " +
                            $"TotalProcessPrivateBytes='{totalCommitedBytes:N0}' ClrPinnedObjects='{clrPinnedObjects:N0}', " +
                            $"COMCallableWrappers='{comCallableWrapper:N0}'. " +
                            $"Sample taken from period {fromTime:HH\\:mm\\:ss.ffffff} to {toTime:HH\\:mm\\:ss.ffffff} ");
                    }
                    var execTime = toTime - fromTime;
                    prevCommitedBytesCount = totalCommitedBytes;
                    prevGen0FloatCount = gen0Count;
                    prevGen1FloatCount = gen1Count;
                    prevGen2FloatCount = gen2Count;
                    Thread.Sleep(
                        TimeSpan.FromTicks(Math.Max(TimeSpan.FromSeconds(0.1).Ticks,
                            (TimeSpan.FromSeconds(0.99) - execTime).Ticks)));
                    fromTime = TimeContext.UtcNow;
                }
                catch (InvalidOperationException ioe)
                {
                    logger.Error("{0} Error getting process counters.  Got {1}", ++errorCount, ioe);
                    Thread.Sleep(errorCount*1000);
                }
            }
        }
        

        public float CalculateSample(CounterSample current, ref CounterSample old)
        {
            var retVal = CounterSample.Calculate(old, current);
            old = current;

            return retVal;
        }

        private static string GetProcessInstanceName(int pid)
        {
            var cat = new PerformanceCounterCategory("Process");

            var instances = cat.GetInstanceNames();
            foreach (var instance in instances)
            {
                try
                {
                    using (var cnt = new PerformanceCounter("Process",
                        "ID Process", instance, true))
                    {
                        var val = (int) cnt.RawValue;
                        if (val == pid)
                        {
                            return instance;
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Debug(
                        "Non-critical problem whilst anaylising performance counter '{0}' to find if this performance counter name has our process ID. {0}",
                        instance, e);
                }
            }
            throw new Exception("Could not find performance counter " +
                                "instance name for current process. This is truly strange ...");
        }

        public struct GcNotification
        {
            public readonly float AllHeapsTotalMemory;
            public readonly DateTime FromTime;
            public readonly float Gen0CollectionCount;
            public readonly float Gen1CollectionCount;
            public readonly float Gen2CollectionCount;
            public readonly float LargeObjectHeapSize;
            public readonly TimeSpan TimeInGc;
            public readonly DateTime ToTime;

            public GcNotification(DateTime from,
                DateTime to,
                float allHeapsTotalMemory,
                float gen0Count,
                float gen1Count,
                float gen2Count,
                TimeSpan timeInGc,
                float largeObjectHeapSize)
            {
                FromTime = from;
                ToTime = to;
                AllHeapsTotalMemory = allHeapsTotalMemory;
                Gen0CollectionCount = gen0Count;
                Gen1CollectionCount = gen1Count;
                Gen2CollectionCount = gen2Count;
                TimeInGc = timeInGc;
                LargeObjectHeapSize = largeObjectHeapSize;
            }
        }
    }
}