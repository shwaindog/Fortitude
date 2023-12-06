#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

public interface IPQSupportsFieldUpdates<T> : ITracksChanges<T> where T : class
{
    int UpdateField(PQFieldUpdate fieldUpdate);

    IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);
}
