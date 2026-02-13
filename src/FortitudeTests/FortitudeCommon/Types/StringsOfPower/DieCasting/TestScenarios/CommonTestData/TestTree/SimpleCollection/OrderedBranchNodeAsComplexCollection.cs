// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.SimpleCollection;

public class OrderedBranchNodeAsComplexCollection<TChild> : OrderedParentNodeAsSimpleCollection<TChild>, IChildNode
    where TChild : class?, IChildNode?
{

    public static void ResetBranchInstanceIds()
    {
        BranchNodeInstanceId = 0;
    }

    public OrderedBranchNodeAsComplexCollection()
    {
        NodeType             = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNodeAsComplexCollection(List<TChild> childNodes, IReadOnlyParentNode? parent = null) : base(childNodes)
    {
        NodeType             = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public OrderedBranchNodeAsComplexCollection(List<TChild> childNodes, string name, IReadOnlyParentNode? parent = null, int? instId = null) 
        : base(childNodes, name, instId)
    {
        NodeType = NodeType.BranchNode;
        Parent   = parent;

        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public INode? Parent { get; set; }

    public int BranchInstanceId { get; }

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var cctm = tos.StartComplexCollectionType(this);
        cctm.AddBaseRevealStateFields(this);
        cctm.LogOnlyField.AlwaysAdd(nameof(BranchInstanceId), BranchInstanceId);
        cctm.LogOnlyField.WhenNonNullReveal(nameof(Parent), Parent);
        return cctm.Complete();
    }
}
