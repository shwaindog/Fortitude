// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryReceiverEndpoint : IFLogEntryEventReceiver, IFLogEntrySink
{
    IReadOnlyList<ITargetingFLogEntrySource> UpstreamSubscriptions { get; }

    void AddNewFLogEntrySubscriptionChain(ITargetingFLogEntrySource toAdd);

    void RemoveFLogEntrySubscriptionChain(ITargetingFLogEntrySource toRemove);
}

public abstract class FLogEntryReceiverEndpointBase : FLogEntrySinkBase, IFLogEntryReceiverEndpoint
{
    protected readonly List<ITargetingFLogEntrySource> SubscriptionsToReceiver = new();

    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState => FLogEntryProcessChainState.Terminating;

    public void AddNewFLogEntrySubscriptionChain(ITargetingFLogEntrySource toAdd)
    {
        if (toAdd.FinalTarget != this) throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        if (SubscriptionsToReceiver.Contains(toAdd)) return;
        SubscriptionsToReceiver.Add(toAdd);
    }

    public void RemoveFLogEntrySubscriptionChain(ITargetingFLogEntrySource toRemove)
    {
        if (toRemove.FinalTarget != this) throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        if (!SubscriptionsToReceiver.Contains(toRemove)) return;
        SubscriptionsToReceiver.Remove(toRemove);
    }

    public IReadOnlyList<ITargetingFLogEntrySource> UpstreamSubscriptions => SubscriptionsToReceiver;
}

public abstract class FLogEntryReceiverEndpointContainer : FLogEntrySinkContainer, IFLogEntryReceiverEndpoint
{
    protected readonly List<ITargetingFLogEntrySource> SubscriptionsToReceiver = new();

    protected FLogEntrySourceSinkType    LinkType;
    protected FLogEntryProcessChainState ProcessState;

    public FLogEntryReceiverEndpointContainer Initialize(
        IFLogEntryEventReceiver logEntryReceiver
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.Sink
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating
    )
    {
        base.Initialize(logEntryReceiver);

        return this;
    }

    public override FLogEntrySourceSinkType LogEntryLinkType => LinkType;

    public override FLogEntryProcessChainState LogEntryProcessState
    {
        get => ProcessState;
        protected set => ProcessState = value;
    }

    public void AddNewFLogEntrySubscriptionChain(ITargetingFLogEntrySource toAdd)
    {
        if (toAdd.FinalTarget != this) throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        if (SubscriptionsToReceiver.Contains(toAdd)) return;
        SubscriptionsToReceiver.Add(toAdd);
    }

    public void RemoveFLogEntrySubscriptionChain(ITargetingFLogEntrySource toRemove)
    {
        if (toRemove.FinalTarget != this) throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        if (!SubscriptionsToReceiver.Contains(toRemove)) return;
        SubscriptionsToReceiver.Remove(toRemove);
    }

    public IReadOnlyList<ITargetingFLogEntrySource> UpstreamSubscriptions => SubscriptionsToReceiver;
}

public class FLogEntryEventDedicatePublisherReceiverEndpoint : FLogEntryReceiverEndpointBase, IFLogEntryEventReceiver
{
    private readonly Action<LogEntryPublishEvent, ITargetingFLogEntrySource> onReceiveLogEntryAction;

    public FLogEntryEventDedicatePublisherReceiverEndpoint
    (
        string name
      , Action<LogEntryPublishEvent, ITargetingFLogEntrySource> onReceiveLogEntryAction
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.Sink
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating
    )
    {
        Name = name;

        LogEntryLinkType     = logEntryLinkType;
        LogEntryProcessState = logEntryProcessState;

        this.onReceiveLogEntryAction = onReceiveLogEntryAction;

        InBoundListener = OnReceiveLogEntry;
    }

    public override FLogEntrySourceSinkType LogEntryLinkType { get; }

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override FLogEntryPublishHandler InBoundListener { get; }

    public override string Name { get; protected set; }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        onReceiveLogEntryAction(logEntryEvent, fromPublisher);
    }
}
