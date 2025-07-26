using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Config.Visitor.LoggerVisitors;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface IFLoggerRootConfig : IFLoggerTreeCommonConfig, ICloneable<IFLoggerRootConfig>
{
    INamedChildLoggersLookupConfig AllLoggers();

    IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName);

    new IFLoggerRootConfig Clone();
}

public interface IMutableFLoggerRootConfig : IFLoggerRootConfig, IMutableFLoggerTreeCommonConfig
{
}

public class FLoggerRootConfig: FLoggerTreeCommonConfig, IMutableFLoggerRootConfig
{
    public FLoggerRootConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerRootConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerRootConfig(string name, FLogLevel logLevel
       ,IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null )
        : this(InMemoryConfigRoot, InMemoryPath, name, logLevel, loggersCfg, appendersCfg, logEntryPool)
    {
    }

    public FLoggerRootConfig(IConfigurationRoot root, string path, string name, FLogLevel logLevel
      ,IMutableNamedChildLoggersLookupConfig? loggersCfg = null, IAppendableNamedAppendersLookupConfig? appendersCfg = null
      , IMutableFLogEntryPoolConfig? logEntryPool = null ) : base(root, path, name, logLevel, loggersCfg, appendersCfg, logEntryPool)
    {
    }

    public FLoggerRootConfig(IFLoggerRootConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
    }

    public FLoggerRootConfig(IFLoggerRootConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INamedChildLoggersLookupConfig AllLoggers()
    {
        var results = new NamedChildLoggersLookupConfig();
        foreach (var childLoggerConfig in Visit(new AllLoggers()))
        {
            results.Add(childLoggerConfig.FullName, childLoggerConfig);
        }
        return results;
    }

    public IFLoggerDescendantConfig ResolveLoggerConfig(string loggerFullName) => throw new NotImplementedException();

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.     Clone() => Clone();

    IFLoggerRootConfig ICloneable<IFLoggerRootConfig>.Clone() => Clone();

    IFLoggerRootConfig IFLoggerRootConfig.Clone() => Clone();

    public override FLoggerRootConfig Clone() => new (this);

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

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
        sbc.AddTypeName(nameof(FLoggerTreeCommonConfig))
           .AddTypeStart()
           .AddField(nameof(Name), Name)
           .AddField(nameof(LogLevel), LogLevel.ToString())
           .AddField(nameof(DescendantLoggers), DescendantLoggers)
           .AddNonNullField(nameof(Appenders), Appenders)
           .AddNonNullField(nameof(LogEntryPool), LogEntryPool)
           .AddTypeEnd();
    }
}
