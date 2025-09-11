// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class LeafToBaseCollectVisitor : LoggerVisitor<LeafToBaseCollectVisitor>
{
    private readonly List<IFLoggerDescendant> nodes = new();

    public IReadOnlyList<IFLoggerDescendant> NodeSequence => nodes.AsReadOnly();

    public override LeafToBaseCollectVisitor Visit(IMutableFLoggerRoot node) => this;

    public override LeafToBaseCollectVisitor Visit(IMutableFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Accept(this);
        return this;
    }
}
