namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[Flags]
public enum PQMessageFlags : byte
{
    None = 0
    , Snapshot = 1
    , Update = 2
    , Replay = 4
    , Persistence = 8
}
