// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.AsyncBuffering;
using FortitudeCommon.Logging.Config.Pooling;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IQueueingAppenderConfig : IForwardingAppenderConfig
{
    int QueueSize { get; }

    FullQueueHandling InboundQueueFullHandling { get; }

    int MaxDownstreamBatchSize    { get; }

    ISizeableItemPoolConfig LogEntryPool { get; }
}
