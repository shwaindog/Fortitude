// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public enum AsyncProcessingType
{
    Default
  , AllAsyncDisabled
  , Synchronise
  , SingleBackgroundAsyncThread
  , ConfigDefinedAsyncThreads
  , AsyncUsesThreadPool
  , ConfigEventBusQueues
}