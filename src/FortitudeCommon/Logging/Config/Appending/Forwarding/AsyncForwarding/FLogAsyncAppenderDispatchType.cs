// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding.FLogAsyncAppenderDispatchType;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;

public enum FLogAsyncAppenderDispatchType
{
    Synchronous
  , ThreadPool
  , SingleBackgroundThread
  , ThreadPerQueueNumber
  , EventBusDispatch
}

public static class FLogAsyncAppenderDispatchTypeExtensions
{
    public static StructStyler<FLogAsyncAppenderDispatchType> FLogAsyncAppenderDispatchTypeFormatter
        = FormatFullQueueHandlingAppender;

    public static void FormatFullQueueHandlingAppender(this FLogAsyncAppenderDispatchType queueFull, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        switch (queueFull)
        {
            case Synchronous: sb.Append($"{nameof(Synchronous)}"); break;

            case SingleBackgroundThread: sb.Append($"{nameof(SingleBackgroundThread)}"); break;
            case ThreadPerQueueNumber:   sb.Append($"{nameof(ThreadPerQueueNumber)}"); break;
            case EventBusDispatch:       sb.Append($"{nameof(EventBusDispatch)}"); break;

            case FLogAsyncAppenderDispatchType.ThreadPool: sb.Append($"{nameof(FLogAsyncAppenderDispatchType.ThreadPool)}"); break;

            default: sb.Append($"{nameof(SingleBackgroundThread)}"); break;
        }
    }
}
