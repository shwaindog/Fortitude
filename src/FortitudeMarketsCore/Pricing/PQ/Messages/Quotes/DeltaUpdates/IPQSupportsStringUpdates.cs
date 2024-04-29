#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public interface IHasNameIdLookup
{
    INameIdLookup? NameIdLookup { get; }
}

public interface ISupportsPQNameIdLookupGenerator : IHasNameIdLookup
{
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }
}

public interface IPQSupportsStringUpdates<T> : ITracksChanges<T> where T : class
{
    bool UpdateFieldString(PQFieldStringUpdate stringUpdate);
    IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags);
}
