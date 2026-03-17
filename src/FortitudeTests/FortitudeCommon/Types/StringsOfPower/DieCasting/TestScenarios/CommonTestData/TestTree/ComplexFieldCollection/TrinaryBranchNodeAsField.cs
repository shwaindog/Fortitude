// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

public class TrinaryBranchNodeAsField : OrderedParentNodeAsField<IChildNode>, IChildNode
{

    public TrinaryBranchNodeAsField()
    {
        NodeType = NodeType.BranchNode;
    }

    public TrinaryBranchNodeAsField(string name, int? instId = null) : base([], name, instId)
    {
        NodeType = NodeType.BranchNode;
    }

    public TrinaryBranchNodeAsField(string name, IChildNode left, IChildNode centre, IChildNode right, int? instId = null) 
        : base(name, instId, left, right)
    {
        NodeType = NodeType.BranchNode;
        
        Left   = left;
        Centre = centre;
        Right  = right;
    }

    public TrinaryBranchNodeAsField(IChildNode left, IChildNode centre, IChildNode right, string name, int? instId = null) 
        : base(name, instId, left, right)
    {
        NodeType = NodeType.BranchNode;
        
        Left   = left;
        Centre = centre;
        Right  = right;
    }

    public IChildNode? Left { get; set; }
    
    public IChildNode? Centre { get; set; }
    
    public IChildNode? Right { get; set; }

    public INode? Parent { get; set; }

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           // to skip over a base RevealState cast derived type to that as AddBaseRevealStateFields will then go to it's base type
           .AddBaseRevealStateFields((OrderedParentNodeAsField<IChildNode>)this)
           .Field.WhenNonNullReveal(nameof(Left), Left)
           .Field.WhenNonNullReveal(nameof(Centre), Centre)
           .Field.WhenNonNullReveal(nameof(Right), Right)
           .Complete();
}
