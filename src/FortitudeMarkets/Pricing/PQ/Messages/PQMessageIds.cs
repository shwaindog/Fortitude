namespace FortitudeMarkets.Pricing.PQ.Messages;

public static class PQMessage
{
    public const uint PQMessageIdBase = 40_000;
}

public enum PQMessageIds : uint
{
    Quote                    = PQMessage.PQMessageIdBase + 1
  , FeedEvent                = PQMessage.PQMessageIdBase + 2
  , HeartBeat                = PQMessage.PQMessageIdBase + 3
  , SnapshotIdsRequest       = PQMessage.PQMessageIdBase + 4
  , SourceTickerInfoRequest  = PQMessage.PQMessageIdBase + 5
  , SourceTickerInfoResponse = PQMessage.PQMessageIdBase + 6
}
