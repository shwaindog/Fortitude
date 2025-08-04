// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences.TriggeringLogEntries;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public enum TriggeringLogEntries
{
    None
  , All
  , First
  , Last
}


public static class TriggeringLogEntriesExtensions
{
    public static Action<TriggeringLogEntries, IStyledTypeStringAppender> TriggeringLogEntriesFormatter
        = FormatTriggeringLogEntriesAppender;

    public static void FormatTriggeringLogEntriesAppender(this TriggeringLogEntries triggeringLogEntries, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (triggeringLogEntries)
        {
            case None:       sb.Append($"{nameof(None)}"); break;
            case All:        sb.Append($"{nameof(All)}"); break;
            case First:      sb.Append($"{nameof(First)}"); break;
            case Last:       sb.Append($"{nameof(Last)}"); break;

            default: sb.Append($"{nameof(All)}"); break;
        }
    }
}