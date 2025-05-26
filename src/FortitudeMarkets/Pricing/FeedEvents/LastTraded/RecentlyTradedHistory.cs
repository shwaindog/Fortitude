// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.Utils;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class RecentlyTradedHistory : ReusableObject<IRecentlyTradedHistory>, IMutableRecentlyTradedHistory
{

    public RecentlyTradedHistory()
    {
        OnTickLastTraded            = new OnTickLastTraded();
        AllLimitedHistoryLastTrades = new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                                                       , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        RecentInternalOrdersTrades = new RecentlyTraded(IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        OpenPositionTrades         = new RecentlyTraded(IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        AlertTrades                = new RecentlyTraded(IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        ClientOnlyReceivedCache    = new RecentlyTraded(IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
    }

    public RecentlyTradedHistory(ISourceTickerInfo sourceTickerInfo)
    {
        OnTickLastTraded = new OnTickLastTraded(sourceTickerInfo);
        AllLimitedHistoryLastTrades
            = new RecentlyTraded(sourceTickerInfo, IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                               , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        RecentInternalOrdersTrades = new RecentlyTraded(sourceTickerInfo, IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        OpenPositionTrades         = new RecentlyTraded(sourceTickerInfo, IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        AlertTrades                = new RecentlyTraded(sourceTickerInfo, IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        ClientOnlyReceivedCache    = new RecentlyTraded(sourceTickerInfo, IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
    }

    public RecentlyTradedHistory
    (IMutableOnTickLastTraded? onTickLastTraded = null, IMutableRecentlyTraded? allLimitedHistoryLastTrades = null
      , IMutableRecentlyTraded? clientOnlyReceivedCache = null
      , IMutableRecentlyTraded? openPositionTrades = null, IMutableRecentlyTraded? recentInternalOrdersTrades = null
      , IMutableRecentlyTraded? alertTrades = null)
    {
        OnTickLastTraded = onTickLastTraded ?? new OnTickLastTraded();
        AllLimitedHistoryLastTrades = allLimitedHistoryLastTrades ??
                                      new RecentlyTraded(IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                                                       , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        RecentInternalOrdersTrades = recentInternalOrdersTrades ??
                                     new RecentlyTraded(IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        OpenPositionTrades = openPositionTrades ?? new RecentlyTraded(IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        AlertTrades        = alertTrades ?? new RecentlyTraded(IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        ClientOnlyReceivedCache
            = clientOnlyReceivedCache ?? new RecentlyTraded(IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
    }

    public RecentlyTradedHistory(IRecentlyTradedHistory toClone)
    {
        if (toClone is RecentlyTradedHistory recentlyTradedHistory)
        {
            OnTickLastTraded            = recentlyTradedHistory.OnTickLastTraded.Clone();
            AllLimitedHistoryLastTrades = recentlyTradedHistory.AllLimitedHistoryLastTrades.Clone();
            RecentInternalOrdersTrades  = recentlyTradedHistory.RecentInternalOrdersTrades.Clone();
            OpenPositionTrades          = recentlyTradedHistory.OpenPositionTrades.Clone();
            AlertTrades                 = recentlyTradedHistory.AlertTrades.Clone();
            ClientOnlyReceivedCache     = recentlyTradedHistory.ClientOnlyReceivedCache.Clone();
        }
        else
        {
            OnTickLastTraded            = new OnTickLastTraded(toClone.OnTickLastTraded);
            AllLimitedHistoryLastTrades = new RecentlyTraded(toClone.AllLimitedHistoryLastTrades);
            RecentInternalOrdersTrades  = new RecentlyTraded(toClone.RecentInternalOrdersTrades);
            OpenPositionTrades          = new RecentlyTraded(toClone.OpenPositionTrades);
            AlertTrades                 = new RecentlyTraded(toClone.AlertTrades);
            ClientOnlyReceivedCache     = new RecentlyTraded(toClone.ClientOnlyReceivedCache);
        }
    }

    IOnTickLastTraded IRecentlyTradedHistory.OnTickLastTraded => OnTickLastTraded;

    public IMutableOnTickLastTraded OnTickLastTraded { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.AllLimitedHistoryLastTrades => AllLimitedHistoryLastTrades;

    public IMutableRecentlyTraded AllLimitedHistoryLastTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.RecentInternalOrdersTrades => RecentInternalOrdersTrades;

    public IMutableRecentlyTraded RecentInternalOrdersTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.OpenPositionTrades => OpenPositionTrades;

    public IMutableRecentlyTraded OpenPositionTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.AlertTrades => AlertTrades;

    public IMutableRecentlyTraded AlertTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.ClientOnlyReceivedCache => ClientOnlyReceivedCache;

    public IMutableRecentlyTraded ClientOnlyReceivedCache { get; set; }

    public bool IsEmpty
    {
        get =>
            OnTickLastTraded.IsEmpty
         && AllLimitedHistoryLastTrades.IsEmpty
         && RecentInternalOrdersTrades.IsEmpty
         && OpenPositionTrades.IsEmpty
         && AlertTrades.IsEmpty
         && ClientOnlyReceivedCache.IsEmpty;
        set
        {
            OnTickLastTraded.IsEmpty            = value;
            AllLimitedHistoryLastTrades.IsEmpty = value;
            RecentInternalOrdersTrades.IsEmpty  = value;
            OpenPositionTrades.IsEmpty          = value;
            AlertTrades.IsEmpty                 = value;
            ClientOnlyReceivedCache.IsEmpty     = value;
        }
    }

    public IMutableRecentlyTradedHistory ResetWithTracking()
    {
        OnTickLastTraded.IsEmpty            = true;
        AllLimitedHistoryLastTrades.IsEmpty = true;
        RecentInternalOrdersTrades.IsEmpty  = true;
        OpenPositionTrades.IsEmpty          = true;
        AlertTrades.IsEmpty                 = true;
        ClientOnlyReceivedCache.IsEmpty     = true;

        return this;
    }

    IMutableRecentlyTradedHistory ICloneable<IMutableRecentlyTradedHistory>.Clone() => Clone();

    IMutableRecentlyTradedHistory IMutableRecentlyTradedHistory.Clone() => Clone();

    public override RecentlyTradedHistory Clone() => Recycler?.Borrow<RecentlyTradedHistory>().CopyFrom(this) ?? new RecentlyTradedHistory(this);

    public override RecentlyTradedHistory CopyFrom(IRecentlyTradedHistory source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OnTickLastTraded.CopyFrom(source.OnTickLastTraded, copyMergeFlags);
        AllLimitedHistoryLastTrades.CopyFrom(source.AllLimitedHistoryLastTrades, copyMergeFlags);
        RecentInternalOrdersTrades.CopyFrom(source.RecentInternalOrdersTrades, copyMergeFlags);
        OpenPositionTrades.CopyFrom(source.OpenPositionTrades, copyMergeFlags);
        AlertTrades.CopyFrom(source.AlertTrades, copyMergeFlags);
        ClientOnlyReceivedCache.CopyFrom(source.ClientOnlyReceivedCache, copyMergeFlags);

        return this;
    }

    public bool AreEquivalent(IRecentlyTradedHistory? other, bool exactTypes = false)
    {
        if (other is null) return false;

        var onTickSame        = OnTickLastTraded.AreEquivalent(other.OnTickLastTraded, exactTypes);
        var limHistSame       = AllLimitedHistoryLastTrades.AreEquivalent(other.AllLimitedHistoryLastTrades, exactTypes);
        var internalOrderSame = RecentInternalOrdersTrades.AreEquivalent(other.RecentInternalOrdersTrades, exactTypes);
        var openPosSame       = OpenPositionTrades.AreEquivalent(other.OpenPositionTrades, exactTypes);
        var alertTradeSame    = AlertTrades.AreEquivalent(other.AlertTrades, exactTypes);

        var clientOnlyRecvSame = true;
        if (exactTypes)
        {
            clientOnlyRecvSame = ClientOnlyReceivedCache.AreEquivalent(other.ClientOnlyReceivedCache, exactTypes);
        }

        var allAreSame = onTickSame && limHistSame && internalOrderSame && openPosSame && alertTradeSame && clientOnlyRecvSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTradedHistory, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OnTickLastTraded.GetHashCode();
            hashCode = (AllLimitedHistoryLastTrades.GetHashCode() * 397) ^ hashCode;
            hashCode = (RecentInternalOrdersTrades.GetHashCode() * 397) ^ hashCode;
            hashCode = (OpenPositionTrades.GetHashCode() * 397) ^ hashCode;
            hashCode = (AlertTrades.GetHashCode() * 397) ^ hashCode;
            hashCode = (ClientOnlyReceivedCache.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string RecentlyTradedHistoryToStringMembers =>
        $"{nameof(OnTickLastTraded)}: {OnTickLastTraded}, {nameof(AllLimitedHistoryLastTrades)}: {AllLimitedHistoryLastTrades}, " +
        $"{nameof(RecentInternalOrdersTrades)}: {RecentInternalOrdersTrades}, {nameof(OpenPositionTrades)}: {OpenPositionTrades}, " +
        $"{nameof(AlertTrades)}: {AlertTrades}, {nameof(ClientOnlyReceivedCache)}: {ClientOnlyReceivedCache}";

    public override string ToString() => $"{nameof(RecentlyTradedHistory)}{{{RecentlyTradedHistoryToStringMembers}}}";
}
