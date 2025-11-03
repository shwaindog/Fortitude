// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public interface IFLoggerVisitor<out T> where T : IFLoggerVisitor<T>
{
    T Visit(IMutableFLoggerCommon logger);
    T Visit(IMutableFLoggerRoot logger);
    T Visit(IMutableFLoggerDescendant logger);
}

public class LoggerVisitor<T> : RecyclableObject, IFLoggerVisitor<T> where T : IFLoggerVisitor<T>
{
    protected T Me => (T)(IFLoggerVisitor<T>)this;

    public virtual T Visit(IMutableFLoggerCommon node)
    {
        switch (node)
        {
            case IMutableFLoggerDescendant descendantConfig: Visit(descendantConfig); break;
            case IMutableFLoggerRoot rootConfig:             Visit(rootConfig); break;
        }
        return Me;
    }

    public virtual T Visit(IMutableFLoggerRoot node) => VisitAllExisting(node);

    public virtual T Visit(IMutableFLoggerDescendant node) => VisitAllExisting(node);

    protected T VisitAllExisting(IMutableFLoggerCommon node)
    {
        foreach (var descendantConfigTreeNode in node.ImmediateEmbodiedChildren) descendantConfigTreeNode.Accept((T)(object)this);
        return Me;
    }
}
