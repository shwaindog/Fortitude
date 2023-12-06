#region

using System.Data;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public class NameIdLookupGenerator : NameIdLookup, INameIdLookupGenerator
{
    protected int LargestAddedId;

    public NameIdLookupGenerator() { }

    public NameIdLookupGenerator(INameIdLookup toClone) : base(toClone)
    {
        if (toClone is NameIdLookupGenerator nameIdToClone)
            LargestAddedId = nameIdToClone.LargestAddedId;
        else
            LargestAddedId = Cache.Keys.Max();
    }

    public NameIdLookupGenerator(IDictionary<int, string> copyDict) : base(copyDict) =>
        LargestAddedId = Cache.Keys.Max();

    public override int this[string? name] => GetOrAddId(name);

    public int GetOrAddId(string? name)
    {
        if (name == null) return 0;
        if (ReverseLookup.TryGetValue(name, out var id)) return id;
        var newId = LargestAddedId + 1;
        AddNewEntry(newId, name);
        return newId;
    }

    public void AppendNewNames(IEnumerable<System.Collections.Generic.KeyValuePair<int, string>> existingPairs)
    {
        foreach (var existingPair in existingPairs) SetIdToName(existingPair.Key, existingPair.Value);
    }

    public void SetIdToName(int id, string name)
    {
        if (Cache.TryGetValue(id, out var existingName))
        {
            if (!existingName.Equals(name))
                throw new DuplicateNameException("Attempting to add a different name to the NameIdLookup");
        }
        else
        {
            AddNewEntry(id, name);
        }
    }

    public virtual INameIdLookup CopyFrom(INameIdLookup source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ReferenceEquals(this, source)) return this;
        if (source is NameIdLookupGenerator nameIdLookupGen)
        {
            if (Cache.Count == nameIdLookupGen.Cache.Count &&
                LargestAddedId == nameIdLookupGen.LargestAddedId) return this;
            foreach (var kvp in nameIdLookupGen.Cache)
            {
                Cache[kvp.Key] = kvp.Value;
                ReverseLookup[kvp.Value] = kvp.Key;
            }

            LargestAddedId = Math.Max(LargestAddedId, nameIdLookupGen.LargestAddedId);
        }
        else
        {
            foreach (var kvp in source) SetIdToName(kvp.Key, kvp.Value); //Updates LargestAddId if required
        }

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

        var baseSame = base.AreEquivalent(other, exactTypes);
        var largestIdSame = true;
        if (exactTypes)
        {
            var nameIdGenerator = other as NameIdLookupGenerator;
            largestIdSame = LargestAddedId == nameIdGenerator?.LargestAddedId;
        }

        return baseSame && largestIdSame;
    }

    protected virtual void AddNewEntry(int id, string name)
    {
        Cache.Add(id, name);
        ReverseLookup.Add(name, id);
        if (id > LargestAddedId) LargestAddedId = id;
    }
}
