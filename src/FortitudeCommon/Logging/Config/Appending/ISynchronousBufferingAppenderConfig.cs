// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending;

public interface ISynchronousBufferingAppenderConfig : IBufferingAppenderConfig
{
    int DrainWhenEntryDurationExceedsMs    { get; }

    FLogLevel DrainWhenLogLevel { get; }
}
