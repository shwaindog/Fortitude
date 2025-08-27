// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public enum FullQueueHandling
{
    Default
  , Block
  , TryAgain
  , TimeoutDropNewest
  , TimeoutDropNewestForwardToFailedAppender
  , DropNewest
  , DropOldest
  , DropNewestForwardToFailAppender
  , DropOldestForwardToFailAppender
  , DropOldestTwoAddDroppedLog
  , DropNewestTwoAddDroppedLog
  , DropAllDebugLevelInQueue
  , DropAllDebugInfoLevelInQueue
  , DropAll
  , DropAllAddDroppedLog
  , DropAllAddDroppedLogToFailAppender
  , DropEveryQueueInterval
  , DropDebugQueueInterval
  , DropDebugAndInfoQueueInterval
}
