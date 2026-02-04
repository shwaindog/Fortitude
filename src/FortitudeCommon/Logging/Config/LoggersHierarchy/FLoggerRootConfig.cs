// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerRootConfig : IFLoggerTreeCommonConfig, IConfigCloneTo<IFLoggerRootConfig>
{
    INamedChildLoggersLookupConfig AllLoggers();

    IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName);

    new IFLoggerRootConfig Clone();
    new IFLoggerRootConfig CloneConfigTo(IConfigurationRoot configRoot, string path);
}

public interface IMutableFLoggerRootConfig : IFLoggerRootConfig, IMutableFLoggerTreeCommonConfig, ICloneable<IMutableFLoggerRootConfig>
{
    new IMutableNamedChildLoggersLookupConfig AllLoggers();

    new IMutableFLoggerRootConfig Clone();
}

public class FLoggerRootConfig : FLoggerTreeCommonConfig, IMutableFLoggerRootConfig
{
    public FLoggerRootConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerRootConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerRootConfig
    (string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerRootConfig
    (IConfigurationRoot root, string path, string name, FLogLevel logLevel
      , IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null) : base(root, path, name, logLevel, loggersCfg, appendersCfg, logEntryPool) { }

    public FLoggerRootConfig(IFLoggerRootConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) { }

    public FLoggerRootConfig(IFLoggerRootConfig toClone) : this(toClone, InMemoryConfigRoot, toClone.ConfigSubPath) { }

    public INamedChildLoggersLookupConfig AllLoggers() => ((IMutableFLoggerRootConfig)this).AllLoggers();

    IMutableNamedChildLoggersLookupConfig IMutableFLoggerRootConfig.AllLoggers()
    {
        var results = new NamedChildLoggersLookupConfig();
        foreach (var childLoggerConfig in Accept(new AllLoggers())) results.Add(childLoggerConfig.FullName, childLoggerConfig);
        return results;
    }

    public IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName) => throw new NotImplementedException();

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IFLoggerRootConfig IConfigCloneTo<IFLoggerRootConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        CloneConfigTo(configRoot, path);

    IFLoggerRootConfig IFLoggerRootConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    public override FLoggerRootConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new (this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IFLoggerRootConfig ICloneable<IFLoggerRootConfig>.Clone() => Clone();

    IFLoggerRootConfig IFLoggerRootConfig.Clone() => Clone();

    IMutableFLoggerRootConfig ICloneable<IMutableFLoggerRootConfig>.Clone() => Clone();

    IMutableFLoggerRootConfig IMutableFLoggerRootConfig.Clone() => Clone();

    public override FLoggerRootConfig Clone() => new(this);

    public override bool AreEquivalent(IFLoggerMatchedAppenders? other, bool exactTypes = false)
    {
        if (other is not IFLoggerRootConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var allAreSame = baseSame;

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

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(Name), Name)
           .Field.AlwaysAdd(nameof(LogLevel), LogLevel.ToString())
           .Field.AlwaysReveal(nameof(DescendantLoggers), DescendantLoggers)
           .Field.WhenNonNullReveal(nameof(Appenders), Appenders)
           .Field.WhenNonNullReveal(nameof(LogEntryPool), LogEntryPool)
           .Complete();
}
