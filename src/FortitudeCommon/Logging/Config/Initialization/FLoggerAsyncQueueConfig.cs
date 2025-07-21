using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization;

public interface IFLogAsyncBufferingInitializationConfig : IFLogConfig
  , IInterfacesComparable<IFLogAsyncBufferingInitializationConfig>, IConfigCloneTo<IFLogAsyncBufferingInitializationConfig>
, IStyledToStringObject
{
    AsyncProcessingType AsyncProcessingType { get; }

    int DefaultBufferQueueSize { get; }

    int DefaultMaxBufferQueueSize { get; }

    FullQueueHandling DefaultFullQueueHandling { get; }

    int DefaultDropInterval { get; }

    int DefaultBufferBatchConsumeSize { get; }

    int InitialAsyncProcessing { get; }

    int MaxAsyncProcessing { get; }

    int DefaultAppenderAsyncQueueNumber { get; }
}

public interface IMutableFLogAsyncBufferingInitializationConfig : IFLogAsyncBufferingInitializationConfig, IMutableFLogConfig
{
    new AsyncProcessingType AsyncProcessingType { get; set; }

    new int DefaultBufferQueueSize { get; set; }

    new int DefaultMaxBufferQueueSize { get; set; }

    new FullQueueHandling DefaultFullQueueHandling { get; set; }

    new int DefaultDropInterval { get; set; }

    new int DefaultBufferBatchConsumeSize { get; set; }

    new int InitialAsyncProcessing { get; set; }

    new int MaxAsyncProcessing { get; set; }

    new int DefaultAppenderAsyncQueueNumber { get; set; }
}

public class FLogAsyncBufferingInitializationConfig : FLogConfig, IMutableFLogAsyncBufferingInitializationConfig
{
    public FLogAsyncBufferingInitializationConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogAsyncBufferingInitializationConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogAsyncBufferingInitializationConfig
    (AsyncProcessingType asyncProcessingType
      , int defaultAppenderAsyncQueueNumber
      , int defaultBufferBatchConsumeSize
      , int defaultBufferEntriesSize
      , FullQueueHandling defaultBufferFullHandling
      , int defaultDropInterval
      , int defaultMaxBufferEntriesSize
      , int initialAsyncProcessing
      , int maxAsyncProcessing)
        : this(InMemoryConfigRoot, InMemoryPath, asyncProcessingType, defaultAppenderAsyncQueueNumber, defaultBufferBatchConsumeSize
             , defaultBufferEntriesSize, defaultBufferFullHandling, defaultDropInterval, defaultMaxBufferEntriesSize
             , initialAsyncProcessing, maxAsyncProcessing) { }

    public FLogAsyncBufferingInitializationConfig
    (IConfigurationRoot root, string path
      , AsyncProcessingType asyncProcessingType
      , int defaultAppenderAsyncQueueNumber
      , int defaultBufferBatchConsumeSize
      , int defaultBufferEntriesSize
      , FullQueueHandling defaultBufferFullHandling
      , int defaultDropInterval
      , int defaultMaxBufferEntriesSize
      , int initialAsyncProcessing
      , int maxAsyncProcessing) : base(root, path)
    {
        AsyncProcessingType             = asyncProcessingType;
        DefaultAppenderAsyncQueueNumber = defaultAppenderAsyncQueueNumber;
        DefaultBufferBatchConsumeSize   = defaultBufferBatchConsumeSize;
        DefaultBufferQueueSize        = defaultBufferEntriesSize;
        DefaultFullQueueHandling       = defaultBufferFullHandling;
        DefaultDropInterval             = defaultDropInterval;
        DefaultMaxBufferQueueSize     = defaultMaxBufferEntriesSize;
        InitialAsyncProcessing          = initialAsyncProcessing;
        MaxAsyncProcessing              = maxAsyncProcessing;
    }

    public FLogAsyncBufferingInitializationConfig
        (IFLogAsyncBufferingInitializationConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AsyncProcessingType             = toClone.AsyncProcessingType;
        DefaultAppenderAsyncQueueNumber = toClone.DefaultAppenderAsyncQueueNumber;
        DefaultBufferBatchConsumeSize   = toClone.DefaultBufferBatchConsumeSize;
        DefaultBufferQueueSize        = toClone.DefaultBufferQueueSize;
        DefaultFullQueueHandling       = toClone.DefaultFullQueueHandling;
        DefaultDropInterval             = toClone.DefaultDropInterval;
        DefaultMaxBufferQueueSize     = toClone.DefaultMaxBufferQueueSize;
        InitialAsyncProcessing          = toClone.InitialAsyncProcessing;
        MaxAsyncProcessing              = toClone.MaxAsyncProcessing;
    }

