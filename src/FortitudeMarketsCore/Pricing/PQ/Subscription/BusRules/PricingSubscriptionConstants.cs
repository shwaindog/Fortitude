namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

public static class PricingSubscriptionConstants
{
    public const string PricingSubscriptionBasePath = "Markets.Pricing.Subscription.PQ";
    public const string ClientMultiFeedStatusUpdates = $"{PricingSubscriptionBasePath}.StatusUpdates";
    public const string SourceFeedUpdatesAddressTemplate = $"{PricingSubscriptionBasePath}.Feed.{{0}}";
    public const string AmendTickerSubscribeAndPublishAddressTemplate = $"{SourceFeedUpdatesAddressTemplate}.AmendPublication.Ticker";
    public const string DefaultAllTickersPublishAddressTemplate = $"{SourceFeedUpdatesAddressTemplate}.Ticker";
    public const string DefaultTickerPublishAddressTemplate = $"{DefaultAllTickersPublishAddressTemplate}.{{1}}";
    public const string RegisterRequestIdResponseSourceTemplate = $"{SourceFeedUpdatesAddressTemplate}.RegisterExpectedResponseHandler";

    public static string FeedAddress(this string feedName) => string.Format(SourceFeedUpdatesAddressTemplate, feedName);

    public static string DefaultAllTickersPublishAddress(this string feedName) =>
        string.Format(DefaultAllTickersPublishAddressTemplate + ".*", feedName);

    public static string DefaultTickerPublishAddress(this string feedName, string ticker) =>
        string.Format(DefaultTickerPublishAddressTemplate, feedName, ticker);

    public static string AmendTickerSubscribeAndPublishAddress(this string feedName) =>
        string.Format(AmendTickerSubscribeAndPublishAddressTemplate, feedName);

    public static string RegisterRequestIdResponseSourceAddress(this string feedName) =>
        string.Format(RegisterRequestIdResponseSourceTemplate, feedName);

    public static string ExtractTickerFromDefaultPublishAddress(this string address) =>
        address.Replace(DefaultAllTickersPublishAddressTemplate + ".", "");
}
