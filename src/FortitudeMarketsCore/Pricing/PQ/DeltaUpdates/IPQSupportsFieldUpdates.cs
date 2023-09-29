using System;
using System.Collections.Generic;
using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    public interface IPQSupportsFieldUpdates<in T> : ITracksChanges<T>
    {
        int UpdateField(PQFieldUpdate fieldUpdate);
        IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSettings = null);
    }
}