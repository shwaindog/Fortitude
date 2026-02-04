// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public interface IAsyncQueuesInitConfig : IFLogConfig, IInterfacesComparable<IAsyncQueuesInitConfig>
  , IConfigCloneTo<IAsyncQueuesInitConfig>, IStringBearer
{
    const int MinimumQueueCapacity     = 256;
    const int DefaultQueueCapacitySize = 1024;
    const int MaximumQueueCapacity     = (int)NumberExtensions.GigaByte;

    AsyncProcessingType AsyncProcessingType { get; }

    int DefaultQueueCapacity { get; }

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

    new int DefaultQueueCapacity { get; set; }

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
        DefaultQueueCapacity            = defaultBufferEntriesSize;
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
        DefaultQueueCapacity            = toClone.DefaultQueueCapacity;
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

    public int DefaultQueueCapacity
    {
        get =>
            int.TryParse(this[nameof(DefaultQueueCapacity)], out var queueCapacity)
                ? Math.Clamp(queueCapacity, IAsyncQueuesInitConfig.MinimumQueueCapacity, IAsyncQueuesInitConfig.MaximumQueueCapacity)
                : IAsyncQueuesInitConfig.DefaultQueueCapacitySize;
        set => this[nameof(DefaultQueueCapacity)] = value.ToString();
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

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IAsyncQueuesInitConfig IConfigCloneTo<IAsyncQueuesInitConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public AsyncQueuesInitConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IAsyncQueuesInitConfig ICloneable<IAsyncQueuesInitConfig>.Clone() => Clone();

    public AsyncQueuesInitConfig Clone() => new(this);

    public bool AreEquivalent(IAsyncQueuesInitConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var asyncTypeSame         = AsyncProcessingType == other.AsyncProcessingType;
        var queueSizeSame         = DefaultQueueCapacity == other.DefaultQueueCapacity;
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
            hashCode = (hashCode * 397) ^ DefaultQueueCapacity;
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

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AsyncProcessingType), AsyncProcessingType)
           .Field.AlwaysAdd(nameof(DefaultQueueCapacity), DefaultQueueCapacity)
           .Field.AlwaysAdd(nameof(DefaultFullQueueHandling), DefaultFullQueueHandling)
           .Field.AlwaysAdd(nameof(DefaultDropInterval), DefaultDropInterval)
           .Field.AlwaysAdd(nameof(InitialAsyncProcessing), InitialAsyncProcessing)
           .Field.AlwaysAdd(nameof(MaxAsyncProcessing), MaxAsyncProcessing)
           .Field.AlwaysAdd(nameof(DefaultAppenderAsyncQueueNumber), DefaultAppenderAsyncQueueNumber)
           .Field.AlwaysAdd(nameof(DefaultAppenderFlushQueueNumber), DefaultAppenderFlushQueueNumber)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(AsyncQueues), AsyncQueues.GetEnumerator())
           .Complete();

    public override string ToString() => this.DefaultToString();
}
