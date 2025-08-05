using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization;

public interface ILogEntryPoolsInitializationConfig : IFLogConfig
  , IInterfacesComparable<ILogEntryPoolsInitializationConfig>, IConfigCloneTo<ILogEntryPoolsInitializationConfig>
  , IStyledToStringObject
{
    int DefaultLogEntryCharCapacity { get; }

    int DefaultLogEntryBatchSize { get; }

    IFLogEntryPoolConfig GlobalLogEntryPool { get; }

    IFLogEntryPoolConfig LargeMessageLogEntryPool { get; }

    IFLogEntryPoolConfig VeryLargeMessageLogEntryPool { get; }

    IFLogEntryPoolConfig LoggersGlobalLogEntryPool { get; }

    IFLogEntryPoolConfig AppendersGlobalLogEntryPool { get; }
}

public interface IMutableLogEntryPoolsInitializationConfig : ILogEntryPoolsInitializationConfig, IMutableFLogConfig
{
    new int DefaultLogEntryCharCapacity { get; set; }

    new int DefaultLogEntryBatchSize { get; set; }

    new IMutableFLogEntryPoolConfig GlobalLogEntryPool { get; set; }

    new IMutableFLogEntryPoolConfig LargeMessageLogEntryPool { get; set; }

    new IMutableFLogEntryPoolConfig VeryLargeMessageLogEntryPool { get; set; }

    new IMutableFLogEntryPoolConfig LoggersGlobalLogEntryPool { get; set; }

    new IMutableFLogEntryPoolConfig AppendersGlobalLogEntryPool { get; set; }
}

public class LogEntryPoolsInitializationConfig : FLogConfig, IMutableLogEntryPoolsInitializationConfig
{
    public LogEntryPoolsInitializationConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public LogEntryPoolsInitializationConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public LogEntryPoolsInitializationConfig
    (int? defaultLogEntryCharCapacity = null
      , int? defaultLogEntryBatchSize = null
      , IMutableFLogEntryPoolConfig? globalLogEntryPool = null
      , IMutableFLogEntryPoolConfig? largeMessageLogEntryPool = null
      , IMutableFLogEntryPoolConfig? veryLargeMessageLogEntryPool = null
      , IMutableFLogEntryPoolConfig? loggersGlobalLogEntryPool = null
      , IMutableFLogEntryPoolConfig? appendersGlobalLogEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, defaultLogEntryCharCapacity, defaultLogEntryBatchSize, globalLogEntryPool
             , largeMessageLogEntryPool, veryLargeMessageLogEntryPool, loggersGlobalLogEntryPool
             , appendersGlobalLogEntryPool) { }

