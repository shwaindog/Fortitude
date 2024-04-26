namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public static class PricingClientSubscriptionConstants
{
    public const string PricingSubscriptionBase = "Markets.Pricing.Subscription";
    public const string AllFeedStatusUpdates = $"{PricingSubscriptionBase}.StatusUpdates";
    public const string FeedBase = $"{PricingSubscriptionBase}.Feed.{{0}}";
    public const string FeedAmendTickerPublication = $"{FeedBase}.AmendTickerPublication";
    public const string FeedDefaultAllTickersPublishBase = $"{FeedBase}.Ticker";
    public const string FeedDefaultTickerPublish = $"{FeedDefaultAllTickersPublishBase}.{{1}}";
    public const string FeedRequestResponseRegistration = $"{FeedBase}.RequestResponseRegistration";
    public const string FeedAvailableTickersUpdateTemplate = $"{FeedBase}.AvailableTickersUpdate";
    public const string FeedAvailableTickersRequest = $"{FeedBase}.AvailableTickers";
    public const string FeedTickersSnapshotRequest = $"{FeedBase}.TickersSnapshotsRequest";
    public const string FeedTickerHealthRequest = $"{FeedBase}.TickerHealthRequest";
    public const string FeedStatusRequest = $"{FeedBase}.TickerHealthRequest";
    public const string FeedStatusUpdate = $"{FeedBase}.StatusUpdate";
    public const string FeedShutdownRequest = $"{FeedBase}.Shutdown";

    public static string FeedAddress(this string feedName) => string.Format(FeedBase, feedName);

    public static string FeedDefaultAllTickersPublishAddress(this string feedName) =>
        string.Format(FeedDefaultAllTickersPublishBase + ".*", feedName);

    public static string FeedDefaultTickerPublishAddress(this string feedName, string ticker) =>
        string.Format(FeedDefaultTickerPublish, feedName, ticker);

    public static string FeedAmendTickerPublicationAddress(this string feedName) => string.Format(FeedAmendTickerPublication, feedName);

    public static string FeedAvailableTickersRequestAddress(this string feedName) => string.Format(FeedAvailableTickersRequest, feedName);

    public static string FeedTickersSnapshotRequestAddress(this string feedName) => string.Format(FeedTickersSnapshotRequest, feedName);

    public static string FeedAvailableTickersUpdateAddress(this string feedName) => string.Format(FeedAvailableTickersUpdateTemplate, feedName);

    public static string FeedRequestResponseRegistrationAddress(this string feedName) => string.Format(FeedRequestResponseRegistration, feedName);

    public static string FeedTickerHealthRequestAddress(this string feedName) => string.Format(FeedTickerHealthRequest, feedName);
    public static string FeedStatusUpdateAddress(this string feedName) => string.Format(FeedStatusUpdate, feedName);

    public static string ExtractTickerFromDefaultTickerPublishAddress(this string address, string feedName) =>
        address.Replace(FeedDefaultAllTickersPublishBase + ".", "");
}