    public FLogAsyncBufferingInitializationConfig
        (IFLogAsyncBufferingInitializationConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

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

    public int DefaultBufferBatchConsumeSize
    {
        get => int.TryParse(this[nameof(DefaultBufferBatchConsumeSize)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultBufferBatchConsumeSize)] = value.ToString();
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

    public int DefaultMaxBufferQueueSize
    {
        get => int.TryParse(this[nameof(DefaultMaxBufferQueueSize)], out var timePart) ? timePart : 0;
        set => this[nameof(DefaultMaxBufferQueueSize)] = value.ToString();
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

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IFLogAsyncBufferingInitializationConfig ICloneable<IFLogAsyncBufferingInitializationConfig>.Clone() => Clone();

    public FLogAsyncBufferingInitializationConfig Clone() => new(this);

    IFLogAsyncBufferingInitializationConfig IConfigCloneTo<IFLogAsyncBufferingInitializationConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    public FLogAsyncBufferingInitializationConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new (this, configRoot, path);

    public bool AreEquivalent(IFLogAsyncBufferingInitializationConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var asyncTypeSame         = AsyncProcessingType == other.AsyncProcessingType;
        var queueSizeSame         = DefaultBufferQueueSize == other.DefaultBufferQueueSize;
        var maxQueueSizeSame      = DefaultMaxBufferQueueSize == other.DefaultMaxBufferQueueSize;
        var fullQueueHandlingSame = DefaultFullQueueHandling == other.DefaultFullQueueHandling;
        var dropIntervalSame      = DefaultDropInterval == other.DefaultDropInterval;
        var batchConsumeSizeSame  = DefaultBufferBatchConsumeSize == other.DefaultBufferBatchConsumeSize;
        var initAsyncNumSame      = InitialAsyncProcessing == other.InitialAsyncProcessing;
        var maxAsyncNumSame       = MaxAsyncProcessing == other.MaxAsyncProcessing;
        var appenderQueueNumSame  = DefaultAppenderAsyncQueueNumber == other.DefaultAppenderAsyncQueueNumber;

        var allAreSame = asyncTypeSame && queueSizeSame && maxQueueSizeSame && fullQueueHandlingSame && dropIntervalSame 
                      && batchConsumeSizeSame && initAsyncNumSame && maxAsyncNumSame && appenderQueueNumSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogAsyncBufferingInitializationConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AsyncProcessingType.GetHashCode();
            hashCode = (hashCode * 397) ^ DefaultBufferQueueSize;
            hashCode = (hashCode * 397) ^ DefaultMaxBufferQueueSize;
            hashCode = (hashCode * 397) ^ (int)DefaultFullQueueHandling;
            hashCode = (hashCode * 397) ^ DefaultDropInterval;
            hashCode = (hashCode * 397) ^ DefaultBufferBatchConsumeSize;
            hashCode = (hashCode * 397) ^ InitialAsyncProcessing;
            hashCode = (hashCode * 397) ^ MaxAsyncProcessing;
            hashCode = (hashCode * 397) ^ DefaultAppenderAsyncQueueNumber;
            return hashCode;
        }
    }

    public IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(FLogAsyncBufferingInitializationConfig))
               .AddTypeStart()
               .AddField(nameof(AsyncProcessingType), AsyncProcessingType, AsyncProcessingTypesExtensions.AsyncProcessingTypeFormatter)
               .AddField(nameof(DefaultBufferQueueSize), DefaultBufferQueueSize)
               .AddField(nameof(DefaultMaxBufferQueueSize), DefaultMaxBufferQueueSize)
               .AddField(nameof(DefaultFullQueueHandling), DefaultFullQueueHandling, FullQueueHandlingExtensions.FullQueueHandlingFormatter)
               .AddField(nameof(DefaultDropInterval), DefaultDropInterval)
               .AddField(nameof(DefaultBufferBatchConsumeSize), DefaultBufferBatchConsumeSize)
               .AddField(nameof(InitialAsyncProcessing), InitialAsyncProcessing)
               .AddField(nameof(MaxAsyncProcessing), MaxAsyncProcessing)
               .AddField(nameof(DefaultAppenderAsyncQueueNumber), DefaultAppenderAsyncQueueNumber)
               .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
