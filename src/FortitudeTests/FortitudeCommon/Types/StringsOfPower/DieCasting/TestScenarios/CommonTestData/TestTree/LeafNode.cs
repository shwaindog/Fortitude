// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public class LeafNode : Node, IChildNode
{

    public static void ResetRootInstanceIds()
    {
        LeafNodeInstanceId = 0;
    }

    public LeafNode()
    {
        NodeType = NodeType.RootNode;

        LeafInstanceId = Interlocked.Increment(ref LeafNodeInstanceId);
    }

    public LeafNode(IReadOnlyParentNode? parent = null)
    {
        NodeType = NodeType.LeafNode;
        Parent   = parent;

        LeafInstanceId = Interlocked.Increment(ref LeafNodeInstanceId);
    }

    public LeafNode(string name, int? instId = null, IReadOnlyParentNode? parent = null) : base(name, instId)
    {
        NodeType = NodeType.LeafNode;
        Parent   = parent;

        LeafInstanceId = Interlocked.Increment(ref LeafNodeInstanceId);
    }

    public INode? Parent { get; set; }

    public int LeafInstanceId { get; }

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var ctm = tos.StartComplexType(this);
        ctm.Field.AlwaysAdd(nameof(LeafInstanceId), LeafInstanceId);
        ctm.AddBaseRevealStateFields(this);
        return ctm.Complete();
    }
}
