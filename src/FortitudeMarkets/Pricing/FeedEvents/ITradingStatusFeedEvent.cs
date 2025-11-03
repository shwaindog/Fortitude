// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Accounts;
using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface ITradingStatusFeedEvent : IReusableObject<ITradingStatusFeedEvent>, IFeedEventStatusUpdate
  , IDoublyLinkedListNode<ITradingStatusFeedEvent>, ICanHaveSourceTickerDefinition
{
    DateTime LastSourceFeedUpdateTime { get; }
    DateTime DownstreamTime           { get; }
    uint     SourceSequenceNumber     { get; }
    uint     AdapterSequenceNumber    { get; }
    uint     ClientSequenceNumber     { get; }
    uint     FeedSequenceNumber       { get; }

    FeedEventUpdateFlags         EventUpdateFlags           { get; }
    IMarketNewsPanel?            MarketNewsPanel            { get; }
    IMarketCalendarPanel?        MarketCalendarPanel        { get; }
    IMarketTradingStatusPanel?   MarketTradingStatusPanel   { get; }
    IRecentlyTradedHistory?      RecentTradedHistory        { get; }
    IPublishedInternalOrders?    PublishedInternalOrders    { get; }
    IPublishedAccounts?          PublishedAccounts          { get; }
    IPublishedLimits?            PublishedLimits            { get; }
    ILimitBreaches?              LimitBreaches              { get; }
    IMarginDetails?              MarginDetails              { get; }
    IPnLConversions?             TickerPnLConversionRate    { get; }
    ITickerRegionInfo?           TickerRegionInfo           { get; }
    IAdapterExecutionStatistics? AdapterExecutionStatistics { get; }

    new ITradingStatusFeedEvent Clone();
}

public interface IMutableTradingStatusFeedEvent : ITradingStatusFeedEvent, IMutableFeedEventStatusUpdate
  , ITransferState<IMutableTradingStatusFeedEvent>, IMutableCanHaveSourceTickerDefinition
{
    new DateTime LastSourceFeedUpdateTime   { get; set; }
    new DateTime InboundSocketReceivingTime { get; set; }
    new DateTime InboundProcessedTime       { get; set; }
    new DateTime SubscriberDispatchedTime   { get; set; }
    new DateTime AdapterReceivedTime        { get; set; }
    new DateTime AdapterSentTime            { get; set; }
    new DateTime DownstreamTime             { get; set; }
    new uint     SourceSequenceNumber       { get; set; }
    new uint     AdapterSequenceNumber      { get; set; }
    new uint     ClientSequenceNumber       { get; set; }
    new uint     FeedSequenceNumber         { get; set; }

    new FeedEventUpdateFlags                EventUpdateFlags           { get; set; }
    new IMutableMarketNewsPanel?            MarketNewsPanel            { get; set; }
    new IMutableMarketCalendarPanel?        MarketCalendarPanel        { get; set; }
    new IMutableMarketTradingStatusPanel?   MarketTradingStatusPanel   { get; set; }
    new IMutableRecentlyTradedHistory?      RecentTradedHistory        { get; set; }
    new IMutablePublishedInternalOrders?    PublishedInternalOrders    { get; set; }
    new IMutablePublishedAccounts?          PublishedAccounts          { get; set; }
    new IMutablePublishedLimits?            PublishedLimits            { get; set; }
    new IMutableLimitBreaches?              LimitBreaches              { get; set; }
    new IMutableMarginDetails?              MarginDetails              { get; set; }
    new IMutablePnLConversions?             TickerPnLConversionRate    { get; set; }
    new IMutableTickerRegionInfo?           TickerRegionInfo           { get; set; }
    new IMutableAdapterExecutionStatistics? AdapterExecutionStatistics { get; set; }

    new IMutableTradingStatusFeedEvent Clone();
}
