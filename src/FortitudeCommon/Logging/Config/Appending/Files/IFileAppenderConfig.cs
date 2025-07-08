// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Files;

public enum FileAppenderType
{
    Unbounded
    , RollingLogFile
}

public interface IFileAppenderConfig : IAppenderDefinitionConfig
{
    FileAppenderType FileAppenderType { get; }

    string Path { get; }

    string FileName { get; }

}