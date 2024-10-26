namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public enum QuoteSyncState
{
    InitializationState
    , Synchronising
    , InSync
    , Stale
    , Replay
}
