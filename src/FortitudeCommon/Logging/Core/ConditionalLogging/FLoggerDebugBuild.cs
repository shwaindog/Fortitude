// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.ConditionalLogging;

public class DebugBuildPopulateIfActive : RecyclableObject
{
    public IMutableFLogEntry? LogEntry;

    public override void StateReset()
    {
        LogEntry = null;
        base.StateReset();
    }

    public void CheckEntryIsMineAndDecrement(IFLogEntry fLogEntry)
    {
        if (ReferenceEquals(LogEntry, fLogEntry))
        {
            LogEntry.Logger.LogEntryDispatched -= CheckEntryIsMineAndDecrement;
            DecrementRefCount();
        }
    }
}

public class FLoggerDebugBuild(IFLogger wrappingLogger)
{
    // ReSharper disable ExplicitCallerInfoArgument
    private static readonly IRecycler Recycler = new Recycler();

    [ThreadStatic] private static DebugBuildPopulateIfActive? nextToken;

    public DebugBuildPopulateIfActive? ClaimToken()
    {
        return nextToken ??= Recycler.Borrow<DebugBuildPopulateIfActive>();
    }

    [Conditional("DEBUG")]
    public void DebugBuildAtLevelWithStaticFilter<T>
    (DebugBuildPopulateIfActive? populateIfActive, FLogLevel level, T filterContext, Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.AtLevelWithStaticFilter(level, filterContext, continuePredicate, activationFlags
                                                            , memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildAtLevelWithClosureFilter<T>
    (DebugBuildPopulateIfActive? populateIfActive, FLogLevel level, T filterContext, Func<T, IFLogger, FLogCallLocation, bool> continuePredicate
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.AtLevelWithClosureFilter(level, filterContext, continuePredicate, activationFlags,
                                                               memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildAtLevel
    (DebugBuildPopulateIfActive? populateIfActive, FLogLevel level, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildTrace<T>
    (DebugBuildPopulateIfActive? populateIfActive, T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Trace(filterContext, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildTrace
    (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildPerfTrace
        (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled)
    {
        var logEntry = wrappingLogger.PerfTrace(activationFlags);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildDebug<T>
    (DebugBuildPopulateIfActive? populateIfActive, T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Debug(filterContext, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildDebug
    (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildPerfDebug
        (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled)
    {
        var logEntry = wrappingLogger.PerfDebug(activationFlags);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildInfo<T>
    (DebugBuildPopulateIfActive? populateIfActive, T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Info(filterContext, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildInfo
    (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildPerfInfo
        (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled)
    {
        var logEntry = wrappingLogger.PerfInfo(activationFlags);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildWarn<T>
    (DebugBuildPopulateIfActive? populateIfActive, T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildWarn
    (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildPerfWarn
        (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled)
    {
        var logEntry = wrappingLogger.PerfWarn(activationFlags);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildError<T>
    (DebugBuildPopulateIfActive? populateIfActive, T filterContext, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Error(filterContext, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildError
    (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = wrappingLogger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    [Conditional("DEBUG")]
    public void DebugBuildPerfError
        (DebugBuildPopulateIfActive? populateIfActive, LoggerActivationFlags activationFlags = LoggerActivationFlags.Disabled)
    {
        var logEntry = wrappingLogger.PerfError(activationFlags);
        if (populateIfActive != null && logEntry != null)
        {
            logEntry.Logger.LogEntryDispatched += populateIfActive.CheckEntryIsMineAndDecrement;

            nextToken = null;

            populateIfActive.LogEntry = logEntry;
        }
    }

    public ISystemTraceBuilder GlobalTraceBuilder() => throw new NotImplementedException();

    public IReadOnlyList<Type> ReceivedTypes => throw new NotImplementedException();
    // ReSharper restore ExplicitCallerInfoArgument
}
