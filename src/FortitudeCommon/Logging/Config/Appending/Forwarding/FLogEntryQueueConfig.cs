// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IFLogEntryQueueConfig : IInterfacesComparable<IFLogEntryQueueConfig>, ICloneable<IFLogEntryQueueConfig>
  , IStyledToStringObject, IFLogConfig
{
    const int DefaultQueueSize          = 1024;
    const int DefaultQueueReadBatchSize = 32;

    const FullQueueHandling DefaultQueueFullHandling = FullQueueHandling.Block;

    int QueueSize { get; }

    FullQueueHandling QueueFullHandling { get; }

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
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, queueSize, inboundQueueFullHandling, queueReadBatchSize, logEntryPool) { }

    public FLogEntryQueueConfig
    (IConfigurationRoot root, string path, int queueSize = IFLogEntryQueueConfig.DefaultQueueSize
      , FullQueueHandling inboundQueueFullHandling = IFLogEntryQueueConfig.DefaultQueueFullHandling
      , int queueReadBatchSize = IFLogEntryQueueConfig.DefaultQueueReadBatchSize, IMutableFLogEntryPoolConfig? logEntryPool = null)
        : base(root, path)
    {
        QueueSize    = queueSize;
        LogEntryPool = logEntryPool;

        QueueFullHandling  = inboundQueueFullHandling;
        QueueReadBatchSize = queueReadBatchSize;
    }

    public FLogEntryQueueConfig(IFLogEntryQueueConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        QueueSize    = toClone.QueueSize;
        LogEntryPool = toClone.LogEntryPool as IMutableFLogEntryPoolConfig;

        QueueFullHandling  = toClone.QueueFullHandling;
        QueueReadBatchSize = toClone.QueueReadBatchSize;
    }

    public FLogEntryQueueConfig(IFLogEntryQueueConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IFLogEntryQueueConfig ICloneable<IFLogEntryQueueConfig>.Clone() => Clone();

    public virtual FLogEntryQueueConfig Clone() => new(this);

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

    public int QueueReadBatchSize
    {
        get => int.TryParse(this[nameof(QueueReadBatchSize)], out var timePart) ? timePart : 0;
        set => this[nameof(QueueReadBatchSize)] = value.ToString();
    }

    public int QueueSize
    {
        get => int.TryParse(this[nameof(QueueSize)], out var timePart) ? timePart : 0;
        set => this[nameof(QueueSize)] = value.ToString();
    }

    public virtual bool AreEquivalent(IFLogEntryQueueConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var queueSizeSame         = QueueSize == other.QueueSize;
        var queueFullHandlingSame = QueueFullHandling == other.QueueFullHandling;
        var readBatchSizeSame     = QueueReadBatchSize == other.QueueReadBatchSize;
        var logEntryPoolSame = LogEntryPool?.AreEquivalent(other.LogEntryPool, exactTypes)
                            ?? other.LogEntryPool == null;

        var allAreSame = queueSizeSame && queueFullHandlingSame && readBatchSizeSame && logEntryPoolSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogEntryQueueConfig, true);

    public override int GetHashCode()
    {
        var hashCode = QueueSize;
        hashCode = (hashCode * 397) ^ QueueFullHandling.GetHashCode();
        hashCode = (hashCode * 397) ^ QueueReadBatchSize;
        hashCode = (hashCode * 397) ^ (LogEntryPool?.GetHashCode() ?? 0);
        return hashCode;
    }

    public virtual IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(FLogEntryQueueConfig))
               .AddTypeStart()
               .AddField(nameof(QueueSize), QueueSize)
               .AddField(nameof(QueueFullHandling), QueueFullHandling, FullQueueHandlingExtensions.FullQueueHandlingFormatter)
               .AddField(nameof(QueueReadBatchSize), QueueReadBatchSize)
               .AddNonNullField(nameof(LogEntryPool), LogEntryPool)
               .AddTypeEnd();
    }
}
