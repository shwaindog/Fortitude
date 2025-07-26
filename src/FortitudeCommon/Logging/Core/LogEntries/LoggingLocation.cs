// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Logging.Core.LogEntries;

public record struct LoggingLocation(string MemberName, string SourceFilePath, int SourceLineNumber)
{
    public const string PerfLogging = "NonePerfLogging";

    public static readonly LoggingLocation NonePerfLoggingUsed = new (PerfLogging, PerfLogging, 0);
}

public static class LoggingLocationExtensions
{
    public static StringBuilder ExtractAppendFileName(this StringBuilder sb, LoggingLocation subject)
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

    public static StringBuilder ExtractAppendLineNumber(this StringBuilder sb, LoggingLocation subject)
    {
        sb.Append(subject.SourceLineNumber);
        return sb;
    }

    public static StringBuilder AppendFileNameLineNumber(this StringBuilder sb, LoggingLocation subject)
    {
        return sb.ExtractAppendFileName(subject).Append(": ").ExtractAppendLineNumber(subject);
    }
}
