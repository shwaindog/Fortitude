using System;
using System.Collections.Generic;
using FortitudeCommon.Types;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    public interface IPQSupportsStringUpdates<in T> : ITracksChanges<T>
    {
        bool UpdateFieldString(PQFieldStringUpdate updates);
        IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle);
    }
}