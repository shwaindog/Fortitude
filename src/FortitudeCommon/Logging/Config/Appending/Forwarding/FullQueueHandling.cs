// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

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

public static class FullQueueHandlingExtensions
{
    public static CustomTypeStyler<FullQueueHandling> FullQueueHandlingFormatter = FormatFullQueueHandlingAppender;

    public static StyledTypeBuildResult FormatFullQueueHandlingAppender(this FullQueueHandling queueFull, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(nameof(FullQueueHandling));
        var sb = tb.StringBuilder("_FullQueueHandlingAsString");

        sb.Append("\"");
        switch (queueFull)
        {
            case FullQueueHandling.Default:    sb.Append($"{nameof(FullQueueHandling.Default)}"); break;
            case FullQueueHandling.Block:      sb.Append($"{nameof(FullQueueHandling.Block)}"); break;
            case FullQueueHandling.TryAgain:   sb.Append($"{nameof(FullQueueHandling.TryAgain)}"); break;
            case FullQueueHandling.DropNewest: sb.Append($"{nameof(FullQueueHandling.DropNewest)}"); break;
            case FullQueueHandling.DropOldest: sb.Append($"{nameof(FullQueueHandling.DropOldest)}"); break;

            case FullQueueHandling.DropNewestForwardToFailAppender: sb.Append($"{nameof(FullQueueHandling.DropNewestForwardToFailAppender)}"); break;
            case FullQueueHandling.DropOldestForwardToFailAppender: sb.Append($"{nameof(FullQueueHandling.DropOldestForwardToFailAppender)}"); break;

            case FullQueueHandling.DropOldestTwoAddDroppedLog:   sb.Append($"{nameof(FullQueueHandling.DropOldestTwoAddDroppedLog)}"); break;
            case FullQueueHandling.DropNewestTwoAddDroppedLog:   sb.Append($"{nameof(FullQueueHandling.DropNewestTwoAddDroppedLog)}"); break;
            case FullQueueHandling.DropAllDebugLevelInQueue:     sb.Append($"{nameof(FullQueueHandling.DropAllDebugLevelInQueue)}"); break;
            case FullQueueHandling.DropAllDebugInfoLevelInQueue: sb.Append($"{nameof(FullQueueHandling.DropAllDebugInfoLevelInQueue)}"); break;

            case FullQueueHandling.DropAll: sb.Append($"{nameof(FullQueueHandling.DropAll)}"); break;

            case FullQueueHandling.DropEveryQueueInterval: sb.Append($"{nameof(FullQueueHandling.DropEveryQueueInterval)}"); break;
            case FullQueueHandling.DropDebugQueueInterval: sb.Append($"{nameof(FullQueueHandling.DropDebugQueueInterval)}"); break;

            case FullQueueHandling.DropDebugAndInfoQueueInterval: sb.Append($"{nameof(FullQueueHandling.DropDebugAndInfoQueueInterval)}"); break;

            default: sb.Append($"{nameof(FullQueueHandling.Default)}"); break;
        }
        sb.Append("\"");
        
        return tb.Complete();
    }
}
