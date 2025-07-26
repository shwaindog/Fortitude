// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class LeafToRootCollectVisitor : LoggerVisitor<LeafToRootCollectVisitor>
{
    private readonly List<IFLoggerCommon> nodes = new ();

    public override LeafToRootCollectVisitor Accept(IMutableFLoggerRoot node)
    {
        nodes.Add(node);
        return this;
    }

    public override LeafToRootCollectVisitor Accept(IMutableFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Visit(this);
        return this;
    }

    public IReadOnlyList<IFLoggerCommon> NodeSequence => nodes.AsReadOnly();
}
