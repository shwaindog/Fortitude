// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.Visitors;

public interface IFLogEntryPublishChainVisitor<out T> where T : IFLogEntryPublishChainVisitor<T>
{
    T Visit(IFLogEntryRootPublisher flogEntryPublisherRoot);
    T Visit(IFLogEntryPipelineInterceptor flogEntryInterceptorNode);
    T Visit(IFLogEntrySink flogEntryPublishSink);
}

public class FLogEntryPublishChainVisitor<T> : RecyclableObject, IFLogEntryPublishChainVisitor<T> where T : IFLogEntryPublishChainVisitor<T>
{
    protected T Me => (T)(IFLogEntryPublishChainVisitor<T>)this;

    public virtual T Visit(IFLogEntryRootPublisher node) => Me;

    public virtual T Visit(IFLogEntryPipelineInterceptor flogEntryInterceptorNode) => Me;

    public virtual T Visit(IFLogEntrySink flogEntryPublishSink) => Me;
}
