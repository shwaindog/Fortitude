// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.LogEntryField;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

[Flags]
public enum LogEntryField : byte
{
    Nothing     = 0x00
  , MessageBody = 0x01
  , LoggerName  = 0x02
  , SourceFile  = 0x04
  , MemberName  = 0x08
  , Any         = 0xFF
}

public static class LogEntryFieldExtensions
{
    public static Action<LogEntryField, IStyledTypeStringAppender> FormatMatchOnLogEntryFieldFormatter
        = FormatMatchOnLogEntryFieldAppender;

    public static void FormatMatchOnLogEntryFieldAppender(this LogEntryField matchOnField, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (matchOnField)
        {
            case Nothing:     sb.Append($"{nameof(Nothing)}"); break;
            case MessageBody: sb.Append($"{nameof(MessageBody)}"); break;
            case LoggerName:  sb.Append($"{nameof(LoggerName)}"); break;
            case SourceFile:  sb.Append($"{nameof(SourceFile)}"); break;
            case MemberName:  sb.Append($"{nameof(MemberName)}"); break;
            case Any:         sb.Append($"{nameof(Any)}"); break;

            default: sb.Append($"{nameof(MessageBody)}"); break;
        }
    }
}
