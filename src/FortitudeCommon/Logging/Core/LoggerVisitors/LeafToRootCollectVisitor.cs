// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class LeafToRootCollectVisitor : LoggerVisitor<LeafToRootCollectVisitor>
{
    private readonly List<IFLoggerCommon> nodes = new();

    public IReadOnlyList<IFLoggerCommon> NodeSequence => nodes.AsReadOnly();

    public override LeafToRootCollectVisitor Visit(IMutableFLoggerRoot node)
    {
        nodes.Add(node);
        return this;
    }

    public override LeafToRootCollectVisitor Visit(IMutableFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Accept(this);
        return this;
    }
}
