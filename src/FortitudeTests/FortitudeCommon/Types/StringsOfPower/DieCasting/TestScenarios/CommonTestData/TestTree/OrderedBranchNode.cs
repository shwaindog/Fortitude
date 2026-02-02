// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public class OrderedBranchNode<TChild> : OrderedParentNode<TChild>, IChildNode
    where TChild : class?, IChildNode?
{

    public static void ResetBranchInstanceIds()
    {
        BranchNodeInstanceId = 0;
    }

    public OrderedBranchNode()
    {
        NodeType             = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNode(List<TChild> childNodes, IReadOnlyParentNode? parent = null) : base(childNodes)
    {
        NodeType             = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNode(List<TChild> childNodes, string name, IReadOnlyParentNode? parent = null, int? instId = null) : base(childNodes, name, instId)
    {
        NodeType = NodeType.BranchNode;
        Parent   = parent;

        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public IReadOnlyParentNode? Parent { get; set; }

    public int BranchInstanceId { get; }

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(BranchInstanceId), BranchInstanceId)
           .AddBaseRevealStateFields(this)
           .Field.WhenNonNullReveal(nameof(Parent), Parent)
           .Complete();
}
