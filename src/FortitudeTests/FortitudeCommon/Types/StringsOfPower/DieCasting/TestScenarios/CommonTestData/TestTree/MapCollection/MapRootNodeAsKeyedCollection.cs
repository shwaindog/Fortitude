// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.MapCollection;

public class MapRootNodeAsKeyedCollection : MapParentNodeAsKeyedCollection<IChildNode?>
{

    public static void ResetRootInstanceIds()
    {
        RootNodeInstanceId = 0;
    }

    public MapRootNodeAsKeyedCollection()
    {
        NodeType       = NodeType.RootNode;
        RootInstanceId = Interlocked.Increment(ref RootNodeInstanceId);
    }

    public MapRootNodeAsKeyedCollection(List<KeyValuePair<string?, IChildNode?>> childNodes) : this()
    {
        ChildNodes     = childNodes;
        RootInstanceId = Interlocked.Increment(ref RootNodeInstanceId);
    }

    public MapRootNodeAsKeyedCollection(List<KeyValuePair<string?, IChildNode?>> childNodes, string name, int? instId = null) : base(name, childNodes, instId)
    {
        NodeType       = NodeType.RootNode;
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