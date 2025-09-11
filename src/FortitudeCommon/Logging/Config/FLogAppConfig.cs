// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting.Console;
using FortitudeCommon.Logging.Config.ConfigSources;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config;

public interface IFLogAppConfig : IFLoggerMatchedAppenders, IInterfacesComparable<IFLogAppConfig>, ICloneable<IFLogAppConfig>
  , IStyledToStringObject, IFLogConfig
{
    const string DefaultFLogAppConfigPath = "FLog";

    IOrderedConfigSourcesLookupConfig ConfigSourcesLookup { get; }

    IFLogInitializationConfig Initialization { get; }

    IFLoggerRootConfig RootLogger { get; }

    string ConfigRootPath { get; }
}

public interface IMutableFLogAppConfig : IFLogAppConfig, IMutableFLoggerMatchedAppenders
{
    new IAppendableOrderedConfigSourcesLookupConfig ConfigSourcesLookup { get; set; }

    new IMutableFLogInitializationConfig Initialization { get; set; }

    new IMutableFLoggerRootConfig RootLogger { get; set; }
}

public class FLogAppConfig : FLoggerMatchedAppenders, IMutableFLogAppConfig
{
    private IAppendableOrderedConfigSourcesLookupConfig? configSources;

    private IMutableFLogInitializationConfig? initializationConfig;
    private IMutableFLoggerRootConfig?        rootLoggerConfig;

    public FLogAppConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogAppConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogAppConfig
    (IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup
      , IAppendableNamedAppendersLookupConfig? appendersCfg, IMutableFLoggerRootConfig? rootLoggerCfg)
        : this(InMemoryConfigRoot, InMemoryPath, configSourcesLookup, appendersCfg, rootLoggerCfg) { }

    public FLogAppConfig
    (IConfigurationRoot root, string path, IAppendableOrderedConfigSourcesLookupConfig configSourcesLookup
      , IAppendableNamedAppendersLookupConfig? appendersCfg = null, IMutableFLoggerRootConfig? rootLoggerCfg = null) : base(root, path, appendersCfg)
    {
        ConfigSourcesLookup = configSourcesLookup;
        RootLogger          = rootLoggerCfg ?? new FLoggerRootConfig();
    }

    public FLogAppConfig(IFLogAppConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        ConfigSourcesLookup = (IAppendableOrderedConfigSourcesLookupConfig)toClone.ConfigSourcesLookup;
        RootLogger          = (IMutableFLoggerRootConfig)toClone.RootLogger;
    }

    public FLogAppConfig(IFLogAppConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    IOrderedConfigSourcesLookupConfig IFLogAppConfig.ConfigSourcesLookup => ConfigSourcesLookup;

    public IAppendableOrderedConfigSourcesLookupConfig ConfigSourcesLookup
    {
        get
        {
            if (configSources == null)
                if (GetSection(nameof(ConfigSourcesLookup)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                    return configSources = new OrderedConfigSourcesLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(ConfigSourcesLookup)}")
                    {
                        ParentConfig = this
                    };
            return configSources ?? throw new ConfigurationErrorsException($"Expected {nameof(ConfigSourcesLookup)} to be configured");
        }
        set =>
            configSources = new OrderedConfigSourcesLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ConfigSourcesLookup)}")
            {
                ParentConfig = this
            };
    }

    IFLogInitializationConfig IFLogAppConfig.Initialization => Initialization;

    public IMutableFLogInitializationConfig Initialization
    {
        get
        {
            return initializationConfig ??= new FLogInitializationConfig(ConfigRoot, $"{Path}{Split}{nameof(Initialization)}")
            {
                ParentConfig = this
            };
        }
        set =>
            initializationConfig = new FLogInitializationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Initialization)}")
            {
                ParentConfig = this
            };
    }

    IFLoggerRootConfig IFLogAppConfig.RootLogger => RootLogger;

    public IMutableFLoggerRootConfig RootLogger
    {
        get
        {
            if (rootLoggerConfig == null)
                if (GetSection(nameof(RootLogger)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    rootLoggerConfig = FLogCreate.MakeRootLoggerConfig(ConfigRoot, $"{Path}{Split}{nameof(RootLogger)}");

                    rootLoggerConfig.ParentConfig = this;

                    return rootLoggerConfig;
                }
            return rootLoggerConfig ??= FLogCreate.MakeRootLoggerConfig(ConfigRoot, $"{Path}{Split}{nameof(RootLogger)}");
        }
        set
        {
            rootLoggerConfig = new FLoggerRootConfig(value, ConfigRoot, $"{Path}{Split}{nameof(RootLogger)}");

            value.ParentConfig = this;
        }
    }

    public string ConfigRootPath => Path;

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public static FLogAppConfig BuildDefaultAppConfig()
    {
        var appConfig = new FLogAppConfig(InMemoryConfigRoot, IFLogAppConfig.DefaultFLogAppConfigPath);
        var consoleAppenderConfig = new ConsoleAppenderConfig
        {
            AppenderName = IConsoleAppenderConfig.DefaultConsoleAppenderName
        };
        appConfig.Appenders.Add(consoleAppenderConfig.AppenderName, consoleAppenderConfig);
        appConfig.RootLogger.LogLevel = FLogLevel.Debug;
        appConfig.RootLogger.Appenders.Add(consoleAppenderConfig.GenerateReferenceToThis());
        return appConfig;
    }

    object ICloneable.Clone() => Clone();

    IFLogAppConfig ICloneable<IFLogAppConfig>.Clone() => Clone();

    public virtual FLogAppConfig Clone() => new(this);

    public bool AreEquivalent(IFLogAppConfig? other, bool exactTypes = false) => AreEquivalent(other as IFLoggerMatchedAppenders, exactTypes);

    public override bool AreEquivalent(IFLoggerMatchedAppenders? other, bool exactTypes = false)
    {
        if (other is not IFLogAppConfig appConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var cfgSourcesSame = ConfigSourcesLookup.AreEquivalent(appConfig.ConfigSourcesLookup, exactTypes);
        var rootLoggerSame = RootLogger.AreEquivalent(appConfig.RootLogger, exactTypes);

        var allAreSame = baseSame && cfgSourcesSame && rootLoggerSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogAppConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = ConfigSourcesLookup.GetHashCode();
            hashCode = (hashCode * 397) ^ Appenders.GetHashCode();
            hashCode = (hashCode * 397) ^ RootLogger.GetHashCode();
            return hashCode;
        }
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ConfigSourcesLookup), ConfigSourcesLookup)
           .Field.AlwaysAdd(nameof(Appenders), Appenders)
           .Field.AlwaysAdd(nameof(RootLogger), RootLogger)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
