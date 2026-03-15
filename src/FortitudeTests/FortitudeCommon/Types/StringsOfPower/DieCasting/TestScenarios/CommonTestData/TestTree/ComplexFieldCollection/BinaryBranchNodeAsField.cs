// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

public class BinaryBranchNodeAsField<TChild> : OrderedParentNodeAsField<TChild>, IChildNode
    where TChild : class?, IChildNode?
{
    private readonly bool showParent;

    public BinaryBranchNodeAsField()
    {
        NodeType = NodeType.BranchNode;
    }

    public BinaryBranchNodeAsField(string name, int? instId = null, bool showParent = true)
        : base([], name, instId)
    {
        this.showParent = showParent;
        NodeType        = NodeType.BranchNode;
    }

    public BinaryBranchNodeAsField(string name, TChild left, TChild right, int? instId = null, bool showParent = true)
        : base(name, instId, left, right)
    {
        this.showParent = showParent;
        NodeType        = NodeType.BranchNode;

        Left  = left;
        Right = right;
    }

    public BinaryBranchNodeAsField(TChild left, TChild right, string name, int? instId = null, bool showParent = true)
        : base(name, instId, left, right)
    {
        this.showParent = showParent;
        NodeType        = NodeType.BranchNode;

        Left  = left;
        Right = right;
    }

    public TChild? Left { get; set; }

    public TChild? Right { get; set; }

    public INode? Parent { get; set; }

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var cm = tos.StartComplexType(this);
               // to skip over a base RevealState cast derived type to that as AddBaseRevealStateFields will then go to it's base type
        cm.AddBaseRevealStateFields((OrderedParentNodeAsField<TChild>)this);
        cm.Field.WhenNonNullReveal(nameof(Left), Left);
        cm.Field.WhenNonNullReveal(nameof(Right), Right);

        if (showParent) cm.Field.WhenNonNullReveal(nameof(Parent), Parent);
        return cm.Complete();
    }
}
