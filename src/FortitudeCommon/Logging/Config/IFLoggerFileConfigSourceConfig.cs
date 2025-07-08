// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerFileConfigSourceConfig : IFloggerConfigSource
{
    uint ConfigPriorityOrder { get; set; } // high Priority overrides lower priority

    bool FileSystemMonitored  { get; set; }

    int PollInterval { get; set; }

    string FilePath { get; set; }

    new bool Optional { get; set; }
}
