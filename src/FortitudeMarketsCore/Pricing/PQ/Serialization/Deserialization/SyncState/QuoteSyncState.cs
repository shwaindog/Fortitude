namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public enum QuoteSyncState
{
    InitializationState
    , Synchronising
    , InSync
    , Stale
    , Replay
}
