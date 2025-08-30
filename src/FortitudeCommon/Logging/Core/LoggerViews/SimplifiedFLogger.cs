// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Core.ActivationProfiles;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;
using static FortitudeCommon.Logging.Core.ActivationProfiles.LoggerActivationFlags;

// ReSharper disable ExplicitCallerInfoArgument

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ISimplifiedFLogger : ILoggerView
{
    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? AtLevelFormat(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? AtLevelAppend<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void AtLevel<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? TraceFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? TraceAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Trace<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? DebugFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? DebugAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Debug<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? InfoFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? InfoAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Info<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? WarnFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? WarnAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Warn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Error if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    IFLogFirstFormatterParameterEntry? ErrorFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use Error if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ErrorAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    void Error<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
}

public class SimplifiedFLogger(IFLogger logger) : LoggerView(logger), ISimplifiedFLogger
{
    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? AtLevelAppend<T>(FLogLevel level, T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? AtLevelFormat(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void AtLevel<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.AtLevel(level, activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? TraceAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? TraceFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void Trace<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Trace(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? DebugAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? DebugFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void Debug<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Debug(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? InfoAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? InfoFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void Info<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Info(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? WarnAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? WarnFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void Warn<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Warn(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }

    [MustUseReturnValue("Use Error if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? ErrorAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile
      , StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.StringAppender(style).AppendMatch(firstAppend);
    }

    [MustUseReturnValue("Use Error if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? ErrorFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        return logEntry?.FormatBuilder(formatString, style);
    }

    public void Error<T>(T toLog, LoggerActivationFlags activationFlags = MergeWithLoggerConfigProfile, StringBuildingStyle style = StringBuildingStyle.Default
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        var logEntry = Logger.Error(activationFlags, memberName, sourceFilePath, sourceLineNumber);
        logEntry?.StringAppender(style).FinalMatchAppend(toLog);
    }
}
