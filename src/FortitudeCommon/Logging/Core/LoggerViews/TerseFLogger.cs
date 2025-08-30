// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core.ActivationProfiles;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;
using static FortitudeCommon.Logging.Core.ActivationProfiles.LoggerActivationFlags;

// ReSharper disable ExplicitCallerInfoArgument

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ITerseFLogger : ILoggerView
{
    [MustUseReturnValue("Use AtLvl if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? AtLvlApnd<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use AtLvl if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? AtLvlFmt(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLvl<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Trc if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? TrcApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Trc if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? TrcFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trc<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Dbg if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? DbgApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Dbg if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? DbgFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Dbg<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Inf if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? InfApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Inf if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? InfFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Inf<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Wrn if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? WrnApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Wrn if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? WrnFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Wrn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Err if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? ErrApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Err if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain? ErrFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    void Err<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
}

public class TerseFLogger(IFLogger logger) : LoggerView(logger), ITerseFLogger
{
    public ISingleInvokeArgumentChain? AtLvlApnd<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? AtLvlFmt(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = ""
      , int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void AtLvl<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    public ISingleInvokeArgumentChain? TrcApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? TrcFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void Trc<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    public ISingleInvokeArgumentChain? DbgApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? DbgFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void Dbg<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    public ISingleInvokeArgumentChain? InfApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? InfFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void Inf<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    public ISingleInvokeArgumentChain? WrnApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? WrnFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void Wrn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    public ISingleInvokeArgumentChain? ErrApnd<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<StringAppenderArgumentChain>()
                         .Initialize(logEntry.StringAppender(style).AppendMatch(firstAppend));
        return null;
    }

    public ISingleInvokeArgumentChain? ErrFmt([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        if (logEntry != null)
            return Logger.LogEntryPool.Recycler.Borrow<FormatParameterArgumentChain>()
                         .Initialize(logEntry.FormatBuilder(formatString, style));
        return null;
    }

    public void Err<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }
}
