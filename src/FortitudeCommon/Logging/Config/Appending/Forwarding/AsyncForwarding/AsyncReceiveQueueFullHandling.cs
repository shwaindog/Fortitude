// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
