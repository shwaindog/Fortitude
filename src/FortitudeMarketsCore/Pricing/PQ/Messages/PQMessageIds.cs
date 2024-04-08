namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public static class PQMessage
{
    public const uint PQMessageIdBase = 40_000;
}

public enum PQMessageIds : uint
{
    SnapshotIdsRequest = PQMessage.PQMessageIdBase + 1
}
