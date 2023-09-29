using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState
{
    public interface IDeserializeStateTransitionFactory<T> where T : class, IPQLevel0Quote
    {
        SyncStateBase<T> TransitionToState(QuoteSyncState desiredState, SyncStateBase<T> currentState);
    }
}