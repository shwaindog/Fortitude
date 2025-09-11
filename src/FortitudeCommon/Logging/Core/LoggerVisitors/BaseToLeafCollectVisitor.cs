// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Collections;

namespace FortitudeCommon.Logging.Core.LoggerVisitors;

public class BaseToLeafCollectVisitor : LoggerVisitor<BaseToLeafCollectVisitor>
{
    private List<IFLoggerDescendant> nodes = new();

    public IReadOnlyList<IFLoggerDescendant> NodeSequence => nodes.AsReadOnly();

    public string FullName => NodeSequence.Select(dctn => dctn.Name).JoinToString(".");

    public override BaseToLeafCollectVisitor Visit(IMutableFLoggerRoot node) => this;

    public override BaseToLeafCollectVisitor Visit(IMutableFLoggerDescendant node)
    {
        node.Parent.Accept(this);
        nodes.Add(node);
        return this;
    }
}
