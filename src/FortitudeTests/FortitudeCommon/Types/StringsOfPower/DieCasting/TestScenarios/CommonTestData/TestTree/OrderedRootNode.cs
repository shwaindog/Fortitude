// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public class OrderedRootNode : OrderedParentNode<IChildNode?>
{
    private static int rootNodeInstanceId;

    public OrderedRootNode()
    {
        NodeType           = NodeType.RootNode;
        RootNodeInstanceId = Interlocked.Increment(ref rootNodeInstanceId);
    }

    public OrderedRootNode(List<IChildNode?> childNodes) : this()
    {
        ChildNodes         = childNodes;
        RootNodeInstanceId = Interlocked.Increment(ref rootNodeInstanceId);
    }

    public OrderedRootNode(List<IChildNode?> childNodes, string name, int? instId = null) : base(childNodes, name, instId)
    {
        NodeType           = NodeType.RootNode;
        RootNodeInstanceId = Interlocked.Increment(ref rootNodeInstanceId);
    }

    public int RootNodeInstanceId { get; }

    public override int DepthToRoot => 0;
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(RootNodeInstanceId), RootNodeInstanceId)
           .AddBaseRevealStateFields(this)
           .Complete();
}
