// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IFLogEntryQueueConfig : IInterfacesComparable<IFLogEntryQueueConfig>, IConfigCloneTo<IFLogEntryQueueConfig>
  , IStyledToStringObject, IFLogConfig
{
    const int DefaultQueueSize          = 1024;
    const int DefaultQueueReadBatchSize = 32;
    const int DefaultQueueDropInterval  = 2;

    const FullQueueHandling DefaultQueueFullHandling = FullQueueHandling.Block;

    int QueueSize { get; }

    FullQueueHandling QueueFullHandling { get; }

    int QueueDropInterval { get; }

    int QueueReadBatchSize { get; }

    IFLogEntryPoolConfig? LogEntryPool { get; }
}

public interface IMutableFLogEntryQueueConfig : IFLogEntryQueueConfig, IMutableFLogConfig
{
    new int QueueSize { get; set; }

    new FullQueueHandling QueueFullHandling { get; set; }

    new int QueueReadBatchSize { get; set; }

    new IMutableFLogEntryPoolConfig? LogEntryPool { get; set; }
}

public class FLogEntryQueueConfig : FLogConfig, IMutableFLogEntryQueueConfig
{
    public FLogEntryQueueConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogEntryQueueConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogEntryQueueConfig
    (int queueSize = IFLogEntryQueueConfig.DefaultQueueSize
      , FullQueueHandling inboundQueueFullHandling = IFLogEntryQueueConfig.DefaultQueueFullHandling
      , int queueReadBatchSize = IFLogEntryQueueConfig.DefaultQueueReadBatchSize
      , int queueDropInterval = IFLogEntryQueueConfig.DefaultQueueDropInterval
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, queueSize, inboundQueueFullHandling, queueReadBatchSize, queueDropInterval, logEntryPool) { }

    public FLogEntryQueueConfig
    (IConfigurationRoot root, string path, int queueSize = IFLogEntryQueueConfig.DefaultQueueSize
      , FullQueueHandling inboundQueueFullHandling = IFLogEntryQueueConfig.DefaultQueueFullHandling
      , int queueReadBatchSize = IFLogEntryQueueConfig.DefaultQueueReadBatchSize
      , int queueDropInterval = IFLogEntryQueueConfig.DefaultQueueDropInterval
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : base(root, path)
    {
        QueueSize    = queueSize;
        LogEntryPool = logEntryPool;

        QueueDropInterval  = queueDropInterval;
        QueueFullHandling  = inboundQueueFullHandling;
        QueueReadBatchSize = queueReadBatchSize;
    }

    public FLogEntryQueueConfig(IFLogEntryQueueConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        QueueSize    = toClone.QueueSize;
        LogEntryPool = toClone.LogEntryPool as IMutableFLogEntryPoolConfig;
        
        QueueDropInterval  = toClone.QueueDropInterval;
        QueueFullHandling  = toClone.QueueFullHandling;
        QueueReadBatchSize = toClone.QueueReadBatchSize;
    }

    public FLogEntryQueueConfig(IFLogEntryQueueConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IFLogEntryQueueConfig ICloneable<IFLogEntryQueueConfig>.Clone() => Clone();

    public virtual FLogEntryQueueConfig Clone() => new(this);

    IFLogEntryQueueConfig IConfigCloneTo<IFLogEntryQueueConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    public virtual FLogEntryQueueConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new(this, configRoot, path);

    IFLogEntryPoolConfig? IFLogEntryQueueConfig.LogEntryPool => LogEntryPool;

    public IMutableFLogEntryPoolConfig? LogEntryPool
    {
        get
        {
            if (GetSection(nameof(LogEntryPool)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FLogEntryPoolConfig(ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}")
                {
                    ParentConfig = this
                };
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new FLogEntryPoolConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LogEntryPool)}");

                value.ParentConfig = this;
            }
        }
    }

    public FullQueueHandling QueueFullHandling
    {
        get =>
            Enum.TryParse<FullQueueHandling>(this[nameof(QueueFullHandling)], out var fullQueueHandling)
                ? fullQueueHandling
                : FullQueueHandling.Block;
        set => this[nameof(QueueFullHandling)] = value.ToString();
    }

    public int QueueDropInterval
    {
        get => int.TryParse(this[nameof(QueueDropInterval)], out var dropInterval) 
            ? dropInterval 
            : IFLogEntryQueueConfig.DefaultQueueDropInterval;
        set => this[nameof(QueueDropInterval)] = value.ToString();
    }

    public int QueueReadBatchSize
    {
        get => int.TryParse(this[nameof(QueueReadBatchSize)], out var readBatchSize) 
            ? readBatchSize 
            : IFLogEntryQueueConfig.DefaultQueueReadBatchSize;
        set => this[nameof(QueueReadBatchSize)] = value.ToString();
    }

    public int QueueSize
    {
        get => int.TryParse(this[nameof(QueueSize)], out var queueSize) 
            ? queueSize 
            : IFLogEntryQueueConfig.DefaultQueueSize;
        set => this[nameof(QueueSize)] = value.ToString();
    }

    public virtual bool AreEquivalent(IFLogEntryQueueConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var queueSizeSame         = QueueSize == other.QueueSize;
        var queueFullHandlingSame = QueueFullHandling == other.QueueFullHandling;
        var readBatchSizeSame     = QueueReadBatchSize == other.QueueReadBatchSize;
        var dropIntervalSame     = QueueDropInterval == other.QueueDropInterval;
        var logEntryPoolSame = LogEntryPool?.AreEquivalent(other.LogEntryPool, exactTypes)
                            ?? other.LogEntryPool == null;

        var allAreSame = queueSizeSame && queueFullHandlingSame && readBatchSizeSame && dropIntervalSame && logEntryPoolSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogEntryQueueConfig, true);

    public override int GetHashCode()
    {
        var hashCode = QueueSize;
        hashCode = (hashCode * 397) ^ QueueFullHandling.GetHashCode();
        hashCode = (hashCode * 397) ^ QueueReadBatchSize;
        hashCode = (hashCode * 397) ^ QueueDropInterval;
        hashCode = (hashCode * 397) ^ (LogEntryPool?.GetHashCode() ?? 0);
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(FLogEntryQueueConfig))
               .Field.AddAlways(nameof(QueueSize), QueueSize)
               .Field.AddAlways(nameof(QueueFullHandling), QueueFullHandling, FullQueueHandlingExtensions.FullQueueHandlingFormatter)
               .Field.AddAlways(nameof(QueueReadBatchSize), QueueReadBatchSize)
               .Field.AddAlways(nameof(QueueDropInterval), QueueDropInterval)
               .Field.AddWhenNonNull(nameof(LogEntryPool), LogEntryPool)
               .Complete();
    }
}
