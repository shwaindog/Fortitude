// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IConfigRegistry
{
    IFLoggerAppConfig AppConfig { get; }
}
