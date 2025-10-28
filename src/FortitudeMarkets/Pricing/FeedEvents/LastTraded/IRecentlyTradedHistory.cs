using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IRecentlyTradedHistory : IReusableObject<IRecentlyTradedHistory>, IInterfacesComparable<IRecentlyTradedHistory>, IShowsEmpty
{
    //at roughly 150+ string len bytes- 100 last trades updates is  ~15k
    public const ushort DefaultLimitedHistoryMaxTradeCount = 100;
    public const ushort PublishedHistoryMaxTradeCount = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;


    public const ListTransmissionFlags DefaultAllLimitedHistoryLastTradedTransmissionFlags =
        ListTransmissionFlags.LimitByPeriodTime | ListTransmissionFlags.LimitByTradeCount |
        ListTransmissionFlags.PublishOnCompleteOrSnapshot |
        ListTransmissionFlags.ResetOnNewTradingDay;

    public const ListTransmissionFlags DefaultRecentInternalOrdersTradesTransmissionFlags =
        ListTransmissionFlags.DoNotUpdateFromOnTickUpdates |
        ListTransmissionFlags.PublishesOnDeltaUpdates | ListTransmissionFlags.PublishOnCompleteOrSnapshot |
        ListTransmissionFlags.ResetOnNewTradingDay;

    public const ListTransmissionFlags DefaultOpenPositionTradesTransmissionFlags = DefaultRecentInternalOrdersTradesTransmissionFlags;

    public const ListTransmissionFlags DefaultAlertTradesTransmissionFlags = DefaultRecentInternalOrdersTradesTransmissionFlags;

    public const ListTransmissionFlags DefaultClientOnlyReceivedCacheTransmissionFlags =
        ListTransmissionFlags.OnlyWhenCopyMergeFlagsKeepCacheItems |
        ListTransmissionFlags.DoNotAutoRemoveExpiredPeriod |
        ListTransmissionFlags.DoNotAutoRemoveExceededCount;

    IOnTickLastTraded OnTickLastTraded            { get; }
    IRecentlyTraded   AllLimitedHistoryLastTrades { get; }
    IRecentlyTraded   RecentInternalOrdersTrades  { get; }
    IRecentlyTraded   OpenPositionTrades          { get; }
    IRecentlyTraded   AlertTrades                 { get; }
    IRecentlyTraded   ClientOnlyReceivedCache     { get; }
}

public interface IMutableRecentlyTradedHistory : IRecentlyTradedHistory, ITrackableReset<IMutableRecentlyTradedHistory>, IEmptyable
  , ICloneable<IMutableRecentlyTradedHistory>,  IListensToLifeCycleChanges<IMarketTradingStateEvent>
{
    new IMutableOnTickLastTraded OnTickLastTraded            { get; set; }
    new IMutableRecentlyTraded   AllLimitedHistoryLastTrades { get; set; }
    new IMutableRecentlyTraded   RecentInternalOrdersTrades  { get; set; }
    new IMutableRecentlyTraded   OpenPositionTrades          { get; set; }
    new IMutableRecentlyTraded   ClientOnlyReceivedCache     { get; set; }
    new IMutableRecentlyTraded   AlertTrades                 { get; set; }

    new IMutableRecentlyTradedHistory Clone();
}