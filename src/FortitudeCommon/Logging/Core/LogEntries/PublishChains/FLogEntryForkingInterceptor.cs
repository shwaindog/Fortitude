// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryForkingInterceptor : IFLogEntrySource, IFLogEntryPipelineInterceptor
{
    new string Name { get; }
}


public abstract class FLogEntryForkingInterceptor : FLogEntrySource, IFLogEntryForkingInterceptor
{
    protected FLogEntryForkingInterceptor()
    {
        InBoundListener = OnReceiveLogEntry;
    }

    public FLogEntryPublishHandler InBoundListener { get; }


    private IFLogEntrySource? inBound;

    public virtual IFLogEntrySource? InBound
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

    public bool Deactivate { get; set; }

    public bool RemoveFromPublishChain() => Remove(this);

    public override T LogEntryChainVisit<T>(T visitor) => visitor.Accept(this);

    public IFLogEntryRootPublisher? RootInBoundEndpoint =>
        InBound as IFLogEntryRootPublisher ?? (InBound is IFLogEntryForkingInterceptor branch ? branch.RootInBoundEndpoint : null);

    public virtual void Dispose()
    {
        InBound = null;
        DecrementRefCount();
    }

    public override void StateReset()
    {
        inBound = null;
        base.StateReset();
    }

    public virtual void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        if (StopProcessing)
        {
            while (true)
            {
                using (var readLock = AcquireReadTreeLock(50))
                {
                    if (readLock != null)
                    {
                        break;
                    }
                }
                SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
            }
        }
        else
        {
            SafeOnReceiveLogEntry(logEntryEvent, fromPublisher);
        }
    }

    protected void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        PublishLogEntryEvent(logEntryEvent);
    }
}
