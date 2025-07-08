// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Files;

public interface IRollingFileAppenderConfig : IFileAppenderConfig
{
    uint RollWhenAtSize { get; }
    
    uint PostRollProcessingDelayMs { get; }

    bool Compress { get; }
}