    public LogEntryPoolsInitializationConfig
    (IConfigurationRoot root, string path
      , int? defaultLogEntryCharCapacity = null
      , int? defaultLogEntryBatchSize = null
      , IMutableFLogEntryPoolConfig? globalLogEntryPool = null
      , IMutableFLogEntryPoolConfig? largeMessageLogEntryPool = null
      , IMutableFLogEntryPoolConfig? veryLargeMessageLogEntryPool = null
      , IMutableFLogEntryPoolConfig? loggersGlobalLogEntryPool = null
      , IMutableFLogEntryPoolConfig? appendersGlobalLogEntryPool = null)
        : base(root, path)
    {
        DefaultLogEntryCharCapacity = defaultLogEntryCharCapacity ?? IFLogEntryPoolConfig.DefaultLogEntryCharsCapacity;
        DefaultLogEntryBatchSize    = defaultLogEntryBatchSize ?? IFLogEntryPoolConfig.DefaultLogEntryBatchSize;

        GlobalLogEntryPool =
            globalLogEntryPool ??
            new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                  , IFLogEntryPoolConfig.Global, poolScope: PoolScope.Global);
        LargeMessageLogEntryPool =
            largeMessageLogEntryPool ??
            new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                  , IFLogEntryPoolConfig.Global, poolScope: PoolScope.Global);
        VeryLargeMessageLogEntryPool =
            veryLargeMessageLogEntryPool ??
            new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                  , IFLogEntryPoolConfig.VeryLargeMessage, poolScope: PoolScope.VeryLargeMessage);
        LoggersGlobalLogEntryPool =
            loggersGlobalLogEntryPool ??
            new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                  , IFLogEntryPoolConfig.LoggersGlobal, poolScope: PoolScope.LoggersGlobal);
        AppendersGlobalLogEntryPool =
            appendersGlobalLogEntryPool ??
            new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                  , IFLogEntryPoolConfig.AppendersGlobal, poolScope: PoolScope.AppendersGlobal);
    }

    public LogEntryPoolsInitializationConfig(ILogEntryPoolsInitializationConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DefaultLogEntryCharCapacity = toClone.DefaultLogEntryCharCapacity;
        DefaultLogEntryBatchSize    = toClone.DefaultLogEntryBatchSize;

        GlobalLogEntryPool           = (IMutableFLogEntryPoolConfig)toClone.GlobalLogEntryPool;
        LargeMessageLogEntryPool     = (IMutableFLogEntryPoolConfig)toClone.LargeMessageLogEntryPool;
        VeryLargeMessageLogEntryPool = (IMutableFLogEntryPoolConfig)toClone.VeryLargeMessageLogEntryPool;
        LoggersGlobalLogEntryPool    = (IMutableFLogEntryPoolConfig)toClone.LoggersGlobalLogEntryPool;
        AppendersGlobalLogEntryPool  = (IMutableFLogEntryPoolConfig)toClone.AppendersGlobalLogEntryPool;
    }

    public LogEntryPoolsInitializationConfig(ILogEntryPoolsInitializationConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public int DefaultLogEntryCharCapacity
    {
        get => int.TryParse(this[nameof(DefaultLogEntryCharCapacity)], out var charCapacity) ? charCapacity : 0;
        set => this[nameof(DefaultLogEntryCharCapacity)] = value.ToString();
    }

    public int DefaultLogEntryBatchSize
    {
        get => int.TryParse(this[nameof(DefaultLogEntryBatchSize)], out var batchSize) ? batchSize : 0;
        set => this[nameof(DefaultLogEntryBatchSize)] = value.ToString();
    }

    IFLogEntryPoolConfig ILogEntryPoolsInitializationConfig.AppendersGlobalLogEntryPool => AppendersGlobalLogEntryPool;

    public IMutableFLogEntryPoolConfig AppendersGlobalLogEntryPool
    {
        get
        {
            if (GetSection(nameof(AppendersGlobalLogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                         , IFLogEntryPoolConfig.AppendersGlobal, poolScope: PoolScope.AppendersGlobal)
            {
                ParentConfig = this
            };
        }
        set
        {
            _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}");

            value.ParentConfig = this;
        }
    }

    IFLogEntryPoolConfig ILogEntryPoolsInitializationConfig.LoggersGlobalLogEntryPool => LoggersGlobalLogEntryPool;

    public IMutableFLogEntryPoolConfig LoggersGlobalLogEntryPool
    {
        get
        {
            if (GetSection(nameof(AppendersGlobalLogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                         , IFLogEntryPoolConfig.LoggersGlobal, poolScope: PoolScope.LoggersGlobal)
            {
                ParentConfig = this
            };
        }
        set =>
            _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
            {
                ParentConfig = this
            };
    }

    IFLogEntryPoolConfig ILogEntryPoolsInitializationConfig.GlobalLogEntryPool => GlobalLogEntryPool;

    public IMutableFLogEntryPoolConfig GlobalLogEntryPool
    {
        get
        {
            if (GetSection(nameof(AppendersGlobalLogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                         , IFLogEntryPoolConfig.Global, poolScope: PoolScope.Global)
            {
                ParentConfig = this
            };
        }
        set =>
            _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
            {
                ParentConfig = this
            };
    }

    IFLogEntryPoolConfig ILogEntryPoolsInitializationConfig.LargeMessageLogEntryPool => LargeMessageLogEntryPool;

    public IMutableFLogEntryPoolConfig LargeMessageLogEntryPool
    {
        get
        {
            if (GetSection(nameof(AppendersGlobalLogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                         , IFLogEntryPoolConfig.Global, poolScope: PoolScope.Global)
            {
                ParentConfig = this
            };
        }
        set =>
            _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
            {
                ParentConfig = this
            };
    }

    IFLogEntryPoolConfig ILogEntryPoolsInitializationConfig.VeryLargeMessageLogEntryPool => VeryLargeMessageLogEntryPool;

    public IMutableFLogEntryPoolConfig VeryLargeMessageLogEntryPool
    {
        get
        {
            if (GetSection(nameof(AppendersGlobalLogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}"
                                         , IFLogEntryPoolConfig.VeryLargeMessage, poolScope: PoolScope.VeryLargeMessage)
            {
                ParentConfig = this
            };
        }
        set =>
            _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AppendersGlobalLogEntryPool)}")
            {
                ParentConfig = this
            };
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    ILogEntryPoolsInitializationConfig ICloneable<ILogEntryPoolsInitializationConfig>.Clone() => Clone();

    public virtual LogEntryPoolsInitializationConfig Clone() => new(this);

    ILogEntryPoolsInitializationConfig IConfigCloneTo<ILogEntryPoolsInitializationConfig>.CloneConfigTo
        (IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public LogEntryPoolsInitializationConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public virtual bool AreEquivalent(ILogEntryPoolsInitializationConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var defaultCharCapacitySame = DefaultLogEntryCharCapacity == other.DefaultLogEntryCharCapacity;
        var defaultBatchSizeSame    = DefaultLogEntryBatchSize == other.DefaultLogEntryBatchSize;
        var globalSame              = GlobalLogEntryPool.AreEquivalent(other.GlobalLogEntryPool, exactTypes);
        var largeMsgSame            = LargeMessageLogEntryPool.AreEquivalent(other.LargeMessageLogEntryPool, exactTypes);
        var veryLargeMsgSame        = VeryLargeMessageLogEntryPool.AreEquivalent(other.VeryLargeMessageLogEntryPool, exactTypes);
        var loggersGlobalSame       = LoggersGlobalLogEntryPool.AreEquivalent(other.LoggersGlobalLogEntryPool, exactTypes);
        var appendersGlobalSame     = AppendersGlobalLogEntryPool.AreEquivalent(other.AppendersGlobalLogEntryPool, exactTypes);

        var allAreSame = defaultCharCapacitySame && defaultBatchSizeSame && globalSame && largeMsgSame && veryLargeMsgSame
                      && loggersGlobalSame && appendersGlobalSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILogEntryPoolsInitializationConfig, true);

    public override int GetHashCode()
    {
        var hashCode = DefaultLogEntryCharCapacity.GetHashCode();
        hashCode = (hashCode * 397) ^ DefaultLogEntryBatchSize.GetHashCode();
        hashCode = (hashCode * 397) ^ GlobalLogEntryPool.GetHashCode();
        hashCode = (hashCode * 397) ^ LargeMessageLogEntryPool.GetHashCode();
        hashCode = (hashCode * 397) ^ VeryLargeMessageLogEntryPool.GetHashCode();
        hashCode = (hashCode * 397) ^ LoggersGlobalLogEntryPool.GetHashCode();
        hashCode = (hashCode * 397) ^ AppendersGlobalLogEntryPool.GetHashCode();
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(LogEntryPoolsInitializationConfig))
               .Field.AlwaysAdd(nameof(DefaultLogEntryCharCapacity), DefaultLogEntryCharCapacity)
               .Field.AlwaysAdd(nameof(DefaultLogEntryBatchSize), DefaultLogEntryBatchSize)
               .Field.AlwaysAdd(nameof(GlobalLogEntryPool), GlobalLogEntryPool)
               .Field.AlwaysAdd(nameof(LargeMessageLogEntryPool), LargeMessageLogEntryPool)
               .Field.AlwaysAdd(nameof(VeryLargeMessageLogEntryPool), VeryLargeMessageLogEntryPool)
               .Field.AlwaysAdd(nameof(LoggersGlobalLogEntryPool), LoggersGlobalLogEntryPool)
               .Field.AlwaysAdd(nameof(AppendersGlobalLogEntryPool), AppendersGlobalLogEntryPool)
               .Complete();
    }
}
