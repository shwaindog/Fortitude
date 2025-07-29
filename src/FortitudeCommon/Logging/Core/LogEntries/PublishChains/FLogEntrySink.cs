// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntrySink : IFLogEntryEventReceiver, IDisposable
{
    IFLogEntryRootPublisher? RootInBoundEndpoint { get; }

    IFLogEntrySource? InBound { get; set; }
}

public abstract class FLogEntrySinkBase : FLogEntryEventReceiverBase, IFLogEntrySink
{
    private IFLogEntrySource? inBound;

    public FLogEntrySinkBase Initialize()
    {
        return this;
    }

    public IFLogEntrySource? InBound
    {
        get => inBound;
        set
        {
            if (inBound != null)
            {
                inBound.RemoveOptionalChild(this);
            }
            inBound = value;
        }
    }

    public IFLogEntryRootPublisher? RootInBoundEndpoint =>
        InBound as IFLogEntryRootPublisher ?? (InBound is IFLogEntryForkingInterceptor branch
            ? branch.RootInBoundEndpoint
            : null);

    public override T LogEntryChainVisit<T>(T visitor) => visitor.Accept(this);


    public virtual void Dispose()
    {
        inBound = null;
        DecrementRefCount();
    }

    public override void StateReset()
    {
        inBound = null;
        base.StateReset();
    }
}

public class FLogEntrySinkContainer : FLogEntrySinkBase, IFLogEntrySink
{
    private IFLogEntryEventReceiver processingReceiver = null!;

    public FLogEntrySinkContainer Initialize(IFLogEntryEventReceiver logEntryReceiver)
    {
        processingReceiver = logEntryReceiver;

        return this;
    }

    public override FLogEntrySourceSinkType    LogEntryLinkType     => ProcessingReceiver.LogEntryLinkType;

    public override FLogEntryProcessChainState LogEntryProcessState
    {
        get => ProcessingReceiver.LogEntryProcessState;
        protected set => _ = value;
    }

    public override string Name
    {
        get => ProcessingReceiver.Name;
        protected set => _ = value;
    }

    public override T LogEntryChainVisit<T>(T visitor) => visitor.Accept(this);

    public IFLogEntryEventReceiver ProcessingReceiver
    {
        get => processingReceiver;
        protected set => processingReceiver = value;
    }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        ProcessingReceiver.OnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public override void Dispose()
    {
        processingReceiver = null!;
        DecrementRefCount();
    }

    public override void StateReset()
    {
        processingReceiver = null!;
        base.StateReset();
    }
}
