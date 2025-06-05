using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Accounts;
using FortitudeMarkets.Pricing.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Limits;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.TradingConversions;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Accounts;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.AdapterExecutionDetails;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Limits;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TradingConversions;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.TimeSeries;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents;

public interface IPQTradingStatusFeedEvent : IMutableTradingStatusFeedEvent, IPQMessage, IDoublyLinkedListNode<IPQTradingStatusFeedEvent>
   , ITransferState<IPQTradingStatusFeedEvent>
{
    new IPQSourceTickerInfo? SourceTickerInfo { get; set; }

    new IPQPublishedAccounts?          PublishedAccounts          { get; set; }
    new IPQPublishedMarketEvents?      MarketEvents               { get; set; }
    new IPQRecentlyTradedHistory?      RecentTradedHistory        { get; set; }
    new IPQPublishedInternalOrders?    PublishedInternalOrders    { get; set; }
    new IPQPublishedLimits?            PublishedLimits            { get; set; }
    new IPQLimitBreaches?              LimitBreaches              { get; set; }
    new IPQMarginDetails?              MarginDetails              { get; set; }
    new IPQPnLConversions?             TickerPnLConversionRate    { get; set; }
    new IPQTickerRegionInfo?           TickerRegionInfo           { get; set; }
    new IPQAdapterExecutionStatistics? AdapterExecutionStatistics { get; set; }

    
    new DateTime InboundSocketReceivingTime { get; set; }
    new DateTime InboundProcessedTime       { get; set; }
    new DateTime SubscriberDispatchedTime   { get; set; }

    bool IsLastSourceFeedUpdateDateUpdated        { get; set; }
    bool IsLastSourceFeedUpdateSub2MinTimeUpdated { get; set; }
    bool IsDownstreamDateUpdated                  { get; set; }
    bool IsDownstreamSub2MinTimeUpdated           { get; set; }
    bool IsLastAdapterReceivedDateUpdated         { get; set; }
    bool IsLastAdapterReceivedSub2MinTimeUpdated  { get; set; }
    bool IsAdapterSentDateUpdated                 { get; set; }
    bool IsAdapterSentSub2MinTimeUpdated          { get; set; }
    bool IsSourceSequenceNumberUpdated            { get; set; }
    bool IsAdapterSequenceNumberUpdated           { get; set; }
    bool IsClientSequenceNumberUpdated            { get; set; }
    bool IsFeedSequenceNumberUpdated              { get; set; }
    bool IsFeedMarketConnectivityStatusUpdated    { get; set; }
    bool IsEventUpdateFlagsUpdated                { get; set; }

    new IPQTradingStatusFeedEvent Clone(); 

    new IPQTradingStatusFeedEvent? Next     { get; set; }
    new IPQTradingStatusFeedEvent? Previous { get; set; }
}

