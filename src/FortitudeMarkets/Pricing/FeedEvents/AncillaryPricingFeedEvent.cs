using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Indicators;
using FortitudeMarkets.Pricing.FeedEvents.PriceAdjustments;
using FortitudeMarkets.Pricing.FeedEvents.Signals;
using FortitudeMarkets.Pricing.FeedEvents.Strategies;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.TrackingTimes;

namespace FortitudeMarkets.Pricing.FeedEvents;

public class AncillaryPricingFeedEvent : TradingStatusFeedEvent, IMutableAncillaryPricingFeedEvent
{
    public AncillaryPricingFeedEvent() { }


    public AncillaryPricingFeedEvent(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }

    public AncillaryPricingFeedEvent(IAncillaryPricingFeedEvent toClone) : base(toClone)
    {
        ConflationSummaryCandle    = toClone.ConflationSummaryCandle?.Clone() as IMutableCandle;
        PublishedCandles           = toClone.PublishedCandles?.Clone() as IMutablePublishedCandles;
        PublishedIndicators        = toClone.PublishedIndicators?.Clone() as IMutablePublishedIndicators;
        AdditionalTrackingFields   = toClone.AdditionalTrackingFields?.Clone() as IMutableAdditionalTrackingFields;
        PublishedSignals           = toClone.PublishedSignals?.Clone() as IMutablePublishedSignals;
        PublishedStrategyDecisions = toClone.PublishedStrategyDecisions?.Clone() as IMutablePublishedStrategyDecisions;

        ContinuousPriceAdjustments = toClone.ContinuousPriceAdjustments?.Clone() as IMutablePublishedContinuousPriceAdjustments;
    }

    public IMutableCandle?                     ConflationSummaryCandle    { get; set; }
    public IMutablePublishedCandles?           PublishedCandles           { get; set; }
    public IMutablePublishedIndicators?        PublishedIndicators        { get; set; }
    public IMutableAdditionalTrackingFields?   AdditionalTrackingFields   { get; set; }
    public IMutablePublishedSignals?           PublishedSignals           { get; set; }
    public IMutablePublishedStrategyDecisions? PublishedStrategyDecisions { get; set; }

