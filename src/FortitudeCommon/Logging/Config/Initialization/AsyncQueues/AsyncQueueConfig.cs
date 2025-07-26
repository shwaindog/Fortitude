using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public interface IAsyncQueueConfig : IFLogConfig, IStyledToStringObject
  , IInterfacesComparable<IAsyncQueueConfig>, IConfigCloneTo<IAsyncQueueConfig>
{
    byte QueueNumber { get; }

    AsyncProcessingType QueueType { get; }

    int QueueCapacity { get; }

    bool LaunchAtFlogStart { get; }

    FullQueueHandling QueueFullHandling { get; }

    int QueueFullDropInterval { get; }

    IAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig { get; }
}

public interface IMutableAsyncQueueConfig : IAsyncQueueConfig, IMutableFLogConfig
{
    new byte QueueNumber { get; set; }

    new AsyncProcessingType QueueType { get; set; }

    new int QueueCapacity { get; set; }

    new bool LaunchAtFlogStart { get; set; }

    new FullQueueHandling QueueFullHandling { get; set; }

    new int QueueFullDropInterval { get; set; }

    new IMutableAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig { get; }
}

public class AsyncQueueConfig : FLogConfig, IMutableAsyncQueueConfig
{
    public AsyncQueueConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AsyncQueueConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AsyncQueueConfig (byte queueNumber
      , AsyncProcessingType queueType
      , int queueCapacity
      , bool launchAtFlogStart
      , FullQueueHandling queueFullHandling
      , int defaultDropInterval)
        : this(InMemoryConfigRoot, InMemoryPath, queueNumber, queueType, queueCapacity, launchAtFlogStart
             , queueFullHandling, defaultDropInterval) { }

    public AsyncQueueConfig
    (IConfigurationRoot root, string path
      , byte queueNumber
      , AsyncProcessingType queueType
      , int queueCapacity
      , bool launchAtFlogStart
      , FullQueueHandling queueFullHandling
      , int queueFullDropInterval) : base(root, path)
    {
        QueueNumber           = queueNumber;
        QueueType             = queueType;
        QueueCapacity         = queueCapacity;
        LaunchAtFlogStart     = launchAtFlogStart;
        QueueFullHandling     = queueFullHandling;
        QueueFullDropInterval = queueFullDropInterval;
    }

    public AsyncQueueConfig
        (IAsyncQueueConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        QueueNumber           = toClone.QueueNumber;
        QueueType             = toClone.QueueType;
        QueueCapacity         = toClone.QueueCapacity;
        LaunchAtFlogStart     = toClone.LaunchAtFlogStart;
        QueueFullHandling     = toClone.QueueFullHandling;
        QueueFullDropInterval = toClone.QueueFullDropInterval;
    }

    public AsyncQueueConfig
        (IAsyncQueueConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public byte QueueNumber
    {
        get => byte.TryParse(this[nameof(QueueNumber)], out var queueNum) ? queueNum : (byte)0;
        set => this[nameof(QueueNumber)] = value.ToString();
    }

    public AsyncProcessingType QueueType
    {
        get =>
            Enum.TryParse<AsyncProcessingType>(this[nameof(QueueType)], out var queueType)
                ? queueType
                : ParentDefaultQueuesInitConfig?.AsyncProcessingType ?? AsyncProcessingType.Synchronise;
        set => this[nameof(QueueType)] = value.ToString();
    }

    public int QueueCapacity
    {
        get => int.TryParse(this[nameof(QueueCapacity)], out var queueCapacity) 
            ? queueCapacity 
            : ParentDefaultQueuesInitConfig?.DefaultBufferQueueSize ?? 1024;
        set => this[nameof(QueueCapacity)] = value.ToString();
    }

    public bool LaunchAtFlogStart
    {
        get => bool.TryParse(this[nameof(LaunchAtFlogStart)], out var launchAtStart) && launchAtStart;
        set => this[nameof(LaunchAtFlogStart)] = value.ToString();
    }

    public FullQueueHandling QueueFullHandling
    {
        get =>
            Enum.TryParse<FullQueueHandling>(this[nameof(QueueFullHandling)], out var poolScope)
                ? poolScope
                : ParentDefaultQueuesInitConfig?.DefaultFullQueueHandling ?? FullQueueHandling.Block;
        set => this[nameof(QueueFullHandling)] = value.ToString();
    }

    public int QueueFullDropInterval
    {
        get =>
            int.TryParse(this[nameof(QueueFullDropInterval)], out var fullQueueDropInterval)
                ? fullQueueDropInterval
                : ParentDefaultQueuesInitConfig?.DefaultDropInterval ?? 1;
        set => this[nameof(QueueFullDropInterval)] = value.ToString();
    }

    IAsyncQueuesInitConfig? IAsyncQueueConfig.ParentDefaultQueuesInitConfig => ParentDefaultQueuesInitConfig;

    public IMutableAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig => 
        (ParentConfig as IAppendableAsyncQueueLookupConfig)?.ParentDefaultQueuesInitConfig ??  
        FLogContext.Context.AsyncRegistry.AsyncBufferingConfig as IMutableAsyncQueuesInitConfig;

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IAsyncQueueConfig ICloneable<IAsyncQueueConfig>.Clone() => Clone();

    public AsyncQueueConfig Clone() => new(this);

    IAsyncQueueConfig IConfigCloneTo<IAsyncQueueConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public AsyncQueueConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public bool AreEquivalent(IAsyncQueueConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var queueNumberSame           = QueueNumber == other.QueueNumber;
        var queueTypeSame             = QueueType == other.QueueType;
        var queueCapacitySame         = QueueCapacity == other.QueueCapacity;
        var launchAtFlogStartSame     = LaunchAtFlogStart == other.LaunchAtFlogStart;
        var queueFullHandlingSame     = QueueFullHandling == other.QueueFullHandling;
        var queueFullDropIntervalSame = QueueFullDropInterval == other.QueueFullDropInterval;

        var allAreSame = queueNumberSame && queueTypeSame && queueCapacitySame && launchAtFlogStartSame && queueFullHandlingSame
                      && queueFullDropIntervalSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAsyncQueueConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)QueueNumber;
            hashCode = (hashCode * 397) ^ (int)QueueType;
            hashCode = (hashCode * 397) ^ QueueCapacity;
            hashCode = (hashCode * 397) ^ LaunchAtFlogStart.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)QueueFullHandling;
            hashCode = (hashCode * 397) ^ QueueFullDropInterval;
            return hashCode;
        }
    }

    public IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(AsyncQueueConfig))
               .AddTypeStart()
               .AddField(nameof(QueueNumber), QueueNumber)
               .AddField(nameof(QueueType), QueueType, AsyncProcessingTypesExtensions.AsyncProcessingTypeFormatter)
               .AddField(nameof(QueueCapacity), QueueCapacity)
               .AddField(nameof(LaunchAtFlogStart), LaunchAtFlogStart)
               .AddField(nameof(QueueFullHandling), QueueFullHandling, FullQueueHandlingExtensions.FullQueueHandlingFormatter)
               .AddField(nameof(QueueFullDropInterval), QueueFullDropInterval)
               .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
