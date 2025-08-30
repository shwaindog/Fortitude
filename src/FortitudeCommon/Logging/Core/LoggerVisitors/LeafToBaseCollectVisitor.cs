// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class LeafToBaseCollectVisitor : LoggerVisitor<LeafToBaseCollectVisitor>
{
    private readonly List<IFLoggerDescendant> nodes = new();

    public IReadOnlyList<IFLoggerDescendant> NodeSequence => nodes.AsReadOnly();

    public override LeafToBaseCollectVisitor Accept(IMutableFLoggerRoot node) => this;

    public override LeafToBaseCollectVisitor Accept(IMutableFLoggerDescendant node)
    {
        nodes.Add(node);
        node.Parent.Visit(this);
        return this;
    }
}
