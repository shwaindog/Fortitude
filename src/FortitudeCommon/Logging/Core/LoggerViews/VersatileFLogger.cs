// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.StringsOfPower.Options;
using JetBrains.Annotations;

// ReSharper disable ExplicitCallerInfoArgument

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface IVersatileFLogger : ISimplifiedFLogger, ITerseFLogger { }

public class VersatileFLogger(IFLogger logger) : TerseFLogger(logger), IVersatileFLogger
{
    private readonly ISimplifiedFLogger simplified = logger.As<ISimplifiedFLogger>();

    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? AtLevelAppend<T>(FLogLevel level, T firstAppend
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.AtLevelAppend(level, firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use AtLevel if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? AtLevelFormat(FLogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.AtLevelFormat(level, formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void AtLevel<T>(FLogLevel level, T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.AtLevel(level, toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? TraceAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.TraceAppend(firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Trace if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? TraceFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.TraceFormat(formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void Trace<T>(T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.Trace(toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? DebugAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.DebugAppend(firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Debug if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? DebugFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.DebugFormat(formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void Debug<T>(T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.Debug(toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? InfoAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.InfoAppend(firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Info if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? InfoFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.InfoFormat(formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void Info<T>(T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.Info(toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? WarnAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.WarnAppend(firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? WarnFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.WarnFormat(formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void Warn<T>(T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.Warn(toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Error if you do not plan on using the returned IFLogStringAppender")]
    public IFLogStringAppender? ErrorAppend<T>(T firstAppend, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.ErrorAppend(firstAppend, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    [MustUseReturnValue("Use Warn if you do not plan on using the returned IFLogFirstFormatterParameterEntry")]
    public IFLogFirstFormatterParameterEntry? ErrorFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString
      , LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig, StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.ErrorFormat(formatString, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);

    public void Error<T>(T toLog, LoggerActivationFlags activationFlags = LoggerActivationFlags.MergeLoggerActivationConfig
      , StringStyle style = StringStyle.CompactLog
      , string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) =>
        simplified.Error(toLog, activationFlags, style, memberName, sourceFilePath, sourceLineNumber);
}
