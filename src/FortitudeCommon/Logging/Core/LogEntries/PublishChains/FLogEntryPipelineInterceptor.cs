// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryPipelineInterceptor : ITargetingFLogEntrySource, IFLogEntrySink
{
    new string Name { get; }

    bool Deactivate { get; set; }

    bool RemoveFromPublishChain();
}

public interface IFLogEntryExaminingInterceptor : IFLogEntryForkingInterceptor
{
    Predicate<IFLogEntry> CheckCondition { get; set; }
}

public abstract class FLogEntryPipelineInterceptor : TargetingFLogEntrySource, IFLogEntryPipelineInterceptor
{
    private IFLogEntrySource? inBound;

    protected FLogEntryPipelineInterceptor() => InBoundListener = OnReceiveLogEntry;

    public FLogEntryPublishHandler InBoundListener { get; }

    public IFLogEntrySource? InBound
    {
        get => inBound;
        set
        {
            if (inBound != null) inBound.Remove(this);
            inBound = value;
        }
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public IFLogEntryRootPublisher? RootInBoundEndpoint =>
        InBound as IFLogEntryRootPublisher ?? (InBound is IFLogEntryForkingInterceptor branch ? branch.RootInBoundEndpoint : null);

    public bool Deactivate { get; set; }

    public bool RemoveFromPublishChain() => Remove(this);

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

    protected abstract void SafeOnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher);
}
