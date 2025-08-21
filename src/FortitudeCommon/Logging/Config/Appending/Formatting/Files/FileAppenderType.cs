// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
    public static CustomTypeStyler<FileAppenderType> FileAppenderTypeFormatter
        = FormatFileAppenderTypeAppender;

    public static StyledTypeBuildResult FormatFileAppenderTypeAppender(this FileAppenderType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(nameof(FileAppenderType));
        var sb = tb.StringBuilder("_FileAppenderTypeAsString");

        sb.Append("\"");
        switch (asyncProcessingType)
        {
            case FileAppenderType.RollingLogFile: sb.Append($"{nameof(FileAppenderType.RollingLogFile)}"); break;

            default: sb.Append($"{nameof(FileAppenderType.Unbounded)}"); break;
        }
        sb.Append("\"");
        return tb.Complete();
    }
}