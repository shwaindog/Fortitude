// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerTreeCommonConfig : IFLoggerMatchedAppenders, IInterfacesComparable<IFLoggerTreeCommonConfig>
  , IConfigCloneTo<IFLoggerTreeCommonConfig>, IFLogConfig, IStyledToStringObject
{
    string Name { get; }

    string FullName { get; }

    FLogLevel LogLevel { get; }

    IFLoggerActivationConfig DescendantActivation { get; }

    IFLogBuildTypeAndDeployEnvConfig FLogEnvironment { get; }

    IFLogEntryPoolConfig? LogEntryPool { get; }

    INamedChildLoggersLookupConfig DescendantLoggers { get; }
}

public interface IMutableFLoggerTreeCommonConfig : IFLoggerTreeCommonConfig, IMutableFLogConfig, IMutableFLoggerMatchedAppenders
{
    new string Name { get; set; }

    new FLogLevel LogLevel { get; set; }

    new IMutableFLoggerActivationConfig DescendantActivation { get; set; }

    new IMutableFLogBuildTypeAndDeployEnvConfig FLogEnvironment { get; set; }

    new IMutableFLogEntryPoolConfig? LogEntryPool { get; set; }

    new IMutableNamedChildLoggersLookupConfig DescendantLoggers { get; set; }
}

public class FLoggerTreeCommonConfig : FLoggerMatchedAppenders, IMutableFLoggerTreeCommonConfig
{
    private IMutableNamedChildLoggersLookupConfig? loggersConfig;

    protected IMutableFLoggerActivationConfig?         LoggerActivationConfig;
    protected IMutableFLogBuildTypeAndDeployEnvConfig? FlogLoggerEnvironment;

