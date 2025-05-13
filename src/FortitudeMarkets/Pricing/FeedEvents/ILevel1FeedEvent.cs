using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
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
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface ILevel1FeedEvent : IReusableObject<ILevel1FeedEvent>, IPricedFeedEventUpdate<ILevel1Quote>,
    INonPricingFeedEvent, IInterfacesComparable<ILevel1FeedEvent> { }

public interface IMutableLevel1FeedEvent : ILevel1FeedEvent, IPricedFeedEventUpdate<IMutableLevel1Quote>, IMutableNonPricingFeedEvent
{
    new IMutableLevel1Quote Quote { get; set; }

    new IMutableLevel1Quote? ContinuousPriceAdjustedQuote { get; }
}

public class Level1FeedEvent : ReusableObject<ILevel1FeedEvent>, IMutableLevel1FeedEvent
{
    public Level1FeedEvent()
    {
        SourceTickerInfo = new SourceTickerInfo();

        Quote = new Level1PriceQuote();
    }


    public Level1FeedEvent(ISourceTickerInfo sourceTickerInfo, IMutableLevel1Quote? initialQuote = null)
    {
        SourceTickerInfo = sourceTickerInfo;

        Quote = initialQuote ?? new Level1PriceQuote();
    }

    public Level1FeedEvent(ILevel1FeedEvent toClone)
    {
        SourceTickerInfo = toClone.SourceTickerInfo is SourceTickerInfo
            ? toClone.SourceTickerInfo.Clone()
            : new SourceTickerInfo(toClone.SourceTickerInfo);
        Quote = CloneQuoteForFeedLevel(toClone.Quote);

        IsReplay                 = toClone.IsReplay;
        LastSourceFeedUpdateTime = toClone.LastSourceFeedUpdateTime;
        ClientReceivedTime       = toClone.ClientReceivedTime;
        ClientProcessedTime      = toClone.ClientProcessedTime;
        ClientPublishedTime      = toClone.ClientPublishedTime;
        AdapterReceivedTime      = toClone.AdapterReceivedTime;
        AdapterSentTime          = toClone.AdapterSentTime;
        DownstreamTime           = toClone.DownstreamTime;
        SourceSequenceNumber     = toClone.SourceSequenceNumber;
        AdapterSequenceNumber    = toClone.AdapterSequenceNumber;
        ClientSequenceNumber     = toClone.ClientSequenceNumber;
        FeedSequenceNumber       = toClone.FeedSequenceNumber;

        FeedSyncStatus               = toClone.FeedSyncStatus;
        FeedMarketConnectivityStatus = toClone.FeedMarketConnectivityStatus;
        FeedEventUpdateFlags         = toClone.FeedEventUpdateFlags;

        var mutableClone = (IMutableLevel1FeedEvent?)toClone;

        ConflationSummaryCandle    = mutableClone?.ConflationSummaryCandle;
        MarketEvents               = mutableClone?.MarketEvents;
        PublishedCandles           = mutableClone?.PublishedCandles;
        PublishedIndicators        = mutableClone?.PublishedIndicators;
        RecentTradedHistory        = mutableClone?.RecentTradedHistory;
        PublishedInternalOrders    = mutableClone?.PublishedInternalOrders;
        PublishedAccounts          = mutableClone?.PublishedAccounts;
        PublishedLimits            = mutableClone?.PublishedLimits;
        LimitBreaches              = mutableClone?.LimitBreaches;
        MarginDetails              = mutableClone?.MarginDetails;
        TickerPnLConversionRate    = mutableClone?.TickerPnLConversionRate;
        AdditionalTrackingFields   = mutableClone?.AdditionalTrackingFields;
        TickerRegionInfo           = mutableClone?.TickerRegionInfo;
        PublishedSignals           = mutableClone?.PublishedSignals;
        PublishedStrategyDecisions = mutableClone?.PublishedStrategyDecisions;
        AdapterExecutionStatistics = mutableClone?.AdapterExecutionStatistics;

        ContinuousPriceAdjustments = mutableClone?.ContinuousPriceAdjustments;
    }


    protected virtual IMutableLevel1Quote CloneQuoteForFeedLevel(ILevel1Quote toClone)
    {
        return toClone is Level1PriceQuote ? (IMutableLevel1Quote)toClone.Clone() : new PQLevel1Quote(toClone);
    }

