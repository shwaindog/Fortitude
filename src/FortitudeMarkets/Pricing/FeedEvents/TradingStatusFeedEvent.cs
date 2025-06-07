using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
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

public class TradingStatusFeedEvent : FeedEventStatusUpdate, IMutableTradingStatusFeedEvent
{
    public TradingStatusFeedEvent()
    {
        SourceTickerInfo = new SourceTickerInfo();
    }

    public TradingStatusFeedEvent(ISourceTickerInfo sourceTickerInfo)
    {
        SourceTickerInfo = sourceTickerInfo;
    }

    public TradingStatusFeedEvent(ITradingStatusFeedEvent toClone) : base(toClone)
    {
        SourceTickerInfo = toClone.SourceTickerInfo is SourceTickerInfo
            ? toClone.SourceTickerInfo.Clone()
            : toClone.SourceTickerInfo != null
                ? new SourceTickerInfo(toClone.SourceTickerInfo)
                : new SourceTickerInfo();

        LastSourceFeedUpdateTime = toClone.LastSourceFeedUpdateTime;
        ClientReceivedTime       = toClone.ClientReceivedTime;
        InboundProcessedTime     = toClone.InboundProcessedTime;
        SubscriberDispatchedTime = toClone.SubscriberDispatchedTime;
        AdapterReceivedTime      = toClone.AdapterReceivedTime;
        AdapterSentTime          = toClone.AdapterSentTime;
        DownstreamTime           = toClone.DownstreamTime;
        SourceSequenceNumber     = toClone.SourceSequenceNumber;
        AdapterSequenceNumber    = toClone.AdapterSequenceNumber;
        ClientSequenceNumber     = toClone.ClientSequenceNumber;
        FeedSequenceNumber       = toClone.FeedSequenceNumber;

        FeedMarketConnectivityStatus = toClone.FeedMarketConnectivityStatus;
        EventUpdateFlags             = toClone.EventUpdateFlags;

        var mutableClone = (IMutableTradingStatusFeedEvent?)toClone;

        MarketNewsPanel            = mutableClone?.MarketNewsPanel;
        MarketCalendarPanel        = mutableClone?.MarketCalendarPanel;
        MarketTradingStatusPanel   = mutableClone?.MarketTradingStatusPanel;
        RecentTradedHistory        = mutableClone?.RecentTradedHistory;
        PublishedInternalOrders    = mutableClone?.PublishedInternalOrders;
        PublishedAccounts          = mutableClone?.PublishedAccounts;
        PublishedLimits            = mutableClone?.PublishedLimits;
        LimitBreaches              = mutableClone?.LimitBreaches;
        MarginDetails              = mutableClone?.MarginDetails;
        TickerPnLConversionRate    = mutableClone?.TickerPnLConversionRate;
        TickerRegionInfo           = mutableClone?.TickerRegionInfo;
        AdapterExecutionStatistics = mutableClone?.AdapterExecutionStatistics;
    }

    public override DateTime SourceTime { get; set; }

    public ISourceTickerInfo? SourceTickerInfo { get; set; }

    public DateTime LastSourceFeedUpdateTime { get; set; }
    public DateTime DownstreamTime           { get; set; }

    public uint SourceSequenceNumber  { get; set; }
    public uint AdapterSequenceNumber { get; set; }
    public uint ClientSequenceNumber  { get; set; }
    public uint FeedSequenceNumber    { get; set; }

    public FeedEventUpdateFlags EventUpdateFlags { get; set; } = FeedEventUpdateFlags.NoDataYetReceived;

