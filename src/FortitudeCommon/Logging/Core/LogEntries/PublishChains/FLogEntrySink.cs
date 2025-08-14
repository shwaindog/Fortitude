// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntrySink : IFLogEntryEventReceiver, IDisposable
{
    IFLogEntryRootPublisher? RootInBoundEndpoint { get; }

    IFLogEntrySource? InBound { get; set; }
}

public interface IForkingFLogEntrySink : IFLogEntrySink
{
    FLogEntryPublishHandler ForkingInBoundListener { get; }
}

public abstract class FLogEntrySinkBase : FLogEntryEventReceiverBase, IFLogEntrySink
{
    public virtual FLogEntrySinkBase Initialize()
    {
        return this;
    }

    public virtual IFLogEntrySource? InBound { get; set; }

    public IFLogEntryRootPublisher? RootInBoundEndpoint =>
        InBound as IFLogEntryRootPublisher ?? (InBound is IFLogEntryForkingInterceptor branch
            ? branch.RootInBoundEndpoint
            : null);

    public override T LogEntryChainVisit<T>(T visitor) => visitor.Accept(this);

    public virtual void Dispose()
    {
        InBound = null;
        DecrementRefCount();
    }

    public override void StateReset()
    {
        InBound = null;
        base.StateReset();
    }
}

public abstract class ForkingFLogEntrySinkBase : FLogEntrySinkBase, IForkingFLogEntrySink
{
    protected ForkingFLogEntrySinkBase()
    {
        ForkingInBoundListener = IncrementRefsOnReceiveLogEntry;
    }

    public override ForkingFLogEntrySinkBase Initialize()
    {
        base.Initialize();

        return this;
    }

    public override IFLogEntrySource? InBound
    {
        get => base.InBound;
        set
        {
            if (base.InBound != null)
            {
                base.InBound.RemoveOptionalChild(this);
            }
            base.InBound = value;
        }
    }

    public void IncrementRefsOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        logEntryEvent.LogEntry?.IncrementRefCount();
        logEntryEvent.LogEntriesBatch?.IncrementRefCount();
        OnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public FLogEntryPublishHandler ForkingInBoundListener { get; set; }
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
