// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

public interface IFLogEntryRootPublisher : IFLogEntrySource
{
}

public abstract class FLogEntryRootPublisherBase : FLogEntrySource, IFLogEntryRootPublisher
{
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Source;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }

    public override T LogEntryChainVisit<T>(T visitor) => visitor.Accept(this);
}

