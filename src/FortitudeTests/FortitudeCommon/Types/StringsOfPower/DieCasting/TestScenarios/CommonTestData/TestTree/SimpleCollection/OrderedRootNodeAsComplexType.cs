// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.SimpleCollection;

public class OrderedRootNodeAsComplexType : OrderedParentNodeAsSimpleCollection<IChildNode?>
{

    public static void ResetRootInstanceIds()
    {
        RootNodeInstanceId = 0;
    }

    public OrderedRootNodeAsComplexType()
    {
        NodeType           = NodeType.RootNode;
        RootInstanceId = Interlocked.Increment(ref RootNodeInstanceId);
    }

    public OrderedRootNodeAsComplexType(List<IChildNode?> childNodes) : this()
    {
        ChildNodes         = childNodes;
        RootInstanceId = Interlocked.Increment(ref RootNodeInstanceId);
    }

    public OrderedRootNodeAsComplexType(List<IChildNode?> childNodes, string name, int? instId = null) : base(childNodes, name, instId)
    {
        NodeType           = NodeType.RootNode;
        RootInstanceId = Interlocked.Increment(ref RootNodeInstanceId);
    }

    public int RootInstanceId { get; }

    public override int DepthToRoot => 0;
    
    public override AppendSummary RevealState(ITheOneString tos)
    {
        var ctm = tos.StartComplexType(this);
        ctm.Field.AlwaysAdd(nameof(RootInstanceId), RootInstanceId);
        ctm.AddBaseRevealStateFields(this);
        return ctm.Complete();
    }
}