    public FLoggerTreeCommonConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerTreeCommonConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerTreeCommonConfig
    (string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerTreeCommonConfig
    (IConfigurationRoot root, string path, string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null) : base(root, path, appendersCfg)
    {
        Name              = name;
        LogLevel          = logLevel;
        DescendantLoggers = loggersCfg!;
        LogEntryPool      = logEntryPool;
    }

    public FLoggerTreeCommonConfig(IFLoggerTreeCommonConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        Name              = toClone.Name;
        LogLevel          = toClone.LogLevel;
        DescendantLoggers = new NamedChildLoggersLookupConfig(toClone.DescendantLoggers, root, $"{Path}{Split}{nameof(DescendantLoggers)}") ;
        LogEntryPool      = toClone.LogEntryPool as IMutableFLogEntryPoolConfig;
    }

    public FLoggerTreeCommonConfig(IFLoggerTreeCommonConfig toClone) : this(toClone, InMemoryConfigRoot, toClone.ConfigSubPath) { }

    protected FLogLevel? WasDefinedLogLevel => Enum.TryParse<FLogLevel>(this[nameof(LogLevel)], out var logLevel) ? logLevel : null;

    INamedChildLoggersLookupConfig IFLoggerTreeCommonConfig.DescendantLoggers => DescendantLoggers;

    public IMutableNamedChildLoggersLookupConfig DescendantLoggers
    {
        get
        {
            if (loggersConfig == null)
                if (GetSection(nameof(DescendantLoggers)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                    return loggersConfig = new NamedChildLoggersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(DescendantLoggers)}")
                    {
                        ParentConfig = this
                    };
            return loggersConfig ??= new NamedChildLoggersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(DescendantLoggers)}");
        }
        set
        {
            loggersConfig = new NamedChildLoggersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DescendantLoggers)}")
            {
                ParentConfig = this
            };

            value.ParentConfig = this;
        }
    }

    IFLogEntryPoolConfig? IFLoggerTreeCommonConfig.LogEntryPool => LogEntryPool;

    public IMutableFLogEntryPoolConfig? LogEntryPool
    {
        get
        {
            if (GetSection(nameof(LogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}")
                {
                    ParentConfig = this
                };
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}");

                value.ParentConfig = this;
            }
        }
    }

    public FLogLevel LogLevel
    {
        get => WasDefinedLogLevel ?? FLogLevel.None;
        set => this[nameof(LogLevel)] = value.ToString();
    }

    public virtual string Name
    {
        get => this[nameof(Name)] ?? "";
        set => this[nameof(Name)] = value;
    }

    IFLoggerActivationConfig IFLoggerTreeCommonConfig.DescendantActivation => DescendantActivation;

    IFLogBuildTypeAndDeployEnvConfig IFLoggerTreeCommonConfig.FLogEnvironment => FLogEnvironment;

    public virtual IMutableFLoggerActivationConfig DescendantActivation
    {
        get
        {
            if (LoggerActivationConfig != null) return LoggerActivationConfig;
            IFLoggerActivationConfig? parentActivation = null;
            if (ParentConfig is IFLoggerTreeCommonConfig parentLoggerConfig)
            {
                parentActivation = parentLoggerConfig.DescendantActivation;
            }
            return GetDescendantConfig(parentActivation);
        }
        set
        {
            _ = new FLogBuildTypeAndDeployEnvConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DescendantActivation)}");

            value.ParentConfig = this;
        }
    }

    protected IMutableFLoggerActivationConfig GetDescendantConfig(IFLoggerActivationConfig? parentActivation = null)
    {
        LoggerActivationConfig = new FLoggerActivationConfig(ConfigRoot, $"{Path}{Split}{nameof(DescendantActivation)}", parentActivation)
        {
            ParentConfig = this
        };
        return LoggerActivationConfig;
    }

    public IMutableFLogBuildTypeAndDeployEnvConfig FLogEnvironment
    {
        get
        {
            if (FlogLoggerEnvironment != null) return FlogLoggerEnvironment;
            IFLogBuildTypeAndDeployEnvConfig? parentActivation = null;
            if (ParentConfig is IFLoggerTreeCommonConfig parentLoggerConfig)
            {
                parentActivation = parentLoggerConfig.FLogEnvironment;
            }
            FlogLoggerEnvironment = new FLogBuildTypeAndDeployEnvConfig(ConfigRoot, $"{Path}{Split}{nameof(FLogEnvironment)}", parentActivation)
            {
                ParentConfig = this
            };
            return FlogLoggerEnvironment;
        }
        set
        {
            _ = new FLogBuildTypeAndDeployEnvConfig(value, ConfigRoot, $"{Path}{Split}{nameof(FLogEnvironment)}");

            value.ParentConfig = this;
        }
    }

    public string FullName => Accept(new ConfigAncestorsOfLogger()).FullName;

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    object ICloneable.Clone() => Clone();

    IFLoggerTreeCommonConfig ICloneable<IFLoggerTreeCommonConfig>.Clone() => Clone();

    public virtual FLoggerTreeCommonConfig Clone() => new(this);

    IFLoggerTreeCommonConfig IConfigCloneTo<IFLoggerTreeCommonConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public virtual FLoggerTreeCommonConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public bool AreEquivalent(IFLoggerTreeCommonConfig? other, bool exactTypes = false) =>
        AreEquivalent(other as IFLoggerMatchedAppenders, exactTypes);

    public override bool AreEquivalent(IFLoggerMatchedAppenders? other, bool exactTypes = false)
    {
        if (other is not IFLoggerTreeCommonConfig floggerCommonCfg) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var nameSame         = Name == floggerCommonCfg.Name;
        var loggersSame      = DescendantLoggers.AreEquivalent(floggerCommonCfg.DescendantLoggers, exactTypes);
        var logEntryPoolSame = LogEntryPool?.AreEquivalent(floggerCommonCfg.LogEntryPool, exactTypes) ?? floggerCommonCfg.LogEntryPool == null;
        var logLevelSame     = LogLevel == floggerCommonCfg.LogLevel;

        var allAreSame = baseSame && nameSame && loggersSame && logLevelSame && logEntryPoolSame && logLevelSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLoggerTreeCommonConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ Appenders.GetHashCode();
            hashCode = (hashCode * 397) ^ (LogEntryPool?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ LogLevel.GetHashCode();
            hashCode = (hashCode * 397) ^ DescendantLoggers.GetHashCode();
            return hashCode;
        }
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(Name), Name)
           .Field.AlwaysAdd(nameof(LogLevel), LogLevel.ToString())
           .Field.AlwaysAdd(nameof(DescendantLoggers), DescendantLoggers)
           .Field.WhenNonNullAddStyled(nameof(Appenders), Appenders)
           .Field.WhenNonNullAddStyled(nameof(LogEntryPool), LogEntryPool)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