    public ISourceTickerInfo   SourceTickerInfo { get; set; }
    public IMutableLevel1Quote Quote            { get; set; }

    public DateTime LastSourceFeedUpdateTime { get; set; }

    public DateTime ClientReceivedTime  { get; set; }
    public DateTime ClientProcessedTime { get; set; }
    public DateTime ClientPublishedTime { get; set; }
    public DateTime AdapterReceivedTime { get; set; }
    public DateTime AdapterSentTime     { get; set; }
    public DateTime DownstreamTime      { get; set; }

    public bool IsReplay              { get; set; }
    public uint SourceSequenceNumber  { get; set; }
    public uint AdapterSequenceNumber { get; set; }
    public uint ClientSequenceNumber  { get; set; }
    public uint FeedSequenceNumber    { get; set; }

    public FeedSyncStatus              FeedSyncStatus               { get; set; }
    public FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; set; } = FeedConnectivityStatusFlags.AwaitClientConnectionStart;
    public FeedEventUpdateFlags        FeedEventUpdateFlags         { get; set; } = FeedEventUpdateFlags.NoDataYetReceived;

    public IMutableCandle?                     ConflationSummaryCandle    { get; set; }
    public IMutablePublishedMarketEvents?      MarketEvents               { get; set; }
    public IMutablePublishedCandles?           PublishedCandles           { get; set; }
    public IMutablePublishedIndicators?        PublishedIndicators        { get; set; }
    public IMutableRecentlyTradedHistory?      RecentTradedHistory        { get; set; }
    public IMutablePublishedInternalOrders?    PublishedInternalOrders    { get; set; }
    public IMutablePublishedAccounts?          PublishedAccounts          { get; set; }
    public IMutablePublishedLimits?            PublishedLimits            { get; set; }
    public IMutableLimitBreaches?              LimitBreaches              { get; set; }
    public IMutableMarginDetails?              MarginDetails              { get; set; }
    public IMutablePnLConversions?             TickerPnLConversionRate    { get; set; }
    public IMutableAdditionalTrackingFields?   AdditionalTrackingFields   { get; set; }
    public IMutableTickerRegionInfo?           TickerRegionInfo           { get; set; }
    public IMutablePublishedSignals?           PublishedSignals           { get; set; }
    public IMutablePublishedStrategyDecisions? PublishedStrategyDecisions { get; set; }
    public IMutableAdapterExecutionStatistics? AdapterExecutionStatistics { get; set; }

    public IMutablePublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; set; }

    public TickerQuoteDetailLevel           TickerQuoteDetailLevel  => SourceTickerInfo.PublishedTickerQuoteDetailLevel;
    ICandle? INonPricingFeedEvent.          ConflationSummaryCandle => ConflationSummaryCandle;
    ILimitBreaches? INonPricingFeedEvent.   LimitBreaches           => LimitBreaches;
    IMarginDetails? INonPricingFeedEvent.   MarginDetails           => MarginDetails;
    IPnLConversions? INonPricingFeedEvent.  TickerPnLConversionRate => TickerPnLConversionRate;
    ITickerRegionInfo? INonPricingFeedEvent.TickerRegionInfo        => TickerRegionInfo;
    IPublishedSignals? INonPricingFeedEvent.PublishedSignals        => PublishedSignals;

    IPublishedLimits? INonPricingFeedEvent.    PublishedLimits     => PublishedLimits;
    IPublishedAccounts? INonPricingFeedEvent.  PublishedAccounts   => PublishedAccounts;
    IPublishedCandles? INonPricingFeedEvent.   PublishedCandles    => PublishedCandles;
    IPublishedIndicators? INonPricingFeedEvent.PublishedIndicators => PublishedIndicators;

    IPublishedMarketEvents? INonPricingFeedEvent.   MarketEvents                 => MarketEvents;
    IMutableLevel1Quote? IMutableLevel1FeedEvent.   ContinuousPriceAdjustedQuote => Quote;
    IRecentlyTradedHistory? INonPricingFeedEvent.   RecentTradedHistory          => RecentTradedHistory;
    IPublishedInternalOrders? INonPricingFeedEvent. PublishedInternalOrders      => PublishedInternalOrders;
    IAdditionalTrackingFields? INonPricingFeedEvent.AdditionalTrackingFields     => AdditionalTrackingFields;

    IPublishedStrategyDecisions? INonPricingFeedEvent. PublishedStrategyDecisions   => PublishedStrategyDecisions;
    IAdapterExecutionStatistics? INonPricingFeedEvent. AdapterExecutionStatistics   => AdapterExecutionStatistics;
    ILevel1Quote IPricedFeedEventUpdate<ILevel1Quote>. Quote                        => Quote;
    ILevel1Quote? IPricedFeedEventUpdate<ILevel1Quote>.ContinuousPriceAdjustedQuote => Quote;

    IPublishedContinuousPriceAdjustments? INonPricingFeedEvent.      ContinuousPriceAdjustments   => ContinuousPriceAdjustments;
    IMutableLevel1Quote? IPricedFeedEventUpdate<IMutableLevel1Quote>.ContinuousPriceAdjustedQuote => Quote;

    public override ILevel1FeedEvent Clone() =>
        Recycler?.Borrow<Level1FeedEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new Level1FeedEvent(this);

    public override Level1FeedEvent CopyFrom(ILevel1FeedEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceTickerInfo = source.SourceTickerInfo is SourceTickerInfo
            ? source.SourceTickerInfo.Clone()
            : new SourceTickerInfo(source.SourceTickerInfo);
        Quote = CloneQuoteForFeedLevel(source.Quote);

        IsReplay                 = source.IsReplay;
        LastSourceFeedUpdateTime = source.LastSourceFeedUpdateTime;
        ClientReceivedTime       = source.ClientReceivedTime;
        ClientProcessedTime      = source.ClientProcessedTime;
        ClientPublishedTime      = source.ClientPublishedTime;
        AdapterReceivedTime      = source.AdapterReceivedTime;
        AdapterSentTime          = source.AdapterSentTime;
        DownstreamTime           = source.DownstreamTime;
        SourceSequenceNumber     = source.SourceSequenceNumber;
        AdapterSequenceNumber    = source.AdapterSequenceNumber;
        ClientSequenceNumber     = source.ClientSequenceNumber;
        FeedSequenceNumber       = source.FeedSequenceNumber;

        FeedSyncStatus               = source.FeedSyncStatus;
        FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;
        FeedEventUpdateFlags         = source.FeedEventUpdateFlags;

        var mutableSource = (IMutableLevel1FeedEvent)source;

        ConflationSummaryCandle    = mutableSource.ConflationSummaryCandle;
        MarketEvents               = mutableSource.MarketEvents;
        PublishedCandles           = mutableSource.PublishedCandles;
        PublishedIndicators        = mutableSource.PublishedIndicators;
        RecentTradedHistory        = mutableSource.RecentTradedHistory;
        PublishedInternalOrders    = mutableSource.PublishedInternalOrders;
        PublishedAccounts          = mutableSource.PublishedAccounts;
        PublishedLimits            = mutableSource.PublishedLimits;
        LimitBreaches              = mutableSource.LimitBreaches;
        MarginDetails              = mutableSource.MarginDetails;
        TickerPnLConversionRate    = mutableSource.TickerPnLConversionRate;
        AdditionalTrackingFields   = mutableSource.AdditionalTrackingFields;
        TickerRegionInfo           = mutableSource.TickerRegionInfo;
        PublishedSignals           = mutableSource.PublishedSignals;
        PublishedStrategyDecisions = mutableSource.PublishedStrategyDecisions;
        AdapterExecutionStatistics = mutableSource.AdapterExecutionStatistics;
        ContinuousPriceAdjustments = mutableSource.ContinuousPriceAdjustments;

        return this;
    }

    public override void StateReset()
    {
        SourceTickerInfo.StateReset();
        Quote.StateReset();
        IsReplay                 = false;
        LastSourceFeedUpdateTime = DateTime.MinValue;
        ClientReceivedTime       = DateTime.MinValue;
        ClientProcessedTime      = DateTime.MinValue;
        ClientPublishedTime      = DateTime.MinValue;
        AdapterReceivedTime      = DateTime.MinValue;
        AdapterSentTime          = DateTime.MinValue;
        DownstreamTime           = DateTime.MinValue;
        SourceSequenceNumber     = 0;
        AdapterSequenceNumber    = 0;
        ClientSequenceNumber     = 0;
        FeedSequenceNumber       = 0;

        FeedSyncStatus               = FeedSyncStatus.FeedDown;
        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AwaitClientConnectionStart;
        FeedEventUpdateFlags         = FeedEventUpdateFlags.NoDataYetReceived;
        ConflationSummaryCandle?.StateReset();
        MarketEvents?.StateReset();
        PublishedCandles?.StateReset();
        PublishedIndicators?.StateReset();
        RecentTradedHistory?.StateReset();
        PublishedInternalOrders?.StateReset();
        PublishedAccounts?.StateReset();
        PublishedLimits?.StateReset();
        LimitBreaches?.StateReset();
        MarginDetails?.StateReset();
        TickerPnLConversionRate?.StateReset();
        AdditionalTrackingFields?.StateReset();
        TickerRegionInfo?.StateReset();
        PublishedSignals?.StateReset();
        PublishedStrategyDecisions?.StateReset();
        AdapterExecutionStatistics?.StateReset();
        ContinuousPriceAdjustments?.StateReset();

        base.StateReset();
    }

    public virtual bool AreEquivalent(ILevel1FeedEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        if (other is not IMutableLevel1FeedEvent mutOther) return false;

        var srcTkrInfoSame = SourceTickerInfo.AreEquivalent(other.SourceTickerInfo, exactTypes);

        var quoteSame              = Quote.AreEquivalent(other.Quote, exactTypes);
        var replaySame             = IsReplay = other.IsReplay;
        var sourceFeedUpdateSame   = LastSourceFeedUpdateTime == other.LastSourceFeedUpdateTime;
        var clientRecTimeTimeSame  = ClientReceivedTime == other.ClientReceivedTime;
        var clientProcTimeSame     = ClientProcessedTime == other.ClientProcessedTime;
        var clientPubTimeSame      = ClientPublishedTime == other.ClientPublishedTime;
        var adapterRecTimeSame     = AdapterReceivedTime == other.AdapterReceivedTime;
        var adapterSentTimeSame    = AdapterSentTime == other.AdapterSentTime;
        var downStreamTimeSameSame = DownstreamTime == other.DownstreamTime;
        var srcSeqNumSame          = SourceSequenceNumber == other.SourceSequenceNumber;
        var adapterSeqNumSame      = AdapterSequenceNumber == other.AdapterSequenceNumber;
        var clntSeqNumSame         = ClientSequenceNumber == other.ClientSequenceNumber;
        var feedSeqNumSame         = FeedSequenceNumber == other.FeedSequenceNumber;

        var confCandleSame = ConflationSummaryCandle?.AreEquivalent(mutOther.ConflationSummaryCandle, exactTypes) ??
                             mutOther.ConflationSummaryCandle is null;
        var mktEvtsSame       = MarketEvents?.AreEquivalent(mutOther.MarketEvents, exactTypes) ?? mutOther.MarketEvents is null;
        var pubCandlesSame    = PublishedCandles?.AreEquivalent(mutOther.PublishedCandles, exactTypes) ?? mutOther.PublishedCandles is null;
        var pubIndicatorsSame = PublishedIndicators?.AreEquivalent(mutOther.PublishedIndicators, exactTypes) ?? mutOther.PublishedIndicators is null;
        var trdHistrySame     = RecentTradedHistory?.AreEquivalent(mutOther.RecentTradedHistory, exactTypes) ?? mutOther.RecentTradedHistory is null;
        var pubIntOrdersSame = PublishedInternalOrders?.AreEquivalent(mutOther.PublishedInternalOrders, exactTypes) ??
                               mutOther.PublishedInternalOrders is null;
        var pubAcctsSame    = PublishedAccounts?.AreEquivalent(mutOther.PublishedAccounts, exactTypes) ?? mutOther.PublishedAccounts is null;
        var pubLimitsSame   = PublishedLimits?.AreEquivalent(mutOther.PublishedLimits, exactTypes) ?? mutOther.PublishedLimits is null;
        var lmtBreachesSame = LimitBreaches?.AreEquivalent(mutOther.LimitBreaches, exactTypes) ?? mutOther.LimitBreaches is null;
        var mrgnDetsSame    = MarginDetails?.AreEquivalent(mutOther.MarginDetails, exactTypes) ?? mutOther.MarginDetails is null;
        var pnlConRateSame = TickerPnLConversionRate?.AreEquivalent(mutOther.TickerPnLConversionRate, exactTypes) ??
                             mutOther.TickerPnLConversionRate is null;
        var addTrkingFldsSame = AdditionalTrackingFields?.AreEquivalent(mutOther.AdditionalTrackingFields, exactTypes) ??
                                mutOther.AdditionalTrackingFields is null;
        var tkrRegInfoSame = TickerRegionInfo?.AreEquivalent(mutOther.TickerRegionInfo, exactTypes) ?? mutOther.TickerRegionInfo is null;
        var pubSignlsSame  = PublishedSignals?.AreEquivalent(mutOther.PublishedSignals, exactTypes) ?? mutOther.PublishedSignals is null;
        var pbStrDecSame = PublishedStrategyDecisions?.AreEquivalent(mutOther.PublishedStrategyDecisions, exactTypes) ??
                           mutOther.PublishedStrategyDecisions is null;
        var adptrExeStatsSame = AdapterExecutionStatistics?.AreEquivalent(mutOther.AdapterExecutionStatistics, exactTypes) ??
                                mutOther.AdapterExecutionStatistics is null;
        var conPrceAdjustSame = ContinuousPriceAdjustments?.AreEquivalent(mutOther.ContinuousPriceAdjustments, exactTypes) ??
                                mutOther.ContinuousPriceAdjustments is null;

        var allAreSame = srcTkrInfoSame && quoteSame && replaySame && sourceFeedUpdateSame && clientRecTimeTimeSame && clientProcTimeSame
                      && clientPubTimeSame && adapterRecTimeSame && adapterSentTimeSame && downStreamTimeSameSame && srcSeqNumSame &&
                         adapterSeqNumSame
                      && clntSeqNumSame && feedSeqNumSame && confCandleSame && mktEvtsSame && pubCandlesSame && pubIndicatorsSame && trdHistrySame
                      && pubIntOrdersSame && pubAcctsSame && pubLimitsSame && lmtBreachesSame && mrgnDetsSame && pnlConRateSame && addTrkingFldsSame
                      && tkrRegInfoSame && pubSignlsSame && pbStrDecSame && adptrExeStatsSame && conPrceAdjustSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1FeedEvent, true);


    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(SourceTickerInfo);
        hashCode.Add(Quote);
        hashCode.Add(LastSourceFeedUpdateTime);
        hashCode.Add(ClientReceivedTime);
        hashCode.Add(ClientProcessedTime);
        hashCode.Add(ClientPublishedTime);
        hashCode.Add(AdapterReceivedTime);
        hashCode.Add(AdapterSentTime);
        hashCode.Add(DownstreamTime);
        hashCode.Add(IsReplay);
        hashCode.Add(SourceSequenceNumber);
        hashCode.Add(AdapterSequenceNumber);
        hashCode.Add(ClientSequenceNumber);
        hashCode.Add(FeedSequenceNumber);
        hashCode.Add((int)FeedSyncStatus);
        hashCode.Add((int)FeedMarketConnectivityStatus);
        hashCode.Add((int)FeedEventUpdateFlags);
        hashCode.Add(ConflationSummaryCandle);
        hashCode.Add(MarketEvents);
        hashCode.Add(PublishedCandles);
        hashCode.Add(PublishedIndicators);
        hashCode.Add(RecentTradedHistory);
        hashCode.Add(PublishedInternalOrders);
        hashCode.Add(PublishedAccounts);
        hashCode.Add(PublishedLimits);
        hashCode.Add(LimitBreaches);
        hashCode.Add(MarginDetails);
        hashCode.Add(TickerPnLConversionRate);
        hashCode.Add(AdditionalTrackingFields);
        hashCode.Add(TickerRegionInfo);
        hashCode.Add(PublishedSignals);
        hashCode.Add(PublishedStrategyDecisions);
        hashCode.Add(AdapterExecutionStatistics);
        hashCode.Add(ContinuousPriceAdjustments);
        return hashCode.ToHashCode();
    }
}
