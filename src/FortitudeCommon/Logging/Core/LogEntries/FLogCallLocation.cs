// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Logging.Core.LogEntries;

public record struct FLogCallLocation(string MemberName, string SourceFilePath, int SourceLineNumber)
{
    public const string PerfLogging          = "NonePerfLogging";
    public const string AppenderAlertLogging = "AppenderAlert";

    public static readonly FLogCallLocation NoneInternalCall         = new(PerfLogging, PerfLogging, 0);
    public static readonly FLogCallLocation NonePerfFLogCallUsed     = new(PerfLogging, PerfLogging, 0);
    public static readonly FLogCallLocation NoneAppenderAlertMessage = new(AppenderAlertLogging, AppenderAlertLogging, 0);
}

public static class FLogCallLocationExtensions
{
    public static StringBearerRevealState<FLogCallLocation> FLogCallLocationStyler = FormatFlogLevelAppender;

    public static StringBearerRevealState<FLogCallLocation> Styler(this FLogCallLocation callLoc) => FLogCallLocationStyler;


    public static StateExtractStringRange FormatFlogLevelAppender(this FLogCallLocation callLoc, ITheOneString sbc) =>
        sbc.StartComplexType(callLoc)
           .Field.AlwaysAdd(nameof(callLoc.MemberName), callLoc.MemberName)
           .Field.AlwaysAdd(nameof(callLoc.SourceLineNumber), callLoc.SourceLineNumber)
           .Field.AlwaysAdd(nameof(callLoc.SourceFilePath), callLoc.SourceFilePath)
           .Complete();


    public static IStringBuilder ExtractAppendFileNameWithExtension(this IStringBuilder sb, FLogCallLocation subject)
    {
        var sourceFilePath = subject.SourceFilePath;

        var sourcePathSpan      = sourceFilePath.AsSpan();
        var indexOfForwardSlash = sourcePathSpan.LastSplitFrom('/');
        if (indexOfForwardSlash.Length > 0 && indexOfForwardSlash.Length < sourcePathSpan.Length)
        {
            sb.Append(indexOfForwardSlash);
            return sb;
        }
        var indexOfBackSlash = sourcePathSpan.LastSplitFrom('\\');
        if (indexOfBackSlash.Length > 0 && indexOfBackSlash.Length < sourcePathSpan.Length)
        {
            sb.Append(indexOfBackSlash);
            return sb;
        }
        sb.Append(sourceFilePath);
        return sb;
    }

    public static IStringBuilder ExtractAppendFileNameNoExt(this IStringBuilder sb, FLogCallLocation subject)
    {
        var sourceFilePath = subject.SourceFilePath;

        var sourcePathSpan      = sourceFilePath.AsSpan();
        var indexOfForwardSlash = sourcePathSpan.LastSplitFrom('/');
        if (indexOfForwardSlash.Length > 0 && indexOfForwardSlash.Length < sourcePathSpan.Length)
        {
            var indexOfdot = sourcePathSpan.LastIndexOf('.');
            sb.Append(indexOfdot > 0 ? indexOfForwardSlash[..indexOfdot] : indexOfForwardSlash);
            return sb;
        }
        var indexOfBackSlash = sourcePathSpan.LastSplitFrom('\\');
        if (indexOfBackSlash.Length > 0 && indexOfBackSlash.Length < sourcePathSpan.Length)
        {
            var indexOfdot = sourcePathSpan.LastIndexOf('.');
            sb.Append(indexOfdot > 0 ? indexOfBackSlash[..indexOfdot] : indexOfBackSlash);
            return sb;
        }
        sb.Append(sourceFilePath);
        return sb;
    }

    public static IStringBuilder ExtractAppendLineNumber(this IStringBuilder sb, FLogCallLocation subject)
    {
        sb.Append(subject.SourceLineNumber);
        return sb;
    }

    public static IStringBuilder AppendFileNameLineNumber(this IStringBuilder sb, FLogCallLocation subject) =>
        sb.ExtractAppendFileNameWithExtension(subject).Append(": ").ExtractAppendLineNumber(subject);
}
