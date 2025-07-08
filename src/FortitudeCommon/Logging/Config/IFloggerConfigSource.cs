// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config;

public enum FLoggerConfigSourceType
{
    Unknown
  , File
  , Network
  , Database
}

public interface IFloggerConfigSource
{
    string ConfigSourceName { get; }

    string SourceLocation { get; }

    FLoggerConfigSourceType SourceType { get; }

    bool Optional { get; }
}
