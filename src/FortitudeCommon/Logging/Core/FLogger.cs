using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.ConditionalLogging;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;

[Flags]
public enum LoggerActivationFlags : uint
{
    Disabled                    = 0x00_00_00_00
  , WhenNoTrace                 = 0x00_00_00_00
  , WhenTrace                   = 0x00_00_00_00
  , WhenDebugProfile            = 0x00_00_00_00
  , WhenReleaseProfile          = 0x00_00_00_00
  , WhenPerfTestProfile         = 0x00_00_00_00
  , WhenNoPerfTestProfile       = 0x00_00_00_00
  , WhenNoStopWatch             = 0x00_00_00_00
  , WhenStopWatch               = 0x00_00_00_00
  , DefaultLogger               = 0x00_00_00_00
  , EveryTime                   = 0x00_00_00_00
  , PerLocationAttemptInterval  = 0x00_00_00_00
  , LoggerGlobalAttemptInterval = 0x00_00_00_00
  , AddSkipCount                = 0x00_00_00_00
  , PerLocationTimeInterval     = 0x00_00_00_00
  , GlobalLoggerTimeInterval    = 0x00_00_00_00
  , DefaultTimingLogger         = 0x00_00_00_00

  , PerLocationPercentilesAtInterval = 0x00_00_00_00
}

public delegate void NotifyLogEntryDispatched(IFLogEntry logEntry);

public interface IFLogger : IFLoggerDescendant
{
    FLoggerDebugBuild? DebugBuildOnlyLogger { get; }

    IFLoggerExecutionDuration FLoggerExecutionDuration { get; }

    event NotifyLogEntryDispatched? LogEntryDispatched;

    void NotifyLogEntryDispatched(IFLogEntry fLogEntry);

    IMutableFLogEntry? AtLevelWithStaticFilter<T>
    (FLogLevel level, T filterContext, [RequireStaticDelegate] Func<T, IFLogger, LoggingLocation, bool> continuePredicate
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? AtLevelWithClosureFilter<T>
    (FLogLevel level, T filterContext, Func<T, IFLogger, LoggingLocation, bool> continuePredicate, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? AtLevel
    (FLogLevel level, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Trace<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Trace
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfTrace(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger);

    IMutableFLogEntry? Debug<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Debug
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfDebug(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger);

    IMutableFLogEntry? Info<T>
    (T filterContext,
        LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Info
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfInfo(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger);

    IMutableFLogEntry? Warn<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Warn
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfWarn(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger);

    IMutableFLogEntry? Error<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? Error
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IMutableFLogEntry? PerfError(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger);

    IReadOnlyList<Type> ReceivedTypes { get; }

    ISystemTraceBuilder GlobalTraceBuilder();
}

public interface IMutableFLogger : IFLogger, IMutableFLoggerDescendant { }

public interface IGlobalTraceContext : IDisposable { }

public interface ISystemTraceBuilder { }

public class FLogger : FLoggerDescendant, IMutableFLogger
{
    private static readonly LoggingLocation PerfLoggingLocation = LoggingLocation.NonePerfLoggingUsed;

    private readonly List<Type> receivedTypes = new();

    internal readonly ForwardLogEntry ForwardToCallback;

    private uint timingDurationSeqNum;

    private FLoggerDebugBuild? debugBuildOnlyLogger;


    public event NotifyLogEntryDispatched? LogEntryDispatched;

    private IFLoggerExecutionDuration? executionDurationFLogger = null;


    public FLoggerDebugBuild? DebugBuildOnlyLogger => debugBuildOnlyLogger;

    internal uint GetNextTimingDurationSequenceEvent() => Interlocked.Increment(ref timingDurationSeqNum);

    public IFLoggerExecutionDuration FLoggerExecutionDuration => executionDurationFLogger ??= new FLoggerExecutionDuration(this);


    [Conditional("DEBUG")]
    public void InitializeDebugLogger()
    {
        debugBuildOnlyLogger ??= new FLoggerDebugBuild(this);
    }

    public void NotifyLogEntryDispatched(IFLogEntry fLogEntry)
    {
        LogEntryDispatched?.Invoke(fLogEntry);
    }

    public FLogger(IFLoggerDescendantConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLogLoggerRegistry loggerRegistry)
        : base(loggerConsolidatedConfig, myParent, loggerRegistry)
    {
        loggerRegistry.RegisterLoggerCallback(this);
        ForwardToCallback = ForwardLogEntryTo;
    }

    public IMutableFLogEntry? AtLevelWithStaticFilter<T>
    (FLogLevel level, T filterContext
      , [RequireStaticDelegate] Func<T, IFLogger, LoggingLocation, bool> continuePredicate
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntry        = LogEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevelWithClosureFilter<T>
    (FLogLevel level, T filterContext
      , Func<T, IFLogger, LoggingLocation, bool> continuePredicate
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntry        = LogEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevel
    (FLogLevel level, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }


    public IMutableFLogEntry? Trace<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Trace < LogLevel) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Trace
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Trace < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfTrace(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger)
    {
        if (FLogLevel.Trace < LogLevel) return null;

        var logEntryLocation = PerfLoggingLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Debug < LogLevel) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Debug < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfDebug(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger)
    {
        if (FLogLevel.Debug < LogLevel) return null;

        var logEntryLocation = PerfLoggingLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Info < LogLevel) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Info < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfInfo(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger)
    {
        if (FLogLevel.Info < LogLevel) return null;

        var logEntryLocation = PerfLoggingLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Warn < LogLevel) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Warn < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfWarn(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger)
    {
        if (FLogLevel.Warn < LogLevel) return null;

        var logEntryLocation = PerfLoggingLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error<T>
    (T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger,
        [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Error < LogLevel) return null;

        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error
    (LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Error < LogLevel) return null;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? PerfError(LoggerActivationFlags activationFlags = LoggerActivationFlags.DefaultLogger)
    {
        if (FLogLevel.Error < LogLevel) return null;

        var logEntryLocation = PerfLoggingLocation;
        var logEntry         = LogEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, ForwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }


    public IReadOnlyList<Type> ReceivedTypes => receivedTypes.AsReadOnly();

    public ISystemTraceBuilder GlobalTraceBuilder() => throw new NotImplementedException();


    private void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        foreach (var appender in Appenders)
        {
            appender.ForwardLogEntryTo(logEntry);
        }
    }
}
