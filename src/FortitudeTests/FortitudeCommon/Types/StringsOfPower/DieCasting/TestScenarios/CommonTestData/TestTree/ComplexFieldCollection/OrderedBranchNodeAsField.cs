// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

public class OrderedBranchNodeAsField<TChild> : OrderedParentNodeAsField<TChild>, IChildNode
    where TChild : class?, IChildNode?
{
    private readonly bool showParent;

    public static void ResetBranchInstanceIds()
    {
        BranchNodeInstanceId = 0;
    }

    public OrderedBranchNodeAsField()
    {
        showParent = true;

        NodeType         = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNodeAsField(List<TChild> childNodes, IReadOnlyParentNode? parent = null, bool showParent = true) : base(childNodes)
    {
        this.showParent = showParent;

        NodeType         = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNodeAsField(List<TChild> childNodes, string name, IReadOnlyParentNode? parent = null, int? instId = null
      , bool showParent = true)
        : base(childNodes, name, instId)
    {
        this.showParent = showParent;

        NodeType = NodeType.BranchNode;
        Parent   = parent;

        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public INode? Parent { get; set; }

    public int BranchInstanceId { get; }

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var cm =
            tos.StartComplexType(this)
               .Field.AlwaysAdd(nameof(BranchInstanceId), BranchInstanceId)
               .AddBaseRevealStateFields(this);

        if(showParent) cm.Field.WhenNonNullReveal(nameof(Parent), Parent);
        return cm.Complete();
    }
}
