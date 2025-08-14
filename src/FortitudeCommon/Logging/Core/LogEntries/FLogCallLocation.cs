// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries;

public record struct FLogCallLocation(string MemberName, string SourceFilePath, int SourceLineNumber)
{
    public const string PerfLogging = "NonePerfLogging";
    public const string AppenderAlertLogging = "AppenderAlert";

    public static readonly FLogCallLocation NoneInternalCall = new (PerfLogging, PerfLogging, 0);
    public static readonly FLogCallLocation NonePerfFLogCallUsed = new (PerfLogging, PerfLogging, 0);
    public static readonly FLogCallLocation NoneAppenderAlertMessage = new (AppenderAlertLogging, AppenderAlertLogging, 0);
}

public static class FLogCallLocationExtensions
{
    public static StructStyler<FLogCallLocation> FLogCallLocationFormatter = FormatFlogLevelAppender;

    public static StructStyler<FLogCallLocation> Styler(this FLogCallLocation callLoc) => FLogCallLocationFormatter;


    public static void FormatFlogLevelAppender(this FLogCallLocation callLoc, IStyledTypeStringAppender sbc)
    {
        sbc.StartComplexType(nameof(FLogCallLocation))
           .Field.AlwaysAdd(nameof(callLoc.MemberName), callLoc.MemberName)
           .Field.AlwaysAdd(nameof(callLoc.SourceLineNumber), callLoc.SourceLineNumber)
           .Field.AlwaysAdd(nameof(callLoc.SourceFilePath), callLoc.SourceFilePath)
           .Complete();
    }


    public static StringBuilder ExtractAppendFileName(this StringBuilder sb, FLogCallLocation subject)
    {
        string sourceFilePath      = subject.SourceFilePath;

        var sourcePathSpan      = sourceFilePath.AsSpan();
        var indexOfForwardSlash = sourcePathSpan.LastSplitFrom('/');
        if (indexOfForwardSlash.Length > 0 && indexOfForwardSlash.Length < sourcePathSpan.Length)
        {
            sb.Append(indexOfForwardSlash);
            return sb;
        }
        var indexOfBackSlash = sourcePathSpan.LastSplitFrom('\\');
        if (indexOfBackSlash.Length > 0  && indexOfBackSlash.Length < sourcePathSpan.Length)
        {
            sb.Append(indexOfBackSlash);
            return sb;
        }
        sb.Append(sourceFilePath);
        return sb;
    }

    public static StringBuilder ExtractAppendLineNumber(this StringBuilder sb, FLogCallLocation subject)
    {
        sb.Append(subject.SourceLineNumber);
        return sb;
    }

    public static StringBuilder AppendFileNameLineNumber(this StringBuilder sb, FLogCallLocation subject)
    {
        return sb.ExtractAppendFileName(subject).Append(": ").ExtractAppendLineNumber(subject);
    }
}
