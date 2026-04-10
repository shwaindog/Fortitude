// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CommonTestData.TestTree.MapCollection;


public interface IReadOnlyMapCollectionParentNode : INode, IEnumerable<KeyValuePair<string?, IChildNode?>>
{
}

public class MapParentNodeAsKeyedCollection<TChild> : Node, IReadOnlyMapCollectionParentNode, IChildNode
    where TChild : class?, IChildNode?
{
    private List<KeyValuePair<string?, TChild>>? childNodes;

    public MapParentNodeAsKeyedCollection()
    {
        NodeType = NodeType.RootNode;
    }

    public MapParentNodeAsKeyedCollection(List<KeyValuePair<string?, TChild>>? childNodes) : this()
    {
        ChildNodes = childNodes ?? new List<KeyValuePair<string?, TChild>>();
        if (childNodes != null)
        {
            foreach (var child in childNodes)
            {
                if (child.Value != null)
                {
                    child.Value.Parent = this; 
                }
            }
        }
    }

    public MapParentNodeAsKeyedCollection(params KeyValuePair<string?, TChild>[] children) : this()
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child.Value != null)
            {
                child.Value.Parent = this; 
            }
        }
    }

    public MapParentNodeAsKeyedCollection(string name, params KeyValuePair<string?, TChild>[] children) : base(name)
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child.Value != null)
            {
                child.Value.Parent = this; 
            }
        }
    }

    public MapParentNodeAsKeyedCollection(string name, int? instId, params KeyValuePair<string?, TChild>[] children) : base(name, instId)
    {
        childNodes = children.ToList();
        foreach (var child in childNodes)
        {
            if (child.Value != null)
            {
                child.Value.Parent = this; 
            }
        }
    }

    public void Add(string? key, TChild value)
    {
        childNodes ??= new List<KeyValuePair<string?, TChild>>();
        childNodes.Add(new KeyValuePair<string?, TChild>(key, value));
    }

    public void Add(KeyValuePair<string?, TChild> item)
    {
        childNodes ??= new List<KeyValuePair<string?, TChild>>();
        childNodes.Add(item);
    }

    public MapParentNodeAsKeyedCollection(string name, List<KeyValuePair<string?, TChild>>? childNodes = null
      , int? instId = null) : base(name, instId)
    {
        ChildNodes = childNodes ?? new List<KeyValuePair<string?, TChild>>();
        NodeType   = NodeType.RootNode;
    }

    public List<KeyValuePair<string?, TChild>> ChildNodes
    {
        get => childNodes ??= new List<KeyValuePair<string?, TChild>>();
        set
        {
            if (childNodes == value) return;
            if (childNodes != null)
            {
                foreach (var childKvp in childNodes)
                {
                    if (childKvp.Value != null)
                    {
                        if (value != null && !value.Contains(childKvp)) { childKvp.Value.Parent = null; }
                    }
                }
            }
            childNodes = value;
            if (childNodes != null)
            {
                foreach (var child in childNodes)
                {
                    if (child.Value != null)
                    {
                        child.Value.Parent = this; 
                    }
                }
            }
        }
    }

    public INode? Parent { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string?,IChildNode?>> GetEnumerator() => 
        childNodes?.Select(kvp => new KeyValuePair<string?,IChildNode?>( kvp.Key, kvp.Value)).GetEnumerator() 
        ?? Enumerable.Empty<KeyValuePair<string?,IChildNode?>>().GetEnumerator();

    public override AppendSummary RevealState(ITheOneString tos)
    {
        var sctm = tos.StartKeyedCollectionType(this);
        sctm.AddAll(ChildNodes);
        return sctm.Complete();
    }
}