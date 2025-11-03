#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;

public interface IDeployDispatchStrategySelector
{
    ISelectionStrategy SelectDeployStrategy(IRule senderRule, IRule deployRule, DeploymentOptions deploymentOptions);

    ISelectionStrategy SelectDispatchStrategy(IRule senderRule, DispatchOptions deploymentOptions
        , string? destinationAddress = null);
}

public class DeployDispatchStrategySelector : UsesRecycler, IDeployDispatchStrategySelector
{
    private readonly SwitchingBroadcastSenderDestinationCache deployDispatchDestinationCache;
    private readonly FixedOrderSelectionStrategy fixedOrderSelectionStrategy;
    private readonly LeastBusySelectionStrategy leastBusySelectionStrategy;
    private readonly ReuseLastCachedResultSelectionStrategy reuseLastCachedResultSelectionStrategy;
    private readonly RotateEvenlySelectionStrategy rotateEvenlySelectionStrategy;
    private readonly TargetSpecificSelectionStrategy targetSpecificSelectionStrategy;


    public DeployDispatchStrategySelector(IRecycler recycler)
    {
        Recycler = recycler;
        deployDispatchDestinationCache = new SwitchingBroadcastSenderDestinationCache();
        fixedOrderSelectionStrategy = new FixedOrderSelectionStrategy(recycler);
        rotateEvenlySelectionStrategy = new RotateEvenlySelectionStrategy(recycler, deployDispatchDestinationCache);
        reuseLastCachedResultSelectionStrategy
            = new ReuseLastCachedResultSelectionStrategy(deployDispatchDestinationCache);
        reuseLastCachedResultSelectionStrategy
            = new ReuseLastCachedResultSelectionStrategy(deployDispatchDestinationCache);
        leastBusySelectionStrategy = new LeastBusySelectionStrategy(recycler);
        targetSpecificSelectionStrategy = new TargetSpecificSelectionStrategy(recycler);
    }

    public ISelectionStrategy SelectDeployStrategy(IRule senderRule, IRule deployRule
        , DeploymentOptions deploymentOptions)
    {
        var selectionAggregator = Recycler!.Borrow<SelectionStrategiesAggregator>();
        var flags = deploymentOptions.RoutingFlags;
        if (flags.IsTargetSpecific())
        {
            selectionAggregator.Add(targetSpecificSelectionStrategy);
            return selectionAggregator;
        }

        if (flags.IsRotateEvenly()) selectionAggregator.Add(rotateEvenlySelectionStrategy);
        if (flags.IsUseLastCacheEntry() && !flags.IsRecalculateCache())
            selectionAggregator.Add(reuseLastCachedResultSelectionStrategy);
        if (flags.IsDestinationCacheLast() || flags.IsSenderCacheLast())
            selectionAggregator.CacheResult = deployDispatchDestinationCache;
        if (flags.IsLeastBusyQueue()) selectionAggregator.Add(leastBusySelectionStrategy);

        selectionAggregator.Add(fixedOrderSelectionStrategy);

        return selectionAggregator;
    }

    public ISelectionStrategy SelectDispatchStrategy(IRule senderRule, DispatchOptions deploymentOptions
        , string? destinationAddress = null)
    {
        var selectionAggregator = Recycler!.Borrow<SelectionStrategiesAggregator>();
        var flags = deploymentOptions.RoutingFlags;
        if (flags.IsTargetSpecific())
        {
            selectionAggregator.Add(targetSpecificSelectionStrategy);
            return selectionAggregator;
        }

        if (flags.IsRotateEvenly() && flags.IsSendToOne()) selectionAggregator.Add(rotateEvenlySelectionStrategy);
        if (flags.IsUseLastCacheEntry() && !flags.IsRecalculateCache())
            selectionAggregator.Add(reuseLastCachedResultSelectionStrategy);
        if (flags.IsDestinationCacheLast() || flags.IsSenderCacheLast())
            selectionAggregator.CacheResult = deployDispatchDestinationCache;
        if (flags.IsLeastBusyQueue() && flags.IsSendToOne()) selectionAggregator.Add(leastBusySelectionStrategy);

        selectionAggregator.Add(fixedOrderSelectionStrategy);

        return selectionAggregator;
    }
}