public class PQTradingStatusFeedEvent : PQReusableMessage, IPQTradingStatusFeedEvent, IDoublyLinkedListNode<PQTradingStatusFeedEvent>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQTradingStatusFeedEvent));

    protected FeedEventFieldUpdatedFlags UpdatedFlags;

    protected IPQSourceTickerInfo? PQSourceTickerInfo;

    private DateTime lastSourceFeedUpdateTime = DateTime.MinValue;
    private DateTime downstreamTime  = DateTime.MinValue;

    private uint sourceSeqNum;
    private uint adapterSeqNum;
    private uint clientSeqNum;
    private uint feedSeqNum;

    private FeedEventUpdateFlags        eventUpdateFlags;

    public PQTradingStatusFeedEvent()
    {
        PQSourceTickerInfo = new PQSourceTickerInfo();
        if (GetType() == typeof(PQPublishableTickInstant)) PQSequenceId = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQTradingStatusFeedEvent(ISourceTickerInfo sourceTickerInfo)
    {
        if (sourceTickerInfo is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            PQSourceTickerInfo = pqSourceTickerInfo;
        }
        else
        {
            PQSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        }
    }


    protected PQTradingStatusFeedEvent
    (ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, DateTime? clientReceivedTime = null)
    {
        if (sourceTickerInfo is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            PQSourceTickerInfo = pqSourceTickerInfo;
        }
        else
        {
            PQSourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        }
        FeedSyncStatus     = feedSyncStatus;
        ClientReceivedTime = clientReceivedTime ?? DateTime.MinValue;

        if (GetType() == typeof(PQPublishableTickInstant)) PQSequenceId = 0;
    }


    public PQTradingStatusFeedEvent(ITradingStatusFeedEvent toClone) 
    {
        base.CopyFrom(toClone);

        if (toClone.SourceTickerInfo is IPQSourceTickerInfo pqSrcTickerInfo)
        {
            PQSourceTickerInfo = pqSrcTickerInfo;
        }
        else if(toClone.SourceTickerInfo is not null)
        {
            PQSourceTickerInfo = new PQSourceTickerInfo(toClone.SourceTickerInfo);
        }

        SetFlagsSame(toClone);
        if (GetType() == typeof(PQTradingStatusFeedEvent)) PQSequenceId = 0;
    }

    public override uint StreamId => SourceTickerInfo!.SourceInstrumentId;
    public override string StreamName => SourceTickerInfo!.InstrumentName;

    [JsonIgnore] public virtual TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.SingleValue;

    [JsonIgnore] public override uint MessageId => (uint)PQMessageIds.FeedEvent;

    [JsonIgnore] public override byte Version => 1;

    [JsonIgnore] ISourceTickerInfo? ITradingStatusFeedEvent.SourceTickerInfo => PQSourceTickerInfo;

    public IPQSourceTickerInfo? SourceTickerInfo
    {
        get => PQSourceTickerInfo;
        set
        {
            if (ReferenceEquals(value, PQSourceTickerInfo)) return;
            if (value is PQSourceTickerInfo pqSourceTickerInfo) // share SourceTickerInfo if possible
                PQSourceTickerInfo = pqSourceTickerInfo;
            if (value != null)
            {
                PQSourceTickerInfo = PQSourceTickerInfo?.CopyFrom(value) ?? new PQSourceTickerInfo(value);
                return;
            }

            PQSourceTickerInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerInfo);
        }
    }

    public DateTime DownstreamTime
    {
        get => downstreamTime;
        set
        {
            if (downstreamTime == value) return;
            IsDownstreamDateUpdated
                |= downstreamTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsDownstreamSub2MinTimeUpdated |= downstreamTime.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            downstreamTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }
    public uint SourceSequenceNumber
    {
        get => sourceSeqNum;
        set
        {
            IsSourceSequenceNumberUpdated |= sourceSeqNum != value || PQSequenceId == 0;
            sourceSeqNum = value;
        }
    }
    public uint AdapterSequenceNumber
    {
        get => adapterSeqNum;
        set
        {
            IsAdapterSequenceNumberUpdated |= adapterSeqNum != value || PQSequenceId == 0;
            adapterSeqNum                 =  value;
        }
    }
    public uint ClientSequenceNumber
    {
        get => clientSeqNum;
        set
        {
            IsClientSequenceNumberUpdated |= clientSeqNum != value || PQSequenceId == 0;
            clientSeqNum                   =  value;
        }
    }

    public uint FeedSequenceNumber
    {
        get => feedSeqNum;
        set
        {
            IsClientSequenceNumberUpdated |= feedSeqNum != value || PQSequenceId == 0;
            feedSeqNum                    =  value;
        }
    }

    public FeedEventUpdateFlags EventUpdateFlags
    {
        get => eventUpdateFlags;
        set
        {
            IsEventUpdateFlagsUpdated |= eventUpdateFlags != value || PQSequenceId == 0;
            eventUpdateFlags                      =  value;
        }
    }
    IPublishedMarketEvents? ITradingStatusFeedEvent.     MarketEvents               => MarketEvents;
    IRecentlyTradedHistory? ITradingStatusFeedEvent.     RecentTradedHistory        => RecentTradedHistory;
    IPublishedInternalOrders? ITradingStatusFeedEvent.   PublishedInternalOrders    => PublishedInternalOrders;
    IPublishedAccounts? ITradingStatusFeedEvent.         PublishedAccounts          => PublishedAccounts;
    IPublishedLimits? ITradingStatusFeedEvent.           PublishedLimits            => PublishedLimits;
    ILimitBreaches? ITradingStatusFeedEvent.             LimitBreaches              => LimitBreaches;
    IMarginDetails? ITradingStatusFeedEvent.             MarginDetails              => MarginDetails;
    IPnLConversions? ITradingStatusFeedEvent.            TickerPnLConversionRate    => TickerPnLConversionRate;
    ITickerRegionInfo? ITradingStatusFeedEvent.          TickerRegionInfo           => TickerRegionInfo;
    IAdapterExecutionStatistics? ITradingStatusFeedEvent.AdapterExecutionStatistics => AdapterExecutionStatistics;
    
    ISourceTickerInfo? IMutableTradingStatusFeedEvent.SourceTickerInfo
    {
        get => SourceTickerInfo!;
        set => SourceTickerInfo = value as IPQSourceTickerInfo;
    }

    IMutablePublishedMarketEvents? IMutableTradingStatusFeedEvent.MarketEvents
    {
        get => MarketEvents;
        set => MarketEvents = value as IPQPublishedMarketEvents;
    }
    public IPQPublishedMarketEvents? MarketEvents { get; set; }

    IMutableRecentlyTradedHistory? IMutableTradingStatusFeedEvent.RecentTradedHistory
    {
        get => RecentTradedHistory;
        set => RecentTradedHistory = value as IPQRecentlyTradedHistory;
    }

    public IPQRecentlyTradedHistory? RecentTradedHistory { get; set; }

    IMutablePublishedInternalOrders? IMutableTradingStatusFeedEvent.PublishedInternalOrders
    {
        get => PublishedInternalOrders;
        set => PublishedInternalOrders = value as IPQPublishedInternalOrders;
    }

    public IPQPublishedInternalOrders? PublishedInternalOrders { get; set; }

    IMutablePublishedAccounts? IMutableTradingStatusFeedEvent.PublishedAccounts
    {
        get => PublishedAccounts;
        set => PublishedAccounts = value as IPQPublishedAccounts;
    }

    public IPQPublishedAccounts? PublishedAccounts { get; set; }

    IMutablePublishedLimits? IMutableTradingStatusFeedEvent.PublishedLimits
    {
        get => PublishedLimits;
        set => PublishedLimits = value as IPQPublishedLimits;
    }

    public IPQPublishedLimits? PublishedLimits { get; set; }

    IMutableLimitBreaches? IMutableTradingStatusFeedEvent.LimitBreaches
    {
        get => LimitBreaches;
        set => LimitBreaches = value as IPQLimitBreaches;
    }

    public IPQLimitBreaches? LimitBreaches { get; set; }

    IMutableMarginDetails? IMutableTradingStatusFeedEvent.MarginDetails
    {
        get => MarginDetails;
        set => MarginDetails = value as IPQMarginDetails;
    }

    public IPQMarginDetails? MarginDetails { get; set; }

    IMutablePnLConversions? IMutableTradingStatusFeedEvent.TickerPnLConversionRate
    {
        get => TickerPnLConversionRate;
        set => TickerPnLConversionRate = value as IPQPnLConversions;
    }

    public IPQPnLConversions? TickerPnLConversionRate { get; set; }

    IMutableTickerRegionInfo? IMutableTradingStatusFeedEvent.TickerRegionInfo
    {
        get => TickerRegionInfo;
        set => TickerRegionInfo = value as IPQTickerRegionInfo;
    }

    public IPQTickerRegionInfo? TickerRegionInfo { get; set; }

    IMutableAdapterExecutionStatistics? IMutableTradingStatusFeedEvent.AdapterExecutionStatistics
    {
        get => AdapterExecutionStatistics;
        set => AdapterExecutionStatistics = value as IPQAdapterExecutionStatistics;
    }

    public IPQAdapterExecutionStatistics? AdapterExecutionStatistics { get; set; }

    public bool IsDownstreamDateUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.DownstreamDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.DownstreamDateUpdatedFlag;

            else if (IsDownstreamDateUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.DownstreamDateUpdatedFlag;
        }
    }
    public bool IsDownstreamSub2MinTimeUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.DownstreamSub2MinTimeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.DownstreamSub2MinTimeUpdatedFlag;

            else if (IsDownstreamSub2MinTimeUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.DownstreamSub2MinTimeUpdatedFlag;
        }
    }
    public bool IsLastAdapterReceivedDateUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeDateUpdatedFlag;

            else if (IsLastAdapterReceivedDateUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeDateUpdatedFlag;
        }
    }
    public bool IsLastAdapterReceivedSub2MinTimeUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeSub2MinUpdatedFlag;

            else if (IsLastAdapterReceivedSub2MinTimeUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.LastAdapterReceivedTimeSub2MinUpdatedFlag;
        }
    }
    public bool IsAdapterSentDateUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;

            else if (IsAdapterSentDateUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
        }
    }
    public bool IsAdapterSentSub2MinTimeUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;

            else if (IsAdapterSentSub2MinTimeUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;
        }
    }
    public bool IsSourceSequenceNumberUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.SourceSequenceNumberUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.SourceSequenceNumberUpdatedFlag;

            else if (IsSourceSequenceNumberUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.SourceSequenceNumberUpdatedFlag;
        }
    }
    public bool IsAdapterSequenceNumberUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.AdapterSequenceNumberUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.AdapterSequenceNumberUpdatedFlag;

            else if (IsAdapterSequenceNumberUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.AdapterSequenceNumberUpdatedFlag;
        }
    }
    public bool IsClientSequenceNumberUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.ClientSequenceNumberUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.ClientSequenceNumberUpdatedFlag;

            else if (IsClientSequenceNumberUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.ClientSequenceNumberUpdatedFlag;
        }
    }
    public bool IsFeedSequenceNumberUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.FeedSequenceNumberUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.FeedSequenceNumberUpdatedFlag;

            else if (IsFeedSequenceNumberUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.FeedSequenceNumberUpdatedFlag;
        }
    }
    public bool IsFeedMarketConnectivityStatusUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.FeedMarketConnectivityStatusUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.FeedMarketConnectivityStatusUpdatedFlag;

            else if (IsFeedMarketConnectivityStatusUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.FeedMarketConnectivityStatusUpdatedFlag;
        }
    }
    public bool IsEventUpdateFlagsUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.FeedEventUpdateTypeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.FeedEventUpdateTypeUpdatedFlag;

            else if (IsEventUpdateFlagsUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.FeedEventUpdateTypeUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime LastSourceFeedUpdateTime
    {
        get => lastSourceFeedUpdateTime;
        set
        {
            IsLastSourceFeedUpdateDateUpdated |= lastSourceFeedUpdateTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() ||
                                                 PQSequenceId == 0;
            IsLastSourceFeedUpdateSub2MinTimeUpdated
                |= lastSourceFeedUpdateTime.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            lastSourceFeedUpdateTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsLastSourceFeedUpdateDateUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.LastSourceFeedSentDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.LastSourceFeedSentDateUpdatedFlag;

            else if (IsLastSourceFeedUpdateDateUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.LastSourceFeedSentDateUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsLastSourceFeedUpdateSub2MinTimeUpdated
    {
        get => (UpdatedFlags & FeedEventFieldUpdatedFlags.LastSourceFeedSentSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= FeedEventFieldUpdatedFlags.LastSourceFeedSentSub2MinUpdatedFlag;

            else if (IsLastSourceFeedUpdateSub2MinTimeUpdated) UpdatedFlags ^= FeedEventFieldUpdatedFlags.LastSourceFeedSentSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public new PQTradingStatusFeedEvent? Previous
    {
        get => GetPrevious<PQTradingStatusFeedEvent?>();
        set => SetPrevious(value);
    }

    [JsonIgnore]
    public new PQTradingStatusFeedEvent? Next
    {
        get => GetNext<PQTradingStatusFeedEvent?>();
        set => SetNext(value);
    }

    IPQTradingStatusFeedEvent? IDoublyLinkedListNode<IPQTradingStatusFeedEvent>.Previous
    {
        get => GetPrevious<IPQTradingStatusFeedEvent?>();
        set => SetPrevious(value);
    }
    IPQTradingStatusFeedEvent? IDoublyLinkedListNode<IPQTradingStatusFeedEvent>.Next
    {
        get => GetNext<IPQTradingStatusFeedEvent?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    IPQTradingStatusFeedEvent? IPQTradingStatusFeedEvent.Previous
    {
        get => GetPrevious<IPQTradingStatusFeedEvent?>();
        set => SetPrevious(value);
    }

    [JsonIgnore]
    IPQTradingStatusFeedEvent? IPQTradingStatusFeedEvent.Next
    {
        get => GetNext<IPQTradingStatusFeedEvent?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    ITradingStatusFeedEvent? IDoublyLinkedListNode<ITradingStatusFeedEvent>.Previous
    {
        get => GetPrevious<ITradingStatusFeedEvent?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    ITradingStatusFeedEvent? IDoublyLinkedListNode<ITradingStatusFeedEvent>.Next
    {
        get => GetNext<ITradingStatusFeedEvent?>();
        set => SetNext(value);
    }


    public override bool IsEmpty
    {
        get
        {
            var baseEmpty             = base.IsEmpty;
            var downstreamTimeEmpty   = DownstreamTime == DateTime.MinValue;
            var srcSeqNumEmpty        = SourceSequenceNumber == 0;
            var adptrSeqNumEmpty      = AdapterSequenceNumber == 0;
            var clntSeqNumEmpty       = ClientSequenceNumber == 0;
            var feedSeqNumEmpty       = FeedSequenceNumber == 0;
            var feedMktConStatusEmpty = FeedMarketConnectivityStatus == FeedConnectivityStatusFlags.AwaitingConnectionStart;
            var feedEvtUpdateEmpty    = EventUpdateFlags == FeedEventUpdateFlags.NoDataYetReceived;
            var mktEvtsEmpty          = MarketEvents?.IsEmpty ?? true;
            var trdHistEmpty          = RecentTradedHistory?.IsEmpty ?? true;
            var pubIntrnlOrdsEmpty    = PublishedInternalOrders?.IsEmpty ?? true;
            var pubAcctsEmpty         = PublishedAccounts?.IsEmpty ?? true;
            var pubLmtsEmpty          = PublishedLimits?.IsEmpty ?? true;
            var pubLmtBrchsEmpty      = LimitBreaches?.IsEmpty ?? true;
            var mrgnDetsEmpty         = MarginDetails?.IsEmpty ?? true;
            var pnlConvEmpty          = TickerPnLConversionRate?.IsEmpty ?? true;
            var tkrRegEmpty           = TickerRegionInfo?.IsEmpty ?? true;
            var adptrExecStatsEmpty   = AdapterExecutionStatistics?.IsEmpty ?? true;


            var allAreEmpty = baseEmpty && downstreamTimeEmpty && srcSeqNumEmpty && adptrSeqNumEmpty &&
                              clntSeqNumEmpty
                           && feedSeqNumEmpty && feedMktConStatusEmpty && feedEvtUpdateEmpty && mktEvtsEmpty && trdHistEmpty && pubIntrnlOrdsEmpty &&
                              pubAcctsEmpty && pubLmtsEmpty
                           && pubLmtBrchsEmpty && mrgnDetsEmpty && pnlConvEmpty && tkrRegEmpty && adptrExecStatsEmpty;

            return allAreEmpty;
        }
        set
        {
            base.IsEmpty = value;
            if (!value) return;

            AdapterReceivedTime      = DateTime.MinValue;
            AdapterSentTime              = DateTime.MinValue;
            DownstreamTime               = DateTime.MinValue;
            SourceSequenceNumber         = 0;
            AdapterSequenceNumber        = 0;
            ClientSequenceNumber         = 0;
            FeedSequenceNumber           = 0;
            FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AwaitingConnectionStart;
            EventUpdateFlags             = FeedEventUpdateFlags.NoDataYetReceived;
            if (MarketEvents != null) MarketEvents!.IsEmpty                             = true;
            if (RecentTradedHistory != null) RecentTradedHistory!.IsEmpty               = true;
            if (PublishedInternalOrders != null) PublishedInternalOrders!.IsEmpty       = true;
            if (PublishedAccounts != null) PublishedAccounts!.IsEmpty                   = true;
            if (PublishedLimits != null) PublishedLimits!.IsEmpty                       = true;
            if (LimitBreaches != null) LimitBreaches!.IsEmpty                           = true;
            if (MarginDetails != null) MarginDetails!.IsEmpty                           = true;
            if (TickerPnLConversionRate != null) TickerPnLConversionRate!.IsEmpty       = true;
            if (TickerRegionInfo != null) TickerRegionInfo!.IsEmpty                     = true;
            if (AdapterExecutionStatistics != null) AdapterExecutionStatistics!.IsEmpty = true;
        }
    }


    [JsonIgnore]
    public override bool HasUpdates
    {
        get
        {
            var localUpdates = UpdatedFlags != FeedEventFieldUpdatedFlags.None;

            var mktEvtsHasUpdates        = MarketEvents?.HasUpdates ?? false;
            var trdHistHasUpdates        = RecentTradedHistory?.HasUpdates ?? false;
            var pubIntrnlOrdsHasUpdates  = PublishedInternalOrders?.HasUpdates ?? false;
            var pubAcctsHasUpdates       = PublishedAccounts?.HasUpdates ?? false;
            var pubLmtsHasUpdates        = PublishedLimits?.HasUpdates ?? false;
            var pubLmtBrchsHasUpdates    = LimitBreaches?.HasUpdates ?? false;
            var mrgnDetsHasUpdates       = MarginDetails?.HasUpdates ?? false;
            var pnlConvHasUpdates        = TickerPnLConversionRate?.HasUpdates ?? false;
            var tkrRegHasUpdates         = TickerRegionInfo?.HasUpdates ?? false;
            var adptrExecStatsHasUpdates = AdapterExecutionStatistics?.HasUpdates ?? false;

            var allHasUpdates =
                localUpdates && mktEvtsHasUpdates && trdHistHasUpdates && pubIntrnlOrdsHasUpdates && pubAcctsHasUpdates && pubLmtsHasUpdates
             && pubLmtBrchsHasUpdates && mrgnDetsHasUpdates && pnlConvHasUpdates && tkrRegHasUpdates &&
                adptrExecStatsHasUpdates;

            return allHasUpdates;
        }
        set
        {
            if (PQSourceTickerInfo != null) PQSourceTickerInfo.HasUpdates = value;
            if (MarketEvents != null) MarketEvents!.HasUpdates = value;
            if (RecentTradedHistory != null) RecentTradedHistory!.HasUpdates = value;
            if (PublishedInternalOrders != null) PublishedInternalOrders!.HasUpdates = value;
            if (PublishedAccounts != null) PublishedAccounts!.HasUpdates = value;
            if (PublishedLimits != null) PublishedLimits!.HasUpdates = value;
            if (LimitBreaches != null) LimitBreaches!.HasUpdates = value;
            if (MarginDetails != null) MarginDetails!.HasUpdates = value;
            if (TickerPnLConversionRate != null) TickerPnLConversionRate!.HasUpdates = value;
            if (TickerRegionInfo != null) TickerRegionInfo!.HasUpdates = value;
            if (AdapterExecutionStatistics != null) AdapterExecutionStatistics!.HasUpdates = value;
            if (value) return;
            UpdatedFlags = FeedEventFieldUpdatedFlags.None;
        }
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        PQSourceTickerInfo?.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
        MarketEvents?.UpdateComplete(updateSequenceId);
        RecentTradedHistory?.UpdateComplete(updateSequenceId);
        PublishedInternalOrders?.UpdateComplete(updateSequenceId);
        PublishedAccounts?.UpdateComplete(updateSequenceId);
        PublishedLimits?.UpdateComplete(updateSequenceId);
        LimitBreaches?.UpdateComplete(updateSequenceId);
        MarginDetails?.UpdateComplete(updateSequenceId);
        TickerPnLConversionRate?.UpdateComplete(updateSequenceId);
        TickerRegionInfo?.UpdateComplete(updateSequenceId);
        AdapterExecutionStatistics?.UpdateComplete(updateSequenceId);
        HasUpdates = false;
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        ClientReceivedTime            += toChangeBy;
        SubscriberDispatchedTime += toChangeBy;
        InboundProcessedTime     += toChangeBy;
        InboundSocketReceivingTime    += toChangeBy;
        ClientReceivedTime            += toChangeBy;
    }

    public override void SetPublisherStateToConnectivityStatus(PublisherStates publisherStates, DateTime atDateTime)
    {
        ResetWithTracking();
        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.Disconnecting | FeedConnectivityStatusFlags.AdapterReporting;
        AdapterSentTime              = atDateTime;
    }


    public override IPQTradingStatusFeedEvent ResetWithTracking()
    {
        base.ResetWithTracking();

        UpdatedFlags = FeedEventFieldUpdatedFlags.None;
        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags) =>
        GetDeltaUpdateFields(snapShotTime, messageFlags, null);

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var quoteContainerUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags))
        {
            yield return quoteContainerUpdates;
        }

        if(PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetDeltaUpdateFields
                         (snapShotTime, messageFlags, quotePublicationPrecisionSettings ?? PQSourceTickerInfo))
                yield return field;
    }


    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerInfo!.UpdateField(pqFieldUpdate);
        if (infoResult >= 0) return infoResult;

        return base.UpdateField(pqFieldUpdate);
    }

    public override void EnsureRelatedItemsAreConfigured(IPQMessage? item)
    {
        if (item is ITradingStatusFeedEvent trdStsFeedEvt)
        {
            EnsureRelatedItemsAreConfigured(trdStsFeedEvt.SourceTickerInfo);
            if (item is PQTradingStatusFeedEvent pqPubTickInstant)
            {
            }
        }
    }

    public virtual void EnsureRelatedItemsAreConfigured(ITradingStatusFeedEvent? referenceInstance)
    {
        if (referenceInstance is { SourceTickerInfo: IPQSourceTickerInfo pqSrcTkrQuoteInfo })
            SourceTickerInfo = pqSrcTkrQuoteInfo;
    }

    public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? srcTickerInfo)
    {
        if (srcTickerInfo == null || ReferenceEquals(SourceTickerInfo, srcTickerInfo)) return;
        if (srcTickerInfo.AreEquivalent(SourceTickerInfo)) return;

        SourceTickerInfo = new PQSourceTickerInfo(srcTickerInfo);
    }

    public bool AreEquivalent(ITickInstant? other, bool exactTypes = false) => AreEquivalent(other as IPQMessage, exactTypes);

    public virtual bool AreEquivalent(ITradingStatusFeedEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        var tickerInfoSame = PQSourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes) ?? other.SourceTickerInfo == null;

        var allAreSame = tickerInfoSame && baseSame;
        return allAreSame;
    }

    public override bool AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes = false)
    {
        if (other is ITradingStatusFeedEvent trdingStsFeedEvt)
        {
            return AreEquivalent(trdingStsFeedEvt, exactTypes);
        }
        return false;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITradingStatusFeedEvent> feedStorageResolver) return feedStorageResolver.ResolveStorageTime(this);
        return FeedEventStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetStringUpdates(snapShotTime, messageFlags))
                yield return field;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate) =>
        PQSourceTickerInfo?.UpdateFieldString(stringUpdate) ?? false;

    IReusableObject<IFeedEventStatusUpdate> ITransferState<IReusableObject<IFeedEventStatusUpdate>>.CopyFrom
        (IReusableObject<IFeedEventStatusUpdate> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    IReusableObject<ITradingStatusFeedEvent> ITransferState<IReusableObject<ITradingStatusFeedEvent>>.CopyFrom
        (IReusableObject<ITradingStatusFeedEvent> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITradingStatusFeedEvent)source, copyMergeFlags);

    IMutableTradingStatusFeedEvent ITransferState<IMutableTradingStatusFeedEvent>.CopyFrom(IMutableTradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);


    IPQTradingStatusFeedEvent ITransferState<IPQTradingStatusFeedEvent>.CopyFrom(IPQTradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IFeedEventStatusUpdate ITransferState<IFeedEventStatusUpdate>.CopyFrom
        (IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    ITradingStatusFeedEvent ITransferState<ITradingStatusFeedEvent>.CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public virtual PQTradingStatusFeedEvent CopyFrom(ITradingStatusFeedEvent source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQTradingStatusFeedEvent ipq0)
        {
            if (PQSourceTickerInfo != null)
                PQSourceTickerInfo.CopyFrom(ipq0.SourceTickerInfo!, copyMergeFlags);
            else
                SourceTickerInfo = ipq0.SourceTickerInfo;

            if (source is PQTradingStatusFeedEvent pq0)
            {
                // only copy if changed
                var isFullReplace               = copyMergeFlags.HasFullReplace();
                if (isFullReplace) UpdatedFlags = pq0.UpdatedFlags;
            }
        }
        else
        {
            if (source.SourceTickerInfo != null)
            {
                SourceTickerInfo ??= new PQSourceTickerInfo(source.SourceTickerInfo);
                SourceTickerInfo.CopyFrom(source.SourceTickerInfo);
            }
            else
            {
                SourceTickerInfo?.StateReset();
            }
        }

        return this;
    }

    public override PQTradingStatusFeedEvent CopyFrom(IFeedEventStatusUpdate? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ITradingStatusFeedEvent pubTickInstant)
        {
            CopyFrom(pubTickInstant, copyMergeFlags);
        }
        else
        {
            base.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    public override PQTradingStatusFeedEvent Clone() =>
        Recycler?.Borrow<PQTradingStatusFeedEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQTradingStatusFeedEvent(this);

    IPQTradingStatusFeedEvent IPQTradingStatusFeedEvent.Clone() => Clone();

    ITradingStatusFeedEvent ICloneable<ITradingStatusFeedEvent>.  Clone() => Clone();

    ITradingStatusFeedEvent ITradingStatusFeedEvent.              Clone() => Clone();

    IMutableTradingStatusFeedEvent IMutableTradingStatusFeedEvent.Clone() => Clone();

    public virtual PQTradingStatusFeedEvent SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        ((IMutableTradingStatusFeedEvent)this).SourceTickerInfo = toSet;
        return this;
    }

    private IPQSourceTickerInfo ConvertToPQSourceTickerInfo
        (ISourceTickerInfo value, IPQSourceTickerInfo? originalQuoteInfo)
    {
        if (originalQuoteInfo == null)
        {
            originalQuoteInfo = new PQSourceTickerInfo(value);
            return originalQuoteInfo;
        }

        originalQuoteInfo.CopyFrom(value);
        return originalQuoteInfo;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITradingStatusFeedEvent, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ (int)PQSequenceId;
            hashCode = (hashCode * 397) ^ SourceTickerInfo?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ LastPublicationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ InboundSocketReceivingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ InboundProcessedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SubscriberDispatchedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)FeedSyncStatus;
            hashCode = (hashCode * 397) ^ (SourceTickerInfo?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    
    protected string TradingStatusToStringMembers =>
        $"{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, {nameof(AdapterSentTime)}: {AdapterSentTime}, " +
        $"{nameof(DownstreamTime)}: {DownstreamTime}, {nameof(SourceSequenceNumber)}: {SourceSequenceNumber}, {nameof(AdapterSequenceNumber)}: {AdapterSequenceNumber}, " +
        $"{nameof(ClientSequenceNumber)}: {ClientSequenceNumber}, {nameof(FeedSequenceNumber)}: {FeedSequenceNumber}, " +
        $"{nameof(FeedMarketConnectivityStatus)}: {FeedMarketConnectivityStatus}, {nameof(EventUpdateFlags)}: {EventUpdateFlags}, " +
        $"{nameof(SourceTickerInfo)}: {SourceTickerInfo}, {nameof(MarketEvents)}: {MarketEvents}, {nameof(RecentTradedHistory)}: {RecentTradedHistory}, " +
        $"{nameof(PublishedInternalOrders)}: {PublishedInternalOrders}, {nameof(PublishedAccounts)}: {PublishedAccounts}, " +
        $"{nameof(PublishedLimits)}: {PublishedLimits}, {nameof(LimitBreaches)}: {LimitBreaches}, {nameof(MarginDetails)}: {MarginDetails}, " +
        $"{nameof(TickerPnLConversionRate)}: {TickerPnLConversionRate}, {nameof(TickerRegionInfo)}: {TickerRegionInfo}, " +
        $"{nameof(AdapterExecutionStatistics)}: {AdapterExecutionStatistics}, {nameof(LastSourceFeedUpdateTime)}: {LastSourceFeedUpdateTime}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQTradingStatusFeedEvent)}{{{TradingStatusToStringMembers}, {UpdatedFlagsToString}}}";

    protected void SetFlagsSame(ITradingStatusFeedEvent toCopyFlags)
    {
        if (toCopyFlags is PQTradingStatusFeedEvent pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }
}
