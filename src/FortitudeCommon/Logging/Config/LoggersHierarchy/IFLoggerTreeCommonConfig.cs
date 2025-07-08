// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerTreeCommonConfig : IFLoggerMatchedAppenders
{
    string Name { get; }

    FLogLevel LogLevel { get; }

    INamedChildLoggersLookupConfig ChildLoggers { get; }
}
