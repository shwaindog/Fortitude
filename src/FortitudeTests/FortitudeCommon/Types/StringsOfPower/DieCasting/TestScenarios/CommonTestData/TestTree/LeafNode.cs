// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public class LeafNode : Node, IChildNode
{
    private static int leafNodeInstanceId;

    public LeafNode()
    {
        NodeType = NodeType.RootNode;

        LeafNodeInstanceId = Interlocked.Increment(ref leafNodeInstanceId);
    }

    public LeafNode(IReadOnlyParentNode? parent = null)
    {
        NodeType = NodeType.LeafNode;
        Parent   = parent;

        LeafNodeInstanceId = Interlocked.Increment(ref leafNodeInstanceId);
    }

    public LeafNode(string name, int? instId = null, IReadOnlyParentNode? parent = null) : base(name, instId)
    {
        NodeType = NodeType.LeafNode;
        Parent   = parent;

        LeafNodeInstanceId = Interlocked.Increment(ref leafNodeInstanceId);
    }

    public IReadOnlyParentNode? Parent { get; set; }

    public int LeafNodeInstanceId { get; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(LeafNodeInstanceId), LeafNodeInstanceId)
           .AddBaseRevealStateFields(this)
           .Complete();
}
