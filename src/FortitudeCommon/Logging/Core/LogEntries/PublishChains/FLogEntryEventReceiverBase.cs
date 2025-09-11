// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryEventReceiver : IFLogEntryPublishChainTreeNode
{
    string Name { get; }

    FLogEntryPublishHandler InBoundListener { get; }

    void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher);
}

public abstract class FLogEntryEventReceiverBase : FLogEntryPublishChainTreeNode, IFLogEntryEventReceiver
{
    protected FLogEntryEventReceiverBase() => InBoundListener = OnReceiveLogEntry;

    public virtual FLogEntryPublishHandler InBoundListener { get; }

    public abstract string Name { get; protected set; }

    public abstract void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher);
}

public abstract class FLogEntryEventSafeReceiverBase : FLogEntryEventReceiverBase, IFLogEntryEventReceiver
{
    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        if (ShouldCheckLock)
            while (true)
            {
                using var readLock = AcquireReadTreeLock(50);
                if (readLock != null)
                {
                    SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
                    break;
                }
            }
        else
            SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    protected abstract void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher);
}

public class FLogEntryEventReceiverContainer : FLogEntryPublishChainTreeNode, IFLogEntryEventReceiver
{
    private readonly Action<LogEntryPublishEvent, ITargetingFLogEntrySource> onReceiveLogEntryAction;

    public FLogEntryEventReceiverContainer(string name
      , Action<LogEntryPublishEvent, ITargetingFLogEntrySource> onReceiveLogEntryAction
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.Sink
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating)
    {
        Name = name;

        LogEntryLinkType     = logEntryLinkType;
        LogEntryProcessState = logEntryProcessState;

        this.onReceiveLogEntryAction = onReceiveLogEntryAction;

        InBoundListener = OnReceiveLogEntry;
    }

    public FLogEntryPublishHandler InBoundListener { get; }

    public override FLogEntrySourceSinkType LogEntryLinkType { get; }

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public string Name { get; protected set; }

    public override T Accept<T>(T visitor) => throw new ApplicationException("This should never be called by containing now");

    public void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        if (ShouldCheckLock)
            while (true)
            {
                using var readLock = AcquireReadTreeLock(50);
                if (readLock != null)
                {
                    SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
                    break;
                }
            }
        else
            SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
    }

    protected void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        onReceiveLogEntryAction(logEntryEvent, fromPublisher);
    }
}
