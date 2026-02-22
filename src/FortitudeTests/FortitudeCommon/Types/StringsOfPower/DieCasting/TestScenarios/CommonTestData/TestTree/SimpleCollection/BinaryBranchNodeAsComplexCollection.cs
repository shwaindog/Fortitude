// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.SimpleCollection;

public class BinaryBranchNodeAsComplexCollection<TChild> : OrderedParentNodeAsSimpleCollection<TChild>, IChildNode
    where TChild : class?, IChildNode?
{

    public BinaryBranchNodeAsComplexCollection()
    {
        NodeType = NodeType.BranchNode;
    }

    public BinaryBranchNodeAsComplexCollection(string name, int? instId = null) : base([], name, instId)
    {
        NodeType = NodeType.BranchNode;
    }

    public BinaryBranchNodeAsComplexCollection(string name, TChild left, TChild right, int? instId = null) : base(name, instId, left, right)
    {
        NodeType = NodeType.BranchNode;
        
        Left     = left;
        Right    = right;
    }

    public BinaryBranchNodeAsComplexCollection(TChild left, TChild right, string name, int? instId = null) : base(name, instId, left, right)
    {
        NodeType = NodeType.BranchNode;
        
        Left     = left;
        Right    = right;
    }

    public TChild? Left { get; set; }
    
    public TChild? Right { get; set; }

    public INode? Parent { get; set; }

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           // to skip over a base RevealState cast derived type to that as AddBaseRevealStateFields will then go to it's base type
           .AddBaseRevealStateFields((OrderedParentNodeAsSimpleCollection<TChild>)this)
           .Field.WhenNonNullReveal(nameof(Left), Left)
           .Field.WhenNonNullReveal(nameof(Right), Right)
           .Field.WhenNonNullReveal(nameof(Parent), Parent)
           .Complete();
}
