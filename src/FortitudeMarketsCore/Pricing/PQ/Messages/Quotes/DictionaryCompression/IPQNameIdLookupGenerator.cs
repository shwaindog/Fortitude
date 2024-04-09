#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

public interface IPQNameIdLookupGenerator : INameIdLookupGenerator,
    IPQSupportsStringUpdates<INameIdLookup>
{
    new IPQNameIdLookupGenerator Clone();
}
