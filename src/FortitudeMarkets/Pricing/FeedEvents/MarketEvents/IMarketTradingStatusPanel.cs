using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IMarketTradingStatusPanel : IReusableObject<IMarketTradingStatusPanel>, IInterfacesComparable<IMarketTradingStatusPanel>, IShowsEmpty
{
    IMarketTradingStateList UpcomingEvents { get; }

    IMarketTradingStateList CurrentlyActive { get; }

    IMarketTradingStateList RecentEndedEvents { get; }
}

public interface IMutableMarketTradingStatusPanel : IMarketTradingStatusPanel, ITrackableReset<IMutableMarketTradingStatusPanel>
  , IEmptyable, IFiresOnTickLifeCycleChanges<IMarketTradingStateEvent>, IDiscreetUpdatable
{
    new IMutableMarketTradingStateList UpcomingEvents { get; set; }

    new IMutableMarketTradingStateList CurrentlyActive { get; set; }

    new IMutableMarketTradingStateList RecentEndedEvents { get; set; }

    int EnsureIsCurrentlyPending(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyPending);

    int EnsureIsCurrentlyActive(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyActive);

    int EnsureIsCurrentlyEnded(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyActive, DateTime? earlyTerminationTime = null);

    void CheckStartTimeAndExpiryTransitionThroughLifecycle(DateTime asAtTime);

    int RemovedEndedOlderThan(DateTime asAtTime, TimeSpan timeSpanInEndedState);
}