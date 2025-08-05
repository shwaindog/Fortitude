// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding.AsyncReceiveQueueFullHandling;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;

public enum AsyncReceiveQueueFullHandling
{
    Default
  , BackPressureBlock
  , BackPressureBlockLogToFailAppender
  , Ignore
  , IgnoreLogToFailAppender
  , DeactivateAppenderForPeriod
  , DeactivateAppenderForPeriodLogToFailAppender
  , AutoDeactivateAppender
  , AutoDeactivateAppenderLogToFailAppender
  , AllMessageIntervalFilter
  , AllMessageIntervalFilterLogToFailAppender
  , DebugMessageIntervalFilter
  , DebugMessageIntervalFilterLogToFailAppender
  , DebugInfoMessageIntervalFilter
  , DebugInfoMessageIntervalFilterLogToFailAppender
}

public static class AsyncReceiveQueueFullHandlingExtensions
{
    public static StructStyler<AsyncReceiveQueueFullHandling> AsyncReceiveQueueFullHandlingFormatter
        = FormatFullQueueHandlingAppender;

    public static void FormatFullQueueHandlingAppender(this AsyncReceiveQueueFullHandling queueFull, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (queueFull)
        {
            case Default:                                         sb.Append($"{nameof(Default)}"); break;
            case BackPressureBlock:                               sb.Append($"{nameof(BackPressureBlock)}"); break;
            case BackPressureBlockLogToFailAppender:              sb.Append($"{nameof(BackPressureBlockLogToFailAppender)}"); break;
            case Ignore:                                          sb.Append($"{nameof(Ignore)}"); break;
            case IgnoreLogToFailAppender:                         sb.Append($"{nameof(IgnoreLogToFailAppender)}"); break;
            case DeactivateAppenderForPeriod:                     sb.Append($"{nameof(DeactivateAppenderForPeriod)}"); break;
            case DeactivateAppenderForPeriodLogToFailAppender:    sb.Append($"{nameof(DeactivateAppenderForPeriodLogToFailAppender)}"); break;
            case AutoDeactivateAppender:                          sb.Append($"{nameof(AutoDeactivateAppender)}"); break;
            case AutoDeactivateAppenderLogToFailAppender:         sb.Append($"{nameof(AutoDeactivateAppenderLogToFailAppender)}"); break;
            case AllMessageIntervalFilter:                        sb.Append($"{nameof(AllMessageIntervalFilter)}"); break;
            case AllMessageIntervalFilterLogToFailAppender:       sb.Append($"{nameof(AllMessageIntervalFilterLogToFailAppender)}"); break;
            case DebugMessageIntervalFilter:                      sb.Append($"{nameof(DebugMessageIntervalFilter)}"); break;
            case DebugMessageIntervalFilterLogToFailAppender:     sb.Append($"{nameof(DebugMessageIntervalFilterLogToFailAppender)}"); break;
            case DebugInfoMessageIntervalFilter:                  sb.Append($"{nameof(DebugInfoMessageIntervalFilter)}"); break;
            case DebugInfoMessageIntervalFilterLogToFailAppender: sb.Append($"{nameof(DebugInfoMessageIntervalFilterLogToFailAppender)}"); break;

            default: sb.Append($"{nameof(Default)}"); break;
        }
    }
}