    public IMutableMarketNewsPanel?            MarketNewsPanel            { get; set; }
    public IMutableMarketCalendarPanel?        MarketCalendarPanel        { get; set; }
    public IMutableMarketTradingStatusPanel?   MarketTradingStatusPanel   { get; set; }
    public IMutableRecentlyTradedHistory?      RecentTradedHistory        { get; set; }
    public IMutablePublishedInternalOrders?    PublishedInternalOrders    { get; set; }
    public IMutablePublishedAccounts?          PublishedAccounts          { get; set; }
    public IMutablePublishedLimits?            PublishedLimits            { get; set; }
    public IMutableLimitBreaches?              LimitBreaches              { get; set; }
    public IMutableMarginDetails?              MarginDetails              { get; set; }
    public IMutablePnLConversions?             TickerPnLConversionRate    { get; set; }
    public IMutableTickerRegionInfo?           TickerRegionInfo           { get; set; }
    public IMutableAdapterExecutionStatistics? AdapterExecutionStatistics { get; set; }

    IPublishedAccounts? ITradingStatusFeedEvent.PublishedAccounts       => PublishedAccounts;
    IPublishedLimits? ITradingStatusFeedEvent.  PublishedLimits         => PublishedLimits;
    ILimitBreaches? ITradingStatusFeedEvent.    LimitBreaches           => LimitBreaches;
    IMarginDetails? ITradingStatusFeedEvent.    MarginDetails           => MarginDetails;
    IPnLConversions? ITradingStatusFeedEvent.   TickerPnLConversionRate => TickerPnLConversionRate;
    ITickerRegionInfo? ITradingStatusFeedEvent. TickerRegionInfo        => TickerRegionInfo;

    IMarketNewsPanel? ITradingStatusFeedEvent.MarketNewsPanel => MarketNewsPanel;

    IMarketCalendarPanel? ITradingStatusFeedEvent.MarketCalendarPanel => MarketCalendarPanel;

    IMarketTradingStatusPanel? ITradingStatusFeedEvent.MarketTradingStatusPanel => MarketTradingStatusPanel;

    IRecentlyTradedHistory? ITradingStatusFeedEvent.RecentTradedHistory => RecentTradedHistory;

    IPublishedInternalOrders? ITradingStatusFeedEvent.PublishedInternalOrders => PublishedInternalOrders;

    IAdapterExecutionStatistics? ITradingStatusFeedEvent.AdapterExecutionStatistics => AdapterExecutionStatistics;

    public ITradingStatusFeedEvent? Previous { get; set; }

    public ITradingStatusFeedEvent? Next { get; set; }

    ITradingStatusFeedEvent ICloneable<ITradingStatusFeedEvent>.Clone() => Clone();

    ITradingStatusFeedEvent ITradingStatusFeedEvent.Clone() => Clone();

    IMutableTradingStatusFeedEvent IMutableTradingStatusFeedEvent.Clone() => Clone();

    public override TradingStatusFeedEvent Clone() =>
        Recycler?.Borrow<TradingStatusFeedEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new TradingStatusFeedEvent(this);

