// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.ComplexFieldCollection;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.MapCollection;

public class MapBranchNodeAsKeyedCollection<TChild> : MapParentNodeAsKeyedCollection<TChild>, IChildNode
    where TChild : class?, IChildNode?
{

    public static void ResetBranchInstanceIds()
    {
        BranchNodeInstanceId = 0;
    }

    public MapBranchNodeAsKeyedCollection()
    {
        NodeType         = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public MapBranchNodeAsKeyedCollection(List<KeyValuePair<string?, TChild>> childNodes, IReadOnlyParentNode? parent = null) : base(childNodes)
    {
        NodeType         = NodeType.BranchNode;
        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public MapBranchNodeAsKeyedCollection(string name, List<KeyValuePair<string?, TChild>>? childNodes = null
      , IReadOnlyParentNode? parent = null, int? instId = null) 
        : base(name, childNodes, instId)
    {
        NodeType = NodeType.BranchNode;
        Parent   = parent;

        BranchInstanceId = Interlocked.Increment(ref BranchNodeInstanceId);
    }

    public int BranchInstanceId { get; }
    
    public TChild? Left => ChildNodes.First().Value;
    
    public TChild? Right => ChildNodes.Last().Value;

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var cctm = tos.StartKeyedCollectionType(this);
        cctm.AddBaseRevealStateFields(this);
        cctm.LogOnlyField.AlwaysAdd(nameof(BranchInstanceId), BranchInstanceId);
        cctm.LogOnlyField.WhenConditionMetReveal(ChildNodes.Count > 2, nameof(Left), Left);
        cctm.LogOnlyField.WhenConditionMetReveal(ChildNodes.Count > 2, nameof(Right), Right);
        cctm.LogOnlyField.WhenNonNullReveal(nameof(Parent), Parent);
        return cctm.Complete();
    }
}
