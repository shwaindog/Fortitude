using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.PriceAdjustments;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface ILevel1FeedEvent : IReusableObject<ILevel1FeedEvent>, IPricedQuoteFeedEventUpdate<ILevel1Quote>,
    IAncillaryPricingFeedEvent, IInterfacesComparable<ILevel1FeedEvent>
{
    new ILevel1FeedEvent Clone();
}

public interface IMutableLevel1FeedEvent : ILevel1FeedEvent, IPricedQuoteFeedEventUpdate<IMutableLevel1Quote>, IMutableAncillaryPricingFeedEvent
{
    new IMutableLevel1Quote Quote { get; set; }

    new IMutableLevel1Quote? ContinuousPriceAdjustedQuote { get; }
    
    new IMutableLevel1FeedEvent Clone();
}

public class Level1FeedEvent : AncillaryPricingFeedEvent, IMutableLevel1FeedEvent
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

    public Level1FeedEvent(ILevel1FeedEvent toClone) : base(toClone)
    {
        Quote = CloneQuoteForFeedLevel(toClone.Quote);
    }

    protected virtual IMutableLevel1Quote CloneQuoteForFeedLevel(ILevel1Quote toClone)
    {
        return toClone is Level1PriceQuote ? (IMutableLevel1Quote)toClone.Clone() : new PQLevel1Quote(toClone);
    }

    public IMutableLevel1Quote Quote { get; set; }

    ILevel1Quote IPricedQuoteFeedEventUpdate<ILevel1Quote>.Quote => Quote;

    IMutableLevel1Quote IMutableLevel1FeedEvent.ContinuousPriceAdjustedQuote => Quote;

    ILevel1Quote IPricedQuoteFeedEventUpdate<ILevel1Quote>.ContinuousPriceAdjustedQuote => Quote;

    IPublishedContinuousPriceAdjustments? IAncillaryPricingFeedEvent.ContinuousPriceAdjustments => ContinuousPriceAdjustments;

    IMutableLevel1Quote IPricedQuoteFeedEventUpdate<IMutableLevel1Quote>.ContinuousPriceAdjustedQuote => Quote;

    ILevel1FeedEvent ICloneable<ILevel1FeedEvent>.Clone() => Clone();

    ILevel1FeedEvent ILevel1FeedEvent.              Clone() => Clone();

    IMutableLevel1FeedEvent IMutableLevel1FeedEvent.Clone() => Clone();

    public override Level1FeedEvent Clone() =>
        Recycler?.Borrow<Level1FeedEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new Level1FeedEvent(this);

    IReusableObject<ILevel1FeedEvent> ITransferState<IReusableObject<ILevel1FeedEvent>>.CopyFrom
        (IReusableObject<ILevel1FeedEvent> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITradingStatusFeedEvent)source, copyMergeFlags);

    ILevel1FeedEvent ITransferState<ILevel1FeedEvent>.CopyFrom
        (ILevel1FeedEvent source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override Level1FeedEvent CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ILevel1FeedEvent l1FeedEvt)
        {
            Quote = CloneQuoteForFeedLevel(l1FeedEvt.Quote);
        }
        return this;
    }

    public override void StateReset()
    {
        Quote.StateReset();

        base.StateReset();
    }

    public virtual bool AreEquivalent(ILevel1FeedEvent? other, bool exactTypes = false)
    {
        if (other is not IMutableLevel1FeedEvent) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var quoteSame = Quote.AreEquivalent(other.Quote, exactTypes);

        var allAreSame = baseSame && quoteSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1FeedEvent, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(Quote);
        return hashCode.ToHashCode();
    }
}
