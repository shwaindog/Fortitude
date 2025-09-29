// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.AsyncForwarding;

public interface IFLogEntryAsyncReceiveConfig : IFLogEntryQueueConfig
{
    const int DefaultConfirmSequenceNumberInterval = 64;

    int ConfirmSequenceNumberInterval { get; }
}

public interface IMutableFLogEntryAsyncReceiveConfig : IFLogEntryAsyncReceiveConfig, IMutableFLogEntryQueueConfig
{
    new int ConfirmSequenceNumberInterval { get; set; }
}

public class FLogEntryAsyncReceiveConfig : FLogEntryQueueConfig, IMutableFLogEntryAsyncReceiveConfig
{
    public FLogEntryAsyncReceiveConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogEntryAsyncReceiveConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogEntryAsyncReceiveConfig
    (int queueSize = IFLogEntryQueueConfig.DefaultQueueSize
      , FullQueueHandling inboundQueueFullHandling = IFLogEntryQueueConfig.DefaultQueueFullHandling
      , int queueReadBatchSize = IFLogEntryQueueConfig.DefaultQueueReadBatchSize
      , int confirmSequenceNumberInterval = IFLogEntryAsyncReceiveConfig.DefaultConfirmSequenceNumberInterval
      , int queueDropInterval = IFLogEntryQueueConfig.DefaultQueueDropInterval
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : this(InMemoryConfigRoot, InMemoryPath, queueSize, inboundQueueFullHandling, queueReadBatchSize
             , confirmSequenceNumberInterval, queueDropInterval, logEntryPool) { }

    public FLogEntryAsyncReceiveConfig
    (IConfigurationRoot root, string path
      , int queueSize = IFLogEntryQueueConfig.DefaultQueueSize
      , FullQueueHandling inboundQueueFullHandling = IFLogEntryQueueConfig.DefaultQueueFullHandling
      , int confirmSequenceNumberInterval = IFLogEntryAsyncReceiveConfig.DefaultConfirmSequenceNumberInterval
      , int queueReadBatchSize = IFLogEntryQueueConfig.DefaultQueueReadBatchSize
      , int queueDropInterval = IFLogEntryQueueConfig.DefaultQueueDropInterval
      , IMutableFLogEntryPoolConfig? logEntryPool = null)
        : base(root, path, queueSize, inboundQueueFullHandling, queueReadBatchSize, queueDropInterval, logEntryPool) =>
        ConfirmSequenceNumberInterval = confirmSequenceNumberInterval;

    public FLogEntryAsyncReceiveConfig(IFLogEntryAsyncReceiveConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) =>
        ConfirmSequenceNumberInterval = toClone.ConfirmSequenceNumberInterval;

    public FLogEntryAsyncReceiveConfig(IFLogEntryAsyncReceiveConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public int ConfirmSequenceNumberInterval
    {
        get =>
            int.TryParse(this[nameof(ConfirmSequenceNumberInterval)], out var timePart)
                ? timePart
                : IFLogEntryAsyncReceiveConfig.DefaultConfirmSequenceNumberInterval;
        set => this[nameof(ConfirmSequenceNumberInterval)] = value.ToString();
    }

    public override FLogEntryAsyncReceiveConfig Clone() => new(this);

    public override bool AreEquivalent(IFLogEntryQueueConfig? other, bool exactTypes = false)
    {
        if (other is not IFLogEntryAsyncReceiveConfig asyncReceiveConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var confirmSeqNumIntervalSame = ConfirmSequenceNumberInterval == asyncReceiveConfig.ConfirmSequenceNumberInterval;

        var allAreSame = baseSame && confirmSeqNumIntervalSame;

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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(QueueSize), QueueSize)
           .Field.AlwaysAdd(nameof(QueueFullHandling), QueueFullHandling)
           .Field.AlwaysAdd(nameof(ConfirmSequenceNumberInterval), ConfirmSequenceNumberInterval)
           .Field.AlwaysAdd(nameof(QueueReadBatchSize), QueueReadBatchSize)
           .Field.WhenNonNullReveal(nameof(LogEntryPool), LogEntryPool)
           .Complete();
}
