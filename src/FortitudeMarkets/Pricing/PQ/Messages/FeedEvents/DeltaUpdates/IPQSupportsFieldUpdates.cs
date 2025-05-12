// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public interface IPQSupportsFieldUpdates<T> : ITracksChanges<T> where T : class
{
    int UpdateField(PQFieldUpdate fieldUpdate);

    IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);
}
