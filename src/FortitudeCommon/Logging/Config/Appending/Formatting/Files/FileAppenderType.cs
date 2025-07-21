// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public enum FileAppenderType
{
    Unbounded
  , RollingLogFile
}


public static class FileAppenderTypeExtensions
{
    public static Action<FileAppenderType, IStyledTypeStringAppender> FileAppenderTypeFormatter
        = FormatFileAppenderTypeAppender;

    public static void FormatFileAppenderTypeAppender(this FileAppenderType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.BackingStringBuilder;

        switch (asyncProcessingType)
        {
            case FileAppenderType.Unbounded: sb.Append($"{nameof(FileAppenderType.Unbounded)}"); break;
            case FileAppenderType.RollingLogFile: sb.Append($"{nameof(FileAppenderType.RollingLogFile)}"); break;

            default: sb.Append($"{nameof(FileAppenderType.Unbounded)}"); break;
        }
    }
}