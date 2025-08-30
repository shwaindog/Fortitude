// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Config.Appending.FLoggerBuiltinAppenderType;

namespace FortitudeCommon.Logging.Config.Appending;

public enum FLoggerBuiltinAppenderType
{
    NotGiven
  , Ref
  , NullEntry
  , ForwardingAppender
  , BufferedForwarding
  , FilteredForwarding
  , SwitchChannelForwarding
  , SyncBufferedForwarding
  , AsyncBufferedForwarding
  , FileUnbounded
  , FileRolling
  , NullFile
  , ConsoleOut
  , NullConsole
  , FLogEntryListAppender
  , FormattedStringListAppender
  , Network
  , Database
}

public static class FloggerBuiltinAppenderTypeExtensions
{
    public static string? GetAppenderType(this IConfigurationRoot configRoot, string configPath) =>
        configRoot.GetSection($"{configPath}{ConfigurationPath.KeyDelimiter}{nameof(IAppenderDefinitionConfig.AppenderType)}").Value;

    public static IMutableAppenderReferenceConfig? GetBuiltAppenderReferenceConfig(this IConfigurationRoot configRoot, string configPath)
    {
        var appenderTypeConfig = GetAppenderType(configRoot, configPath);
        switch (appenderTypeConfig)
        {
            case null:
            case nameof(Ref):
                return new AppenderReferenceConfig(configRoot, configPath);
            case nameof(NullEntry):               return new NullAppenderConfig(configRoot, configPath);
            case nameof(ForwardingAppender):      return new ForwardingAppenderConfig(configRoot, configPath);
            case nameof(BufferedForwarding):      return new BufferingAppenderConfig(configRoot, configPath);
            case nameof(AsyncBufferedForwarding): return new AsyncForwardingAppendersConfig(configRoot, configPath);
            case nameof(FilteredForwarding):      return new FilteringForwardingAppenderConfig(configRoot, configPath);
            case nameof(ConsoleOut):              return new ConsoleAppenderConfig(configRoot, configPath);
            case nameof(FileUnbounded):           return new FileAppenderConfig(configRoot, configPath);
            default:
                string[] assemblyAndTypeFullName;
                if (appenderTypeConfig.IsNotNullOrEmpty() && (assemblyAndTypeFullName = appenderTypeConfig.Split(',', 2)).Length == 2)
                    return (IMutableAppenderReferenceConfig?)Activator.CreateInstanceFrom
                        (assemblyAndTypeFullName[0], assemblyAndTypeFullName[1], true, BindingFlags.CreateInstance
                       , null, [configRoot, configPath], CultureInfo.InvariantCulture, [])!.Unwrap();
                return null;
        }
    }
}
