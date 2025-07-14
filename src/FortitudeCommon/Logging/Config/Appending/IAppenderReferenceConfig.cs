// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending;

public interface IAppenderReferenceConfig
{
    string?  AppenderName       { get; }

    string? AppenderConfigRef { get; }

    bool DisableHere { get; }

    IAppenderDefinitionConfig? ResolveAppenderDefinition();
}

public interface IMutableAppenderReferenceConfig : IAppenderReferenceConfig
{
    new string?  AppenderName       { get; set; }

    new string? AppenderConfigRef { get; set; }

    new bool DisableHere { get; set; }

    new IMutableAppenderDefinitionConfig? ResolveAppenderDefinition();
}