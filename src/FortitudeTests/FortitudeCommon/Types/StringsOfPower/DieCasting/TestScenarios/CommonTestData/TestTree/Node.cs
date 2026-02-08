// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;

public enum NodeType
{
    RootNode   = 1
  , BranchNode = 2
  , LeafNode   = 3
}

public interface INode : IEquatable<INode>, IStringBearer
{
    string Name { get; set; }
    int GlobalNodeInstanceId { get; }
    NodeType NodeType { get; }
    int CalcDepthToRoot(IList<INode> visited);

    int DepthToRoot { get; }
}

public abstract class Node : INode
{
    private static   int globalNodeInstanceId;
    protected static int BranchNodeInstanceId;
    protected static int RootNodeInstanceId;
    protected static int LeafNodeInstanceId;

    public static void ResetInstanceIds()
    {
        globalNodeInstanceId = 0;
        BranchNodeInstanceId = 0;
        RootNodeInstanceId   = 0;
        LeafNodeInstanceId   = 0;
    }

    public Node()
    {
        GlobalNodeInstanceId = Interlocked.Increment(ref globalNodeInstanceId);
        Name                 = GetType().Name + "_" + GlobalNodeInstanceId;
    }

    public Node(string name, int? instId = null)
    {
        Name                 = name;
        GlobalNodeInstanceId = instId ?? Interlocked.Increment(ref globalNodeInstanceId);
    }

    public string Name { get; set; }
    public int GlobalNodeInstanceId { get; set; }

    public NodeType NodeType { get; set; }


    public virtual int DepthToRoot
    {
        get
        {
            var circleRefDetect = Recycler.ThreadStaticRecycler.Borrow<ReusableList<INode>>();
            int calcedDepth     = CalcDepthToRoot(circleRefDetect);
            circleRefDetect.DecrementRefCount();
            return calcedDepth;
        }
    }

    public virtual AppendSummary RevealState(ITheOneString tos)
    {
        var md = tos.StartComplexType(this);
        md.Field.AlwaysAdd(nameof(Name), Name);
        md.Field.AlwaysAdd(nameof(GlobalNodeInstanceId), GlobalNodeInstanceId);
        md.Field.AlwaysAdd(nameof(NodeType), NodeType);
        md.Field.WhenNonDefaultAdd(nameof(DepthToRoot), DepthToRoot);
        return md.Complete();
    }

    public bool Equals(INode? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GlobalNodeInstanceId == other.GlobalNodeInstanceId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((INode)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => GlobalNodeInstanceId;

    public override string ToString() => "toS-" + this.DefaultToString();

    public int CalcDepthToRoot(IList<INode> visited)
    {
        if (NodeType == NodeType.RootNode) return 0;
        if (this is IChildNode child)
        {
            if (visited.Contains(child)) { return int.MinValue; }
            visited.Add(this);
            return (child.Parent?.CalcDepthToRoot(visited) ?? -1) + 1;
        }
        throw new InvalidOperationException("Expected either a root or child node");
    }
}

public interface IChildNode : INode
{
    IReadOnlyParentNode? Parent { get; set; }
}
