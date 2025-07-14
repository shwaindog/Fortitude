// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerTreeCommonConfig : IFLoggerMatchedAppenders, IInterfacesComparable<IFLoggerTreeCommonConfig>
  , ICloneable<IFLoggerTreeCommonConfig>, IStyledToStringObject
{
    string Name { get; }

    FLogLevel LogLevel { get; }

    IFLogEntryPoolConfig? LogEntryPool { get; }

    INamedChildLoggersLookupConfig DescendantLoggers { get; }
}

public interface IMutableFLoggerTreeCommonConfig : IFLoggerTreeCommonConfig, IMutableFLoggerMatchedAppenders
{
    new string Name { get; set; }

    new FLogLevel LogLevel { get; set; }

    new IFLogEntryPoolConfig? LogEntryPool { get; set; }

    new INamedChildLoggersLookupConfig DescendantLoggers { get; set; }
}

public class FLoggerTreeCommonConfig: ConfigSection, IMutableFLoggerTreeCommonConfig
{
    private IAppendableNamedAppendersLookupConfig? appendersConfig;
    private IMutableNamedChildLoggersLookupConfig? loggersConfig;

    public FLoggerTreeCommonConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerTreeCommonConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerTreeCommonConfig(string name, FLogLevel logLevel
       ,IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IFLogEntryPoolConfig? logEntryPool = null )
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool)
    {
    }

    public FLoggerTreeCommonConfig(IConfigurationRoot root, string path, string name, FLogLevel logLevel
      ,IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IFLogEntryPoolConfig? logEntryPool = null ) : base(root, path)
    {
        Name              = name;
        LogLevel          = logLevel;
        DescendantLoggers = loggersCfg! ;
        Appenders         = appendersCfg!;
        LogEntryPool      = logEntryPool;
    }

    public FLoggerTreeCommonConfig(IFLoggerTreeCommonConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Name              = toClone.Name;
        LogLevel          = toClone.LogLevel;
        DescendantLoggers = toClone.DescendantLoggers ;
        Appenders         = (IAppendableNamedAppendersLookupConfig)toClone.Appenders;
        LogEntryPool      = toClone.LogEntryPool;
    }

    public FLoggerTreeCommonConfig(IFLoggerTreeCommonConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

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

    public INamedChildLoggersLookupConfig DescendantLoggers
    {
        get
        {
            if (loggersConfig == null)
            {
                if (GetSection(nameof(DescendantLoggers)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return loggersConfig = new NamedChildLoggersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(DescendantLoggers)}");
                }
            }
            return loggersConfig ?? throw new ConfigurationErrorsException($"Expected {nameof(DescendantLoggers)} to be configured");
        }
        set => loggersConfig = new NamedChildLoggersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DescendantLoggers)}");
    }

    public IFLogEntryPoolConfig? LogEntryPool
    {
        get
        {
            if (GetSection(nameof(LogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}");
            }
            return null;
        }
        set => _ = value != null ? new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}") : null;
    }
    public FLogLevel LogLevel
    {
        get =>
            Enum.TryParse<FLogLevel>(this[nameof(LogLevel)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : FLogLevel.None;
        set => this[nameof(LogLevel)] = value.ToString();
    }

    public virtual string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    object ICloneable.     Clone() => Clone();

    IFLoggerTreeCommonConfig ICloneable<IFLoggerTreeCommonConfig>.Clone() => Clone();

    public virtual FLoggerTreeCommonConfig Clone() => new (this);


    public virtual bool AreEquivalent(IFLoggerTreeCommonConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        
        var nameSame     = Name == other.Name;
        var appendersSame    = Appenders.AreEquivalent(other.Appenders, exactTypes);
        var loggersSame      = DescendantLoggers.AreEquivalent(other.DescendantLoggers, exactTypes);
        var logEntryPoolSame = LogEntryPool?.AreEquivalent(other.LogEntryPool, exactTypes) ?? other.LogEntryPool == null;
        var logLevelSame     = LogLevel == other.LogLevel;

        var allAreSame = nameSame && appendersSame && loggersSame && logLevelSame && logEntryPoolSame && logLevelSame;

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

    public virtual void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(FLoggerTreeCommonConfig))
           .AddTypeStart()
           .AddField(nameof(Name), Name)
           .AddField(nameof(LogLevel), LogLevel.ToString())
           .AddField(nameof(DescendantLoggers), DescendantLoggers)
           .AddNonNullField(nameof(Appenders), Appenders)
           .AddNonNullField(nameof(LogEntryPool), LogEntryPool)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
