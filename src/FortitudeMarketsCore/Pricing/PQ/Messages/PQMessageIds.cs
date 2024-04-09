namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public static class PQMessage
{
    public const uint PQMessageIdBase = 40_000;
}

public enum PQMessageIds : uint
{
    Quote = PQMessage.PQMessageIdBase + 1
    , HeartBeat = PQMessage.PQMessageIdBase + 2
    , SnapshotIdsRequest = PQMessage.PQMessageIdBase + 3
    , SourceTickerInfoRequest = PQMessage.PQMessageIdBase + 4
    , SourceTickerInfoResponse = PQMessage.PQMessageIdBase + 5
}
