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