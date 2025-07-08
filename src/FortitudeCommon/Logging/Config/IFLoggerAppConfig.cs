// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.LoggersHierarchy;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerAppConfig : IFLoggerMatchedAppenders
{
    IFLoggerConfigSourceDefinitions ConfigSources { get; }

    IFLoggerRootConfig RootLogger { get; }
}

public interface IMutableFLoggerAppConfig : IFLoggerAppConfig, IMutableFLoggerMatchedAppenders
{
    new IMutableFLoggerConfigSourceDefinitions ConfigSources { get; set; }

    new IMutableFLoggerRootConfig RootLogger { get; set; }
}