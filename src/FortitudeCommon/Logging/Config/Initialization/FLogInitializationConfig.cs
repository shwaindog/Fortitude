// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization;

public interface IFLogInitializationConfig : IFLogConfig, IInterfacesComparable<IFLogInitializationConfig>
  , IConfigCloneTo<IFLogInitializationConfig>, IStyledToStringObject
{
    IAsyncQueuesInitConfig AsyncBufferingInit { get; }

    ILogEntryPoolsInitializationConfig LogEntryPoolsInit { get; }
}

public interface IMutableFLogInitializationConfig : IFLogInitializationConfig, IMutableFLogConfig
{
    new IMutableAsyncQueuesInitConfig AsyncBufferingInit { get; set; }

    new IMutableLogEntryPoolsInitializationConfig LogEntryPoolsInit { get; set; }
}

public class FLogInitializationConfig : FLogConfig, IMutableFLogInitializationConfig
{
    public FLogInitializationConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogInitializationConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogInitializationConfig
    (IMutableAsyncQueuesInitConfig? asyncBufferingInit = null
      , IMutableLogEntryPoolsInitializationConfig? logEntryPoolsInit = null)
        : this(InMemoryConfigRoot, InMemoryPath, asyncBufferingInit, logEntryPoolsInit) { }

    public FLogInitializationConfig
    (IConfigurationRoot root, string path
      , IMutableAsyncQueuesInitConfig? asyncBufferingInit = null
      , IMutableLogEntryPoolsInitializationConfig? logEntryPoolsInit = null)
        : base(root, path)
    {
        AsyncBufferingInit = asyncBufferingInit ??
                             new AsyncQueuesInitConfig(ConfigRoot, $"{Path}{Split}{nameof(AsyncBufferingInit)}");

        LogEntryPoolsInit = logEntryPoolsInit ??
                            new LogEntryPoolsInitializationConfig(ConfigRoot, $"{Path}{Split}{nameof(LogEntryPoolsInit)}");
    }

    public FLogInitializationConfig(IFLogInitializationConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AsyncBufferingInit = (IMutableAsyncQueuesInitConfig)toClone.AsyncBufferingInit;

        LogEntryPoolsInit = (IMutableLogEntryPoolsInitializationConfig)toClone.LogEntryPoolsInit;
    }

    public FLogInitializationConfig(IFLogInitializationConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    IAsyncQueuesInitConfig IFLogInitializationConfig.AsyncBufferingInit => AsyncBufferingInit;

    public IMutableAsyncQueuesInitConfig AsyncBufferingInit
    {
        get =>
            new AsyncQueuesInitConfig(ConfigRoot, $"{Path}{Split}{nameof(AsyncBufferingInit)}")
            {
                ParentConfig = this
            };
        set
        {
            _ = new AsyncQueuesInitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AsyncBufferingInit)}");

            value.ParentConfig = this;
        }
    }

    ILogEntryPoolsInitializationConfig IFLogInitializationConfig.LogEntryPoolsInit => LogEntryPoolsInit;

    public IMutableLogEntryPoolsInitializationConfig LogEntryPoolsInit
    {
        get =>
            new LogEntryPoolsInitializationConfig(ConfigRoot, $"{Path}{Split}{nameof(LogEntryPoolsInit)}")
            {
                ParentConfig = this
            };
        set
        {
            _ = new LogEntryPoolsInitializationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LogEntryPoolsInit)}");

            value.ParentConfig = this;
        }
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IFLogInitializationConfig IConfigCloneTo<IFLogInitializationConfig>.CloneConfigTo
        (IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public FLogInitializationConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IFLogInitializationConfig ICloneable<IFLogInitializationConfig>.Clone() => Clone();

    public virtual FLogInitializationConfig Clone() => new(this);

    public virtual bool AreEquivalent(IFLogInitializationConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var asyncBufferingSame = AsyncBufferingInit.AreEquivalent(other.AsyncBufferingInit, exactTypes);
        var logEntryPoolSame   = LogEntryPoolsInit.AreEquivalent(other.LogEntryPoolsInit, exactTypes);

        var allAreSame = asyncBufferingSame && logEntryPoolSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogInitializationConfig, true);

    public override int GetHashCode()
    {
        var hashCode = AsyncBufferingInit.GetHashCode();
        hashCode = (hashCode * 397) ^ LogEntryPoolsInit.GetHashCode();
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AsyncBufferingInit), AsyncBufferingInit)
           .Field.AlwaysAdd(nameof(LogEntryPoolsInit), LogEntryPoolsInit)
           .Complete();
}
