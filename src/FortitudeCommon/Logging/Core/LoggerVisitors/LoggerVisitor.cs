// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public interface IFLoggerVisitor<out T> where T : IFLoggerVisitor<T>
{
    T Accept(IMutableFLoggerCommon logger);
    T Accept(IMutableFLoggerRoot logger);
    T Accept(IMutableFLoggerDescendant logger);
}

public class LoggerVisitor<T> : RecyclableObject, IFLoggerVisitor<T> where T : IFLoggerVisitor<T>
{
    protected T Me => (T)(IFLoggerVisitor<T>)this;

    public virtual T Accept(IMutableFLoggerCommon node)
    {
        switch (node)
        {
            case IMutableFLoggerDescendant descendantConfig: Accept(descendantConfig); break;
            case IMutableFLoggerRoot rootConfig:             Accept(rootConfig); break;
        }
        return Me;
    }

    public virtual T Accept(IMutableFLoggerRoot node) => VisitAllExisting(node);

    public virtual T Accept(IMutableFLoggerDescendant node) => VisitAllExisting(node);

    protected T VisitAllExisting(IMutableFLoggerCommon node)
    {
        foreach (var descendantConfigTreeNode in node.ImmediateEmbodiedChildren) descendantConfigTreeNode.Visit((T)(object)this);
        return Me;
    }
}
