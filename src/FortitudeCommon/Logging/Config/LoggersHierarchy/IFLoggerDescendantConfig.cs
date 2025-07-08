// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerDescendantConfig : IFLoggerTreeCommonConfig
{
    IFLoggerTreeCommonConfig ParentLoggerConfig { get; }

    string ParentName { get; }

    string FullName { get; }

    bool Inherits { get; }

    new INamedChildLoggersLookupConfig ChildLoggers { get; }

    IFLoggerDescendantConfig ResolveChildConfig(string childRelativeLoggerName);
}

public interface IMutableFLoggerDescendantConfig : IFLoggerDescendantConfig, IMutableFLoggerMatchedAppenders
{
    new string Name { get; set; }

    new bool Inherits { get; set; }

    new IMutableNamedChildLoggersLookupConfig ChildLoggers { get; set; }
}