    public override TradingStatusFeedEvent CopyFrom
        (IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is not ITradingStatusFeedEvent l1FeedEvent)
        {
            throw new ArgumentException("Only expected to copy another ITradingStatusFeedEvent");
        }
        CopyFrom(l1FeedEvent, copyMergeFlags);
        return this;
    }

    IReusableObject<ITradingStatusFeedEvent> ITransferState<IReusableObject<ITradingStatusFeedEvent>>.CopyFrom
        (IReusableObject<ITradingStatusFeedEvent> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITradingStatusFeedEvent)source, copyMergeFlags);

    ITradingStatusFeedEvent ITransferState<ITradingStatusFeedEvent>.CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableTradingStatusFeedEvent ITransferState<IMutableTradingStatusFeedEvent>.CopyFrom
        (IMutableTradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public virtual TradingStatusFeedEvent CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        SourceTickerInfo = source.SourceTickerInfo is SourceTickerInfo
            ? source.SourceTickerInfo.Clone()
            : source.SourceTickerInfo != null
                ? new SourceTickerInfo(source.SourceTickerInfo)
                : new SourceTickerInfo();

        LastSourceFeedUpdateTime = source.LastSourceFeedUpdateTime;
        InboundProcessedTime     = source.InboundProcessedTime;
        SubscriberDispatchedTime = source.SubscriberDispatchedTime;
        AdapterReceivedTime      = source.AdapterReceivedTime;
        AdapterSentTime          = source.AdapterSentTime;
        DownstreamTime           = source.DownstreamTime;
        SourceSequenceNumber     = source.SourceSequenceNumber;
        AdapterSequenceNumber    = source.AdapterSequenceNumber;
        ClientSequenceNumber     = source.ClientSequenceNumber;
        FeedSequenceNumber       = source.FeedSequenceNumber;

        FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;
        EventUpdateFlags             = source.EventUpdateFlags;

        var mutableSource = (IMutableLevel1FeedEvent)source;

        MarketNewsPanel = mutableSource.MarketNewsPanel != null
            ? (MarketNewsPanel?.CopyFrom(mutableSource.MarketNewsPanel, copyMergeFlags) ?? mutableSource.MarketNewsPanel?.Clone()) as
            IMutableMarketNewsPanel
            : MarketNewsPanel?.ResetWithTracking();
        MarketCalendarPanel = mutableSource.MarketCalendarPanel != null
            ? (MarketCalendarPanel?.CopyFrom(mutableSource.MarketCalendarPanel, copyMergeFlags) ?? mutableSource.MarketCalendarPanel?.Clone()) as
            IMutableMarketCalendarPanel
            : MarketCalendarPanel?.ResetWithTracking();
        MarketTradingStatusPanel = mutableSource.MarketTradingStatusPanel != null
            ? (MarketTradingStatusPanel?.CopyFrom(mutableSource.MarketTradingStatusPanel, copyMergeFlags) ??
               mutableSource.MarketTradingStatusPanel?.Clone()) as
            IMutableMarketTradingStatusPanel
            : MarketTradingStatusPanel?.ResetWithTracking();
        RecentTradedHistory = mutableSource.RecentTradedHistory != null
            ? (RecentTradedHistory?.CopyFrom(mutableSource.RecentTradedHistory, copyMergeFlags) ?? mutableSource.RecentTradedHistory?.Clone()) as
            IMutableRecentlyTradedHistory
            : RecentTradedHistory?.ResetWithTracking();
        PublishedInternalOrders = mutableSource.PublishedInternalOrders != null
            ? (PublishedInternalOrders?.CopyFrom(mutableSource.PublishedInternalOrders, copyMergeFlags) ??
               mutableSource.PublishedInternalOrders?.Clone()) as IMutablePublishedInternalOrders
            : PublishedInternalOrders?.ResetWithTracking();
        PublishedAccounts = mutableSource.PublishedAccounts != null
            ? (PublishedAccounts?.CopyFrom(mutableSource.PublishedAccounts, copyMergeFlags) ?? mutableSource.PublishedAccounts?.Clone()) as
            IMutablePublishedAccounts
            : PublishedAccounts?.ResetWithTracking();
        PublishedLimits = mutableSource.PublishedLimits != null
            ? (PublishedLimits?.CopyFrom(mutableSource.PublishedLimits, copyMergeFlags) ?? mutableSource.PublishedLimits?.Clone()) as
            IMutablePublishedLimits
            : PublishedLimits?.ResetWithTracking();
        LimitBreaches = mutableSource.LimitBreaches != null
            ? (LimitBreaches?.CopyFrom(mutableSource.LimitBreaches, copyMergeFlags) ?? mutableSource.LimitBreaches?.Clone()) as
            IMutableLimitBreaches
            : LimitBreaches?.ResetWithTracking();
        MarginDetails = mutableSource.MarginDetails != null
            ? (MarginDetails?.CopyFrom(mutableSource.MarginDetails, copyMergeFlags) ?? mutableSource.MarginDetails?.Clone()) as
            IMutableMarginDetails
            : MarginDetails?.ResetWithTracking();
        TickerPnLConversionRate = mutableSource.TickerPnLConversionRate != null
            ? (TickerPnLConversionRate?.CopyFrom(mutableSource.TickerPnLConversionRate, copyMergeFlags) ??
               mutableSource.TickerPnLConversionRate?.Clone()) as IMutablePnLConversions
            : TickerPnLConversionRate;
        TickerRegionInfo = mutableSource.TickerRegionInfo != null
            ? (TickerRegionInfo?.CopyFrom(mutableSource.TickerRegionInfo, copyMergeFlags) ?? mutableSource.TickerRegionInfo?.Clone()) as
            IMutableTickerRegionInfo
            : TickerRegionInfo?.ResetWithTracking();
        AdapterExecutionStatistics = mutableSource.AdapterExecutionStatistics != null
            ? (AdapterExecutionStatistics?.CopyFrom(mutableSource.AdapterExecutionStatistics, copyMergeFlags) ??
               mutableSource.AdapterExecutionStatistics?.Clone()) as IMutableAdapterExecutionStatistics
            : AdapterExecutionStatistics?.ResetWithTracking();

        return this;
    }

    public override void StateReset()
    {
        SourceTickerInfo?.StateReset();
        LastSourceFeedUpdateTime = DateTime.MinValue;
        InboundProcessedTime     = DateTime.MinValue;
        SubscriberDispatchedTime = DateTime.MinValue;
        AdapterReceivedTime      = DateTime.MinValue;
        AdapterSentTime          = DateTime.MinValue;
        DownstreamTime           = DateTime.MinValue;
        SourceSequenceNumber     = 0;
        AdapterSequenceNumber    = 0;
        ClientSequenceNumber     = 0;
        FeedSequenceNumber       = 0;

        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AwaitingConnectionStart;
        EventUpdateFlags             = FeedEventUpdateFlags.NoDataYetReceived;
        MarketNewsPanel?.StateReset();
        MarketCalendarPanel?.StateReset();
        MarketTradingStatusPanel?.StateReset();
        RecentTradedHistory?.StateReset();
        PublishedInternalOrders?.StateReset();
        PublishedAccounts?.StateReset();
        PublishedLimits?.StateReset();
        LimitBreaches?.StateReset();
        MarginDetails?.StateReset();
        TickerPnLConversionRate?.StateReset();
        TickerRegionInfo?.StateReset();
        AdapterExecutionStatistics?.StateReset();

        base.StateReset();
    }


    bool IInterfacesComparable<IFeedEventStatusUpdate>.AreEquivalent
        (IFeedEventStatusUpdate? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    public virtual bool AreEquivalent(ITradingStatusFeedEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        if (other is not IMutableLevel1FeedEvent mutOther) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var srcTkrInfoSame = SourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes) ?? other.SourceTickerInfo is null;

        var sourceFeedUpdateSame   = LastSourceFeedUpdateTime == other.LastSourceFeedUpdateTime;
        var clientProcTimeSame     = InboundProcessedTime == other.InboundProcessedTime;
        var clientPubTimeSame      = SubscriberDispatchedTime == other.SubscriberDispatchedTime;
        var adapterRecTimeSame     = AdapterReceivedTime == other.AdapterReceivedTime;
        var adapterSentTimeSame    = AdapterSentTime == other.AdapterSentTime;
        var downStreamTimeSameSame = DownstreamTime == other.DownstreamTime;
        var srcSeqNumSame          = SourceSequenceNumber == other.SourceSequenceNumber;
        var adapterSeqNumSame      = AdapterSequenceNumber == other.AdapterSequenceNumber;
        var clientSeqNumSame       = ClientSequenceNumber == other.ClientSequenceNumber;
        var feedSeqNumSame         = FeedSequenceNumber == other.FeedSequenceNumber;

        var mktNewsSame     = MarketNewsPanel?.AreEquivalent(mutOther.MarketNewsPanel, exactTypes) ?? mutOther.MarketNewsPanel is null;
        var mktCalendarSame = MarketCalendarPanel?.AreEquivalent(mutOther.MarketCalendarPanel, exactTypes) ?? mutOther.MarketCalendarPanel is null;
        var mktTradingSame = MarketTradingStatusPanel?.AreEquivalent(mutOther.MarketTradingStatusPanel, exactTypes) ??
                             mutOther.MarketTradingStatusPanel is null;
        var trdHistorySame = RecentTradedHistory?.AreEquivalent(mutOther.RecentTradedHistory, exactTypes) ?? mutOther.RecentTradedHistory is null;
        var pubIntOrdersSame = PublishedInternalOrders?.AreEquivalent(mutOther.PublishedInternalOrders, exactTypes) ??
                               mutOther.PublishedInternalOrders is null;
        var accountsSame    = PublishedAccounts?.AreEquivalent(mutOther.PublishedAccounts, exactTypes) ?? mutOther.PublishedAccounts is null;
        var pubLimitsSame   = PublishedLimits?.AreEquivalent(mutOther.PublishedLimits, exactTypes) ?? mutOther.PublishedLimits is null;
        var lmtBreachesSame = LimitBreaches?.AreEquivalent(mutOther.LimitBreaches, exactTypes) ?? mutOther.LimitBreaches is null;
        var marginsSame     = MarginDetails?.AreEquivalent(mutOther.MarginDetails, exactTypes) ?? mutOther.MarginDetails is null;
        var pnlConRateSame = TickerPnLConversionRate?.AreEquivalent(mutOther.TickerPnLConversionRate, exactTypes) ??
                             mutOther.TickerPnLConversionRate is null;
        var tkrRegInfoSame = TickerRegionInfo?.AreEquivalent(mutOther.TickerRegionInfo, exactTypes) ?? mutOther.TickerRegionInfo is null;
        var adapterExeStatsSame = AdapterExecutionStatistics?.AreEquivalent(mutOther.AdapterExecutionStatistics, exactTypes) ??
                                  mutOther.AdapterExecutionStatistics is null;


        var allAreSame = srcTkrInfoSame && sourceFeedUpdateSame && clientProcTimeSame
                      && clientPubTimeSame && adapterRecTimeSame && adapterSentTimeSame && downStreamTimeSameSame && srcSeqNumSame
                      && adapterSeqNumSame && clientSeqNumSame && feedSeqNumSame && mktNewsSame && mktCalendarSame && mktTradingSame
                      && trdHistorySame && pubIntOrdersSame && accountsSame && pubLimitsSame && lmtBreachesSame && marginsSame
                      && pnlConRateSame && tkrRegInfoSame && adapterExeStatsSame && baseSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel1FeedEvent, true);


    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(SourceTickerInfo);
        hashCode.Add(LastSourceFeedUpdateTime);
        hashCode.Add(ClientReceivedTime);
        hashCode.Add(InboundProcessedTime);
        hashCode.Add(SubscriberDispatchedTime);
        hashCode.Add(AdapterReceivedTime);
        hashCode.Add(AdapterSentTime);
        hashCode.Add(DownstreamTime);
        hashCode.Add(SourceSequenceNumber);
        hashCode.Add(AdapterSequenceNumber);
        hashCode.Add(ClientSequenceNumber);
        hashCode.Add(FeedSequenceNumber);
        hashCode.Add((int)FeedMarketConnectivityStatus);
        hashCode.Add((int)EventUpdateFlags);
        hashCode.Add(MarketNewsPanel);
        hashCode.Add(MarketCalendarPanel);
        hashCode.Add(MarketTradingStatusPanel);
        hashCode.Add(RecentTradedHistory);
        hashCode.Add(PublishedInternalOrders);
        hashCode.Add(PublishedAccounts);
        hashCode.Add(PublishedLimits);
        hashCode.Add(LimitBreaches);
        hashCode.Add(MarginDetails);
        hashCode.Add(TickerPnLConversionRate);
        hashCode.Add(TickerRegionInfo);
        hashCode.Add(AdapterExecutionStatistics);
        return hashCode.ToHashCode();
    }
}
