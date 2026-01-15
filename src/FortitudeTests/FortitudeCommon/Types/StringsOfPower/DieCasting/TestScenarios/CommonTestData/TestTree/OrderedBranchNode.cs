// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public class OrderedBranchNode<TChild> : OrderedParentNode<TChild>, IChildNode
    where TChild : class?, IChildNode?
{
    private static int branchNodeInstanceId;

    public OrderedBranchNode()
    {
        NodeType             = NodeType.BranchNode;
        BranchNodeInstanceId = Interlocked.Increment(ref branchNodeInstanceId);
    }

    public OrderedBranchNode(List<TChild> childNodes, IReadOnlyParentNode? parent = null) : base(childNodes)
    {
        NodeType             = NodeType.BranchNode;
        BranchNodeInstanceId = Interlocked.Increment(ref branchNodeInstanceId);
    }

    public OrderedBranchNode(List<TChild> childNodes, string name, IReadOnlyParentNode? parent, int? instId = null) : base(childNodes, name, instId)
    {
        NodeType = NodeType.BranchNode;
        Parent   = parent;

        BranchNodeInstanceId = Interlocked.Increment(ref branchNodeInstanceId);
    }

    public IReadOnlyParentNode? Parent { get; set; }

    public int BranchNodeInstanceId { get; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(BranchNodeInstanceId), BranchNodeInstanceId)
           .AddBaseRevealStateFields(this)
           .Field.WhenNonNullReveal(nameof(Parent), Parent)
           .Complete();
}
