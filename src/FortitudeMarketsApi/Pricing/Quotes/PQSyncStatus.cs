namespace FortitudeMarketsApi.Pricing.Quotes
{
    public enum PQSyncStatus : byte
    {
        OutOfSync = 0,
        Good =      1,
        Stale =     2,
        FeedDown =  4
    }
}