// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.SimpleCollection;


public interface IReadOnlyCollectionParentNode : INode, IEnumerable<IChildNode>
{
}

public class OrderedParentNodeAsSimpleCollection<TChild> : Node, IReadOnlyCollectionParentNode, IChildNode
    where TChild : class?, IChildNode?
{
    private List<TChild>? childNodes;

    public OrderedParentNodeAsSimpleCollection()
    {
        NodeType = NodeType.RootNode;
    }

    public OrderedParentNodeAsSimpleCollection(List<TChild>? childNodes) : this()
    {
        ChildNodes = childNodes ?? new List<TChild>();
        if (childNodes != null)
        {
            foreach (var child in childNodes)
            {
                if (child != null)
                {
                    child.Parent = this; 
                }
            }
        }
    }

    public OrderedParentNodeAsSimpleCollection(params TChild[] children) : this()
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child != null)
            {
                child.Parent = this; 
            }
        }
    }

    public OrderedParentNodeAsSimpleCollection(string name, params TChild[] children) : base(name)
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child != null)
            {
                child.Parent = this; 
            }
        }
    }

    public OrderedParentNodeAsSimpleCollection(string name, int? instId, params TChild[] children) : base(name, instId)
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child != null)
            {
                child.Parent = this; 
            }
        }
    }

    public OrderedParentNodeAsSimpleCollection(List<TChild> childNodes, string name, int? instId = null) : base(name, instId)
    {
        ChildNodes = childNodes;
        NodeType   = NodeType.RootNode;
    }

    public List<TChild> ChildNodes
    {
        get => childNodes ??= new List<TChild>();
        set
        {
            if (childNodes == value) return;
            if (childNodes != null)
            {
                foreach (var child in childNodes)
                {
                    if (child != null)
                    {
                        if (value != null && !value.Contains(child)) { child.Parent = null; }
                    }
                }
            }
            childNodes = value;
            if (childNodes != null)
            {
                foreach (var child in childNodes)
                {
                    if (child != null)
                    {
                        child.Parent = this; 
                    }
                }
            }
        }
    }

    public INode? Parent { get; set; }

    IEnumerator IEnumerable.       GetEnumerator() => GetEnumerator();

    public IEnumerator<IChildNode> GetEnumerator() => childNodes!.GetEnumerator();

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var sctm = tos.StartSimpleCollectionType(this);
        sctm.RevealAll(ChildNodes);
        return sctm.Complete();
    }
}
