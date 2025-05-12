using FortitudeMarkets.Pricing.FeedEvents.Accounts;
using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Indicators;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.FeedEvents.PriceAdjustments;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Signals;
using FortitudeMarkets.Pricing.FeedEvents.Strategies;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.TrackingTimes;
using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface INonPricingFeedEvent
{
    ISourceTickerInfo      SourceTickerInfo       { get; }
    TickerQuoteDetailLevel TickerQuoteDetailLevel { get; }

    bool     IsReplay                 { get; }
    DateTime LastSourceFeedUpdateTime { get; }
    DateTime ClientReceivedTime       { get; }
    DateTime ClientProcessedTime      { get; }
    DateTime ClientPublishedTime      { get; }
    DateTime AdapterReceivedTime      { get; }
    DateTime AdapterSentTime          { get; }
    DateTime DownstreamTime           { get; }
    uint     SourceSequenceNumber     { get; }
    uint     AdapterSequenceNumber    { get; }
    uint     ClientSequenceNumber     { get; }
    uint     FeedSequenceNumber       { get; }

    FeedSyncStatus               FeedSyncStatus               { get; }
    FeedConnectivityStatusFlags  FeedMarketConnectivityStatus { get; }
    FeedEventUpdateFlags         FeedEventUpdateFlags         { get; }
    ICandle?                     ConflationSummaryCandle      { get; }
    IPublishedMarketEvents?      MarketEvents                 { get; }
    IPublishedCandles?           PublishedCandles             { get; }
    IPublishedIndicators?        PublishedIndicators          { get; }
    IRecentlyTradedHistory?      RecentTradedHistory          { get; }
    IPublishedInternalOrders?    PublishedInternalOrders      { get; }
    IPublishedAccounts?          PublishedAccounts            { get; }
    IPublishedLimits?            PublishedLimits              { get; }
    ILimitBreaches?              LimitBreaches                { get; }
    IMarginDetails?              MarginDetails                { get; }
    IPnLConversions?             TickerPnLConversionRate      { get; }
    IAdditionalTrackingFields?   AdditionalTrackingFields     { get; }
    ITickerRegionInfo?           TickerRegionInfo             { get; }
    IPublishedSignals?           PublishedSignals             { get; }
    IPublishedStrategyDecisions? PublishedStrategyDecisions   { get; }
    IAdapterExecutionStatistics? AdapterExecutionStatistics   { get; }


    IPublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; }
}

public interface IMutableNonPricingFeedEvent : INonPricingFeedEvent
{
    new ISourceTickerInfo SourceTickerInfo { get; set; }

    new bool     IsReplay                 { get; set; }
    new DateTime LastSourceFeedUpdateTime { get; set; }
    new DateTime ClientReceivedTime       { get; set; }
    new DateTime ClientProcessedTime      { get; set; }
    new DateTime ClientPublishedTime      { get; set; }
    new DateTime AdapterReceivedTime      { get; set; }
    new DateTime AdapterSentTime          { get; set; }
    new DateTime DownstreamTime           { get; set; }
    new uint     SourceSequenceNumber     { get; set; }
    new uint     AdapterSequenceNumber    { get; set; }
    new uint     ClientSequenceNumber     { get; set; }
    new uint     FeedSequenceNumber       { get; set; }

    new FeedSyncStatus                      FeedSyncStatus               { get; set; }
    new FeedConnectivityStatusFlags         FeedMarketConnectivityStatus { get; set; }
    new FeedEventUpdateFlags                FeedEventUpdateFlags         { get; set; }
    new IMutableCandle?                     ConflationSummaryCandle      { get; set; }
    new IMutablePublishedMarketEvents?      MarketEvents                 { get; set; }
    new IMutablePublishedCandles?           PublishedCandles             { get; set; }
    new IMutablePublishedIndicators?        PublishedIndicators          { get; set; }
    new IMutableRecentlyTradedHistory?      RecentTradedHistory          { get; set; }
    new IMutablePublishedInternalOrders?    PublishedInternalOrders      { get; set; }
    new IMutablePublishedAccounts?          PublishedAccounts            { get; set; }
    new IMutablePublishedLimits?            PublishedLimits              { get; set; }
    new IMutableLimitBreaches?              LimitBreaches                { get; set; }
    new IMutableMarginDetails?              MarginDetails                { get; set; }
    new IMutablePnLConversions?             TickerPnLConversionRate      { get; set; }
    new IMutableAdditionalTrackingFields?   AdditionalTrackingFields     { get; set; }
    new IMutableTickerRegionInfo?           TickerRegionInfo             { get; set; }
    new IMutablePublishedSignals?           PublishedSignals             { get; set; }
    new IMutablePublishedStrategyDecisions? PublishedStrategyDecisions   { get; set; }
    new IMutableAdapterExecutionStatistics? AdapterExecutionStatistics   { get; set; }

    new IMutablePublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; set; }
}

public interface IPricedFeedEventUpdate<out T> where T : ILevel1Quote
{
    T  Quote                        { get; }
    T? ContinuousPriceAdjustedQuote { get; }
}
