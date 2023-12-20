#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

public interface IPQNameIdLookupGenerator : INameIdLookupGenerator,
    IPQSupportsStringUpdates<INameIdLookup>
{
    new IPQNameIdLookupGenerator Clone();
}
