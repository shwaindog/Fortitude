// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryRootPublisher : IFLogEntrySource { }

public abstract class FLogEntryRootPublisherBase : FLogEntrySource, IFLogEntryRootPublisher
{
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Source;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override T Accept<T>(T visitor) => visitor.Visit(this);
}

public class FLogEntryPublishSource : FLogEntryRootPublisherBase, IFLogEntryRootPublisher
{
    public FLogEntryPublishSource
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

    public FLogEntryPublishSource Initialize
    (
        string name
      , FLogEntrySourceSinkType logEntryLinkType = FLogEntrySourceSinkType.InterceptionPoint
      , FLogEntryProcessChainState logEntryProcessState = FLogEntryProcessChainState.Terminating
    ) => this;

    public override string Name { get; }

    public override FLogEntrySourceSinkType LogEntryLinkType { get; }

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }
}
