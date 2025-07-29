// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains.Visitors;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryPublishChainTreeNode : IRecyclableObject
{
    IReadWriterSyncLock? AcquireUpdateTreeLock(int timeoutMs);

    IReadWriterSyncLock? AcquireReadTreeLock(int timeoutMs);

    FLogEntrySourceSinkType LogEntryLinkType { get; }

    FLogEntryProcessChainState LogEntryProcessState { get; }

    T LogEntryChainVisit<T>(T visitor) where T : IFLogEntryPublishChainVisitor<T>;

    new IRecycler Recycler { get; set; }
}

public abstract class FLogEntryPublishChainTreeNode : RecyclableObject, IFLogEntryPublishChainTreeNode
{
    private static readonly Recycler LogEntryPublishRecycler = new();

    private PaddedLong writeToken = new(0);

    private readonly Action releaseWriteToken;

    protected FLogEntryPublishChainTreeNode()
    {
        releaseWriteToken = () =>
        {
            Thread.VolatileWrite(ref writeToken.Value, 0);
        };
    }

    protected bool StopProcessing => Thread.VolatileRead(ref writeToken.Value) == 0;

    private readonly IReadWriterSyncLock        updateTreeLock = new ReaderWriterSyncLock();
    public abstract  FLogEntrySourceSinkType    LogEntryLinkType     { get; }
    public abstract  FLogEntryProcessChainState LogEntryProcessState { get; protected set; }
    public override IRecycler Recycler
    {
        get => base.Recycler ?? LogEntryPublishRecycler;
        #pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        #pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        set => base.Recycler = value;
        #pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        #pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }

    public abstract T LogEntryChainVisit<T>(T visitor) where T : IFLogEntryPublishChainVisitor<T>;

    public virtual IReadWriterSyncLock? AcquireUpdateTreeLock(int timeoutMs)
    {
        if (updateTreeLock.TryAcquireWriterLock(timeoutMs))
        {
            Thread.VolatileWrite(ref writeToken.Value, 1);
            return updateTreeLock;
        }
        return null;
    }

    public virtual IReadWriterSyncLock? AcquireReadTreeLock(int timeoutMs)
    {
        if (updateTreeLock.TryAcquireUpgradeableReaderLock(timeoutMs))
        {
            return updateTreeLock;
        }
        return null;
    }
}