    public IMutablePublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; set; }

    public TickerQuoteDetailLevel                 TickerQuoteDetailLevel  => SourceTickerInfo.PublishedTickerQuoteDetailLevel;
    ICandle? IAncillaryPricingFeedEvent.          ConflationSummaryCandle => ConflationSummaryCandle;
    IPublishedSignals? IAncillaryPricingFeedEvent.PublishedSignals        => PublishedSignals;

    IPublishedCandles? IAncillaryPricingFeedEvent.   PublishedCandles    => PublishedCandles;
    IPublishedIndicators? IAncillaryPricingFeedEvent.PublishedIndicators => PublishedIndicators;

    IAdditionalTrackingFields? IAncillaryPricingFeedEvent.AdditionalTrackingFields => AdditionalTrackingFields;

    IPublishedStrategyDecisions? IAncillaryPricingFeedEvent.PublishedStrategyDecisions => PublishedStrategyDecisions;

    IPublishedContinuousPriceAdjustments? IAncillaryPricingFeedEvent.ContinuousPriceAdjustments => ContinuousPriceAdjustments;

    public override AncillaryPricingFeedEvent Clone() =>
        Recycler?.Borrow<AncillaryPricingFeedEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new AncillaryPricingFeedEvent(this);


    public override AncillaryPricingFeedEvent CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IAncillaryPricingFeedEvent mutableSource)
        {
            ConflationSummaryCandle = mutableSource.ConflationSummaryCandle != null
                ? (ConflationSummaryCandle?.CopyFrom(mutableSource.ConflationSummaryCandle, copyMergeFlags) ??
                   mutableSource.ConflationSummaryCandle?.Clone()) as IMutableCandle
                : ConflationSummaryCandle?.ResetWithTracking();
            PublishedCandles = mutableSource.PublishedCandles != null
                ? (PublishedCandles?.CopyFrom(mutableSource.PublishedCandles, copyMergeFlags) ?? mutableSource.PublishedCandles?.Clone()) as
                IMutablePublishedCandles
                : PublishedCandles?.ResetWithTracking();
            PublishedIndicators = mutableSource.PublishedIndicators != null
                ? (PublishedIndicators?.CopyFrom(mutableSource.PublishedIndicators, copyMergeFlags) ??
                   mutableSource.PublishedIndicators?.Clone()) as
                IMutablePublishedIndicators
                : PublishedIndicators?.ResetWithTracking();
            AdditionalTrackingFields = mutableSource.AdditionalTrackingFields != null
                ? (AdditionalTrackingFields?.CopyFrom(mutableSource.AdditionalTrackingFields, copyMergeFlags) ??
                   mutableSource.AdditionalTrackingFields?.Clone()) as IMutableAdditionalTrackingFields
                : AdditionalTrackingFields?.ResetWithTracking();
            PublishedSignals = mutableSource.PublishedSignals != null
                ? (PublishedSignals?.CopyFrom(mutableSource.PublishedSignals, copyMergeFlags) ?? mutableSource.PublishedSignals?.Clone()) as
                IMutablePublishedSignals
                : PublishedSignals?.ResetWithTracking();
            PublishedStrategyDecisions = mutableSource.PublishedStrategyDecisions != null
                ? (PublishedStrategyDecisions?.CopyFrom(mutableSource.PublishedStrategyDecisions, copyMergeFlags) ??
                   mutableSource.PublishedStrategyDecisions?.Clone()) as IMutablePublishedStrategyDecisions
                : PublishedStrategyDecisions?.ResetWithTracking();
            AdapterExecutionStatistics = mutableSource.AdapterExecutionStatistics != null
                ? (AdapterExecutionStatistics?.CopyFrom(mutableSource.AdapterExecutionStatistics, copyMergeFlags) ??
                   mutableSource.AdapterExecutionStatistics?.Clone()) as IMutableAdapterExecutionStatistics
                : AdapterExecutionStatistics?.ResetWithTracking();
            ContinuousPriceAdjustments = mutableSource.ContinuousPriceAdjustments != null
                ? (ContinuousPriceAdjustments?.CopyFrom(mutableSource.ContinuousPriceAdjustments, copyMergeFlags) ??
                   mutableSource.ContinuousPriceAdjustments?.Clone()) as IMutablePublishedContinuousPriceAdjustments
                : ContinuousPriceAdjustments?.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        ConflationSummaryCandle?.StateReset();
        PublishedCandles?.StateReset();
        PublishedIndicators?.StateReset();
        AdditionalTrackingFields?.StateReset();
        PublishedSignals?.StateReset();
        PublishedStrategyDecisions?.StateReset();
        ContinuousPriceAdjustments?.StateReset();

        base.StateReset();
    }

    public override bool AreEquivalent(ITradingStatusFeedEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        if (other is not IMutableLevel1FeedEvent mutOther) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var confCandleSame = ConflationSummaryCandle?.AreEquivalent(mutOther.ConflationSummaryCandle, exactTypes) ??
                             mutOther.ConflationSummaryCandle is null;
        var pubCandlesSame    = PublishedCandles?.AreEquivalent(mutOther.PublishedCandles, exactTypes) ?? mutOther.PublishedCandles is null;
        var pubIndicatorsSame = PublishedIndicators?.AreEquivalent(mutOther.PublishedIndicators, exactTypes) ?? mutOther.PublishedIndicators is null;
        var addTrkingFldsSame = AdditionalTrackingFields?.AreEquivalent(mutOther.AdditionalTrackingFields, exactTypes) ??
                                mutOther.AdditionalTrackingFields is null;
        var pubSignlsSame  = PublishedSignals?.AreEquivalent(mutOther.PublishedSignals, exactTypes) ?? mutOther.PublishedSignals is null;
        var pbStrDecSame = PublishedStrategyDecisions?.AreEquivalent(mutOther.PublishedStrategyDecisions, exactTypes) ??
                           mutOther.PublishedStrategyDecisions is null;
        var conPrceAdjustSame = ContinuousPriceAdjustments?.AreEquivalent(mutOther.ContinuousPriceAdjustments, exactTypes) ??
                                mutOther.ContinuousPriceAdjustments is null;

        var allAreSame = confCandleSame && pubCandlesSame && pubIndicatorsSame && addTrkingFldsSame
                      && pubSignlsSame && pbStrDecSame && conPrceAdjustSame && baseSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1FeedEvent, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(ConflationSummaryCandle);
        hashCode.Add(PublishedCandles);
        hashCode.Add(PublishedIndicators);
        hashCode.Add(AdditionalTrackingFields);
        hashCode.Add(PublishedSignals);
        hashCode.Add(PublishedStrategyDecisions);
        hashCode.Add(ContinuousPriceAdjustments);
        return hashCode.ToHashCode();
    }
}
