// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.ConditionalLogging;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using FortitudeCommon.Logging.Core.LoggerViews;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;

public delegate void NotifyLogEntryDispatched(IFLogEntry logEntry);

public interface IFLogger : IFLoggerDescendant, ISwitchFLoggerView
{
    FLoggerDebugBuild? DebugBuildOnlyLogger { get; }

    IFLogEntryRootPublisher PublishEndpoint { get; }

    IFLoggerExecutionDuration FLoggerExecutionDuration { get; }

    IReadOnlyList<Type> ReceivedTypes { get; }

    event NotifyLogEntryDispatched? LogEntryDispatched;

    void NotifyLogEntryDispatched(IFLogEntry fLogEntry);

    IMutableFLogEntry? AtLevelWithStaticFilter<T>
    (FLogLevel level, T filterContext, [RequireStaticDelegate] Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? AtLevelWithClosureFilter<T>
    (FLogLevel level, T filterContext, Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? AtLevel
    (FLogLevel level, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Trace<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Trace
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfTrace(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig);

    IMutableFLogEntry? Debug<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Debug
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfDebug(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig);

    IMutableFLogEntry? Info<T>
    (T filterContext,
        LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Info
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfInfo(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig);

    IMutableFLogEntry? Warn<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Warn
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfWarn(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig);

    IMutableFLogEntry? Error<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Error
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfError(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig);

    ISystemTraceBuilder GlobalTraceBuilder();
}

public interface IMutableFLogger : IFLogger, IMutableFLoggerDescendant { }

public interface IGlobalTraceContext : IDisposable { }

public interface ISystemTraceBuilder { }

public class FLogger : FLoggerDescendant, IMutableFLogger
{
    private static readonly FLogCallLocation PerfFLogCallLocation = FLogCallLocation.NonePerfFLogCallUsed;

    internal readonly IFLogEntrySink ForwardToCallback;

    private readonly List<Type> receivedTypes = new();

    private List<ILoggerView>? createdViews;

    private IFLoggerExecutionDuration? executionDurationFLogger;

    private uint timingDurationSeqNum;

    public FLogger(IMutableFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
        : base(loggerConsolidatedConfig, myParent, loggerRegistry)
    {
        loggerRegistry.RegisterLoggerCallback(this);
        ForwardToCallback = new ForwardToAppendersSink(this);
        PublishEndpoint =
            new FLogEntryPipelineEndpoint(FullName, ForwardToCallback, FLogEntrySourceSinkType.Source, FLogEntryProcessChainState.Active);
    }
    
    public event NotifyLogEntryDispatched? LogEntryDispatched;

    public FLoggerDebugBuild? DebugBuildOnlyLogger { get; private set; }

    public IFLoggerExecutionDuration FLoggerExecutionDuration => executionDurationFLogger ??= new FLoggerExecutionDuration(this);

    public IFLogEntryRootPublisher PublishEndpoint { get; }


    public T As<T>() where T : ISwitchFLoggerView
    {
        if (this is T asFloggerView) return asFloggerView;
        createdViews ??= new List<ILoggerView>();
        for (var i = 0; i < createdViews.Count; i++)
            if (createdViews[i] is T requestedView)
                return requestedView;
        var view = FLogCreate.MakeFLoggerView(this, typeof(T));

        createdViews.Add((ILoggerView)view);
        return (T)view;
    }

    public void NotifyLogEntryDispatched(IFLogEntry fLogEntry)
    {
        LogEntryDispatched?.Invoke(fLogEntry);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsLoggingDeactivated(FLogLevel level, LoggerActivationFlags activationFlags)
    {
        activationFlags = activationFlags.MergeWithConfigIfAllowed(ConfigActivationProfile);
        if (LogLevel.IsLevelDisabled(level) && activationFlags.LogLevelExclusionsEnabled()) return true;
        if (activationFlags.HasActivationExclusion(CurrentFLoggerExecutionEnvironment)) return true;

        return false;
    }

    public IMutableFLogEntry? AtLevelWithStaticFilter<T>
    (FLogLevel level, T filterContext, [RequireStaticDelegate] Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(level, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntry        = LogEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, level);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevelWithClosureFilter<T>
    (FLogLevel level, T filterContext, Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(level, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntry        = LogEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, level);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevel
    (FLogLevel level, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(level, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, level);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }


    public IMutableFLogEntry? Trace<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Trace, activationFlags)) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Trace);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Trace
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Trace, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Trace);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfTrace(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig)
    {
        if (IsLoggingDeactivated(FLogLevel.Trace, activationFlags)) return null;

        var logEntryLocation = PerfFLogCallLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Trace);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Debug, activationFlags)) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Debug);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Debug, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Debug);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfDebug(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig)
    {
        if (IsLoggingDeactivated(FLogLevel.Debug, activationFlags)) return null;

        var logEntryLocation = PerfFLogCallLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Debug);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Info, activationFlags)) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Info);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Info, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Info);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfInfo(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig)
    {
        if (IsLoggingDeactivated(FLogLevel.Info, activationFlags)) return null;

        var logEntryLocation = PerfFLogCallLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Info);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Warn, activationFlags)) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Warn);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Warn, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Warn);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfWarn(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig)
    {
        if (IsLoggingDeactivated(FLogLevel.Warn, activationFlags)) return null;

        var logEntryLocation = PerfFLogCallLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Warn);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Error, activationFlags)) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType)) receivedTypes.Add(receivedType);
        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Error);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (IsLoggingDeactivated(FLogLevel.Error, activationFlags)) return null;

        var logEntryLocation = new FLogCallLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Error);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfError(LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig)
    {
        if (IsLoggingDeactivated(FLogLevel.Error, activationFlags)) return null;

        var logEntryLocation = PerfFLogCallLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, PublishEndpoint, logEntryLocation, FLogLevel.Error);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }


    public IReadOnlyList<Type> ReceivedTypes => receivedTypes.AsReadOnly();

    public ISystemTraceBuilder GlobalTraceBuilder() => throw new NotImplementedException();

    internal uint GetNextTimingDurationSequenceEvent() => Interlocked.Increment(ref timingDurationSeqNum);

    [Conditional("DEBUG")]
    public void InitializeDebugLogger()
    {
        DebugBuildOnlyLogger ??= new FLoggerDebugBuild(this);
    }

    private class ForwardToAppendersSink(FLogger logger) : FLogEntrySinkBase
    {
        private IReadOnlyList<IAppenderClient>? appenderCache;

        public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;
        public override FLogEntryProcessChainState LogEntryProcessState
        {
            get => FLogEntryProcessChainState.Active;
            protected set => _ = value;
        }
        public override string Name
        {
            get => logger.FullName;
            protected set => _ = value;
        }

        public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
        {
            var appendersCount = logger.Appenders.Count;
            if (appenderCache == null || logger.AppendersUpdatedFlag || appenderCache.Count != appendersCount)
                lock (logger.Appenders)
                {
                    if (appenderCache == null || logger.AppendersUpdatedFlag || appenderCache.Count != appendersCount)
                    {
                        var buildCache = new List<IAppenderClient>(appendersCount);
                        for (var i = 0; i < appendersCount; i++) buildCache.Add(logger.Appenders[i]);
                        logger.AppendersUpdatedFlag = false;

                        appenderCache = buildCache;
                    }
                }
            for (var i = 0; i < appendersCount; i++)
            {
                var appender = appenderCache[i];
                appender.PublishLogEntryEvent(logEntryEvent);
            }

            logEntryEvent.DecrementRefCount();
            // Console.Out.WriteLine("Post Logger " + logEntryEvent.LogEntry);
        }
    }
}
