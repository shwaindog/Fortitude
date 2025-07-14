// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerFileConfigSourceConfig : IFloggerConfigSource
{

    bool FileSystemMonitored  { get; set; }

    int PollInterval { get; set; }

    string FilePath { get; set; }

    new bool Optional { get; set; }
}
