// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using static FortitudeCommon.Logging.Config.Appending.Formatting.Files.FLogRollFilePathPart;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public enum FLogRollFilePathPart
{
    Default
  , RollNumPlaceholder
  , CompressionType
  , CompressionTypeExt
  , ToRollFileName
  , ToRollFileNameAndExt
  , ToRollFileExt
  , ToRollFilePath
  , ToRollFullFilePath
}

public static class FLogRollFilePathPartExtensions
{
    public static Action<FLogRollFilePathPart, IStyledTypeStringAppender> FLogRollFilePathPartFormatter
        = FormatFLogRollFilePathPartAppender;

    public static void FormatFLogRollFilePathPartAppender(this FLogRollFilePathPart rollFilePathPart, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (rollFilePathPart)
        {
            case RollNumPlaceholder: sb.Append($"{nameof(RollNumPlaceholder)}"); break;

            case FLogRollFilePathPart.CompressionType: sb.Append($"{nameof(FLogRollFilePathPart.CompressionType)}"); break;
            case CompressionTypeExt:                   sb.Append($"{nameof(CompressionTypeExt)}"); break;

            case ToRollFileName:           sb.Append($"{nameof(ToRollFileName)}"); break;
            case ToRollFileNameAndExt:     sb.Append($"{nameof(ToRollFileNameAndExt)}"); break;
            case ToRollFileExt:            sb.Append($"{nameof(ToRollFileExt)}"); break;
            case ToRollFilePath:           sb.Append($"{nameof(ToRollFilePath)}"); break;
            case ToRollFullFilePath:       sb.Append($"{nameof(ToRollFullFilePath)}"); break;

            default: sb.Append($"{nameof(Default)}"); break;
        }
    }
}
