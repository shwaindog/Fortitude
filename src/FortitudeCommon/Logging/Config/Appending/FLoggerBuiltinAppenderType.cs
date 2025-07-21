// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Config.Appending.FLoggerBuiltinAppenderType;

namespace FortitudeCommon.Logging.Config.Appending;

public enum FLoggerBuiltinAppenderType
{
    NotGiven
  , Ref
  , ForwardToRef
  , Ignore
  , SyncForwarding
  , FilteredForwarding
  , SwitchChannelForwarding
  , SyncBufferedForwarding
  , AsyncBufferedForwarding
  , FileUnbounded
  , FileRolling
  , Console
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
            case nameof(Ref) :                  
                return new AppenderReferenceConfig(configRoot, configPath);
            case nameof(SyncForwarding) : return new AppenderReferenceConfig(configRoot, configPath);
            default :
                string[] assemblyAndTypeFullName;
                if (appenderTypeConfig.IsNotNullOrEmpty() && (assemblyAndTypeFullName = appenderTypeConfig.Split(',', 2)).Length == 2)
                {
                    return (IMutableAppenderReferenceConfig?)Activator.CreateInstanceFrom
                        (assemblyAndTypeFullName[0], assemblyAndTypeFullName[1], true, BindingFlags.CreateInstance
                       , null, [configRoot, configPath], CultureInfo.InvariantCulture, [])!.Unwrap();
                }
                return null;
        }
    }

    public static IMutableAppenderReferenceConfig CopyAppenderConfigTo(this IAppenderReferenceConfig toClone, IConfigurationRoot configRoot, string collectionPath)
    {
        var savePath = $"{collectionPath}{ConfigurationPath.KeyDelimiter}{toClone.AppenderName}";
        switch (toClone)
        {
            case AppenderReferenceConfig appenderRef : return new AppenderReferenceConfig(appenderRef, configRoot, savePath);
            default : return new AppenderReferenceConfig(toClone, configRoot, savePath);
        }
    }
}