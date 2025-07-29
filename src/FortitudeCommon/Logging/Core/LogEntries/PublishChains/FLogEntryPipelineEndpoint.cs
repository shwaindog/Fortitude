// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryPipelineEndpoint : IFLogEntryRootPublisher, IFLogEntryForkingInterceptor, IFLogEntryReceiverEndpoint
{
}

public class FLogEntryPipelineEndpoint : FLogEntryForkingInterceptor, IFLogEntryPipelineEndpoint
{
    protected readonly List<ITargetingFLogEntrySource> SubscriptionsToReceiver = new();

    public FLogEntryPipelineEndpoint
    (
        string name
      , IFLogEntrySink finalTarget
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.InterceptionPoint
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating
    )
    {
        Name = name;

        LogEntryLinkType     = logEntryLinkType;
        LogEntryProcessState = logEntryProcessState;

        FinalTarget = finalTarget;
    }

    public FLogEntryPipelineEndpoint Initialize
    (
        string name
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.InterceptionPoint
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating
    )
    {
        return this;
    }

    public override string Name { get; }

    public override FLogEntrySourceSinkType LogEntryLinkType { get; }

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }
    
    public void AddNewFLogEntrySubscriptionChain(ITargetingFLogEntrySource toAdd)
    {
        if (toAdd.FinalTarget != this)
        {
            throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        }
        if (SubscriptionsToReceiver.Contains(toAdd)) return;
        SubscriptionsToReceiver.Add(toAdd);
    }

    public void RemoveFLogEntrySubscriptionChain(ITargetingFLogEntrySource toRemove)
    {
        if (toRemove.FinalTarget != this)
        {
            throw new ArgumentException("Subscriptions to a receiver must target the correct destination");
        }
        if (!SubscriptionsToReceiver.Contains(toRemove)) return;
        SubscriptionsToReceiver.Remove(toRemove);
    }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    public IReadOnlyList<ITargetingFLogEntrySource> UpstreamSubscriptions => SubscriptionsToReceiver;
}
