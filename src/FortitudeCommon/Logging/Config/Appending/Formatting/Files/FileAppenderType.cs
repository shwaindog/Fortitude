// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public enum FileAppenderType
{
    Unbounded
  , RollingLogFile
}


public static class FileAppenderTypeExtensions
{
    public static StructStyler<FileAppenderType> FileAppenderTypeFormatter
        = FormatFileAppenderTypeAppender;

    public static void FormatFileAppenderTypeAppender(this FileAppenderType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (asyncProcessingType)
        {
            case FileAppenderType.Unbounded: sb.Append($"{nameof(FileAppenderType.Unbounded)}"); break;
            case FileAppenderType.RollingLogFile: sb.Append($"{nameof(FileAppenderType.RollingLogFile)}"); break;

            default: sb.Append($"{nameof(FileAppenderType.Unbounded)}"); break;
        }
    }
}