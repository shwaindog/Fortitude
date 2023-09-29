using System;
using System.Collections.Generic;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance
{
    public class NoOpPerfLogger : IPerfLogger
    {
        private static volatile NoOpPerfLogger instance;

        private NoOpPerfLogger()
        {
            LastCallStat = new CallStat();
        }

        public static NoOpPerfLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof (NoOpPerfLogger))
                    {
                        if (instance == null)
                        {
                            instance = new NoOpPerfLogger();
                        }
                    }
                }
                return instance;
            }
        }

        public string FullNameOfLogger => "NoOpPerfLogger";

        public Action<IHierarchialLogger, string> SettingTranslation
        {
            get { return (hl, str) => { }; }
        }

        public string DefaultStringValue { get; } = "NoOpPerfLogger";

        public bool Enabled
        {
            get { return false; }
            set { }
        }

        public void Start()
        {
        }

        public void Add<TU>(string identifier, TU subject)
        {
        }

        public void Add(string identifier)
        {
        }

        public void Indent()
        {
        }

        public void Dedent()
        {
        }

        public LatencyLoggingLevel LatencyLoggingLevel
        {
            get { return LatencyLoggingLevel.None; }
            set { }
        }

        public CallStat LastCallStat { get; private set; }

        public DateTime StartTime { get; } = DateTime.MinValue;

        public DateTime StopTime { get; } = DateTime.MaxValue;

        public TimeSpan MaxAcceptablePeriod
        {
            get { return TimeSpan.MaxValue; }
            set { }
        }

        public void StartTiming()
        {
        }

        public void StopTiming<T, TU, TV>(T optionalData, TU moreOptionalData, TV evenMoreOptionalData)
        {
        }

        public void StopTiming<T, TU>(T optionalData, TU moreOptionalData)
        {
        }

        public void StopTiming<T>(T optionalData = default(T))
        {
        }

        public void StopTiming(string optionalData = null)
        {
        }

        public bool WriteTrace
        {
            get { return false; }
            set { }
        }

        public bool DefaultWriteTrace
        {
            get { return false; }
            set { }
        }

        public List<TraceEntry> TraceFinished()
        {
            return new List<TraceEntry>(0);
        }

        public int BatchSize
        {
            get { return -1; }
            set { }
        }

        public void AddCallStat(CallStat lastCallStat)
        {
        }

        public void AddCallStat(DateTime startTime, DateTime stopTime)
        {
        }

        public void AddContextMeasurement(double measurement)
        {
        }
    }
}