// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Runtime.CompilerServices;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.StyledToString;
using static FortitudeCommon.Logging.Core.LoggerActivationFlags;

#endregion

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ISimplifiedFLogger
{
    IFLogFirstFormatterParameterEntry? TraceFormat(string formatString, LoggerActivationFlags activationFlags = DefaultLogger
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IFLogStringAppender? TraceAppend(LoggerActivationFlags activationFlags = DefaultLogger, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    IFLogFirstFormatterParameterEntry? DebugFormat(string formatString, LoggerActivationFlags activationFlags = DefaultLogger
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IFLogStringAppender? DebugAppend(LoggerActivationFlags activationFlags = DefaultLogger, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    IFLogFirstFormatterParameterEntry? InfoFormat(string formatString, LoggerActivationFlags activationFlags = DefaultLogger
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IFLogStringAppender? InfoAppend(LoggerActivationFlags activationFlags = DefaultLogger, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    IFLogFirstFormatterParameterEntry? WarnFormat(string formatString, LoggerActivationFlags activationFlags = DefaultLogger
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IFLogStringAppender? WarnAppend(LoggerActivationFlags activationFlags = DefaultLogger, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);


    IFLogFirstFormatterParameterEntry? ErrorFormat(string formatString, LoggerActivationFlags activationFlags = DefaultLogger
      , StringBuildingStyle style = StringBuildingStyle.Default, [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    IFLogStringAppender? ErrorAppend(LoggerActivationFlags activationFlags = DefaultLogger, StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

}
