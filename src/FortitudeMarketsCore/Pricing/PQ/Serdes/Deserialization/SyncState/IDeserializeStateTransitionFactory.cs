#region

using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

public interface IDeserializeStateTransitionFactory<T> where T : PQLevel0Quote, new()
{
    SyncStateBase<T> TransitionToState(QuoteSyncState desiredState, SyncStateBase<T> currentState);
}
