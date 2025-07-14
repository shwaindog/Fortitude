// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerDescendantConfig : IFLoggerTreeCommonConfig
{
    IFLoggerTreeCommonConfig ParentLoggerConfig { get; }

    bool Inherits { get; }

    string ResolveFullName();
}

public interface IMutableFLoggerDescendantConfig : IFLoggerDescendantConfig, IMutableFLoggerTreeCommonConfig
{
    new IFLoggerTreeCommonConfig ParentLoggerConfig { get; set; }

    new bool Inherits { get; set; }
}