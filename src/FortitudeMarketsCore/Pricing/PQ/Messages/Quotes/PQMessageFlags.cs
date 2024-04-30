namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[Flags]
public enum PQMessageFlags : byte
{
    None = 0
    , Complete = 1
    , Snapshot = 3
    , Update = 4
    , CompleteUpdate = 5
    , Replay = 8
    , Persistence = 16
}
