using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQRecentlyTradedHistory : IMutableRecentlyTradedHistory, IPQSupportsNumberPrecisionFieldUpdates
  , IPQSupportsStringUpdates, ITrackableReset<IPQRecentlyTradedHistory>, ISupportsPQNameIdLookupGenerator
{
    new IPQOnTickLastTraded OnTickLastTraded            { get; set; }
    new IPQRecentlyTraded   AllLimitedHistoryLastTrades { get; set; }
    new IPQRecentlyTraded   RecentInternalOrdersTrades  { get; set; }
    new IPQRecentlyTraded   OpenPositionTrades          { get; set; }
    new IPQRecentlyTraded   ClientOnlyReceivedCache     { get; set; }
    new IPQRecentlyTraded   AlertTrades                 { get; set; }

    new IPQRecentlyTradedHistory ResetWithTracking();

    new IPQRecentlyTradedHistory Clone();
}

public class PQRecentlyTradedHistory : ReusableObject<IRecentlyTradedHistory>, IPQRecentlyTradedHistory
{
    protected uint SequenceId = uint.MaxValue;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;

    public PQRecentlyTradedHistory()
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        OnTickLastTraded      = new PQOnTickLastTraded(nameIdLookupGenerator);
        AllLimitedHistoryLastTrades = new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                                                         , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        RecentInternalOrdersTrades = new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        OpenPositionTrades         = new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        AlertTrades                = new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        ClientOnlyReceivedCache    = new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
    }

    public PQRecentlyTradedHistory(ISourceTickerInfo sourceTickerInfo)
    {
        nameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        OnTickLastTraded      = new PQOnTickLastTraded(sourceTickerInfo, nameIdLookupGenerator);
        AllLimitedHistoryLastTrades
            = new PQRecentlyTraded(sourceTickerInfo, nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                                 , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        RecentInternalOrdersTrades
            = new PQRecentlyTraded(sourceTickerInfo, nameIdLookupGenerator, IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        OpenPositionTrades      = new PQRecentlyTraded(sourceTickerInfo, nameIdLookupGenerator, IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        AlertTrades             = new PQRecentlyTraded(sourceTickerInfo, nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        ClientOnlyReceivedCache = new PQRecentlyTraded(sourceTickerInfo, nameIdLookupGenerator, IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
    }

    public PQRecentlyTradedHistory
    (IPQOnTickLastTraded? onTickLastTraded = null, IPQRecentlyTraded? allLimitedHistoryLastTrades = null
      , IPQRecentlyTraded? clientOnlyReceivedCache = null
      , IPQRecentlyTraded? openPositionTrades = null, IPQRecentlyTraded? recentInternalOrdersTrades = null
      , IPQRecentlyTraded? alertTrades = null)
    {
        nameIdLookupGenerator         = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        OnTickLastTraded              = onTickLastTraded ?? new PQOnTickLastTraded(nameIdLookupGenerator);
        OnTickLastTraded.NameIdLookup = nameIdLookupGenerator;

        AllLimitedHistoryLastTrades = allLimitedHistoryLastTrades ??
                                      new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAllLimitedHistoryLastTradedTransmissionFlags
                                                       , IRecentlyTradedHistory.DefaultLimitedHistoryMaxTradeCount);
        AllLimitedHistoryLastTrades.NameIdLookup = nameIdLookupGenerator;

        RecentInternalOrdersTrades = recentInternalOrdersTrades ??
                                     new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultRecentInternalOrdersTradesTransmissionFlags);
        RecentInternalOrdersTrades.NameIdLookup = nameIdLookupGenerator;

        OpenPositionTrades = openPositionTrades ?? new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultOpenPositionTradesTransmissionFlags);
        OpenPositionTrades.NameIdLookup = nameIdLookupGenerator;

        AlertTrades        = alertTrades ?? new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags);
        AlertTrades.NameIdLookup = nameIdLookupGenerator;

        ClientOnlyReceivedCache
            = clientOnlyReceivedCache ?? new PQRecentlyTraded(nameIdLookupGenerator, IRecentlyTradedHistory.DefaultClientOnlyReceivedCacheTransmissionFlags);
        ClientOnlyReceivedCache.NameIdLookup = nameIdLookupGenerator;
    }

    public PQRecentlyTradedHistory(IRecentlyTradedHistory toClone)
    {
        if (toClone is PQRecentlyTradedHistory recentlyTradedHistory)
        {
            nameIdLookupGenerator = recentlyTradedHistory.NameIdLookup.Clone();

            OnTickLastTraded            = recentlyTradedHistory.OnTickLastTraded.Clone();
            OnTickLastTraded.NameIdLookup = nameIdLookupGenerator;

            AllLimitedHistoryLastTrades              = recentlyTradedHistory.AllLimitedHistoryLastTrades.Clone();
            AllLimitedHistoryLastTrades.NameIdLookup = nameIdLookupGenerator;

            RecentInternalOrdersTrades              = recentlyTradedHistory.RecentInternalOrdersTrades.Clone();
            RecentInternalOrdersTrades.NameIdLookup = nameIdLookupGenerator;

            OpenPositionTrades              = recentlyTradedHistory.OpenPositionTrades.Clone();
            OpenPositionTrades.NameIdLookup = nameIdLookupGenerator;

            AlertTrades              = recentlyTradedHistory.AlertTrades.Clone();
            AlertTrades.NameIdLookup = nameIdLookupGenerator;

            ClientOnlyReceivedCache              = recentlyTradedHistory.ClientOnlyReceivedCache.Clone();
            ClientOnlyReceivedCache.NameIdLookup = nameIdLookupGenerator;
        }
        else
        {
            nameIdLookupGenerator         = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

            OnTickLastTraded            = new PQOnTickLastTraded(toClone.OnTickLastTraded, nameIdLookupGenerator);
            AllLimitedHistoryLastTrades = new PQRecentlyTraded(toClone.AllLimitedHistoryLastTrades, nameIdLookupGenerator);
            RecentInternalOrdersTrades  = new PQRecentlyTraded(toClone.RecentInternalOrdersTrades, nameIdLookupGenerator);
            OpenPositionTrades          = new PQRecentlyTraded(toClone.OpenPositionTrades, nameIdLookupGenerator);
            AlertTrades                 = new PQRecentlyTraded(toClone.AlertTrades, nameIdLookupGenerator);
            ClientOnlyReceivedCache     = new PQRecentlyTraded(toClone.ClientOnlyReceivedCache, nameIdLookupGenerator);
        }
    }

    IOnTickLastTraded IRecentlyTradedHistory.OnTickLastTraded => OnTickLastTraded;

    IMutableOnTickLastTraded IMutableRecentlyTradedHistory.OnTickLastTraded
    {
        get => OnTickLastTraded;
        set => OnTickLastTraded = (IPQOnTickLastTraded)value;
    }

    public IPQOnTickLastTraded OnTickLastTraded { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.AllLimitedHistoryLastTrades => AllLimitedHistoryLastTrades;

    IMutableRecentlyTraded IMutableRecentlyTradedHistory.AllLimitedHistoryLastTrades
    {
        get => AllLimitedHistoryLastTrades;
        set => AllLimitedHistoryLastTrades = (IPQRecentlyTraded)value;
    }

    public IPQRecentlyTraded AllLimitedHistoryLastTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.RecentInternalOrdersTrades => RecentInternalOrdersTrades;

    IMutableRecentlyTraded IMutableRecentlyTradedHistory.RecentInternalOrdersTrades
    {
        get => RecentInternalOrdersTrades;
        set => RecentInternalOrdersTrades = (IPQRecentlyTraded)value;
    }

    public IPQRecentlyTraded RecentInternalOrdersTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.OpenPositionTrades => OpenPositionTrades;

    IMutableRecentlyTraded IMutableRecentlyTradedHistory.OpenPositionTrades
    {
        get => OpenPositionTrades;
        set => OpenPositionTrades = (IPQRecentlyTraded)value;
    }

    public IPQRecentlyTraded OpenPositionTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.AlertTrades => AlertTrades;

    IMutableRecentlyTraded IMutableRecentlyTradedHistory.AlertTrades
    {
        get => AlertTrades;
        set => AlertTrades = (IPQRecentlyTraded)value;
    }

    public IPQRecentlyTraded AlertTrades { get; set; }

    IRecentlyTraded IRecentlyTradedHistory.ClientOnlyReceivedCache => ClientOnlyReceivedCache;

    IMutableRecentlyTraded IMutableRecentlyTradedHistory.ClientOnlyReceivedCache
    {
        get => ClientOnlyReceivedCache;
        set => ClientOnlyReceivedCache = (IPQRecentlyTraded)value;
    }

    public IPQRecentlyTraded ClientOnlyReceivedCache { get; set; }

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            if (ReferenceEquals(value, nameIdLookupGenerator)) return;
            nameIdLookupGenerator  = value;

            OnTickLastTraded.NameIdLookup            = value;
            AllLimitedHistoryLastTrades.NameIdLookup = value;
            RecentInternalOrdersTrades.NameIdLookup  = value;
            OpenPositionTrades.NameIdLookup          = value;
            AlertTrades.NameIdLookup                 = value;
            ClientOnlyReceivedCache.NameIdLookup     = value;
        }
    }

    public bool HasUpdates
    {
        get =>
            OnTickLastTraded.HasUpdates
         || AllLimitedHistoryLastTrades.HasUpdates
         || RecentInternalOrdersTrades.HasUpdates
         || OpenPositionTrades.HasUpdates
         || AlertTrades.HasUpdates
         || ClientOnlyReceivedCache.HasUpdates;
        set
        {
            OnTickLastTraded.HasUpdates            = value;
            AllLimitedHistoryLastTrades.HasUpdates = value;
            RecentInternalOrdersTrades.HasUpdates  = value;
            OpenPositionTrades.HasUpdates          = value;
            AlertTrades.HasUpdates                 = value;
            ClientOnlyReceivedCache.HasUpdates     = value;
        }
    }

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
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableRecentlyTradedHistory ITrackableReset<IMutableRecentlyTradedHistory>.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTradedHistory ITrackableReset<IPQRecentlyTradedHistory>.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTradedHistory IPQRecentlyTradedHistory.ResetWithTracking() => ResetWithTracking();

    public virtual PQRecentlyTradedHistory ResetWithTracking()
    {
        OnTickLastTraded.ResetWithTracking();
        AllLimitedHistoryLastTrades.ResetWithTracking();
        RecentInternalOrdersTrades.ResetWithTracking();
        OpenPositionTrades.ResetWithTracking();
        AlertTrades.ResetWithTracking();
        ClientOnlyReceivedCache.ResetWithTracking();

        SequenceId = 0;
        return this;
    }

    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        OnTickLastTraded.UpdateComplete(updateSequenceId);
        AllLimitedHistoryLastTrades.UpdateComplete(updateSequenceId);
        RecentInternalOrdersTrades.UpdateComplete(updateSequenceId);
        OpenPositionTrades.UpdateComplete(updateSequenceId);
        AlertTrades.UpdateComplete(updateSequenceId);
        ClientOnlyReceivedCache.UpdateComplete(updateSequenceId);
    }

    public void ReceiveEventLifeCycleChange(IMarketTradingStateEvent updatedItem, EventStateLifecycle eventType)
    {
        if (eventType.IsActive() && updatedItem.MarketTradingStatus.HasMarketOpenFlag())
        {
            AllLimitedHistoryLastTrades.ReceiveEventLifeCycleChange(updatedItem, eventType);
            RecentInternalOrdersTrades.ReceiveEventLifeCycleChange(updatedItem, eventType);
            OpenPositionTrades.ReceiveEventLifeCycleChange(updatedItem, eventType);
            AlertTrades.ReceiveEventLifeCycleChange(updatedItem, eventType);
            ClientOnlyReceivedCache.ReceiveEventLifeCycleChange(updatedItem, eventType);
        }
    }

    public void SubscribeToUpdates(IFiresOnTickLifeCycleChanges<IMarketTradingStateEvent> eventSource)
    {
        eventSource.NewlyActiveOnTick += ReceiveEventLifeCycleChange;
    }


    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var onTickLastTraded in OnTickLastTraded.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return onTickLastTraded;
        }
        foreach (var limHistLastTraded in AllLimitedHistoryLastTrades.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return limHistLastTraded.WithFieldId(PQFeedFields.LastTradedAllRecentlyLimitedHistory);
        }
        foreach (var internalOrderLastTraded in RecentInternalOrdersTrades.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return internalOrderLastTraded.WithFieldId(PQFeedFields.LastTradedRecentInternalOrderTrades);
        }
        foreach (var openPosLastTraded in OpenPositionTrades.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return openPosLastTraded.WithFieldId(PQFeedFields.LastTradedInternalOpeningPositionTrades);
        }
        foreach (var alertLastTraded in AlertTrades.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return alertLastTraded.WithFieldId(PQFeedFields.LastTradedAlertTrades);
        }
        // ClientOnlyReceivedCache is not transmitted as it is up to the client to copy and retain desired history
    }

    public int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFeedFields.LastTradedTickTrades :                    OnTickLastTraded.UpdateField(fieldUpdate); break;
            case PQFeedFields.LastTradedAllRecentlyLimitedHistory :     AllLimitedHistoryLastTrades.UpdateField(fieldUpdate); break;
            case PQFeedFields.LastTradedRecentInternalOrderTrades :     RecentInternalOrdersTrades.UpdateField(fieldUpdate); break;
            case PQFeedFields.LastTradedInternalOpeningPositionTrades : OpenPositionTrades.UpdateField(fieldUpdate); break;
            case PQFeedFields.LastTradedAlertTrades :                   AlertTrades.UpdateField(fieldUpdate); break;
        }
        return -1;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.LastTradedStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    IMutableRecentlyTradedHistory ICloneable<IMutableRecentlyTradedHistory>.Clone() => Clone();

    IMutableRecentlyTradedHistory IMutableRecentlyTradedHistory.Clone() => Clone();

    IPQRecentlyTradedHistory IPQRecentlyTradedHistory.Clone() => Clone();

    public override PQRecentlyTradedHistory Clone() =>
        Recycler?.Borrow<PQRecentlyTradedHistory>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQRecentlyTradedHistory(this);

    public override PQRecentlyTradedHistory CopyFrom
        (IRecentlyTradedHistory source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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
}
