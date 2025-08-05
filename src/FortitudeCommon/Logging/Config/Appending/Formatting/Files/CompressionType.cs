// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using static FortitudeCommon.Logging.Config.Appending.Formatting.Files.CompressionType;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public enum CompressionType
{
    Uncompressed
  , Zip
  , GZip
  , Lzma // 7zip compression
}

public static class CompressionTypeExtensions
{
    public static Action<CompressionType, IStyledTypeStringAppender> CompressionTypeFormatter
        = FormatCompressionTypeAppender;

    public static void FormatCompressionTypeAppender(this CompressionType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (asyncProcessingType)
        {
            case Zip: sb.Append($"{nameof(Zip)}"); break;

            case GZip: sb.Append($"{nameof(GZip)}"); break;
            case Lzma: sb.Append($"{nameof(Lzma)}"); break;

            default: sb.Append($"{nameof(Zip)}"); break;
        }
    }
}
