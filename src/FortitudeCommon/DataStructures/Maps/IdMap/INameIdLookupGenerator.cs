#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public interface INameIdLookupGenerator : INameIdLookup, IStoreState<INameIdLookup>
{
    int GetOrAddId(string? name);
    void AppendNewNames(IEnumerable<System.Collections.Generic.KeyValuePair<int, string>> existingPairs);
    void SetIdToName(int id, string name);
    new INameIdLookupGenerator Clone();
}
