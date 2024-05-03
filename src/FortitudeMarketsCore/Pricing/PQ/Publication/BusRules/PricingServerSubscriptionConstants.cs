namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public static class PricingServerSubscriptionConstants
{
    public const string PricingPublisherBase = "Markets.Pricing.Publishing";
    public const string AllFeedStatusUpdates = $"{PricingPublisherBase}.StatusUpdates";
    public const string FeedBase = $"{PricingPublisherBase}.Feed.{{0}}";
    public const string FeedTickerPublish = $"{FeedBase}.PublishTicker";
    public const string FeedTickerLastPublishedQuotesRequest = $"{FeedBase}.LastPublishedQuotes";
    public const string FeedTickerResponderClient = $"{FeedBase}.ResponderClient";
    public const string FeedTickerResponderClientSnapshotsRequest = $"{PricingPublisherBase}.SnapshotsRequest";
    public const string FeedTickerResponderClientActiveTickersRequest = $"{PricingPublisherBase}.ActiveTickers";
    public const string FeedPricingConfiguredTickersRequest = $"{PricingPublisherBase}.AllConfiguredTickers";
    public static string FeedTickerPublishAddress(this string feedName) => string.Format(FeedTickerPublish, feedName);

    public static string FeedTickerLastPublishedQuotesRequestAddress(this string feedName) =>
        string.Format(FeedTickerLastPublishedQuotesRequest, feedName);

    public static string FeedTickerLastPublishedRequestAddress(this string feedName) => string.Format(FeedTickerPublish, feedName);

    public static string FeedPricingConfiguredTickersRequestAddress(this string feedName) =>
        string.Format(FeedPricingConfiguredTickersRequest, feedName);
}
