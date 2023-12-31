#region

using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

internal class ReplayState<T> : SyncStateBase<T> where T : PQLevel0Quote, new()
{
    public ReplayState(IPQQuoteDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Replay) { }
}
