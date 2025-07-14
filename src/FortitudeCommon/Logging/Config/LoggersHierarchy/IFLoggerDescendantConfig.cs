// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerDescendantConfig : IFLoggerTreeCommonConfig, ICloneable<IFLoggerDescendantConfig>
{
    IFLoggerTreeCommonConfig ParentLoggerConfig { get; }

    bool Inherits { get; }

    string ResolveFullName();

    new IFLoggerDescendantConfig Clone();
}

public interface IMutableFLoggerDescendantConfig : IFLoggerDescendantConfig, IMutableFLoggerTreeCommonConfig
{
    new IFLoggerTreeCommonConfig ParentLoggerConfig { get; set; }

    new bool Inherits { get; set; }
}

public class FLoggerDescendantConfig : FLoggerTreeCommonConfig, IMutableFLoggerDescendantConfig
{
    public FLoggerDescendantConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerDescendantConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerDescendantConfig
    (string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerDescendantConfig
    (IConfigurationRoot root, string path, string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IFLogEntryPoolConfig? logEntryPool = null) : base(root, path, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerDescendantConfig(IFLoggerDescendantConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) { }

    public FLoggerDescendantConfig(IFLoggerDescendantConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool Inherits
    {
        get => !bool.TryParse(this[nameof(Inherits)], out var disabled) || disabled;
        set => this[nameof(Inherits)] = value.ToString();
    }

    public IFLoggerTreeCommonConfig ParentLoggerConfig { get; set; } = null!;

    public string ResolveFullName() => throw new NotImplementedException();

    IFLoggerDescendantConfig ICloneable<IFLoggerDescendantConfig>.Clone() => Clone();

    IFLoggerDescendantConfig IFLoggerDescendantConfig.Clone() => Clone();

    public override FLoggerDescendantConfig Clone() => new(this);


    public override bool AreEquivalent(IFLoggerTreeCommonConfig? other, bool exactTypes = false)
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

    public override void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(FLoggerTreeCommonConfig))
           .AddTypeStart()
           .AddField(nameof(Name), Name)
           .AddField(nameof(LogLevel), LogLevel.ToString())
           .AddField(nameof(Inherits), Inherits)
           .AddField(nameof(DescendantLoggers), DescendantLoggers)
           .AddNonNullField(nameof(LogEntryPool), LogEntryPool)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
