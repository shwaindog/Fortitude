// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public class NameIdLookup : IdLookup<string>, INameIdLookup
{
    public NameIdLookup() { }

    public NameIdLookup(INameIdLookup toClone) : base(toClone) { }

    public NameIdLookup(IDictionary<int, string> copyDict) : base(copyDict) { }

    public string? GetName(int id) => GetValue(id);

    public override object Clone() => new NameIdLookup(this);

    INameIdLookup INameIdLookup.Clone() => (INameIdLookup)Clone();

    public override bool AreEquivalent(IIdLookup<string>? other, bool exactTypes = false)
    {
        if (!(other is INameIdLookup)) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        return base.AreEquivalent(other, exactTypes);
    }
}
