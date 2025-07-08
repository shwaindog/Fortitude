// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;


public enum DownstreamQueueFullHandling
{
    Default
  , BackPressureBlock
  , BackPressureBlockLogToFailAppender
  , Ignore
  , IgnoreLogToFailAppender
  , DisableAppenderForPeriod
  , DisableAppenderForPeriodLogToFailAppender
  , AutoDeactivateAppender
  , AutoDeactivateAppenderLogToFailAppender
  , AllMessageIntervalFilter
  , AllMessageIntervalFilterLogToFailAppender
  , DebugMessageIntervalFilter
  , DebugMessageIntervalFilterLogToFailAppender
  , DebugInfoMessageIntervalFilter
  , DebugInfoMessageIntervalFilterLogToFailAppender
}

public enum AsyncAppenderType
{
     Synchronous
    , ThreadPool
    , SingleBackgroundThread  
    , ThreadPerQueueNumber  
    , EventBusDispatch 
}

public interface IAsyncForwardingAppendersConfig : IBufferingAppenderConfig
{
    AsyncAppenderType AppenderType { get; }

    bool Broadcast { get; }

    int MaxDispatchUnconfirmed { get; }

    int ConfirmSequenceNumberInterval { get; }

    DownstreamQueueFullHandling DownstreamQueueFullHandling { get; }

}
