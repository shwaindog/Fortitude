using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.Pooling;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;


public interface IFLogger : IFLoggerDescendant
{
    IMutableFLogEntry? AtLevelWithStaticFilter<T>(FLogLevel level, T filterContext,[RequireStaticDelegate] Func<T, IFLogger, LoggingLocation, bool> continuePredicate, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? AtLevelWithClosureFilter<T>(FLogLevel level, T filterContext, Func<T, IFLogger, LoggingLocation, bool> continuePredicate, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? AtLevel<T>(FLogLevel level, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Trace<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Trace( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Debug<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Debug( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Info<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Info( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Warn<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Warn( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Error<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);
    IMutableFLogEntry? Error( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0);

    IReadOnlyList<Type> ReceivedTypes { get; }

    ISystemTraceBuilder GlobalTraceBuilder();
}
public interface IMutableFLogger : IFLogger, IMutableFLoggerDescendant
{
}

public interface IGlobalTraceContext : IDisposable
{


}

public interface ISystemTraceBuilder
{

}

public class FLogger : FLoggerDescendant, IMutableFLogger
{
    private readonly List<Type>      receivedTypes = new();

    private readonly FLogEntryPool   logEntryPool;
    private readonly ForwardLogEntry forwardToCallback;

    public FLogger(ConsolidatedLoggerConfig loggerConsolidatedConfig, IFLoggerCommon myParent, IFLoggerRegistry loggerRegistry) : base(loggerConsolidatedConfig, myParent)
    {
        loggerRegistry.RegisterLoggerCallback(this);
        logEntryPool = loggerRegistry.SourceFLogEntryPool(loggerConsolidatedConfig.LogEntryPool);
        forwardToCallback = ForwardLogEntryTo;
    }

    public IMutableFLogEntry? AtLevelWithStaticFilter<T>
        (FLogLevel level, T filterContext,[RequireStaticDelegate] Func<T, IFLogger, LoggingLocation, bool> continuePredicate
          , [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null!;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntry        = logEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevelWithClosureFilter<T>
    (FLogLevel level, T filterContext, Func<T, IFLogger, LoggingLocation, bool> continuePredicate
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        if (!continuePredicate(filterContext, this, logEntryLocation)) return null!;
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntry        = logEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? AtLevel<T>
    (FLogLevel level, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (level < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry        = logEntryPool.SourceLogEntry();
        var logEntryContext = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Trace<T>
    (T filterContext, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Trace < LogLevel) return null!;
        
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Trace( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0) 
    {
        if (FLogLevel.Trace < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Debug < LogLevel) return null!;
        
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Debug( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Debug < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Info < LogLevel) return null!;
        
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Info( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Info < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Warn < LogLevel) return null!;
        
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Warn( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Warn < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error<T>(T filterContext, [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Error < LogLevel) return null!;
        
        var receivedType = typeof(T);
        if (!receivedTypes.Contains(receivedType))
        {
            receivedTypes.Add(receivedType);
        }
        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
        logEntry.Initialize(logEntryContext);
        return logEntry;
    }

    public IMutableFLogEntry? Error( [CallerMemberName] string memberName = "" , [CallerFilePath] string sourceFilePath = "" , [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (FLogLevel.Error < LogLevel) return null!;

        var logEntryLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);
        var logEntry         = logEntryPool.SourceLogEntry();
        var logEntryContext  = new LoggerEntryContext(this, forwardToCallback, logEntryLocation, LogLevel);
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
