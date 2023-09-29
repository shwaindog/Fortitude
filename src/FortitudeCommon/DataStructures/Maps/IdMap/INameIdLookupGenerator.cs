using System.Collections.Generic;
using FortitudeCommon.Types;
using Generic = System.Collections.Generic;

namespace FortitudeCommon.DataStructures.Maps.IdMap
{
    public interface INameIdLookupGenerator : INameIdLookup, IStoreState<INameIdLookup>
    {
        int GetOrAddId(string name);
        void AppendNewNames(IEnumerable<Generic.KeyValuePair<int, string>> existingPairs);
        void SetIdToName(int id, string name);
        new INameIdLookupGenerator Clone();
    }
}