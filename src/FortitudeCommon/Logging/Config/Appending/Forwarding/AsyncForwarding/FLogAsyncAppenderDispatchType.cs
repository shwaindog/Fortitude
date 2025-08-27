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
