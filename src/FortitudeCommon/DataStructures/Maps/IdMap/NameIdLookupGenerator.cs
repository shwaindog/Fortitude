// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Data;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public class NameIdLookupGenerator : NameIdLookup, INameIdLookupGenerator
{
    protected readonly List<int> InsertionOrder;

    private INameIdLookup? lastCopiedInstance;

    public NameIdLookupGenerator() => InsertionOrder = new List<int>();

    public NameIdLookupGenerator(INameIdLookup toClone) : base(toClone)
    {
        if (toClone is NameIdLookupGenerator nameIdLookup)
            InsertionOrder = nameIdLookup.InsertionOrder.ToList();
        else
            InsertionOrder = Cache.Keys.ToList();
    }

    public NameIdLookupGenerator(IDictionary<int, string> copyDict) : base(copyDict) => InsertionOrder = copyDict.Keys.ToList();

    public override int this[string? name] => GetOrAddId(name);

    public int GetOrAddId(string? name)
    {
        if (name == null) return 0;
        if (ReverseLookup.TryGetValue(name, out var id)) return id;
        var newId = InsertionOrder.Any() ? InsertionOrder.Max() + 1 : 1;
        TryAddNewEntry(newId, name);
        return newId;
    }

    public void AppendNewNames(IEnumerable<KeyValuePair<int, string>> existingPairs)
    {
        foreach (var existingPair in existingPairs) SetIdToName(existingPair.Key, existingPair.Value);
    }

    public void SetIdToName(int id, string? name)
    {
        TryAddNewEntry(id, name);
    }

    public virtual INameIdLookup CopyFrom(INameIdLookup source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ReferenceEquals(this, source)) return this;
        var hasFullReplace = copyMergeFlags.HasFullReplace();
        if (hasFullReplace) Clear();
        var hasAppendMissing = copyMergeFlags.HasAppendMissing();
        if (source is NameIdLookupGenerator nameIdLookupGen)
        {
            if (hasFullReplace || hasAppendMissing || !ReferenceEquals(lastCopiedInstance, source))
            {
                for (var i = 0; i < nameIdLookupGen.InsertionOrder.Count; i++)
                {
                    var newIndex = nameIdLookupGen.InsertionOrder[i];
                    var newName  = nameIdLookupGen.Cache[newIndex];
                    TryAddNewEntry(newIndex, newName);
                }
            }
            else
            {
                if (Cache.Count == nameIdLookupGen.Cache.Count) return this;

                for (var i = InsertionOrder.Count; i < nameIdLookupGen.InsertionOrder.Count; i++)
                {
                    var newIndex = nameIdLookupGen.InsertionOrder[i];
                    var newName  = nameIdLookupGen.Cache[newIndex];
                    TryAddNewEntry(newIndex, newName);
                }
            }
        }
        else
        {
            foreach (var kvp in source) TryAddNewEntry(kvp.Key, kvp.Value);
        }
        lastCopiedInstance = source;
        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((INameIdLookup)source, copyMergeFlags);
        return this;
    }

    public override object Clone() => new NameIdLookupGenerator(this);

    INameIdLookupGenerator INameIdLookupGenerator.Clone() => (INameIdLookupGenerator)Clone();

    public override bool AreEquivalent(IIdLookup<string>? other, bool exactTypes = false)
    {
        if (!(other is INameIdLookupGenerator)) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var baseSame             = base.AreEquivalent(other, exactTypes);
        var sameType             = true;
        if (exactTypes) sameType = other is NameIdLookupGenerator;

        return baseSame && sameType;
    }

    public override IEnumerator<KeyValuePair<int, string>> GetEnumerator() =>
        InsertionOrder
            .Select(i => new KeyValuePair<int, string>(i, Cache[i]))
            .GetEnumerator();

    public virtual void Clear()
    {
        Cache.Clear();
        InsertionOrder.Clear();
        ReverseLookup.Clear();
    }

    protected virtual bool TryAddNewEntry(int id, string? name)
    {
        var wasAdded = false;
        if (id == 0 || name == null) return wasAdded;
        if (Cache.TryGetValue(id, out var existingName))
        {
            if (!existingName.Equals(name))
                throw new
                    DuplicateNameException($"Attempting to add a different name ('{name}') to the" +
                                           $" NameIdLookup ('{existingName}') for attempted id {id}");
            return wasAdded;
        }
        if (ReverseLookup.TryGetValue(name, out var checkoutId))
        {
            if (!id.Equals(checkoutId))
                // Console.Out.WriteLine($"Attempting to add Chache[{id} = \"{name}\"]");
                // foreach (var i in ReverseLookup) Console.Out.WriteLine($"ReverseLookup[{i.Key}]={i.Value}");
                // foreach (var i in Cache) Console.Out.WriteLine($"Cache[{i.Key}]={i.Value}");
                throw new
                    DuplicateNameException($"Attempting to add a different name ('{name}') with" +
                                           $" id {id} to the NameIdLookup ('{checkoutId}') already set to {Cache[checkoutId]}");
            return wasAdded;
        }
        Cache.Add(id, name);
        InsertionOrder.Add(id);
        ReverseLookup.Add(name, id);
        AddedIdAndName(id, name);
        wasAdded = true;
        return wasAdded;
    }

    protected virtual void AddedIdAndName(int id, string? name) { }
}
