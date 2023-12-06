#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

public interface IPQSupportsStringUpdates<T> : ITracksChanges<T> where T : class
{
    bool UpdateFieldString(PQFieldStringUpdate updates);
    IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle);
}
