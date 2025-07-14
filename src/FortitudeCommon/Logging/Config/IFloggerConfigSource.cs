// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;

namespace FortitudeCommon.Logging.Config;

public enum FLoggerConfigSourceType
{
    Unknown
  , File
  , Network
  , Database
}

public interface IFloggerConfigSource : ICollection<IFloggerConfigSource>, IInterfacesComparable<IFloggerConfigSource>
  , IStyledToStringObject
{
    uint   ConfigPriorityOrder { get; set; } // high Priority overrides lower priority

    string ConfigSourceName    { get; }

    string SourceLocation { get; }

    FLoggerConfigSourceType SourceType { get; }

    bool Optional { get; }
}
