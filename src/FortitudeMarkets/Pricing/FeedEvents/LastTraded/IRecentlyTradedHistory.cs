using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IRecentlyTradedHistory : IReusableObject<IRecentlyTradedHistory>, IInterfacesComparable<IRecentlyTradedHistory>, IShowsEmpty
{
    public const ushort DefaultLimitedHistoryMaxTradeCount = 100;
    public const ushort PublishedHistoryMaxTradeCount = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;


    public const LastTradedTransmissionFlags DefaultAllLimitedHistoryLastTradedTransmissionFlags =
        LastTradedTransmissionFlags.LimitByPeriodTime | LastTradedTransmissionFlags.LimitByTradeCount |
        LastTradedTransmissionFlags.PublishOnCompleteOrSnapshot |
        LastTradedTransmissionFlags.ResetOnNewTradingDay;

    public const LastTradedTransmissionFlags DefaultRecentInternalOrdersTradesTransmissionFlags =
        LastTradedTransmissionFlags.DoNotUpdateFromOnTickLastTraded |
        LastTradedTransmissionFlags.PublishesOnDeltaUpdates | LastTradedTransmissionFlags.PublishOnCompleteOrSnapshot |
        LastTradedTransmissionFlags.ResetOnNewTradingDay;

    public const LastTradedTransmissionFlags DefaultOpenPositionTradesTransmissionFlags = DefaultRecentInternalOrdersTradesTransmissionFlags;

    public const LastTradedTransmissionFlags DefaultAlertTradesTransmissionFlags = DefaultRecentInternalOrdersTradesTransmissionFlags;

    public const LastTradedTransmissionFlags DefaultClientOnlyReceivedCacheTransmissionFlags =
        LastTradedTransmissionFlags.OnlyWhenCopyMergeFlagsKeepCacheItems |
        LastTradedTransmissionFlags.DoNotAutoRemoveExpiredPeriod |
        LastTradedTransmissionFlags.DoNotAutoRemoveExceededCount;

    IOnTickLastTraded OnTickLastTraded            { get; }
    IRecentlyTraded   AllLimitedHistoryLastTrades { get; }
    IRecentlyTraded   RecentInternalOrdersTrades  { get; }
    IRecentlyTraded   OpenPositionTrades          { get; }
    IRecentlyTraded   AlertTrades                 { get; }
    IRecentlyTraded   ClientOnlyReceivedCache     { get; }
}

public interface IMutableRecentlyTradedHistory : IRecentlyTradedHistory, ITrackableReset<IMutableRecentlyTradedHistory>, IEmptyable
  , ICloneable<IMutableRecentlyTradedHistory>, IScopedDiscreetUpdatable
{
    new IMutableOnTickLastTraded OnTickLastTraded            { get; set; }
    new IMutableRecentlyTraded   AllLimitedHistoryLastTrades { get; set; }
    new IMutableRecentlyTraded   RecentInternalOrdersTrades  { get; set; }
    new IMutableRecentlyTraded   OpenPositionTrades          { get; set; }
    new IMutableRecentlyTraded   ClientOnlyReceivedCache     { get; set; }
    new IMutableRecentlyTraded   AlertTrades                 { get; set; }

    new IMutableRecentlyTradedHistory Clone();
}