// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public interface IHasNameIdLookup
{
    [JsonIgnore] INameIdLookup? NameIdLookup { get; }
}

public interface ISupportsPQNameIdLookupGenerator : IHasNameIdLookup
{
    [JsonIgnore] new IPQNameIdLookupGenerator NameIdLookup { get; set; }
}

public interface IPQSupportsStringUpdates<T> : ITracksChanges<T> where T : class
{
    bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags);
}
