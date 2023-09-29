using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState
{
    internal class ReplayState<T> : SyncStateBase<T> where T : class, IPQLevel0Quote
    {
        public ReplayState(IPQQuoteDeserializer<T> linkedDeserializer) 
            : base(linkedDeserializer, QuoteSyncState.Replay)
        {
        }
    }
}