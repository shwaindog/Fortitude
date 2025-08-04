using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public interface IAsyncQueuesInitConfig : IFLogConfig, IInterfacesComparable<IAsyncQueuesInitConfig>
  , IConfigCloneTo<IAsyncQueuesInitConfig>, IStyledToStringObject
{
    AsyncProcessingType AsyncProcessingType { get; }

    int DefaultBufferQueueSize { get; }

    FullQueueHandling DefaultFullQueueHandling { get; }

    int DefaultDropInterval { get; }

    int InitialAsyncProcessing { get; }

    int MaxAsyncProcessing { get; }

    int DefaultAppenderAsyncQueueNumber { get; }

    int DefaultAppenderFlushQueueNumber { get; }

    IAsyncQueueLookupConfig AsyncQueues { get; }
}

public interface IMutableAsyncQueuesInitConfig : IAsyncQueuesInitConfig, IMutableFLogConfig
{
    new AsyncProcessingType AsyncProcessingType { get; set; }

    new int DefaultBufferQueueSize { get; set; }

    new FullQueueHandling DefaultFullQueueHandling { get; set; }

    new int DefaultDropInterval { get; set; }

    new int InitialAsyncProcessing { get; set; }

    new int MaxAsyncProcessing { get; set; }

    new int DefaultAppenderAsyncQueueNumber { get; set; }

    new int DefaultAppenderFlushQueueNumber { get; set; }

    new IAppendableAsyncQueueLookupConfig AsyncQueues { get; set; }
}

public class AsyncQueuesInitConfig : FLogConfig, IMutableAsyncQueuesInitConfig
{
    private IAppendableAsyncQueueLookupConfig? asyncQueuesLookup;
    public AsyncQueuesInitConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AsyncQueuesInitConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AsyncQueuesInitConfig
    (AsyncProcessingType asyncProcessingType
      , int defaultAppenderAsyncQueueNumber
      , int defaultAppenderFlushQueueNumber
      , int defaultBufferEntriesSize
      , FullQueueHandling defaultBufferFullHandling
      , int defaultDropInterval
      , int initialAsyncProcessing
      , int maxAsyncProcessing)
        : this(InMemoryConfigRoot, InMemoryPath, asyncProcessingType, defaultAppenderAsyncQueueNumber
             , defaultAppenderFlushQueueNumber, defaultBufferEntriesSize, defaultBufferFullHandling, defaultDropInterval
             , initialAsyncProcessing, maxAsyncProcessing) { }

    public AsyncQueuesInitConfig
    (IConfigurationRoot root, string path
      , AsyncProcessingType asyncProcessingType
      , int defaultAppenderAsyncQueueNumber
      , int defaultAppenderFlushQueueNumber
      , int defaultBufferEntriesSize
      , FullQueueHandling defaultBufferFullHandling
      , int defaultDropInterval
      , int initialAsyncProcessing
      , int maxAsyncProcessing) : base(root, path)
    {
        AsyncProcessingType             = asyncProcessingType;
        DefaultAppenderAsyncQueueNumber = defaultAppenderAsyncQueueNumber;
        DefaultAppenderFlushQueueNumber = defaultAppenderFlushQueueNumber;
        DefaultBufferQueueSize          = defaultBufferEntriesSize;
        DefaultFullQueueHandling        = defaultBufferFullHandling;
        DefaultDropInterval             = defaultDropInterval;
        InitialAsyncProcessing          = initialAsyncProcessing;
        MaxAsyncProcessing              = maxAsyncProcessing;
    }

    public AsyncQueuesInitConfig
        (IAsyncQueuesInitConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AsyncProcessingType             = toClone.AsyncProcessingType;
        DefaultAppenderAsyncQueueNumber = toClone.DefaultAppenderAsyncQueueNumber;
        DefaultAppenderFlushQueueNumber = toClone.DefaultAppenderFlushQueueNumber;
        DefaultBufferQueueSize          = toClone.DefaultBufferQueueSize;
        DefaultFullQueueHandling        = toClone.DefaultFullQueueHandling;
        DefaultDropInterval             = toClone.DefaultDropInterval;
        InitialAsyncProcessing          = toClone.InitialAsyncProcessing;
        MaxAsyncProcessing              = toClone.MaxAsyncProcessing;
    }

