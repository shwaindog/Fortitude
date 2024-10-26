// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

public static class PricingClientSubscriptionConstants
{
    public const string PricingSubscriptionBase            = "Markets.Pricing.Subscription";
    public const string AllFeedStatusUpdates               = $"{PricingSubscriptionBase}.StatusUpdates";
    public const string FeedBase                           = $"{PricingSubscriptionBase}.Feed.{{0}}";
    public const string FeedAmendTickerPublicationBase     = $"{FeedBase}.AmendTickerPublication.Ticker";
    public const string FeedAmendTickerPublication         = $"{FeedAmendTickerPublicationBase}.{{1}}";
    public const string FeedDefaultAllTickersPublishBase   = $"{FeedBase}.Ticker";
    public const string FeedDefaultSpecificTickersPublish  = $"{FeedDefaultAllTickersPublishBase}.{{1}}";
    public const string FeedRequestResponseRegistration    = $"{FeedBase}.RequestResponseRegistration";
    public const string FeedAvailableTickersUpdateTemplate = $"{FeedBase}.AvailableTickersUpdate";
    public const string FeedAvailableTickersRequest        = $"{FeedBase}.AvailableTickers";
    public const string FeedTickersSnapshotRequest         = $"{FeedBase}.TickersSnapshotsRequest";
    public const string FeedTickerHealthRequest            = $"{FeedBase}.TickerHealthRequest";
    public const string FeedStatusRequest                  = $"{FeedBase}.FeedStatusRequest";
    public const string FeedStatusUpdate                   = $"{FeedBase}.StatusUpdate";
    public const string FeedShutdownRequest                = $"{FeedBase}.Shutdown";
    public const string FeedAmendTickerPublicationRule     = $"{FeedBase}.AmendTickerPublicationRule";
    public const string FeedRegisterRemoteResponseRule     = $"{FeedBase}.RegisterRemoteResponseCallback";

    public static string FeedAddress(this string feedName) => string.Format(FeedBase, feedName);

    public static string SubscribeToTickerQuotes(this string feedName, string tickerName) =>
        string.Format(FeedDefaultSpecificTickersPublish, feedName, tickerName);

    public static string FeedDefaultAllTickersPublishBaseAddress(this string feedName) => string.Format(FeedDefaultAllTickersPublishBase, feedName);

    public static string FeedDefaultAllTickersPublishInterceptPattern(this string feedName) =>
        feedName.FeedDefaultAllTickersPublishBaseAddress() + ".*";

    public static string AllFeedsStatusAddress() => string.Format(AllFeedStatusUpdates);

    public static string FeedAmendTickerPublicationAddress(this string feedName) => string.Format(FeedAmendTickerPublicationBase + ".", feedName);

    public static string FeedAvailableTickersRequestAddress(this string feedName) => string.Format(FeedAvailableTickersRequest, feedName);

    public static string FeedTickersSnapshotRequestAddress(this string feedName) => string.Format(FeedTickersSnapshotRequest, feedName);

    public static string FeedAvailableTickersUpdateAddress(this string feedName) => string.Format(FeedAvailableTickersUpdateTemplate, feedName);

    public static string FeedRequestResponseRegistrationAddress(this string feedName) => string.Format(FeedRequestResponseRegistration, feedName);

    public static string FeedStatusRequestAddress(this string feedName) => string.Format(FeedStatusRequest, feedName);

    public static string FeedTickerHealthRequestAddress(this string feedName) => string.Format(FeedTickerHealthRequest, feedName);
    public static string FeedStatusUpdateAddress(this string feedName)        => string.Format(FeedStatusUpdate, feedName);

    public static string FeedAmendTickerPublicationRuleName(this string feedName) => string.Format(FeedAmendTickerPublicationRule, feedName);
    public static string FeedRegisterRemoteResponseRuleName(this string feedName) => string.Format(FeedRegisterRemoteResponseRule, feedName);

    public static string FeedAmendTickerPublicationAddress(this string feedName, string ticker) =>
        string.Format(FeedAmendTickerPublication, feedName, ticker);

    public static string ExtractTickerFromFeedDefaultTickerPublishAddress(this string address, string feedName) =>
        address.Replace(feedName.FeedDefaultAllTickersPublishBaseAddress() + ".", "");

    public static string ExtractTickerFromAmendPublicationAddress(this string address, string feedName) =>
        address.Replace(feedName.FeedAmendTickerPublicationAddress(), "");
}
