// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.AsyncBuffering;

namespace FortitudeCommon.Logging.Config.Appending;


public interface IBufferingAppenderConfig
{
    int InboundBufferEntriesSize { get; }

    FullQueueHandling InboundQueueFullHandling { get; }

    int MaxBufferBatchDispatchSize    { get; }
}
