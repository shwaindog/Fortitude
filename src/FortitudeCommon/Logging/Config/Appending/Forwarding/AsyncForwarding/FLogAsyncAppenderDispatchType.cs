// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;

public enum FLogAsyncAppenderDispatchType
{
    Synchronous
  , ThreadPool
  , SingleBackgroundThread
  , ThreadPerQueueNumber
  , EventBusDispatch
}
