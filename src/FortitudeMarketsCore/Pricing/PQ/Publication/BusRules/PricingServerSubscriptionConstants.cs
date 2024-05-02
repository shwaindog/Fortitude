namespace FortitudeMarketsCore.Pricing.PQ.Publication.BusRules;

public static class PricingServerSubscriptionConstants
{
    public const string PricingPublisherBase = "Markets.Pricing.Publishing";
    public const string AllFeedStatusUpdates = $"{PricingPublisherBase}.StatusUpdates";
    public const string FeedBase = $"{PricingPublisherBase}.Feed.{{0}}";
    public const string FeedTickerPublish = $"{FeedBase}.Ticker";
    public static string FeedTickerPublishAddress(this string feedName) => string.Format(FeedTickerPublish, feedName);
}
