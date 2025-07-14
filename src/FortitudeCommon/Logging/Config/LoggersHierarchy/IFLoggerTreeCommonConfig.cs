// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerTreeCommonConfig : IFLoggerMatchedAppenders
{
    string Name { get; }

    FLogLevel LogLevel { get; }

    ISizeableItemPoolConfig? LogEntryPool { get; }

    INamedChildLoggersLookupConfig DescendantLoggers { get; }
}

public interface IMutableFLoggerTreeCommonConfig : IFLoggerTreeCommonConfig, IMutableFLoggerMatchedAppenders
{
    new string Name { get; set; }

    new FLogLevel LogLevel { get; set; }

    new ISizeableItemPoolConfig? LogEntryPool { get; set; }

    new INamedChildLoggersLookupConfig DescendantLoggers { get; set; }
}
