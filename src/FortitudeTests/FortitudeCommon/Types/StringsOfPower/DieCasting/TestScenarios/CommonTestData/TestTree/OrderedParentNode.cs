// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree;


public interface IReadOnlyParentNode : INode
{
    IEnumerable<IChildNode?>? ChildNodes { get; }
}

public abstract class OrderedParentNode<TChild> : Node, IReadOnlyParentNode
    where TChild : class?, IChildNode?
{
    private List<TChild>? childNodes;

    protected OrderedParentNode()
    {
        NodeType = NodeType.RootNode;
    }

    protected OrderedParentNode(List<TChild>? childNodes) : this()
    {
        ChildNodes = childNodes;
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

    protected OrderedParentNode(params TChild[] children) : this()
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

    protected OrderedParentNode(string name, params TChild[] children) : base(name)
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

    protected OrderedParentNode(string name, int? instId, params TChild[] children) : base(name, instId)
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

    protected OrderedParentNode(List<TChild> childNodes, string name, int? instId = null) : base(name, instId)
    {
        ChildNodes = childNodes;
        NodeType   = NodeType.RootNode;
    }

    IEnumerable<IChildNode?>? IReadOnlyParentNode.ChildNodes => ChildNodes?.Select(cn => cn as IChildNode);

    public List<TChild>? ChildNodes
    {
        get => childNodes;
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .AddBaseRevealStateFields(this)
           .CollectionField.AlwaysRevealAll(nameof(ChildNodes), ChildNodes)
           .Complete();
}
