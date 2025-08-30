// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.Visitors;

public interface IFLogEntryPublishChainVisitor<out T> where T : IFLogEntryPublishChainVisitor<T>
{
    T Accept(IFLogEntryRootPublisher flogEntryPublisherRoot);
    T Accept(IFLogEntryPipelineInterceptor flogEntryInterceptorNode);
    T Accept(IFLogEntrySink flogEntryPublishSink);
}

public class FLogEntryPublishChainVisitor<T> : RecyclableObject, IFLogEntryPublishChainVisitor<T> where T : IFLogEntryPublishChainVisitor<T>
{
    protected T Me => (T)(IFLogEntryPublishChainVisitor<T>)this;

    public virtual T Accept(IFLogEntryRootPublisher node) => Me;

    public virtual T Accept(IFLogEntryPipelineInterceptor flogEntryInterceptorNode) => Me;

    public virtual T Accept(IFLogEntrySink flogEntryPublishSink) => Me;
}
