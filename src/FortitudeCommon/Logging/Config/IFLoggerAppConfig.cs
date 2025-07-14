// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerAppConfig : IFLoggerMatchedAppenders, IInterfacesComparable<IFLoggerAppConfig>, ICloneable<IFLoggerAppConfig>
  , IStyledToStringObject
{
    IFLoggerConfigSourceDefinitions ConfigSources { get; }

    IFLoggerRootConfig RootLogger { get; }
}

public interface IMutableFLoggerAppConfig : IFLoggerAppConfig, IMutableFLoggerMatchedAppenders
{
    new IMutableFLoggerConfigSourceDefinitions ConfigSources { get; set; }

    new IMutableFLoggerRootConfig RootLogger { get; set; }
}

public class FLoggerAppConfig : ConfigSection, IMutableFLoggerAppConfig
{
    private IAppendableNamedAppendersLookupConfig?  appendersConfig;
    private IMutableFLoggerConfigSourceDefinitions? configSources;
    private IMutableFLoggerRootConfig?              rootLoggerConfig;
    public FLoggerAppConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerAppConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerAppConfig
    (IMutableFLoggerConfigSourceDefinitions configSources
      , IAppendableNamedAppendersLookupConfig? appendersCfg, IMutableFLoggerRootConfig? rootLoggerCfg)
        : this(InMemoryConfigRoot, InMemoryPath, configSources, appendersCfg, rootLoggerCfg) { }

    public FLoggerAppConfig
    (IConfigurationRoot root, string path, IMutableFLoggerConfigSourceDefinitions configSources
      , IAppendableNamedAppendersLookupConfig? appendersCfg = null, IMutableFLoggerRootConfig? rootLoggerCfg = null) : base(root, path)
    {
        ConfigSources = configSources;
        Appenders     = appendersCfg ?? new NamedAppendersLookupConfig();
        RootLogger    = rootLoggerCfg ?? new FLoggerRootConfig();
    }

    public FLoggerAppConfig(IFLoggerAppConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        ConfigSources = (IMutableFLoggerConfigSourceDefinitions)toClone.ConfigSources;
        Appenders     = (IAppendableNamedAppendersLookupConfig)toClone.Appenders;
        RootLogger    = (IMutableFLoggerRootConfig)toClone.RootLogger;
    }

    public FLoggerAppConfig(IFLoggerAppConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    INamedAppendersLookupConfig IFLoggerMatchedAppenders.Appenders => Appenders;

    public IAppendableNamedAppendersLookupConfig Appenders
    {
        get
        {
            if (appendersConfig == null)
            {
                if (GetSection(nameof(Appenders)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return appendersConfig = new NamedAppendersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(Appenders)}");
                }
            }
            return appendersConfig ?? throw new ConfigurationErrorsException($"Expected {nameof(Appenders)} to be configured");
        }
        set => appendersConfig = new NamedAppendersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Appenders)}");
    }

    IFLoggerConfigSourceDefinitions IFLoggerAppConfig.ConfigSources => ConfigSources;

    public IMutableFLoggerConfigSourceDefinitions ConfigSources
    {
        get
        {
            if (configSources == null)
            {
                if (GetSection(nameof(ConfigSources)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return configSources = new FLoggerConfigSourceDefinitions(ConfigRoot, $"{Path}{Split}{nameof(ConfigSources)}");
                }
            }
            return configSources ?? throw new ConfigurationErrorsException($"Expected {nameof(ConfigSources)} to be configured");
        }
        set => configSources = new FLoggerConfigSourceDefinitions(value, ConfigRoot, $"{Path}{Split}{nameof(ConfigSources)}");
    }

    IFLoggerRootConfig IFLoggerAppConfig.RootLogger => RootLogger;

    public IMutableFLoggerRootConfig RootLogger
    {
        get
        {
            if (rootLoggerConfig == null)
            {
                if (GetSection(nameof(RootLogger)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return rootLoggerConfig = new FLoggerRootConfig(ConfigRoot, $"{Path}{Split}{nameof(RootLogger)}");
                }
            }
            return rootLoggerConfig ?? throw new ConfigurationErrorsException($"Expected {nameof(RootLogger)} to be configured");
        }
        set => rootLoggerConfig = new FLoggerRootConfig(value, ConfigRoot, $"{Path}{Split}{nameof(RootLogger)}");
    }


    public IntervalExpansionType IntervalExpansionType
    {
        get =>
            Enum.TryParse<IntervalExpansionType>(this[nameof(IntervalExpansionType)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : IntervalExpansionType.Constant;
        set => this[nameof(IntervalExpansionType)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IFLoggerAppConfig ICloneable<IFLoggerAppConfig>.Clone() => Clone();

    public virtual FLoggerAppConfig Clone() => new(this);


    public virtual bool AreEquivalent(IFLoggerAppConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var cfgSourcesSame = ConfigSources.AreEquivalent(other.ConfigSources, exactTypes);
        var appendersSame  = Appenders.AreEquivalent(other.Appenders, exactTypes);
        var rootLoggerSame = RootLogger.AreEquivalent(other.RootLogger, exactTypes);

        var allAreSame = cfgSourcesSame && appendersSame && rootLoggerSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLoggerAppConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = ConfigSources.GetHashCode();
            hashCode = (hashCode * 397) ^ Appenders.GetHashCode();
            hashCode = (hashCode * 397) ^ RootLogger.GetHashCode();
            return hashCode;
        }
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(FLoggerAppConfig))
           .AddTypeStart()
           .AddField(nameof(ConfigSources), ConfigSources)
           .AddField(nameof(Appenders), Appenders)
           .AddField(nameof(RootLogger), RootLogger)
           .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