    public AsyncQueuesInitConfig
        (IAsyncQueuesInitConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public AsyncProcessingType AsyncProcessingType
    {
        get =>
            Enum.TryParse<AsyncProcessingType>(this[nameof(AsyncProcessingType)], out var poolScope)
                ? poolScope
                : AsyncProcessingType.ConfigDefinedAsyncThreads;
        set => this[nameof(AsyncProcessingType)] = value.ToString();
    }

    public int DefaultAppenderAsyncQueueNumber
    {
        get => int.TryParse(this[nameof(DefaultAppenderAsyncQueueNumber)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultAppenderAsyncQueueNumber)] = value.ToString();
    }

    public int DefaultAppenderFlushQueueNumber
    {
        get => int.TryParse(this[nameof(DefaultAppenderFlushQueueNumber)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultAppenderFlushQueueNumber)] = value.ToString();
    }

    public int DefaultBufferQueueSize
    {
        get => int.TryParse(this[nameof(DefaultBufferQueueSize)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultBufferQueueSize)] = value.ToString();
    }

    public FullQueueHandling DefaultFullQueueHandling
    {
        get =>
            Enum.TryParse<FullQueueHandling>(this[nameof(DefaultFullQueueHandling)], out var poolScope)
                ? poolScope
                : FullQueueHandling.Block;
        set => this[nameof(DefaultFullQueueHandling)] = value.ToString();
    }

    public int DefaultDropInterval
    {
        get => int.TryParse(this[nameof(DefaultDropInterval)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultDropInterval)] = value.ToString();
    }

    public int InitialAsyncProcessing
    {
        get => int.TryParse(this[nameof(InitialAsyncProcessing)], out var timePart) ? timePart : 0;
        set => this[nameof(InitialAsyncProcessing)] = value.ToString();
    }

    public int MaxAsyncProcessing
    {
        get => int.TryParse(this[nameof(MaxAsyncProcessing)], out var timePart) ? timePart : 0;
        set => this[nameof(MaxAsyncProcessing)] = value.ToString();
    }

    IAsyncQueueLookupConfig IAsyncQueuesInitConfig.AsyncQueues => AsyncQueues;

    public IAppendableAsyncQueueLookupConfig AsyncQueues
    {
        get
        {
            return asyncQueuesLookup ??= new AsyncQueueLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(AsyncQueues)}")
            {
                ParentConfig = this
            };
        }
        set =>
            asyncQueuesLookup = new AsyncQueueLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AsyncQueues)}")
            {
                ParentConfig = this
            };
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IAsyncQueuesInitConfig ICloneable<IAsyncQueuesInitConfig>.Clone() => Clone();

    public AsyncQueuesInitConfig Clone() => new(this);

    IAsyncQueuesInitConfig IConfigCloneTo<IAsyncQueuesInitConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public AsyncQueuesInitConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public bool AreEquivalent(IAsyncQueuesInitConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var asyncTypeSame         = AsyncProcessingType == other.AsyncProcessingType;
        var queueSizeSame         = DefaultBufferQueueSize == other.DefaultBufferQueueSize;
        var fullQueueHandlingSame = DefaultFullQueueHandling == other.DefaultFullQueueHandling;
        var dropIntervalSame      = DefaultDropInterval == other.DefaultDropInterval;
        var initAsyncNumSame      = InitialAsyncProcessing == other.InitialAsyncProcessing;
        var maxAsyncNumSame       = MaxAsyncProcessing == other.MaxAsyncProcessing;
        var appenderQueueNumSame  = DefaultAppenderAsyncQueueNumber == other.DefaultAppenderAsyncQueueNumber;

        var appenderFlushQueueNumSame = DefaultAppenderFlushQueueNumber == other.DefaultAppenderFlushQueueNumber;

        var asyncQueuesConfigSame = AsyncQueues.AreEquivalent(other.AsyncQueues);

        var allAreSame = asyncTypeSame && queueSizeSame && fullQueueHandlingSame && dropIntervalSame
                      && initAsyncNumSame && maxAsyncNumSame && appenderQueueNumSame && appenderFlushQueueNumSame
                         && asyncQueuesConfigSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAsyncQueuesInitConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AsyncProcessingType.GetHashCode();
            hashCode = (hashCode * 397) ^ DefaultBufferQueueSize;
            hashCode = (hashCode * 397) ^ (int)DefaultFullQueueHandling;
            hashCode = (hashCode * 397) ^ DefaultDropInterval;
            hashCode = (hashCode * 397) ^ InitialAsyncProcessing;
            hashCode = (hashCode * 397) ^ MaxAsyncProcessing;
            hashCode = (hashCode * 397) ^ DefaultAppenderAsyncQueueNumber;
            hashCode = (hashCode * 397) ^ DefaultAppenderFlushQueueNumber;
            hashCode = (hashCode * 397) ^ AsyncQueues.GetHashCode();
            return hashCode;
        }
    }

    public IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(AsyncQueuesInitConfig))
               .AddTypeStart()
               .AddField(nameof(AsyncProcessingType), AsyncProcessingType, AsyncProcessingTypesExtensions.AsyncProcessingTypeFormatter)
               .AddField(nameof(DefaultBufferQueueSize), DefaultBufferQueueSize)
               .AddField(nameof(DefaultFullQueueHandling), DefaultFullQueueHandling, FullQueueHandlingExtensions.FullQueueHandlingFormatter)
               .AddField(nameof(DefaultDropInterval), DefaultDropInterval)
               .AddField(nameof(InitialAsyncProcessing), InitialAsyncProcessing)
               .AddField(nameof(MaxAsyncProcessing), MaxAsyncProcessing)
               .AddField(nameof(DefaultAppenderAsyncQueueNumber), DefaultAppenderAsyncQueueNumber)
               .AddField(nameof(DefaultAppenderFlushQueueNumber), DefaultAppenderFlushQueueNumber)
               .AddNonNullAndPopulatedCollectionField(nameof(AsyncQueues), (IEnumerable<KeyValuePair<byte, IMutableAsyncQueueConfig>>)AsyncQueues)
               .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
