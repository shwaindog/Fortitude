// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class RootToLeafCollectVisitor : LoggerVisitor<RootToLeafCollectVisitor>
{
    private List<IFLoggerCommon> nodes = new();

    public IReadOnlyList<IFLoggerCommon> NodeSequence => nodes.AsReadOnly();

    public override RootToLeafCollectVisitor Visit(IMutableFLoggerRoot node)
    {
        nodes.Add(node);
        return this;
    }

    public override RootToLeafCollectVisitor Visit(IMutableFLoggerDescendant node)
    {
        node.Parent.Accept(this);
        nodes.Add(node);
        return this;
    }
}
