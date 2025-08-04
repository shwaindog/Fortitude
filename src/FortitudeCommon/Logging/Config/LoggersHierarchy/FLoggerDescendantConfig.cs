﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerDescendantConfig : IFLoggerTreeCommonConfig, ICloneable<IFLoggerDescendantConfig>
{
    IFLoggerTreeCommonConfig ParentLoggerConfig { get; }

    bool Inherits { get; }

    new IFLoggerDescendantConfig Clone();
}

public interface IMutableFLoggerDescendantConfig : IFLoggerDescendantConfig, IMutableFLoggerTreeCommonConfig
  , IConfigCloneTo<IMutableFLoggerDescendantConfig>
{
    new IFLoggerTreeCommonConfig ParentLoggerConfig { get; set; }

    new bool Inherits { get; set; }
    
    IMutableFLoggerDescendantConfig CreateInheritedDescendantConfig(IFLoggerTreeCommonConfig ancestorConfig);

    new IMutableFLoggerDescendantConfig Clone();
}

public class FLoggerDescendantConfig : FLoggerTreeCommonConfig, IMutableFLoggerDescendantConfig
{
    public FLoggerDescendantConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerDescendantConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerDescendantConfig
    (string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerDescendantConfig
    (IConfigurationRoot root, string path, string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : base(root, path, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerDescendantConfig(IFLoggerDescendantConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        Inherits = toClone.Inherits;
    }

    public FLoggerDescendantConfig(IFLoggerDescendantConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerDescendantConfig(IFLoggerTreeCommonConfig toClone, string? configPath = null)
        : base(toClone, InMemoryConfigRoot, configPath ?? toClone.ConfigSubPath)
    {
        if (toClone is IFLoggerDescendantConfig descendantConfig)
        {
            Inherits = descendantConfig.Inherits;
        }
    }

    public bool Inherits
    {
        get => !bool.TryParse(this[nameof(Inherits)], out var disabled) || disabled;
        set => this[nameof(Inherits)] = value.ToString();
    }

    public IFLoggerTreeCommonConfig ParentLoggerConfig { get; set; } = null!;

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public IMutableFLoggerDescendantConfig CreateInheritedDescendantConfig(IFLoggerTreeCommonConfig ancestorConfig)
    {
        var mergedInheritedConfig = Clone();
        if (mergedInheritedConfig.WasDefinedLogLevel == null)
        {
            mergedInheritedConfig.LogLevel = ancestorConfig.LogLevel;
        }
        if (!Inherits)
        {
            return mergedInheritedConfig;
        }
        foreach (var parentAppender in ancestorConfig.Appenders)
        {
            if (!Appenders.ContainsKey(parentAppender.Key))
            {
                Appenders.Add(parentAppender.Value);
            }
        }
        return mergedInheritedConfig;
    }

    IFLoggerDescendantConfig ICloneable<IFLoggerDescendantConfig>.Clone() => Clone();

    IFLoggerDescendantConfig IFLoggerDescendantConfig.Clone() => Clone();

    IMutableFLoggerDescendantConfig ICloneable<IMutableFLoggerDescendantConfig>.Clone() => Clone();

    IMutableFLoggerDescendantConfig IMutableFLoggerDescendantConfig.Clone() => Clone();

    public override FLoggerDescendantConfig Clone() => new(this);

    public IMutableFLoggerDescendantConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new FLoggerDescendantConfig(this, configRoot, path);

    public override bool AreEquivalent(IFLoggerMatchedAppenders? other, bool exactTypes = false)
    {
        if (other is not IFLoggerDescendantConfig descendantConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var inheritsSame = Inherits == descendantConfig.Inherits;

        var allAreSame = baseSame && inheritsSame;

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

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(FLoggerTreeCommonConfig))
               .Field.AddAlways(nameof(Name), Name)
               .Field.AddAlways(nameof(LogLevel), LogLevel.ToString())
               .Field.AddAlways(nameof(Inherits), Inherits)
               .Field.AddAlways(nameof(DescendantLoggers), DescendantLoggers)
               .Field.AddWhenNonNull(nameof(LogEntryPool), LogEntryPool).Complete();
    }

    public override string ToString() => this.DefaultToString();
}
