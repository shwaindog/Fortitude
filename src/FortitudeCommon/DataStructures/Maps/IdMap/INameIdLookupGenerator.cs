// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public interface INameIdLookupGenerator : INameIdLookup, IStoreState<INameIdLookup>
{
    int                        GetOrAddId(string? name);
    void                       AppendNewNames(IEnumerable<KeyValuePair<int, string>> existingPairs);
    void                       SetIdToName(int id, string name);
    new INameIdLookupGenerator Clone();
}
